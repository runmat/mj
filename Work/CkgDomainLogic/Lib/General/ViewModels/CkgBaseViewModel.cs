using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Services;
using GeneralTools.Models;
using MvcTools.Web;

namespace CkgDomainLogic.General.ViewModels
{
    public class CkgBaseViewModel : Store 
    {
        [XmlIgnore, ScriptIgnore]
        public IAppSettings AppSettings { get; private set; }

        [XmlIgnore, ScriptIgnore]
        public ILogonContextDataService LogonContext { get; private set; }


        private void InitDataService<T>(T dataService) where T : class
        {
            var cachedService = CacheGet<T>();
            if (cachedService == null)
                CacheSet(dataService);

            var generalCachedService = (CacheGet<T>() as ICkgGeneralDataService);
            if (generalCachedService != null)
                generalCachedService.Init(this.AppSettings, this.LogonContext);
        }

        public virtual void Init(IAppSettings appSettings, ILogonContextDataService logonContext)
        {
            this.AppSettings = appSettings;
            this.LogonContext = logonContext;

            DashboardTryInitCurrentReportSelector();
        }

        public void Init<T>(
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1)
            where T : class
        {
            Init(appSettings, logonContext);

            InitDataService(dataService1);
        }

        public void Init<T, TU>(
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1, TU dataService2)
            where T : class
            where TU : class
        {
            Init(appSettings, logonContext);

            InitDataService(dataService1);
            InitDataService(dataService2);
        }

        public void Init<T, TU, TV>(
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1, TU dataService2, TV dataService3)
            where T : class
            where TU : class
            where TV : class
        {
            Init(appSettings, logonContext);

            InitDataService(dataService1);
            InitDataService(dataService2);
            InitDataService(dataService3);
        }

        public void Init<T, TU, TV, TW>(
            IAppSettings appSettings, ILogonContextDataService logonContext,
            T dataService1, TU dataService2, TV dataService3, TW dataService4)
            where T : class
            where TU : class
            where TV : class
            where TW : class
        {
            Init(appSettings, logonContext);

            InitDataService(dataService1);
            InitDataService(dataService2);
            InitDataService(dataService3);
            InitDataService(dataService4);
        }


        #region Dashboard Support

        protected void DashboardSessionSaveCurrentReportSelector<T>(T reportSelector) where T : class
        {
            var callingMethod = new StackFrame(1, true).GetMethod();
            var dashboardItemsLoadMethodAttribute = callingMethod.GetCustomAttributes(true).OfType<DashboardItemsLoadMethodAttribute>().FirstOrDefault();
            if (dashboardItemsLoadMethodAttribute == null)
                return;

            var dashboardItemKey = dashboardItemsLoadMethodAttribute.Key;

            SessionHelper.SetSessionObject("DashboardCurrentReportSelectorKey", dashboardItemKey);
            SessionHelper.SetSessionObject("DashboardCurrentReportSelectorObject_" + dashboardItemKey, reportSelector);
            SessionHelper.SetSessionObject("DashboardCurrentReportSelectorType_" + dashboardItemKey, typeof(T));
        }

        protected void DashboardSessionSaveAllItems(List<IDashboardItem> dashboardItems)
        {
            SessionHelper.SetSessionObject("DashboardCurrentReportSelectorAllItems", dashboardItems);
        }

        protected List<IDashboardItem> DashboardSessionGetAllItems()
        {
            return SessionHelper.GetSessionValue("DashboardCurrentReportSelectorAllItems", (List<IDashboardItem>)null);
        }

        private void DashboardSessionSaveCurrentReportSelectorTimeStamp()
        {
            SessionHelper.SetSessionObject("DashboardCurrentReportSelectorTimeStamp", DateTime.Now);
        }

        private static DateTime DashboardSessionGetCurrentReportSelectorTimeStamp()
        {
            return SessionHelper.GetSessionValue("DashboardCurrentReportSelectorTimeStamp", DateTime.MinValue);
        }

        private void DashboardTryInitCurrentReportSelector()
        {
            var dashboardItemKey = SessionHelper.GetSessionValue("DashboardCurrentReportSelectorKey", "");
            if (dashboardItemKey.IsNullOrEmpty())
                return;

            var reportSelectorObject = SessionHelper.GetSessionValue("DashboardCurrentReportSelectorObject_" + dashboardItemKey, (object)null);
            if (reportSelectorObject == null)
                return;

            var reportSelectorType = SessionHelper.GetSessionValue("DashboardCurrentReportSelectorType_" + dashboardItemKey, (Type)null);
            if (reportSelectorType == null)
                return;

            const int totalSecondsReportSelectorExpiration = 30;
            var secondsElapsed = Math.Abs((DateTime.Now - DashboardSessionGetCurrentReportSelectorTimeStamp()).TotalSeconds);
            if (secondsElapsed > totalSecondsReportSelectorExpiration)
                return;

            var viewModelType = this.GetType();
            var reportSelectorProperty = viewModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList().FirstOrDefault(p => p.PropertyType == reportSelectorType);
            if (reportSelectorProperty == null)
                return;

            reportSelectorProperty.SetValue(this, reportSelectorObject, null);
        }

        public string DashboardPrepareReportForItem(int id)
        {
            DashboardSessionSaveCurrentReportSelectorTimeStamp();

            var allDashboardItems = DashboardSessionGetAllItems();
            if (allDashboardItems == null || allDashboardItems.None())
                return "";

            var dashboardItem = allDashboardItems.FirstOrDefault(item => item.ID == id);
            if (dashboardItem == null)
                return "";

            var redirectUrl = dashboardItem.RelatedAppUrl.NotNullOrEmpty().Replace("mvc/", "~/");
            SessionHelper.SetSessionObject("DashboardCurrentReportSelectorKey", dashboardItem.Title);

            return redirectUrl;
        }

        #endregion

        protected string GetApplicationConfigValueForCustomer(string configValue)
        {
            if (LogonContext == null || LogonContext.Customer == null)
                return "";

            var userCustomerId = LogonContext.Customer.CustomerID;
            var userGroupId = 0;
            var appId = LogonContext.GetAppIdCurrent();

            return ApplicationConfiguration.GetApplicationConfigValue(configValue, appId.ToString(), userCustomerId, userGroupId);
        }
    }
}
