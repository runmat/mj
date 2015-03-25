using System;
using System.Web.Mvc;
using CkgDomainLogic.Finance.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Finance.ViewModels;
using CkgDomainLogic.General.Services;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Finance-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FinanceController  
    {
        public FinanceTelefonieReportViewModel TelefonieReportViewModel { get { return GetViewModel<FinanceTelefonieReportViewModel>(); } }

        [CkgApplication]
        public ActionResult TelefonieReport()
        {
            TelefonieReportViewModel.FillVertragsarten();

            return View(TelefonieReportViewModel);
        }

        [HttpPost]
        public ActionResult LoadTelefoniedaten(TelefoniedatenSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                TelefonieReportViewModel.LoadTelefoniedaten(model);
                if (TelefonieReportViewModel.Telefoniedaten.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            model.AuswahlVertragsart = TelefonieReportViewModel.DataService.Suchparameter.AuswahlVertragsart;

            return PartialView("TelefonieReport/TelefoniedatenSuche", model);
        }

        [HttpPost]
        public ActionResult ShowTelefoniedaten()
        {
            return PartialView("TelefonieReport/TelefoniedatenGrid", TelefonieReportViewModel);
        }

        [GridAction]
        public ActionResult TelefoniedatenAjaxBinding()
        {
            return View(new GridModel(TelefonieReportViewModel.TelefoniedatenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridTelefoniedaten(string filterValue, string filterColumns)
        {
            TelefonieReportViewModel.FilterTelefoniedaten(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportTelefoniedatenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = TelefonieReportViewModel.TelefoniedatenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Telefoniedaten", dt);

            return new EmptyResult();
        }

        public ActionResult ExportTelefoniedatenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = TelefonieReportViewModel.TelefoniedatenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Telefoniedaten", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
