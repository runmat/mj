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
        public FinanceFehlendeSchluesseltueteViewModel FehlendeSchluesseltueteViewModel { get { return GetViewModel<FinanceFehlendeSchluesseltueteViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportFehlendeSchluesseltuete()
        {
            FehlendeSchluesseltueteViewModel.DataInit();
            return View(FehlendeSchluesseltueteViewModel);
        }

        [HttpPost]
        public ActionResult LoadFehlendeSchluesseltuete(FinanceFehlendeSchluesseltueteViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (FehlendeSchluesseltueteViewModel.FehlendeSchluesseltuetes.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("FehlendeSchluesseltuete/FehlendeSchluesseltueteSuche", model);
        }

        [HttpPost]
        public ActionResult ShowFehlendeSchluesseltuete()
        {
            return PartialView("FehlendeSchluesseltuete/FehlendeSchluesseltueteGrid", FehlendeSchluesseltueteViewModel);
        }

        [GridAction]
        public ActionResult FehlendeSchluesseltueteAjaxBinding()
        {
            return View(new GridModel(FehlendeSchluesseltueteViewModel.FehlendeSchluesseltuetesFiltered));
        }

        [HttpPost]
        public ActionResult FilterFehlendeSchluesseltueteGrid(string filterValue, string filterColumns)
        {
            FehlendeSchluesseltueteViewModel.FilterFehlendeSchluesseltuetes(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteFehlendeSchluesseltuete()
        {
            FehlendeSchluesseltueteViewModel.DeleteFehlendeSchluesseltuete();

            return new EmptyResult();
        }



        [HttpPost]
        public JsonResult FahrzeugAuswahlFSSelectionChanged(string fin, bool isChecked)
        {
            int allSelectionCount, allCount = 0, allFoundCount = 0;
            if (String.IsNullOrEmpty(fin))
                FehlendeSchluesseltueteViewModel.SelectFahrzeuge(isChecked, out allSelectionCount, out allCount, out allFoundCount);
            else
                FehlendeSchluesseltueteViewModel.SelectFahrzeug(fin, isChecked, out allSelectionCount);

            return Json(new { allSelectionCount, allCount, allFoundCount });
        }


        public ActionResult ExportFehlendeSchluesseltueteFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FehlendeSchluesseltueteViewModel.FehlendeSchluesseltuetesFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("FehlendeSchluesseltuete", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFehlendeSchluesseltueteFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = FehlendeSchluesseltueteViewModel.FehlendeSchluesseltuetesFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("FehlendeSchluesseltuete", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
