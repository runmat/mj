// ReSharper disable All

using System;
using System.Globalization;
using System.Linq;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Models.OpenWeatherMap
{

    #region Weather Cities

    public class WeatherCity
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
    }

    #endregion



    #region Weather Data

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

        public WdItemWeather weatherFirst
        {
            get { return weather == null || weather.None() ? new WdItemWeather() : weather.First(); }
        }

        public string dateWeekdayShort
        {
            get { return getDate().ToString("ddd").SubstringTry(0, 2); }
        }

        public string dateHeaderTop
        {
            get
            {
                var dt = getDate();
                var today = DateTime.Today;

                if (dt.Date <= DateTime.Today.Date.AddDays(1))
                    return string.Format("{0}, {1}", dateWeekdayShort, dt.ToString("dd.MM."));

                return string.Format("{0}", dt.ToString("dd.MM."));
            }
        }

        public string dateHeaderBottomBold
        {
            get
            {
                var dt = getDate();
                var today = DateTime.Today;

                if (dt.Date == DateTime.Today.Date)
                    return Localize.Today.ToLower();

                if (dt.Date == DateTime.Today.Date.AddDays(1))
                    return Localize.Tomorrow.ToLower();

                return dt.ToString("dddd");
            }
        }

        private DateTime getDate()
        {
            DateTime date;
            if (!DateTime.TryParseExact(dt_txt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out date))
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

    #endregion

}