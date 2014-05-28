using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Archive.Models;
using CkgDomainLogic.Archive.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Leasing-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class ArchiveController : CkgDomainController 
    {
        public EasyAccessViewModel EasyViewModel { get { return GetViewModel<EasyAccessViewModel>(); } }

        public ActionResult OptischesArchiv()
        {
            return View(EasyViewModel);
        }

        [HttpPost]
        public ActionResult InitSearch()
        {
            EasyViewModel.LoadArchives();

            return PartialView("EasyAccess/EasyAccessSuche", EasyViewModel.GetSuchparameter(ModelState.AddModelError));
        }

        [HttpPost]
        public ActionResult RefreshSearch(string archiveType)
        {
            EasyViewModel.ApplyArchiveTypeAndLoadArchives(archiveType);

            return PartialView("EasyAccess/EasyAccessSuche", EasyViewModel.GetSuchparameter(ModelState.AddModelError));
        }

        [HttpPost]
        public ActionResult LoadDocuments(EasyAccessSuchparameter suchparameter)
        {
            EasyViewModel.ApplySelectionAndLoadDocuments(suchparameter);

            return PartialView("EasyAccess/EasyAccessSuche", EasyViewModel.GetSuchparameter(ModelState.AddModelError));
        }

        [HttpPost]
        public ActionResult ShowDocuments()
        {
            return PartialView("EasyAccess/EasyAccessGrid", EasyViewModel.DocumentsFiltered);
        }

        [GridAction]
        public ActionResult DocumentsAjaxBinding()
        {
            return View(new GridModel(EasyViewModel.DocumentsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridEasyAccess(string filterValue, string filterColumns)
        {
            EasyViewModel.FilterDocuments(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportDocumentsFilteredExcel(int page, string orderBy, string filterBy, string jsonColumns)
        {
            var dt = EasyViewModel.DocumentsFiltered;
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Documents", dt);

            return new EmptyResult();
        }

        public ActionResult ExportDocumentsFilteredPDF(int page, string orderBy, string filterBy, string jsonColumns)
        {
            var dt = EasyViewModel.DocumentsFiltered;
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Documents", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        public ActionResult DocumentView(string docId)
        {
            string filePath = EasyViewModel.ViewDocument(docId);

            return File(filePath, "application/pdf", docId + ".pdf");
        }

        [HttpPost]
        public ActionResult DocumentDetails(string docId)
        {
            EasyAccessDetail item = EasyViewModel.GetDocumentDetail(docId);

            return PartialView("EasyAccess/EasyAccessDetails", item);
        }

    }
}
