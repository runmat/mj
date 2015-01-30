using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Services
{
    public class ChartService
    {
        public static ChartItemsPackage GetGroupedAndStackedItemsWithLabels<T>
            (
                IList<T> items,
                Func<T, string> xAxisKey,
                Action<IList<string>> addAdditionalXaxisKeys = null,
                IEnumerable<string> stackedGroupValues = null,
                Func<T, string> stackedKey = null,
                Func<IGrouping<int, T>, int> aggregate = null
            ) where T : class
        {
            var xAxisList = items.GroupBy(xAxisKey).OrderBy(k => k.Key).Select(k => k.Key).ToListOrEmptyList();
            if (xAxisList.Any() && addAdditionalXaxisKeys != null)
                addAdditionalXaxisKeys(xAxisList);
            var xAxisLabels = xAxisList.ToArray();

            if (stackedGroupValues == null)
                stackedGroupValues = new []{""};

            if (stackedKey == null)
                stackedKey = e => "";

            var groupValuesArray = stackedGroupValues.ToArray();
            var data = new object[stackedGroupValues.Count()];
            for (var k = 0; k < stackedGroupValues.Count(); k++)
            {
                var subArray = items
                    .Where(item => stackedKey(item) == groupValuesArray[k])
                    .GroupBy(group => xAxisList.IndexOf(xAxisKey(group)))
                    .Select(g => new[] { g.Key, (aggregate == null ? g.Count() : aggregate(g)) })
                    .ToArray();

                data[k] = new
                {
                    data = subArray,
                    label = groupValuesArray[k]
                };
            }

            return new ChartItemsPackage
            {
                data = data,
                labels = xAxisLabels
            };
        }
    }
}
