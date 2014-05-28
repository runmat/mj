using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using CkgDomainLogic.Charts.Contracts;
using CkgDomainLogic.Charts.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.Charts.Services
{
    public class ChartsSqlDbContext : DbContext
    {
        protected string ConnectionString { get; private set; }


        public IEnumerable<KbaPlzKgs> GetGroupKbaPlzKgsItems(KgsSelector selector)
        {
            return Database.SqlQuery<KbaPlzKgs>(selector.GetGroupSqlSelectStatement());
        }

        public IEnumerable<KbaPlzKgs> GetDetailsKbaPlzKgsItems(KgsSelector selector)
        {
            return Database.SqlQuery<KbaPlzKgs>(selector.GetDetailsSqlSelectStatement());
        }


        public IEnumerable<ChartEntity> GetGroupChartItems(IChartsSqlDataDescriptor dataDescriptor, ChartDataSelector selector)
        {
            return Database.SqlQuery<ChartEntity>(dataDescriptor.GetGroupChartItemsStatement(selector));
        }

        public void GetDetailsChartItems(IChartsSqlDataDescriptor dataDescriptor, ChartDataSelector selector, out Type dynamicType, out IEnumerable<dynamic> dynamicObjects, params string[] detailsFilterValues)
        {
            this.GetDynamicObjects(dataDescriptor.GetDetailsChartItemsStatement(selector, detailsFilterValues), out dynamicType, out dynamicObjects);
        }

        public IEnumerable<IEnumerable<ChartEntity>> GetAdditionalChartItemListsStatements(IChartsSqlDataDescriptor dataDescriptor, ChartDataSelector selector)
        {
            return dataDescriptor.GetAdditionalChartItemListsStatements(selector).ToListOrEmptyList().Select(statement => Database.SqlQuery<ChartEntity>(statement));
        }


        public ChartsSqlDbContext(string connectionString) 
            : base(connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<ChartsSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
