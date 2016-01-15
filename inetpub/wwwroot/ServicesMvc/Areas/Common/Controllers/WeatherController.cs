﻿using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Common.Controllers
{
    public class WeatherController : CkgDomainController
    {
        public override string DataContextKey { get { return "WeatherViewModel"; } }

        public WeatherViewModel ViewModel { get { return GetViewModel<WeatherViewModel>(); } }


        public WeatherController(IAppSettings appSettings, ILogonContextDataService logonContext, IWeatherDataService weatherServiceDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, weatherServiceDataService);
        }

        [CkgApplication]
        public ActionResult Index()
        {
            //ViewModel.DataInit(); "hamburg,de"

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult PrepareWeatherCityDropdown(string city)
        {
            const string country = "de";

            var jsonData = ViewModel.GetWeatherCities(country, city);

            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult PrepareWeatherWidget(string cityAndCountry)
        {
            var jsonData = ViewModel.GetWeatherData(cityAndCountry);

            return Json(jsonData);
        }
    }
}
