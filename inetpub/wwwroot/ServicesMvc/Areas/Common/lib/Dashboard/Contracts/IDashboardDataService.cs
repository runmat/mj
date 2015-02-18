using System.Collections.Generic;

namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardDataService
    {
        IList<IDashboardItem> GetDashboardItems(string userName);

        void ApplyVisibilityAndSortAnnotatorItems(IList<IDashboardItem> items, IList<int> itemIds);
    }
}
