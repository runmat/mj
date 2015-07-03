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
            ViewModel.LoadFahrerAuftragsFahrten();

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult ProtokollUpload()
        {
            return FotoUpload(modeProtokoll: "1");
        }

        [CkgApplication]
        public ActionResult QmReport()
        {
            return View(ViewModel);
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


        #region Foto Upload

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
            ViewModel.SaveUploadedImageFile(file.FileName, file.SaveAs);

            return Json(new
            {
                files = new object[] { new { url = "-" } }   // VirtualPathUtility.ToAbsolute(ViewModel.FotoUploadPathVirtual) + Path.GetFileName(destinationFileName);
            });
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

        //Protokoll

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
    }
}
