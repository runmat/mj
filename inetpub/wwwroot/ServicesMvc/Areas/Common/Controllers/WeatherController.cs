using System.Web.Mvc;
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
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult PrepareWeatherCountryDropdown(string country, int index)
        {
            ViewModel.SetCountry(country, index);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult PrepareWeatherCityTextbox(string city, int index)
        {
            var jsonData = ViewModel.GetWeatherCities(city, index);

            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult PrepareWeatherWidget(string city, int index)
        {
            var jsonData = ViewModel.GetWeatherData(city, index);

            return Json(jsonData);
        }
    }
}
