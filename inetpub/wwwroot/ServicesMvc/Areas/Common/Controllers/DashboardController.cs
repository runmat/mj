// ReSharper disable RedundantUsingDirective

using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using System.Linq;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;

namespace ServicesMvc.Common.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class DashboardController : CkgDomainController
    {
        private const string CacheJsonDataPersistableGroupKey = "DashboardJsonDataCache";

        public override string DataContextKey { get { return "DashboardViewModel"; } }

        public DashboardViewModel ViewModel { get { return GetViewModel<DashboardViewModel>(); } }

        public WeatherViewModel WeatherViewModel { get { return GetViewModel<WeatherViewModel>(); } }


        public DashboardController(IAppSettings appSettings, ILogonContextDataService logonContext, IDashboardDataService dashboardDataService, IWeatherDataService weatherServiceDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, dashboardDataService);
            InitViewModel(WeatherViewModel, appSettings, logonContext, weatherServiceDataService);
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult Weather()
        {
            WeatherViewModel.DataInit(3);

            return View(WeatherViewModel);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult ShowReportForDashboardItem(int id, string token)
        {
            var redirectUrl = ViewModel.DashboardPrepareReportForItem(id);

            return Redirect(redirectUrl);
        }

        [HttpPost]
        public ActionResult GetChartData(string id, bool clearDataCache)
        {
            var dashboardItem = ViewModel.GetDashboardItem(id);
            if (dashboardItem == null)
                return new EmptyResult();

            if (dashboardItem.IsPartialView)
            {
                ViewBag.IsDashboard = true;

                if (dashboardItem.RelatedAppUrl.ToLower().Contains("weather"))
                {
                    WeatherViewModel.DataInit(dashboardItem.RowSpanReal);
                    return PartialView(dashboardItem.RelatedAppUrl, WeatherViewModel);
                }

                return PartialView(dashboardItem.RelatedAppUrl);
            }

            // <Json data caching>
            var pService = LogonContext.PersistanceService;
            var itemData = pService.GetCachedItemAsJsonPackage(
                                id, LogonContext.UserName, CacheJsonDataPersistableGroupKey, 
                                LogonContext.UserName, clearDataCache,
                                data => dashboardItem.Options.JsonDataCacheExpired(data.EditDate), 
                                () => ViewModel.GetChartData(id, dashboardItem));
            // </Json data caching>

            return Json(itemData);
        }

        [HttpPost]
        public ActionResult DashboardItemsSave(string commaSeparatedIds)
        {
            ViewModel.DashboardItemsSave(commaSeparatedIds);

            return Json(new { hiddenItemsCount = ViewModel.HiddenDashboardItems.Count });
        }



        #region Dashboard Weather Widget Support

        [HttpPost]
        public ActionResult PrepareWeatherCountryDropdown(string country, int index)
        {
            WeatherViewModel.SetCountry(country, index);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult PrepareWeatherCityTextbox(string city, int index)
        {
            var jsonData = WeatherViewModel.GetWeatherCities(city, index);

            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult PrepareWeatherWidget(string city, int index)
        {
            var jsonData = WeatherViewModel.GetWeatherData(city, index);

            return Json(jsonData);
        }


        [HttpPost]
        public ActionResult DashboardItemUserRowSpanSave(int userRowSpanOverride, int rawWidgetId)
        {
            var dashboardItem = ViewModel.GetDashboardItem(rawWidgetId.ToString());
            if (dashboardItem == null)
                return new EmptyResult();

            ViewModel.DashboardItemUserRowSpanSave(userRowSpanOverride, rawWidgetId);
            WeatherViewModel.DataInit(dashboardItem.RowSpanReal);

            return new EmptyResult();
        }

        #endregion
    }
}
