// ReSharper disable RedundantUsingDirective
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Keys = OpenQA.Selenium.Keys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using Selenium.WebDriver.Extensions.JQuery;
using By = OpenQA.Selenium.By;

namespace WatchlistViewer
{
    public class FirefoxWebDriver
    {
        public const int BrowserMarginRight = 130;
        public const int BrowserWidth = 560;
        private const string FirefoxProcessName = "FireFox";

        static FirefoxDriver _driver;
        private static IntPtr _browserWindowIntPtr;
        private static IWebElement _daxDiv;

        public static bool IsBrowserVisible { get; set; }

        public static void GetStockDataTest()
        {
            var stockData = "Name Kurs Aktuell 52W\r\ndb DAX\r\n11:24:25\r\nDeutsche Bank\r\n10.681,50\r\nPkt.\r\n+0,06%\r\n+6,00\r\n-3,46%\r\n-383,00\r\nGoldpreis (Spot)\r\n11:24:31\r\nIDC Forex\r\n1.187,49\r\nUSD\r\n-0,16%\r\n-1,91\r\n+10,43%\r\n+112,14\r\nEuro / US Dollar (EUR/USD)\r\n11:24:31\r\nIDC Forex\r\n1,0552\r\nUSD\r\n+0,10%\r\n+0,0010\r\n-0,79%\r\n-0,0084\r\nSiemens\r\n723610\r\n11:08:39\r\nXetra\r\n107,45\r\nEUR\r\n+0,00%\r\n+0,00\r\n+12,14%\r\n+11,63\r\ndb Ölpreis Brent\r\n11:24:31\r\nDeutsche Bank\r\n49,07\r\nUSD\r\n+0,03%\r\n+0,02\r\n+8,70%\r\n+3,93\r\ndb S&P 500\r\n11:24:30\r\nDeutsche Bank\r\n2.203,75\r\nPkt.\r\n+0,01%\r\n+0,25\r\n+5,62%\r\n+117,25\r\ndb Dow Jones\r\n11:24:12\r\nDeutsche Bank\r\n19.094,00\r\nPkt.\r\n+0,03%\r\n+6,00\r\n+7,33%\r\n+1.304,00\r\nEuro / Schweizer Franken (EUR/CHF)\r\n11:24:31\r\nHSBC\r\n1,0730\r\nCHF\r\n+0,00%\r\n+0,0000\r\n-0,88%\r\n-0,0095\r\nVolkswagen Vz.\r\n766403\r\n11:09:24\r\nXetra\r\n124,12\r\nEUR\r\n+0,91%\r\n+1,12\r\n+12,94%\r\n+14,22\r\nOSRAM Licht\r\nLED400\r\n11:08:35\r\nXetra\r\n47,52\r\nEUR\r\n+0,04%\r\n+0,02\r\n+22,14%\r\n+8,62\r\nNational Bank of Greece\r\nA2ABB9\r\n11:05:29\r\nFrankfurt\r\n0,22\r\nEUR\r\n-5,68%\r\n-0,01\r\n-93,51%\r\n-3,11\r\nCall auf Goldpreis (Spot)\r\nDZV90N\r\n10:53:27\r\nStuttgart\r\n0,001\r\nEUR\r\n+0,00%\r\n+0,000\r\n-99,88%\r\n-0,839\r\nEuro-Bund-Future (FGBL) - EUX/C1\r\n11:09:31\r\nEurex\r\n161,22\r\nEUR\r\n+0,04%\r\n+0,07\r\n+2,47%\r\n+3,88";
            var parsedStocks = StockService.ParseStocks(stockData);
        }

        public static List<Stock> GetStockData()
        {
            string stockData;
            try
            {
                stockData = _driver.FindElementByClassName("PortfolioTable").Text;
            }
            catch
            {
                // ignored
                return null;
            }

            var parsedStocks = StockService.ParseStocks(stockData);

            return parsedStocks;
        }

        public static void InvokeDax()
        {
            ProcessHelper.KillAllProcessesOf(FirefoxProcessName);

            try
            {
                var ffBinary = new FirefoxBinary(@"c:\Program Files (x86)\Mozilla Firefox\firefox.exe");
                var firefoxProfile =
                    new FirefoxProfile(
                        @"C:\Users\JenzenM\AppData\Roaming\Mozilla\Firefox\Profiles\q9bqzwdx.default");
                _driver = new FirefoxDriver(ffBinary, firefoxProfile);
                
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                _browserWindowIntPtr = WindowHelper.ShowWindow(WindowShowStyle.Hide, FirefoxProcessName);

                _driver.Url = "http://www.finanzen.net/aktien/DAX-Realtimekurse";

                _daxDiv = _driver.FindElement(new JQuerySelector("table.header_height"));
            }
            catch
            {
                // ignored
            }
        }

        public static string GetDaxValue()
        {
            return _daxDiv.Text;
        }

        public static void InvokeWatchlist()
        {
            ProcessHelper.KillAllProcessesOf(FirefoxProcessName);

            try
            {
                var ffBinary = new FirefoxBinary(@"c:\Program Files (x86)\Mozilla Firefox\firefox.exe");
                var firefoxProfile =
                    new FirefoxProfile(
                        @"C:\Users\JenzenM\AppData\Roaming\Mozilla\Firefox\Profiles\15szcw7c.default");
                _driver = new FirefoxDriver(ffBinary, firefoxProfile);
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                var window = _driver.Manage().Window;

                const int width = 600;
                const int height = 1300;

                var screenBounds = Screen.PrimaryScreen.Bounds;
                _driver.FindElement(By.TagName("body")).SendKeys(Keys.F11);
                //Thread.Sleep(1000); 
                window.Position = new Point
                {
                    X = (screenBounds.Width / 2 - width / 2),
                    Y = -175, //(screenBounds.Height / 2 - height / 2),
                };
                window.Size = new Size { Height = height, Width = width };

                _driver.Url = "https://www.finanzen100.de/watchlist/Matz/";

                try
                { 
                    var tbMail = _driver.FindElementById("email");
                    tbMail.SendKeys("runningmatzi@web.de");
                    var tbPwd = _driver.FindElementById("password");
                    tbPwd.SendKeys("Walter36");
                    var submit = _driver.FindElementByClassName("cta-button--primary");
                    if (submit!= null && submit.Text.ToLower() == "anmelden")
                        submit.Click();
                }
                catch
                {
                    // ignored
                }
                var html = _driver.FindElement(By.TagName("html"));
                new Actions(_driver)
                    .SendKeys(html, Keys.LeftControl + Keys.Subtract + Keys.Null)
                    .Perform();

                HideScrollbar();

                _browserWindowIntPtr = WindowHelper.ShowWindow(WindowShowStyle.Hide, FirefoxProcessName);
                IsBrowserVisible = false;
            }
            catch
            {
                // ignored
            }
        }

        static void HideScrollbar()
        {
            _driver.ExecuteScript("document.body.style.overflow = 'hidden';");
            _driver.ExecuteScript("document.getElementById('scroll-top').style.overflow = 'hidden';");
        }

        public static void ShowBrowser()
        {
            WindowHelper.ShowWindow(WindowShowStyle.ShowNormal, _browserWindowIntPtr);
            IsBrowserVisible = true;
        }

        public static void HideBrowser()
        {
            WindowHelper.ShowWindow(WindowShowStyle.Hide, _browserWindowIntPtr);
            IsBrowserVisible = false;
        }

        public static void RefreshBrowser()
        {
            _driver.Navigate().Refresh();
            HideScrollbar();
        }
    }
}
