using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using BahnCheckDatabase.Models;

namespace BahnCheckDispatcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            KillOtherProcessesOfMe();

            while (true)
            {
                try
                {
                    using (var ct = new RbEntities())
                    {
                        if (ct.OpenRbRequestProcessingExpired)
                        {
                            var remoteExePath =
                                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                             @"..\..\..\BahnCheck\bin\debug\BahnCheck.exe");
                            var p = Process.Start(remoteExePath);
                            if (p != null) p.PriorityClass = ProcessPriorityClass.RealTime;
                            Thread.Sleep(15000);
                        }
                    }
                }
                catch (Exception)
                {
                }

                Thread.Sleep(500);
            }
        }

        static void KillOtherProcessesOfMe()
        {
            var otherBahnCheckerProcess = Process.GetProcessesByName("BahnCheckDispatcher").Where(p => p.Id != Process.GetCurrentProcess().Id).ToList();
            if (otherBahnCheckerProcess.Any())
                otherBahnCheckerProcess.ForEach(p => p.Kill());
        }
    }
}
