using System.Collections;
using System.Configuration;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.AppUserOverview.Contracts;
using CkgDomainLogic.AppUserOverview.Models;
using CkgDomainLogic.AppUserOverview.ViewModels;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using CkgDomainLogic.UserReporting.Contracts;
using ServicesMvc.AppUserOverview.Models;
using System.Collections.Generic;

// namespace ServicesMvc.Controllers
namespace ServicesMvc.Admin.Controllers
{
    public class AppUserOverviewController : CkgDomainController
    {
        public override string DataContextKey { get { return "AppUserOverviewViewModel"; } }

        public AppUserOverviewViewModel ViewModel { get { return GetViewModel<AppUserOverviewViewModel>(); } }

        public AppUserOverviewController(IAppSettings appSettings, ILogonContextDataService logonContext, IAppUserOverviewDataService appUserOverviewDataService) : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, appUserOverviewDataService);
        }

        [CkgApplication]
        public ActionResult Report() 
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadAppUserOverview(AppUserOverviewSelektor model)
        {
            ViewModel.AppUserOverviewSelektor = model;

            ViewModel.Validate(ModelState.AddModelError);

            if (ModelState.IsValid && !PersistableMode)
            {
                ViewModel.LoadAppUserOverview();
                if (ViewModel.AppUserOverviewList.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PersistablePartialView("Partial/SucheAppUserOverview", ViewModel.AppUserOverviewSelektor);
        }

        [HttpPost]
        public ActionResult ShowAppUserOverview()
        {
            return PartialView("Partial/AppUserOverviewGrid", ViewModel);
        }

        [GridAction]
        public ActionResult AppUserOverviewAjaxBinding()
        {
            return View(new GridModel(ViewModel.AppUserOverviewFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridAppUserOverview(string filterValue, string filterColumns)
        {
            ViewModel.FilterAppUserOverview(filterValue, filterColumns);

            return new EmptyResult();
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.AppUserOverviewFiltered;
        }

        #endregion
    }
}
