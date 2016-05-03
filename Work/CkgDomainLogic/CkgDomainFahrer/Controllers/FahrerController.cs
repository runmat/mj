using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.Fahrer.Models;
using CkgDomainLogic.Fahrer.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrer.Contracts;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public class FahrerController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<FahrerViewModel>(); } }

        public FahrerViewModel ViewModel { get { return GetViewModel<FahrerViewModel>(); } }


        public FahrerController(IAppSettings appSettings, ILogonContextDataService logonContext, IFahrerDataService fahrerDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, fahrerDataService);
            InitModelStatics();
        }

        void InitModelStatics()
        {
            ProtokollEditModel.GetViewModel = GetViewModel<FahrerViewModel>;
        }


        [CkgApplication]
        public ActionResult Index()
        {
            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult Meldung()
        {
            ViewModel.LoadFahrerTagBelegungen();

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult Auftraege()
        {
            ViewModel.LoadFahrerAuftraege("NEW");

            FahrerAuftrag.AuftragsDetailsTemplate = auftrag => this.RenderPartialViewToString("Partial/Auftraege/AuftraegeGridAuftragsDetails", auftrag);
            FahrerAuftrag.AuftragsCommandTemplate = auftrag => this.RenderPartialViewToString("Partial/Auftraege/AuftraegeGridAuftragsCommandBar", auftrag);

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult FotoUpload(string modeProtokoll)
        {
            ViewModel.SetParamProtokollMode(modeProtokoll);

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult ProtokollUpload()
        {
            return FotoUpload(modeProtokoll: "1");
        }

        [CkgApplication]
        public ActionResult Auftragsauswahl()
        {
            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult QmReport()
        {
            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult ProtokollArchivierung()
        {
            ViewModel.SetParamProtokollMode("1");
            ViewModel.LoadFahrerProtokolle();

            return View(ViewModel);
        }



        [HttpPost]
        public JsonResult LoadFahrerAuftragsFahrten()
        {
            var errorMessage = ViewModel.LoadFahrerAuftragsFahrten().NotNullOrEmpty();

            return Json(new
            {
                errorMessage,
                fahrtKeys = string.Join("~", ViewModel.FahrerAuftragsFahrten.Select(f => f.UniqueKey)),
                fahrtNamen = string.Join("~", ViewModel.FahrerAuftragsFahrten.Select(f => f.AuftragsDetails)),
            });
        }

        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.FahrerAuftraegeFiltered;
        }

        #endregion


        #region Verfügbarkeitsmeldung

        [HttpPost]
        public ActionResult FahrerMeldungFormSave(FahrerBelegungViewModel model)
        {
            ModelState.Clear();

            if (ModelState.IsValid)
            {
            }

            return PartialView("Partial/Meldung/FahrerMeldungForm", ViewModel.FahrerBelegung);
        }

        [HttpPost]
        public ActionResult SaveSelectedBelegungen(string model, int fahrerAnzahl, string comment, string belegungsType)
        {
            var modelObject = JSon.Deserialize<FahrerBelegungViewModel>(model);

            ViewModel.FahrerBelegung.FahrerTagBelegungen = modelObject.FahrerTagBelegungen;
            ViewModel.FahrerBelegung.FahrerAnzahl = fahrerAnzahl;
            ViewModel.FahrerBelegung.Kommentar = comment;
            ViewModel.FahrerBelegung.BelegungsTyp = (FahrerTagBelegungsTyp)Enum.Parse(typeof(FahrerTagBelegungsTyp), belegungsType);

            ViewModel.SaveFahrerTagBelegungen();

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult FahrerMeldungenDownloadExcel()
        {
            var dt = ViewModel.ExcelDownloadFahrerMeldungenData.GetGridFilteredDataTable(ViewModel.ExcelDownloadFahrerMeldungenJsonColumns);
                
            var excelFileName = string.Format("Meldungen_Fahrer_{0}", ViewModel.DataService.FahrerID);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(excelFileName, dt);

            return new EmptyResult();
        }

        #endregion


        #region Fahrer Aufträge

        [GridAction]
        public ActionResult FahrerAuftraegeAjaxBinding()
        {
            return View(new GridModel(ViewModel.FahrerAuftraegeFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridAuftraege(string filterValue, string filterColumns)
        {
            ViewModel.FilterFahrerAuftraege(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult GetAuftragsPdfBytes(string auftragsNr)
        {
            var pdfBytes = ViewModel.GetAuftragsPdfBytes(auftragsNr);

            if (pdfBytes == null)
                return new ContentResult { Content = Localize.PdfFileCouldNotBeGenerated };

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = string.Format("{0}.pdf", auftragsNr) };
        }

        [HttpPost]
        public ActionResult SetFahrerAuftragsStatus(string auftragsNr, string status)
        {
            return Json(new { message = ViewModel.SetFahrerAuftragsStatus(auftragsNr, status) });
        }

        [HttpPost]
        public ActionResult SaveFahrerAuftragsStatusFilter(string status)
        {
            ViewModel.LoadFahrerAuftraege(status);

            return new EmptyResult();
        }

        #endregion


        #region Foto / Protokoll Upload

        [HttpPost]
        public ActionResult SetSelectedFahrerAuftragsKey(string auftragsKey)
        {
            ViewModel.SetSelectedFahrerAuftragsKey(auftragsKey);

            return Json(new { selectedAuftragsKey = ViewModel.SelectedFahrerAuftrag.AuftragsDetails });
        }

        [HttpPost]
        public ActionResult SetProtokollHasMultipleImages(bool check)
        {
            ViewModel.SetProtokollHasMultipleImages(check);

            return new EmptyResult();
        }
        
        [HttpPost]
        public ActionResult UploadImage(IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null || files.None())
                return new EmptyResult();

            var file = files.First();
            var res = ViewModel.SaveUploadedImageFile(file.FileName, file.SaveAs);

            if (res)
            {
                return Json(new
                {
                    files = new object[] { new { url = "-" } }
                });
            }
            else
            {
                return Json(new
                {
                    files = new object[] { new { error = "Das Bild konnte nicht gespeichert werde. Bitte erneut versuchen oder ein anderes Bild wählen." } }
                });
            }
        }

        public void GetUploadedImageFiles()
        {
            ViewModel.GetUploadedImageFiles();
        }

        [HttpPost]
        public ActionResult GetUploadedImageFilesPartial()
        {
            var subDir = (ViewModel.ModeProtokoll ? "ProtokollUpload" : "FotoUpload");
            return PartialView("Partial/Upload/" + subDir + "/UploadEdit", ViewModel);
        }

        [HttpPost]
        public ActionResult DeleteUploadedImage(string imageFileName)
        {
            var success = ViewModel.DeleteUploadedImage(imageFileName);

            return Json(new { success  });
        }

        [HttpPost]
        public ActionResult ProtokollCreateAndShowPdf()
        {
            var success = ViewModel.ProtokollCreateAndShowPdf();

            return Json(new { success });
        }

        [HttpPost]
        public ActionResult ProtokollDeleteUploadedImagesAndPdf()
        {
            var success = ViewModel.ProtokollDeleteUploadedImagesAndPdf();

            return Json(new { success });
        }

        [HttpPost]
        public ActionResult ProtokollTryLoadSonstigenAuftrag(string auftragsnr, string fahrtTyp)
        {
            var success = ViewModel.ProtokollTryLoadSonstigenAuftrag(auftragsnr, fahrtTyp);

            return Json(new { success });
        }

        public FileContentResult ProtokollDownloadPdf()
        {
            var pdfFilePath = ViewModel.ProtokollGetFullPdfFilePath();
            var pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = ViewModel.GetUploadedPdfFileName() };
        }
        #endregion


        #region Monitor / QM Auswertung

        [HttpPost]
        public ActionResult QmReportSearch(QmSelektor model)
        {
            ViewModel.QmSelektor = model;

            ViewModel.Validate(ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                if (!ViewModel.LoadQmData())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/QmReport/Filter", ViewModel.QmSelektor);
        }

        [HttpPost]
        public ActionResult QmReportShow()
        {
            return PartialView("Partial/QmReport/Results", ViewModel);
        }

        #endregion


        #region Protokollarchivierung

        [GridAction]
        public ActionResult FahrerProtokolleAjaxBinding()
        {
            return View(new GridModel(ViewModel.FahrerProtokolleFiltered));
        }

        [HttpPost]
        public ActionResult ShowProtokollEdit(string fileName)
        {
            return PartialView("Partial/ProtokollArchivierung/ProtokollEdit", ViewModel.GetProtokollEditModel(fileName));
        }

        [HttpPost]
        public ActionResult ProtokollEdit(ProtokollEditModel model)
        {
            if (ModelState.IsValid)
                ViewModel.ProtokollArchivieren(model, ModelState);

            return PartialView("Partial/ProtokollArchivierung/ProtokollEdit", model);
        }

        public ActionResult ShowProtokollEditPdf()
        {
            var contentDispostion = new System.Net.Mime.ContentDisposition
            {
                FileName = ViewModel.ProtokollEditFileName,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", contentDispostion.ToString());

            var pdf = ViewModel.GetProtokollEditPdf();

            return File(pdf, "application/pdf");
        }

        [HttpPost]
        public JsonResult DeleteProtokoll()
        {
            var erg = ViewModel.ProtokollLoeschen();

            return Json(String.IsNullOrEmpty(erg) ? new { ok = true, message = "" } : new { ok = false, message = String.Format("{0}: {1}", Localize.DeleteFailed, erg) });
        }

        [HttpPost]
        public ActionResult FilterGridFahrerProtokolle(string filterValue, string filterColumns)
        {
            ViewModel.FilterFahrerProtokolle(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportFahrerProtokolleFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.FahrerProtokolleFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.DriverProtocols, dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrerProtokolleFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.FahrerProtokolleFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.DriverProtocols, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
