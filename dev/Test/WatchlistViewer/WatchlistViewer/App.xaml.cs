#define __TEST
using System.Diagnostics;
using System.Windows;

namespace WatchlistViewer
{
    public partial class App 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
#if __TEST
            Test();
#else
            ProcessHelper.KillAllProcessesOf("WatchlistViewer");
#endif

            base.OnStartup(e);
        }

        static void Test()
        {
            var text = @"Name Letzter Kurs* Aktuell Volumen Intraday-Spanne\r\n
                        DAX\r\n846900 13:12:39 Xetra 11.246,27 Pkt. +36,00\r\n+0,32% 4.178\r\n35.748.707 11.256,73\r\n11.187,00\r\n
                        Siemens\r\n723610 13:12:34 Xetra 98,43 EUR -0,35\r\n-0,35% 737\r\n601.623 98,95\r\n98,30\r\n
                        Euro / US Dollar (EUR/...\r\n13:27:39 Außerbörslich 1,1339 USD -0,0025\r\n-0,22% -\r\n- 1,1380\r\n1,1333\r\n
                        Euro / Schweizer Frank...\r\n13:27:39 HSBC 1,0765 CHF -0,0030\r\n-0,28% -\r\n- 1,0785\r\n1,0735\r\n
                        Goldpreis (Spot)\r\n13:27:39 Außerbörslich 1.217,62 USD +12,15\r\n+1,01% -\r\n- 1.220,20\r\n1.203,45\r\n
                        db Ölpreis Brent\r\n13:27:28 Deutsche Bank 61,92 USD -0,04\r\n-0,06% -\r\n- 62,61\r\n61,36\r\n
                        National Bank of Greece\r\nA1WZMS 13:10:47 Frankfurt 1,48 EUR -0,01\r\n-0,54% 20.000\r\n578.737 1,49\r\n1,30\r\n
                        OSRAM Licht\r\nLED400 13:12:27 Xetra 41,76 EUR +0,61\r\n+1,48% 64\r\n96.662 41,92\r\n41,03\r\n
                        Dow Jones\r\n969420 22:32:29 Dow Jones 18.224,57 Pkt. +15,38\r\n+0,08% -\r\n80.483.121 18.244,38\r\n18.182,76
                        ";

            //MessageBox.Show(text);

            StockService.ParseStocks(text);

            Process.GetCurrentProcess().Kill();
        }
    }
}
