using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Logs.Contracts;
using CkgDomainLogic.Logs.Models;
using CkgDomainLogic.Logs.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public class LogsController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<LogsViewModel>(); } }

        public LogsViewModel ViewModel { get { return GetViewModel<LogsViewModel>(); } }


        public LogsController(IAppSettings appSettings, ILogonContextDataService logonContext, ILogsDataService logsDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, logsDataService);
        }


        [CkgApplication]
        public ActionResult Sap() 
        {
            SapLogItem.StackContextItemTemplate = stackContext => this.RenderPartialViewToString("Partial/Sap/StackContext", stackContext);
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult WebServiceTraffic()
        {
            ViewModel.DataInit();

            ViewBag.AllLogTables = ViewModel.AllWebServiceTrafficLogTables;

            return View(ViewModel);
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.SapLogItemsFiltered;
        }

        public ActionResult ExportWebServiceTrafficLogItemsFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.WebServiceTrafficLogItemsUIFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Webservice-Log", dt);

            return new EmptyResult();
        }

        public ActionResult ExportWebServiceTrafficLogItemsFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.WebServiceTrafficLogItemsUIFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Webservice-Log", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion


        #region Sap Logs

        [HttpPost]
        public ActionResult LoadSapLogItems(SapLogItemSelector model)
        {
            ModelState.Clear();

            ViewModel.Validate(ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                if (ViewModel.LoadSapLogItems(model))
                    if (ViewModel.SapLogItemsFiltered.None())
                        ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/Sap/SucheSapLogItems", ViewModel.SapLogItemSelector);
        }

        [HttpPost]
        public ActionResult ShowSapLogItems()
        {
            return PartialView("Partial/Sap/Grid", ViewModel);
        }

        [GridAction]
        public ActionResult LogsSapAjaxBinding()
        {
            return View(new GridModel(ViewModel.SapLogItemsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridLogsSap(string filterValue, string filterColumns)
        {
            ViewModel.FilterSapLogItems(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        #region Webservice Logs

        [HttpPost]
        public ActionResult LoadWebServiceTrafficLogItems(WebServiceTrafficLogItemSelector model)
        {
            ModelState.Clear();

            ViewModel.Validate(ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                if (ViewModel.LoadWebServiceTrafficLogItems(model))
                    if (ViewModel.WebServiceTrafficLogItemsUIFiltered.None())
                        ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            ViewBag.AllLogTables = ViewModel.AllWebServiceTrafficLogTables;

            return PartialView("Partial/WebServiceTraffic/SucheWebServiceTrafficLogItems", ViewModel.WebServiceTrafficLogItemSelector);
        }

        [HttpPost]
        public ActionResult ShowWebServiceTrafficLogItems()
        {
            return PartialView("Partial/WebServiceTraffic/Grid", ViewModel);
        }

        [GridAction]
        public ActionResult LogsWebServiceTrafficAjaxBinding()
        {
            return View(new GridModel(ViewModel.WebServiceTrafficLogItemsUIFiltered));
        }

        [HttpPost]
        public ActionResult ShowWebServiceTrafficLogDetails(int id)
        {
            return PartialView("Partial/WebServiceTraffic/Details", ViewModel.GetDetails(id));
        }

        [HttpPost]
        public ActionResult FilterGridLogsWebServiceTraffic(string filterValue, string filterColumns)
        {
            ViewModel.FilterWebServiceTrafficLogItems(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion
    }
}
