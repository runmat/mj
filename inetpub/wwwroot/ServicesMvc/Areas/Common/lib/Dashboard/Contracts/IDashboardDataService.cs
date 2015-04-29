using System.Collections.Generic;

namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardDataService
    {
        IList<IDashboardItem> GetDashboardItems(string userName);

        void SaveDashboardItems(IList<IDashboardItem> items, string userName, string commaSeparatedIds);
    }
}
