using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController : CkgDomainController
    {
        public AbgemeldeteFahrzeugeViewModel AbgemeldeteFahrzeugeViewModel { get { return GetViewModel<AbgemeldeteFahrzeugeViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportAbmeldungen()
        {
            _dataContextKey = typeof(AbgemeldeteFahrzeugeViewModel).Name;
            AbgemeldeteFahrzeugeViewModel.DataInit();

            return View(AbgemeldeteFahrzeugeViewModel);
        }

        [HttpPost]
        public ActionResult LoadAbgmeldeteFahrzeuge(AbgemeldeteFahrzeugeSelektor model)
        {
            AbgemeldeteFahrzeugeViewModel.AbgemeldeteFahrzeugeSelektor = model;

            AbgemeldeteFahrzeugeViewModel.Validate(AddModelError);

            if (ModelState.IsValid)
            {
                AbgemeldeteFahrzeugeViewModel.LoadAbgemeldeteFahrzeuge();
                if (AbgemeldeteFahrzeugeViewModel.Fahrzeuge.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Abmeldungen/SucheFahrzeuge", AbgemeldeteFahrzeugeViewModel.AbgemeldeteFahrzeugeSelektor);
        }

        [HttpPost]
        public ActionResult ShowAbgmeldeteFahrzeuge()
        {
            return PartialView("Abmeldungen/FahrzeugeGrid", AbgemeldeteFahrzeugeViewModel);
        }

        [GridAction]
        public ActionResult AbgemeldeteFahrzeugeAjaxBinding()
        {
            return View(new GridModel(AbgemeldeteFahrzeugeViewModel.FahrzeugeFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridAbgemeldeteFahrzeuge(string filterValue, string filterColumns)
        {
            AbgemeldeteFahrzeugeViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult GetAbgemeldeteFahrzeugeHistorie(string fahrgestellnummer)
        {
            AbgemeldeteFahrzeugeViewModel.LoadHistorie(fahrgestellnummer);

            return PartialView("Abmeldungen/Historie", AbgemeldeteFahrzeugeViewModel);
        }
        
        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return AbgemeldeteFahrzeugeViewModel.FahrzeugeFiltered;
        }

        #endregion
    }
}
