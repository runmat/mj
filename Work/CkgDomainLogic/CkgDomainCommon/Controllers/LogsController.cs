using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Logs.Contracts;
using CkgDomainLogic.Logs.Models;
using CkgDomainLogic.Logs.ViewModels;
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


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.SapLogItemsFiltered;
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
    }
}
