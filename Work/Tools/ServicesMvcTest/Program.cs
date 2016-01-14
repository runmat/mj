using System;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.General.Models.OpenWeatherMap;
using CkgDomainLogic.Services;
using GeneralTools.Models;
// ReSharper disable All

namespace ServicesMvcTest
{
    class Program
    {
        static void Main()
        {
            Test2();
        }

        static bool WeatherDateMatches(string firstDateTxt, string dtTxt)
        {
            var firstDate = DateTime.Parse(firstDateTxt);
            var dt = DateTime.Parse(dtTxt);

            var hour = dt.Date == DateTime.Now.Date ? firstDate.Hour : 12;

            return dt.Hour == hour;
        }

        static void Test()
        {
            string sData = File.ReadAllText(@"C:\Users\JenzenM\Downloads\data.json");
            var jsonData = new JavaScriptSerializer().Deserialize<WeatherData>(sData);

            var firstDateTxt = jsonData.list.FirstOrDefault().dt_txt;
            var list = jsonData.list.Where(d => WeatherDateMatches(firstDateTxt, d.dt_txt)).Select(d => d).ToArray();

            jsonData.list = list;

            sData = new JavaScriptSerializer().Serialize(jsonData);
        }

        static void Test2()
        {
            var appSettings = new CkgDomainAppSettings();

            var service = new WeatherDataServiceOpenWeatherMap();
            service.InternalUseForExportRequestGetWeatherCities(appSettings.DataPath, "CH");

            //service.RequestGetWeatherCities(appSettings.DataPath, "DE");
            //service.RequestGetWeatherCities(appSettings.DataPath, "AT");
            //service.RequestGetWeatherCities(appSettings.DataPath, "CH");
        }
    }
}
