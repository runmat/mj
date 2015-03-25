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
        public FinanceAnzeigePruefpunkteViewModel AnzeigePruefpunkteViewModel { get { return GetViewModel<FinanceAnzeigePruefpunkteViewModel>(); } }

        [CkgApplication]
        public ActionResult AnzeigePruefpunkte()
        {
            return View(AnzeigePruefpunkteViewModel);
        }

        [HttpPost]
        public ActionResult LoadPruefpunkte(PruefpunktSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                AnzeigePruefpunkteViewModel.LoadPruefpunkte(model);
                if (AnzeigePruefpunkteViewModel.Pruefpunkte.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("AnzeigePruefpunkte/PruefpunkteSuche", model);
        }

        [HttpPost]
        public ActionResult ShowPruefpunkte()
        {
            return PartialView("AnzeigePruefpunkte/PruefpunkteGrid", AnzeigePruefpunkteViewModel);
        }

        [GridAction]
        public ActionResult PruefpunkteAjaxBinding()
        {
            return View(new GridModel(AnzeigePruefpunkteViewModel.PruefpunkteFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridPruefpunkte(string filterValue, string filterColumns)
        {
            AnzeigePruefpunkteViewModel.FilterPruefpunkte(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportPruefpunkteFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = AnzeigePruefpunkteViewModel.PruefpunkteFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Prüfpunkte", dt);

            return new EmptyResult();
        }

        public ActionResult ExportPruefpunkteFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = AnzeigePruefpunkteViewModel.PruefpunkteFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Prüfpunkte", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
