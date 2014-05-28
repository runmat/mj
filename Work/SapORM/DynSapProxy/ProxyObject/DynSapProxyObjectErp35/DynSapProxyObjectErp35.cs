using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using GeneralTools.Contracts;
using GeneralTools.Services;
using Microsoft.VisualBasic;
using SapORM.Contracts;

using ERPConnect;


namespace SapORM.Services
{
    [Serializable]
    public class DynSapProxyObjectErp35 : IDynSapProxyObject
    {
		#region Properties


	    private bool _clearMe;

		private ISapConnection _sapConnection;

	    public string BapiName { get; set; }

	    public DateTime BapiDate { get; set; }

	    public DateTime BapiLoaded { get; set; }

	    public DataTable Import { get; set; }

	    public DataTable Export { get; set; }


	    #endregion

		#region Methods


        public DynSapProxyObjectErp35(string bapiName, DateTime sapDatum, DataTable impStruktur, DataTable expStruktur)
        {
            this.BapiName = bapiName;
            BapiDate = sapDatum;
            BapiLoaded = DateTime.Now;
            Import = GenerateANewDataTableCopy(impStruktur);
            Export = GenerateANewDataTableCopy(expStruktur);
        }

        private R3Connection CreateErpConnection(ISapConnection sapConnection = null)
        {
            if (sapConnection == null)
                throw new Exception("SAP Connection not initialized!");

            _sapConnection = sapConnection;

            if (string.IsNullOrEmpty(_sapConnection.ErpConnectLicense))
                throw new Exception("SAP 'ERPConnectLicense' is empty!");

            var conn = new R3Connection(_sapConnection.SAPAppServerHost, _sapConnection.SAPSystemNumber, _sapConnection.SAPUsername, _sapConnection.SAPPassword, "DE", Convert.ToInt16(_sapConnection.SAPClient).ToString());
            
            LIC.SetLic(_sapConnection.ErpConnectLicense);
            
            return conn;
        }

        public void SetSapConnection(ISapConnection sapConnection)
		{
            _sapConnection = sapConnection;
		}

		private static DataTable GenerateANewDataTableCopy(DataTable originalDataTable)
		{
		    var newDataTableCopy = originalDataTable.Clone();
			foreach (DataRow tmpRow in originalDataTable.Rows) {
				var newRow = newDataTableCopy.NewRow();
				for (var i = 0; i <= tmpRow.ItemArray.Length - 1; i += 1) {
					if ((tmpRow[i]) is DataTable) {
						newRow[i] = GenerateANewDataTableCopy((DataTable)tmpRow[i]);
					} else {
						newRow[i] = tmpRow[i];
					}
				}
				newDataTableCopy.Rows.Add(newRow);
				newDataTableCopy.AcceptChanges();
			}
			return newDataTableCopy;
		}



