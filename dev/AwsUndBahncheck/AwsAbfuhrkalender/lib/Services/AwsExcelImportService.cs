using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AwsAbfuhrkalender.Contracts;
using Contracts.CalendarService;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace AwsAbfuhrkalender.Services
{
    public class AwsExcelImportService : IAwsExceImportService 
    {
        public Object OMissing = Type.Missing;

        public ICalendarService CalendarService { get; set; }

        public void Import(string excelFile, int year)
        {
            if (CalendarService == null)
            {
                System.Diagnostics.Trace.TraceError("Property 'CalendarService' should be set before calling method 'Import'");
                return;
            }
            if (!File.Exists(excelFile))
            {
                System.Diagnostics.Trace.TraceError("Method 'Import': The Excel-Filename '{0}' does not exist", excelFile);
                return;
            }


            //
            // delete old events first:
            //

            //CalendarService.DeleteAllEvents();

            //
            // now generate the new events:
            //

            CultureInfo ciOrg = null;
            var excelApp = new Application {Visible = false, DisplayAlerts = false};
            Workbook workbook = null;
            try
            {
                OpenWorkbook(excelApp, ref ciOrg, ref workbook, excelFile);
            }
            catch (Exception)
            {
                System.Diagnostics.Trace.TraceError("Method 'Import': Problem while opening Excel-Filename '{0}'", excelFile);
                excelApp.Quit();
                return;
            }
            if (workbook == null)
            {
                System.Diagnostics.Trace.TraceError("Method 'Import': Excel-Filename seems not to be a valid excel file'{0}'", excelFile);
                excelApp.Quit();
                return;
            }

            var sheet = (Worksheet)(workbook.ActiveSheet);
            for (var m = 1; m <= 12; m++)
            {
                if (ImportMonth(sheet, year, m)) continue;
                excelApp.Quit();
                return;
            }

            excelApp.Quit();
        }

        bool ImportMonth(Worksheet sheet, int year, int month)
        {
            var dayColumn = ((month-1)*2)+1;
            var actionColumn = dayColumn+1;
            const int startRow = 3;

            for (var row = startRow; row <= startRow+31; row++)
            {
                var day = getCellValue(sheet, row, dayColumn);
                if (day == "") continue;
                //day = day.Substring(0, 2).Trim();
                int iDay;
                if (!Int32.TryParse(day, out iDay)) continue;

                var actions = getCellValue(sheet, row, actionColumn);
                if (actions == "") continue;

                ImportDayActions(year, month, iDay, actions);
            }

            return true;
        }

        void ImportDayActions(int year, int month, int day, string actions)
        {
            actions.Split(',').ToList().ForEach(a => ImportDayAction(year, month, day, a));
        }

        void ImportDayAction(int year, int month, int day, string action)
        {
            var actionFullNames = new Dictionary<string, string>
                                      {
                                          {"r", "Müll"},
                                          {"g", "Gelber Sack"},
                                          {"b", "Bio-Tonne"},
                                          {"p", "Papier-Tonne"},
                                          {"w", "Weihnachtsbaum-Abfuhr"},
                                      };
            var actionFullName = action;
            if (!actionFullNames.TryGetValue(action, out actionFullName)) return;

            //System.Diagnostics.Trace.TraceError("{0} {1} {2} {3}", year, month, day, actionFullName);

            const int eventStartHour = 9;
            const int eventDurationHours = 1;
            const int eventReminderHours = 13;
            var eventStartTime = new DateTime(year, month, day, eventStartHour, 0, 0);
            CalendarService.CreateEvent(actionFullName, eventStartTime, eventDurationHours, eventReminderHours);
        }

        private string getCellValue(Worksheet sheet, int row, int column)
        {
            string cellValue = ((Range)sheet.Cells[row, column]).Formula.ToString();
            if (string.IsNullOrEmpty(cellValue))
                return "";

            return cellValue;
        }

        private void OpenWorkbook(Excel.Application e, ref CultureInfo ciOrg, ref Excel.Workbook wb, string excelFilename)
        {
            try
            {
                wb = e.Workbooks.Open(excelFilename,
                    0, false, 0, "", "", false, Excel.XlPlatform.xlWindows, "",
                    true, false, 0, false, false, false);
            }
            catch
            {
                // if UI system culture is german but office version is english
                // ==> we get this exception here:
                // we can resolve this problem by temporarily switching the culture to "english"
                CultureInfo ci = new CultureInfo("en-US");

                ciOrg = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                wb = e.Workbooks.Open(excelFilename,
                    0, false, OMissing, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, false, false, false);
            }
        }
    }
}
