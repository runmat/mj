using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using DocumentTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController
    {
        public FahrzeugzulaeufeViewModel FahrzeugzulaeufeViewModel { get { return GetViewModel<FahrzeugzulaeufeViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportFahrzeugzulaeufe()
        {
            FahrzeugzulaeufeViewModel.InitHerstellerListe();

            ViewBag.HerstellerListe = FahrzeugzulaeufeViewModel.HerstellerListe;

            return View(FahrzeugzulaeufeViewModel);
        }

        [HttpPost]
        public ActionResult LoadFahrzeugzulaeufe(FahrzeugzulaeufeSelektor model)
        {
            if (ModelState.IsValid)
                FahrzeugzulaeufeViewModel.LoadFahrzeugzulaeufe(model, ModelState);

            ViewBag.HerstellerListe = FahrzeugzulaeufeViewModel.HerstellerListe;

            return PartialView("Fahrzeugzulaeufe/Suche", model);
        }

        [HttpPost]
        public ActionResult ShowFahrzeugzulaeufe()
        {
            return PartialView("Fahrzeugzulaeufe/Grid", FahrzeugzulaeufeViewModel);
        }

        [GridAction]
        public ActionResult FahrzeugzulaeufeAjaxBinding()
        {
            return View(new GridModel(FahrzeugzulaeufeViewModel.FahrzeugzulaeufeFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugzulaeufe(string filterValue, string filterColumns)
        {
            FahrzeugzulaeufeViewModel.FilterFahrzeugzulaeufe(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugzulaeufeFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugzulaeufeViewModel.FahrzeugzulaeufeFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.VehicleTransits, dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugzulaeufeFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugzulaeufeViewModel.FahrzeugzulaeufeFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.VehicleTransits, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
