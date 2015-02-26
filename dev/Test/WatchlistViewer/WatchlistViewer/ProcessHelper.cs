using System.Diagnostics;
using System.Linq;

namespace WatchlistViewer
{
    public class ProcessHelper
    {
        public static void KillAllProcessesOf(string processName)
        {
            var otherBahnCheckerProcess = Process.GetProcessesByName(processName).Where(p => p.Id != Process.GetCurrentProcess().Id).ToList();
            if (otherBahnCheckerProcess.Any())
                otherBahnCheckerProcess.ForEach(p => p.Kill());
        }
    }
}
