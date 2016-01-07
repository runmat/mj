// ReSharper disable RedundantUsingDirective

using System;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using System.Linq;
using System.Web.Script.Serialization;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using MvcTools.Web;
using Newtonsoft.Json;

namespace ServicesMvc.Common.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class DashboardController : CkgDomainController
    {
        private const string CacheJsonDataPersistableGroupKey = "DashboardJsonDataCache";

        public override string DataContextKey { get { return "DashboardViewModel"; } }

        public DashboardViewModel ViewModel { get { return GetViewModel<DashboardViewModel>(); } }


        public DashboardController(IAppSettings appSettings, ILogonContextDataService logonContext, IDashboardDataService dashboardDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, dashboardDataService);
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
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
                return PartialView(dashboardItem.RelatedAppUrl);
            }

            // <Json data caching>
            var pService = LogonContext.PersistanceService;
            var itemData = pService.GetCachedChartItemAsJson(
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
    }
}
