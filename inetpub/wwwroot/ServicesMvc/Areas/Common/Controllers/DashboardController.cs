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
            Thread.Sleep(1000);

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
                                new []{5, 0}, new []{1, 1}, new []{9, 2}, new []{4, 3}
                            },
                    };

            var options = new
                {
                    bars = new
                        {
                            show = true,
                            horizontal = true,
                            shadowSize = 0,
                            barWidth = 0.5,
                            fillColor = new {
                                colors = new[] { (id.Contains("003") ? "#CB4B4B" : "#4B4BCB"), "#fff" },
                              start = "top",
                              end = "bottom"
                            },
                        },
                    mouse = new
                        {
                            track = true,
                            relative = true
                        },
                    xaxis = new
                        {
                            min = 0,
                        },
                    yaxis = new
                        {
                            min = 0,
                            autoscaleMargin = 1
                        }
                };

            return Json(new { data, options });
        }
    }
}
