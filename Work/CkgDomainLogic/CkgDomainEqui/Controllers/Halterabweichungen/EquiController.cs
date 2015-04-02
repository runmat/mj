using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Equi.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Equi-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class EquiController  
    {
        public HalterAbweichungenViewModel HalterAbweichungenViewModel { get { return GetViewModel<HalterAbweichungenViewModel>(); } }

        [CkgApplication]
        public ActionResult Halterabweichungen()
        {
            HalterAbweichungenViewModel.LoadHalterabweichungen(ModelState);

            return View(HalterAbweichungenViewModel);
        }

        [GridAction]
        public ActionResult HalterabweichungenAjaxBinding()
        {
            return View(new GridModel(HalterAbweichungenViewModel.GridItems));
        }

        [HttpPost]
        public ActionResult HalterabweichungenSelectAll()
        {
            return Json(HalterAbweichungenViewModel.GetFahrgestellnummern(), "text/plain");
        }

        [HttpPost]
        public ActionResult SaveHalterabweichungen(string selectedItems)
        {
            HalterAbweichungenViewModel.SaveHalterabweichungen(selectedItems, ModelState);

            return PartialView("Halterabweichungen/HalterabweichungenGrid", HalterAbweichungenViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridHalterabweichungen(string filterValue, string filterColumns)
        {
            HalterAbweichungenViewModel.FilterHalterabweichungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportHalterabweichungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = HalterAbweichungenViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Halterabweichungen", dt);

            return new EmptyResult();
        }

        public ActionResult ExportHalterabweichungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = HalterAbweichungenViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Halterabweichungen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
