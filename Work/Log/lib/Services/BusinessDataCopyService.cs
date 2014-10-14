using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GeneralTools.Log.Models.MultiPlatform;
using GeneralTools.Models;
using GeneralTools.Services;
using LogMaintenance.Models;
using MySql.Data.MySqlClient;

namespace LogMaintenance.Services
{
    public class BusinessDataCopyService
    {
        private static Action<string> _infoMessageAction;


        #region MaintenanceLogsDb

        public static bool MaintenanceLogsDb(Action<string> infoMessageAction, string appDataFilePath)
        {
            _infoMessageAction = infoMessageAction;

            foreach (var xmlFileName in Directory.GetFiles(appDataFilePath))
                if (!MaintenanceLogsDbForServer("Prod", xmlFileName))
                    return false;

            return true;
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
                Alert(string.Format("\r\n***  Refreshing content of table '{0}'  ***", sqlMaintenanceTable.DestTableName));

                foreach (var sqlMaintenanceStep in sqlMaintenanceTable.Steps)
                {
                    Alert(sqlMaintenanceTable.PrepareStatement(sqlMaintenanceStep.Description.Trim()));

                    var multiSqlSteps = new [] { "" };
                    if (sqlMaintenanceStep.IsSqlIndexStatement)
                        multiSqlSteps = sqlMaintenanceTable.GetTableIndexColumnNames();

                    foreach (var multiSqlStep in multiSqlSteps)
                    {
                        try
                        {
                            var sql = sqlMaintenanceTable.PrepareStatement(sqlMaintenanceStep.Sql, multiSqlStep);
                            var stopWatch = Stopwatch.StartNew();
                            logsDbContext.Database.ExecuteSqlCommand(sql);
                            stopWatch.Stop();
                            Alert(string.Format("(Dauer: {0:00}:{1:00})", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds));
                        }
                        catch (MySqlException e)
                        {
                            if (!sqlMaintenanceStep.IgnoreSqlException)
                            {
                                Alert(string.Format("\r\nA MySql Error has occured:\r\n>> {0} <<\r\nAborting !!!\r\n", e.Message));
                                return false;
                            }

                        }
                    }
                }

                Alert(string.Format("\r\n***   Table '{0}' finished ;-)   ***\r\n", sqlMaintenanceTable.DestTableName));
            }

            return true;
        }

        /// <summary>
        /// Die Log DB soll in regelmäßigen Abständen von alteren Inhalten und Datensätze bereinigt werden
        /// - Page-Visits: 2 Jahre
        /// - BAPI-Aufrufe mit den entsprechenden Laufzeiten: 1 Jahr
        /// - BAPI-Aufrufe mit kompletten Data-Context (welcher User hat welche Daten an SAP übergeben): 4 Monate
        /// </summary>
        /// <param name="infoMessageAction">Output für Nachrichten</param>
        /// <param name="serverType">Prod|Test|Dev</param>
        /// <param name="pageVisitExpirydate">Nachrichten älter als das Datum werden gelöscht</param>
        /// <param name="sapBapiExpiryDate">Nachrichten älter als das Datum werden gelöscht</param>
        /// <param name="sapBapiDataExpiryDate">DataContext, ImportParameters, ImportTables, werden gelöscht in SAP Logs wenn Nachricht älter als das Datum</param>
        /// <returns></returns>
        public static bool MaintenanceLogsDb(Action<string> infoMessageAction, string serverType, DateTime pageVisitExpirydate, DateTime sapBapiExpiryDate, DateTime sapBapiDataExpiryDate)
        {
            if (serverType.IsNullOrEmpty())
            {
                infoMessageAction.Invoke("Löschen von alten PageVisit Einträgen: Kein Server Type (prod, test, dev) angegeben, Operation wird abgebrochen");
                return false;                
            }

            var logsDbContext = CreateLogsDbContext(serverType);
            
            // Ich habe keinen neuen Step in PageVisit.xml eingetragen: habe keine Möglichkeit gefunden zu parametrisieren...
            // SQL_SAFE_UPDATES, MySql Globale Server Variable wird ausgesetzt (== 0) während der Operation, da sonst die Operation fehlschlägt
            var deleteExpiredPageVisits = string.Format("SET SQL_SAFE_UPDATES = 0;DELETE FROM pagevisit WHERE time_stamp < '{0}';SET SQL_SAFE_UPDATES = 1;", pageVisitExpirydate.Date.ToString("yyyy-MM-dd"));
            var deleteExpiredBapiVisits = string.Format("SET SQL_SAFE_UPDATES = 0;DELETE FROM sapbapi WHERE time_stamp < '{0}';SET SQL_SAFE_UPDATES = 1;", sapBapiExpiryDate.Date.ToString("yyyy-MM-dd"));
            var deleteExpiredBapiData = string.Format("SET SQL_SAFE_UPDATES = 0;UPDATE sapbapi SET ImportParameters = '', ImportTables = '', DataContext ='' WHERE time_stamp < '{0}';SET SQL_SAFE_UPDATES = 1;", sapBapiDataExpiryDate.Date.ToString("yyyy-MM-dd"));

            var commands = new List<string>
                {
                    deleteExpiredPageVisits,
                    deleteExpiredBapiVisits,
                    deleteExpiredBapiData
                };

            commands.ForEach(x =>
                {
                    try
                    {
                        logsDbContext.Database.ExecuteSqlCommand(x, new object[0]);
                    }
                    catch (Exception e)
                    {
                        infoMessageAction.Invoke(string.Format(@"Folgender Befehl schlug fehl\r\n\{0}\r\nFehlermeldung ist {1}",x, e.Message));
                    }
                });

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

            Alert("");
        }

