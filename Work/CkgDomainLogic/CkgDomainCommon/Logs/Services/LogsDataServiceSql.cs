using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Logs.Contracts;
using CkgDomainLogic.Logs.Models;
using GeneralTools.Log.Models.MultiPlatform;

namespace CkgDomainLogic.Logs.Services
{
    public class LogsDataServiceSql : ILogsDataService 
    {
        public string LogsDefaultConnectionString { get { return "LogsProd"; } }

        public string LogsConnectionString { get; set; }

        LogsSqlDbContext CreateLogsDbContext() { return new LogsSqlDbContext(LogsConnectionString ?? LogsDefaultConnectionString); }
       
        public List<MpApplicationTranslated> Applications { get { return CreateLogsDbContext().MpApplicationsTranslated.OrderBy(m => m.AppFriendlyName).ToList(); } }

        public List<MpCustomer> Customers { get { return CreateLogsDbContext().MpCustomers.OrderBy(m => m.Customername).ToList(); } }

        public List<MpWebUser> Users { get { return CreateLogsDbContext().MpWebUsers.OrderBy(m => m.Username).ToList(); } }


        public List<SapLogItem> GetSapLogItems(SapLogItemSelector sapLogItemSelector)
        {
            LogsConnectionString = sapLogItemSelector.LogsConnection;

            var logsDbContext = CreateLogsDbContext();

            var sapLogItems = logsDbContext.GetSapLogItems(sapLogItemSelector);

            return sapLogItems.ToList();
        }

        public List<PageVisitLogItem> GetPageVisitLogItems(PageVisitLogItemSelector pageVisitLogItemSelector)
        {
            LogsConnectionString = pageVisitLogItemSelector.LogsConnection;

            var logsDbContext = CreateLogsDbContext();

            var customerApps = logsDbContext.GetCustomerApplicationsForPageVisits(pageVisitLogItemSelector).ToList();
            var pageVisits = logsDbContext.GetPageVisitPerCustomerPerDayItems(pageVisitLogItemSelector).ToList();

            var hitlist = new List<PageVisitLogItem>();

            if (pageVisitLogItemSelector.OnlyUnusedApplications)
            {
                foreach (var item in Applications)
                {
                    var appId = item.AppID;

                    if (pageVisits.All(p => p.AppID != appId))
                    {
                        if (customerApps.Any(a => a.AppID == appId))
                        {
                            foreach (var custApp in customerApps.Where(a => a.AppID == appId))
                            {
                                int intKunnr;
                                if (!Int32.TryParse(custApp.KUNNR, out intKunnr))
                                    intKunnr = 0;

                                hitlist.Add(new PageVisitLogItem
                                {
                                    CustomerID = custApp.CustomerID,
                                    CustomerName = custApp.CustomerName,
                                    KUNNR = intKunnr,
                                    AppID = appId,
                                    AppFriendlyName = item.AppFriendlyName,
                                    Hits = 0
                                });
                            }
                        }
                        else
                        {
                            hitlist.Add(new PageVisitLogItem
                            {
                                CustomerID = null,
                                CustomerName = null,
                                KUNNR = null,
                                AppID = appId,
                                AppFriendlyName = item.AppFriendlyName,
                                Hits = 0
                            });
                        }
                    }
                }
            }
            else
            {
                foreach (var item in customerApps)
                {
                    int intKunnr;
                    if (!Int32.TryParse(item.KUNNR, out intKunnr))
                        intKunnr = 0;

                    if (pageVisitLogItemSelector.OnlyUnusedCustomerApplications)
                    {
                        if (!pageVisits.Any(p => p.CustomerID == item.CustomerID && p.AppID == item.AppID))
                        {
                            hitlist.Add(new PageVisitLogItem
                            {
                                CustomerID = item.CustomerID,
                                CustomerName = item.CustomerName,
                                KUNNR = intKunnr,
                                AppID = item.AppID,
                                AppFriendlyName = item.AppFriendlyName,
                                Hits = 0
                            });
                        }
                    }
                    else
                    {
                        var hitCount = (from p in pageVisits
                                        where p.CustomerID == item.CustomerID && p.AppID == item.AppID
                                        select p.Hits).Sum();

                        hitlist.Add(new PageVisitLogItem
                        {
                            CustomerID = item.CustomerID,
                            CustomerName = item.CustomerName,
                            KUNNR = intKunnr,
                            AppID = item.AppID,
                            AppFriendlyName = item.AppFriendlyName,
                            Hits = hitCount
                        });
                    }
                }
            }

            return hitlist;
        }

        public List<PageVisitLogItemDetail> GetPageVisitLogItemsDetail(PageVisitLogItemDetailSelector pageVisitLogItemDetailSelector)
        {
            LogsConnectionString = pageVisitLogItemDetailSelector.LogsConnection;

            var logsDbContext = CreateLogsDbContext();

            var pageVisitLogItems = logsDbContext.GetPageVisitLogItems(pageVisitLogItemDetailSelector);

            return pageVisitLogItems.ToList();
        }

        public SapLogItemDetailed GetSapLogItemDetailed(int id)
        {
            var logsDbContext = CreateLogsDbContext();

            return logsDbContext.GetSapLogItemDetailed(id);
        }

        public List<WebServiceTrafficLogItem> GetWebServiceTrafficLogItems(WebServiceTrafficLogItemSelector webServiceTrafficLogItemSelector)
        {
            LogsConnectionString = webServiceTrafficLogItemSelector.LogsConnection;

            var logsDbContext = CreateLogsDbContext();

            var webserviceLogItems = logsDbContext.GetWebServiceTrafficLogItems(webServiceTrafficLogItemSelector);

            return webserviceLogItems.ToList();
        }

        public List<WebServiceTrafficLogTable> GetWebServiceTrafficLogTables()
        {
            var logsDbContext = CreateLogsDbContext();

            var logTables = logsDbContext.GetWebServiceTrafficLogTables();

            return logTables.ToList();
        }

        public List<ErrorLogItem> GetErrorLogItems(ErrorLogItemSelector errorLogItemSelector)
        {
            LogsConnectionString = errorLogItemSelector.LogsConnection;

            var logsDbContext = CreateLogsDbContext();

            var errorLogItems = logsDbContext.GetErrorLogItems(errorLogItemSelector);

            return errorLogItems.ToList();
        }
    }
}
