using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Logs.Contracts;
using CkgDomainLogic.Logs.Models;
using GeneralTools.Log.Models.MultiPlatform;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;

namespace CkgDomainLogic.Logs.ViewModels
{
    public class LogsViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public ILogsDataService DataService { get { return CacheGet<ILogsDataService>(); } }

        public SapLogItemSelector SapLogItemSelector
        {
            get { return PropertyCacheGet(() => new SapLogItemSelector
                {
                    LogsConnection = DataService.LogsDefaultConnectionString,
                        // Test:
                        AppIds = new List<int> { 1366 },
                        UserIds = new List<string> { "19418" },
                });
            }
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

        public PageVisitLogItemSelector PageVisitLogItemSelector
        {
            get
            {
                return PropertyCacheGet(() => new PageVisitLogItemSelector
                {
                    LogsConnection = DataService.LogsDefaultConnectionString,
                });
            }
            set { PropertyCacheSet(value); }
        }

        public List<PageVisitLogItem> PageVisitLogItems
        {
            get { return PropertyCacheGet(() => new List<PageVisitLogItem>()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<PageVisitLogItem> PageVisitLogItemsFiltered
        {
            get { return PropertyCacheGet(() => PageVisitLogItems); }
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

            PageVisitLogItemSelector.AllApplications = Applications.ToSelectList();
            PageVisitLogItemSelector.AllCustomers = Customers.ToSelectList();
            PageVisitLogItemSelector.AllUsers = Users.ToSelectList();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.SapLogItemsFiltered);
            PropertyCacheClear(this, m => m.PageVisitLogItemsFiltered);
            PropertyCacheClear(this, m => m.WebServiceTrafficLogItemsUIFiltered);

            PropertyCacheClear(this, m => m.Applications);
            PropertyCacheClear(this, m => m.Customers);
            PropertyCacheClear(this, m => m.Users);
        }

        public void FilterSapLogItems(string filterValue, string filterProperties)
        {
            SapLogItemsFiltered = SapLogItems.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterPageVisitLogItems(string filterValue, string filterProperties)
        {
            PageVisitLogItemsFiltered = PageVisitLogItems.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterWebServiceTrafficLogItems(string filterValue, string filterProperties)
        {
            WebServiceTrafficLogItemsUIFiltered = WebServiceTrafficLogItemsUI.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public SapCallContext LastSapCallContext { get; private set; }

        public void GetSapCallContext(int id)
        {
            var sapLogItemDetailed = DataService.GetSapLogItemDetailed(id);

            //var strExpTables = (sapLogItemDetailed.ExportTables == null ? null : sapLogItemDetailed.ExportTables.Replace("<ExportTables>", "").Replace("</ExportTables>", "").Trim('\r', '\n'));

            var strExpTables = sapLogItemDetailed.ExportTables;

            var expTables = new List<ExportTable>();
            if (strExpTables != null)
            {
                var teile = strExpTables.Replace('\r', '\n').Split('\n');
                for (int i = 0; i < teile.Length; i++)
                {
                    if (teile[i].Contains("TableName"))
                    {
                        var strName = teile[i].Substring(teile[i].IndexOf("TableName=\"") + 11);
                        strName = strName.Substring(0, strName.IndexOf("\""));
                        var strCount = teile[i].Substring(teile[i].IndexOf("RowCount=\"") + 10);
                        strCount = strCount.Substring(0, strCount.IndexOf("\""));
                        expTables.Add(new ExportTable { TableName = strName, RowCount = strCount });
                    }
                }
            }

            LastSapCallContext = new SapCallContext
                {
                    ImportParameters = (sapLogItemDetailed.ImportParameters == null ? null : XmlService.XmlDeserializeFromString<DataTable>(sapLogItemDetailed.ImportParameters)),
                    ImportTables = (sapLogItemDetailed.ImportTables == null ? null : XmlService.XmlDeserializeFromString<DataTable[]>(sapLogItemDetailed.ImportTables)),
                    ExportParameters = (sapLogItemDetailed.ExportParameters == null ? null : XmlService.XmlDeserializeFromString<DataTable>(sapLogItemDetailed.ExportParameters)),
                    ExportTables = (expTables.Count == 0 ? null : expTables.ToArray())
                };
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

        public bool LoadPageVisitLogItems(PageVisitLogItemSelector newPageVisitLogItemSelector)
        {
            if (PageVisitLogItemSelector.LogsConnection != newPageVisitLogItemSelector.LogsConnection)
            {
                // Logs connection changed ==> reset filters that depend on server specifiy keys (app ids, user ids, customer ids, etc)

                DataService.LogsConnectionString = newPageVisitLogItemSelector.LogsConnection;
                PageVisitLogItemSelector = newPageVisitLogItemSelector;

                PropertyCacheClear(this, m => m.Applications);
                PropertyCacheClear(this, m => m.Customers);
                PropertyCacheClear(this, m => m.Users);

                DataInit();
            }

            PageVisitLogItemSelector = newPageVisitLogItemSelector;

            if (PageVisitLogItemSelector.SubmitWithNoDataQuerying)
                return false;

            PageVisitLogItems = DataService.GetPageVisitLogItems(PageVisitLogItemSelector);
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
