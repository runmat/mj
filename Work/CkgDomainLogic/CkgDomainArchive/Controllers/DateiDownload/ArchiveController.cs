using System;
using System.IO;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Archive.ViewModels;
using CkgDomainLogic.General.Services;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class ArchiveController : CkgDomainController 
    {
        public DateiDownloadViewModel DateiDownloadViewModel { get { return GetViewModel<DateiDownloadViewModel>(); } }

        [CkgApplication]
        public ActionResult DateiDownload()
        {
            DateiDownloadViewModel.Init();

            return View(DateiDownloadViewModel);
        }

        [GridAction]
        public ActionResult DateiDownloadAjaxBinding()
        {
            return View(new GridModel(DateiDownloadViewModel.DateienFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridDateiDownload(string filterValue, string filterColumns)
        {
            DateiDownloadViewModel.FilterDateien(filterValue, filterColumns);

            return new EmptyResult();
        }

        public FileResult GetDatei(string dateiName)
        {
            var virtualFilePath = Path.Combine(DateiDownloadViewModel.Verzeichnis, dateiName);
            return File(virtualFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(virtualFilePath));
        }

        #region Export

        public ActionResult ExportDateiDownloadFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = DateiDownloadViewModel.DateienFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Documents, dt);

            return new EmptyResult();
        }

        public ActionResult ExportDateiDownloadFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = DateiDownloadViewModel.DateienFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Documents, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
