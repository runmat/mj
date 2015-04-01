using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.FzgModelle.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.FzgModelle.ViewModels;
using CkgDomainLogic.FzgModelle.Contracts;
using GeneralTools.Contracts;
using Telerik.Web.Mvc;
using MvcTools.Web;

namespace ServicesMvc.Controllers
{
    public class ModellIdController : CkgDomainController
    {
        public override string DataContextKey { get { return "ModellIdViewModel"; } }

        public ModellIdViewModel ViewModel { get { return GetViewModel<ModellIdViewModel>(); } }


        public ModellIdController(IAppSettings appSettings, ILogonContextDataService logonContext, IModellIdDataService modellIdDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, modellIdDataService);
        }

        [CkgApplication]
        public ActionResult Verwaltung()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [GridAction]
        public ActionResult ModellIdsAjaxBinding()
        {
            return View(new GridModel(ViewModel.ModellIdsFiltered));
        }


        [HttpPost]
        public ActionResult EditModellId(string id)
        {
            ViewModel.InsertMode = false;
            ModelState.Clear();
            return PartialView("Partial/DetailsForm", ViewModel.GetItem(id).SetInsertMode(ViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult NewModellId(string idToDuplicate)
        {
            ViewModel.InsertMode = true;
            ModelState.Clear();
            return PartialView("Partial/DetailsForm", ViewModel.NewItem(idToDuplicate).SetInsertMode(ViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult ModellIdDetailsFormSave(ModellId model)
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
        public ActionResult FilterGridModellId(string filterValue, string filterColumns)
        {
            ViewModel.FilterModellIds(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.ModellIdsFiltered;
        }

        #endregion
    }
}
