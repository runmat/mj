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
        public DatenOhneDokumenteViewModel DatenOhneDokumenteViewModel { get { return GetViewModel<DatenOhneDokumenteViewModel>(); } }

        [CkgApplication]
        public ActionResult DatenOhneDokumente()
        {
            DatenOhneDokumenteViewModel.LoadDatenOhneDokumente(ModelState);

            return View(DatenOhneDokumenteViewModel);
        }

        [GridAction]
        public ActionResult DatenOhneDokumenteAjaxBinding()
        {
            var items = DatenOhneDokumenteViewModel.GridItems; 

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterDatenOhneDokumenteData(string Selektionsfilter)
        {
            DatenOhneDokumenteViewModel.ApplyDatenfilter(Selektionsfilter);
            DatenOhneDokumenteViewModel.MarkForRefreshDatenOhneDokumenteFiltered();

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveDatenOhneDokumente(string mode, string selectedItems)
        {
            DatenOhneDokumenteViewModel.SaveDatenOhneDokumente(mode, selectedItems, ModelState);

            return PartialView("DatenOhneDokumente/DatenOhneDokumenteGrid", DatenOhneDokumenteViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridDatenOhneDokumente(string filterValue, string filterColumns)
        {
            DatenOhneDokumenteViewModel.FilterDatenOhneDokumente(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportDatenOhneDokumenteFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = DatenOhneDokumenteViewModel.DatenOhneDokumenteFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("DatenOhneDokumente", dt);

            return new EmptyResult();
        }

        public ActionResult ExportDatenOhneDokumenteFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = DatenOhneDokumenteViewModel.DatenOhneDokumenteFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("DatenOhneDokumente", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
