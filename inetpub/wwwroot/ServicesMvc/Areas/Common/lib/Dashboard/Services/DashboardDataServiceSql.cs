// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
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

            var items = ct.GetDashboardItems().Cast<IDashboardItem>().ToList();
            PrepareAnnotatorItems(ct, items, userName);

            items.ForEach(item => item.Title = Localize.TranslateResourceKey(item.ItemKey));

            return items.OrderBy(item => item.UserSort).ToList();
        }

        public void SaveDashboardItems(IList<IDashboardItem> items, string userName, string commaSeparatedIds)
        {
            var ct = CreateDbContext();

            IList<int> ids;
            if (commaSeparatedIds.IsNotNullOrEmpty())
                ids = commaSeparatedIds.Split(',').Select(s => s.ToInt()).ToList();
            else
                ids = items.Where(i => i.IsUserVisible).Select(i => i.ID).ToList();

            HideAllAnnotatorItems(items);
            ApplyVisibilityAndSortAnnotatorItems(items, ids, true);

            var annotatorItems = items.OrderBy(item => item.UserSort).Select(item => item.ItemAnnotator);
            ct.DashboardAnnotatorItemsUserSave(userName, annotatorItems);
        }

        static void PrepareAnnotatorItems(DashboardSqlDbContext ct, IList<IDashboardItem> items, string userName)
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
            if (annotatorItems == null || annotatorItems.None())
                return;

            var visibleItemIds = annotatorItems.Where(i => i.IsUserVisible).OrderBy(i => i.UserSort).Select(i => i.ItemID);
            var hiddenItemIds = annotatorItems.Where(i => !i.IsUserVisible).Select(i => i.ItemID);

            if (hiddenItemIds.Any())
                ApplyVisibilityAndSortAnnotatorItems(items, hiddenItemIds.ToList(), false);

            if (visibleItemIds.Any())
                ApplyVisibilityAndSortAnnotatorItems(items, visibleItemIds.ToList(), true);
        }

        static void ApplyVisibilityAndSortAnnotatorItems(IList<IDashboardItem> items, IList<int> itemIds, bool setVisible)
        {
            if (itemIds == null)
                return;

            var sort = 0;
            foreach (var itemId in itemIds)
            {
                var item = items.FirstOrDefault(i => i.ID == itemId);
                if (item == null)
                    continue;

                item.ItemAnnotator.IsUserVisible = setVisible;
                if (!item.ItemAnnotator.IsUserVisible)
                {
                    item.ItemAnnotator.UserSort = 0;
                    continue;
                }

                sort += 10;
                item.ItemAnnotator.UserSort = sort;
            }
        }

        static void HideAllAnnotatorItems(IList<IDashboardItem> items)
        {
            foreach (var item in items)
            {
                item.ItemAnnotator.IsUserVisible = false;
                item.ItemAnnotator.UserSort = 0;
            }
        }

        static DashboardSqlDbContext CreateDbContext()
        {
            return new DashboardSqlDbContext();
        }
    }
}
