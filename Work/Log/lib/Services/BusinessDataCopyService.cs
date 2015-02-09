using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using Elmah;
using GeneralTools.Log.Models.MultiPlatform;
using GeneralTools.Log.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using LogMaintenance.Models;

namespace LogMaintenance.Services
{
    public class BusinessDataCopyService
    {
        private static Action<string> _infoMessageAction;


        #region MaintenanceLogsDb

        public static bool MaintenanceLogsDb(Action<string> infoMessageAction, string appDataFilePath)
        {
            _infoMessageAction = infoMessageAction;

            return Directory.GetFiles(appDataFilePath).All(xmlFileName => MaintenanceLogsDbForServer("Prod", xmlFileName));
        }

        private static bool MaintenanceLogsDbForServer(string serverType, string appDataFileName)
        {
            if (serverType.IsNullOrEmpty())
                return false;

            var logsDbContext = CreateLogsDbContext(serverType);
            ((IObjectContextAdapter)logsDbContext).ObjectContext.CommandTimeout = 3600;

            var sqlMaintenanceTables = XmlService.XmlTryDeserializeFromFile<DbMaintenanceTable[]>(appDataFileName);
            foreach (var sqlMaintenanceTable in sqlMaintenanceTables)
            {
                foreach (var sqlMaintenanceStep in sqlMaintenanceTable.Steps)
                {
                    var multiSqlSteps = new [] { "" };
                    if (sqlMaintenanceStep.IsSqlIndexStatement)
                        multiSqlSteps = sqlMaintenanceTable.GetTableIndexColumnNames();

                    foreach (var multiSqlStep in multiSqlSteps)
                    {
                        var sql = sqlMaintenanceTable.PrepareStatement(sqlMaintenanceStep.Sql, multiSqlStep);
                        var success = ExecuteSqlCommand(logsDbContext, sql, new object[0]);
                        if (!success && !sqlMaintenanceStep.IgnoreSqlException)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Die Log DB soll in regelmäßigen Abständen von alteren Inhalten und Datensätze bereinigt werden
        /// </summary>
        /// <param name="serverType">Prod|Test|Dev</param>
        /// <param name="pageVisitExpirydate">PageVisist Nachrichten älter als das Datum werden gelöscht</param>
        /// <param name="sapBapiExpiryDate">SAP BAPI Nachrichten älter als das Datum werden gelöscht</param>
        /// <param name="sapBapiDataExpiryDate">DataContext, ImportParameters, ImportTables, werden gelöscht in SAP Logs wenn Nachricht älter als das Datum</param>
        /// <param name="elmahExpiryDate"></param>
        /// <returns>Ist eine Operation fehlgeschlagen?</returns>
        public static bool DeleteExpiredLogMessages(string serverType, DateTime pageVisitExpirydate, DateTime sapBapiExpiryDate, DateTime sapBapiDataExpiryDate, DateTime elmahExpiryDate)
        {
            // SQL_SAFE_UPDATES, MySql Globale Server Variable wird ausgesetzt (== 0) während der Operation, da sonst die Operation fehlschlägt
            var deleteExpiredPageVisits = string.Format("SET SQL_SAFE_UPDATES = 0;DELETE FROM PageVisit WHERE time_stamp < '{0}';SET SQL_SAFE_UPDATES = 1;", pageVisitExpirydate.Date.ToString("yyyy-MM-dd"));
            var deleteExpiredBapi = string.Format("SET SQL_SAFE_UPDATES = 0;DELETE FROM SapBapi WHERE time_stamp < '{0}';SET SQL_SAFE_UPDATES = 1;", sapBapiExpiryDate.Date.ToString("yyyy-MM-dd"));
            var deleteExpiredBapiData = string.Format("SET SQL_SAFE_UPDATES = 0;UPDATE SapBapi SET ImportParameters = '', ImportTables = '', ExportParameters ='', ExportTables = '' WHERE time_stamp < '{0}';SET SQL_SAFE_UPDATES = 1;", sapBapiDataExpiryDate.Date.ToString("yyyy-MM-dd"));
            const string deleteServiceMenueMessages = "SET SQL_SAFE_UPDATES = 0;DELETE FROM elmah_error WHERE AllXml like '%bei ASP.menue_servicesmenue_ascx.__RenderPanel1%';SET SQL_SAFE_UPDATES = 1;";
            var deleteExpiredElmah = string.Format("SET SQL_SAFE_UPDATES = 0;DELETE FROM elmah_error WHERE TimeUtc < '{0}';SET SQL_SAFE_UPDATES = 1;", elmahExpiryDate.Date.ToString("yyyy-MM-dd"));
            const string optimizeElmahTabelleAfterDelete = "SET SQL_SAFE_UPDATES = 0;OPTIMIZE TABLE elmah_error;SET SQL_SAFE_UPDATES = 1;";

            var commands = new List<string>
                {
                    deleteExpiredPageVisits,
                    deleteExpiredBapi,
                    deleteExpiredBapiData,
                    deleteServiceMenueMessages,
                    deleteExpiredElmah,
                    optimizeElmahTabelleAfterDelete
                };

            var status = from command in commands
                         let result = ExecuteSqlCommand(CreateLogsDbContext(serverType), command, new object[0])
                         select result;

            return status.All(x => x);
        }

        public static bool OptimizeTables(string serverType)
        {
            const string sapBapi = "sapbapi;";
            const string elmah = "elmah_error;";
            const string pageVisit = "pagevisit;";
            const string optimizeTable = "OPTIMIZE TABLE {0}";

            var commands = new List<string>
                {
                    string.Format(optimizeTable, sapBapi),
                    string.Format(optimizeTable, elmah),
                    string.Format(optimizeTable, pageVisit)
                };

            var status = from command in commands
                         let result = ExecuteSqlCommand(CreateLogsDbContext(serverType), command, new object[0])
                         select result;

            return status.All(x => x);
        }

        public static bool CreateElmahShortcutLandingpage(string serverType, string path)
        {
            const string html = "<html>\r\n\t<body>\r\n\t\t<table>\r\n{0}\r\n\t\t</table>\r\n\t</body></html>";
            const string tableRow = "\t\t\t<tr>\r\n\t\t\t\t<td>{0}</td>\r\n\t\t\t\t<td>{1}</td>\r\n\t\t\t\t<td>{2}</td>\r\n\t\t\t</tr>";
            const string testurl = "<a href=\"/elmahviewer/Test/elmah.axd?app={0}\">Test</a>";
            const string produrl = "<a href=\"/elmahviewer/Prod/elmah.axd?app={0}\">Prod</a>";

            var logsDbContext = CreateLogsDbContext(serverType);
            ((IObjectContextAdapter)logsDbContext).ObjectContext.CommandTimeout = 3600;
            var apps = logsDbContext.Database.SqlQuery<string>("SELECT Distinct application FROM elmah_error");

            var q = (from app in apps
                     let testlink = string.Format(testurl, System.Net.WebUtility.HtmlEncode(app))
                     let prodlink = string.Format(produrl, System.Net.WebUtility.HtmlEncode(app))
                     let row = string.Format(tableRow, app, testlink, prodlink)
                     select string.Concat(row, Environment.NewLine)).Aggregate(string.Concat);

            var htmlready = string.Format(html, q);

            var pathOfIndexHtml = Path.Combine(path, "Index.html");

            File.WriteAllText(pathOfIndexHtml, htmlready);

            return true;
        }

        #endregion

        #region CopyToLogsDB

        public static bool CopyToLogsDb(Action<string> infoMessageAction)
        {
            _infoMessageAction = infoMessageAction;

            //CopyToLogsDbForServer("Dev");
            //CopyToLogsDbForServer("Test");
            CopyToLogsDbForServer("Prod");

            return true;
        }

        private static void CopyToLogsDbForServer(string serverType)
        {
            if (serverType.IsNullOrEmpty())
                return;

            CopyToLogsDb<MpWebUser>(serverType);
            CopyToLogsDb<MpCustomer>(serverType);
            CopyToLogsDb<MpApplicationTranslated>(serverType);
            CopyCustomerRightsToLogsDb(serverType);
        }

        private static void CopyToLogsDb<T>(string serverType) where T : class, new()
        {
            var tableName = typeof (T).Name;
            var tableAttribute = typeof (T).GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();
            if (tableAttribute != null)
                tableName = tableAttribute.Name;

            var businessDbContext = CreateBusinessDbContext(serverType);
            var logsDbContext = CreateLogsDbContext(serverType);

            var sql = string.Concat("delete from ", tableName);
            ExecuteSqlCommand(logsDbContext, sql, new object[0]); //  Fehler werden geschrieben via ELMAH

            logsDbContext.SaveChanges();

            var businessData = businessDbContext.GetData<T>(tableName);
            if (businessData.None())
                return;

            businessData.ToList().ForEach(m => logsDbContext.AddData(ModelMapping.Copy(m)));
            logsDbContext.SaveChanges();
        }

        private static void CopyCustomerRightsToLogsDb(string serverType)
        {
            var businessDbContext = CreateBusinessDbContext(serverType);
            var logsDbContext = CreateLogsDbContext(serverType);

            ExecuteSqlCommand(logsDbContext, "DELETE FROM CustomerRights", new object[0]);
            logsDbContext.SaveChanges();

            var businessData = businessDbContext.Database.SqlQuery<MpCustomerRights>("SELECT * FROM CustomerRights");
            if (businessData.None())
                return;

            businessData.ToList().ForEach(m => ExecuteSqlCommand(logsDbContext, "INSERT INTO CustomerRights (CustomerID,AppID) VALUES ({0},{1})", m.CustomerID, m.AppID));
        }

        #endregion

        #region Misc

        private static MultiDbPlatformContext CreateLogsDbContext(string serverType)
        {
            var logsConnectionString = string.Format("Logs{0}", serverType);
            var multiDbPlatformContext = new MultiDbPlatformContext(logsConnectionString);
            ((IObjectContextAdapter)multiDbPlatformContext).ObjectContext.CommandTimeout = 3600;
            return multiDbPlatformContext;
        }

        private static MultiDbPlatformContext CreateBusinessDbContext(string serverType)
        {
            var businessConnectionString = string.Format("Source{0}", serverType);
            return new MultiDbPlatformContext(businessConnectionString);
        }

        /// <summary>
        /// Führt einen SQL Befehl aus mit Hilfe des angegebenen DB Contexts
        /// Fehler werden direkt an ELMAH weitergeleitet
        /// </summary>
        /// <param name="multiDbPlatformContext">Geladener DB Context</param>
        /// <param name="sql">SQL Befehl</param>
        /// <param name="parameters">Parameter</param>
        /// <returns>Ist der Befehl erfolgreich ausgeführt werden, Anzahl der AffectedRows wird nicht berücksichtigt</returns>
        private static bool ExecuteSqlCommand(MultiDbPlatformContext multiDbPlatformContext, string sql, params object[] parameters)
        {
            try
            {
                multiDbPlatformContext.Database.ExecuteSqlCommand(sql, parameters);
                return true;
            }
            catch (Exception e)
            {
                var elmahLogger = new ElmahLogger();
                var elmahError = new Error(e);
                elmahLogger.Log("LogMaintainance",
                    elmahError.HostName,
                    elmahError.Type,
                    elmahError.Source,
                    elmahError.Message,
                    elmahError.User,
                    ErrorXml.EncodeString(elmahError),
                    elmahError.StatusCode,
                    elmahError.Time
                    );
                return false;
            }
        }

        #endregion
    }
}
