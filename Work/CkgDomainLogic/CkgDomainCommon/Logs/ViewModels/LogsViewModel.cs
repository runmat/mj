using System;
using System.Collections.Generic;
using System.Data;
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

        public PageVisitLogItemDetailSelector PageVisitLogItemDetailSelector
        {
            get
            {
                return PropertyCacheGet(() => new PageVisitLogItemDetailSelector
                {
                    LogsConnection = DataService.LogsDefaultConnectionString,
                });
            }
            set { PropertyCacheSet(value); }
        }

        public List<PageVisitLogItemDetail> PageVisitLogItemsDetail
        {
            get { return PropertyCacheGet(() => new List<PageVisitLogItemDetail>()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<PageVisitLogItemDetail> PageVisitLogItemsDetailFiltered
        {
            get { return PropertyCacheGet(() => PageVisitLogItemsDetail); }
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

        public ErrorLogItemSelector ErrorLogItemSelector
        {
            get
            {
                return PropertyCacheGet(() => new ErrorLogItemSelector
                {
                    LogsConnection = DataService.LogsDefaultConnectionString,
                });
            }
            set { PropertyCacheSet(value); }
        }

        public List<ErrorLogItem> ErrorLogItems
        {
            get { return PropertyCacheGet(() => new List<ErrorLogItem>()); }
            set { PropertyCacheSet(value); }
        }

        public List<ErrorLogItemUI> ErrorLogItemsUI
        {
            get { return PropertyCacheGet(() => new List<ErrorLogItemUI>()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<ErrorLogItemUI> ErrorLogItemsUIFiltered
        {
            get { return PropertyCacheGet(() => ErrorLogItemsUI); }
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

            PageVisitLogItemSelector.AllApplications = Applications.ToSelectList();
            PageVisitLogItemSelector.AllCustomers = Customers.ToSelectList();

            PageVisitLogItemDetailSelector.AllApplications = Applications.ToSelectList();
            PageVisitLogItemDetailSelector.AllCustomers = Customers.ToSelectList();
            PageVisitLogItemDetailSelector.AllUsers = Users.ToSelectList();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.SapLogItemsFiltered);
            PropertyCacheClear(this, m => m.PageVisitLogItemsFiltered);
            PropertyCacheClear(this, m => m.PageVisitLogItemsDetailFiltered);
            PropertyCacheClear(this, m => m.WebServiceTrafficLogItemsUIFiltered);
            PropertyCacheClear(this, m => m.ErrorLogItemsUIFiltered);

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

        public void FilterPageVisitLogItemsDetail(string filterValue, string filterProperties)
        {
            PageVisitLogItemsDetailFiltered = PageVisitLogItemsDetail.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterWebServiceTrafficLogItems(string filterValue, string filterProperties)
        {
            WebServiceTrafficLogItemsUIFiltered = WebServiceTrafficLogItemsUI.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterErrorLogItems(string filterValue, string filterProperties)
        {
            ErrorLogItemsUIFiltered = ErrorLogItemsUI.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public SapCallContext LastSapCallContext { get; private set; }

        public void GetSapCallContext(int id)
        {
            var sapLogItemDetailed = DataService.GetSapLogItemDetailed(id);

            DataTable impParams = null;
            if (sapLogItemDetailed.ImportParameters.IsNotNullOrEmpty())
            {
                try
                {
                    impParams = XmlService.XmlTryDeserializeCompressedString<DataTable>(sapLogItemDetailed.ImportParameters);
                }
                catch (Exception)
                {
                    impParams = XmlService.XmlDeserializeFromString<DataTable>(sapLogItemDetailed.ImportParameters);
                }                
            }

            var impTables = new List<DataTable>();
            if (sapLogItemDetailed.ImportTables.IsNotNullOrEmpty())
            {
                try
                {
                    impTables = XmlService.XmlTryDeserializeCompressedString<List<DataTable>>(sapLogItemDetailed.ImportTables);
                }
                catch (Exception)
                {
                    impTables = XmlService.XmlDeserializeFromString<List<DataTable>>(sapLogItemDetailed.ImportTables);
                }
            }

            DataTable expParams = null;
            if (sapLogItemDetailed.ExportParameters.IsNotNullOrEmpty())
            {
                try
                {
                    expParams = XmlService.XmlTryDeserializeCompressedString<DataTable>(sapLogItemDetailed.ExportParameters);
                }
                catch (Exception)
                {
                    expParams = XmlService.XmlDeserializeFromString<DataTable>(sapLogItemDetailed.ExportParameters);
                }
            }

            var strExpTables = "";
            if (sapLogItemDetailed.ExportTables.IsNotNullOrEmpty())
            {
                // Habe ich einen Base64 encoded String?, sonst nehme ich den Wert aus der DB
                try
                {
                    strExpTables = XmlService.DecompressString(sapLogItemDetailed.ExportTables);
                }
                catch (Exception)
                {
                    strExpTables = sapLogItemDetailed.ExportTables;
                }
            }        

            var expTables = new List<ExportTable>();
            if (strExpTables.IsNotNullOrEmpty())
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
                    ImportParameters = impParams,
                    ImportTables = impTables.ToArray(),
                    ExportParameters = expParams,
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
                // Logs connection changed ==> reset filters that depend on server specifiy keys (app ids, customer ids, etc)

                DataService.LogsConnectionString = newPageVisitLogItemSelector.LogsConnection;
                PageVisitLogItemSelector = newPageVisitLogItemSelector;

                PropertyCacheClear(this, m => m.Applications);
                PropertyCacheClear(this, m => m.Customers);

                DataInit();
            }

            PageVisitLogItemSelector = newPageVisitLogItemSelector;

            if (PageVisitLogItemSelector.SubmitWithNoDataQuerying)
                return false;

            PageVisitLogItems = DataService.GetPageVisitLogItems(PageVisitLogItemSelector);
            DataMarkForRefresh();
            return true;
        }

        public bool LoadPageVisitLogItemsDetail(PageVisitLogItemDetailSelector newPageVisitLogItemDetailSelector)
        {
            if (PageVisitLogItemDetailSelector.LogsConnection != newPageVisitLogItemDetailSelector.LogsConnection)
            {
                // Logs connection changed ==> reset filters that depend on server specifiy keys (app ids, user ids, customer ids, etc)

                DataService.LogsConnectionString = newPageVisitLogItemDetailSelector.LogsConnection;
                PageVisitLogItemDetailSelector = newPageVisitLogItemDetailSelector;

                PropertyCacheClear(this, m => m.Applications);
                PropertyCacheClear(this, m => m.Customers);
                PropertyCacheClear(this, m => m.Users);

                DataInit();
            }

            PageVisitLogItemDetailSelector = newPageVisitLogItemDetailSelector;

            if (PageVisitLogItemDetailSelector.SubmitWithNoDataQuerying)
                return false;

            PageVisitLogItemsDetail = DataService.GetPageVisitLogItemsDetail(PageVisitLogItemDetailSelector);
            DataMarkForRefresh();
            return true;
        }

        public bool LoadUnusedApps(PageVisitLogItemSelector newPageVisitLogItemSelector)
        {
            newPageVisitLogItemSelector.OnlyUnusedApplications = true;

            return LoadPageVisitLogItems(newPageVisitLogItemSelector);
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
                WebServiceTrafficLogItemsUI.Add(new WebServiceTrafficLogItemUI
                    {
                        Id = item.Id,
                        Time_Stamp = item.Time_Stamp,
                        Type = item.Type,
                        AllXmlPreview = item.AllXmlPreview
                    });
            }

            DataMarkForRefresh();
            return true;
        }

        public WebServiceTrafficLogItem GetWebServiceTrafficLogItemDetails(int id)
        {
            return WebServiceTrafficLogItems.Find(i => i.Id == id);
        }

        public bool LoadErrorLogItems(ErrorLogItemSelector newErrorLogItemSelector)
        {
            if (ErrorLogItemSelector.LogsConnection != newErrorLogItemSelector.LogsConnection)
            {
                // Logs connection changed ==> reset filters that depend on server specifiy keys (app ids, user ids, customer ids, etc)

                DataService.LogsConnectionString = newErrorLogItemSelector.LogsConnection;
                ErrorLogItemSelector = newErrorLogItemSelector;

                DataInit();
            }

            ErrorLogItemSelector = newErrorLogItemSelector;

            ErrorLogItems = DataService.GetErrorLogItems(ErrorLogItemSelector);

            ErrorLogItemsUI.Clear();
            foreach (var item in ErrorLogItems)
            {
                ErrorLogItemsUI.Add(new ErrorLogItemUI
                    {
                        ErrorId = item.ErrorId,
                        Application = item.Application,
                        Host = item.Host,
                        Type = item.Type,
                        Source = item.Source,
                        Message = item.Message,
                        Sequence = item.Sequence,
                        User = item.User,
                        UserName = item.UserName,
                        StatusCode = item.StatusCode,
                        TimeUtc = item.TimeUtc
                    });
            }

            DataMarkForRefresh();
            return true;
        }

        public ErrorLogItem GetErrorLogItemDetails(string id)
        {
            return ErrorLogItems.Find(i => i.ErrorId == id);
        }
    }
}
