using System;
using System.Web.Mvc;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class EquiController
    {
        public EquiHistorieRemarketingViewModel EquipmentHistorieRemarketingViewModel { get { return GetViewModel<EquiHistorieRemarketingViewModel>(); } }

        [CkgApplication]
        public ActionResult FahrzeugHistorieRemarketing()
        {
            return View(EquipmentHistorieRemarketingViewModel);
        }

        [HttpPost]
        public ActionResult GetFahrzeugHistorieRemarketingPartial(string fahrgestellnummer)
        {
            EquipmentHistorieRemarketingViewModel.LoadHistorie(fahrgestellnummer);

            return PartialView("Historie/HistorieRemarketingDetail", EquipmentHistorieRemarketingViewModel.EquipmentHistorie);
        }

        [HttpPost]
        public ActionResult LoadHistorieRemarketingEquis(EquiHistorieSuchparameter model)
        {
            EquipmentHistorieRemarketingViewModel.Suchparameter = model;

            if (ModelState.IsValid)
                model.AnzahlTreffer = EquipmentHistorieRemarketingViewModel.LoadHistorieInfos(ModelState);

            return PartialView("Historie/HistorieRemarketingSuche", model);
        }

        [HttpPost]
        public ActionResult ShowHistorieRemarketingEquis()
        {
            return PartialView("Historie/HistorieRemarketingGrid");
        }

        [GridAction]
        public ActionResult EquiHistorieRemarketingInfosAjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieRemarketingViewModel.HistorieInfosFiltered));
        }

        public ActionResult ShowBelastungsanzeige()
        {
            var pdfBytes = EquipmentHistorieRemarketingViewModel.GetBelastungsanzeigePdf();

            if (pdfBytes == null)
                return new ContentResult { Content = Localize.NoDocumentsFound };

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = string.Format("{0}_{1}.pdf", Localize.DebitNote, DateTime.Now.ToString("yyyyMMddHHmmss")) };
        }

        public ActionResult ShowSchadensgutachten()
        {
            var pdfBytes = EquipmentHistorieRemarketingViewModel.GetSchadensgutachtenPdf();

            if (pdfBytes == null)
                return new ContentResult { Content = Localize.NoDocumentsFound };

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = string.Format("{0}_{1}.pdf", Localize.DamageSurvey, DateTime.Now.ToString("yyyyMMddHHmmss")) };
        }

        public ActionResult ShowReparaturKalkulation()
        {
            var pdfBytes = EquipmentHistorieRemarketingViewModel.GetReparaturKalkulationPdf();

            if (pdfBytes == null)
                return new ContentResult { Content = Localize.NoDocumentsFound };

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = string.Format("{0}_{1}.pdf", Localize.RepairCalculation, DateTime.Now.ToString("yyyyMMddHHmmss")) };
        }

        public ActionResult ShowRechnung()
        {
            var pdfBytes = EquipmentHistorieRemarketingViewModel.GetRechnungPdf();

            if (pdfBytes == null)
                return new ContentResult { Content = Localize.NoDocumentsFound };

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = string.Format("{0}_{1}.pdf", Localize.Invoice, DateTime.Now.ToString("yyyyMMddHHmmss")) };
        }

        [HttpPost]
        public ActionResult FilterGridEquiHistorieRemarketingInfos(string filterValue, string filterColumns)
        {
            EquipmentHistorieRemarketingViewModel.FilterHistorieInfos(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportEquiHistorieRemarketingInfosFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = EquipmentHistorieRemarketingViewModel.HistorieInfosFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.VehicleHistory, dt);

            return new EmptyResult();
        }

        public ActionResult ExportEquiHistorieRemarketingInfosFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = EquipmentHistorieRemarketingViewModel.HistorieInfosFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.VehicleHistory, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
