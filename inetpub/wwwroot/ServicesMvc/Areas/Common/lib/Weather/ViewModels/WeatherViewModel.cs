 // ReSharper disable RedundantUsingDirective
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
using WebTools.Services;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class WeatherViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IWeatherDataService DataService { get { return CacheGet<IWeatherDataService>(); } }

        public void DataInit()
        {
            const string cityAndCountry = "hamburg,de";

            var jsonData = DataService.GetWeatherData(cityAndCountry);
        }
    }
}
