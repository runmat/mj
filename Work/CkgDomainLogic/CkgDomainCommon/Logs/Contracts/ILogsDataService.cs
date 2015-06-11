﻿using System.Collections.Generic;
using CkgDomainLogic.Logs.Models;
using GeneralTools.Log.Models.MultiPlatform;

namespace CkgDomainLogic.Logs.Contracts
{
    public interface ILogsDataService
    {
        string LogsDefaultConnectionString { get; }

        string LogsConnectionString { get; set; }


        List<SapLogItem> GetSapLogItems(SapLogItemSelector sapLogItemSelector);
        
        SapLogItemDetailed GetSapLogItemDetailed(int id);


        List<PageVisitLogItem> GetPageVisitLogItems(PageVisitLogItemSelector pageVisitLogItemDetailSelector);

        List<PageVisitLogItemDetail> GetPageVisitLogItemsDetail(PageVisitLogItemDetailSelector pageVisitLogItemSelector);


        List<WebServiceTrafficLogItem> GetWebServiceTrafficLogItems(WebServiceTrafficLogItemSelector webServiceTrafficLogItemSelector);

        List<WebServiceTrafficLogTable> GetWebServiceTrafficLogTables();

        List<ErrorLogItem> GetErrorLogItems(ErrorLogItemSelector errorLogItemSelector);


        List<MpApplicationTranslated> Applications { get; }

        List<MpCustomer> Customers { get; }

        List<MpWebUser> Users { get; }
    }
}
