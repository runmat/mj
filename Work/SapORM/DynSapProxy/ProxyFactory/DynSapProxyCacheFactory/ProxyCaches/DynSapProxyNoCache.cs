using System;
using System.Collections.Generic;
using System.Linq;
using SapORM.Contracts;
using System.Data;

namespace SapORM.Services
{
    public class DynSapProxyNoCache : IDynSapProxyCache
    {
        #region Properties

        public ISapConnection SapConnection { get; set; }

        public IDynSapProxyFactory DynSapProxyFactory { get; set; }

        public string BapiName { get; set; }

        private readonly List<IDynSapProxyObject> _proxys = new List<IDynSapProxyObject>();

        #endregion


        #region Methods

        public DynSapProxyNoCache(string bapiName, ISapConnection sapConnection,
                                     IDynSapProxyFactory dynSapProxyFactory)
        {
            BapiName = bapiName;
            DynSapProxyFactory = dynSapProxyFactory;
            SapConnection = sapConnection;
        }

        public IDynSapProxyObject GetProxy()
        {
            if (!string.IsNullOrEmpty(BapiName.Trim(' ')) && (SapConnection != null))
            {
                var tmpProxyObj = GetProxyByName(BapiName);

                if (tmpProxyObj == null)
                {
                    tmpProxyObj = GenerateNewProxy(BapiName.ToUpper().ToLower(), SapConnection);
                    if ((tmpProxyObj != null))
                    {
                        //WriteStrukturIntoSQLDB(tmpProxyObj.Import, tmpProxyObj.Export, tmpProxyObj.BapiDate, tmpProxyObj.BapiName, SapConnection);
                        _proxys.Add(tmpProxyObj);
                    }
                }
                
                if (tmpProxyObj != null)
                    tmpProxyObj.SetSapConnection(SapConnection);

                return tmpProxyObj;
            }
            throw new Exception("Fehlende Parameter zum Abfragen eines ProxyObj");
        }

        IDynSapProxyObject GetProxyByName(string bapiName)
        {
            return _proxys.FirstOrDefault(d => d.BapiName.ToUpper() == bapiName.ToUpper());   
        }

