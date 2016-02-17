using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GeneralTools.Models;
using SapORM.Contracts;
using GeneralTools.Contracts;

namespace SapORM.Services
{
    public class SapDataService : ISapDataService
    {
        public ISapConnection SapConnection { get; private set; }

        public IDynSapProxyFactory DynSapProxyFactory { get; private set; }

        private static ISapDataService _defaultSapDataService;
        public static ISapDataService DefaultInstance { get { return (_defaultSapDataService ?? (_defaultSapDataService = new SapDataServiceDefaultFactory().Create())); } }

        private IDynSapProxyObject _sapProxy;

        public int ResultCode
        {
            get
            {
                if (_sapProxy == null) return -1;

                try
                {
                    int resultCode;
                    if (Int32.TryParse(_sapProxy.GetExportParameter("E_SUBRC"), out resultCode))
                        return resultCode;
                }
                catch { return 0; }
                return -1;
            }
        }

        private bool _noDataOccured;

        public string ResultMessage
        {
            get
            {
                if (_sapProxy == null) return "-1";
                if (_noDataOccured) return "Keine Daten vorhanden";

                string ret;
                try { ret = _sapProxy.GetExportParameter("E_MESSAGE"); }
                catch { ret = ""; }
                return ret;
            }
        }

        public Func<ILogonContext> GetLogonContext { get; set; }
        ILogonContext LogonContext { get { return GetLogonContext == null ? null : GetLogonContext(); } }

        public Func<ILogService> GetLogService { get; set; }
        ILogService LogService { get { return GetLogService == null ? null : GetLogService(); } }


        public SapDataService(ISapConnection sapConnection, IDynSapProxyFactory dynSapProxyFactory, Func<ILogService> getLogService = null)
        {
            SapConnection = sapConnection;
            DynSapProxyFactory = dynSapProxyFactory;

            GetLogService = getLogService;
        }

        public void Dispose()
        {
        }


        private IDynSapProxyObject GetProxy(string sapFunction)
        {
            return DynSapProxyFactory.CreateProxyCache(sapFunction, SapConnection, DynSapProxyFactory).GetProxy();
        }

        public byte[] GetSerializedBapiStructuresForBapiCheck(string sapFunction)
        {
            sapFunction = sapFunction.NotNullOrEmpty().ToUpper();

            var sapProxy = DynSapProxyFactory.CreateProxyCache(sapFunction, SapConnection, DynSapProxyFactory).GetEmptyProxy();

            return sapProxy.GetBapiStructureSerialized();
        }

        private void PrepareBapi(string sapFunction, string inputParameterKeys, params object[] inputParameterValues)
        {
            _sapProxy = GetProxy(sapFunction);
            if (_sapProxy == null)
                return;

            //var count = ((DataTable)_sapProxy.Import.Select("ElementCode='PARA'")[0][0]).Rows.Count;
            //_sapProxy.Reset();

            if (!string.IsNullOrEmpty(inputParameterKeys))
            {
                var inputParameterKeyArray = inputParameterKeys.Split(',').ToArray();
                for (var i = 0; i < inputParameterKeyArray.Length; i++)
                    _sapProxy.SetImportParameter(inputParameterKeyArray[i].Trim(), 
                                                  (inputParameterValues[i] == null) ? null : inputParameterValues[i].ToString());
            }
        }

        public void Init(string sapFunction, string inputParameterKeys, params object[] inputParameterValues)
        {
            PrepareBapi(sapFunction, inputParameterKeys, inputParameterValues);
        }

        public void Init(string sapFunction)
        {
            Init(sapFunction, "", null);
        }

        public void InitExecute(string sapFunction, string inputParameterKeys, params object[] inputParameterValues)
        {
            Init(sapFunction, inputParameterKeys, inputParameterValues);
            Execute();
        }

        public void InitExecute(string sapFunction)
        {
            Init(sapFunction, "", null);
            Execute();
        }

        public void Execute()
        {
            if (_sapProxy == null) return;
            _noDataOccured = (_sapProxy.CallBapi(LogService, LogonContext) == false);
        }


        #region Export

        public IEnumerable<DataTable> GetExportTablesWithInitExecute(string sapFunction, string inputParameterKeys, params object[] inputParameterValues)
        {
            InitExecute(sapFunction, inputParameterKeys, inputParameterValues);

            return GetExportTables();
        }

