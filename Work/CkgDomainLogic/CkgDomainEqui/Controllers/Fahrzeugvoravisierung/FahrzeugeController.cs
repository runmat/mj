using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using MvcTools.Web;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using DocumentTools.Services;


namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Fahrzeug-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FahrzeugeController : CkgDomainController
    {
        public FahrzeugvoravisierungViewModel FahrzeugvoravisierungViewModel { get { return GetViewModel<FahrzeugvoravisierungViewModel>(); } }

                
        [CkgApplication]
        public ActionResult FahrzeugvoravisierungExcelUpload()
        {
            _dataContextKey = typeof(FahrzeugvoravisierungViewModel).Name;           
            FahrzeugvoravisierungViewModel.Init();

            return View(FahrzeugvoravisierungViewModel);
        }
                                                                          
        [HttpPost]
        public ActionResult ShowFahrzeugvoravisierungSuche(FahrzeugvoravisierungSelektor selector)
        {
            FahrzeugvoravisierungViewModel.FahrzeugvoravisierungSelektor = selector;
            return PartialView("Fahrzeugvoravisierung/FahrzeugvoravisierungSuche", FahrzeugvoravisierungViewModel.FahrzeugvoravisierungSelektor);
        }
             
        [GridAction]
        public ActionResult FahrzeugvoravisierungAjaxBinding()
        {
            return View(new GridModel(FahrzeugvoravisierungViewModel.UploadItemsFiltered));
        }

        [HttpPost]
        public ActionResult FilterFahrzeugvoravisierungGrid(string filterValue, string filterColumns)
        {
            FahrzeugvoravisierungViewModel.FilterFahrzeugvoravisierungUploadModels(filterValue, filterColumns);

            return new EmptyResult();
        }

               
        #region Excel Upload

        public FileResult DownloadCsvTemplateVW()
        {
            var pfad = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Documents/Templates/")), FahrzeugvoravisierungViewModel.CsvTemplateVWFileName);
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, FahrzeugvoravisierungViewModel.CsvTemplateVWFileName);
        }

        public FileResult DownloadCsvTemplateOthers()
        {
            var pfad = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Documents/Templates/")), FahrzeugvoravisierungViewModel.CsvTemplateOthersFileName);
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, FahrzeugvoravisierungViewModel.CsvTemplateOthersFileName);
        }


        [HttpPost]
        public JsonResult FahrzeugAuswahlAvisierungSelectionChanged(string fin, bool isChecked)
        {
            int allSelectionCount, allCount = 0, allFoundCount = 0, itemsWithoutErrorOnly = 0;
            bool ret = false;
            if (String.IsNullOrEmpty(fin))
                ret = FahrzeugvoravisierungViewModel.SelectFahrzeuge(isChecked, out allSelectionCount, out allCount, out allFoundCount);
            else
                ret = FahrzeugvoravisierungViewModel.SelectFahrzeug(fin, isChecked, out allSelectionCount);

            itemsWithoutErrorOnly = ret == true ? 0 : 1;
            return Json(new { allSelectionCount, allCount, allFoundCount, itemsWithoutErrorOnly });
        }

        [HttpPost]
        public ActionResult ExcelUploadAvisierungStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            // Step 1:  Upload the CSV file
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.Error + ": " + Localize.FileUploadNoFileAssignedWarning }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.First();

            if (!FahrzeugvoravisierungViewModel.CsvUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = Localize.Error + ": " + Localize.FileUploadCouldNotSaveWarning }, "text/plain");

            return Json(new { success = true, message = "ok", uploadFileName = file.FileName, }, "text/plain");
        }

        [HttpPost]
        public ActionResult ExcelUploadAvisierungShowGrid(bool showErrorsOnly)
        {
            // Step 2:  Show CSV data in a grid for user validation

            FahrzeugvoravisierungViewModel.UploadItemsShowErrorsOnly = showErrorsOnly;

            FahrzeugvoravisierungViewModel.SaveUploadItems();

            return PartialView("Fahrzeugvoravisierung/ExcelUpload/ValidationGrid", FahrzeugvoravisierungViewModel);
        }

        [HttpPost]
        public ActionResult ExcelUploadAvisierungSubmit()
        {
            // Step 3:  Save CSV data to data store

            //FahrzeugvoravisierungViewModel.SaveUploadItems();

            return PartialView("Fahrzeugvoravisierung/ExcelUpload/Receipt", FahrzeugvoravisierungViewModel);
        }

        #endregion

        #region Export

        public ActionResult ExportFahrzeugvoravisierungFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugvoravisierungViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Fahrzeugvoravisierung", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugvoravisierungFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugvoravisierungViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Fahrzeugvoravisierung", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
        
        #endregion
    }
}
