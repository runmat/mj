//#define __TEST
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
                        Dow Jones\r\n969420 22:32:29 Dow Jones 18.224,57 Pkt. +15,38\r\n+0,08% -\r\n80.483.121 18.244,38\r\n18.182,76";

            var text2 = @"Name Letzter Kurs* Aktuell Volumen Intraday-Spanne\r\n
                        DAX\r\n846900 12:57:30 Xetra 11.337,73 Pkt. +10,54\r\n+0,09% 10\r\n28.997.363 11.353,25\r\n11.301,34\r\n
                        Siemens\r\n723610 12:56:11 Xetra 99,56 EUR +0,63\r\n+0,64% 55\r\n609.071 99,69\r\n99,00\r\n
                        Euro / US Dollar (EUR/...\r\n13:12:30 Außerbörslich 1,1226 USD +0,0023\r\n+0,20% -\r\n- 1,1245\r\n1,1194\r\n
                        Euro / Schweizer Frank...\r\n13:12:28 HSBC 1,0645 CHF -0,0050\r\n-0,47% -\r\n- 1,0675\r\n1,0635\r\n
                        Goldpreis (Spot)\r\n13:12:29 Außerbörslich 1.208,58 USD -1,69\r\n-0,14% -\r\n- 1.212,50\r\n1.204,10\r\n
                        db Ölpreis Brent\r\n13:12:27 Deutsche_Bank 61,01 USD +0,37\r\n+0,61% -\r\n- 61,74\r\n60,82\r\n
                        National Bank of Greece\r\nA1WZMS 12:57:24 Frankfurt 1,38 EUR -0,11\r\n-7,18% 20.000\r\n611.731 1,49\r\n1,36\r\n
                        OSRAM Licht\r\nLED400 12:56:53 Xetra 40,72 EUR -0,96\r\n-2,29% 21\r\n125.108 41,08\r\n40,58\r\n
                        Dow_Jones\r\n969420 22:33:58 Dow_Jones 18.214,42 Pkt. -10,15\r\n-0,06% -\r\n81.500.575 18.239,43\r\n18.157,07";

            var text3 =
                "Name Letzter Kurs* Aktuell Volumen Intraday-Spanne\r\nDAX\r\n846900 12:57:30 Xetra 11.337,73 Pkt. +10,54\r\n+0,09% 10\r\n28.997.363 11.353,25\r\n11.301,34\r\nSiemens\r\n723610 12:56:11 Xetra 99,56 EUR +0,63\r\n+0,64% 55\r\n609.071 99,69\r\n99,00\r\nEuro / US Dollar (EUR/...\r\n13:12:30 Außerbörslich 1,1226 USD +0,0023\r\n+0,20% -\r\n- 1,1245\r\n1,1194\r\nEuro / Schweizer Frank...\r\n13:12:28 HSBC 1,0645 CHF -0,0050\r\n-0,47% -\r\n- 1,0675\r\n1,0635\r\nGoldpreis (Spot)\r\n13:12:29 Außerbörslich 1.208,58 USD -1,69\r\n-0,14% -\r\n- 1.212,50\r\n1.204,10\r\ndb Ölpreis Brent\r\n13:12:27 Deutsche_Bank 61,01 USD +0,37\r\n+0,61% -\r\n- 61,74\r\n60,82\r\nNational Bank of Greece\r\nA1WZMS 12:57:24 Frankfurt 1,38 EUR -0,11\r\n-7,18% 20.000\r\n611.731 1,49\r\n1,36\r\nOSRAM Licht\r\nLED400 12:56:53 Xetra 40,72 EUR -0,96\r\n-2,29% 21\r\n125.108 41,08\r\n40,58\r\nDow_Jones\r\n969420 22:33:58 Dow_Jones 18.214,42 Pkt. -10,15\r\n-0,06% -\r\n81.500.575 18.239,43\r\n18.157,07";

            //MessageBox.Show(text);

            StockService.ParseStocks(text3);

            Process.GetCurrentProcess().Kill();
        }
    }
}
