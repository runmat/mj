// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using GeneralTools.Models;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Resources;
using ServicesMvc.DomainCommon.Models;
using MvcTools.Web;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public enum GridAdminMode { GridColumns, FormControls  };

    public class GridAdminViewModel : CkgBaseViewModel
    {
        [ModelMappingCopyIgnore]
        public GridAdminMode Mode { get; set; }

        [ModelMappingCopyIgnore]
        public bool GlobalTranslationDangerous { get; set; }

        [XmlIgnore]
        [ModelMappingCopyIgnore]
        public IGridAdminDataService DataService { get { return CacheGet<IGridAdminDataService>(); } }

        public bool TmpDeleteCustomerTranslation { get; set; }
        public bool TmpSwitchGlobalFlag { get; set; }

        [ModelMappingCopyIgnore]
        public int CurrentCustomerID { get; set; }

        [ModelMappingCopyIgnore]
        public string CurrentResourceID { get; set; }

        public TranslatedResource CurrentTranslatedResource { get; set; }

        public TranslatedResourceCustom CurrentTranslatedResourceCustomer { get; set; }

        [ModelMappingCopyIgnore]
        public ReportSolution ReportSettings { get; set; }


        [ModelMappingCopyIgnore]
        public Customer CurrentCustomer { get; set; }

        [ModelMappingCopyIgnore]
        public List<Customer> Customers
        {
            get
            {
                return PropertyCacheGet(() => DataService.GetCustomers().OrderBy(c => c.Customername).ToListOrEmptyList()
                            .CopyAndInsertAtTop(new Customer { CustomerID = -1, Customername = "" }));
            }
        }

        [ModelMappingCopyIgnore]
        public User CurrentUser { get; set; }

        [ModelMappingCopyIgnore]
        public List<User> Users { get; set; }

        [ModelMappingCopyIgnore]
        public string ModelTypeName { get; set; }

        [ModelMappingCopyIgnore]
        public string PropertyName { get; set; }

        [LocalizedDisplay(LocalizeConstants.FieldTranslationIsHidden)]
        public bool IsHidden { get; set; }

        [LocalizedDisplay(LocalizeConstants.FieldTranslationIsRequired)]
        public bool IsRequired { get; set; }

        [LocalizedDisplay(LocalizeConstants.FieldTranslationIsGlobal)]
        public bool IsGlobal { get; set; }


        public bool DataInit(Type modelType, string propertyName)
        {
            ModelTypeName = modelType.GetFullTypeName();
            PropertyName = propertyName;

            CurrentCustomerID = LogonContext.KundenNr.ToInt();

            if (!LoadTranslatedResourcesForProperty(true))
                return false;

            if (CurrentTranslatedResource != null && CurrentTranslatedResource.Resource != null)
            {
                GlobalTranslationDangerous = !CurrentTranslatedResource.Resource.ToLower().StartsWith("customizable");
                IsGlobal = (Mode == GridAdminMode.GridColumns || !GlobalTranslationDangerous);
            }

            if (!LoadTranslatedResourcesForProperty())
                return false;

            return true;
        }

        private bool LoadTranslatedResourcesForProperty(bool forceLoadingFromLocalizeAttribute = false)
        {
            var modelType = Type.GetType(ModelTypeName);
            if (modelType == null)
                 return false;

            var propertyInfo = modelType.GetProperty(PropertyName);
            if (propertyInfo == null)
                return false;

            var localizeAttribute = propertyInfo.GetCustomAttributes(true).OfType<LocalizedDisplayAttribute>().FirstOrDefault();
            if (localizeAttribute == null)
                return false;

            if (IsGlobal || forceLoadingFromLocalizeAttribute)
                CurrentResourceID = localizeAttribute.ResourceID;
            else
                CurrentResourceID = "InvoiceDate";

            CurrentTranslatedResource = DataService.TranslatedResourceLoad(CurrentResourceID);
            CurrentTranslatedResourceCustomer = DataService.TranslatedResourceCustomerLoad(CurrentResourceID, CurrentCustomerID);

            return true;
        }

        public void DataSave()
        {
            if (TmpSwitchGlobalFlag)
            {
                LoadTranslatedResourcesForProperty();
                return;
            }

            if (TmpDeleteCustomerTranslation)
            {
                DataService.TranslatedResourceCustomerDelete(CurrentTranslatedResourceCustomer);
                UpdateTranslationTimeStamp();
                return;
            }

            DataService.TranslatedResourceUpdate(CurrentTranslatedResource);
            DataService.TranslatedResourceCustomerUpdate(CurrentTranslatedResourceCustomer);
            UpdateTranslationTimeStamp();

            if (Mode == GridAdminMode.FormControls)
            {
                var key = CreateConfigKey();

                var appConf = DependencyResolver.Current.GetService<ICustomerConfigurationProvider>();
                if (appConf != null)
                {
                    appConf.SetCurrentBusinessCustomerConfigVal(key.Replace("[SELECTOR]", "REQUIRED"), IsRequired.ToString().ToLower());
                    appConf.SetCurrentBusinessCustomerConfigVal(key.Replace("[SELECTOR]", "HIDDEN"), IsHidden.ToString().ToLower());
                }
            }
        }

        string CreateConfigKey()
        {
            var partialViewUrl = SessionHelper.GetPartialViewUrlCurrent();
            if (partialViewUrl == null)
                return "";

            var modelType = Type.GetType(ModelTypeName);
            if (modelType == null)
                return "";

            var key = string.Format("[SELECTOR]: {0} - {1} - {2}", partialViewUrl, modelType.Name, PropertyName);

            return key;
        }

        void UpdateTranslationTimeStamp()
        {
            DataService.TranslationsMarkForRefresh();
        }

        public bool TrySetReportSettings(string un)
        {
            ReportSettings = new ReportSolution();

            var items = un.NotNullOrEmpty().Split('-');
            if (items.None() || items.Count() < 3)
                return false;

            var adminDate = new DateTime(items[2].ToLong(0));
            if ((DateTime.Now - adminDate).TotalMinutes > 1 && !Environment.MachineName.ToUpper().StartsWith("AHW"))
                return false;

            var appID = items[0].ToInt();
            if (appID < 0)
                return false;

            ReportSettings.CallingDateTime = adminDate;
            ReportSettings.AppID = items[0].ToInt();
            ReportSettings.AppFriendlyName = GetRelativeAppFriendlyName(ReportSettings.AppID);
            ReportSettings.AdminUserName = items[1];
            ReportSettings.AdminIsAuthorized = true;
            return true;
        }

        public void LoadUserForCustomer(int customerId)
        {
            Users = new List<User>();
            CurrentUser = null;

            CurrentCustomer = Customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (CurrentCustomer == null)
                return;

            Users = DataService.GetUsersForCustomer(CurrentCustomer)
                        .CopyAndInsertAtTop(new User { UserID = -1, Username = Localize.DropdownDefaultOptionNotSpecified });
        }

        public void SetCurrentUser(int userId)
        {
            if (Users == null || Users.None())
                return;

            CurrentUser = Users.FirstOrDefault(user => user.UserID == userId);
        }

        public User GetCurrentValidUser()
        {
            if (CurrentUser != null)
                return CurrentUser;

            return Users.FirstOrDefault(user => user.UserID > 0);
        }

        public string GetRelativeAppUrl()
        {
            var appUrl = DataService.GetAppUrl(ReportSettings.AppID);
            appUrl = LogonContext.FormatUrl(appUrl).ToString();

            return appUrl;
        }

        string GetRelativeAppFriendlyName(int appId)
        {
            var appFriendlyName = DataService.GetAppFriendlyName(appId);

            return appFriendlyName;
        }
    }
}
