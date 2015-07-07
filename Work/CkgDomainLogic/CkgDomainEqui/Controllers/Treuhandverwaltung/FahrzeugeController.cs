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
        public TreuhandverwaltungViewModel TreuhandverwaltungViewModel { get { return GetViewModel<TreuhandverwaltungViewModel>(); } }

        
        [CkgApplication]
        public ActionResult ReportTreuhandverwaltung(int? appIndex = 3)
        {
            _dataContextKey = typeof(TreuhandverwaltungViewModel).Name;
            TreuhandverwaltungViewModel.DataMarkForRefresh();
            TreuhandverwaltungViewModel.Init();
            

            switch (appIndex)
            { 
                case 1:
                    TreuhandverwaltungViewModel.TreuhandverwaltungSelektor.Reporttype = ReportType.TG;
                    break;
                case 2:
                    TreuhandverwaltungViewModel.TreuhandverwaltungSelektor.Reporttype = ReportType.AG;
                    break;
                case 3:
                    TreuhandverwaltungViewModel.TreuhandverwaltungSelektor.Reporttype = ReportType.Services;
                    break;
                default:
                     TreuhandverwaltungViewModel.TreuhandverwaltungSelektor.Reporttype = ReportType.TG;
                    break;

            }

            return View(TreuhandverwaltungViewModel);
        }

       
        [HttpPost]
        public ActionResult ExcelUpload()
        {
            _dataContextKey = typeof(TreuhandverwaltungViewModel).Name;
            TreuhandverwaltungViewModel.DataMarkForRefresh();

            return PartialView("Treuhandverwaltung/ExcelUpload", TreuhandverwaltungViewModel);
        }
        
        [HttpPost]
        public ActionResult ApplyKunde(string kunnr)
        {
            TreuhandverwaltungViewModel.LoadBerechtigungen(kunnr);

            return PartialView("Treuhandverwaltung/TreuhandverwaltungKundenAuswahl", TreuhandverwaltungViewModel.TreuhandverwaltungSelektor);
        }


        [HttpPost]
        public ActionResult LoadTreuhandverwaltungBestand(TreuhandverwaltungSelektor selector)
        {
            TreuhandverwaltungViewModel.TreuhandverwaltungSelektor = selector;

            if (ModelState.IsValid)
            {
                TreuhandverwaltungViewModel.LoadTreuhandbestand(selector);

                if (TreuhandverwaltungViewModel.Treuhandbestands.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }
            
            return PartialView("Treuhandverwaltung/TreuhandverwaltungReportsSuche", TreuhandverwaltungViewModel.TreuhandverwaltungSelektor);
        }

        [HttpPost]
        public ActionResult ShowTreuhandverwaltungBestand()
        {
            return PartialView("Treuhandverwaltung/TreuhandverwaltungGrid", TreuhandverwaltungViewModel);
        }

        [HttpPost]
        public ActionResult LoadTreuhandverwaltungFreigaben(TreuhandverwaltungSelektor selector)
        {
            TreuhandverwaltungViewModel.TreuhandverwaltungSelektor = selector;

            if (ModelState.IsValid)
            {
                TreuhandverwaltungViewModel.LoadTreuhandfreigabe(selector);

                if (selector.Sperraktion == SperrAktion.Freigeben && TreuhandverwaltungViewModel.Treuhandbestands.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Treuhandverwaltung/TreuhandverwaltungKundenAuswahl", TreuhandverwaltungViewModel.TreuhandverwaltungSelektor);
        }

        [HttpPost]
        public ActionResult ShowTreuhandverwaltungFreigaben()
        {
            return PartialView("Treuhandverwaltung/TreuhandverwaltungFreigabeGrid", TreuhandverwaltungViewModel);
        }

        [HttpPost]
        public ActionResult FahrzeugeFreigeben()
        {
            TreuhandverwaltungViewModel.FreigebenAblehnen(true, ModelState);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FreigabeAblehnen()
        {
            TreuhandverwaltungViewModel.FreigebenAblehnen(false, ModelState);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FahrzeugKommentar(string fin)
        {
            Treuhandbestand selectedFzg = TreuhandverwaltungViewModel.TreuhandbestandsFiltered.Where(x => x.Fahrgestellnummer == fin).ToList().FirstOrDefault();
            return PartialView("Treuhandverwaltung/TreuhandverwaltungFreigabeKommentar", selectedFzg);
        }

        [HttpPost]
        public ActionResult ModifyTreuhandverwaltungFreigabenGesperrte(Treuhandbestand bestand)
        {
            TreuhandverwaltungViewModel.ModifyModelAblehnungsgrund(bestand);
            return PartialView("Treuhandverwaltung/TreuhandverwaltungFreigabeGrid", TreuhandverwaltungViewModel);
        }

        [HttpPost]
        public ActionResult ShowTreuhandverwaltungFreigabenGesperrte()
        {            
            return PartialView("Treuhandverwaltung/TreuhandverwaltungFreigabeGrid", TreuhandverwaltungViewModel);
        }

        [HttpPost]
        public JsonResult FahrzeugAuswahlFreigabeSelectionChanged(string fin, bool isChecked)
        {
            int allSelectionCount, allCount = 0, allFoundCount = 0;
         
            if (String.IsNullOrEmpty(fin))
                TreuhandverwaltungViewModel.SelectFahrzeugeFreigabe(isChecked, out allSelectionCount, out allCount, out allFoundCount);
            else
                TreuhandverwaltungViewModel.SelectFahrzeugFreigabe(fin, isChecked, out allSelectionCount);
            
            return Json(new { allSelectionCount, allCount, allFoundCount });
        }


        [HttpPost]
        public ActionResult ShowReportsSuche(int reportType)
        {
            TreuhandverwaltungViewModel.TreuhandverwaltungSelektor.Reporttype = (reportType == 0) ? ReportType.TG : ReportType.AG;
            return PartialView("Treuhandverwaltung/TreuhandverwaltungReportsSuche", TreuhandverwaltungViewModel.TreuhandverwaltungSelektor);
        }

        [GridAction]
        public ActionResult TreuhandverwaltungAjaxBinding()
        {
            return View(new GridModel(TreuhandverwaltungViewModel.TreuhandbestandsFiltered));
        }
      
        [GridAction]
        public ActionResult TreuhandServicesAjaxBinding()
        {
            return View(new GridModel(TreuhandverwaltungViewModel.UploadItemsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridTreuhandverwaltung(string filterValue, string filterColumns)
        {
            TreuhandverwaltungViewModel.FilterTreuhandbestands(filterValue, filterColumns);

            return new EmptyResult();
        }

        public FileResult DownloadCsvTemplate()
        {
            var pfad = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Documents/Templates/")), TreuhandverwaltungViewModel.CsvTemplateFileName);
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, TreuhandverwaltungViewModel.CsvTemplateFileName);
        }


        [HttpPost]
        public ActionResult FilterGridTreuhandverwaltungFreigabe(string filterValue, string filterColumns)
        {
            TreuhandverwaltungViewModel.FilterTreuhandbestands(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Excel Upload

        [HttpPost]
        public JsonResult FahrzeugAuswahlFSSelectionChanged(string fin, bool isChecked)
        {
            int allSelectionCount, allCount = 0, allFoundCount = 0, itemsWithoutErrorOnly = 0;
            bool ret = false;
            if (String.IsNullOrEmpty(fin))
                ret = TreuhandverwaltungViewModel.SelectFahrzeuge(isChecked, out allSelectionCount, out allCount, out allFoundCount);
            else
                ret = TreuhandverwaltungViewModel.SelectFahrzeug(fin, isChecked, out allSelectionCount);

            itemsWithoutErrorOnly = ret == true ? 0 : 1;
            return Json(new { allSelectionCount, allCount, allFoundCount, itemsWithoutErrorOnly });
        }

        [HttpPost]
        public ActionResult ExcelUploadStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            // Step 1:  Upload the CSV file

            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = "Fehler: Keine Datei angegeben!" }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!TreuhandverwaltungViewModel.CsvUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = "Fehler: CSV Datei konnte nicht gespeichert werden!" }, "text/plain");

            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult ExcelUploadShowGrid(bool showErrorsOnly)
        {
            // Step 2:  Show CSV data in a grid for user validation

            TreuhandverwaltungViewModel.UploadItemsShowErrorsOnly = showErrorsOnly;

            return PartialView("Treuhandverwaltung/ExcelUpload/ValidationGrid", TreuhandverwaltungViewModel);
        }

        [HttpPost]
        public ActionResult ExcelUploadSubmit()
        {
            // Step 3:  Save CSV data to data store

            TreuhandverwaltungViewModel.SaveUploadItems(ModelState);

            return PartialView("Treuhandverwaltung/ExcelUpload/Receipt", TreuhandverwaltungViewModel);
        }

        #endregion

        #region Export

        public ActionResult ExportTreuhandverwaltungFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = TreuhandverwaltungViewModel.TreuhandbestandsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Treuhandverwaltung", dt);

            return new EmptyResult();
        }

        public ActionResult ExportTreuhandverwaltungFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = TreuhandverwaltungViewModel.TreuhandbestandsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Treuhandverwaltung", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
        
        #endregion
    }
}
