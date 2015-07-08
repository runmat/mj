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

            // PDF-Test...
            var bapiParameterSet = new BapiParameterSet
                {
                    AbholungAnsprechpartner = "AbholungAnsprechpartner",
                    AbholungDateTime = new DateTime(2015,7,1),
                    AbholungHinweis = "AbholungHinweis",
                    AbholungKunde = "AbholungKunde",
                    AbholungMobilitaetsfahrzeug = "0",
                    AbholungOrt = "AbholungOrt",
                    AbholungPlz = "11111",
                    AbholungStrasseHausNr = "AbholungStrasseHausNr",
                    AbholungTel = "AbholungTel",
                    AnlieferungAbholungAbDt = new DateTime(2015, 7, 2, 10,0,0),
                    AnlieferungAnlieferungBisDt = new DateTime(2015,7,2, 15,15,0),
                    AnlieferungAnsprechpartner = "AnlieferungAnsprechpartner",
                    AnlieferungHinweis = "AnlieferungHinweis",
                    AnlieferungKunde = "AnlieferungKunde",
                    AnlieferungMobilitaetsfahrzeug = "1",
                    AnlieferungOrt = "AnlieferungOrt",
                    AnlieferungPlz = "22222",
                    AnlieferungStrasseHausNr = "AnlieferungStrasseHausNr",
                    AnlieferungTel = "AnlieferungTel",
                    Ansprechpartner = "Ansprechpartner",
                    AnsprechpartnerTel = "AnsprechpartnerTel",
                    AuftragerstellerTel = "AntragstellerTel",
                    Auftragsersteller = "Antragsteller",
                    BetriebHausNr = "BetriebHausNr",
                    BetriebName = "BetriebName",
                    BetriebOrt = "BetriebOrt",
                    BetriebPLZ = "BetriebPLZ",
                    BetriebStrasse = "BetriebStraße",
                    Fahrzeugart = "Fahrzeugart",
                    Kennnzeichen = "Kennzeichen",
                    KundeTel = "KundeTel",
                    Repco = "Repco",

                    Return = ""
                };

            var bapiParameterSets = new List<BapiParameterSet> { bapiParameterSet };

            var pdf = ViewModel.GenerateSapPdf(bapiParameterSets);

            ViewModel.Overview = new Overview();

            ViewModel.Overview.PdfGenerated = pdf;

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult Auftraggeber(Auftraggeber model)   
        {
            model.Auftragsersteller = ViewModel.GlobalViewData.Auftragsersteller;  // Sicherstellen, dass Antragsteller nicht durch Formularfeld-Manipulation im Browser geändert werden kann

            if (ModelState.IsValid)
            {
                ViewModel.Auftraggeber = model;

                if (!string.IsNullOrEmpty(model.Kunde))
                {
                    ViewModel.Abholung.AbholungKunde = model.Kunde;
                    ViewModel.Anlieferung.AnlieferungKunde = model.Kunde;
                }
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
            // Hier vom BAPI ein PDF abgerufen und angezeigt

            ViewModel.Overview = new Overview();
            ViewModel.Overview.PdfUploaded = ViewModel.Upload.PdfBytes = null;

            // return PartialView("Partial/Overview", ViewModel.Overview);

            return PartialView("GeneratedPdf", ViewModel.Overview);
        }

        public FileContentResult GeneratedPdf()
        {

            // var summaryHtml = this.RenderPartialViewToString("Partial/SummaryPdf", zulassung.CreateSummaryModel());
            
            var summaryPdfBytes = ViewModel.Overview.PdfGenerated; //  PdfDocumentFactory.HtmlToPdf(summaryHtml);

            return new FileContentResult(summaryPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.Overview) };
        }

        //public FileStreamResult PDFGenerator()
        //{
        //    Stream fileStream = GeneratePDF();

        //    HttpContext.Response.AddHeader("content-disposition",
        //    "attachment; filename=form.pdf");

        //    return new FileStreamResult(fileStream, "application/pdf");
        //}

        //private Stream GeneratePDF()
        //{
        //    //create your pdf and put it into the stream... pdf variable below
        //    //comes from a class I use to write content to PDF files

        //    MemoryStream ms = new MemoryStream();

        //    byte[] byteInfo = pdf.Output();
        //    ms.Write(byteInfo, 0, byteInfo.Length);
        //    ms.Position = 0;

        //    return ms;
        //}

    }
}
