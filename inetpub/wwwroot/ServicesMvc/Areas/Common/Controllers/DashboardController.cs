// ReSharper disable RedundantUsingDirective

using System.Threading;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
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
            Thread.Sleep(300);

            var data = new []
                {
                    new []
                        {
                            new []{3, 0}, new []{9, 1}, new []{2, 2}, new []{10, 3}
                        },
                };
            if (id.Contains("003"))
                data = new []
                    {
                        new []
                            {
                                new []{5, 0}, new []{1, 1}, new []{9, 2}, new []{4, 3}, new []{7, 4}
                            },
                    };

            var dbId = id.Replace("id_", "").Replace("#", "");
            var dashboardItem = ViewModel.DashboardItems.FirstOrDefault(item => item.ID == dbId.ToInt());

            var options = "";
            if (dashboardItem != null)
                options = dashboardItem.ChartJsonOptions;

            return Json(new { data, options });
        }
    }
}
