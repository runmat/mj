using System.Collections.Generic;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SapORM.Contracts;

namespace SapORM.Services
{
    public class DynSapProxySqlDbCache : IDynSapProxyCache
    {
        #region Properties

        public ISapConnection SapConnection { get; set; }

        public IDynSapProxyFactory DynSapProxyFactory { get; set; }

        public string BapiName { get; set; }

        private static string _sourceModuleString = "";
        public static string SourceModuleString
        {
            get
            {
                if (String.IsNullOrEmpty(_sourceModuleString))
                {
                    System.Reflection.Assembly executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                    System.Diagnostics.FileVersionInfo myFileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(executingAssembly.Location);
                    _sourceModuleString = executingAssembly.GetName().Name + "," + myFileVersion.FileVersion
                        + ",.NET " + executingAssembly.ImageRuntimeVersion + " (called by .NET " + Environment.Version.ToString() + ")";
                }

                return _sourceModuleString;
            }
        }

        protected bool TestSap { get { return (SapConnection == null || (!SapConnection.ProdSAP)); } }

        #endregion


        #region Methods

        public DynSapProxySqlDbCache(string bapiName, ISapConnection sapConnection, IDynSapProxyFactory dynSapProxyFactory)
        {
            BapiName = bapiName;
            DynSapProxyFactory = dynSapProxyFactory;
            SapConnection = sapConnection;
        }

        public IDynSapProxyObject GetProxy()
		{
            if (!string.IsNullOrEmpty(BapiName.Trim(' ')) && (SapConnection != null))
            {
                if (!CheckBapiInDB(BapiName.ToUpper(), SapConnection))
                {
                    var tmpProxyObj = GenerateNewProxy(BapiName.ToUpper());
					if ((tmpProxyObj != null)) {
                        WriteStrukturIntoSQLDB(tmpProxyObj.Import, tmpProxyObj.Export, tmpProxyObj.BapiDate, tmpProxyObj.BapiName);
					}
				}
                var tmpObj = GetBapiStrukturFromSQLDB(BapiName.ToUpper());
                tmpObj.SetSapConnection(SapConnection);
				return tmpObj;
			}
		    throw new Exception("Fehlende Parameter zum Abfragen eines ProxyObj");
		}

		protected bool RemoveBapiFromDB(string bapiName, string srcModule)
		{
			var cn = new System.Data.SqlClient.SqlConnection(SapConnection.SqlServerConnectionString);
			var cmd = new System.Data.SqlClient.SqlCommand();
			cmd.Connection = cn;
			cn.Open();
			try {
			    int tmpInt;

                string sqlQuery = "Delete FROM BapiStruktur WHERE bapiName=@bapiName AND ISNULL(TestSap,0)=@TestSap AND SourceModule=@SourceModule;";
                cmd.Parameters.AddWithValue("@bapiName", bapiName);
                cmd.Parameters.AddWithValue("@TestSap", (TestSap ? 1 : 0));
                cmd.Parameters.AddWithValue("@SourceModule", srcModule);

                cmd.CommandText = sqlQuery;
                tmpInt = cmd.ExecuteNonQuery();

                return (tmpInt == 1);

			} catch (Exception ex) {
				throw new Exception("Fehler beim löschen von: BAPINAME= " + bapiName + " \\ " + ex.Message + " \\ " + ex.StackTrace);
			} finally {
				cn.Close();
			}
		}

        private static bool CheckBapiInDB(string bapiName, ISapConnection sapConnection)
		{
			var cn = new System.Data.SqlClient.SqlConnection(sapConnection.SqlServerConnectionString);
			var cmd = new System.Data.SqlClient.SqlCommand();
		    try {
				cmd.Connection = cn;
				cn.Open();
			    string sqlQuery;

                sqlQuery = "Select * FROM BapiStruktur WHERE bapiName=@bapiName AND ISNULL(TestSap,0)=@TestSap AND SourceModule=@SourceModule;";
				cmd.Parameters.AddWithValue("@bapiName", bapiName);
                cmd.Parameters.AddWithValue("@TestSap", (!sapConnection.ProdSAP ? 1 : 0));
                cmd.Parameters.AddWithValue("@SourceModule", SourceModuleString);

				cmd.CommandText = sqlQuery;
				var obj = cmd.ExecuteScalar();

                return (obj != null);

			} catch (Exception ex) {
				throw new Exception("FEHLER BEI PROXY EXISTENZABFRAGE: BAPINAME= " + bapiName + " \\ " + ex.Message + " \\ " + ex.StackTrace);
			} finally {
				cn.Close();
			}
		}

