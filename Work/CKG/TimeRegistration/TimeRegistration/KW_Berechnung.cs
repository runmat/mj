using System;
using System.Globalization;

namespace BusinessDateFunctions
{
	public static class KW_Berechnung
	{
		public static int KwOfDate(DateTime Datum)
		{
		    int kw = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(Datum, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // Nur Jahre, deren erster Tag ein Donnerstag (oder Mittwoch bei Schaltjahren) ist, haben 53 Wochen
            if ((Datum.DayOfYear > 300) && (kw == 53))
            {
                DateTime ersterTagDesJahres = new DateTime(Datum.Year, 1, 1);
                DayOfWeek ersterWochentagDesJahres = ersterTagDesJahres.DayOfWeek;
                if ((ersterWochentagDesJahres != DayOfWeek.Thursday) || ((DateTime.IsLeapYear(Datum.Year)) && (ersterWochentagDesJahres != DayOfWeek.Wednesday)))
                {
                    kw = 1;
                }
            }

		    return kw;
		}

	}
}
