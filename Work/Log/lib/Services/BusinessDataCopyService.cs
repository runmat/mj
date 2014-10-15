﻿using System;
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
                    Alert(sqlMaintenanceTable.PrepareStatement(sqlMaintenanceStep.Description.Trim()));

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
        /// <returns>Ist eine Operation fehlgeschlagen?</returns>
        public static bool MaintenanceLogsDb(string serverType, DateTime pageVisitExpirydate, DateTime sapBapiExpiryDate, DateTime sapBapiDataExpiryDate)
        {
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

            var status = from command in commands
                         let result = ExecuteSqlCommand(CreateLogsDbContext(serverType), command, new object[0])
                         select result;

            return status.All(x => x);
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
