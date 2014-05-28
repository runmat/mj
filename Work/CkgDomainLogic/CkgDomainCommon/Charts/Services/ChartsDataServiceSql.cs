using System;
using System.Collections.Generic;
using CkgDomainLogic.Charts.Contracts;
using CkgDomainLogic.Charts.Models;
using CkgDomainLogic.General.Services;

namespace CkgDomainLogic.Charts.Services
{
    public class ChartsDataServiceSql : CkgGeneralDataService, IChartsDataService 
    {
        static string ConnectionString { get { return "KBA"; } }

        static ChartsSqlDbContext CreateChartsDbContext() { return new ChartsSqlDbContext(ConnectionString); }


        public IEnumerable<KbaPlzKgs> GetGroupKgsItems(KgsSelector selector)
        {
            return CreateChartsDbContext().GetGroupKbaPlzKgsItems(selector);
        }

        public IEnumerable<KbaPlzKgs> GetDetailsKgsItems(KgsSelector selector)
        {
            return CreateChartsDbContext().GetDetailsKbaPlzKgsItems(selector);
        }


        public IEnumerable<ChartEntity> GetGroupChartItems(IChartsSqlDataDescriptor dataDescriptor, ChartDataSelector selector)
        {
            return CreateChartsDbContext().GetGroupChartItems(dataDescriptor, selector);
        }

        public void GetDetailsChartItems(IChartsSqlDataDescriptor dataDescriptor, ChartDataSelector selector, out Type dynamicType, out IEnumerable<dynamic> dynamicObjects, params string[] detailsFilterValues)
        {
            CreateChartsDbContext().GetDetailsChartItems(dataDescriptor, selector, out dynamicType, out dynamicObjects, detailsFilterValues);
        }

        public IEnumerable<IEnumerable<ChartEntity>> GetAdditionalChartItemLists(IChartsSqlDataDescriptor dataDescriptor, ChartDataSelector selector)
        {
            return CreateChartsDbContext().GetAdditionalChartItemListsStatements(dataDescriptor, selector);
        }

        //
        // Tabelle "zvertr": KBA Zulassungsstellen
        //
        // Tabelle "ZFIL_FSP_VK" Verkauf Feinstaubplaketten
        //
        // Bapi "Z_FIL_STANDORTE" VKBUR Adressen
        //
    }
}
