// ReSharper disable RedundantUsingDirective
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using System.Linq;
using GeneralTools.Models;

namespace ServicesMvc.Common.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class DashboardController : CkgDomainController
    {
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
        public ActionResult GetChartData(string id)
        {
            var itemData = ViewModel.GetChartData(id);
            if (itemData is IDashboardItem)
                return PartialView("Partial/Test");

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
