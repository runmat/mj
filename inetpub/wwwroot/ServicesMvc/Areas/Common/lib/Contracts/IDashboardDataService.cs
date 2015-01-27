using System.Collections.Generic;

namespace CkgDomainLogic.DomainCommon.Contracts
{
    public interface IDashboardDataService
    {
        IEnumerable<string> DashboardItems { get;  }
    }
}
