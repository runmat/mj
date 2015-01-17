using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using BahnCheckDatabase.Models;
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
                using (var ct = new RbEntities())
                {
                    //MessageBox.Show("4");
                    if (!ct.OpenRbRequestAvailable) return;
                    //MessageBox.Show("5");

                    var openRequest = ct.GetOpenRbRequest("", 0);
                    // Offener Request überhaupt verfügbar? Falls nein, raus hier!
                    if (openRequest == null) return;


                    // Dem Dispatcher mitteilen, dass wir diesen Request nun bearbeiten
                    openRequest.ProcessingStartDate = DateTime.Now;
                    ct.SaveChanges();


                    // ...
                    // ... start doing the job here
                    // ...

                    openRequest.Abfahrt = null;
                    openRequest.Bahnhof = "-";
                    openRequest.Zug = "-";

                    var rbStammList = ct.RbStamm.ToList();
                    if (string.IsNullOrEmpty(openRequest.WantedZug))
                        if (!rbStammList.Any(r => NaechsterZug(r, null)))
                        {
                            // kein relevanter Zug mehr verfügbar heute!
                            openRequest.Auskunft = "(kein relevanter Zug mehr verfügbar heute)";
                            openRequest.UpdDate = DateTime.Now;
                            ct.SaveChanges();
                            return;
                        }

                    // den zeitlich nächsten Zug holen:
                    RbStamm rbWanted;
                    if (!string.IsNullOrEmpty(openRequest.WantedZug))
                        rbWanted = rbStammList.FirstOrDefault(r => r.Zugnummer.Equals(openRequest.WantedZug));
                    else
                        rbWanted = rbStammList.FirstOrDefault(r => r.Abfahrt == rbStammList.Where(r3 => NaechsterZug(r3, null)).Min(r2 => r2.Abfahrt));

                    // --- ein Zug später?
                    if (openRequest.WantedZugOfs == 1)
                        rbWanted = rbStammList.FirstOrDefault(r => r.Abfahrt == rbStammList.Where(r3 => NaechsterZug(r3, rbWanted)).Min(r2 => r2.Abfahrt));

                    // --- ein Zug früher?
                    if (openRequest.WantedZugOfs == -1)
                        rbWanted = rbStammList.FirstOrDefault(r => r.Abfahrt == rbStammList.Where(r3 => VorherigerZug(r3, rbWanted)).Max(r2 => r2.Abfahrt));

                    if (rbWanted == null)
                    {
                        // kein relevanter Zug mehr verfügbar heute!
                        openRequest.Auskunft = "FEHLER - bei der Datenbank Abfrage des zeitlich nächsten Zuges!";
                        openRequest.UpdDate = DateTime.Now;
                        ct.SaveChanges();
                        return;
                    }

                    openRequest.Abfahrt = DateTime.Today + rbWanted.Abfahrt;
                    openRequest.Bahnhof = rbWanted.Bahnhof;
                    openRequest.Zug = rbWanted.Zugnummer;

                    HttpQueryBahn(ct, openRequest);
                }
            }
            catch (Exception) { }

            //new BahnCheckForm().ShowDialog();
        }

        static bool NaechsterZug(RbStamm r, RbStamm rNachDiesemZug)
        {
            return (WochenTagMatches(r) && r.Abfahrt >= DateTime.Now.TimeOfDay && (rNachDiesemZug == null || r.Abfahrt > rNachDiesemZug.Abfahrt));
        }

        static bool VorherigerZug(RbStamm r, RbStamm rVorDiesemZug)
        {
            return (WochenTagMatches(r) && r.Abfahrt < rVorDiesemZug.Abfahrt);
        }

        private static bool WochenTagMatches(RbStamm r)
        {
            var wochenTagMatches = true;
            var wochenEnde = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(DateTime.Now.DayOfWeek);
            if (wochenEnde)
                wochenTagMatches = r.WochenendeAuch;

            return wochenTagMatches;
        }

        static void HttpQueryBahn(DbContext ct, RbRequest openRequest)
        {
            openRequest.Auskunft = "... Firefox Web Remote Control für die HTTP Anfrage in Arbeit ...";
            ct.SaveChanges();

            KillAllProcessesOf("FireFox");

            string html;
            FirefoxDriver driver;
            try
            {
                // ...
                // ... starting firefox remote control
                // ...
                var ffBinary = new FirefoxBinary(@"c:\Program Files (x86)\Mozilla Firefox\firefox.exe");
                var firefoxProfile =
                    new FirefoxProfile(
                        @"C:\Users\Konsole\AppData\Roaming\Mozilla\Firefox\Profiles\j82xktbf.default");
                driver = new FirefoxDriver(ffBinary, firefoxProfile) { Url = "http://reiseauskunft.bahn.de/bin/bhftafel.exe/dn?" };
            }
            catch (Exception)
            {
                openRequest.Auskunft = "FEHLER - beim Firefox Web Remote Control für die HTTP Anfrage!";
                ct.SaveChanges();
                return;
            }

            try
            {
                var window = driver.Manage().Window;
                window.Position = new Point { X = 0, Y = 0 };
                window.Size = new Size { Height = 800, Width = 1100 };

                var tb = driver.FindElementById("rplc0");
                tb.SendKeys(openRequest.Bahnhof);

                tb = driver.FindElementById("HFS_trainName");
                tb.SendKeys(openRequest.Zug);

                var submit = driver.FindElementByName("start");
                submit.Click();

                html = driver.PageSource;

                driver.Close();
                // ...
                // ... end firefox remote control
                // ...
            }
            catch (Exception)
            {
                driver.Close();

                openRequest.Auskunft = "FEHLER - beim Remote Steuern der Bahn Website!";
                ct.SaveChanges();
                return;
            }

            openRequest.Auskunft = ParseAuskunftFromHtml(html);

            //if (!openRequest.Auskunft.ToUpper().Contains("FEHLER"))
            openRequest.UpdDate = DateTime.Now;

            ct.SaveChanges();
        }

        static string ParseAuskunftFromHtml(string html)
        {
            html = html.Replace("\r", "").Replace("\n", "");

            //MessageBox.Show(html);
            const string patternStart = "<td class=\"ris\">";
            const string patternEnd = "</td>";
            const string pattern = patternStart + "(.*?)" + patternEnd;
            var matches = Regex.Matches(html, pattern, RegexOptions.Multiline).OfType<Match>().ToList();
            //MessageBox.Show(matches.Count().ToString());
            // nur die nicht leeren <td> ( <==> m.Length > 30)
            var match = matches.FirstOrDefault(m => m.Length > 30);
            if (match == null && matches.Count > 0)
                return "ok";

            if (match != null)
            {
                var tdMatch = match.ToString();
                tdMatch = tdMatch.Replace(patternStart, "").Replace(patternEnd, "");

                var innerHtml = tdMatch;
                if (tdMatch.ToLower().Contains("<span"))
                {
                    var web = new HtmlAgilityPack.HtmlDocument();
                    web.LoadHtml(tdMatch);

                    var list = new List<string>();
                    web.DocumentNode.SelectNodes("//span").Where(n => !string.IsNullOrEmpty(n.InnerText)).ToList().
                        ForEach(n => list.Add(n.InnerText));
                    list = list.Distinct().ToList();
                    innerHtml = "";
                    list.ForEach(n => innerHtml += string.Format(" ; {0}", n));
                    innerHtml = innerHtml.Replace("&#228;", "ä");
                }
                if (innerHtml.Length > 3)
                    innerHtml = innerHtml.Substring(3);

                return innerHtml;
            }

            return "FEHLER - beim Parsen der HTML Auskunft!";
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