        private IDynSapProxyObject GenerateNewProxy(string bapiName)
		{
			var exportStruktur = new DataTable();
			var importStruktur = new DataTable();
			var impTable = new DataTable();
			var expTable = new DataTable();

            var tmpProxyObject = DynSapProxyFactory.CreateProxyObject(bapiName, DateTime.Now, importStruktur, exportStruktur);
            var bapiSapDate = tmpProxyObject.CallBapiForBapi(ref expTable, ref impTable, bapiName, SapConnection);

            try 
            {
				//importStruktur Definieren 
				importStruktur.Columns.Add(new DataColumn("Element", typeof(DataTable)));
				importStruktur.Columns.Add(new DataColumn("ElementCode", typeof(string)));
				importStruktur.AcceptChanges();

				//exportStruktur Definieren 
				exportStruktur.Columns.Add(new DataColumn("Element", typeof(DataTable)));
				exportStruktur.Columns.Add(new DataColumn("ElementCode", typeof(string)));
				exportStruktur.AcceptChanges();

				//import
				var parameterTabelle = new DataTable("ParameterTabelle");
				parameterTabelle.Columns.Add(new DataColumn("PARAMETER", typeof(string)));
				parameterTabelle.Columns.Add(new DataColumn("ParameterDATATYPE", typeof(string)));
				parameterTabelle.Columns.Add(new DataColumn("ParameterValue"));
				parameterTabelle.Columns.Add(new DataColumn("ParameterLength", typeof(Int32)));
				parameterTabelle.AcceptChanges();

			    var sapTabellen = new DataTable[1];
			    var sapConformImportTabellen = new DataTable[1];

				foreach (DataRow tmprowX in impTable.Rows) {
					if ((!ReferenceEquals(tmprowX["STRUCTURE"], DBNull.Value)) && !string.IsNullOrEmpty(tmprowX["STRUCTURE"].ToString().Trim(' '))) {
						var found = false;
						var sapConformCount = 0;
					    DataTable sapConformImportTable;
					    foreach (var tmpDatatable in sapTabellen) {
							sapConformCount += 1;
							if ((tmpDatatable != null)) {
								if (tmpDatatable.TableName == tmprowX["STRUCTURE"].ToString()) {
									found = true;

									sapConformImportTable = sapConformImportTabellen[sapConformCount - 1];

									switch (tmprowX["DATATYPE"].ToString())
									{
									    case "STRING":
									        tmpDatatable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof(string)));
									        tmpDatatable.Columns[tmpDatatable.Columns.Count - 1].MaxLength = Convert.ToInt32(tmprowX["LENGTH"]);
									        break;
									    case "DATE":
									        tmpDatatable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof(DateTime)));
									        break;
									}

									sapConformImportTable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof(string)));
									sapConformImportTable.Columns[sapConformImportTable.Columns.Count - 1].MaxLength = Convert.ToInt32(tmprowX["LENGTH"]);
									tmpDatatable.AcceptChanges();
									sapConformImportTable.AcceptChanges();
									break; 
								}
							}
						}
						if (found == false) {
							var sapTabelle = new DataTable(tmprowX["STRUCTURE"].ToString());
							sapTabellen[sapTabellen.Length - 1] = sapTabelle;
							Array.Resize(ref sapTabellen, sapTabellen.Length + 1);
							sapConformImportTable = new DataTable(tmprowX["STRUCTURE"].ToString());
							sapConformImportTabellen[sapConformImportTabellen.Length - 1] = sapConformImportTable;
							Array.Resize(ref sapConformImportTabellen, sapConformImportTabellen.Length + 1);

							if (tmprowX["DATATYPE"].ToString() == "STRING") {
								sapTabelle.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof(string)));
								sapTabelle.Columns[sapTabelle.Columns.Count - 1].MaxLength = Convert.ToInt32(tmprowX["LENGTH"]);
							} else if (tmprowX["DATATYPE"].ToString() == "DATE") {
								sapTabelle.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof(DateTime)));
							}

							sapConformImportTable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof(string)));
							sapConformImportTable.Columns[sapConformImportTable.Columns.Count - 1].MaxLength = Convert.ToInt32(tmprowX["LENGTH"]);
							sapTabelle.AcceptChanges();
							sapConformImportTable.AcceptChanges();
						}
					} else {
						var rowX = parameterTabelle.NewRow();
						rowX["PARAMETER"] = tmprowX["PARAMETER"].ToString();
						rowX["ParameterDATATYPE"] = tmprowX["DATATYPE"].ToString();
						rowX["ParameterValue"] = null;
						rowX["ParameterLength"] = Convert.ToInt32(tmprowX["LENGTH"]);
						parameterTabelle.Rows.Add(rowX);
						parameterTabelle.AcceptChanges();
					}
				}

			    var strukturRow = importStruktur.NewRow();
				strukturRow["Element"] = parameterTabelle;
				strukturRow["ElementCode"] = "PARA";
				importStruktur.Rows.Add(strukturRow);
				importStruktur.AcceptChanges();
			    foreach (var dataTable in sapTabellen) {
					if ((dataTable != null)) {
						strukturRow = importStruktur.NewRow();
						strukturRow["Element"] = dataTable;
						strukturRow["ElementCode"] = "TABLE";
						importStruktur.Rows.Add(strukturRow);
						importStruktur.AcceptChanges();
					}
				}
				foreach (var dataTable in sapConformImportTabellen) {
					if ((dataTable != null)) {
						strukturRow = importStruktur.NewRow();
						strukturRow["Element"] = dataTable;
						strukturRow["ElementCode"] = "SAPTABLE";
						importStruktur.Rows.Add(strukturRow);
						importStruktur.AcceptChanges();
					}
				}

				//export
				var parameterTabelle2 = new DataTable("parameterTabelle");

				parameterTabelle2.Columns.Add(new DataColumn("PARAMETER", typeof(string)));
				parameterTabelle2.Columns.Add(new DataColumn("ParameterDATATYPE", typeof(string)));
				parameterTabelle2.Columns.Add(new DataColumn("ParameterValue", typeof(object)));
				parameterTabelle2.Columns.Add(new DataColumn("ParameterLength", typeof(Int32)));
				parameterTabelle2.AcceptChanges();

				var sapTabellen2 = new DataTable[1];
			    foreach (DataRow tmprowX in expTable.Rows) {
					//Trace.WriteLine(String.Join(", ", tmprowX.ItemArray))
					if ((!ReferenceEquals(tmprowX["STRUCTURE"], DBNull.Value)) && !string.IsNullOrEmpty(tmprowX["STRUCTURE"].ToString().Trim(' '))) {
						var found2 = false;
						foreach (var tmpDatatable in sapTabellen2) {
							if ((tmpDatatable != null)) {
								if (tmpDatatable.TableName == tmprowX["STRUCTURE"].ToString()) {
									found2 = true;

									switch (tmprowX["DATATYPE"].ToString())
									{
									    case "STRING":
									        tmpDatatable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof(string)));
									        tmpDatatable.Columns[tmpDatatable.Columns.Count - 1].MaxLength = Convert.ToInt32(tmprowX["LENGTH"]);
									        break;
									    case "DATE":
									        tmpDatatable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof(DateTime)));
									        break;
									}

									tmpDatatable.AcceptChanges();
									break; 
								}
							}
						}
						if (!found2) {
							var sapTabelle = new DataTable(tmprowX["STRUCTURE"].ToString());
							sapTabellen2[sapTabellen2.Length - 1] = sapTabelle;
							Array.Resize(ref sapTabellen2, sapTabellen2.Length + 1);

							switch (tmprowX["DATATYPE"].ToString())
							{
							    case "STRING":
							        sapTabelle.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof(string)));
							        sapTabelle.Columns[sapTabelle.Columns.Count - 1].MaxLength = Convert.ToInt32(tmprowX["LENGTH"]);
							        break;
							    case "DATE":
							        sapTabelle.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof(DateTime)));
							        break;
							}

							sapTabelle.AcceptChanges();
						}
					} else {
						var rowX = parameterTabelle2.NewRow();
						rowX["PARAMETER"] = tmprowX["PARAMETER"].ToString();
						rowX["ParameterDATATYPE"] = tmprowX["DATATYPE"].ToString();
						rowX["ParameterValue"] = null;
						rowX["ParameterLength"] = Convert.ToInt32(tmprowX["LENGTH"]);
						parameterTabelle2.Rows.Add(rowX);
						parameterTabelle2.AcceptChanges();
					}
				}

				strukturRow = exportStruktur.NewRow();
				strukturRow["Element"] = parameterTabelle2;
				strukturRow["ElementCode"] = "PARA";
				exportStruktur.Rows.Add(strukturRow);
				exportStruktur.AcceptChanges();
			    foreach (var dataTable in sapTabellen2) {
					if ((dataTable != null)) {
						strukturRow = exportStruktur.NewRow();
						strukturRow["Element"] = dataTable;
						strukturRow["ElementCode"] = "TABLE";
						exportStruktur.Rows.Add(strukturRow);
						exportStruktur.AcceptChanges();
					}
				}

                return DynSapProxyFactory.CreateProxyObject(bapiName, bapiSapDate.GetValueOrDefault(), importStruktur, exportStruktur);
			} catch (Exception ex) {
				throw new Exception("Fehler bei der Erstellung des Proxys für BAPI: " + bapiName + " / " + ex.Message);
			}
		}

		private IDynSapProxyObject GetBapiStrukturFromSQLDB(string bapiName)
		{
			var cn = new System.Data.SqlClient.SqlConnection(SapConnection.SqlServerConnectionString);
			var myFormatter = new BinaryFormatter();
			try {
				cn.Open();
                var cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM BapiStruktur WHERE bapiName=@bapiName AND ISNULL(TestSap,0)=@TestSap AND SourceModule=@SourceModule;", cn);
                cmd.Parameters.AddWithValue("@bapiName", bapiName);
                cmd.Parameters.AddWithValue("@TestSap", (TestSap ? 1 : 0));
                cmd.Parameters.AddWithValue("@SourceModule", SourceModuleString);
				var myDataReader = cmd.ExecuteReader();

				if (myDataReader.Read())
				{
					return DynSapProxyFactory.CreateProxyObject(bapiName,
                        Convert.ToDateTime(myDataReader.GetValue(myDataReader.GetOrdinal("BapiDate"))),
                        (DataTable)myFormatter.Deserialize(new MemoryStream((byte[])myDataReader.GetValue(myDataReader.GetOrdinal("ImportStruktur")))),
                        (DataTable)myFormatter.Deserialize(new MemoryStream((byte[])myDataReader.GetValue(myDataReader.GetOrdinal("ExportStruktur")))));
				} else {
					return null;
				}
			} catch (Exception) {
				return null;
			} finally {
				cn.Close();
			}
		}

        private void WriteStrukturIntoSQLDB(DataTable importStruktur, DataTable exportStruktur, DateTime bapiDate, string bapiname)
		{
			var msI = new MemoryStream();
			var msE = new MemoryStream();
			var myFormatterImportS = new BinaryFormatter();
			var myFormatterExportS = new BinaryFormatter();
		    var cn = new System.Data.SqlClient.SqlConnection(SapConnection.SqlServerConnectionString);
			var cmd = new System.Data.SqlClient.SqlCommand();

			myFormatterImportS.Serialize(msI, importStruktur);
			myFormatterExportS.Serialize(msE, exportStruktur);
			msI.Close();
			msE.Close();
			byte[] serializedE = msE.ToArray();
			byte[] serializedI = msI.ToArray();

		    try {
				cn.Open();
				cmd.Connection = cn;
				cmd.CommandType = CommandType.Text;
		        string sqlQuery;
		        if (!CheckBapiInDB(bapiname, SapConnection)) {
					sqlQuery = "INSERT INTO BapiStruktur (bapiName, BapiDate, TestSap, SourceModule, ImportStruktur, ExportStruktur) VALUES (@bapiName, @BapiDate, @TestSap, @SourceModule, @ImportStruktur, @ExportStruktur);";
					var with6 = cmd;
					with6.Parameters.AddWithValue("@bapiName", bapiname);
					with6.Parameters.AddWithValue("@BapiDate", bapiDate);
                    with6.Parameters.AddWithValue("@TestSap", (TestSap ? 1 : 0));
                    with6.Parameters.AddWithValue("@SourceModule", SourceModuleString);
					with6.Parameters.AddWithValue("@ImportStruktur", serializedI);
					with6.Parameters.AddWithValue("@ExportStruktur", serializedE);
				} else {
                    sqlQuery = "UPDATE BapiStruktur " + "SET ImportStruktur=@ImportStruktur," + "ExportStruktur=@ExportStruktur," + "BapiDate=@BapiDate, " + "updated=@updated " + "WHERE bapiName=@bapiName AND ISNULL(TestSap,0)=@TestSap AND SourceModule=@SourceModule;";
					var with7 = cmd;
					with7.Parameters.AddWithValue("@ImportStruktur", serializedI);
					with7.Parameters.AddWithValue("@ExportStruktur", serializedE);
					with7.Parameters.AddWithValue("@BapiDate", bapiDate);
                    with7.Parameters.AddWithValue("@updated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@bapiName", bapiname);
                    cmd.Parameters.AddWithValue("@TestSap", (TestSap ? 1 : 0));
                    cmd.Parameters.AddWithValue("@SourceModule", SourceModuleString);
				}
				cmd.CommandText = sqlQuery;
				cmd.ExecuteNonQuery();
			} catch (Exception ex) {
				throw new Exception("FEHLER BEI DEM SCHREIBEN/UPDATE EINES PROXY OBJ IN DIE SQL DB: BAPINAME= " + bapiname + " \\ " + ex.Message + " \\ " + ex.StackTrace);
			} finally {
				cn.Close();
			}
		}

		#endregion


        #region Unused

        #endregion
    }
}
