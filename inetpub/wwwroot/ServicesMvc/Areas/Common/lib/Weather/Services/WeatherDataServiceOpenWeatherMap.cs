// ReSharper disable RedundantUsingDirective
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GeneralTools.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models.OpenWeatherMap;
using GeneralTools.Contracts;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class WeatherDataServiceOpenWeatherMap : IWeatherDataService
    {
        static IGeneralConfigurationProvider GeneralConfigurationProvider { get { return DependencyResolver.Current.GetService<IGeneralConfigurationProvider>(); } }

        public string ConfigurationContextKey { get { return "WeatherDataService_OpenWeatherMap"; } }

        private string ApiKey { get { return GeneralConfigurationProvider.GetConfigAllServerVal(ConfigurationContextKey, "License_ApiKey"); } }

        private string ServiceRequestUrl { get { return GeneralConfigurationProvider.GetConfigAllServerVal(ConfigurationContextKey, "ServiceRequestUrl"); } }


        static bool WeatherDateMatches(string firstDateTxt, string dtTxt)
        {
            var firstDate = DateTime.Parse(firstDateTxt);
            var dt = DateTime.Parse(dtTxt);

            var hour = dt.Date == DateTime.Now.Date ? firstDate.Hour : 12;

            return dt.Hour == hour;
        }

        public WeatherData FilterWeatherData(string jsonDataAsString)
        {
            var jsonData = new JavaScriptSerializer().Deserialize<WeatherData>(jsonDataAsString);

            if (jsonData == null || jsonData.list == null)
                return new WeatherData();

            var firstOrDefault = jsonData.list.FirstOrDefault();
            if (firstOrDefault == null)
                return jsonData;

            var firstDateTxt = firstOrDefault.dt_txt;
            jsonData.list = jsonData.list.Where(d => WeatherDateMatches(firstDateTxt, d.dt_txt)).Select(d => d).ToArray();

            return jsonData;
        }

        public JsonItemsPackage RequestGetWeatherData(string cityAndCountry)
        {
           try
            {
                var url = string.Format(ServiceRequestUrl, cityAndCountry, ApiKey);

                var request = WebRequest.Create(url);

                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream == null)
                {
                    response.Close();
                    return new JsonItemsPackage();
                }

                var streamReader = new StreamReader(stream);
                var jsonDataAsString = streamReader.ReadToEnd();

                response.Close();
                streamReader.Close();

                var jsonData = FilterWeatherData(jsonDataAsString);

                return new JsonItemsPackage
                {
                    ID = "",
                    data = jsonData,
                    dataAsText = jsonDataAsString
                };
            }
            catch (WebException)
            {
                return new JsonItemsPackage();
            }
        }
    }
}