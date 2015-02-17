using System.Collections.Generic;

namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardDataService
    {
        IEnumerable<IDashboardItem> GetDashboardItems();
    }
}
