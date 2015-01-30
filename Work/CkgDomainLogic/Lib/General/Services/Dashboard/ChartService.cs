using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Services
{
    public class ChartService
    {
<<<<<<< HEAD
        public static ChartItemsPackage GetPieChartGroupedItemsWithLabels<T>
            (
                IList<T> items,
                Func<T, string> xAxisKey,
                Func<IGrouping<int, T>, int> aggregate = null
            ) where T : class
        {
            var xAxisList = items.OrderBy(xAxisKey).GroupBy(xAxisKey).Select(k => k.Key).ToListOrEmptyList();
            var xAxisLabels = xAxisList.ToArray();

            var groupArray = items
                .OrderBy(xAxisKey)
                .GroupBy(group => xAxisList.IndexOf(xAxisKey(group)))
                .Select(g => new[] { g.Key, (aggregate == null ? g.Count() : aggregate(g)) })
                .ToArray();

            var data = new object[groupArray.Count()];
            for (var k = 0; k < groupArray.Count(); k++)
            {
                data[k] = new
                    {
                        data = new [] { groupArray[k] },
                        label = xAxisLabels[k]
                    };
            }

            return new ChartItemsPackage
            {
                data = data
            };
        }

        public static ChartItemsPackage GetBarChartGroupedStackedItemsWithLabels<T>
            (
            IList<T> items,
            Func<T, string> xAxisKey,
            Action<IList<string>> addAdditionalXaxisKeys = null,
            //IEnumerable<string> stackedGroupValues = null,
            Func<T, string> stackedKey = null,
            Func<IGrouping<int, T>, int> aggregate = null
            ) where T : class
        {
            var xAxisList = items.OrderBy(xAxisKey).GroupBy(xAxisKey).Select(k => k.Key).ToListOrEmptyList();
=======
        public static ChartItemsPackage GetGroupedAndStackedItemsWithLabels<T>
            (
                IList<T> items,
                IEnumerable<string> stackedGroupValues,
                Func<T, string> stackedKey,
                Func<T, string> xAxisKey,
                Action<IList<string>> addAdditionalXaxisKeys = null,
                Func<IGrouping<int, T>, int> aggregate = null
            ) where T : class
        {
            var xAxisList = items.GroupBy(xAxisKey).OrderBy(k => k.Key).Select(k => k.Key).ToListOrEmptyList();
>>>>>>> 5ffbd0e... simplification of chart data grouping and aggregating
            if (xAxisList.Any() && addAdditionalXaxisKeys != null)
                addAdditionalXaxisKeys(xAxisList);
            var xAxisLabels = xAxisList.ToArray();

<<<<<<< HEAD
            //if (stackedGroupValues == null)
            //    stackedGroupValues = new[] { "" };

            if (stackedKey == null)
                stackedKey = e => "";

            var stackedGroupValues = items.GroupBy(stackedKey).OrderBy(k => k.Key).Select(k => k.Key);
            var stackedGroupValuesArray = stackedGroupValues.ToArray();

=======
            var groupValuesArray = stackedGroupValues.ToArray();
>>>>>>> 5ffbd0e... simplification of chart data grouping and aggregating
            var data = new object[stackedGroupValues.Count()];
            for (var k = 0; k < stackedGroupValues.Count(); k++)
            {
                var subArray = items
<<<<<<< HEAD
                    .Where(item => stackedKey(item) == stackedGroupValuesArray[k])
=======
                    .Where(item => stackedKey(item) == groupValuesArray[k])
>>>>>>> 5ffbd0e... simplification of chart data grouping and aggregating
                    .GroupBy(group => xAxisList.IndexOf(xAxisKey(group)))
                    .Select(g => new[] { g.Key, (aggregate == null ? g.Count() : aggregate(g)) })
                    .ToArray();

                data[k] = new
                {
                    data = subArray,
<<<<<<< HEAD
                    label = stackedGroupValuesArray[k]
=======
                    label = groupValuesArray[k]
>>>>>>> 5ffbd0e... simplification of chart data grouping and aggregating
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
