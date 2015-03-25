using System.Web.Mvc;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.CoC.ViewModels;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class CocBeauftragungController : CkgDomainController 
    {
        public override sealed string DataContextKey { get { return GetDataContextKey<CocBeauftragungViewModel>(); } }

        public CocBeauftragungViewModel ViewModel {  get { return GetViewModel<CocBeauftragungViewModel>(); } }


        public CocBeauftragungController(IAppSettings appSettings, ILogonContextDataService logonContext, ICocErfassungDataService erfassungDataService, IAdressenDataService adressenDataService, IBriefVersandDataService briefVersandDataService, IZulassungDataService zulassungDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, erfassungDataService, adressenDataService, briefVersandDataService, zulassungDataService);
        }

        public ActionResult Index(string vins)
        {
            return RedirectToActionPermanent("Versand", new { vins });
        }

        [CkgApplication]
        public ActionResult Versand(string vins)
        {
            ViewModel.DataMarkForRefresh(vins);
            ViewModel.SetCocBeauftragungMode(CocBeauftragungMode.Versand);

            return View("Beauftragung", ViewModel);
        }

        [CkgApplication]
        public ActionResult DuplikatDruck()
        {
            ViewModel.DataMarkForRefresh("");
            ViewModel.SetCocBeauftragungMode(CocBeauftragungMode.VersandDuplikat);

            return View("Beauftragung", ViewModel);
        }
        
        public ActionResult TestVersandAdressenGrid()
        {
            ViewModel.DataMarkForRefresh("");

            return View(ViewModel);
        }

        public ActionResult TestMultiSelect()
        {
            ViewModel.DataMarkForRefresh("");

            return View();
        }


        #region Fahrzeug Auswahl

        [GridAction]
        public ActionResult FahrzeugAuswahlAjaxBinding()
        {
            var items = ViewModel.CocAuftraegeFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugAuswahl(string filterValue, string filterColumns)
        {
            ViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult FahrzeugAuswahlSelectionChanged(int id, bool isChecked)
        {
            int allSelectionCount;
            ViewModel.SelectCocAuftrag(id, isChecked, out allSelectionCount);
            
            ViewModel.DataMarkForRefreshWunschkennzeichen();

            return Json(new { allSelectionCount });
        }

        #endregion


        #region Druck Optionen

        [HttpPost]
        public ActionResult DruckOptionen()
        {
            return PartialView(ViewModel);
        }

        [HttpPost]
        public ActionResult DruckOptionenForm(DruckOptionen model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.DruckOptionen = model;
                LogonContext.DataContextPersist(ViewModel);
            }

            return PartialView(model);
        }

        #endregion


        #region VersandAdresse

        [HttpPost]
        public ActionResult VersandAdresse()
        {
            return PartialView(ViewModel);
        }

        [HttpPost]
        public ActionResult VersandAdresseSelbstDruck()
        {
            return PartialView(ViewModel);
        }

        [HttpGet]
        public ActionResult SelbstDruckGetPdf(string vnr, string vorlage)
        {
            var pdfBytes = ViewModel.GetCocAsPdf(vnr, vorlage);

            if (pdfBytes == null)
                return new ContentResult { Content = Localize.PdfFileCouldNotBeGenerated };

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = ViewModel.LastCocPdfFileName };
        }

        [HttpPost]
        public JsonResult SelbstDruckPollAuftragsDatum(string vnr, string vorlage)
        {
            return Json(new
                {
                    auftragsDatum = ViewModel.CocSelbstDruckAuftragsDatum(vnr) == null ? "" : ViewModel.CocSelbstDruckAuftragsDatum(vnr).GetValueOrDefault().ToString("dd.MM.yyyy HH:mm"),
                    vnr,
                    vorlage
                });
        }

        [GridAction]
        public ActionResult VersandAdressenAjaxBinding()
        {
            var items = ViewModel.VersandAdressenFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterVersandAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterVersandAdressen(filterValue, filterColumns);

            return new EmptyResult();
        }
        
        [HttpPost]
        public JsonResult VersandAdresseGetAutoCompleteItems()
        {
            return Json(new { items = ViewModel.VersandAdressenAsAutoCompleteItems });
        }

        [HttpPost]
        public ActionResult VersandAdressenShowGrid()
        {
            ViewModel.DataMarkForRefreshVersandAdressenFiltered();

            return PartialView("Partial/VersandAdressenAuswahlGrid");
        }

        [HttpPost]
        public ActionResult VersandAdresseForm(Adresse model)
        {
            // Avoid ModelState clearing on saving => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();

            if (model.TmpSelectionKey.IsNotNullOrEmpty())
            {
                model = ViewModel.GetVersandAdresseFromKey(model.TmpSelectionKey);
                if (model == null)
                    return new EmptyResult();

                ModelState.Clear();
                model.IsValid = false;
                return PartialView(model);
            }

            if (ModelState.IsValid)
            {
                ViewModel.VersandAdresse = model;

                LogonContext.DataContextPersist(ViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView(model);
        }

        #endregion


        #region Versand Optionen

        [HttpPost]
        public ActionResult VersandOptionen()
        {
            return PartialView(ViewModel);
        }

        [HttpPost]
        public ActionResult VersandOptionenForm(VersandOptionen model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.VersandOptionen = model;

                LogonContext.DataContextPersist(ViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView(model);
        }

        #endregion


        #region Summary + Receipt

        [HttpPost]
        public ActionResult Summary()
        {
            return PartialView(ViewModel.CreateSummaryModel(false, GetAdressenSelectionLink));
        }

        public FileContentResult SummaryAsPdf()
        {
            var summaryHtml = this.RenderPartialViewToString("Partial/SummaryPdf", ViewModel.CreateSummaryModel(true, GetAdressenSelectionLink));

            var logoPath = AppSettings.LogoPath.IsNotNullOrEmpty() ? Server.MapPath(AppSettings.LogoPath) : "";
            var summaryPdfBytes = PdfDocumentFactory.HtmlToPdf(summaryHtml, logoPath, AppSettings.LogoPdfPosX, AppSettings.LogoPdfPosY);

            return new FileContentResult(summaryPdfBytes, "application/pdf") { FileDownloadName = "Übersicht.pdf" };
        }

        public ActionResult SummaryAsHtml()
        {
            return View("Partial/SummaryPdf", ViewModel.CreateSummaryModel(true, GetAdressenSelectionLink));
        }

        [HttpPost]
        public ActionResult Receipt()
        {
            LogonContext.DataContextPersist(ViewModel);
            ViewModel.Save();

            return PartialView(ViewModel);
        }

        #endregion


        #region grid data export

        public ActionResult FahrzeugAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.CocAuftraegeFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("CocAuftraege", dt);

            return new EmptyResult();
        }

        public ActionResult FahrzeugAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.CocAuftraegeFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("CocAuftraege", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        public ActionResult VersandadressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.VersandAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("VersandAdressen", dt);

            return new EmptyResult();
        }

        public ActionResult VersandadressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.VersandAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("VersandAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
