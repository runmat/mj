using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using CkgDomainLogic.FzgModelle.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
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

             
        void InitModelStatics()
        {
           
        }


        [CkgApplication]
        public ActionResult ReportStatusEinsteuerung()
        {
            return ReportStatus();
        }

        [CkgApplication]
        public ActionResult ReportStatusbericht()
        {
            return ReportStatus();
        }

        private ActionResult ReportStatus()
        {
            StatusEinsteuerungViewModel.DataInit();
            StatusEinsteuerungViewModel.Init();
            return View(StatusEinsteuerungViewModel);
        }

        [HttpPost]
        public ActionResult LoadStatusEinsteuerung()
        {
                        
            if (ModelState.IsValid)
            {
                StatusEinsteuerungViewModel.LoadStatusEinsteuerung();
                if (StatusEinsteuerungViewModel.StatusEinsteuerungs.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/GridEinsteuerung", StatusEinsteuerungViewModel);
        }

        [HttpPost]
        public ActionResult LoadStatusbericht()
        {

            if (ModelState.IsValid)
            {
                StatusEinsteuerungViewModel.LoadStatusbericht();
                if (StatusEinsteuerungViewModel.StatusEinsteuerungs.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/Grid", StatusEinsteuerungViewModel);
        }

        [GridAction]
        public ActionResult StatusEinsteuerungAjaxBinding()
        {
            return View(new GridModel(StatusEinsteuerungViewModel.StatusEinsteuerungsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridStatusEinsteuerung(string filterValue, string filterColumns)
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
