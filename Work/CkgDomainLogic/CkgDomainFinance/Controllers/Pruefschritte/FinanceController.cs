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
        public FinancePruefschritteViewModel PruefschritteViewModel { get { return GetViewModel<FinancePruefschritteViewModel>(); } }

        [CkgApplication]
        public ActionResult Pruefschritte()
        {
            return View(PruefschritteViewModel);
        }

        [HttpPost]
        public ActionResult LoadPruefschritte(PruefschrittSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                PruefschritteViewModel.LoadPruefschritte(model);
                if (PruefschritteViewModel.Pruefschritte.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Pruefschritte/PruefschritteSuche", model);
        }

        [HttpPost]
        public ActionResult ShowPruefschritte()
        {
            return PartialView("Pruefschritte/PruefschritteGrid", PruefschritteViewModel);
        }

        [GridAction]
        public ActionResult PruefschritteAjaxBinding()
        {
            return View(new GridModel(PruefschritteViewModel.PruefschritteFiltered));
        }

        [HttpPost]
        public ActionResult PruefschrittErledigen(string aktionsnr, string bucid)
        {
            var erg = PruefschritteViewModel.PruefschrittErledigen(aktionsnr, bucid);

            return Json(erg);
        }

        [HttpPost]
        public ActionResult FilterGridPruefschritte(string filterValue, string filterColumns)
        {
            PruefschritteViewModel.FilterPruefschritte(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportPruefschritteFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = PruefschritteViewModel.PruefschritteFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Aktionen", dt);

            return new EmptyResult();
        }

        public ActionResult ExportPruefschritteFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = PruefschritteViewModel.PruefschritteFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Aktionen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
