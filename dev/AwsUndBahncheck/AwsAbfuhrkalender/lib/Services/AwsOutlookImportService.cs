using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Contracts.CalendarService;
using DocumentTools.Services;
using Microsoft.Office.Interop.Outlook;

namespace AwsAbfuhrkalender.Services
{
    public class AwsOutlookImportService
    {
        /// <summary>
        /// Pre-Conditions:
        ///    1. AWS Seite aufrufen und dort Outlook "ICS" Datei exportieren
        ///    2. Outlook öffnen, ICS Datei öffnen => "Datei / Öffnen / Kalender öffnen (.ics)"
        ///    3. Nun die ICS Kalender Items kopieren auf den Standard Kalender:
        ///    3.a)    => "Ansicht / Ansicht ändern / Liste"
        ///    3.b)    ICS Kalender anwählen + alle Termine auswählen (STRG + a)
        ///           OBSOLETE WEGEN Com.Error CO_E_SERVER_EXEC_FAILURE  3.c)    Termine per Drag & Drop in den Standard Kalender verschieben
        ///    3.c)    Nach Excel kopieren und dort speichern, 2 Spalten "Beginn", "Betreff", Kopfzeile beibehalten 
        /// </summary>
        public List<CalendarItem> GetAllCalendarItems(int year)
        {
            var list = new List<CalendarItem>();

            var dt = ExcelDocumentFactory.ReadToDataTable(@"C:\Users\JenzenM\Downloads\Aws_Termine_2017.xls", true);
            var rows = dt.Rows;

            foreach (DataRow item in rows)
            {
                var datum = DateTime.Parse((string)item[0]);
                var subject = (string)item[1];

                //if (item.IsRecurring) continue;
                if (datum.Year == year)
                {
                    var sItem = subject + " -> " + datum.ToLongDateString();
                    var date = datum;

                    if (date < DateTime.Parse("01.01.2017"))
                        continue;

                    list.Add(new CalendarItem
                    {
                        ItemType = subject[0].ToString().ToLower(),
                        Date = datum,
                    });
                }
            }

            return list;
        }
    }
}
