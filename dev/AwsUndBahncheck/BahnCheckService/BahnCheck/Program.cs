using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using OpenQA.Selenium.Firefox;

namespace BahnCheck
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            KillOtherProcessesOfMe();

            try
            {
                HttpQueryBahn();
            }
            catch (Exception) { }
        }

        static void HttpQueryBahn()
        {
            KillAllProcessesOf("FireFox");

            FirefoxDriver driver;
            try
            {
                // ...
                // ... starting firefox remote control
                // ...
                var ffBinary = new FirefoxBinary(@"c:\Program Files (x86)\Mozilla Firefox\firefox.exe");
                var firefoxProfile =
                    new FirefoxProfile(
                        @"C:\Users\JenzenM\AppData\Roaming\Mozilla\Firefox\Profiles\8c0l0x02.default-1366789569892");
                driver = new FirefoxDriver(ffBinary, firefoxProfile)
                                 {Url = "http://reiseauskunft.bahn.de/bin/bhftafel.exe/dn?"};
            }
            catch(Exception)
            {
                return;
            }

            try
            {
                var window = driver.Manage().Window;
                //window.Position = new Point {X = 0, Y = 0};
                //window.Size = new Size {Height = 800, Width = 1100};

                var tb = driver.FindElementById("rplc0");
                tb.SendKeys("Ahrensburg");

                //tb = driver.FindElementById("HFS_trainName");
                //tb.SendKeys(openRequest.Zug);

                var submit = driver.FindElementByName("start");
                submit.Click();

                //driver.Close();
                // ...
                // ... end firefox remote control
                // ...
            }
            catch(Exception)
            {
                //driver.Close();
            }
        }

        static void KillOtherProcessesOfMe()
        {
            KillAllProcessesOf("BahnCheck");
        }

        static void KillAllProcessesOf(string processName)
        {
            var otherBahnCheckerProcess = Process.GetProcessesByName(processName).Where(p => p.Id != Process.GetCurrentProcess().Id).ToList();
            if (otherBahnCheckerProcess.Any())
                otherBahnCheckerProcess.ForEach(p => p.Kill());
        }
    }
}
