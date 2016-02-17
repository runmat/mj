using System;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Services;
using Telerik.Web.Mvc;
using DocumentTools.Services;


namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Fahrzeug-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FahrzeugeController : CkgDomainController
    {
        public FloorcheckViewModel FloorcheckViewModel { get { return GetViewModel<FloorcheckViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportFloorcheck()
        {
            _dataContextKey = typeof(FloorcheckViewModel).Name;
            FloorcheckViewModel.DataMarkForRefresh();
            
            return View(FloorcheckViewModel);            
        }

        [HttpPost]
        public ActionResult LoadFloorcheckHaendler()
        {
            if (ModelState.IsValid)
            {
                FloorcheckViewModel.LoadFloorcheckHaendler();

                if (FloorcheckViewModel.FloorcheckHaendlers.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Floorcheck/FloorcheckSuche", FloorcheckViewModel);
        }

        [HttpPost]
        public ActionResult LoadFloorcheck(FloorcheckViewModel model)
        {
            if (ModelState.IsValid)
            {
                FloorcheckViewModel.FloorcheckHaendler = model.FloorcheckHaendler;
                
                FloorcheckViewModel.LoadFloorcheck();

                if (FloorcheckViewModel.Floorchecks.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Floorcheck/FloorcheckSuche", model);
        }

        [HttpPost]
        public ActionResult ShowFloorcheck()
        {
            return PartialView("Floorcheck/FloorcheckGrid", FloorcheckViewModel);
        }

        [GridAction]
        public ActionResult FloorcheckAjaxBinding()
        {
            return View(new GridModel(FloorcheckViewModel.FloorchecksFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridFloorcheck(string filterValue, string filterColumns)
        {
            FloorcheckViewModel.FilterFloorchecks(filterValue, filterColumns);
            
            return new EmptyResult();
        }

        public ActionResult ExportFloorcheckFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FloorcheckViewModel.FloorchecksFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Floorcheck", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFloorcheckFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = FloorcheckViewModel.FloorchecksFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Floorcheck", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
