using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace GeneralTools.Services
{
    public class AssemblyService
    {
        static public bool ApplicationCloneOfMeIsAlreadyRunning(Action<string> alertAction = null, string processNameForAlert = null)
        {
            var processFullName = Assembly.GetEntryAssembly().GetName().FullName.ToLower();
            var count = Process.GetProcesses().Count(p => processFullName.Contains(p.ProcessName.ToLower()));

            var cloneIsRunning = count > 1; // me plus at least 1 other clone of me

            var processName = Assembly.GetEntryAssembly().GetName().Name;
            if (cloneIsRunning && alertAction != null)
                 alertAction(string.Format("Die Anwendung {0} läuft bereits in einem anderen Fenster! Bitte verwenden Sie die andere Instanz!", processNameForAlert ?? processName));

            return cloneIsRunning;  
        }
    }
}
