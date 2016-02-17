using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.WFM.Models;
using CkgDomainLogic.WFM.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.WFM.Contracts;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class WfmController : CkgDomainController
    {
        public override string DataContextKey { get { return "WflViewModel"; } }

        public WfmViewModel ViewModel { get { return GetViewModel<WfmViewModel>(); } }

        public WfmController(IAppSettings appSettings, ILogonContextDataService logonContext, IAdressenDataService adressenDataService, IWfmDataService wflDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, wflDataService, adressenDataService);
            InitModelStatics();
        }

        void InitModelStatics()
        {
            WfmAuftrag.GetViewModel = GetViewModel<WfmViewModel>;
            WfmToDo.GetViewModel = GetViewModel<WfmViewModel>;
        }

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

        [CkgApplication]
        public ActionResult Durchlauf()
        {
            ViewModel.DataInit(SelektionsModus.Durchlauf);

            return View(ViewModel);
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

        [HttpPost]
        public ActionResult SetOrderToKlaerfall(string vorgangsNr, string remark)
        {
            var message = ViewModel.SetOrderToKlaerfall(vorgangsNr, remark);

            return Json(new
            {
                success = message.IsNullOrEmpty(),
                message = (message.IsNotNullOrEmpty() ? message : (Localize.SetClarificationCase + " " + Localize.Successful.ToLower()))
            });
        }

        [HttpPost]
        public ActionResult CancelOrder(string vorgangsNr)
        {
            var message = ViewModel.StornoAuftrag(vorgangsNr);

            return Json(new
            {
                success = message.IsNullOrEmpty(),
                message = (message.IsNotNullOrEmpty() ? message : (Localize.CancelOrder + " " + Localize.Successful.ToLower()))
            });
        }

        [HttpPost]
        public ActionResult VersandOptionChanged(string versandOption)
        {
            ViewModel.VersandOptionChanged(versandOption);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult CreateVersandAdresse()
        {
            var message = ViewModel.CreateVersandAdresse();

            return Json(new
            {
                success = message.IsNullOrEmpty(),
                message = (message.IsNotNullOrEmpty() ? message : Localize.CreateShippingAddressSuccessfullyCreated)
            });
        }

        #endregion


        #region VersandAdresse

        [HttpPost]
        public ActionResult VersandAdresse()
        {
            ViewModel.PrepareVersandAdresse();
            return PartialView("Abmeldevorgaenge/Partial/VersandAdresse", ViewModel);
        }

        [HttpPost]
        public ActionResult VersandAdresseForm(Adresse model)
        {
            if (model.TmpSelectionKey.IsNotNullOrEmpty())
            {
                model = ViewModel.GetVersandAdresseFromKey(model.TmpSelectionKey);
                if (model == null)
                    return new EmptyResult();

                ModelState.Clear();
                model.IsValid = false;
                return PartialView("Abmeldevorgaenge/Partial/VersandAdresseForm", model);
            }

            if (ModelState.IsValid)
            {
                ViewModel.VersandAdresse = model;

                LogonContext.DataContextPersist(ViewModel);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView("Abmeldevorgaenge/Partial/VersandAdresseForm", model);
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

            return PartialView("Abmeldevorgaenge/Partial/VersandAdressenAuswahlGrid");
        }

        public ActionResult VersandadressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.VersandAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("VersandAdressen", dt);

            return new EmptyResult();
        }

        public ActionResult VersandadressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.VersandAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("VersandAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

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
            var dt = ViewModel.InformationenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Informations, dt);

            return new EmptyResult();
        }

        public ActionResult ExportInformationenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.InformationenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
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

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", docId) };
        }

        [HttpPost]
        public ActionResult UploadDokumentStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            var message = ViewModel.SaveDokument(file);

            return Json(new
            {
                success = message.IsNullOrEmpty(),
                message = (message.IsNotNullOrEmpty() ? message : Localize.UploadFailed),
                uploadFileName = file.FileName 
            });
        }

        [HttpPost]
        public ActionResult FilterGridDokumente(string filterValue, string filterColumns)
        {
            ViewModel.FilterDokumente(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportDokumenteFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DokumenteFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Documents, dt);

            return new EmptyResult();
        }

        public ActionResult ExportDokumenteFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DokumenteFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
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

        [HttpPost]
        public ActionResult ConfirmToDo(string lfdNr, string remark)
        {
            var message = ViewModel.ConfirmToDo(lfdNr.ToInt(), remark);

            return Json(new
            {
                success = message.IsNullOrEmpty(),
                message = (message.IsNotNullOrEmpty() ? message : (Localize.Confirm + " " + Localize.Successful.ToLower()))
            });
        }

        [HttpPost]
        public ActionResult SetSelectedDokArt(string dokArt)
        {
            ViewModel.SelectedDokArt = dokArt;

            return new EmptyResult();
        }

        public ActionResult ExportAufgabenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.AufgabenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Tasks, dt);

            return new EmptyResult();
        }

        public ActionResult ExportAufgabenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.AufgabenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Tasks, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion


        #region Durchlauf

        [HttpPost]
        public ActionResult LoadDurchlauf(WfmAuftragSelektor model)
        {
            ViewModel.Selektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadDurchlauf(ModelState, false);

            return PartialView("Durchlauf/Suche", ViewModel.Selektor);
        }

        [HttpPost]
        public ActionResult ShowDurchlaufGrids()
        {
            return PartialView("Durchlauf/Grids", ViewModel);
        }


        [GridAction]
        public ActionResult DurchlaufDetailsAjaxBinding()
        {
            return View(new GridModel(ViewModel.DurchlaufDetailsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridDurchlaufDetails(string filterValue, string filterColumns)
        {
            ViewModel.FilterDurchlaufDetails(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportDurchlaufDetailsFilteredExcel(int page, string orderBy, string filterBy)
        {
            var gridCurrentSettings = GridSettingsPerName["GridDurchlaufDetails"];

            var dt = ViewModel.DurchlaufDetailsFiltered.GetGridFilteredDataTable(orderBy, filterBy, gridCurrentSettings.Columns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Details, dt);

            return new EmptyResult();
        }

        public ActionResult ExportDurchlaufDetailsFilteredPDF(int page, string orderBy, string filterBy)
        {
            var gridCurrentSettings = GridSettingsPerName["GridDurchlaufDetails"];

            var dt = ViewModel.DurchlaufDetailsFiltered.GetGridFilteredDataTable(orderBy, filterBy, gridCurrentSettings.Columns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Details, dt, landscapeOrientation: true);

            return new EmptyResult();
        }


        [GridAction]
        public ActionResult DurchlaufStatistikAjaxBinding()
        {
            return View(new GridModel(ViewModel.DurchlaufStatistiken));
        }

        public ActionResult ExportDurchlaufStatistikenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var gridCurrentSettings = GridSettingsPerName["GridDurchlaufStatistik"];

            var dt = ViewModel.DurchlaufStatistiken.GetGridFilteredDataTable(orderBy, filterBy, gridCurrentSettings.Columns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Statistics, dt);

            return new EmptyResult();
        }

        public ActionResult ExportDurchlaufStatistikenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var gridCurrentSettings = GridSettingsPerName["GridDurchlaufStatistik"];

            var dt = ViewModel.DurchlaufStatistiken.GetGridFilteredDataTable(orderBy, filterBy, gridCurrentSettings.Columns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Statistics, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult GetChartData(int chartID)
        {
            return Json(ViewModel.GetChartData(chartID));
        }

        #endregion

    }
}
