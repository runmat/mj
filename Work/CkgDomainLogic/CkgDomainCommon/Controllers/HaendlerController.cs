using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public class HaendlerController : CkgDomainController
    {
        public override string DataContextKey { get { return "HaendlerAdressenViewModel"; } }

        public HaendlerAdressenViewModel ViewModel { get { return GetViewModel<HaendlerAdressenViewModel>(); } }


        public HaendlerController(IAppSettings appSettings, ILogonContextDataService logonContext, IHaendlerAdressenDataService haendlerAdressenDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, haendlerAdressenDataService);
            InitModelStatics();
        }

        [CkgApplication]
        public ActionResult Adressen()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [GridAction]
        public ActionResult HaendlerAdressenAjaxBinding()
        {
            return View(new GridModel(ViewModel.HaendlerAdressenFiltered));
        }

        void InitModelStatics()
        {
            HaendlerAdresse.GetViewModel = GetViewModel<HaendlerAdressenViewModel>;
        }


        [HttpPost]
        public ActionResult EditHaendlerAdresse(string id)
        {
            ViewModel.InsertMode = false;
            ModelState.Clear();
            return PartialView("Partial/DetailsForm", ViewModel.GetItem(id).SetInsertMode(ViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult NewHaendlerAdresse()
        {
            ViewModel.InsertMode = true;
            ModelState.Clear();
            return PartialView("Partial/DetailsForm", ViewModel.NewItem().SetInsertMode(ViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult HaendlerAdresseDetailsFormSave(HaendlerAdresse model)
        {
            var viewModel = ViewModel;

            viewModel.ValidateModel(model, viewModel.InsertMode, ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                if (viewModel.InsertMode)
                    viewModel.AddItem(model);

                viewModel.SaveItem(model, ModelState.AddModelError);
            }

            model.InsertModeTmp = viewModel.InsertMode;

            return PartialView("Partial/DetailsForm", model);
        }

        [HttpPost]
        public ActionResult FilterGridHaendlerAdressen(string filterValue, string filterColumns)
        {
            ViewModel.FilterHaendlerAdressen(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.HaendlerAdressenFiltered;
        }

        #endregion
    }
}
