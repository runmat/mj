using System;
using System.Web.Mvc;
using CkgDomainLogic.Finance.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Finance.ViewModels;
using CkgDomainLogic.General.Services;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Finance-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FinanceController  
    {
        public FinanceBewertungViewModel BewertungViewModel { get { return GetViewModel<FinanceBewertungViewModel>(); } }

        [CkgApplication]
        public ActionResult Fahrzeugbewertung()
        {
            BewertungViewModel.FillVertragsarten();

            return View(BewertungViewModel);
        }

        [HttpPost]
        public ActionResult LoadVorgaenge(VorgangSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                BewertungViewModel.LoadVorgaenge(model);
                if (BewertungViewModel.Vorgaenge.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            model.AuswahlVertragsart = BewertungViewModel.DataService.Suchparameter.AuswahlVertragsart;

            return PartialView("Bewertung/VorgangSuche", model);
        }

        [HttpPost]
        public ActionResult ShowVorgaenge()
        {
            return PartialView("Bewertung/VorgaengeGrid", BewertungViewModel);
        }

        [GridAction]
        public ActionResult VorgaengeAjaxBinding()
        {
            return View(new GridModel(BewertungViewModel.VorgaengeFiltered));
        }

        [HttpPost]
        public ActionResult ShowVorgangEdit(string kontonr, string cin, string paid)
        {
            var vorg = BewertungViewModel.GetVorgangFuerBewertung(kontonr, cin, paid);

            return PartialView("Bewertung/VorgangEdit", vorg);
        }

        [HttpPost]
        public ActionResult SaveBewertung(VorgangBewertung model)
        {
            if (ModelState.IsValid)
            {
                BewertungViewModel.SaveBewertung(model, ModelState);
            }

            return PartialView("Bewertung/VorgangEdit", model);
        }

        [HttpPost]
        public ActionResult FilterGridVorgaenge(string filterValue, string filterColumns)
        {
            BewertungViewModel.FilterVorgaenge(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportVorgaengeFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = BewertungViewModel.VorgaengeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Vorgänge", dt);

            return new EmptyResult();
        }

        public ActionResult ExportVorgaengeFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = BewertungViewModel.VorgaengeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Vorgänge", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
