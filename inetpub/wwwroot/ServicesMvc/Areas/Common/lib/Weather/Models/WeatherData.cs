// ReSharper disable All

using System;
using System.Globalization;
using System.Linq;
using GeneralTools.Models;
using SapORM.Contracts;

namespace CkgDomainLogic.General.Models.OpenWeatherMap
{
    public class WeatherData
    {
        public WdCity city { get; set; }
        public WdItem[] list { get; set; }
    }

    public class WdCity
    {
        public long id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public int cnt { get; set; }
    }

    public class WdItem
    {
        public long dt { get; set; }
        public WdItemMain main { get; set; }
        public WdItemWeather[] weather { get; set; }
        public string dt_txt { get; set; }

        public WdItemWeather weatherFirst { get { return weather == null || weather.None() ? new WdItemWeather() : weather.First(); } }

        public string dateWeekday { get { return getDate().ToString("ddd").SubstringTry(0, 2); } }

        private DateTime getDate()
        {
            DateTime date;
            if (!DateTime.TryParseExact(dt_txt, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return DateTime.Today;

            return date;
        }
    }

    public class WdItemMain
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double temp_kf { get; set; }
    }

    public class WdItemWeather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
}