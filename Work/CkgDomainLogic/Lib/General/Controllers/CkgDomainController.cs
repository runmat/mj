using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.DataConverter.ViewModels;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Contracts;
using MvcTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;

namespace CkgDomainLogic.General.Controllers
{
    public abstract class CkgDomainController : LogonCapableController, IPersistableSelectorProvider, IGridColumnsAutoPersistProvider, IGridSettingsAdministrationProvider
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

        public bool GridSettingsAdminMode
        {
            get { return SessionHelper.GetSessionValue("GridSettingsAdminMode", false); }
            set
            {
                SessionHelper.SetSessionValue("GridSettingsAdminMode", value);
                SessionHelper.SetSessionValue("GridSettingsAdminModeChanged", true);
            }
        }

        protected GridSettings GridCurrentSettings
        {
            get { return SessionHelper.GetSessionObject("GridCurrentSettings", () => new GridSettings()); }
            set { SessionHelper.SetSessionValue("GridCurrentSettings", value); }
        }

        protected Dictionary<string, GridSettings> GridSettingsPerName
        {
            get { return SessionHelper.GetSessionObject("GridSettingsPerName", () => new Dictionary<string, GridSettings>()); }
            set { SessionHelper.SetSessionValue("GridSettingsPerName", value); }
        }


        protected string GridCurrentColumns
        {
            get { return GridCurrentSettings.Columns; }
            set { GridCurrentSettings.Columns = value; }
        }

        public GridSettings GridCurrentSettingsAutoPersist
        {
            get
            {
                var gridCurrentGetAutoPersistColumnsKey = SessionHelper.GridCurrentGetAutoPersistColumnsKey();
                if (gridCurrentGetAutoPersistColumnsKey.IsNullOrEmpty())
                    return null;

                var gridCurrentAutoPersistColumnsItems = PersistanceGetObjects<GridSettings>(gridCurrentGetAutoPersistColumnsKey);
                var gridCurrentAutoPersistColumnsItem = (gridCurrentAutoPersistColumnsItems == null ? null : gridCurrentAutoPersistColumnsItems.FirstOrDefault());

                if (gridCurrentAutoPersistColumnsItem == null)
                {
                    // Try loading from customer administrated grid settings (customer presets)
                    gridCurrentAutoPersistColumnsItems = PersistanceGetObjects<GridSettings>(gridCurrentGetAutoPersistColumnsKey, CustomerAdminPersistanceOwnerKey);
                    gridCurrentAutoPersistColumnsItem = (gridCurrentAutoPersistColumnsItems == null ? null : gridCurrentAutoPersistColumnsItems.FirstOrDefault());

                    if (gridCurrentAutoPersistColumnsItem != null)
                    {
                        // copy customer presets to user individual settings
                        var gridSettings = ModelMapping.Copy(gridCurrentAutoPersistColumnsItem);
                        gridSettings.ObjectKey = "";
                        gridSettings.ObjectName = "GridCurrentAutoPersistColumns";
                        PersistanceSaveObject(gridCurrentGetAutoPersistColumnsKey, gridSettings.ObjectKey, gridSettings);
                    }
                }

                if (gridCurrentAutoPersistColumnsItem == null)
                    return null;

                return gridCurrentAutoPersistColumnsItem;
            } 
            private set
            {
                var gridCurrentGetAutoPersistColumnsKey = SessionHelper.GridCurrentGetAutoPersistColumnsKey();
                if (gridCurrentGetAutoPersistColumnsKey.IsNullOrEmpty())
                    return;

                var objectKey = (GridCurrentSettingsAutoPersist == null ? "" : GridCurrentSettingsAutoPersist.ObjectKey);
                var gridSettings = ModelMapping.Copy(value);

                gridSettings.ObjectKey = objectKey;
                gridSettings.ObjectName = "GridCurrentAutoPersistColumns";

                PersistanceSaveObject(gridCurrentGetAutoPersistColumnsKey, gridSettings.ObjectKey, gridSettings);
            }
        }

        public void PersistableSelectorResetCurrent()
        {
            PersistableSelectorObjectKeyCurrent = null;
        }

