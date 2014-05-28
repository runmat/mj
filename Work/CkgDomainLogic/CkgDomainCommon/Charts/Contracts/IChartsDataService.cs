using System;
using System.Collections.Generic;
using CkgDomainLogic.Charts.Models;

namespace CkgDomainLogic.Charts.Contracts
{
    public interface IChartsDataService
    {
        IEnumerable<KbaPlzKgs> GetGroupKgsItems(KgsSelector selector);

        IEnumerable<KbaPlzKgs> GetDetailsKgsItems(KgsSelector selector);


        IEnumerable<ChartEntity> GetGroupChartItems(IChartsSqlDataDescriptor dataDescriptor, ChartDataSelector selector);

        void GetDetailsChartItems(IChartsSqlDataDescriptor dataDescriptor, ChartDataSelector selector, out Type dynamicType, out IEnumerable<dynamic> dynamicObjects, params string[] detailsFilterValues);

        IEnumerable<IEnumerable<ChartEntity>> GetAdditionalChartItemLists(IChartsSqlDataDescriptor dataDescriptor, ChartDataSelector selector);
    }
}
