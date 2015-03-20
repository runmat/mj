using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.WFM.Models;
using CkgDomainLogic.WFM.ViewModels;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class WfmController 
    {
        public WfmViewModel ViewModel { get { return GetViewModel<WfmViewModel>(); } }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadAbmeldungen(WfmAbmeldungSelektor model)
        {
            ViewModel.WfmAbmeldungSelektor = model;

            ViewModel.Validate(AddModelError);

            if (ModelState.IsValid)
            {
                ViewModel.LoadWflAbmeldungen();
                if (ViewModel.WfmAbmeldungen.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Abmeldungen/SucheAbmeldungen", ViewModel.WfmAbmeldungSelektor);
        }

        [HttpPost]
        public ActionResult ShowAbgmeldeteFahrzeuge()
        {
            return PartialView("Abmeldungen/AbmeldungenGrid", ViewModel);
        }

        [GridAction]
        public ActionResult AbgemeldeteFahrzeugeAjaxBinding()
        {
            return View(new GridModel(ViewModel.WfmAbmeldungenFiltered));
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
            return ViewModel.WfmAbmeldungenFiltered;
        }

        #endregion
    }
}
