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
        public FinanceTempVersandZweitschluesselViewModel TempVersandZweitschluesselViewModel { get { return GetViewModel<FinanceTempVersandZweitschluesselViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportTempVersandZweitschluessel()
        {
            TempVersandZweitschluesselViewModel.DataInit();
            return View(TempVersandZweitschluesselViewModel);
        }
        
        [HttpPost]
        public ActionResult LoadTempVersandZweitschluessel(FinanceTempVersandZweitschluesselViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (TempVersandZweitschluesselViewModel.TempVersandZweitschluessels.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("VersandZweitschluessel/TempVersandZweitschluesselSuche", model);
        }

        [HttpPost]
        public ActionResult ShowTempVersandZweitschluessel()
        {
            return PartialView("VersandZweitschluessel/TempVersandZweitschluesselGrid", TempVersandZweitschluesselViewModel);
        }

        [GridAction]
        public ActionResult TempVersandZweitschluesselAjaxBinding()
        {
            return View(new GridModel(TempVersandZweitschluesselViewModel.TempVersandZweitschluesselsFiltered));
        }

        [HttpPost]
        public ActionResult FilterTempVersandZweitschluesselGrid(string filterValue, string filterColumns)
        {
            TempVersandZweitschluesselViewModel.FilterTempVersandZweitschluessels(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SetMahnsperre()
        {
            TempVersandZweitschluesselViewModel.SetMahnsperre();

            return new EmptyResult(); 
        }

        [HttpPost]
        public JsonResult FahrzeugAuswahlSelectionChanged(string fin, bool isChecked)
        {
            int allSelectionCount, allCount = 0, allFoundCount = 0;
            if (String.IsNullOrEmpty(fin))
                TempVersandZweitschluesselViewModel.SelectFahrzeuge(isChecked, out allSelectionCount, out allCount, out allFoundCount);
            else
                TempVersandZweitschluesselViewModel.SelectFahrzeug(fin, isChecked, out allSelectionCount);

            return Json(new { allSelectionCount, allCount, allFoundCount });
        }


        public ActionResult ExportTempVersandZweitschluesselFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = TempVersandZweitschluesselViewModel.TempVersandZweitschluesselsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("TempVersandZweitschluessel", dt);

            return new EmptyResult();
        }

        public ActionResult ExportTempVersandZweitschluesselFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = TempVersandZweitschluesselViewModel.TempVersandZweitschluesselsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("TempVersandZweitschluessel", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
