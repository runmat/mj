using System.Collections.Generic;

namespace CkgDomainLogic.General.Contracts
{
    public interface IDashboardDataService
    {
        IList<IDashboardItem> GetDashboardItems(string userName);

        void ApplyVisibilityAndSortAnnotatorItems(string userName, IList<IDashboardItem> items, IList<int> itemIds);

        void SaveGetDashboardItems(IList<IDashboardItem> items, string userName, string commaSeparatedIds);
    }
}