        public bool CallBapi(ILogService logService = null, ILogonContext logonContext = null)
		{
		    var itemsCollect = new RFCTableCollection();

            var con = CreateErpConnection(_sapConnection);

            Stopwatch stopwatch = null;

            try {
				if (_clearMe) {
					ClearAllValues();
				}

				con.Open(false);

				var func = con.CreateFunction(BapiName);

			    RFCStructure item;

                #region Aufbereitung der Import Paramter, Werttypen und Tabellen

                try {
					foreach (DataRow impRow in Import.Rows) {
						if (impRow[1].ToString() == "PARA") {
							var paraTabelle = (DataTable)impRow[0];
							foreach (DataRow paraRow in paraTabelle.Rows) {

							if (paraRow[1].ToString() != "DATE") {
                                // Wenn kein DBNull-Wert dann Inhalt 1:1 kopieren
								if (!Information.IsDBNull(paraRow[2])) {
									func.Exports[paraRow[0].ToString()].ParamValue = paraRow[2];                                    
								} 
                                else if (func.Exports[paraRow[0].ToString()].Type == RFCTYPE.CHAR) // Wenn Feldtyp Character einen Leerwert definieren 
                                { 
									if (func.Exports[paraRow[0].ToString()].ParamValue.ToString().Length > 0) {
										func.Exports[paraRow[0].ToString()].ParamValue = "";
									}

                                }
                                else if (func.Exports[paraRow[0].ToString()].Type == RFCTYPE.BCD)// Wenn Feldtyp Character einen Leerwert definieren
                                {  
                                    if (func.Exports[paraRow[0].ToString()].ParamValue.ToString().Length > 0)
                                    {
                                        func.Exports[paraRow[0].ToString()].ParamValue = 0d;
                                    }
                                }
							} else {
								if (!Information.IsDBNull(paraRow[2])) {
									if (paraRow[2].ToString() != "00000000" & !string.IsNullOrEmpty(paraRow[2].ToString())) {
										func.Exports[paraRow[0].ToString()].ParamValue = ConversionUtils.NetDate2SAPDate(Convert.ToDateTime(paraRow[2]));
									}
								}
							}
						}
					}


					if (impRow[1].ToString() == "TABLE") {
						var items = new RFCTable();

						var booFound = false;

						//Schauen, ob die Importtabelle Bestandteil der SAP-Tabellen ist.

						foreach (RFCTable rfcT in func.Tables) {
							if (rfcT.Name == ((DataTable)impRow[0]).TableName) {
								booFound = true;
								break; 
							}

						}

						DataTable tempTable;
						if (booFound) {
							items = func.Tables[((DataTable)impRow[0]).TableName];
							//new RFCStructure(items.Columns);
						//wenn nein dann schauen ob die Tabelle eine Importstruktur ist
						} else {
							foreach (RFCParameter sapStructure in func.Exports) {

								if (sapStructure.IsStructure()) {
									if (sapStructure.Name == ((DataTable)impRow[0]).TableName) {
										item = func.Exports[((DataTable)impRow[0]).TableName].ToStructure();

										tempTable = (DataTable)impRow[0];
										foreach (DataRow dr in tempTable.Rows) {
											foreach (RFCTableColumn col in item.Columns) {
												switch (col.Type) {
													case RFCTYPE.NUM:
														if ((!ReferenceEquals(dr[col.Name], DBNull.Value))) {
															if (Information.IsNumeric(dr[col.Name].ToString())) {
																item[col.Name] = dr[col.Name].ToString();
															}

														}
														break;
													case RFCTYPE.CHAR:
														item[col.Name] = dr[col.Name].ToString();
														break;
													case RFCTYPE.BCD:
														if ((!ReferenceEquals(dr[col.Name], DBNull.Value))) {
															if (Information.IsNumeric(dr[col.Name].ToString())) {
																item[col.Name] = dr[col.Name].ToString();
															}
														}
														break;
													case RFCTYPE.DATE:
														if (dr[col.Name].ToString() == "00000000" || string.IsNullOrEmpty(dr[col.Name].ToString())) {
															item[col.Name] = "";
														} else {
															item[col.Name] = ConversionUtils.NetDate2SAPDate(Convert.ToDateTime(dr[col.Name].ToString()));
														}
														break;
													case RFCTYPE.TIME:
														if (dr[col.Name].ToString() == "000000" || string.IsNullOrEmpty(dr[col.Name].ToString())) {
															item[col.Name] = "";
														} else {
															item[col.Name] = dr[col.Name].ToString();
														}

														break;
												}
											}
										}
										func.Exports[sapStructure.Name].ParamValue = item;
									}
								}
							}

						}

						if (booFound) {
							tempTable = (DataTable)impRow[0];
							foreach (DataRow dr in tempTable.Rows) {
								item = items.AddRow();

								foreach (RFCTableColumn col in item.Columns) {
									switch (col.Type) {
										case RFCTYPE.NUM:
											if ((!ReferenceEquals(dr[col.Name], DBNull.Value))) {
												if (Information.IsNumeric(dr[col.Name].ToString())) {
													item[col.Name] = dr[col.Name].ToString();
												}
											}
											break;
										case RFCTYPE.CHAR:
											item[col.Name] = dr[col.Name].ToString();
											break;
										case RFCTYPE.BCD:
											if ((!ReferenceEquals(dr[col.Name], DBNull.Value))) {
												if (Information.IsNumeric(dr[col.Name].ToString())) {
													item[col.Name] = Convert.ToDecimal(dr[col.Name].ToString());
												}
											}
											break;
										case RFCTYPE.DATE:
											if (dr[col.Name].ToString() == "00000000" || string.IsNullOrEmpty(dr[col.Name].ToString())) {
												item[col.Name] = "";
											} else {
												item[col.Name] = ConversionUtils.NetDate2SAPDate(Convert.ToDateTime(dr[col.Name].ToString()));
											}
											break;
										case RFCTYPE.TIME:
											if (dr[col.Name].ToString() == "000000" || string.IsNullOrEmpty(dr[col.Name].ToString())) {
												item[col.Name] = "";
											} else {
												item[col.Name] = dr[col.Name].ToString();
											}

											break;
									}
								}
							}

							itemsCollect.Add(items);

						} 
					}
				}
				} 
                catch
                {
                }

                #endregion

                stopwatch = Stopwatch.StartNew();

				func.Execute();

                if (stopwatch != null)
                    stopwatch.Stop();

                #region Aufbereitung der Export Paramter, Werttypen und Tabellen

                //Export-Tabellen
				foreach (var tmpRow in Export.Select("ElementCode='TABLE'")) {
					// Exportparameter Struktur oder Tabelle
					foreach (RFCParameter it in func.Imports) {
						if (it.IsStructure()) {
							if (it.Name == ((DataTable)tmpRow[0]).TableName) {
								item = it.ToStructure();
							    var tblTemp = new DataTable {TableName = it.Name};
							    foreach (RFCTableColumn col in item.Columns) {
									tblTemp.Columns.Add(col.Name, typeof(String));
								}
							    var row = tblTemp.NewRow();

								foreach (RFCTableColumn col in item.Columns) {
									switch (col.Type) {
										case RFCTYPE.NUM:
											row[col.Name] = item[col.Name].ToString();
											break;
										case RFCTYPE.CHAR:
											row[col.Name] = item[col.Name].ToString();
											break;
										case RFCTYPE.BCD:
											if (ReferenceEquals(item[col.Name], DBNull.Value)) {
												row[col.Name] = "";
											}
											break;
										case RFCTYPE.DATE:
											if (item[col.Name].ToString() == "00000000" || string.IsNullOrEmpty(item[col.Name].ToString())) {
												row[col.Name] = "";
											} else {
												row[col.Name] = ConversionUtils.SAPDate2NetDate(item[col.Name].ToString());
											}
											break;
										case RFCTYPE.TIME:
											if (item[col.Name].ToString() == "000000" || string.IsNullOrEmpty(item[col.Name].ToString())) {
												row[col.Name] = "";
											} else {
												row[col.Name] = item[col.Name].ToString();
											}
											break;
										case RFCTYPE.BYTE:
										case RFCTYPE.ITAB:

											break;
									}
								}
								tblTemp.Rows.Add(row);
								tblTemp.AcceptChanges();
								tmpRow[0] = tblTemp;
							} else if (it.IsTable()) {
								var rfcT = it.ToTable();

								if (rfcT.Name == ((DataTable)tmpRow[0]).TableName) {
									var tblTemp = rfcT.ToADOTable();


									foreach (RFCTableColumn col in rfcT.Columns) {
										foreach (DataRow row in tblTemp.Rows) {
											switch (col.Type) {

												case RFCTYPE.NUM:
													row[col.Name] = row[col.Name].ToString();

													break;
												case RFCTYPE.CHAR:
													row[col.Name] = row[col.Name].ToString();
													break;
												case RFCTYPE.BCD:
													if (ReferenceEquals(row[col.Name], DBNull.Value)) {
														row[col.Name] = "";
													}
													break;
												case RFCTYPE.DATE:
													if (row[col.Name].ToString() == "00000000" || string.IsNullOrEmpty(row[col.Name].ToString())) {
														row[col.Name] = "";
													} else {
														row[col.Name] = ConversionUtils.SAPDate2NetDate(row[col.Name].ToString());
													}
													break;
												case RFCTYPE.BYTE:
												case RFCTYPE.ITAB:

													break;
												case RFCTYPE.TIME:
													if (row[col.Name].ToString() == "000000" || string.IsNullOrEmpty(row[col.Name].ToString())) {
														row[col.Name] = "";
													} else {
														row[col.Name] = row[col.Name].ToString();
													}
													break;
											}

										}

									}

									tblTemp.AcceptChanges();

									tmpRow[0] = tblTemp;
								}
							}
						}
					}

					foreach (RFCTable rfcT in func.Tables) {

						if (rfcT.Name == ((DataTable)tmpRow[0]).TableName) {

                            tmpRow[0] = UpdateColumnTypes(rfcT);
						}
					}
				}
							
                // Tabellen die nicht als Export-Parameter gespeichert sind manuell hinzufügen
                // +++++++
                foreach (RFCTable rfcT in func.Tables)
                {
                    var bInList = false;
                    foreach(DataRow expItem in Export.Rows){
                        if (((DataTable)expItem[0]).TableName == rfcT.Name)
                        {
                            bInList = true;
                            break;
                        }
                    }

                    if (bInList) continue;

			
                    DataRow newExpRow = Export.NewRow();

                    newExpRow[0] = UpdateColumnTypes(rfcT);
                    newExpRow[1] = "TABLE";

                    Export.Rows.Add(newExpRow);
                    Export.AcceptChanges();
                
                }
                // +++++++


				foreach (var tmpRow in Export.Select("ElementCode='PARA'")) {
					var paraTabelle = (DataTable)tmpRow[0];
					foreach (DataRow paraRow in paraTabelle.Rows) {
						if (paraRow[1].ToString() != "DATE") {
							foreach (RFCParameter it in func.Imports) {
								if (paraRow["Parameter"].ToString() == it.Name) {
									paraRow["ParameterValue"] = it.ParamValue;
								}
							}
						} else {
							foreach (RFCParameter it in func.Imports) {
								if (paraRow["Parameter"].ToString() == it.Name) {
									paraRow["ParameterValue"] = ConversionUtils.SAPDate2NetDate(it.ParamValue.ToString());
								}
							}
						}
					}
				}

#endregion

                LogSapBapiCall(logService, logonContext, false, stopwatch == null ? -1 : stopwatch.Elapsed.TotalSeconds);

				return true;

			} 
            catch (Exception generalException)
            {

                double callduration = -1;
                if (stopwatch != null && stopwatch.IsRunning)
                {
                    stopwatch.Stop();
                    callduration = stopwatch.Elapsed.TotalSeconds;
                }


                PreserveStackTrace(generalException);

                LogSapBapiCall(logService, logonContext, true, callduration);

                // ERPException NRF, NO_DATA, ERR_NO_DATA, und ERR_DATA hier behandeln und den Aufrufer einen false zurückgeben
                // Alle anderen Exceptions NICHT hier behandeln und weitergeben
                // Anstelle von throw new Exception(generalException.Message); komme ich hier mit einem throw aus
                // - der original Exception ist erhalten und wird von der Fehlerbehandlung abgefangen
                // - der original Stacktrace ist erhalten (PreserveStackTrace ist aufgerufen worden) 
                var ex = generalException as ERPException;
			    if (ex != null && ex.Source.Contains("ERPConnect35")) {
					if (ex.ABAPException.Length > 0) {
                        if (ex.ABAPException.ToUpper() == "NRF")
                            return false;
                        if (ex.ABAPException.ToUpper() == "NO_DATA")
                            return false;
                        if (ex.ABAPException.ToUpper() == "NO_RESULT")
                            return false;
                        if (ex.ABAPException.ToUpper() == "ERR_NO_DATA")
                            return false;
                        if (ex.ABAPException.ToUpper() == "ERR_DAT")
                            return false;

					    
						throw;
					} 
                    else if (ex.Message.Contains("Connect to SAP gateway failed")) {
                        
                        throw;
					} 
                    else
                    {
                        throw;
					}
				} 
                else {
                    throw;
				}
			} 
            finally 
            {
                if ((con != null)) {
					con.Close();
					con.Dispose();
				}
				_clearMe = true;
			}
		}

