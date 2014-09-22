using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Controllers;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace CkgDomainLogic.General.Controllers
{
    public abstract class CkgDomainController : LogonCapableController
    {
        public IAppSettings AppSettings { get; protected set; }

        public new ILogonContextDataService LogonContext
        {
            get { return (ILogonContextDataService)base.LogonContext; }
            set { base.LogonContext = value; }
        }

        public CkgBaseViewModel MainViewModel
        {
            get { return (CkgBaseViewModel)SessionStore.GetModel("MainViewModel"); } 
            set { SessionStore.SetModel("MainViewModel", value); }
        }


        protected string GetDataContextKey<T>()
        {
            return typeof (T).GetFullTypeName();
        }

        public override void ValidateMaintenance()
        {
            if (LogonContext != null)
                LogonContext.ValidateMaintenance();
        }

        protected T GetViewModel<T>() where T : class, new()
        {
            var typeName = typeof (T).Name;

            if (LogonContext != null && LogonContext.UserName.IsNotNullOrEmpty() && SessionHelper.GetSessionString(typeName + "_valid").IsNullOrEmpty())
            {
                // User Context changed => a probably stored viewModel should abandon!
                SessionHelper.SetSessionValue(typeName + "_valid", "valid");
                SetViewModel<T>(null);
            }

            return SessionStore<T>.GetModel(() =>
                {
                    var storedViewModel = (T)LogonContext.DataContextRestore(typeName);
                    return storedViewModel ?? new T();
                });
        }

        protected void SetViewModel<T>(T model) where T : class, new()
        {
            SessionStore<T>.Model = model;
        }

        protected CkgDomainController(IAppSettings appSettings, ILogonContextDataService logonContext)
            : base(logonContext)
        {
            AppSettings = appSettings;
        }

        protected ILogonContextDataService GetValidLogonContext(ILogonContextDataService logonContextToTry) 
        {
            var validLogonContext = logonContextToTry;
            if (this.LogonContext != null && this.LogonContext.UserName.IsNotNullOrEmpty())
                validLogonContext = this.LogonContext;

            return validLogonContext;
        }

        public void InitViewModel(
            CkgBaseViewModel viewModel,
            IAppSettings appSettings, ILogonContextDataService logonContext)
        {
            viewModel.Init(appSettings, GetValidLogonContext(logonContext));
        }


        public void InitViewModel<T>(
            CkgBaseViewModel viewModel,
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1)
            where T : class
        {
            viewModel.Init(appSettings, GetValidLogonContext(logonContext), dataService1);
        }

        public void InitViewModel<T, TU>(
            CkgBaseViewModel viewModel,
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1, TU dataService2)
            where T : class
            where TU : class
        {
            viewModel.Init(appSettings, GetValidLogonContext(logonContext), dataService1, dataService2);
        }

        public void InitViewModel<T, TU, TV>(
            CkgBaseViewModel viewModel,
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1, TU dataService2, TV dataService3)
            where T : class
            where TU : class
            where TV : class
        {
            viewModel.Init(appSettings, GetValidLogonContext(logonContext), dataService1, dataService2, dataService3);
        }

        public void InitViewModel<T, TU, TV, TW>(
            CkgBaseViewModel viewModel,
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1, TU dataService2, TV dataService3, TW dataService4)
            where T : class
            where TU : class
            where TV : class
            where TW : class
        {
            viewModel.Init(appSettings, GetValidLogonContext(logonContext), dataService1, dataService2, dataService3, dataService4);
        }

        [HttpPost]
        public ActionResult LogonContextSwitchLogonLevel(string level)
        {
            LogonLevel logonLevel;
            if (!Enum.TryParse(level, true, out logonLevel))
                return Json(new { message = "Fehlerhafter Logon-Level!" });

            LogonContext.UserLogonLevel = logonLevel;

            return Json(new { message = "ok" });
        }

        [HttpPost]
        public ActionResult LogonContextPersistColumns(string jsonColumns, bool persistInDb)
        {
            LogonContext.CurrentGridColumns = jsonColumns;

            if (persistInDb)
            {
                var jCols = jsonColumns.GetGridColumns();
                var colMembers = jCols.Select(j => j.member).ToList();

                LogonContext.SetUserGridColumnNames(GridGroup, string.Join(",", colMembers));
            }

            return Json(new { message = "ok" });
        }

        [HttpPost]
        public override JsonResult GetAutoPostcodeCityMappings(string plz)
        {
            return Json(new { items = AppSettings.GetAddressPostcodeCityMappings(plz) });
        }

        protected override int GetTokenExpirationMinutes()
        {
            return AppSettings.TokenExpirationMinutes;
        }


        #region Export

        protected virtual IEnumerable GetGridExportData()
        {
            return new List<object>();
        }

        [ValidateInput(false)]
        public ActionResult GridDataExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = GetGridExportData().GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("ExcelExport", dt);

            return new EmptyResult();
        }

        [ValidateInput(false)]
        public ActionResult GridDataExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = GetGridExportData().GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("ExcelExport", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        protected void AddModelError<T>(Expression<Func<T, object>> expression, string errorMessage)
        {
            ModelState.AddModelError(expression.GetPropertyName(), errorMessage);
        }

        #endregion
    }
}
