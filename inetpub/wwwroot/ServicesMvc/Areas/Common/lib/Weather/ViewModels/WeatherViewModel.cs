// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class WeatherViewModel : CkgBaseViewModel
    {
        public const int WidgetMax = 3;

        public int WidgetVisibleCount { get; set; }

        public WeatherWidgetUserSettings[] WidgetUserSettings { get; set; }
    

        [XmlIgnore]
        static IGeneralConfigurationProvider GeneralConfigurationProvider { get { return DependencyResolver.Current.GetService<IGeneralConfigurationProvider>(); } }

        [XmlIgnore]
        public IWeatherDataService DataService { get { return CacheGet<IWeatherDataService>(); } }

        [XmlIgnore]
        private int JsonDataCacheExpirationMinutes
        {
            get { return GeneralConfigurationProvider.GetConfigAllServerVal(DataService.ConfigurationContextKey, "ServiceRequestCacheExpirationMinutes").ToInt(0); }
        }

        public Dictionary<string, string> CountryDict = new Dictionary<string, string>
        {
            { "de", "Deutschland"},
            { "at", "Österreich"},
            { "ch", "Schweiz"},
        };


        public void DataInit()
        {
            WidgetVisibleCount = 2;

            if (WidgetUserSettings == null)
                WidgetUserSettings = new []
                {
                    new WeatherWidgetUserSettings { Country = "de", City = "Hamburg" },
                    new WeatherWidgetUserSettings { Country = "at", City = "Wien" },
                    new WeatherWidgetUserSettings { Country = "de", City = "Berlin" },
                };
        }


        bool JsonDataCacheExpired(JsonItemsPackage jsonPackage)
        {
            return jsonPackage.EditDate < (DateTime.Now.AddMinutes(-1 * JsonDataCacheExpirationMinutes));
        }

        public JsonItemsPackage GetWeatherCities(string city, int index)
        {
            var jsonData = DataService.RequestGetWeatherCities(AppSettings.DataPath, WidgetUserSettings[index].Country, city);

            return jsonData;
        }

        public JsonItemsPackage GetWeatherData(string city, int index)
        {
            WidgetUserSettings[index].City = city;

            var cityAndCountry = city + "," + WidgetUserSettings[index].Country;

            var itemId = cityAndCountry;
            var ownerKey = cityAndCountry;

            // <Json data caching>
            var pService = LogonContext.PersistanceService;
            var jsonData = pService.GetCachedItemAsJsonPackage(
                                itemId, ownerKey, DataService.ConfigurationContextKey,
                                LogonContext.UserName, false,
                                JsonDataCacheExpired,
                                () => DataService.RequestGetWeatherData(cityAndCountry));
            // </Json data caching>

            return jsonData;
        }

        public void SetCountry(string country, int index)
        {
            WidgetUserSettings[index].Country = country;
        }
    }
}
