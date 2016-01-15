// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GeneralTools.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models.OpenWeatherMap;
using GeneralTools.Contracts;
using Newtonsoft.Json;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class WeatherDataServiceOpenWeatherMap : IWeatherDataService
    {
        static IGeneralConfigurationProvider GeneralConfigurationProvider { get { return DependencyResolver.Current.GetService<IGeneralConfigurationProvider>(); } }

        public string ConfigurationContextKey { get { return "WeatherDataService_OpenWeatherMap"; } }

        private string ApiKey { get { return GeneralConfigurationProvider.GetConfigAllServerVal(ConfigurationContextKey, "License_ApiKey"); } }

        private string ServiceRequestWeatherUrl { get { return GeneralConfigurationProvider.GetConfigAllServerVal(ConfigurationContextKey, "ServiceRequestWeatherUrl"); } }

        private static Dictionary<string, WeatherCity[]> _weatherCities = new Dictionary<string, WeatherCity[]>();


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

        private JsonItemsPackage RequestGetJsonData(string url)
        {
            try
            {
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

        public JsonItemsPackage RequestGetWeatherData(string cityAndCountry)
        {
            var url = string.Format(ServiceRequestWeatherUrl, cityAndCountry, ApiKey);

            return RequestGetJsonData(url);
        }

        public JsonItemsPackage RequestGetWeatherCities(string dataPath, string country, string city)
        {
            country = country.ToLower();
            if (_weatherCities.ContainsKey(country))
                return new JsonItemsPackage
                {
                    ID = country,
                    data = FilterWeatherCities(_weatherCities[country], city),
                    dataAsText = ""
                };

            var weatherCitiesFileName = Path.Combine(dataPath, "Weather", "JsonData", "city.list." + country + ".json");
            var jsonDataAsString = File.ReadAllText(weatherCitiesFileName);

            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            var jsonData = serializer.Deserialize<WeatherCity[]>(jsonDataAsString);
            jsonData = jsonData.Distinct(new WeatherCityComparer()).OrderBy(c => c.name).ToArray();

            _weatherCities.Add(country, jsonData);

            return new JsonItemsPackage
            {
                ID = country,
                data = FilterWeatherCities(_weatherCities[country], city),
                dataAsText = ""
            };

        }

        static string[] FilterWeatherCities(IEnumerable<WeatherCity> cities, string city)
        {
            var jsonData = cities.Select(c => c.name).ToArray(); 
            if (city.IsNotNullOrEmpty())
                jsonData = jsonData.Where(c => c.ToLower().StartsWith(city.ToLower())).ToArray();

            return jsonData;
        }


        #region _internal use

        public void WeatherCitiesSaveToFile(string dataPath, string country)
        {
            // for internal use only
            var weatherCitiesFileName = Path.Combine(dataPath, "Weather", "JsonData", "city.list.json");
            var jsonDataAsString = File.ReadAllText(weatherCitiesFileName);

            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            var jsonData = serializer.Deserialize<WeatherCity[]>(jsonDataAsString);
            jsonData = jsonData.Where(d => d.country == country).ToArray();
            var len = jsonData.Length;

            var countryDataAsString = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            weatherCitiesFileName = Path.Combine(dataPath, "Weather", "JsonData", "city.list." + country + ".json");
            File.WriteAllText(weatherCitiesFileName, countryDataAsString);
        }

        #endregion
    }
}