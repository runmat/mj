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
using SimpleBrowser.WebDriver;
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

        public static List<Stock> GetStockData()
        {
#if TEST
                var stockData = "Name Letzter Kurs* Aktuell Volumen Intraday-Spanne\r\nDAX\r\n846900 12:57:30 Xetra 11.337,73 Pkt. +10,54\r\n+0,09% 10\r\n28.997.363 11.353,25\r\n11.301,34\r\nSiemens\r\n723610 12:56:11 Xetra 99,56 EUR +0,63\r\n+0,64% 55\r\n609.071 99,69\r\n99,00\r\nEuro / US Dollar (EUR/...\r\n13:12:30 Außerbörslich 1,1226 USD +0,0023\r\n+0,20% -\r\n- 1,1245\r\n1,1194\r\nEuro / Schweizer Frank...\r\n13:12:28 HSBC 1,0645 CHF -0,0050\r\n-0,47% -\r\n- 1,0675\r\n1,0635\r\nGoldpreis (Spot)\r\n13:12:29 Außerbörslich 1.208,58 USD -1,69\r\n-0,14% -\r\n- 1.212,50\r\n1.204,10\r\ndb Ölpreis Brent\r\n13:12:27 Deutsche_Bank 61,01 USD +0,37\r\n+0,61% -\r\n- 61,74\r\n60,82\r\nNational Bank of Greece\r\nA1WZMS 12:57:24 Frankfurt 1,38 EUR -0,11\r\n-7,18% 20.000\r\n611.731 1,49\r\n1,36\r\nOSRAM Licht\r\nLED400 12:56:53 Xetra 40,72 EUR -0,96\r\n-2,29% 21\r\n125.108 41,08\r\n40,58\r\nDow_Jones\r\n969420 22:33:58 Dow_Jones 18.214,42 Pkt. -10,15\r\n-0,06% -\r\n81.500.575 18.239,43\r\n18.157,07";
#else
                var stockData = _driver.FindElementById("TABLE").Text;
#endif
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
