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
        public DokumenteOhneDatenViewModel DokumenteOhneDatenViewModel { get { return GetViewModel<DokumenteOhneDatenViewModel>(); } }

        [CkgApplication]
        public ActionResult DokumenteOhneDaten()
        {
            DokumenteOhneDatenViewModel.LoadDokumenteOhneDaten(ModelState);

            return View(DokumenteOhneDatenViewModel);
        }

        [GridAction]
        public ActionResult DokumenteOhneDatenAjaxBinding()
        {
            return View(new GridModel(DokumenteOhneDatenViewModel.DokumenteOhneDatenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridDokumenteOhneDaten(string filterValue, string filterColumns)
        {
            DokumenteOhneDatenViewModel.FilterDokumenteOhneDaten(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportDokumenteOhneDatenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = DokumenteOhneDatenViewModel.DokumenteOhneDatenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("DokumenteOhneDaten", dt);

            return new EmptyResult();
        }

        public ActionResult ExportDokumenteOhneDatenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = DokumenteOhneDatenViewModel.DokumenteOhneDatenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("DokumenteOhneDaten", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