        private IDynSapProxyObject GenerateNewProxy(string bapiName, ISapConnection sapConnection)
        {
            var exportStruktur = new DataTable();
            var importStruktur = new DataTable();
            var impTable = new DataTable();
            var expTable = new DataTable();

            var tmpProxyObject = DynSapProxyFactory.CreateProxyObject(bapiName, DateTime.Now, importStruktur,
                                                                      exportStruktur);
            var bapiSapDate = tmpProxyObject.CallBapiForBapi(ref expTable, ref impTable, bapiName, sapConnection);

            try
            {
                //importStruktur Definieren 
                importStruktur.Columns.Add(new DataColumn("Element", typeof (DataTable)));
                importStruktur.Columns.Add(new DataColumn("ElementCode", typeof (string)));
                importStruktur.AcceptChanges();

                //exportStruktur Definieren 
                exportStruktur.Columns.Add(new DataColumn("Element", typeof (DataTable)));
                exportStruktur.Columns.Add(new DataColumn("ElementCode", typeof (string)));
                exportStruktur.AcceptChanges();

                //import
                var parameterTabelle = new DataTable("ParameterTabelle");
                parameterTabelle.Columns.Add(new DataColumn("PARAMETER", typeof (string)));
                parameterTabelle.Columns.Add(new DataColumn("ParameterDATATYPE", typeof (string)));
                parameterTabelle.Columns.Add(new DataColumn("ParameterValue"));
                parameterTabelle.Columns.Add(new DataColumn("ParameterLength", typeof (Int32)));
                parameterTabelle.AcceptChanges();

                var sapTabellen = new DataTable[1];
                var sapConformImportTabellen = new DataTable[1];

                foreach (DataRow tmprowX in impTable.Rows)
                {
                    if ((!ReferenceEquals(tmprowX["STRUCTURE"], DBNull.Value)) &&
                        !string.IsNullOrEmpty(tmprowX["STRUCTURE"].ToString().Trim(' ')))
                    {
                        var found = false;
                        var sapConformCount = 0;
                        DataTable sapConformImportTable;
                        foreach (var tmpDatatable in sapTabellen)
                        {
                            sapConformCount += 1;
                            if ((tmpDatatable != null))
                            {
                                if (tmpDatatable.TableName == tmprowX["STRUCTURE"].ToString())
                                {
                                    found = true;

                                    sapConformImportTable = sapConformImportTabellen[sapConformCount - 1];

                                    switch (tmprowX["DATATYPE"].ToString())
                                    {
                                        case "STRING":
                                            tmpDatatable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(),
                                                                                    typeof (string)));
                                            tmpDatatable.Columns[tmpDatatable.Columns.Count - 1].MaxLength =
                                                Convert.ToInt32(tmprowX["LENGTH"]);
                                            break;
                                        case "DATE":
                                            tmpDatatable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(),
                                                                                    typeof (DateTime)));
                                            break;
                                    }

                                    sapConformImportTable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(),
                                                                                     typeof (string)));
                                    sapConformImportTable.Columns[sapConformImportTable.Columns.Count - 1].MaxLength =
                                        Convert.ToInt32(tmprowX["LENGTH"]);
                                    tmpDatatable.AcceptChanges();
                                    sapConformImportTable.AcceptChanges();
                                    break;
                                }
                            }
                        }
                        if (found == false)
                        {
                            var sapTabelle = new DataTable(tmprowX["STRUCTURE"].ToString());
                            sapTabellen[sapTabellen.Length - 1] = sapTabelle;
                            Array.Resize(ref sapTabellen, sapTabellen.Length + 1);
                            sapConformImportTable = new DataTable(tmprowX["STRUCTURE"].ToString());
                            sapConformImportTabellen[sapConformImportTabellen.Length - 1] = sapConformImportTable;
                            Array.Resize(ref sapConformImportTabellen, sapConformImportTabellen.Length + 1);

                            if (tmprowX["DATATYPE"].ToString() == "STRING")
                            {
                                sapTabelle.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof (string)));
                                sapTabelle.Columns[sapTabelle.Columns.Count - 1].MaxLength =
                                    Convert.ToInt32(tmprowX["LENGTH"]);
                            }
                            else if (tmprowX["DATATYPE"].ToString() == "DATE")
                            {
                                sapTabelle.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(), typeof (DateTime)));
                            }

                            sapConformImportTable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(),
                                                                             typeof (string)));
                            sapConformImportTable.Columns[sapConformImportTable.Columns.Count - 1].MaxLength =
                                Convert.ToInt32(tmprowX["LENGTH"]);
                            sapTabelle.AcceptChanges();
                            sapConformImportTable.AcceptChanges();
                        }
                    }
                    else
                    {
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
                foreach (var dataTable in sapTabellen)
                {
                    if ((dataTable != null))
                    {
                        strukturRow = importStruktur.NewRow();
                        strukturRow["Element"] = dataTable;
                        strukturRow["ElementCode"] = "TABLE";
                        importStruktur.Rows.Add(strukturRow);
                        importStruktur.AcceptChanges();
                    }
                }
                foreach (var dataTable in sapConformImportTabellen)
                {
                    if ((dataTable != null))
                    {
                        strukturRow = importStruktur.NewRow();
                        strukturRow["Element"] = dataTable;
                        strukturRow["ElementCode"] = "SAPTABLE";
                        importStruktur.Rows.Add(strukturRow);
                        importStruktur.AcceptChanges();
                    }
                }

                //export
                var parameterTabelle2 = new DataTable("parameterTabelle");

                parameterTabelle2.Columns.Add(new DataColumn("PARAMETER", typeof (string)));
                parameterTabelle2.Columns.Add(new DataColumn("ParameterDATATYPE", typeof (string)));
                parameterTabelle2.Columns.Add(new DataColumn("ParameterValue", typeof (object)));
                parameterTabelle2.Columns.Add(new DataColumn("ParameterLength", typeof (Int32)));
                parameterTabelle2.AcceptChanges();

                var sapTabellen2 = new DataTable[1];
                foreach (DataRow tmprowX in expTable.Rows)
                {
                    //Trace.WriteLine(String.Join(", ", tmprowX.ItemArray))
                    if ((!ReferenceEquals(tmprowX["STRUCTURE"], DBNull.Value)) &&
                        !string.IsNullOrEmpty(tmprowX["STRUCTURE"].ToString().Trim(' ')))
                    {
                        var found2 = false;
                        foreach (var tmpDatatable in sapTabellen2)
                        {
                            if ((tmpDatatable != null))
                            {
                                if (tmpDatatable.TableName == tmprowX["STRUCTURE"].ToString())
                                {
                                    found2 = true;

                                    switch (tmprowX["DATATYPE"].ToString())
                                    {
                                        case "STRING":
                                            tmpDatatable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(),
                                                                                    typeof (string)));
                                            tmpDatatable.Columns[tmpDatatable.Columns.Count - 1].MaxLength =
                                                Convert.ToInt32(tmprowX["LENGTH"]);
                                            break;
                                        case "DATE":
                                            tmpDatatable.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(),
                                                                                    typeof (DateTime)));
                                            break;
                                    }

                                    tmpDatatable.AcceptChanges();
                                    break;
                                }
                            }
                        }
                        if (!found2)
                        {
                            var sapTabelle = new DataTable(tmprowX["STRUCTURE"].ToString());
                            sapTabellen2[sapTabellen2.Length - 1] = sapTabelle;
                            Array.Resize(ref sapTabellen2, sapTabellen2.Length + 1);

                            switch (tmprowX["DATATYPE"].ToString())
                            {
                                case "STRING":
                                    sapTabelle.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(),
                                                                          typeof (string)));
                                    sapTabelle.Columns[sapTabelle.Columns.Count - 1].MaxLength =
                                        Convert.ToInt32(tmprowX["LENGTH"]);
                                    break;
                                case "DATE":
                                    sapTabelle.Columns.Add(new DataColumn(tmprowX["PARAMETER"].ToString(),
                                                                          typeof (DateTime)));
                                    break;
                            }

                            sapTabelle.AcceptChanges();
                        }
                    }
                    else
                    {
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
                foreach (var dataTable in sapTabellen2)
                {
                    if ((dataTable != null))
                    {
                        strukturRow = exportStruktur.NewRow();
                        strukturRow["Element"] = dataTable;
                        strukturRow["ElementCode"] = "TABLE";
                        exportStruktur.Rows.Add(strukturRow);
                        exportStruktur.AcceptChanges();
                    }
                }

                return DynSapProxyFactory.CreateProxyObject(bapiName, bapiSapDate.GetValueOrDefault(), importStruktur,
                                                            exportStruktur);
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler bei der Erstellung des Proxys für BAPI: " + bapiName + " / " + ex.Message);
            }
        }

        public void GetSerializedBapiStructuresForBapiCheck(string sapFunction, ref byte[] importStructure, ref byte[] exportStructure)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}