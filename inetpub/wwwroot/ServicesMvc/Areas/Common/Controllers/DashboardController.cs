// ReSharper disable RedundantUsingDirective

using System.Threading;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using System.Linq;
using GeneralTools.Models;

namespace ServicesMvc.Common.Controllers
{
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

        [HttpPost]
        public ActionResult GetBarChartData(string id)
        {
            var dbId = id.Replace("id_", "").Replace("#", "");
            var dashboardItem = ViewModel.DashboardItems.FirstOrDefault(item => item.ID == dbId.ToInt());

            if (dashboardItem != null)
            {
                var data = DashboardAppUrlService.InvokeViewModelForAppUrl(dashboardItem.RelatedAppUrl, dashboardItem.Title);
                var options = dashboardItem.ChartJsonOptions;
                if (options.NotNullOrEmpty().Contains("@ticks") && data.labels != null)
                {
                    // [[0,"walter"], [1,"zabel"]]
                    var labelArray = data.labels;
                    options = options.Replace("@ticks", 
                        string.Format("[{0}]", 
                            string.Join(",", labelArray.Select(s => string.Format("[{0},\"{1}\"]", labelArray.ToList().IndexOf(s), s)) )));
                }

                return Json(new { data, options });
            }

            return Json(new { });
        }
    }
}
