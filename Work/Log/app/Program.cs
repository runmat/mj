using System;
using System.Configuration;
using System.IO;
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

        static void Main()
        {
            var success = false;

            success = BusinessDataCopyService.CopyToLogsDb(Console.WriteLine);
            if (!success) Environment.Exit(-1);

            success = BusinessDataCopyService.MaintenanceLogsDb(Console.WriteLine, LogsDbInternalMaintenanceXmlPath);
            if (!success) Environment.Exit(-1);

            var now = DateTime.Now.Date;

            var pageVisitExpiryMonthsAgo = int.Parse(ConfigurationManager.AppSettings["PageVisitMonthsOldToDelete"]) ;
            var pageVisitExpiryDate = now.AddMonths(pageVisitExpiryMonthsAgo);

            var sapBapiExpiryMonthsAgo = int.Parse(ConfigurationManager.AppSettings["SapBapiMonthsOldToDelete"]);
            var sapBapiExpiryDate = now.AddMonths(sapBapiExpiryMonthsAgo);

            var bapiDataExpiryMonthsAgo = int.Parse(ConfigurationManager.AppSettings["SapBapiMonthsOldToClearData"]);
            var bapiDataExpiryDate = now.AddMonths(bapiDataExpiryMonthsAgo);

            var elmahExpiryMonthsAgo = int.Parse(ConfigurationManager.AppSettings["ElmahMonthsOldToClearData"]);
            var elmahExpiryDate = now.AddMonths(elmahExpiryMonthsAgo);

            success = BusinessDataCopyService.DeleteExpiredLogMessages("Prod", pageVisitExpiryDate, sapBapiExpiryDate, bapiDataExpiryDate, elmahExpiryDate);
            if (!success) Environment.Exit(-1);
        }
    }
}
