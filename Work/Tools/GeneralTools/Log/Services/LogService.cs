using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Elmah;
using GeneralTools.Contracts;
using GeneralTools.Log.Services;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    public class LogService : ILogService
    {
        public string LogFileName { get; set; }

        public string AppName { get; set; }

        public LogService() 
        {
            Init("","");
        }

        public LogService(string appName, string logFileName)
        {
            Init(appName, logFileName);
        }

        void Init(string appName, string logFileName)
        {
            AppName = appName;
            LogFileName = logFileName;
            
        }
        

        public string GetLogFileName(ILogonContext logonContext = null)
        {
            var path = Path.GetDirectoryName(LogFileName);
            if (path == null)
                return LogFileName;

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(LogFileName);
            var extension = Path.GetExtension(LogFileName);

            if (logonContext != null)
                return Path.Combine(path, string.Format("{0}_{1}{2}", fileNameWithoutExtension, logonContext.UserID, extension));

            return Path.Combine(path, string.Format("{0}{1}", fileNameWithoutExtension, extension));
        }

        public LogItem LogFatal(Exception exception, ILogonContext logonContext, object dataContext = null)
        {
            return Log(exception, LogItemLevel.Fatal, logonContext, dataContext);
        }

        public LogItem LogError(Exception exception, ILogonContext logonContext, object dataContext = null)
        {
            return Log(exception, LogItemLevel.Error, logonContext, dataContext);
        }

        public void LogElmahError(Exception exception, ILogonContext logonContext, object dataContext = null)
        {
            Exception exceptionToLog;

            // Warum die Ausnahmebehandlung?
            // weil einige der DataContext Klassen keinen paramterlosen Constructor haben und deshalb nicht serliasiert werden können
            try
            {
                exceptionToLog = new CkgSessionWrapperException(XmlService.XmlSerializeRawBulkToString(dataContext), exception);
            }
            catch (Exception)
            {
                exceptionToLog = exception;
            }

            // Wichtig: erfrage das httpContext, falls vorhanden werden die Daten des Context mitgeloggt
            var elmahError = HttpContext.Current == null ? new Error(exceptionToLog) : new Error(exception, HttpContext.Current);
             
            var logger = new ElmahLogger();

            var appurl = "/";

            if (logonContext != null && !string.IsNullOrEmpty(logonContext.AppUrl))
            {
                appurl = logonContext.AppUrl;
            }
            else
            {
                if (!string.IsNullOrEmpty(AppName))
                {
                    appurl = AppName;
                }
            }



            logger.Log(appurl,
                       elmahError.HostName,
                       elmahError.Type,
                       elmahError.Source,
                       elmahError.Message,
                       elmahError.User,
                       ErrorXml.EncodeString(elmahError),
                       elmahError.StatusCode,
                       elmahError.Time
                );
        }

        public LogItem LogInfo(ILogonContext logonContext, object dataContext = null)
        {
            return Log(null, LogItemLevel.Info, logonContext, dataContext);
        }

        public LogItem Log(Exception exception, LogItemLevel level, ILogonContext logonContext, object dataContext = null)
        {
            var logItem = new LogItem
                {
                    Level = level, 
                    DataContext = dataContext
                };

            return Log(logItem, logonContext, exception);
        }

        public LogItem Log(LogItem logItem, ILogonContext logonContext, Exception exception = null, object dataContext = null)
        {
            try
            {
                logItem.LogonContext = logonContext;

                logItem.WebAppName = AppName;

                if (logItem.ID.IsNullOrEmpty())
                    logItem.ID = Guid.NewGuid().ToString();

                if (logItem.Date == null)
                    logItem.Date = DateTime.Now;

                if (exception != null)
                    logItem.ExceptionMessage = exception.Message;

                if (logItem.StackContext == null)
                {
                    var stackContext = new StackContext();
                    stackContext.Init(exception);
                    logItem.StackContext = stackContext;
                }

                logItem.ChildLogItemID = GetLogItemIDtoLinkAsChild(logonContext);

                XmlService.BulkWriteXmlItem(GetLogFileName(logonContext), logItem);
            }
            catch
            {
                // ... a general catch is necessary here to prevent stackoverflow as a result of recursive logging events at higher application levels ...
                // ToDo: Useful would be an email notification to global logging admins at this point.
            }

            // reset an occassionally pointer to a linked child logItem:
            SetLogItemIDtoLinkAsChild(logonContext, null);

            return logItem;
        }

        /// <summary>
        /// SAP Aufrufe loggen.  
        /// </summary>
        /// <param name="bapiName"></param>
        /// <param name="logon"></param>
        /// <param name="import">Import Parameter und Import Tabellen schreiben</param>
        /// <param name="export">Export Parameter und die Export-Tabellen Info (Name der Tabelle und Anzahl der Sätze) schriben</param>
        /// <param name="success"></param>
        /// <param name="dauer">Dauer des Aufrufs (Aufbereitung + SAP Aufruf) in Sekunden</param>        
        public void LogSapCall(string bapiName, string logon, DataTable import, DataTable export, bool success, double dauer)
        {
            var importParams = ((DataTable)import.Select("ElementCode='PARA'")[0][0]);

            var importTables = new List<DataTable>();

            foreach (var tmpRow in import.Select("ElementCode='TABLE'"))
                if ((!ReferenceEquals(tmpRow[0], DBNull.Value)))
                    importTables.Add((DataTable)tmpRow[0]);

            var exportParams = ((DataTable)export.Select("ElementCode='PARA'")[0][0]);

            var exportTables = new List<DataTable>();

            foreach (var tmpRow in export.Select("ElementCode='TABLE'"))
                if ((!ReferenceEquals(tmpRow[0], DBNull.Value)))
                    exportTables.Add((DataTable)tmpRow[0]);

            // Achtung: die Export Tables werden nicht serializiert sondern nur die Namen der Tabellen und die Anzahl der Elemente wird ermittelt
            var exportTablesInfo = new XElement("ExportTables", from table in exportTables select new XElement("ExportTable", new XAttribute("TableName", table.TableName), new XAttribute("RowCount", table.Rows.Count)));

            int appID, userID, customerID, kunnr, portalType;
            TrySessionGetUserData(out appID, out userID, out customerID, out kunnr, out portalType);

            var sapLogger = new SapLogger();
            sapLogger.Log(
                appID, userID, customerID, kunnr, portalType,
                logon,
                bapiName,
                XmlService.XmlSerializeRawBulkToString(importParams),
                XmlService.XmlSerializeRawBulkToString(importTables),
                string.Empty,
                XmlService.XmlSerializeRawBulkToString(logon),
                success,
                dauer,
                XmlService.XmlSerializeRawBulkToString(exportParams),
                XmlService.XmlSerializeRawBulkToString(exportTablesInfo));
        }

        public void LogPageVisit(int appID, int userID, int customerID, int kunnr, int portalType, string clientIp = null)
        {
            TrySessionSetUserData(appID, userID, customerID, kunnr, portalType);
            //TrySessionGetUserData(out appID, out userID, out customerID, out kunnr, out portalType);

            var logger = new PageVisitLogger();
            logger.Log(appID, userID, customerID, kunnr, portalType, clientIp);
        }

        static void TrySessionSetUserData(int appID, int userID, int customerID, int kunnr, int portalType)
        {
            TrySessionSetIntValue("LastAppID", appID);
            TrySessionSetIntValue("LastUserID", userID);
            TrySessionSetIntValue("LastCustomerID", customerID);
            TrySessionSetIntValue("LastKunnr", kunnr);
            TrySessionSetIntValue("LastPortalType", portalType);
        }

        static void TrySessionGetUserData(out int appID, out int userID, out int customerID, out int kunnr, out int portalType)
        {
            appID = TrySessionGetIntValue("LastAppID");
            userID = TrySessionGetIntValue("LastUserID");
            customerID = TrySessionGetIntValue("LastCustomerID");
            kunnr = TrySessionGetIntValue("LastKunnr");
            portalType = TrySessionGetIntValue("LastPortalType");

            //portalType = -2;
            //if (HttpContext.Current == null)
            //    portalType = -5;
            //if (HttpContext.Current != null && HttpContext.Current.Session == null)
            //    portalType = -9;
        }

        static void TrySessionSetIntValue(string key, int value)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                return;

            HttpContext.Current.Session[key] = value;
        }

        static int TrySessionGetIntValue(string key)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                return 0;

            var sValue = HttpContext.Current.Session[key];
            if (sValue == null)
                return 0;
            
            return sValue.ToString().ToInt(0);
        }

        #region LogItemIdsToLinkAsChild

        /// <summary>
        /// used for more detailed logging information in deeper scope (like SAP).
        /// points to anothe logitem that provides info about this scope.
        /// </summary>

        private readonly Dictionary<string, string> _logItemIdsToLinkAsChild = new Dictionary<string, string>();
        public Dictionary<string, string> LogItemIdsToLinkAsChild
        {
            get { return _logItemIdsToLinkAsChild; }
        }

        static string GetLogItemIDtoLinkAsChildKey(ILogonContext logonContext)
        {
            return logonContext == null ? "default" : logonContext.UserID;
        }

        public string GetLogItemIDtoLinkAsChild(ILogonContext logonContext)
        {
            var key = GetLogItemIDtoLinkAsChildKey(logonContext);

            if (!LogItemIdsToLinkAsChild.ContainsKey(key))
                return null;

            string val;
            LogItemIdsToLinkAsChild.TryGetValue(key, out val);

            return val;
        }

        public void SetLogItemIDtoLinkAsChild(ILogonContext logonContext, string value)
        {
            var key = GetLogItemIDtoLinkAsChildKey(logonContext);

            if (LogItemIdsToLinkAsChild.ContainsKey(key))
                LogItemIdsToLinkAsChild.Remove(key);

            LogItemIdsToLinkAsChild.Add(key, value);
        }

        #endregion
    }
}
