using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.CalendarService;
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
        ///    3.c)    Termine per Drag & Drop in den Standard Kalender verschieben
        /// </summary>
        public List<CalendarItem> GetAllCalendarItems(int year)
        {
            var list = new List<CalendarItem>();

            Application oApp = null;
            NameSpace mapiNamespace = null;
            MAPIFolder calendarFolder = null;
            Items outlookCalendarItems = null;

            oApp = new Application();
            mapiNamespace = oApp.GetNamespace("MAPI");

            calendarFolder = mapiNamespace.GetDefaultFolder(OlDefaultFolders.olFolderCalendar);

            outlookCalendarItems = calendarFolder.Items;
            outlookCalendarItems.IncludeRecurrences = false;

            var outlookCalendarItemsList = outlookCalendarItems.OfType<AppointmentItem>().OrderBy(i => i.Start).ToList();

            foreach (var item in outlookCalendarItemsList)
            {
                //if (item.IsRecurring) continue;
                if (item.Start.Year == year)
                {
                    var sItem = item.Subject + " -> " + item.Start.ToLongDateString();
                    var date = item.Start;

                    if (date < DateTime.Parse("09.06.2016"))
                        continue;

                    list.Add( new CalendarItem
                                     {
                                         ItemType = item.Subject[0].ToString().ToLower(),
                                         Date = item.Start,
                                     });
                }
            }

            oApp.Quit();

            return list;
        }
    }
}
