using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Insurance.Models;
using CkgDomainLogic.Insurance.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class InsuranceController
    {
        public UploadBestandsdatenViewModel UploadBestandsdatenViewModel { get { return GetViewModel<UploadBestandsdatenViewModel>(); } }

        [CkgApplication]
        public ActionResult UploadBestandsdaten()
        {
            UploadBestandsdatenViewModel.DataMarkForRefresh();

            return View(UploadBestandsdatenViewModel);
        }

        #region CsvUpload

        [HttpPost]
        public ActionResult UploadBestandsdatenStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = "Fehler: Keine Datei angegeben!" }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!UploadBestandsdatenViewModel.CsvUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = "Fehler: CSV Datei konnte nicht gespeichert werden!" }, "text/plain");


            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult UploadBestandsdatenShowGrid()
        {
            return PartialView("UploadBestandsdaten/UploadBestandsdatenGrid", UploadBestandsdatenViewModel);
        }

        [GridAction]
        public ActionResult UploadBestandsdatenAjaxBinding()
        {
            return View(new GridModel(UploadBestandsdatenViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadSaveBestandsdatenAjaxBinding()
        {
            return View(new GridModel(UploadBestandsdatenViewModel.UploadItemsFiltered));
        }

        [GridAction]
        public ActionResult UploadBestandsdatenAjaxUpdateItem(int DatensatzNr)
        {
            var item = UploadBestandsdatenViewModel.GetDatensatzById(DatensatzNr);
            if (TryUpdateModel(item))
            {
                UploadBestandsdatenViewModel.ApplyChangedData(item);
            }

            return View(new GridModel(UploadBestandsdatenViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadBestandsdatenAjaxDeleteItem(int DatensatzNr)
        {
            UploadBestandsdatenViewModel.RemoveDatensatzById(DatensatzNr);

            return View(new GridModel(UploadBestandsdatenViewModel.UploadItems));
        }

        [HttpPost]
        public ActionResult UploadBestandsdatenCheck()
        {
            UploadBestandsdatenViewModel.ValidateUploadItems();

            return PartialView("UploadBestandsdaten/UploadBestandsdatenGrid", UploadBestandsdatenViewModel);
        }

        [HttpPost]
        public ActionResult UploadBestandsdatenResetSubmitMode()
        {
            UploadBestandsdatenViewModel.ResetSubmitMode();

            return PartialView("UploadBestandsdaten/UploadBestandsdatenGrid", UploadBestandsdatenViewModel);
        }

        [HttpPost]
        public ActionResult UploadBestandsdatenSubmit()
        {
            UploadBestandsdatenViewModel.SaveUploadItems();

            return PartialView("UploadBestandsdaten/Receipt", UploadBestandsdatenViewModel);
        }

        public FileResult DownloadBestandsdatenCsvTemplate()
        {
            var pfad = Server.MapPath(Url.Content("/ServicesMvc/Documents/Templates/UploadBestandsdaten.csv"));
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, "UploadBestandsdaten.csv");
        }

        [HttpPost]
        public ActionResult FilterGridUploadSaveBestandsdaten(string filterValue, string filterColumns)
        {
            UploadBestandsdatenViewModel.FilterUploadItems(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        #region Export

        public ActionResult ExportUploadSaveBestandsdatenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = UploadBestandsdatenViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("UploadBestandsdaten", dt);

            return new EmptyResult();
        }

        public ActionResult ExportUploadSaveBestandsdatenFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = UploadBestandsdatenViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("UploadBestandsdaten", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
