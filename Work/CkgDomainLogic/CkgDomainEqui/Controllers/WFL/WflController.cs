using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.WFL.Models;
using CkgDomainLogic.Wfl.ViewModels;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class WflController 
    {
        public WflViewModel ViewModel { get { return GetViewModel<WflViewModel>(); } }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadAbmeldungen(WflAbmeldungSelektor model)
        {
            ViewModel.WflAbmeldungSelektor = model;

            ViewModel.Validate(AddModelError);

            if (ModelState.IsValid)
            {
                ViewModel.LoadWflAbmeldungen();
                if (ViewModel.WflAbmeldungen.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Abmeldungen/SucheFahrzeuge", ViewModel.WflAbmeldungSelektor);
        }

        [HttpPost]
        public ActionResult ShowAbgmeldeteFahrzeuge()
        {
            return PartialView("Abmeldungen/FahrzeugeGrid", ViewModel);
        }

        [GridAction]
        public ActionResult AbgemeldeteFahrzeugeAjaxBinding()
        {
            return View(new GridModel(ViewModel.WflAbmeldungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridAbgemeldeteFahrzeuge(string filterValue, string filterColumns)
        {
            ViewModel.FilterWflAbmeldungen(filterValue, filterColumns);

            return new EmptyResult();
        }
        
        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.WflAbmeldungenFiltered;
        }

        #endregion
    }
}