        public IEnumerable<DataTable> GetExportTablesWithInitExecute(string sapFunction)
        {
            InitExecute(sapFunction);

            return GetExportTables();
        }

        public DataTable GetExportTableWithInitExecute(string sapFunctionAndTable, string inputParameterKeys, params object[] inputParameterValues)
        {
            return GetTableUsingFunction(GetExportTablesWithInitExecute, sapFunctionAndTable, inputParameterKeys, inputParameterValues);
        }

        public DataTable GetExportTableWithInitExecute(string sapFunctionAndTable)
        {
            return GetTableUsingFunction(GetExportTablesWithInitExecute, sapFunctionAndTable, "");
        }

        public IEnumerable<DataTable> GetExportTables()
        {
            if (_sapProxy == null) return new List<DataTable>();
            return _sapProxy.GetExportTables();
        }

        public DataTable GetExportTable(string tableName)
        {
            if (_sapProxy == null) return new DataTable();
            return _sapProxy.GetExportTable(tableName);
        }

        public IEnumerable<DataTable> GetExportTablesWithExecute()
        {
            Execute();
            return GetExportTables();
        }

        public DataTable GetExportTableWithExecute(string tableName)
        {
            Execute();
            return GetExportTable(tableName);
        }

        public string GetExportParameterWithInitExecute(string sapFunction, string paramName, string inputParameterKeys, params object[] inputParameterValues)
        {
            InitExecute(sapFunction, inputParameterKeys, inputParameterValues);
            return GetExportParameter(paramName);
        }

        public byte[] GetExportParameterByteWithInitExecute(string sapFunction, string paramName, string inputParameterKeys, params object[] inputParameterValues)
        {
            InitExecute(sapFunction, inputParameterKeys, inputParameterValues);
            return GetExportParameterByte(paramName);
        }

        public string GetExportParameterWithExecute(string paramName)
        {
            Execute();
            return GetExportParameter(paramName);
        }

        public string GetExportParameter(string paramName)
        {
            if (_sapProxy == null) return "";
            return _sapProxy.GetExportParameter(paramName);
        }

        public byte[] GetExportParameterByte(string paramName)
        {
            if (_sapProxy == null) return null;
            return _sapProxy.GetExportParameterByte(paramName);
        }

        public T GetExportParameter<T>(string paramName)
        {
            if (_sapProxy == null) return default(T);
            return _sapProxy.GetExportParameter<T>(paramName);
        }

        #endregion


        #region Import

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „Init“ aus und liefert eine Liste aller Import-Tabellen zurück.
        /// </summary>
        public IEnumerable<DataTable> GetImportTablesWithInit(string sapFunction, string inputParameterKeys, params object[] inputParameterValues)
        {
            Init(sapFunction, inputParameterKeys, inputParameterValues);

            return GetImportTables();
        }

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „Init“ aus und liefert eine Liste aller Import-Tabellen zurück.
        /// </summary>
        public IEnumerable<DataTable> GetImportTablesWithInit(string sapFunction) {
            Init(sapFunction);

            return GetImportTables();
        }

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „Init“ aus und liefert eine bestimmte Import-Tabelle zurück.
        /// Die Aufruf-Konvention führ den Parameter „sapFunctionAndTable“ lautet: 
        /// [Bapi-Name].[Import-Tabellenname]  (Bapi und Tabelle werden mit einem Punkt getrennt)
        /// </summary>
        public DataTable GetImportTableWithInit(string sapFunctionAndTable, string inputParameterKeys, params object[] inputParameterValues)
        {
            return GetTableUsingFunction(GetImportTablesWithInit, sapFunctionAndTable, inputParameterKeys, inputParameterValues);
        }

        /// <summary>
        /// Führt für eine SAP-Funktion (Bapi) ein „Init“ aus und liefert eine bestimmte Import-Tabelle zurück.
        /// Die Aufruf-Konvention führ den Parameter „sapFunctionAndTable“ lautet: 
        /// [Bapi-Name].[Import-Tabellenname]  (Bapi und Tabelle werden mit einem Punkt getrennt)
        /// </summary>
        public DataTable GetImportTableWithInit(string sapFunctionAndTable)
        {
            return GetTableUsingFunction(GetImportTablesWithInit, sapFunctionAndTable,"");
        }

