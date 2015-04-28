using System.Collections;
using System.Web.Mvc;
using System.Web;
using System.Linq;
using MvcTools.Web;
using System.Collections.Generic;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController : CkgDomainController
    {
        public FahrzeuguebersichtViewModel FahrzeuguebersichtViewModel { get { return GetViewModel<FahrzeuguebersichtViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportFahrzeuguebersicht()
        {
            _dataContextKey = typeof(FahrzeuguebersichtViewModel).Name;
            FahrzeuguebersichtViewModel.DataInit();
            FahrzeuguebersichtViewModel.Init();

            return View(FahrzeuguebersichtViewModel);
        }

        [HttpPost]
        public ActionResult LoadFahrzeuguebersicht(FahrzeuguebersichtSelektor model)
        {
            FahrzeuguebersichtViewModel.FahrzeuguebersichtSelektor = model;

            //FahrzeuguebersichtViewModel.Validate(AddModelError);

            if (ModelState.IsValid)
            {
                FahrzeuguebersichtViewModel.LoadFahrzeuguebersicht();
                if (FahrzeuguebersichtViewModel.Fahrzeuguebersichts.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Fahrzeuguebersicht/FahrzeuguebersichtSuche", FahrzeuguebersichtViewModel.FahrzeuguebersichtSelektor);
        }

        [HttpPost]
        public ActionResult ShowFahrzeuguebersicht()
        {
            return PartialView("Fahrzeuguebersicht/FahrzeuguebersichtGrid", FahrzeuguebersichtViewModel);
        }

        [GridAction]
        public ActionResult FahrzeuguebersichtAjaxBinding()
        {
            return View(new GridModel(FahrzeuguebersichtViewModel.FahrzeuguebersichtsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeuguebersicht(string filterValue, string filterColumns)
        {
            FahrzeuguebersichtViewModel.FilterFahrzeuguebersicht(filterValue, filterColumns);

            return new EmptyResult();
        }


        #region Excel Upload

       
        [HttpPost]
        public ActionResult ExcelUploadFahrzeuguebersichtStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            // Step 1:  Upload the CSV file

            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = "Fehler: Keine Datei angegeben!" }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!FahrzeuguebersichtViewModel.CsvUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = "Fehler: CSV Datei konnte nicht gespeichert werden!" }, "text/plain");

            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult ExcelUploadFahrzeuguebersichtShowGrid()
        {
            // Step 2:  Show filter results
        
            //FahrzeuguebersichtViewModel.ApplyFilter(); 

            return PartialView("Fahrzeuguebersicht/FahrzeuguebersichtGrid", FahrzeuguebersichtViewModel);
        }

        //[HttpPost]
        //public ActionResult ExcelUploadFahrzeuguebersichtSubmit()
        //{
        //    // Step 3:  Save CSV data to data store

        //    // -> TODO -> wird das gebraucht? 

        //    return PartialView("Fahrzeuguebersicht/ExcelUpload/Receipt", FahrzeuguebersichtViewModel);
        //}

        #endregion


       
        #region Export
       
        public ActionResult ExportFahrzeuguebersichtFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeuguebersichtViewModel.FahrzeuguebersichtsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.RegistrationRequests, dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeuguebersichtFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeuguebersichtViewModel.FahrzeuguebersichtsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.RegistrationRequests, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
