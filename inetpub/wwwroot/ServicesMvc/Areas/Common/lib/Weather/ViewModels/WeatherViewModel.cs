// ReSharper disable RedundantUsingDirective
using System;
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
        [XmlIgnore]
        static IGeneralConfigurationProvider GeneralConfigurationProvider { get { return DependencyResolver.Current.GetService<IGeneralConfigurationProvider>(); } }

        [XmlIgnore]
        public IWeatherDataService DataService { get { return CacheGet<IWeatherDataService>(); } }

        [XmlIgnore]
        private int JsonDataCacheExpirationMinutes
        {
            get { return GeneralConfigurationProvider.GetConfigAllServerVal(DataService.ConfigurationContextKey, "ServiceRequestCacheExpirationMinutes").ToInt(0); }
        }


        bool JsonDataCacheExpired(JsonItemsPackage jsonPackage)
        {
            return jsonPackage.EditDate < (DateTime.Now.AddMinutes(-1 * JsonDataCacheExpirationMinutes));
        }

        public JsonItemsPackage GetWeatherCities(string country, string city)
        {
            var jsonData = DataService.RequestGetWeatherCities(AppSettings.DataPath, country, city);

            return jsonData;
        }

        public JsonItemsPackage GetWeatherData(string cityAndCountry)
        {
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
    }
}
