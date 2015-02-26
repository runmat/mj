using System.Drawing;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Keys = OpenQA.Selenium.Keys;
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchlistViewer
{
    public class FirefoxWebDriver
    {
        private const string FirefoxProcessName = "FireFox";

        static FirefoxDriver _driver;
        private static IntPtr _browserWindowIntPtr;

        public static void ShowBrowser()
        {
            WindowHelper.ShowWindow(WindowShowStyle.ShowNormal, _browserWindowIntPtr);
        }

        public static void HideBrowser()
        {
            WindowHelper.ShowWindow(WindowShowStyle.Hide, _browserWindowIntPtr);
        }

        public static void InvokeWatchlist()
        {
            ProcessHelper.KillAllProcessesOf(FirefoxProcessName);

            try
            {
                var ffBinary = new FirefoxBinary(@"c:\Program Files (x86)\Mozilla Firefox\firefox.exe");
                var firefoxProfile =
                    new FirefoxProfile(
                        @"C:\Users\JenzenM\AppData\Roaming\Mozilla\Firefox\Profiles\8c0l0x02.default-1366789569892");
                _driver = new FirefoxDriver(ffBinary, firefoxProfile)
                {
                    Url = "http://www.finanzen100.de/watchlist/uebersicht.html?NAME_DEPOT=Matz"
                };
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                var window = _driver.Manage().Window;

                var tbMail = _driver.FindElementById("MAIL_ADDRESS");
                tbMail.SendKeys("runningmatzi@web.de");

                var tbPwd = _driver.FindElementById("PASSWORD");
                tbPwd.SendKeys("Walter36");

                var submit = _driver.FindElementByName("LOGIN_FORM_SUBMIT");
                submit.Click();

                var width = 1000;
                var height = 600;

                var screenBounds = Screen.PrimaryScreen.Bounds;
                _driver.FindElement(By.TagName("body")).SendKeys(Keys.F11);
                window.Position = new Point
                {
                    X = (screenBounds.Width / 2 - width / 2),
                    Y = (screenBounds.Height / 2 - height / 2),
                };
                window.Size = new Size { Height = height, Width = width };
                _driver.ExecuteScript("document.getElementById('TABLE').scrollIntoView(true);");

                _browserWindowIntPtr = WindowHelper.ShowWindow(WindowShowStyle.Hide, FirefoxProcessName);
                var watchListTable = _driver.FindElementById("TABLE");
                //WindowHelper.ShowWindow(WindowShowStyle.ShowNormal, FirefoxProcessName);

                //driver.Close();
            }
            catch 
            {
            }
        }
    }
}
