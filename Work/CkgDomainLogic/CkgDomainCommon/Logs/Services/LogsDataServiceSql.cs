using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.Logs.Contracts;
using CkgDomainLogic.Logs.Models;
using GeneralTools.Log.Models.MultiPlatform;
using GeneralTools.Models;

namespace CkgDomainLogic.Logs.Services
{
    public class LogsDataServiceSql : ILogsDataService 
    {
        static bool BusinessDataOnProdSystem { get { return ConfigurationManager.AppSettings["ProdSAP"].NotNullOrEmpty().ToLower() == "true"; } }

        public string LogsDefaultConnectionString { get { return BusinessDataOnProdSystem ? "LogsProd" : "LogsTest"; } }

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
    }
}
