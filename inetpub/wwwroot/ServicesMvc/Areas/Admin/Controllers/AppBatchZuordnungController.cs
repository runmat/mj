using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.Admin.Contracts;
using CkgDomainLogic.Admin.Models;
using CkgDomainLogic.Admin.ViewModels;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Admin.Controllers
{
    public class AppBatchZuordnungController : CkgDomainController
    {
        public override string DataContextKey { get { return "AppBatchZuordnungViewModel"; } }

        public AppBatchZuordnungViewModel ViewModel { get { return GetViewModel<AppBatchZuordnungViewModel>(); } }

        public AppBatchZuordnungController(IAppSettings appSettings, ILogonContextDataService logonContext, IAppBatchZuordnungDataService appBatchZuordnungDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, appBatchZuordnungDataService);
        }

        [CkgApplication(AdminLevel.Master)]
        public ActionResult Index() 
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [GridAction]
        public ActionResult ApplicationsAjaxBinding()
        {
            return View(new GridModel(ViewModel.ApplicationsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridApplications(string filterValue, string filterColumns)
        {
            ViewModel.FilterApplications(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult ShowAppZuordnungen(int appId)
        {
            ViewModel.LoadAppZuordnungen(appId);

            return PartialView("Partial/GridAppZuordnungen", ViewModel);
        }

        [GridAction]
        public ActionResult AppZuordnungenAjaxBinding()
        {
            return View(new GridModel(ViewModel.AppZuordnungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridAppZuordnungen(string filterValue, string filterColumns)
        {
            ViewModel.FilterAppZuordnungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AppZuordnungenSelectionChanged(string id, bool isChecked)
        {
            if (id.IsNullOrEmpty())
                ViewModel.ChangeZuordnungen(isChecked);
            else
                ViewModel.ChangeZuordnung(id, isChecked);

            return Json(new { allSelectionCount = ViewModel.AppZuordnungenAssignedCount });
        }

        [HttpPost]
        public ActionResult SaveAppZuordnungen()
        {
            string message;

            var success = ViewModel.SaveZuordnungen(out message);

            return Json(new { message, success });
        }


        #region Export

        public ActionResult ExportApplicationsFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.ApplicationsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Applications, dt);

            return new EmptyResult();
        }

        public ActionResult ExportApplicationsFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.ApplicationsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Applications, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        public ActionResult ExportAppZuordnungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.AppZuordnungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.ApplicationAssignments, dt);

            return new EmptyResult();
        }

        public ActionResult ExportAppZuordnungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.AppZuordnungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.ApplicationAssignments, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
