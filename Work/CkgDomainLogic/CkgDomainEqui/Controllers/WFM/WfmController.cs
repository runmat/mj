using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.WFM.Models;
using CkgDomainLogic.WFM.ViewModels;
using DocumentTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class WfmController 
    {
        public WfmViewModel ViewModel { get { return GetViewModel<WfmViewModel>(); } }

        [CkgApplication]
        public ActionResult Abmeldevorgaenge()
        {
            ViewModel.DataInit();

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

            return PartialView("Abmeldevorgaenge/AuftragsDetail", ViewModel.AktuellerAuftrag);
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