        /// <summary>
        /// Generiert eine neue DataTable mit angepassten Spaltentypen aus einer RFCTable
        /// </summary>
        /// <param name="rfcT">die zu analysierende Tabelle</param>
        /// <returns>Formatierte DataTable</returns>
        private DataTable UpdateColumnTypes(RFCTable rfcT)
        {
            var tblTemp = rfcT.ToADOTable();
            var tblTempResult = rfcT.ToADOTable().Clone();

            //Datentyp auf DateTime ändern
            foreach (RFCTableColumn col in rfcT.Columns)
            {
                if (col.Type == RFCTYPE.DATE)
                {
                    tblTempResult.Columns[col.Name].DataType = typeof(DateTime);
                    tblTempResult.AcceptChanges();
                }
            }

            //Vorhandene Werte die nicht DateTime sind in die neue Tabelle schreiben.

            foreach (DataRow dr in tblTemp.Rows)
            {
                var newRow = tblTempResult.NewRow();

                foreach (RFCTableColumn col in rfcT.Columns)
                {
                    if (col.Type != RFCTYPE.DATE)
                    {
                        newRow[col.Name] = dr[col.Name];
                    }
                }
                tblTempResult.Rows.Add(newRow);
            }
            tblTempResult.AcceptChanges();



            foreach (RFCTableColumn col in rfcT.Columns)
            {

                for (var i = 0; i <= tblTempResult.Rows.Count - 1; i++)
                {
                    var row = tblTempResult.Rows[i];

                    switch (col.Type)
                    {

                        case RFCTYPE.NUM:
                            row[col.Name] = row[col.Name].ToString();

                            break;
                        case RFCTYPE.CHAR:
                            row[col.Name] = row[col.Name].ToString();
                            break;
                        case RFCTYPE.BCD:
                            row[col.Name] = row[col.Name].ToString();
                            break;
                        case RFCTYPE.DATE:
                            if (rfcT.Rows[i][col.Name].ToString() == "00000000" || string.IsNullOrEmpty(rfcT.Rows[i][col.Name].ToString()))
                            {
                                row[col.Name] = DBNull.Value;
                            }
                            else if (Information.IsDate(ConversionUtils.SAPDate2NetDate(rfcT.Rows[i][col.Name].ToString())))
                            {
                                row[col.Name] = ConversionUtils.SAPDate2NetDate(rfcT.Rows[i][col.Name].ToString());
                            }
                            break;
                        case RFCTYPE.TIME:
                            if (row[col.Name].ToString() == "000000" || string.IsNullOrEmpty(row[col.Name].ToString()))
                            {
                                row[col.Name] = "";
                            }
                            else
                            {
                                row[col.Name] = row[col.Name].ToString();
                            }
                            break;
                        case RFCTYPE.BYTE:
                        case RFCTYPE.ITAB:

                            break;
                    }

                }

            }

            tblTempResult.AcceptChanges();
            return tblTempResult;
        }

