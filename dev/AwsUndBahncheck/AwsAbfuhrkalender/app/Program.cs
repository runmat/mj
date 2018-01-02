using AwsAbfuhrkalender.Services;
using GoogleCalendar.Services;

namespace AwsAbfuhrkalenderApp
{
    class Program
    {
        private const int Year = 2018;

        /// <summary>
        /// Pre-Conditions:
        ///    1. AWS Seite aufrufen und dort Outlook "ICS" Datei exportieren
        ///    2. Outlook öffnen, ICS Datei öffnen => "Datei / Öffnen / Kalender öffnen (.ics)"
        ///    3. Nun die ICS Kalender Items kopieren auf den Standard Kalender:
        ///    3.a)    => "Ansicht / Ansicht ändern / Liste"
        ///    3.b)    ICS Kalender anwählen + alle Termine auswählen (STRG + a)
        ///    3.c)    Termine per Drag & Drop in den Standard Kalender verschieben
        /// </summary>
        static void Main(string[] args)
        {
            // 4. adapt "Year" variable
            // 5. uncomment the following line
            OutlookImport();
        }

        static void OutlookImport()
        {
            var itemList = new AwsOutlookImportService().GetAllCalendarItems(Year);
            var googleCalendarService = new GoogleCalendarService();
            googleCalendarService.ImportCalendarItems(itemList);
        }
    }
}
