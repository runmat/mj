using System;
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
            return new MultiDbPlatformContext(logsConnectionString);
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
