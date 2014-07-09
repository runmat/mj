using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Leasing.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Leasing-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class LeasingController  
    {
        public LeasingSicherungsscheineViewModel SicherungsscheineViewModel { get { return GetViewModel<LeasingSicherungsscheineViewModel>(); } }

        [CkgApplication]
        public ActionResult BestandSicherungsscheine()
        {
            SicherungsscheineViewModel.LoadSicherungsscheine();

            return View(SicherungsscheineViewModel);
        }

        [GridAction]
        public ActionResult SicherungsscheineAjaxBinding()
        {
            var items = SicherungsscheineViewModel.SicherungsscheineFiltered; 

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridSicherungsscheine(string filterValue, string filterColumns)
        {
            SicherungsscheineViewModel.FilterSicherungsscheine(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportSicherungsscheineFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = SicherungsscheineViewModel.SicherungsscheineFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Sicherungsscheine", dt);

            return new EmptyResult();
        }

        public ActionResult ExportSicherungsscheineFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = SicherungsscheineViewModel.SicherungsscheineFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Sicherungsscheine", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
