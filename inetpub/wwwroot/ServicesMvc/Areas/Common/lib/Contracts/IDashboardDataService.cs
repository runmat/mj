using System.Collections.Generic;

namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardDataService
    {
        IEnumerable<IDashboardItem> GetDashboardItems();

        IEnumerable<IDashboardItemUser> GetDashboardItemsUser();

        void SaveDashboardItemsUser(IEnumerable<IDashboardItemUser> userItems);
    }
}
