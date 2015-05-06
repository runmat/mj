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
       
        [CkgApplication]
        public ActionResult ReportAbmeldungen2()
        {
            _dataContextKey = typeof(AbgemeldeteFahrzeugeViewModel).Name;
            AbgemeldeteFahrzeugeViewModel.DataInit(true);
            return View(AbgemeldeteFahrzeugeViewModel);                        
        }

        [HttpPost]
        public ActionResult LoadAbgmeldeteFahrzeuge2(AbgemeldeteFahrzeugeSelektor model)
        {
            AbgemeldeteFahrzeugeViewModel.AbgemeldeteFahrzeugeSelektor = model;

            AbgemeldeteFahrzeugeViewModel.Validate(AddModelError);

            if (ModelState.IsValid)
            {
                AbgemeldeteFahrzeugeViewModel.LoadAbgemeldeteFahrzeuge2();
                if (AbgemeldeteFahrzeugeViewModel.Fahrzeuge.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Abmeldungen/SucheFahrzeuge2", AbgemeldeteFahrzeugeViewModel.AbgemeldeteFahrzeugeSelektor);
        }


        [HttpPost]
        public ActionResult ShowAbgmeldeteFahrzeuge2()
        {
            return PartialView("Abmeldungen/FahrzeugeGrid2", AbgemeldeteFahrzeugeViewModel);
        }                             
    }
}
