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

            PropertyCacheClear(this, m => m.Applications);
            PropertyCacheClear(this, m => m.Customers);
            PropertyCacheClear(this, m => m.Users);
        }

        public void FilterSapLogItems(string filterValue, string filterProperties)
        {
            SapLogItemsFiltered = SapLogItems.SearchPropertiesWithOrCondition(filterValue, filterProperties);
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
    }
}
