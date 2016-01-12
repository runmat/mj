using System.Collections;
using System.Globalization;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Strafzettel.Contracts;
using CkgDomainLogic.Strafzettel.Models;
using CkgDomainLogic.Strafzettel.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Infrastructure;

namespace ServicesMvc.Controllers
{
    public class StrafzettelController : CkgDomainController
    {
        public override string DataContextKey { get { return "StrafzettelViewModel"; } }

        public StrafzettelViewModel ViewModel { get { return GetViewModel<StrafzettelViewModel>(); } }


        public StrafzettelController(IAppSettings appSettings, ILogonContextDataService logonContext, IStrafzettelDataService strafzettelDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, strafzettelDataService);
        }

        [CkgApplication]
        public ActionResult Report()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadStrafzettel(StrafzettelSelektor model)
        {
            ViewModel.StrafzettelSelektor = model;

            ViewModel.Validate(ModelState.AddModelError);

            if (ModelState.IsValid && !PersistableMode)
            {
                ViewModel.LoadStrafzettel();
                if (ViewModel.Strafzettel.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PersistablePartialView("Partial/SucheStrafzettel", ViewModel.StrafzettelSelektor);
        }

        [HttpPost]
        public ActionResult ShowStrafzettel()
        {
            return PartialView("Partial/StrafzettelGrid", ViewModel);
        }

        [GridAction]
        public ActionResult StrafzettelAjaxBinding()
        {
            return View(new GridModel(ViewModel.StrafzettelFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridStrafzettel(string filterValue, string filterColumns)
        {
            ViewModel.FilterStrafzettel(filterValue, filterColumns);

            return new EmptyResult();
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.StrafzettelFiltered;
        }

        #endregion
    }
}
