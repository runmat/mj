using System;
using System.IO;
using System.Reflection;
using LogMaintenance.Services;

namespace LogMaintenance
{
    class Program
    {
        static string LogsDbInternalMaintenanceXmlFileName
        {
            get
            {
                var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (exePath == null) return "";
                return Path.Combine(exePath, "AppData", "LogsDbInternalMaintenance.xml");
            }
        }

        static void Main()
        {
            bool success;

            //success = BusinessDataCopyService.CopyToLogsDb(Console.WriteLine);
            //if (!success) Environment.Exit(-1);

            success = BusinessDataCopyService.MaintenanceLogsDb(Console.WriteLine, LogsDbInternalMaintenanceXmlFileName);
            if (!success) Environment.Exit(-1);
        }
    }
}
