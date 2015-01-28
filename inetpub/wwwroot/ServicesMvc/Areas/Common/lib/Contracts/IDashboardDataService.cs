using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.DomainCommon.Contracts
{
    public interface IDashboardDataService
    {
        IEnumerable<IDashboardItem> GetDashboardItems();
    }
}
