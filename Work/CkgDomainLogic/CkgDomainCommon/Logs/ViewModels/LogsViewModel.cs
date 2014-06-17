using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Logs.Contracts;
using CkgDomainLogic.Logs.Models;
using GeneralTools.Log.Models.MultiPlatform;
using GeneralTools.Models;
using MvcTools.Web;

namespace CkgDomainLogic.Logs.ViewModels
{
    public class LogsViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public ILogsDataService DataService { get { return CacheGet<ILogsDataService>(); } }

        public SapLogItemSelector SapLogItemSelector
        {
            get { return PropertyCacheGet(() => new SapLogItemSelector { LogsConnection = DataService.LogsDefaultConnectionString }); }
            set { PropertyCacheSet(value); }
        }

        public List<SapLogItem> SapLogItems
        {
            get { return PropertyCacheGet(() => new List<SapLogItem>()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SapLogItem> SapLogItemsFiltered
        {
            get { return PropertyCacheGet(() => SapLogItems); }
            private set { PropertyCacheSet(value); }
        }

        public WebServiceTrafficLogItemSelector WebServiceTrafficLogItemSelector
        {
            get { return PropertyCacheGet(() => new WebServiceTrafficLogItemSelector { LogsConnection = DataService.LogsDefaultConnectionString }); }
            set { PropertyCacheSet(value); }
        }

        public List<WebServiceTrafficLogItem> WebServiceTrafficLogItems
        {
            get { return PropertyCacheGet(() => new List<WebServiceTrafficLogItem>()); }
            set { PropertyCacheSet(value); }
        }

        public List<WebServiceTrafficLogItemUI> WebServiceTrafficLogItemsUI
        {
            get { return PropertyCacheGet(() => new List<WebServiceTrafficLogItemUI>()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WebServiceTrafficLogItemUI> WebServiceTrafficLogItemsUIFiltered
        {
            get { return PropertyCacheGet(() => WebServiceTrafficLogItemsUI); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<WebServiceTrafficLogTable> AllWebServiceTrafficLogTables { get { return DataService.GetWebServiceTrafficLogTables(); } }

        public List<MpApplicationTranslated> Applications { get { return PropertyCacheGet(() => DataService.Applications); } }

        public List<MpCustomer> Customers { get { return PropertyCacheGet(() => DataService.Customers); } }
        
        public List<MpWebUser> Users { get { return PropertyCacheGet(() => DataService.Users); } }


        public void DataInit()
        {
            DataMarkForRefresh();

            SapLogItemSelector.AllApplications = Applications.ToSelectList();
            SapLogItemSelector.AllCustomers = Customers.ToSelectList();
            SapLogItemSelector.AllUsers = Users.ToSelectList();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.SapLogItemsFiltered);
            PropertyCacheClear(this, m => m.WebServiceTrafficLogItemsUIFiltered);

            PropertyCacheClear(this, m => m.Applications);
            PropertyCacheClear(this, m => m.Customers);
            PropertyCacheClear(this, m => m.Users);
        }

        public void FilterSapLogItems(string filterValue, string filterProperties)
        {
            SapLogItemsFiltered = SapLogItems.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterWebServiceTrafficLogItems(string filterValue, string filterProperties)
        {
            WebServiceTrafficLogItemsUIFiltered = WebServiceTrafficLogItemsUI.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        public bool LoadSapLogItems(SapLogItemSelector newSapLogItemSelector)
        {
            if (SapLogItemSelector.LogsConnection != newSapLogItemSelector.LogsConnection)
            {
                // Logs connection changed ==> reset filters that depend on server specifiy keys (app ids, user ids, customer ids, etc)

                DataService.LogsConnectionString = newSapLogItemSelector.LogsConnection;
                SapLogItemSelector = newSapLogItemSelector;

                PropertyCacheClear(this, m => m.Applications);
                PropertyCacheClear(this, m => m.Customers);
                PropertyCacheClear(this, m => m.Users);

                DataInit();
            }

            SapLogItemSelector = newSapLogItemSelector;

            if (SapLogItemSelector.SubmitWithNoDataQuerying)
                return false;

            SapLogItems = DataService.GetSapLogItems(SapLogItemSelector);
            DataMarkForRefresh();
            return true;
        }

        public bool LoadWebServiceTrafficLogItems(WebServiceTrafficLogItemSelector newWebServiceTrafficLogItemSelector)
        {
            if (WebServiceTrafficLogItemSelector.LogsConnection != newWebServiceTrafficLogItemSelector.LogsConnection)
            {
                // Logs connection changed ==> reset filters that depend on server specifiy keys (app ids, user ids, customer ids, etc)

                DataService.LogsConnectionString = newWebServiceTrafficLogItemSelector.LogsConnection;
                WebServiceTrafficLogItemSelector = newWebServiceTrafficLogItemSelector;

                PropertyCacheClear(this, m => m.Applications);
                PropertyCacheClear(this, m => m.Customers);
                PropertyCacheClear(this, m => m.Users);

                DataInit();
            }

            WebServiceTrafficLogItemSelector = newWebServiceTrafficLogItemSelector;

            WebServiceTrafficLogItems = DataService.GetWebServiceTrafficLogItems(WebServiceTrafficLogItemSelector);

            WebServiceTrafficLogItemsUI.Clear();
            foreach (var item in WebServiceTrafficLogItems)
            {
                WebServiceTrafficLogItemsUI.Add(new WebServiceTrafficLogItemUI{ Id = item.Id, Time_Stamp = item.Time_Stamp, Type = item.Type, AllXmlPreview = item.AllXmlPreview });
            }

            DataMarkForRefresh();
            return true;
        }

        public WebServiceTrafficLogItem GetDetails(int id)
        {
            return WebServiceTrafficLogItems.Find(i => i.Id == id);
        }
    }
}
