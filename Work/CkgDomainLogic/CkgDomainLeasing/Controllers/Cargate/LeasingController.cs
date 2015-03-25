using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;


namespace ServicesMvc.Controllers
{
    public partial class LeasingController
    {

        [CkgApplication]
        public ActionResult CsvUpload()
        {
            LeasingCargateCsvUploadViewModel.DataMarkForRefresh();

            return View(LeasingCargateCsvUploadViewModel);
        }

        [CkgApplication]
        public ActionResult DisplayCargate()
        {
            LeasingCargateCsvUploadViewModel.DataMarkForRefresh();
            return View();
        }

        #region CsvUpload

        [HttpPost]
        public ActionResult CsvUploadStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            // Step 1:  Upload the CSV file

            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = "Fehler: Keine Datei angegeben!" }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!LeasingCargateCsvUploadViewModel.CsvUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = "Fehler: CSV Datei konnte nicht gespeichert werden!" }, "text/plain");


            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult CsvUploadShowGrid(bool showErrorsOnly)
        {
            // Step 2:  Show CSV data in a grid for user validation

            LeasingCargateCsvUploadViewModel.UploadItemsShowErrorsOnly = showErrorsOnly;

            return PartialView("CsvUpload/ValidationGrid", LeasingCargateCsvUploadViewModel);
        }

        [GridAction]
        public ActionResult LeasingCargateCsvUploadAjaxSelect()
        {
            return View(new GridModel( LeasingCargateCsvUploadViewModel.UploadItems));
        }

        [HttpPost]
        public ActionResult CsvUploadSubmit()
        {
            // Step 3:  Save CSV data to data store

            LeasingCargateCsvUploadViewModel.SaveUploadItems();

            return PartialView("CsvUpload/Receipt", LeasingCargateCsvUploadViewModel);
        }

        public ActionResult CsvTemplate()
        {
            return PartialView("CsvUpload/CsvTemplate");
        }

        #endregion

        #region Grid Anzeige

        [GridAction]
        public ActionResult DisplayCargateAjaxBinding()
        {
            return View(new GridModel(LeasingCargateCsvUploadViewModel.LeasingCargateDisplayListItemsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridCargateAnzeige(string filterValue, string filterColumns)
        {
            LeasingCargateCsvUploadViewModel.FilterCargateDisplayItems(filterValue, filterColumns);

            return PartialView("Partial/DisplayCargateGrid", null);
        }

        #endregion

        #region Export

        public ActionResult ExportCsvUploadFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = LeasingCargateCsvUploadViewModel.LeasingCargateDisplayListItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("CarGateCsvUpload", dt);

            return new EmptyResult();
        }

        public ActionResult ExportCsvUploadFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = LeasingCargateCsvUploadViewModel.LeasingCargateDisplayListItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("CarGateCsvUpload", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
