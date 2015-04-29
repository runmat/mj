using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Finance.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Finance-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FinanceController  
    {
        public FinanceVersandsperreReportViewModel VersandsperreReportViewModel { get { return GetViewModel<FinanceVersandsperreReportViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportVersandsperre()
        {
            VersandsperreReportViewModel.LoadVorgaenge(ModelState);

            return View(VersandsperreReportViewModel);
        }

        [GridAction]
        public ActionResult VersandsperreReportAjaxBinding()
        {
            return View(new GridModel(VersandsperreReportViewModel.VorgaengeFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridVorgaengeVersandsperreReport(string filterValue, string filterColumns)
        {
            VersandsperreReportViewModel.FilterVorgaenge(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportVorgaengeVersandsperreReportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = VersandsperreReportViewModel.VorgaengeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Versandsperren", dt);

            return new EmptyResult();
        }

        public ActionResult ExportVorgaengeVersandsperreReportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = VersandsperreReportViewModel.VorgaengeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Versandsperren", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
