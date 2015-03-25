using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Equi.ViewModels;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Equi-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class EquiController  
    {
        public BriefversandViewModel BriefversandViewModel { get { return GetViewModel<BriefversandViewModel>(); } }

        [CkgApplication]
        public ActionResult Briefversand(string vins)
        {
            BriefversandViewModel.DataMarkForRefresh(vins);

            return View(BriefversandViewModel);
        }


        #region Fahrzeug Auswahl

        [GridAction]
        public ActionResult FahrzeugAuswahlAjaxBinding()
        {
            var items = BriefversandViewModel.FahrzeugeFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugAuswahl(string filterValue, string filterColumns)
        {
            BriefversandViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult FahrzeugAuswahlSelectionChanged(string vin, bool isChecked)
        {
            int allSelectionCount, allCount = 0, allFoundCount = 0;
            if (vin.IsNullOrEmpty())
                BriefversandViewModel.SelectFahrzeuge(isChecked, f => !f.IsMissing, out allSelectionCount, out allCount, out allFoundCount);
            else
                BriefversandViewModel.SelectFahrzeug(vin, isChecked, out allSelectionCount);

            return Json(new { allSelectionCount, allCount, allFoundCount });
        }

        #endregion


        #region Versandart Optionen

        [HttpPost]
        public ActionResult VersandartOptionen()
        {
            return PartialView("Briefversand/VersandartOptionen", BriefversandViewModel);
        }

        [HttpPost]
        public ActionResult VersandartOptionenForm(VersandartOptionen model)
        {
            if (ModelState.IsValid)
            {
                BriefversandViewModel.VersandartOptionen = model;

                LogonContext.DataContextPersist(BriefversandViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView("Briefversand/VersandartOptionenForm", model);
        }

        #endregion


        #region VersandAdresse

        [HttpPost]
        public ActionResult VersandAdresse()
        {
            return PartialView("Briefversand/VersandAdresse", BriefversandViewModel);
        }

        [HttpPost]
        public ActionResult VersandAdresseForm(Adresse model)
        {
            // Avoid ModelState clearing on saving => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();

            if (model.TmpSelectionKey.IsNotNullOrEmpty())
            {
                model = BriefversandViewModel.GetVersandAdresseFromKey(model.TmpSelectionKey);
                if (model == null)
                    return new EmptyResult();

                ModelState.Clear();
                model.IsValid = false;
                return PartialView("Briefversand/VersandAdresseForm", model);
            }

            if (ModelState.IsValid)
            {
                BriefversandViewModel.VersandAdresse = model;

                LogonContext.DataContextPersist(BriefversandViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView("Briefversand/VersandAdresseForm", model);
        }

        [GridAction]
        public ActionResult VersandAdressenAjaxBinding()
        {
            var items = BriefversandViewModel.VersandAdressenFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterVersandAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            BriefversandViewModel.FilterVersandAdressen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult VersandAdresseGetAutoCompleteItems()
        {
            return Json(new { items = BriefversandViewModel.VersandAdressenAsAutoCompleteItems });
        }

        [HttpPost]
        public ActionResult VersandAdressenShowGrid()
        {
            BriefversandViewModel.DataMarkForRefreshVersandAndZulassungAdressenFiltered();

            return PartialView("Briefversand/Partial/VersandAdressenAuswahlGrid");
        }

        [GridAction]
        public ActionResult ZulassungAdressenAjaxBinding()
        {
            var items = BriefversandViewModel.ZulassungAdressenFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterZulassungAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            BriefversandViewModel.FilterZulassungAdressen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult ZulassungAdresseGetAutoCompleteItems()
        {
            return Json(new { items = BriefversandViewModel.ZulassungAdressenAsAutoCompleteItems });
        }

        [HttpPost]
        public ActionResult ZulassungAdressenShowGrid()
        {
            BriefversandViewModel.DataMarkForRefreshVersandAndZulassungAdressenFiltered();

            return PartialView("Briefversand/Partial/ZulassungAdressenAuswahlGrid");
        }

        #endregion


        #region Versand Optionen

        [HttpPost]
        public ActionResult VersandOptionen()
        {
            BriefversandViewModel.DataMarkForRefreshVersandoptionen();
            BriefversandViewModel.DataMarkForRefreshVersandgruende();
            BriefversandViewModel.VersandOptionen.AufAbmeldungWartenAvailable = BriefversandViewModel.VersandOptionAufAbmeldungWartenAvailable;

            return PartialView("Briefversand/VersandOptionen", BriefversandViewModel);
        }

        [HttpPost]
        public ActionResult VersandOptionenForm(VersandOptionen model)
        {
            if (ModelState.IsValid)
            {
                BriefversandViewModel.VersandOptionen = model;

                LogonContext.DataContextPersist(BriefversandViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView("Briefversand/VersandOptionenForm", model);
        }

        #endregion


        #region Summary + Receipt

        [HttpPost]
        public ActionResult Summary()
        {
            return PartialView("Briefversand/Summary", BriefversandViewModel.CreateSummaryModel(false));
        }

        public FileContentResult SummaryAsPdf()
        {
            var summaryHtml = this.RenderPartialViewToString("Briefversand/Partial/SummaryPdf", BriefversandViewModel.CreateSummaryModel(true));

            var logoPath = AppSettings.LogoPath.IsNotNullOrEmpty() ? Server.MapPath(AppSettings.LogoPath) : "";
            var summaryPdfBytes = PdfDocumentFactory.HtmlToPdf(summaryHtml, logoPath, AppSettings.LogoPdfPosX, AppSettings.LogoPdfPosY);

            return new FileContentResult(summaryPdfBytes, "application/pdf") { FileDownloadName = "Übersicht.pdf" };
        }

        public ActionResult SummaryAsHtml()
        {
            return View("Briefversand/Partial/SummaryPdf", BriefversandViewModel.CreateSummaryModel(true));
        }

        [HttpPost]
        public ActionResult Receipt()
        {
            LogonContext.DataContextPersist(BriefversandViewModel);
            BriefversandViewModel.Save();

            return PartialView("Briefversand/Receipt", BriefversandViewModel);
        }

        #endregion


        #region General Address Helpers

        [HttpPost]
        public ActionResult UpdateAddressAndGetSummary(int updateAddressid, string addressType)
        {
            return PartialView("Briefversand/Summary", BriefversandViewModel.CreateSummaryModel(false));
        }

        #endregion


        #region CSV Upload

        [HttpPost]
        public ActionResult CsvUploadStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            // Step 1:  Upload the CSV file

            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = "Fehler: Keine Datei angegeben!" }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!BriefversandViewModel.CsvUploadFileSaveForPrefilter(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = "Fehler: CSV Datei konnte nicht gespeichert werden!" }, "text/plain");

            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult CsvUploadReset()
        {
            BriefversandViewModel.SetFahrzeugeMergedWithCsvUpload(null);

            return new EmptyResult();   
        }

        public ActionResult CsvTemplate()
        {
            return PartialView("Briefversand/Partial/CsvTemplate");
        }

        #endregion


        #region grid data export

        public ActionResult FahrzeugAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = BriefversandViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Fahrzeuge", dt);

            return new EmptyResult();
        }

        public ActionResult FahrzeugAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = BriefversandViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Fahrzeuge", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        public ActionResult VersandadressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = BriefversandViewModel.VersandAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("VersandAdressen", dt);

            return new EmptyResult();
        }

        public ActionResult VersandadressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = BriefversandViewModel.VersandAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("VersandAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        public ActionResult ZulassungadressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = BriefversandViewModel.ZulassungAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("ZulassungsAdressen", dt);

            return new EmptyResult();
        }

        public ActionResult ZulassungadressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = BriefversandViewModel.ZulassungAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("ZulassungsAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
