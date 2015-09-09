using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController
    {
        public UploadAvislisteViewModel UploadAvislisteViewModel { get { return GetViewModel<UploadAvislisteViewModel>(); } }

        [CkgApplication]
        public ActionResult UploadAvisliste()
        {
            UploadAvislisteViewModel.DataMarkForRefresh();

            return View(UploadAvislisteViewModel);
        }

        #region CsvUpload

        [HttpPost]
        public ActionResult UploadAvislisteStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!UploadAvislisteViewModel.ExcelUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = Localize.ErrorFileCouldNotBeSaved }, "text/plain");


            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult UploadAvislisteShowGrid()
        {
            return PartialView("UploadAvisliste/Grid", UploadAvislisteViewModel);
        }

        [GridAction]
        public ActionResult UploadAvislisteAjaxBinding()
        {
            return View(new GridModel(UploadAvislisteViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadSaveAvislisteAjaxBinding()
        {
            return View(new GridModel(UploadAvislisteViewModel.UploadItemsFiltered));
        }

        [GridAction]
        public ActionResult UploadAvislisteAjaxUpdateItem(int DatensatzNr)
        {
            var item = UploadAvislisteViewModel.GetDatensatzById(DatensatzNr);
            if (TryUpdateModel(item))
            {
                UploadAvislisteViewModel.ApplyChangedData(item);
            }

            return View(new GridModel(UploadAvislisteViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadAvislisteAjaxDeleteItem(int DatensatzNr)
        {
            UploadAvislisteViewModel.RemoveDatensatzById(DatensatzNr);

            return View(new GridModel(UploadAvislisteViewModel.UploadItems));
        }

        [HttpPost]
        public ActionResult UploadAvislisteCheck()
        {
            UploadAvislisteViewModel.ValidateUploadItems();

            return PartialView("UploadAvisliste/UploadGrid", UploadAvislisteViewModel);
        }

        [HttpPost]
        public ActionResult UploadAvislisteResetSubmitMode()
        {
            UploadAvislisteViewModel.ResetSubmitMode();

            return PartialView("UploadAvisliste/UploadGrid", UploadAvislisteViewModel);
        }

        [HttpPost]
        public ActionResult UploadAvislisteSubmit()
        {
            UploadAvislisteViewModel.SaveUploadItems();

            return PartialView("UploadAvisliste/Receipt", UploadAvislisteViewModel);
        }

        public FileResult DownloadAvislisteCsvTemplate()
        {
            var pfad = Server.MapPath(Url.Content("/ServicesMvc/Documents/Templates/UploadAvisliste.csv"));
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, "UploadAvisliste.csv");
        }

        public FileResult DownloadAvislisteXlsTemplate()
        {
            var pfad = Server.MapPath(Url.Content("/ServicesMvc/Documents/Templates/UploadAvisliste.xls"));
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, "UploadAvisliste.xls");
        }

        [HttpPost]
        public ActionResult FilterGridUploadSaveAvisliste(string filterValue, string filterColumns)
        {
            UploadAvislisteViewModel.FilterUploadItems(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        #region Export

        public ActionResult ExportUploadSaveAvislisteFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = UploadAvislisteViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("UploadAvisliste", dt);

            return new EmptyResult();
        }

        public ActionResult ExportUploadSaveAvislisteFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = UploadAvislisteViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("UploadAvisliste", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
