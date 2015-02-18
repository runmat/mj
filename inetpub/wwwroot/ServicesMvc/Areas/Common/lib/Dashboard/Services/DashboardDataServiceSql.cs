// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Contracts;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class DashboardDataServiceSql : IDashboardDataService 
    {
        public IList<IDashboardItem> GetDashboardItems(string userName)
        {
            var ct = CreateDbContext();

            var list = ct.GetDashboardItems().Cast<IDashboardItem>().ToList();
            PrepareAnnotatorItems(ct, list, userName);

            return list;
        }

        void PrepareAnnotatorItems(DashboardSqlDbContext ct, IList<IDashboardItem> items, string userName)
        {
            foreach (var item in items)
            {
                item.ItemAnnotator = new DashboardItemAnnotator
                    {
                        ItemID = item.ID,
                        IsUserVisible = true,
                        UserSort = item.InitialSort.GetValueOrDefault(),
                    };
            }

            var annotatorItems = ct.DashboardAnnotatorItemsUserGet(userName);

            var itemIds = items.OrderBy(i => i.InitialSort).Select(i => i.ID);
            if (annotatorItems != null)
                itemIds = annotatorItems.Where(i => i.IsUserVisible).OrderBy(i => i.UserSort).Select(i => i.ItemID);

            ApplyVisibilityAndSortAnnotatorItems(items, itemIds.ToList());
        }

        public void ApplyVisibilityAndSortAnnotatorItems(IList<IDashboardItem> items, IList<int> itemIds)
        {
            foreach (var item in items)
            {
                item.ItemAnnotator.IsUserVisible = false;
                item.ItemAnnotator.UserSort = 0;
            }

            if (itemIds == null)
                return;

            var sort = 0;
            foreach (var itemId in itemIds)
            {
                var item = items.FirstOrDefault(i => i.ID == itemId);
                if (item == null)
                    continue;

                item.ItemAnnotator.IsUserVisible = true;
                if (!item.ItemAnnotator.IsUserVisible)
                    continue;

                sort += 10;
                item.ItemAnnotator.UserSort = sort;
            }
        }

        private static DashboardSqlDbContext CreateDbContext()
        {
            return new DashboardSqlDbContext();
        }
    }
}
