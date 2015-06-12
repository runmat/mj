using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Fahrzeug.Controllers
{
    public class ZulassungController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<ZulassungViewModel>(); } }

        public ZulassungViewModel ViewModel 
        { 
            get { return GetViewModel<ZulassungViewModel>(); } 
            set { SetViewModel(value); } 
        }

        public ZulassungController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IFahrzeugeDataService fahrzeugeDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, fahrzeugeDataService);
            InitModelStatics();
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        static void InitModelStatics()
        {
        }


        #region Fahrzeug Auswahl

        [GridAction]
        public ActionResult FahrzeugAuswahlAjaxBinding()
        {
            var items = ViewModel.FahrzeugeFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugAuswahl(string filterValue, string filterColumns)
        {
            ViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult FahrzeugAuswahlSelectionChanged(string vin, bool isChecked)
        {
            int allSelectionCount = 0;
            if (vin.IsNullOrEmpty())
                ViewModel.SelectFahrzeuge(isChecked, f => true, out allSelectionCount);
            else
                ViewModel.SelectFahrzeug(vin, isChecked, out allSelectionCount);

            return Json(new
            {
                allSelectionCount,
                zulassungenAnzahlPdiTotal = ViewModel.ZulassungenAnzahlPdiTotal,
                zulassungenAnzahlGesamtTotal = ViewModel.ZulassungenAnzahlGesamtTotal,
            });
        }

        [HttpPost]
        public JsonResult OnChangeFilterValues(string type, string value)
        {
            ViewModel.OnChangeFilterValues(type, value);

            return Json(new
            {
                zulassungenAnzahlPdiTotal = ViewModel.ZulassungenAnzahlPdiTotal,
                zulassungenAnzahlGesamtTotal = ViewModel.ZulassungenAnzahlGesamtTotal,
            });
        }

        [HttpPost]
        public JsonResult OnChangePresetValues(string type, string value)
        {
            var errorMessage = ViewModel.OnChangePresetValues(type, ref value);

            return Json(new
            {
                value, errorMessage,
                zulassungenAnzahlPdiTotal = ViewModel.ZulassungenAnzahlPdiTotal,
                zulassungenAnzahlGesamtTotal = ViewModel.ZulassungenAnzahlGesamtTotal,
            });
        }

        #endregion    


        #region Fahrzeug Summary

        [HttpPost]
        public ActionResult FahrzeugSummary()
        {
            ViewModel.DataMarkForRefreshFahrzeugeSummary();

            return PartialView("Partial/GridFahrzeugSummary", ViewModel);
        }

        [GridAction]
        public ActionResult FahrzeugSummaryAjaxBinding()
        {
            var items = ViewModel.FahrzeugeSummaryFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugSummary(string filterValue, string filterColumns)
        {
            ViewModel.FilterFahrzeugeSummary(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion


        #region Summary + Receipt

        public FileContentResult SummaryAsPdf()
        {
            var header = this.RenderPartialViewToString("Partial/Summary/SummaryPdfBody", ViewModel.CreateSummaryTitle());
            var footer = this.RenderPartialViewToString("Partial/Summary/SummaryPdfBody", ViewModel.CreateSummaryOverview());
            var summaryHtml = this.RenderPartialViewToString("Partial/Summary/SummaryPdf", ViewModel.CreateSummaryDetails(header, footer));

            var logoPath = AppSettings.LogoPath.IsNotNullOrEmpty() ? Server.MapPath(AppSettings.LogoPath) : "";
            var summaryPdfBytes = PdfDocumentFactory.HtmlToPdf(summaryHtml, logoPath, AppSettings.LogoPdfPosX, AppSettings.LogoPdfPosY);

            return new FileContentResult(summaryPdfBytes, "application/pdf") { FileDownloadName = "Uebersicht.pdf" };
        }

        [HttpPost]
        public ActionResult Receipt()
        {
            ViewModel.Save();

            return PartialView("Partial/Receipt", ViewModel);
        }

        #endregion


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.FahrzeugeCurrentFiltered;
        }

        public ActionResult ExportFahrzeugeFilteredExcel(int page, string orderBy, string filterBy, string groupBy)
        {
            ViewModel.IsFahrzeugSummary = false;
            return GridDataExportFilteredExcel(page, orderBy, filterBy, groupBy);
        }

        public ActionResult ExportFahrzeugeFilteredPDF(int page, string orderBy, string filterBy)
        {
            ViewModel.IsFahrzeugSummary = false;
            return GridDataExportFilteredPDF(page, orderBy, filterBy);
        }

        public ActionResult ExportFahrzeugeSummaryFilteredExcel(int page, string orderBy, string filterBy, string groupBy)
        {
            ViewModel.IsFahrzeugSummary = true;
            return GridDataExportFilteredExcel(page, orderBy, filterBy, groupBy);
        }

        public ActionResult ExportFahrzeugeSummaryFilteredPDF(int page, string orderBy, string filterBy, string groupBy)
        {
            ViewModel.IsFahrzeugSummary = true;
            return GridDataExportFilteredPDF(page, orderBy, filterBy);
        }

        #endregion
    }
}
