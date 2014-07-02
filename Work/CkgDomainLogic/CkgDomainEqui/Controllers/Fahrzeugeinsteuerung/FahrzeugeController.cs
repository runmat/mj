using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using MvcTools.Web;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController : CkgDomainController
    {
        public UploadFahrzeugeinsteuerungViewModel UploadFahrzeugeinsteuerungViewModel { get { return GetViewModel<UploadFahrzeugeinsteuerungViewModel>(); } }

        [CkgApplication]
        public ActionResult UploadFahrzeugeinsteuerung()
        {
            UploadFahrzeugeinsteuerungViewModel.DataMarkForRefresh();

            return View(UploadFahrzeugeinsteuerungViewModel);
        }

        #region CsvUpload

        [HttpPost]
        public ActionResult UploadFahrzeugeinsteuerungStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = "Fehler: Keine Datei angegeben!" }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!UploadFahrzeugeinsteuerungViewModel.CsvUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = "Fehler: CSV Datei konnte nicht gespeichert werden!" }, "text/plain");


            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult UploadFahrzeugeinsteuerungShowGrid()
        {
            return PartialView("Fahrzeugeinsteuerung/UploadFahrzeugeinsteuerungGrid", UploadFahrzeugeinsteuerungViewModel);
        }

        [GridAction]
        public ActionResult UploadFahrzeugeinsteuerungAjaxBinding()
        {
            return View(new GridModel(UploadFahrzeugeinsteuerungViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadSaveFahrzeugeinsteuerungAjaxBinding()
        {
            return View(new GridModel(UploadFahrzeugeinsteuerungViewModel.UploadItemsFiltered));
        }

        [HttpPost]
        public ActionResult UploadFahrzeugeinsteuerungSubmit()
        {
            UploadFahrzeugeinsteuerungViewModel.SaveUploadItems();

            return PartialView("Fahrzeugeinsteuerung/Receipt", UploadFahrzeugeinsteuerungViewModel);
        }

        public FileResult DownloadFahrzeugeinsteuerungCsvTemplate()
        {
            var pfad = Server.MapPath(Url.Content("/ServicesMvc/Documents/Templates/UploadFahrzeugeinsteuerung.csv"));
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, "UploadFahrzeugeinsteuerung.csv");
        }

        [HttpPost]
        public ActionResult FilterGridUploadSaveFahrzeugeinsteuerung(string filterValue, string filterColumns)
        {
            UploadFahrzeugeinsteuerungViewModel.FilterUploadItems(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        #region Export

        public ActionResult ExportUploadSaveFahrzeugeinsteuerungFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = UploadFahrzeugeinsteuerungViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("UploadFahrzeugeinsteuerung", dt);

            return new EmptyResult();
        }

        public ActionResult ExportUploadSaveFahrzeugeinsteuerungFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = UploadFahrzeugeinsteuerungViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("UploadFahrzeugeinsteuerung", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
