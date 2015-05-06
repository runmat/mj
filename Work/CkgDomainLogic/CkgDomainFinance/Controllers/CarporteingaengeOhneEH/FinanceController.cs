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
        public FinanceCarporteingaengeOhneEHViewModel CarporteingaengeOhneEHViewModel { get { return GetViewModel<FinanceCarporteingaengeOhneEHViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportCarporteingaengeOhneEH()
        {
            CarporteingaengeOhneEHViewModel.DataInit();
            return View(CarporteingaengeOhneEHViewModel);
        }

        [HttpPost]
        public ActionResult LoadCarporteingaengeOhneEH(FinanceCarporteingaengeOhneEHViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (CarporteingaengeOhneEHViewModel.CarporteingaengeOhneEHs.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("CarporteingaengeOhneEH/CarporteingaengeOhneEHSuche", model);
        }

        [HttpPost]
        public ActionResult ShowCarporteingaengeOhneEH()
        {
            return PartialView("CarporteingaengeOhneEH/CarporteingaengeOhneEHGrid", CarporteingaengeOhneEHViewModel);
        }

        [GridAction]
        public ActionResult CarporteingaengeOhneEHAjaxBinding()
        {
            return View(new GridModel(CarporteingaengeOhneEHViewModel.CarporteingaengeOhneEHsFiltered));
        }

        [HttpPost]
        public ActionResult FilterCarporteingaengeOhneEHGrid(string filterValue, string filterColumns)
        {
            CarporteingaengeOhneEHViewModel.FilterCarporteingaengeOhneEHs(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteCarporteingaengeOhneEH()
        {
            CarporteingaengeOhneEHViewModel.DeleteCarporteingaengeOhneEH();

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult FahrzeugAuswahlCoEHSelectionChanged(string fin, bool isChecked)
        {
            int allSelectionCount, allCount = 0, allFoundCount = 0;
            if (String.IsNullOrEmpty(fin))
                CarporteingaengeOhneEHViewModel.SelectFahrzeuge(isChecked, out allSelectionCount, out allCount, out allFoundCount);
            else
                CarporteingaengeOhneEHViewModel.SelectFahrzeug(fin, isChecked, out allSelectionCount);

            return Json(new { allSelectionCount, allCount, allFoundCount });
        }


        public ActionResult ExportCarporteingaengeOhneEHFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = CarporteingaengeOhneEHViewModel.CarporteingaengeOhneEHsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("CarporteingaengeOhneEH", dt);

            return new EmptyResult();
        }

        public ActionResult ExportCarporteingaengeOhneEHFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = CarporteingaengeOhneEHViewModel.CarporteingaengeOhneEHsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("CarporteingaengeOhneEH", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
