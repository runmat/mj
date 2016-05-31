using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralTools.Services
{
    public class DateService
    {
        private static List<Feiertag> _feiertage;
        public static List<Feiertag> Feiertage
        {
            get
            {
                if (_feiertage != null) 
                    return _feiertage;

                _feiertage = new List<Feiertag>();
                for (var i = -1; i < 3; i++)
                    _feiertage = _feiertage.Concat(new DeutscheFeiertageEinesJahres(DateTime.Now.Year + i).Feiertage).ToList();

                return _feiertage;
            }
        }

        public static string FeiertageAsString { get { return string.Join(",", Feiertage.Select(f => f.Datum.ToString("dd.MM.yyyy")).ToArray()); } }

        public static Feiertag GetFeiertag(DateTime? datum)
        {
            if (datum == null)
                return null;

            return Feiertage.FirstOrDefault(f => f.Datum == datum);
        }

        public static bool IstFeiertag(DateTime? datum)
        {
            return (GetFeiertag(datum) != null);
        }

        public static DateTime NaechsterWerktag(bool inklHeute = false)
        {
            var datum = (inklHeute ? DateTime.Today : DateTime.Today.AddDays(1));

            while (IstFeiertag(datum) || datum.DayOfWeek == DayOfWeek.Saturday || datum.DayOfWeek == DayOfWeek.Sunday)
            {
                datum = datum.AddDays(1);
            }

            return datum;
        }
    }
}
