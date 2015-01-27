// ReSharper disable RedundantUsingDirective
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
    }
}
