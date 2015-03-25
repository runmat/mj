using System.Web.Mvc;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Controllers;
using DocumentTools.Services;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class EquiController
    {
        public EquiHistorieViewModel EquipmentHistorieViewModel { get { return GetViewModel<EquiHistorieViewModel>(); } }

        [CkgApplication]
        public ActionResult FahrzeugHistorie()
        {
            return View(EquipmentHistorieViewModel);
        }

        [CkgApplication]
        public ActionResult FahrzeugHistorieZurFin(string fin)
        {
            EquipmentHistorieViewModel.LoadHistorie(fin);

            return View(EquipmentHistorieViewModel);
        }

        [HttpPost]
        public ActionResult GetFahrzeugHistoriePartial(string fahrgestellnummer)
        {
            EquipmentHistorieViewModel.LoadHistorie(fahrgestellnummer);

            return PartialView("Historie/HistorieDetail", EquipmentHistorieViewModel.EquipmentHistorie);
        }

        [HttpPost]
        public ActionResult LoadHistorieEquis(EquiHistorieSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                EquipmentHistorieViewModel.LoadHistorieInfos(ref model, ModelState);
            }

            return PartialView("Historie/HistorieSuche", model);
        }

        [HttpPost]
        public ActionResult ShowHistorieEquis()
        {
            return PartialView("Historie/HistorieGrid", EquipmentHistorieViewModel);
        }

        [GridAction]
        public ActionResult EquiHistorieInfosAjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieViewModel.HistorieInfosFiltered));
        }

        public FileContentResult FahrzeughistoriePdf()
        {
            var summaryHtml = this.RenderPartialViewToString("Historie/Partial/Pdf", EquipmentHistorieViewModel.CreateSummaryModel());
            var summaryPdfBytes = PdfDocumentFactory.HtmlToPdf(summaryHtml);

            return new FileContentResult(summaryPdfBytes, "application/pdf") { FileDownloadName = "Historie.pdf" };
        }

        [HttpPost]
        public ActionResult FilterGridEquiHistorieInfos(string filterValue, string filterColumns)
        {
            EquipmentHistorieViewModel.FilterHistorieInfos(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportEquiHistorieInfosFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = EquipmentHistorieViewModel.HistorieInfosFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Historie", dt);

            return new EmptyResult();
        }

        public ActionResult ExportEquiHistorieInfosFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = EquipmentHistorieViewModel.HistorieInfosFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Historie", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
