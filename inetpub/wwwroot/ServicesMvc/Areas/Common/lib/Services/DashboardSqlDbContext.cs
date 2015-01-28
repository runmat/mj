using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CkgDomainLogic.General.Database.Models;

namespace CkgDomainLogic.General.Services
{
    public class DashboardSqlDbContext : DbContext
    {
        public DashboardSqlDbContext()
            : base(ConfigurationManager.AppSettings["Connectionstring"])
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            System.Data.Entity.Database.SetInitializer<DashboardSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public IEnumerable<DashboardItem> GetDashboardItems()
        {
            return Database.SqlQuery<DashboardItem>("SELECT * FROM DashboardItem");
        }
    }
}
