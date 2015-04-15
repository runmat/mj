using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Controllers;
using MvcTools.Web;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;

namespace CkgDomainLogic.General.Controllers
{
    public abstract class CkgDomainController : LogonCapableController
    {
        public IAppSettings AppSettings { get; protected set; }

        public virtual AdressenPflegeViewModel AdressenPflegeViewModel { get { return GetViewModel<AdressenPflegeViewModel>(); } }

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

        private static string[] GetValidGridAggregatesColumnNames(IGrid grid) 
        {
            var columns = grid.Columns.Cast<IGridBoundColumn>();
            var subTotalColumns = columns.Where(c => c.ClientGroupFooterTemplate.IsNotNullOrEmpty());
            var validSubTotalColumns = subTotalColumns.Where(c => c.MemberType != typeof(string));

            return validSubTotalColumns.Select(c => c.Member).ToArray();
        }

        [ValidateInput(false)]
        public ActionResult GridDataExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var exportList = GetGridExportData();
            var modelType = exportList.GetItemType();

            var dt = exportList.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);

            var grid = (IGrid)SessionHelper.GetSessionObject(string.Format("Telerik_Grid_{0}", modelType.Name));
            if (grid == null || grid.Grouping == null || grid.Grouping.Groups == null || grid.Grouping.Groups.Count == 0)
                new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("ExcelExport", dt);
            else
            {
                // Advanced Excel Export with groupings, aggregates, subtotals, etc
                // no Aspose needed anymore, based on SpreadsheetLight library + "DocumentFormat Open XML" library

                var gridAggregateColumnNames = GetValidGridAggregatesColumnNames(grid);

                new ExcelDocumentFactory().CreateExcelGroupedDocumentAndSendAsResponse("ExcelExport", dt, gridAggregateColumnNames);
            }

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


        #region Adressen Pflege