        private void LogSapBapiCall(ILogService logService, ILogonContext logonContext, bool exceptionOccurred, double dauer)
        {
            if (logService == null)
                logService = new LogService(string.Empty, string.Empty);                

            var logon = string.Empty;

            if (logonContext != null)
                logon = logonContext.UserID;

            if (exceptionOccurred)
            {
                logService.LogSapCall(BapiName, logon, Import, Export, false, dauer);
                return;
            }

            logService.LogSapCall(BapiName, logon, Import, Export, true, dauer);
        }

		public DataTable GetImportTable(string name)
		{
			try {
                //if (_clearMe) {
                //    ClearAllValues();
                //}
				foreach (var tmpRow in Import.Select("ElementCode='TABLE'")) {
					if (((DataTable)tmpRow[0]).TableName == name) {
						return (DataTable)tmpRow[0];
					}
				}
				throw new Exception();
			} catch (Exception) {
				throw new Exception("Importtabelle mit dem Namen: " + name + " nicht vorhanden!");
			}
		}

		public IEnumerable<DataTable> GetImportTables()
		{
			//----------------------------------------------------------------------
			//Methode:       GetImportTables
			//Autor:         Matthias Jenzen
			//Beschreibung:  liefert alle Importtabellen zurück
			//Erstellt am:   10.12.2012
			//----------------------------------------------------------------------

		    var list = new List<DataTable>();

			foreach (var tmpRow in Import.Select("ElementCode='TABLE'"))
			    if ((!ReferenceEquals(tmpRow[0], DBNull.Value)))
			        list.Add((DataTable) tmpRow[0]);

			return list;
		}

