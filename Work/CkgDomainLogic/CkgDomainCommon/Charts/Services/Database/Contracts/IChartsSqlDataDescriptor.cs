using System.Collections.Generic;
using CkgDomainLogic.Charts.Models;

namespace CkgDomainLogic.Charts.Contracts
{
    public interface IChartsSqlDataDescriptor
    {
        string SqlTableOrViewName { get; }

        string FilterString { get; }

        string GetGroupChartItemsStatement(ChartDataSelector selector);

        string GetDetailsChartItemsStatement(ChartDataSelector selector, params string[] detailsFilterValues);

        List<string> GetAdditionalChartItemListsStatements(ChartDataSelector selector);
    }
}
