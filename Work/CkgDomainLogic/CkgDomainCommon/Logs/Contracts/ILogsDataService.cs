using System.Collections.Generic;
using CkgDomainLogic.Logs.Models;
using GeneralTools.Log.Models.MultiPlatform;

namespace CkgDomainLogic.Logs.Contracts
{
    public interface ILogsDataService
    {
        string LogsDefaultConnectionString { get; }

        string LogsConnectionString { get; set; }

        List<SapLogItem> GetSapLogItems(SapLogItemSelector sapLogItemSelector);

        List<WebServiceTrafficLogItem> GetWebServiceTrafficLogItems(WebServiceTrafficLogItemSelector webServiceTrafficLogItemSelector);

        List<WebServiceTrafficLogTable> GetWebServiceTrafficLogTables();

        List<MpApplicationTranslated> Applications { get; }

        List<MpCustomer> Customers { get; }

        List<MpWebUser> Users { get; }
    }
}
