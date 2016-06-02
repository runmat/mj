using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models.HolBringService;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Web;
using ServicesMvc.Areas.Fahrzeug;
using ServicesMvc.Areas.Fahrzeug.Models.HolBringService;

namespace ServicesMvc.Fahrzeug.Controllers
{
    [HolBringServiceInjectGlobalData]
    public class HolBringServiceController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<HolBringServiceViewModel>(); } }

        public HolBringServiceViewModel ViewModel
        {
            get { return GetViewModel<HolBringServiceViewModel>(); }
            set { SetViewModel(value); }
        }

        public HolBringServiceController(IAppSettings appSettings, ILogonContextDataService logonContext, IHolBringServiceDataService holBringServiceDataService) 
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, holBringServiceDataService);
            InitModelStatics();
        }

        static void InitModelStatics()
        {}

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult Auftraggeber(Auftraggeber model)   
        {
            model.Auftragsersteller = ViewModel.GlobalViewData.Auftragsersteller;  // Sicherstellen, dass Antragsteller nicht durch Formularfeld-Manipulation im Browser geändert werden kann

            if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model = ViewModel.Auftraggeber;

            if (ModelState.IsValid)
            {
                ViewModel.Auftraggeber = model;

                if (!string.IsNullOrEmpty(model.Kunde))
                {
                    ViewModel.Abholung.AbholungKunde = model.Kunde;
                    ViewModel.Anlieferung.AnlieferungKunde = model.Kunde;                    
                }

                ViewModel.SetBetriebAddress();

                ViewModel.Auftraggeber.Fahrzeugart = ViewModel.GlobalViewData.Fahrzeugarten.FirstOrDefault(x => x.Wert == model.FahrzeugartId.ToString()).Beschreibung;
            }

            return PartialView("Partial/Auftraggeber", model);
        }

        [HttpPost]
        public ActionResult Abholung(Abholung model)
        {
            if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model = ViewModel.Abholung;

            if (ModelState.IsValid)
            {
                ViewModel.Abholung = model;
                ViewModel.CopyDefaultValuesToAnlieferung(model);

                ViewModel.GlobalViewData.ValidationAbholungDt = model.AbholungDatum;
            }

            return PartialView("Partial/Abholung", model);
        }

        [HttpPost]
        public ActionResult Anlieferung(Anlieferung model)
        {
            if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model = ViewModel.Anlieferung;

            if (ModelState.IsValid)
            {
                ViewModel.Anlieferung = model;
            }

            return PartialView("Partial/Anlieferung", model);
        }

        #region PDF-Upload
        [HttpPost]
        public ActionResult Upload(Upload model)
        {
            if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model = ViewModel.Upload;

            if (ModelState.IsValid)
            {
                if (model.DeleteUploadedPdf)
                {
                    ViewModel.Overview.PdfUploaded = null;
                    ViewModel.Overview.PdfCreateDt = null;
                }
                
                ViewModel.Upload = model;
            }

            return PartialView("Partial/Upload", model);
        }

        [HttpPost]
        public ActionResult UploadPdfStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!ViewModel.PdfUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = Localize.ErrorFileCouldNotBeSaved }, "text/plain");

            ViewModel.Upload.UploadFileName = file.FileName;
            
            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        #endregion

        [HttpPost]
        public ActionResult Overview()
        {
            var bapiParameterSets = new List<BapiParameterSet> { ViewModel.GetBapiParameterSets };
            var pdf = ViewModel.GenerateSapPdf(bapiParameterSets);
            
            ViewModel.Overview.PdfGenerated = pdf;

            // PDFs zusammenfassen. Falls keine PDF-Datei hochgeladen wurde, wird nur das SAP-Pdf genutzt
            ViewModel.MergePdf();

            return PartialView("Partial/Overview", ViewModel.Overview);
        }

        public FileContentResult DownloadGeneratedPdf()
        {            
            var pdf = ViewModel.Overview.PdfGenerated;
            var filename = ViewModel.GetPdfFilename();
            return new FileContentResult(pdf, "application/pdf") { FileDownloadName = filename };
        }

        /// <summary>
        /// Gibt zusammengefasstes PDF aus GeneratedPdf und UploadedPdf zurück
        /// </summary>
        /// <returns></returns>
        public FileContentResult DownloadMergedPdf()
        {
            var pdf = ViewModel.Overview.PdfMerged;
            var filename = ViewModel.GetPdfFilename();
            return new FileContentResult(pdf, "application/pdf") { FileDownloadName = filename };
        }

        public ActionResult ShowGeneratedPdf()
        {
            var pdfBytes = ViewModel.Overview.PdfGenerated;

            return new FileContentResult(pdfBytes, "application/pdf");
        }

        public FileContentResult ShowUploadedPdf()
        {
            var pdfBytes = ViewModel.Overview.PdfUploaded;
            if (pdfBytes == null)
            {
                string html = "<html><p>" + Localize.DefaultEmptyPdf +"</p></html>";
                pdfBytes = PdfDocumentFactory.HtmlToPdf(html);
            }

            return new FileContentResult(pdfBytes, "application/pdf");
        }

        [HttpPost]
        public ActionResult SendMail(Mail model)
        {
            if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model = ViewModel.SendMail;

            if (ModelState.IsValid)
            {
                // Versenden...
                var result = ViewModel.SendMailTo();

                model.SendMailResult = result;

                model.UploadedPdfExists = ViewModel.Overview.PdfUploaded != null;
            }

            return PartialView("Partial/SendMail", model);
        }

    }
}