        public void ResetGridCurrentModelTypeAutoPersist()
        {
            GridCurrentSettings = null;
            SessionHelper.SetSessionObject("Telerik_Grid_CurrentModelTypeForAutoPersistColumns", null);
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
            var typeName = typeof(T).Name;
            SessionHelper.SetSessionValue(typeName + "_valid", "valid");
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
            if (LogonContext != null && LogonContext.UserName.IsNotNullOrEmpty())
                validLogonContext = LogonContext;

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

        public ActionResult GridSettingsPersist(string jsonColumns, string orderBy, string filterBy, string groupBy, bool autoPersistInDb, string gridName)
        {
            GridCurrentSettings = new GridSettings
                {
                    ObjectKey = PersistableSelectorObjectKeyCurrent,
                    Columns = jsonColumns,
                    OrderBy = orderBy,
                    FilterBy = filterBy,
                    GroupBy = groupBy
                };

            if (autoPersistInDb && PersistableSelectorObjectKeyCurrent.IsNullOrEmpty())
                GridCurrentSettingsAutoPersist = GridCurrentSettings;

            if (!GridSettingsPerName.ContainsKey(gridName))
                GridSettingsPerName.Add(gridName, new GridSettings());
            GridSettingsPerName[gridName] = GridCurrentSettings;

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
        public ActionResult GridDataExportFilteredExcel(int page, string orderBy, string filterBy, string groupBy)
        {
            var exportList = GetGridExportData();
            var modelType = exportList.GetItemType();

            var dt = exportList.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 

            var grid = (IGrid)SessionHelper.GetSessionObject(string.Format("Telerik_Grid_{0}", modelType.Name));
            if (grid == null || groupBy.NotNullOrEmpty().Length <= 1)
                new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("ExcelExport", dt);
            else
            {
                // Advanced Excel Export with groupings, aggregates, subtotals, etc
                // no Aspose needed anymore, based on SpreadsheetLight library + "DocumentFormat Open XML" library

                var gridAggregateColumnNames = GetValidGridAggregatesColumnNames(grid);

                var groupByFirstColumn = groupBy.Split('~').FirstOrDefault();
                if (groupByFirstColumn.NotNullOrEmpty().Contains("-"))
                    groupByFirstColumn = groupBy.Split('-').FirstOrDefault();

                new ExcelDocumentFactory().CreateExcelGroupedDocumentAndSendAsResponse("ExcelExport", dt, gridAggregateColumnNames, groupByFirstColumn);
            }

            return new EmptyResult();
        }

        [ValidateInput(false)]
        public ActionResult GridDataExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = GetGridExportData().GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
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

            var test = AdressenPflegeViewModel.GetItem(id).SetInsertMode(AdressenPflegeViewModel.InsertMode);

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
            AdressenPflegeViewModel.AdressenKennungGruppeChange(model.AdressenKennungGruppe, model.AdressenKennungTemp);
            ModelState.Clear();

            return PartialView("Partial/AdressenGruppeKennungSelect", AdressenPflegeViewModel);
        }

        public ActionResult ExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = AdressenPflegeViewModel.AdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Adressen", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = AdressenPflegeViewModel.AdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Adressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion


        #region Persistence Service, General Objects

        protected virtual string GetPersistanceOwnerKey()
        {
            return LogonContext.UserName;
        }

        private string CustomerAdminPersistanceOwnerKey
        {
            get { return string.Format("ADMIN_for_customer_{0}", LogonContext.KundenNr); }
        }

        protected string GetRealPersistanceOwnerKey()
        {
            if (GridSettingsAdminMode)
                return CustomerAdminPersistanceOwnerKey;

            return GetPersistanceOwnerKey();
        }

        string GetRealPersistanceOwnerKeyMultiKey(string ownerKey)
        {
            return ownerKey.NotNullOrEmpty().ToUpper() == "ALL" ? null : ownerKey ?? GetRealPersistanceOwnerKey();
        }


        private IEnumerable<IPersistableObjectContainer> PersistanceGetObjectContainers(string groupKey, string ownerKey = null)
        {
            var pService = LogonContext.PersistanceService;
            if (pService == null)
                return new List<IPersistableObjectContainer>();

            return pService.GetObjectContainers(GetRealPersistanceOwnerKeyMultiKey(ownerKey), groupKey);
        }

        protected List<T> PersistanceGetObjects<T>(string groupKey, string ownerKey = null)
        {
            return PersistanceGetObjectContainers(groupKey, ownerKey)
                    .Select(pContainer => (T)pContainer.Object)
                        .ToListOrEmptyList();
        }

        protected List<T> PersistanceGetObjects2<T>(string groupKey, string ownerKey = null)
        {
            return PersistanceGetObjectContainers(groupKey, ownerKey)
                    .Select(pContainer => (T)pContainer.Object2)
                        .ToListOrEmptyList();
        }

        protected void PersistanceSaveObject(string groupKey, string objectKey, ref IPersistableObject o, ref IPersistableObject o2)
        {
            var pService = LogonContext.PersistanceService;
            if (pService == null)
                return;

            if (o == null && o2 == null)
                return;

            pService.SaveObject(objectKey, GetRealPersistanceOwnerKey(), groupKey, LogonContext.UserName, ref o, ref o2);
        }

        protected IPersistableObject PersistanceSaveObject(string groupKey, string objectKey, IPersistableObject o)
        {
            if (o == null)
                return null;

            var pService = LogonContext.PersistanceService;
            if (pService == null)
                return o;

            return pService.SaveObject(objectKey, GetRealPersistanceOwnerKey(), groupKey, LogonContext.UserName, o);
        }

        protected void PersistanceDeleteAllObjects(string groupKey, string ownerKey = null, string additionalFilter = null)
        {
            var pService = LogonContext.PersistanceService;
            if (pService == null)
                return;

            pService.DeleteAllObjects(GetRealPersistanceOwnerKeyMultiKey(ownerKey), groupKey, additionalFilter);
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


        #region Shopping Cart (based on 'Persistence Service')

        protected virtual IEnumerable ShoppingCartLoadItems()
        {
            return null;
        }

        protected virtual void ShoppingCartOnShow()
        {
            
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

        protected IPersistableObject ShoppingCartSaveItem(string groupKey, IPersistableObject o)
        {
            IPersistableObject savedObject2 = null;
            PersistanceSaveObject(groupKey, o.ObjectKey, ref o, ref savedObject2);
            ShoppingCartLoadAndCacheItems();
            return o;
        }

        [HttpPost]
        public ActionResult ShoppingCartGridShow()
        {
            ShoppingCartOnShow();
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
            var dt = ShoppingCartItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Warenkorb", dt);

            return new EmptyResult();
        }

        public ActionResult ShoppingCartExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ShoppingCartItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
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

        public virtual void ShoppingCartItemSelect(string objectKey, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var item = ShoppingCartItemsFilteredAsStore.FirstOrDefault(f => f.ObjectKey == objectKey);
            if (item == null)
                return;

            item.IsSelected = select;
            allSelectionCount = ShoppingCartItemsFilteredAsStore.Count(c => c.IsSelected);
        }

        public virtual void ShoppingCartItemsSelect(bool select, out int allSelectionCount, out int allCount)
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


        #region EVB-Prüfung -> Rückgabe der Versicherung

        [HttpPost]
        public JsonResult GetEvbVersInfo(string evb)
        {
            if (evb.IsNullOrEmpty() || evb.Length < 2)
                return null;

            evb = evb.Substring(0, 2).ToUpper();

            var viewModel = AdressenPflegeViewModel;
            string message;
            bool isValid;
            viewModel.GetEvbInstantInfo(evb, out message, out isValid);

            return Json(new { message = message, isValid = isValid });
        }
        #endregion


        #region Persistence Service, Selector Persistence

        private static string PersistableSelectorGroupKeyCurrent
        {
            get { return SessionHelper.GetSessionString("PersistableSelectorGroupKeyCurrent"); }
            set { SessionHelper.SetSessionValue("PersistableSelectorGroupKeyCurrent", value); }
        }

        private string PersistableSelectorObjectKeyCurrent
        {
            get { return SessionHelper.GetSessionString("PersistableSelectorObjectKeyCurrent"); }
            set
            {
                SessionHelper.SetSessionValue("PersistableSelectorObjectKeyCurrent", value);
                
                PersistableGridSettingsCurrentLoad(value);
            }
        }
        
        protected static bool PersistableMode { get { return PersistableSelectorIsPersistMode; } }
        
        private static bool PersistableSelectorIsPersistMode
        {
            get { return SessionHelper.GetSessionValue("PersistableSelectorIsPersistMode", false); }
            set { SessionHelper.SetSessionValue("PersistableSelectorIsPersistMode", value); }
        }

        private static string PersistableSelectorPersistMode
        {
            get { return SessionHelper.GetSessionString("PersistableSelectorPersistMode"); }
            set { SessionHelper.SetSessionValue("PersistableSelectorPersistMode", value); }
        }

        public static List<IPersistableObject> PersistableSelectorItems
        {
            get { return (List<IPersistableObject>)SessionHelper.GetSessionObject("PersistableSelectorItems"); }
            set { SessionHelper.SetSessionValue("PersistableSelectorItems", value); }
        }

        static void PersistableSelectorPersistModeReset()
        {
            PersistableSelectorIsPersistMode = false;
            PersistableSelectorPersistMode = null;
        }

        protected PartialViewResult PersistablePartialView<T>(string viewName, T model) where T : class, new()
        {
            if (PersistableSelectorIsPersistMode)
            {
                // remove "No data found" error if we are in "form persisting" mode:
                var noDataFoundModelError = ModelState.FirstOrDefault(ms => ms.Value.Errors != null && ms.Value.Errors.Any(error => error.ErrorMessage == Localize.NoDataFound));
                if (noDataFoundModelError.Key != null && noDataFoundModelError.Value != null)
                    ModelState.Remove(noDataFoundModelError);
            }

            var persistableSelector = (model as IPersistableObject);

            if (!ModelState.IsValid || !PersistableSelectorIsPersistMode)
            {
                if (persistableSelector != null && persistableSelector.ObjectKey != null)
                {
                    PersistableSelectorObjectKeyCurrent = persistableSelector.ObjectKey;
                    PersistableGridSettingsCurrentLoad(persistableSelector.ObjectKey, true);
                }

                return PartialView(viewName, model);
            }

            var persistMode = (PersistableSelectorPersistMode ?? "save");
            
            var modeLocalizationMessage = "";
            if (persistableSelector != null)
            {
                switch (persistMode)
                {
                    case "load":
                        model = PersistablePartialViewLoad<T>();

                        modeLocalizationMessage = Localize.FormPersistableSelector_FormLoad + " " + Localize.Successful.ToLower();
                        break;

                    case "delete":
                        PersistablePartialViewDelete();
                        model = new T();

                        modeLocalizationMessage = Localize.FormPersistableSelector_FormDelete + " " + Localize.Successful.ToLower();
                        break;

                    case "clear":
                        PersistablePartialViewClear();
                        model = new T();

                        modeLocalizationMessage = Localize.FormPersistableSelector_FormReset + " " + Localize.Successful.ToLower();
                        break;
                    
                    case "save":
                        model = (T)PersistablePartialViewSave(persistableSelector);

                        modeLocalizationMessage = Localize.FormPersistableSelector_FormSave + " " + Localize.Successful.ToLower();
                        break;
                    
                    case "saveas":
                        model = (T)PersistablePartialViewSaveAs(persistableSelector);

                        modeLocalizationMessage = Localize.FormPersistableSelector_FormSaveAs + " " + Localize.Successful.ToLower();
                        break;
                }
            }

            PersistableSelectorPersistModeReset();

            var persistMessage = string.Format("{0}{1}", MvcTag.FormPersistenceModeErrorPrefix, modeLocalizationMessage);
            ModelState.AddModelError("", persistMessage);

            return PartialView(viewName, model);
        }

        private T PersistablePartialViewLoad<T>() where T : class, new()
        {
            var persistableSelector = (T)PersistableSelectorItems.FirstOrDefault(p => p.ObjectKey == PersistableSelectorObjectKeyCurrent);

            ModelState.Clear();

            PersistableGridSettingsCurrentLoad(GridCurrentSettings.ObjectKey, true);

            var selectorOnlyPersistableProperties = ModelMapping.CopyOnlyPersistableProperties(persistableSelector);

            return selectorOnlyPersistableProperties;
        }

        private void PersistablePartialViewDelete()
        {
            PersistanceDeleteObject(PersistableSelectorObjectKeyCurrent);

            PersistablePartialViewClear();
        }

        private void PersistablePartialViewClear()
        {
            PersistableSelectorResetCurrent();
            ModelState.Clear();
        }

        private IPersistableObject PersistablePartialViewSave(IPersistableObject persistableSelector, string defaultObjectName = null)
        {
            IPersistableObject dummy = null;
            PersistanceSaveObject(PersistableSelectorGroupKeyCurrent, persistableSelector.ObjectKey, ref persistableSelector, ref dummy);
            if (persistableSelector != null)
            {
                if (persistableSelector.ObjectName.IsNullOrEmpty())
                {
                    persistableSelector.ObjectName = PersistableSelectorsGetDefaultObjectNameFor(defaultObjectName ?? Localize.MyReport);
                    PersistanceSaveObject(PersistableSelectorGroupKeyCurrent, persistableSelector.ObjectKey, ref persistableSelector, ref dummy);
                }

                PersistableSelectorObjectKeyCurrent = persistableSelector.ObjectKey;

                ModelState.SetModelValue("ObjectKey", persistableSelector.ObjectKey);
                ModelState.SetModelValue("ObjectName", persistableSelector.ObjectName);
                ModelState.SetModelValue("EditUser", persistableSelector.EditUser);
                ModelState.SetModelValue("EditDate", persistableSelector.EditDate);
            }

            return persistableSelector;
        }

        private IPersistableObject PersistablePartialViewSaveAs(IPersistableObject persistableSelector)
        {
            var defaultObjectName = persistableSelector.ObjectName;
            persistableSelector.ObjectName = null;
            persistableSelector.ObjectKey = null;

            persistableSelector = PersistablePartialViewSave(persistableSelector, defaultObjectName);

            ModelState.Clear();

            return persistableSelector;
        }

        [HttpPost]
        public ActionResult PersistablePartialViewSetMode(string mode, string objectKey)
        {
            PersistableSelectorIsPersistMode = true;
            PersistableSelectorPersistMode = mode;
            PersistableSelectorObjectKeyCurrent = objectKey;

            if (PersistableSelectorPersistMode == "savegrid")
            {
                IPersistableObject dummy = null;
                IPersistableObject gridSettings = GridCurrentSettings;
                
                PersistanceSaveObject(PersistableSelectorGroupKeyCurrent, objectKey, ref dummy, ref gridSettings);
                GridCurrentSettings = (gridSettings as GridSettings);

                PersistableSelectorPersistModeReset();
            }

            return Json(new { success = true });
        }

        string PersistableSelectorsGetDefaultObjectNameFor(string objectNameThumb)
        {
            objectNameThumb = objectNameThumb.NotNullOrEmpty().TrimEnd(new [] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' '});

            Func<IPersistableObject, bool> compareFunction = (p => p.ObjectName.NotNullOrEmpty().ToLower().StartsWith(objectNameThumb.ToLower()));

            if (PersistableSelectors.None(compareFunction)) 
                return objectNameThumb;

            return string.Format("{0} {1}", objectNameThumb, PersistableSelectors.Count(compareFunction));
        }

        private void PersistableGridSettingsCurrentLoad(string objectKeyCurrent, bool forceLoad = false)
        {
            if (objectKeyCurrent.IsNullOrEmpty())
            {
                GridCurrentSettings = null;
                return;
            }

            if (PersistableSelectorGroupKeyCurrent.IsNullOrEmpty() || objectKeyCurrent.IsNullOrEmpty())
                return;

            if (!forceLoad && GridCurrentSettings != null && GridCurrentSettings.ObjectKey == objectKeyCurrent)
                return;

            var gridSettingsItems = PersistanceGetObjects2<IPersistableObject>(PersistableSelectorGroupKeyCurrent).OfType<GridSettings>();
            if (gridSettingsItems.None())
                return;

            GridCurrentSettings = gridSettingsItems.FirstOrDefault(gs => gs.ObjectKey == objectKeyCurrent);
        }


        #region IPersistableSelectorProvider

        public List<IPersistableObject> PersistableSelectors
        {
            get { return PersistableSelectorItems; }
        }

        public void PersistableSelectorsLoad<T>(string groupKey = null) where T : class, new()
        {
            var relativeUrl = LogonContextHelper.GetAppUrlCurrent();
            PersistableSelectorGroupKeyCurrent = groupKey ?? (string.Format("{0}_{1}", relativeUrl, typeof(T).Name).ToLower());
            PersistableSelectorItems = PersistanceGetObjects<T>(PersistableSelectorGroupKeyCurrent).Cast<IPersistableObject>().ToListOrEmptyList();
        }

        public void PersistableSelectorsLoad() 
        {
            if (PersistableSelectorGroupKeyCurrent.IsNullOrEmpty())
                return;

            PersistableSelectorItems = PersistanceGetObjects<IPersistableObject>(PersistableSelectorGroupKeyCurrent).Cast<IPersistableObject>().ToListOrEmptyList();
        }

        #endregion


        public bool FormSettingsAdminModeWysiwygMode
        {
            get { return SessionHelper.FormSettingsAdminModeWysiwygModeGet(); }
            set { SessionHelper.FormSettingsAdminModeWysiwygModeSet(value); }
        }

        protected override PartialViewResult PartialView(string viewName, object model)
        {
            if (FormSettingsAdminModeWysiwygMode)
                ModelState.Clear();

            var viewHtml = base.PartialView(viewName, model);

            FormSettingsAdminModeWysiwygMode = false;

            return viewHtml;
        }

        public ActionResult FormSettingsAdminModeSetWysiwygMode(string modelTypeName, string propertyName)
        {
            FormSettingsAdminModeWysiwygMode = true;

            return new EmptyResult();
        }

        public ActionResult GridSettingsAdminModeActivate(bool set)
        {
            GridSettingsAdminMode = set;

            return new EmptyResult();
        }


        #endregion

        public string GetConfigValue(string context, string keyName)
        {
            return GeneralConfiguration.GetConfigValue(context, keyName);
        }

        public string GetConfigValueForCurrentCustomer(string keyName)
        {
            return ApplicationConfiguration.GetApplicationConfigValue(keyName, "0", LogonContext.Customer.CustomerID);
        }
    }
}
