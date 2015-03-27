using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.WFM.Models;
using CkgDomainLogic.WFM.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class WfmController 
    {
        public WfmViewModel ViewModel { get { return GetViewModel<WfmViewModel>(); } }

        [CkgApplication]
        public ActionResult Abmeldevorgaenge()
        {
            ViewModel.DataInit(SelektionsModus.Abmeldevorgaenge);

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult KlaerfallWorkplace()
        {
            ViewModel.DataInit(SelektionsModus.KlaerfallWorkplace);

            return View("Abmeldevorgaenge", ViewModel);
        }

        [HttpPost]
        public ActionResult LoadAuftraege(WfmAuftragSelektor model)
        {
            ViewModel.Selektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadAuftraege(ModelState);

            return PartialView("Abmeldevorgaenge/AuftraegeSuche", ViewModel.Selektor);
        }

        [HttpPost]
        public ActionResult ShowAuftraege()
        {
            return PartialView("Abmeldevorgaenge/AuftraegeGrid", ViewModel);
        }

        [GridAction]
        public ActionResult AuftraegeAjaxBinding()
        {
            return View(new GridModel(ViewModel.AuftraegeFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridAuftraege(string filterValue, string filterColumns)
        {
            ViewModel.FilterAuftraege(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult GetAuftragsDetailPartial(string vorgangsnr)
        {
            ViewModel.LoadAuftragsDetails(vorgangsnr, ModelState);

            return PartialView("Abmeldevorgaenge/AuftragsDetail", ViewModel);
        }

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.AuftraegeFiltered;
        }

        #region Übersicht/Storno

        #endregion

        #region Informationen

        [GridAction]
        public ActionResult InformationenAjaxBinding()
        {
            return View(new GridModel(ViewModel.InformationenFiltered));
        }

        [HttpPost]
        public ActionResult SaveNewInformation(string neueInfo)
        {
            var saveErg = ViewModel.SaveNeueInformation(neueInfo);

            if (!String.IsNullOrEmpty(saveErg))
                return Json(saveErg);

            return Json("OK");
        }

        [HttpPost]
        public ActionResult FilterGridInformationen(string filterValue, string filterColumns)
        {
            ViewModel.FilterInformationen(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportInformationenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.InformationenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Informations, dt);

            return new EmptyResult();
        }

        public ActionResult ExportInformationenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.InformationenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Informations, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region Dokumente

        [GridAction]
        public ActionResult DokumenteAjaxBinding()
        {
            return View(new GridModel(ViewModel.DokumenteFiltered));
        }

        public FileContentResult PdfDocumentDownload(string docId)
        {
            var pdfBytes = ViewModel.GetDokument(docId);

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", objectId) };
        }

        [HttpPost]
        public ActionResult UploadDokumentStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!ViewModel.SaveDokument(file))
                return Json(new { success = false, message = Localize.ErrorFileCouldNotBeSaved }, "text/plain");

            return Json(new { success = true, message = "ok", uploadFileName = file.FileName }, "text/plain");
        }

        [HttpPost]
        public ActionResult FilterGridDokumente(string filterValue, string filterColumns)
        {
            ViewModel.FilterDokumente(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportDokumenteFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DokumenteFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Documents, dt);

            return new EmptyResult();
        }

        public ActionResult ExportDokumenteFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DokumenteFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Documents, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region Aufgaben

        [GridAction]
        public ActionResult AufgabenAjaxBinding()
        {
            return View(new GridModel(ViewModel.AufgabenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridAufgaben(string filterValue, string filterColumns)
        {
            ViewModel.FilterAufgaben(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportAufgabenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.AufgabenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Tasks, dt);

            return new EmptyResult();
        }

        public ActionResult ExportAufgabenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.AufgabenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Tasks, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