        [GridAction]
        public ActionResult AdressenAjaxBinding()
        {
            var items = AdressenPflegeViewModel.AdressenFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridAdresse(string filterValue, string filterColumns)
        {
            AdressenPflegeViewModel.FilterVersandAdressen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult EditAddress(int id)
        {
            AdressenPflegeViewModel.InsertMode = false;
            ModelState.Clear();
            return PartialView("AdressenPflege/AdressenDetailsForm", AdressenPflegeViewModel.GetItem(id).SetInsertMode(AdressenPflegeViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult NewAddress()
        {
            AdressenPflegeViewModel.InsertMode = true;
            ModelState.Clear();
            return PartialView("AdressenPflege/AdressenDetailsForm", AdressenPflegeViewModel.NewItem().SetInsertMode(AdressenPflegeViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult DeleteAddress(int id)
        {
            AdressenPflegeViewModel.RemoveItem(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AddressDetailsFormSave(Adresse model)
        {
            // Avoid ModelState clearing on saving 
            // => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();

            var viewModel = AdressenPflegeViewModel;

            viewModel.ValidateModel(model, viewModel.InsertMode, ModelState.AddModelError);

            if (ModelState.IsValid)
                model = viewModel.SaveItem(model, ModelState.AddModelError);

            model.IsValid = ModelState.IsValid;
            model.InsertModeTmp = viewModel.InsertMode;

            return PartialView("AdressenPflege/AdressenDetailsForm", model);
        }

        [CkgApplication]
        public ActionResult AdressenPflege(string kennung, string kdnr)
        {
            AdressenPflegeViewModel.AdressenDataInit(kennung ?? "VERSANDADRESSE", kdnr ?? LogonContext.KundenNr);
            AdressenPflegeViewModel.AdressenKennungGruppeInit();

            return View(AdressenPflegeViewModel);
        }

        [HttpPost]
        public ActionResult AdressenKennungChange(AdressenPflegeViewModel model)
        {
            AdressenPflegeViewModel.AdressenKennungGruppeChange(model.AdressenKennungGruppe, model.AdressenKennung);
            ModelState.Clear();

            return PartialView("Partial/AdressenGruppeKennungSelect", AdressenPflegeViewModel);
        }

        public ActionResult ExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = AdressenPflegeViewModel.AdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Adressen", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = AdressenPflegeViewModel.AdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Adressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion



        #region Persistance Service

        protected virtual string GetPersistanceOwnerKey()
        {
            return LogonContext.UserName;
        }


        private IEnumerable<IPersistableObjectContainer> PersistanceGetObjectContainers(string groupKey)
        {
            var pService = LogonContext.PersistanceService;
            if (pService == null)
                return new List<IPersistableObjectContainer>();

            return pService.GetObjectContainers(GetPersistanceOwnerKey(), groupKey);
        }

        protected List<T> PersistanceGetObjects<T>(string groupKey)
        {
            return PersistanceGetObjectContainers(groupKey)
                    .Select(pContainer => (T)pContainer.Object)
                        .ToListOrEmptyList();
        }

        protected void PersistanceSaveObject(string groupKey, IPersistableObject o)
        {
            var pService = LogonContext.PersistanceService;
            if (pService == null)
                return;

            pService.SaveObject(o.ObjectKey, GetPersistanceOwnerKey(), groupKey, LogonContext.UserName, o);
        }

        protected void PersistanceDeleteObject(string objectKey)
        {
            if (objectKey.IsNullOrEmpty())
                return;

            var pService = LogonContext.PersistanceService;
            if (pService == null)
                return;

            pService.DeleteObject(objectKey);
        }

        #endregion

        
        #region Shopping Cart (based on 'Persistance Service')

        protected virtual IEnumerable ShoppingCartLoadItems()
        {
            return null;
        }

        protected virtual void ShoppingCartEditItem(string objectKey)
        {
        }

        protected virtual void ShoppingCartFilterItems(string filterValue, string filterProperties)
        {
        }

        private static string ShoppingCartEditItemKeyCurrent
        {
            get { return SessionHelper.GetSessionString("ShoppingCartEditItemKeyCurrent"); }
            set { SessionHelper.SetSessionValue("ShoppingCartEditItemKeyCurrent", value); }
        }

        protected static IEnumerable ShoppingCartItems
        {
            get { return (IEnumerable)SessionHelper.GetSessionObject("ShoppingCartItems"); }
            set { SessionHelper.SetSessionValue("ShoppingCartItems", value); }
        }

        private static IEnumerable ShoppingCartItemsFiltered
        {
            get { return (IEnumerable)SessionHelper.GetSessionObject("ShoppingCartItemsFiltered"); }
            set { SessionHelper.SetSessionValue("ShoppingCartItemsFiltered", value); }
        }

        protected static void ShoppingCartFilterGenericItems<T>(string filterValue, string filterProperties)
        {
            ShoppingCartItemsFiltered = ShoppingCartItems.Cast<T>().ToList().SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        private static void ShoppingCartPushEditItemKey(string id)
        {
            ShoppingCartEditItemKeyCurrent = id;
        }

        protected static string ShoppingCartPopEditItemKey()
        {
            var id = ShoppingCartEditItemKeyCurrent;
            ShoppingCartEditItemKeyCurrent = null;
            return id;
        }

        protected void ShoppingCartLoadAndCacheItems()
        {
            ShoppingCartItems = ShoppingCartLoadItems();
            ShoppingCartItemsFiltered = ShoppingCartItems;
        }

        protected List<T> ShoppingCartLoadGenericItems<T>(string groupKey)
        {
            return PersistanceGetObjects<T>(groupKey);
        }

        protected object ShoppingCartGetItem(string id)
        {
            return ShoppingCartItems.Cast<Store>().FirstOrDefault(item => item.ObjectKey == id);
        }

        protected void ShoppingCartSaveItem(string groupKey, IPersistableObject o)
        {
            PersistanceSaveObject(groupKey, o);
            ShoppingCartLoadAndCacheItems();
        }

        [HttpPost]
        public ActionResult ShoppingCartGridShow()
        {
            ShoppingCartLoadAndCacheItems();

            return PartialView("ShoppingCart/PortletGrid");
        }

        [GridAction]
        public ActionResult ShoppingCartAjaxBinding()
        {
            var items = ShoppingCartItemsFiltered;
            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridShoppingCart(string filterValue, string filterColumns)
        {
            ShoppingCartFilterItems(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult ShoppingCartItemEdit(string id)
        {
            ShoppingCartPushEditItemKey(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult ShoppingCartItemRemove(string id)
        {
            PersistanceDeleteObject(id);

            ShoppingCartLoadAndCacheItems();

            return new EmptyResult();
        }

        public ActionResult ShoppingCartExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ShoppingCartItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Warenkorb", dt);

            return new EmptyResult();
        }

        public ActionResult ShoppingCartExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ShoppingCartItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Warenkorb", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        public ActionResult ShoppingCartMenuRefreshCount()
        {
            var count = ShoppingCartItems.Cast<object>().Count();
            
            return Json(new { count });
        }

        protected static List<Store> ShoppingCartItemsFilteredAsStore
        {
            get { return ShoppingCartItems.Cast<Store>().ToListOrEmptyList(); }
        }


        //
        // <Multi Selection>
        //

        public void ShoppingCartItemSelect(string objectKey, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var item = ShoppingCartItemsFilteredAsStore.FirstOrDefault(f => f.ObjectKey == objectKey);
            if (item == null)
                return;

            item.IsSelected = select;
            allSelectionCount = ShoppingCartItemsFilteredAsStore.Count(c => c.IsSelected);
        }

        public void ShoppingCartItemsSelect(bool select, out int allSelectionCount, out int allCount)
        {
            ShoppingCartItemsFilteredAsStore.ForEach(f => f.IsSelected = select);

            allSelectionCount = ShoppingCartItemsFilteredAsStore.Count(c => c.IsSelected);
            allCount = ShoppingCartItemsFilteredAsStore.Count();
        }

        [HttpPost]
        public JsonResult ShoppingCartItemSelectionChanged(string objectKey, bool isChecked)
        {
            int allSelectionCount, allCount = 0;
            if (objectKey.IsNullOrEmpty())
                ShoppingCartItemsSelect(isChecked, out allSelectionCount, out allCount);
            else
                ShoppingCartItemSelect(objectKey, isChecked, out allSelectionCount);

            return Json(new { allSelectionCount, allCount });
        }

        [HttpPost]
        public virtual ActionResult ShoppingCartSelectedItemsSubmit()
        {
            return Json(new { success = true });
        }

        

        //
        // </Multi Selection>
        //


        #endregion
    }
}