        /// <summary>
        /// Liefert eine Liste aller SAP Import-Tabellen zurück ohne „Init“.
        /// Ein „Init“ muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        public IEnumerable<DataTable> GetImportTables()
        {
            if (_sapProxy == null) return new List<DataTable>();
            return _sapProxy.GetImportTables();
        }

        /// <summary>
        /// Liefert eine bestimmte SAP Import-Tabelle zurück ohne „Init“.
        /// Ein „Init“ muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        public DataTable GetImportTable(string tableName)
        {
            if (_sapProxy == null) return new DataTable();
            return _sapProxy.GetImportTable(tableName);
        }

        /// <summary>
        /// Füllt einen bestimmten Import-Parameter. 
        /// Ein "Init" muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        public void SetImportParameter(string paramName, object value)
        { 
            if (_sapProxy == null)
                return;

            if (!string.IsNullOrEmpty(paramName))
            {
                if (value is  byte[])
                    _sapProxy.SetImportParameter(paramName, value);
                else
                    _sapProxy.SetImportParameter(paramName, (value == null) ? null : value.ToString());
            }       
        }

        /// <summary>
        /// Ersetzt eine bestimmte Import-Tabelle. 
        /// Ein "Init" muss aber im Vorwege separat durchgeführt worden sein.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        public void SetImportTable(string paramName, DataTable value)
        {
            if (_sapProxy == null)
                return;

            if (!string.IsNullOrEmpty(paramName))
            {
                _sapProxy.SetImportTable(paramName, value);
            }
        }

        #endregion


        #region Template functions

        public void ApplyImport<T>(IEnumerable<T> list)
        {
            if (_sapProxy == null) return;

            var importTable = _sapProxy.GetImportTables().FirstOrDefault(table => table.TableName.ToUpper() == typeof(T).Name.ToUpper());

            if (importTable != null)
                SapDataServiceExtensions.Apply(list, importTable);
        }

        private static DataTable GetTableUsingFunction(Func<string, string, object[], IEnumerable<DataTable>> tableFunction,
                                                    string sapFunctionAndTable, string inputParameterKeys, params object[] inputParameterValues)
        {
            var tableName = "";
            var functionName = sapFunctionAndTable;
            if (sapFunctionAndTable.Contains("."))
            {
                var arr = sapFunctionAndTable.Split('.');
                functionName = arr[0];
                tableName = arr[1];
            }
            //else
            //{
            //    if (_sapProxy != null)
            //    {
            //        // wenn der Sap Proxy bereits initialisiert ist und wir einen sapFunctionAndTable ohne "." haben
            //        // ==> interpretieren wir sapFunctionAndTable als reinen Tabellen-Namen
            //        functionName = _sapProxy.BapiName;
            //        tableName = sapFunctionAndTable;
            //    }
            //}

            var tables = tableFunction(functionName, inputParameterKeys, inputParameterValues);
            if (tables != null)
                return (string.IsNullOrEmpty(tableName) ? tables.FirstOrDefault() : tables.FirstOrDefault(t => t.TableName.ToLower() == tableName.ToLower()));
            
            return null;
        }

        public string ExecuteAndCatchErrors(Action sapAction, Func<string> getCustomErrorMessageFunction = null, bool ignoreResultCode = false)
        {
            var errorMessage = "";

            try
            {
                sapAction();
            }
            catch (Exception e)
            {
                return string.Format("Es ist ein System Fehler aufgetreten: {0}", e.Message);
            }

            if (!ignoreResultCode && this.ResultCode != 0)
                errorMessage = string.Format("Ihre Anforderung konnte im System nicht erstellt werden, Fehlermeldung: {0}", this.ResultMessage);

            if (getCustomErrorMessageFunction != null)
            {
                var customErrorMessage = getCustomErrorMessageFunction();
                if (customErrorMessage.IsNotNullOrEmpty())
                {
                    if (!errorMessage.NotNullOrEmpty().ToLower().Contains(customErrorMessage.NotNullOrEmpty().ToLower()))
                    {
                        errorMessage += errorMessage.IsNotNullOrEmpty()
                                            ? ";Meldung im Detail: "
                                            : "Eine oder mehrere Anforderungen konnten im System nicht erstellt werden: ";
                        errorMessage += customErrorMessage;
                    }
                }
            }

            return errorMessage;
        }

        #endregion
    }
}
