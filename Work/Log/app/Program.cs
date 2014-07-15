using System;
using System.IO;
using System.Reflection;
using LogMaintenance.Services;

namespace LogMaintenance
{
    class Program
    {
        static void Main()
        {
            //BusinessDataCopyService.CopyToLogsDb(Console.WriteLine);

            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (exePath == null) return;
            var appDataFileName = Path.Combine(exePath, "AppData", "LogsDbInternalMaintenance.xml");
            BusinessDataCopyService.MaintenanceLogsDb(Console.WriteLine, appDataFileName);
        }
    }
}