        private static void CopyToLogsDb<T>(string serverType) where T : class, new()
        {
            var tableName = typeof (T).Name;
            var tableAttribute = typeof (T).GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();
            if (tableAttribute != null)
                tableName = tableAttribute.Name;

            var businessDbContext = CreateBusinessDbContext(serverType);
            var logsDbContext = CreateLogsDbContext(serverType);

            logsDbContext.Database.ExecuteSqlCommand("delete from " + tableName);
            logsDbContext.SaveChanges();

            var businessData = businessDbContext.GetData<T>(tableName);
            if (businessData.None())
                return;

            businessData.ToList().ForEach(m => logsDbContext.AddData(ModelMapping.Copy(m)));
            logsDbContext.SaveChanges();

            Alert(string.Format("{0}-Server: Successfully copied data for '{1}' !", serverType, tableName));
        }

        private static void CopyCustomerRightsToLogsDb(string serverType)
        {
            var businessDbContext = CreateBusinessDbContext(serverType);
            var logsDbContext = CreateLogsDbContext(serverType);

            logsDbContext.Database.ExecuteSqlCommand("DELETE FROM CustomerRights");
            logsDbContext.SaveChanges();

            var businessData = businessDbContext.Database.SqlQuery<MpCustomerRights>("SELECT * FROM CustomerRights");
            if (businessData.None())
                return;

            businessData.ToList().ForEach(m => logsDbContext.Database.ExecuteSqlCommand("INSERT INTO CustomerRights (CustomerID,AppID) VALUES ({0},{1})", m.CustomerID, m.AppID));

            Alert(string.Format("{0}-Server: Successfully copied data for '{1}' !", serverType, "CustomerRights"));
        }

        #endregion


        #region Misc

        private static MultiDbPlatformContext CreateLogsDbContext(string serverType)
        {
            var logsConnectionString = string.Format("Logs{0}", serverType);
            var multiDbPlatformContext = new MultiDbPlatformContext(logsConnectionString);
            return multiDbPlatformContext;
        }

        private static MultiDbPlatformContext CreateBusinessDbContext(string serverType)
        {
            var businessConnectionString = string.Format("Source{0}", serverType);
            return new MultiDbPlatformContext(businessConnectionString);
        }

        private static void Alert(string info)
        {
            if (_infoMessageAction != null)
                _infoMessageAction(info);
        }

        #endregion
    }
}
