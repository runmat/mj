using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using LogMaintenance.Services;

namespace LogMaintenance
{
    class Program
    {
        static string LogsDbInternalMaintenanceXmlPath
        {
            get
            {
                var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (exePath == null) return "";
                return Path.Combine(exePath, "AppData", "LogsDbInternalMaintenance");
            }
        }

        private const string PageVisitMonthsOldToDeleteDefault = "PageVisitMonthsOldToDelete";
        private const string SapBapiMonthsOldToDeleteDefault = "SapBapiMonthsOldToDelete";
        private const string SapBapiMonthsOldToClearDataDefault = "SapBapiMonthsOldToClearData";
        private const string ElmahMonthsOldToClearDataDefault = "ElmahMonthsOldToClearData";

        static void Main()
        {
            var success = false;

            success = BusinessDataCopyService.CopyToLogsDb(Console.WriteLine);
            if (!success) Environment.Exit(-1);

            success = BusinessDataCopyService.MaintenanceLogsDb(Console.WriteLine, LogsDbInternalMaintenanceXmlPath);
            if (!success) Environment.Exit(-1);

            // Liste der DB Suffixe die wir bearbeiten sollen
            string[] logDbsNameSuffix = new[]
                {
                    "Prod",
                    "Test",
                    "Dev",
                    "CkgTest",
                    "CkgProd",
                    "CkgDev"
                };

            var cleanUpAction = from db in logDbsNameSuffix
                    let result = CleanUpLogMessages(db)
                    select result;

            success = cleanUpAction.All(x => x);

            if (!success) Environment.Exit(-1);

            var optimizeAction = from db in logDbsNameSuffix
                                 let result = OptimizeTables(db)
                                 select result;

            success = optimizeAction.All(x => x);

            if (!success) Environment.Exit(-1);
            

            var pathToElmahViewer = ConfigurationManager.AppSettings["ElmahViewerInstallationFolder"];

            success = BusinessDataCopyService.CreateElmahShortcutLandingpage("Prod", pathToElmahViewer);
            if (!success) Environment.Exit(-1);
        }

        private static bool OptimizeTables(string dbnamesuffix)
        {
            var success = BusinessDataCopyService.OptimizeTables(dbnamesuffix);
            return success;
        }

        private static bool CleanUpLogMessages(string dbnamesuffix)
        {
            var now = DateTime.Now;

            var pageVisitMonthsOldSettingName = string.Concat(dbnamesuffix, PageVisitMonthsOldToDeleteDefault);
            var sapBapiMonthsOldToDeleteSettingName = string.Concat(dbnamesuffix, SapBapiMonthsOldToDeleteDefault);
            var sapBapiMonthsOldToClearDataSettingName = string.Concat(dbnamesuffix, SapBapiMonthsOldToClearDataDefault);
            var elmahMonthsOldToClearDataSettingName = string.Concat(dbnamesuffix, ElmahMonthsOldToClearDataDefault);

            var pageVisitExpiryMonthsAgo =
                ConfigurationManager.AppSettings.AllKeys.Contains(pageVisitMonthsOldSettingName)
                    ? int.Parse(ConfigurationManager.AppSettings[pageVisitMonthsOldSettingName])
                    : int.Parse(ConfigurationManager.AppSettings[PageVisitMonthsOldToDeleteDefault]);

            var pageVisitExpiryDate = now.AddMonths(pageVisitExpiryMonthsAgo);

            var sapBapiMonthsAgoToDelete =
                ConfigurationManager.AppSettings.AllKeys.Contains(sapBapiMonthsOldToDeleteSettingName)
                    ? int.Parse(ConfigurationManager.AppSettings[sapBapiMonthsOldToDeleteSettingName])
                    : int.Parse(ConfigurationManager.AppSettings[SapBapiMonthsOldToDeleteDefault]);

            var sapBapiMonthsOldToDeleteDate = now.AddMonths(sapBapiMonthsAgoToDelete);


            var sapBapiMonthsAgoToClearData =
                ConfigurationManager.AppSettings.AllKeys.Contains(sapBapiMonthsOldToClearDataSettingName)
                    ? int.Parse(ConfigurationManager.AppSettings[sapBapiMonthsOldToClearDataSettingName])
                    : int.Parse(ConfigurationManager.AppSettings[SapBapiMonthsOldToClearDataDefault]);

            var bapiDataExpiryDate = now.AddMonths(sapBapiMonthsAgoToClearData);

            var elmahMontsAgoToDelete =
                ConfigurationManager.AppSettings.AllKeys.Contains(elmahMonthsOldToClearDataSettingName)
                    ? int.Parse(ConfigurationManager.AppSettings[elmahMonthsOldToClearDataSettingName])
                    : int.Parse(ElmahMonthsOldToClearDataDefault);

            var elmahExpiryDate = now.AddMonths(elmahMontsAgoToDelete);

            var success = BusinessDataCopyService.DeleteExpiredLogMessages(dbnamesuffix, pageVisitExpiryDate, sapBapiMonthsOldToDeleteDate,
                                                                            bapiDataExpiryDate, elmahExpiryDate);
            return success;
        }
    }
}