        //private List<DataTable> GetImportTablesToListOrNull()
        //{
        //    var list = GetImportTables().ToList();
        //    return list.Any() ? list : null;
        //}

		public void SetImportParameter(string name, object wert)
		{
			try 
            {
				((DataTable)Import.Select("ElementCode='PARA'")[0][0]).Select("PARAMETER='" + name + "'")[0][2] = wert;
			} 
            catch (Exception) {
				throw new Exception("ImportParameter mit dem Namen: " + name + " und dem Wert: " + wert + " konnte nicht gesetzt werden!");
			}
		}

        public void SetImportTable(string name, DataTable table)
        {
            try
            {
                foreach (var tmpRow in Import.Select("ElementCode='TABLE'"))
                {
                    if (((DataTable)tmpRow[0]).TableName == name)
                    {
                        tmpRow[0] = table;
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Importtabelle mit dem Namen: " + name + " nicht vorhanden!");
            }
        }

		public string GetExportParameter(string name)
		{
			try
			{
			    if ((!ReferenceEquals(((DataTable)Export.Select("ElementCode='PARA'")[0][0]).Select("PARAMETER='" + name + "'")[0][2], DBNull.Value))) {
					return ((DataTable)Export.Select("ElementCode='PARA'")[0][0]).Select("PARAMETER='" + name + "'")[0][2].ToString().Trim();
				}
			    return "";
			}
            catch (Exception)
            {
				throw new Exception("ExportParameter mit dem Namen: " + name + " nicht vorhanden!");
			}
		}

		public byte[] GetExportParameterByte(string name)
		{
		    try {
				if (((!ReferenceEquals(((DataTable)Export.Select("ElementCode='PARA'")[0][0]).Select(("PARAMETER='" + name + "'"))[0][2], DBNull.Value)))) {
					if ((ReferenceEquals(((DataTable)Export.Select("ElementCode='PARA'")[0][0]).Select(("PARAMETER='" + name + "'"))[0][2].GetType(), Type.GetType("System.Byte[]")))) {
						return (byte[])((DataTable)Export.Select("ElementCode='PARA'")[0][0]).Select(("PARAMETER='" + name + "'"))[0][2];
					}
					return null;
				}
            }
            catch (Exception)
            {
				throw new Exception(("ExportParameter mit dem Namen: " + name + " nicht vorhanden!"));
			}
			return null;
		}



		public DataTable GetExportTable(string name)
		{
			try {
				foreach (var tmpRow in Export.Select("ElementCode='TABLE'")) {
					if ((!ReferenceEquals(tmpRow[0], DBNull.Value))) {
						if (((DataTable)tmpRow[0]).TableName == name) {
							return ((DataTable)tmpRow[0]).Copy();
						}
					}
				}
				throw new Exception();
            }
            catch (Exception)
            {
				throw new Exception("Exportabelle mit dem Namen: " + name + " nicht vorhanden!");
			}
		}

		public IEnumerable<DataTable> GetExportTables()
		{
			//----------------------------------------------------------------------
			//Methode:       GetExportTables
			//Autor:         Matthias Jenzen
			//Beschreibung:  liefert alle Exporttabellen zurück
			//Erstellt am:   10.12.2012
			//----------------------------------------------------------------------

		    var list = new List<DataTable>();

			foreach (var tmpRow in Export.Select("ElementCode='TABLE'")) {
				if ((!ReferenceEquals(tmpRow[0], DBNull.Value))) {
					list.Add(((DataTable)tmpRow[0]).Copy());
				}
			}

			return list;
		}

        public void Reset()
        {
            ClearAllValues();
        }

        private void ClearAllValues()
		{
			try {
				foreach (DataRow impRow in Import.Rows) {
					if (impRow[1].ToString() == "PARA") {
						var paraTabelle = (DataTable)impRow[0];
						foreach (DataRow paraRow in paraTabelle.Rows) {
							paraRow[2] = "";
						}
						paraTabelle.AcceptChanges();
					}
					if (impRow[1].ToString() == "TABLE" || impRow[1].ToString() == "SAPTABLE") {
						((DataTable)impRow[0]).Rows.Clear();
						((DataTable)impRow[0]).Clear();
						((DataTable)impRow[0]).AcceptChanges();
					}
				}
				foreach (DataRow expRow in Export.Rows) {
					if (expRow[1].ToString() == "PARA") {
						var paraTabelle = (DataTable)expRow[0];
						foreach (DataRow paraRow in paraTabelle.Rows) {
							paraRow[2] = "";
						}
						paraTabelle.AcceptChanges();
					}

					if (expRow[1].ToString() == "TABLE") {
						((DataTable)expRow[0]).Rows.Clear();
						((DataTable)expRow[0]).Clear();
						((DataTable)expRow[0]).AcceptChanges();
					}
				}
				//mbizTalkExportParameter.Clear()
				//mbizTalkImportParameter.Clear()
				_clearMe = false;
			} catch (Exception ex) {
				throw new Exception("Fehler in DynSapProxyObjectErp35.clearAllValues: " + ex.Message);
			}
		}

        public DateTime? CallBapiForBapi(ref DataTable mExportTabelle, ref DataTable mImportTabelle, string bapiName, ISapConnection sapConnection)
        {
            var con = CreateErpConnection(sapConnection);


            try
            {
                con.Open(false);

                var func = con.CreateFunction("Z_S_GET_BAPI_INTERFACE");

                func.Exports["I_FUNCNAME"].ParamValue = bapiName.Trim(' ').ToUpper();

                func.Execute();

                var pEFuncname = func.Imports["E_FUNCNAME"];
                var pEUdate = func.Imports["E_UDATE"];


                if (ReferenceEquals(pEFuncname.ParamValue, DBNull.Value) || pEFuncname.ParamValue.ToString().Trim(' ') != bapiName.Trim(' ').ToUpper())
                {
                    throw new Exception("angefragtes Bapi:" + bapiName.Trim(' ') + " <> struktur von: " + pEFuncname.ParamValue);
                }

                //auswerten der exportparameter
                if (func.Tables["EXP_TAB"].RowCount > 0)
                {
                    mExportTabelle = func.Tables["EXP_TAB"].ToADOTable();
                    KillAllDBNullValuesInDataTable(ref mExportTabelle);
                    mExportTabelle.AcceptChanges();
                }

                if (func.Tables["IMP_TAB"].RowCount > 0)
                {
                    mImportTabelle = func.Tables["IMP_TAB"].ToADOTable();
                    KillAllDBNullValuesInDataTable(ref mImportTabelle);
                    mImportTabelle.AcceptChanges();
                }

                if (ReferenceEquals(pEUdate.ParamValue, DBNull.Value))
                {
                    throw new Exception("angefragtes Bapi:" + bapiName.Trim(' ') + " enhält kein Änderungsdatum");
                }
                var strTemp = Strings.Right(pEUdate.ParamValue.ToString(), 2) + "." + Strings.Mid(Convert.ToString(pEUdate.ParamValue.ToString()), 5, 2) + "." + Strings.Left(pEUdate.ParamValue.ToString(), 4);

                DateTime date;
                //if (!Information.IsDate(strTemp))
                if (!DateTime.TryParse(strTemp, new CultureInfo("de-DE"), DateTimeStyles.None, out date))
                {
                    throw new Exception("angefragtes Bapi:" + bapiName.Trim(' ') + " enhält kein Änderungsdatum");
                }
                return date;
            }
            catch (Exception ex)
            {
                throw new Exception("FEHLER BEIM AUFRUF DES BAPIS FÜR DIE BAPISTRUKTUR: BAPINAME= " + bapiName + " \\ " + ex.Message + " \\ " + ex.StackTrace);
            }
            finally
            {
                con.Close();
            }
        }

        private static void KillAllDBNullValuesInDataTable(ref DataTable datentabelle)
        {
            foreach (DataRow tmpRow in datentabelle.Rows)
            {
                for (var i = 0; i <= tmpRow.ItemArray.Length - 1; i++)
                {
                    if (ReferenceEquals(tmpRow[i], DBNull.Value))
                    {
                        tmpRow[i] = string.Empty;
                    }
                    if ((tmpRow[i]) is DataTable)
                    {
                        var dataTable = (tmpRow[i] as DataTable);
                        KillAllDBNullValuesInDataTable(ref dataTable);
                    }
                }
            }
            datentabelle.AcceptChanges();
        }

        /// <summary>
        /// In bestimmten Fällen kann das Aufbewahen des Stacktrace fehlschlagen (wenn das Exception in einer anderen Routine ausgelöst wurde als die in der die Catch sich befindet)
        /// </summary>
        /// <param name="exception">Ausnahme deren Stacktrace bewahrt werden soll</param>
        private static void PreserveStackTrace(Exception e)
        {
            var ctx = new StreamingContext(StreamingContextStates.CrossAppDomain);
            var mgr = new ObjectManager(null, ctx);
            var si = new SerializationInfo(e.GetType(), new FormatterConverter());

            e.GetObjectData(si, ctx);
            mgr.RegisterObject(e, 1, si); 
            mgr.DoFixups(); 
        }

        #endregion
    }
}
