using System;
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
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.Equi.ViewModels;


namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController : CkgDomainController
    {
        public FahrzeuguebersichtViewModel FahrzeuguebersichtViewModel { get { return GetViewModel<FahrzeuguebersichtViewModel>(); } }

        public EquiHistorieVermieterViewModel EquipmentHistorieVermieterViewModel { get { return GetViewModel<EquiHistorieVermieterViewModel>(); } }

      
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

            if (ModelState.IsValid && !PersistableMode)
            {
                FahrzeuguebersichtViewModel.LoadFahrzeuguebersicht();
                if (FahrzeuguebersichtViewModel.Fahrzeuguebersichts.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PersistablePartialView("Fahrzeuguebersicht/FahrzeuguebersichtSuche", FahrzeuguebersichtViewModel.FahrzeuguebersichtSelektor);
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


        #region History


        [HttpPost]
        public ActionResult ShowHistory(string fin)
        {                           
            EquiHistorieSuchparameter model = new EquiHistorieSuchparameter();
            model.FahrgestellNr = fin;
            EquipmentHistorieVermieterViewModel.LoadHistorieInfos(ref model, ModelState);

            if (EquipmentHistorieVermieterViewModel.HistorieInfos != null)
                EquipmentHistorieVermieterViewModel.LoadHistorie(EquipmentHistorieVermieterViewModel.HistorieInfos[0].EquipmentNr, null);
            else
                ModelState.AddModelError(string.Empty, Localize.NoDataFound);

            return PartialView("Historie/HistorieVermieterDetail", EquipmentHistorieVermieterViewModel.EquipmentHistorie);            
        }


         [HttpPost]
        public ActionResult GetFahrzeugHistorieVermieterPartial(string equiNr, string meldungsNr)
        {
            EquipmentHistorieVermieterViewModel.LoadHistorie(equiNr, meldungsNr);

            return PartialView("Historie/HistorieVermieterDetail", EquipmentHistorieVermieterViewModel.EquipmentHistorie);
        }

       
        [HttpPost]
        public ActionResult ShowHistorieVermieterEquis()
        {
            return PartialView("Historie/HistorieVermieterGrid", EquipmentHistorieVermieterViewModel);
        }

        [GridAction]
        public ActionResult EquiHistorieInfosVermieterAjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieVermieterViewModel.HistorieInfosFiltered));
        }

        [GridAction]
        public ActionResult EquiHistorieVermieterLebenslaufZb2AjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieVermieterViewModel.EquipmentHistorie.LebenslaufZb2));
        }

        [GridAction]
        public ActionResult EquiHistorieVermieterLebenslaufFsmAjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieVermieterViewModel.EquipmentHistorie.LebenslaufFsm));
        }

        [GridAction]
        public ActionResult EquiHistorieVermieterTueteninhaltFsmAjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieVermieterViewModel.EquipmentHistorie.InhalteFsm));
        }

        public FileContentResult FahrzeughistorieVermieterPdf()
        {
            var formularPdfBytes = EquipmentHistorieVermieterViewModel.GetHistorieAsPdf();

            return new FileContentResult(formularPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}_{1}.pdf", Localize.VehicleHistory, EquipmentHistorieVermieterViewModel.EquipmentHistorie.HistorieInfo.FahrgestellNr) };
        }

        [HttpPost]
        public ActionResult FilterGridEquiHistorieInfosVermieter(string filterValue, string filterColumns)
        {
            EquipmentHistorieVermieterViewModel.FilterHistorieInfos(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportEquiHistorieInfosVermieterFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = EquipmentHistorieVermieterViewModel.HistorieInfosFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.VehicleHistory, dt);

            return new EmptyResult();
        }

        public ActionResult ExportEquiHistorieInfosVermieterFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = EquipmentHistorieVermieterViewModel.HistorieInfosFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.VehicleHistory, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

    } // class
            
} // ns
