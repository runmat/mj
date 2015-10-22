using CkgDomainLogic.General.Controllers;
using DocumentTools.Services;
using GeneralTools.Services;
using GeneralTools.Models;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Ueberfuehrung.Contracts;
using CkgDomainLogic.Ueberfuehrung.Models;
using CkgDomainLogic.Ueberfuehrung.ViewModels;
using GeneralTools.Contracts;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace AutohausPortalMvc.Controllers
{
    public class UeberfuehrungHistoryController : CkgDomainController
    {
        protected override bool NeedsDefaultIndexActionInUrl { get { return false; } }

        public UeberfuehrungHistoryViewModel ViewModel { get { return GetViewModel<UeberfuehrungHistoryViewModel>(); } }

        #region LogonCapableController overrides

        #region DataContextCapableController overrides

        public override string DataContextKey { get { return "UeberfuehrungHistoryViewModel"; } }

        #endregion

        static protected ILogonContext CreateLogonContext()
        {
            //if (AppSettings.IsClickDummyMode)
            //    return new LogonContextTest(new Localize()) { KundenNr = "0010000649" };

            return new LogonContextDataServiceAutohaus();
        }

        #endregion 


        public UeberfuehrungHistoryController(IAppSettings appSettings, ILogonContextDataService logonContext, IUeberfuehrungDataService ueberfuehrungDataService) : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, ueberfuehrungDataService);
        }
        
        #region History

        public ActionResult Index()
        {
            ViewModel.DataMarkForRefresh();
            return View(ViewModel);
        }

        //public ActionResult Test(string un, string appID)
        //{
        //    var view = Init(un, appID, () => View("Test", ViewModel));

        //    ViewModel.HistoryAuftraege = ViewModel.GetHistoryAuftraege();
        //    ViewModel.PrepareHistoryAuftragsDokumente("23673828", "1");
            
        //    return view;
        //}

        [HttpPost]
        public ActionResult HistoryAuftragFilterSubmit(HistoryAuftragFilter filter)
        {
            filter.IsValidCustom = ModelState.IsValid;
            if (!filter.IsValidCustom)
                // simple model validation already failed...
                filter.ValidationErrorMessage = "Bitte geben Sie gültige Suchkriterien ein";
            else
                // deeper validation right here...
                ViewModel.ValidateHistoryAuftragFilter(ref filter, ModelState.AddModelError);

            if (filter.IsValidCustom)
                ViewModel.HistoryAuftragFilter = filter;

            return PartialView("Partial/HistoryAuftragFilter", filter);
        }

        [HttpPost]
        public ActionResult HistoryAuftraegeShow()
        {
            return PartialView("Partial/HistoryAuftraegeGrid");
        }

        [GridAction]
        public ActionResult HistoryAuftraegeAjaxBinding()
        {
            return View(new GridModel(ViewModel.GetHistoryAuftraege()));
        }

        [HttpPost]
        public ActionResult HistoryAuftragDetailsRequestShow(string auftragsNr, string fahrt)
        {
            ViewModel.PrepareHistoryAuftragsDokumente(auftragsNr, fahrt);
            if (ViewModel.HistoryAuftragCurrent == null)
                return new EmptyResult();

            return PartialView("Partial/HistoryAuftragDetailsWithDocuments", ViewModel);
        }

        [HttpPost]
        public ActionResult HistoryAuftragShowBigImage(int tour, int fileNr)
        {
            ViewModel.CopySingleBigImage(tour, fileNr);

            return PartialView("Partial/HistoryAuftragDetailsBigImageDialog", ViewModel.GetImageFileNameForIndex(tour, fileNr));
        }

        public ActionResult HistoryDownloadPdfFiles()
        {
            var zipFileName = ViewModel.GetPdfFilesAsZip();
            if (zipFileName.IsNullOrEmpty())
                return new EmptyResult();

            return new FileContentResult(FileService.GetBytesFromFile(zipFileName), "application/zip")
            {
                FileDownloadName = FileService.PathGetFileName(zipFileName)
            };
        }


        public ActionResult ExportHistoryAuftraegeExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.HistoryAuftraege.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Überführungsaufträge", dt);

            return new EmptyResult();
        }

        public ActionResult ExportHistoryAuftraegePDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.HistoryAuftraege.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Überführungsaufträge", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
