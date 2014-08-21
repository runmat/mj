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
        public HalterabweichungenViewModel HalterabweichungenViewModel { get { return GetViewModel<HalterabweichungenViewModel>(); } }

        [CkgApplication]
        public ActionResult Halterabweichungen()
        {
            HalterabweichungenViewModel.LoadHalterabweichungen(ModelState);

            return View(HalterabweichungenViewModel);
        }

        [GridAction]
        public ActionResult HalterabweichungenAjaxBinding()
        {
            return View(new GridModel(HalterabweichungenViewModel.GridItems));
        }

        [HttpPost]
        public ActionResult HalterabweichungenSelectAll()
        {
            return Json(HalterabweichungenViewModel.GetFahrgestellnummern(), "text/plain");
        }

        [HttpPost]
        public ActionResult SaveHalterabweichungen(string selectedItems)
        {
            HalterabweichungenViewModel.SaveHalterabweichungen(selectedItems, ModelState);

            return PartialView("Halterabweichungen/HalterabweichungenGrid", HalterabweichungenViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridHalterabweichungen(string filterValue, string filterColumns)
        {
            HalterabweichungenViewModel.FilterHalterabweichungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportHalterabweichungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = HalterabweichungenViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Halterabweichungen", dt);

            return new EmptyResult();
        }

        public ActionResult ExportHalterabweichungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = HalterabweichungenViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Halterabweichungen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
