using System.Text;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.ViewModels;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;
using GeneralTools.Contracts;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace CkgDomainLogic.Controllers
{
    public class StatusEinsteuerungController : CkgDomainController
    {
        private string _dataContextKey = "";
        public override string DataContextKey
        {
            get { return _dataContextKey; }
        }

        public StatusEinsteuerungViewModel StatusEinsteuerungViewModel { get { return GetViewModel<StatusEinsteuerungViewModel>(); } }   

        public StatusEinsteuerungController(IAppSettings appSettings, ILogonContextDataService logonContext, IStatusEinsteuerungDataService statusEinsteuerungDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(StatusEinsteuerungViewModel, appSettings, logonContext, statusEinsteuerungDataService);            
        }

        [CkgApplication]
        public ActionResult ReportStatusbericht()
        {
            return View(StatusEinsteuerungViewModel);
        }
 
        [CkgApplication]
        public ActionResult ReportStatusEinsteuerung()
        {
            return View(StatusEinsteuerungViewModel);
        }

        [HttpPost]
        public ActionResult LoadStatusbericht()
        {
            StatusEinsteuerungViewModel.ModusStatusReport = true;

            if (ModelState.IsValid)
            {
                StatusEinsteuerungViewModel.LoadStatusEinsteuerung();
                if (StatusEinsteuerungViewModel.StatusEinsteuerungs.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/Suche", StatusEinsteuerungViewModel);
        }

        [HttpPost]
        public ActionResult LoadStatusEinsteuerung()
        {
            StatusEinsteuerungViewModel.ModusStatusReport = false;

            if (ModelState.IsValid)
            {
                StatusEinsteuerungViewModel.LoadStatusEinsteuerung();
                if (StatusEinsteuerungViewModel.StatusEinsteuerungs.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/SucheEinsteuerung", StatusEinsteuerungViewModel);
        }

        [HttpPost]
        public ActionResult ShowStatusReport()
        {
            return PartialView("Partial/Grid", StatusEinsteuerungViewModel);
        }

        [HttpPost]
        public ActionResult ShowStatusEinsteuerung()
        {
            return PartialView("Partial/GridEinsteuerung", StatusEinsteuerungViewModel);
        }

        [GridAction]
        public ActionResult StatusEinsteuerungAjaxBinding()
        {
            return View(new GridModel(StatusEinsteuerungViewModel.StatusEinsteuerungsFiltered));
        }

        public ActionResult ReportAsExcel()
        {
            var dt = StatusEinsteuerungViewModel.GetReportDataAsDataTable();
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(StatusEinsteuerungViewModel.ReportAsExcelFilename, dt);

            return new EmptyResult();
        }

        public ActionResult ReportAsHtml()
        {
            return Content(StatusEinsteuerungViewModel.ReportAsHtml);
        }

        [HttpPost]
        public ActionResult FilterGridStatus(string filterValue, string filterColumns)
        {
            StatusEinsteuerungViewModel.FilterStatusEinsteuerung(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Export

        public ActionResult ExportStatusEinsteuerungFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = StatusEinsteuerungViewModel.StatusEinsteuerungsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Report_StatusEinsteuerung, dt);

            return new EmptyResult();
        }

        public ActionResult ExportStatusEinsteuerungFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = StatusEinsteuerungViewModel.StatusEinsteuerungsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Report_StatusEinsteuerung, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
