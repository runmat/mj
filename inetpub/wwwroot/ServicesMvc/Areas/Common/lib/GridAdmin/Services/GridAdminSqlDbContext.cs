using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Services
{
    public class GridAdminSqlDbContext : DbContext
    {
        //public DbSet<GridAdminItemUser> GridAdminItemsUser { get; set; }

        public GridAdminSqlDbContext(string connectionString) : base(connectionString)  //: base(ConfigurationManager.AppSettings["Connectionstring"])
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            System.Data.Entity.Database.SetInitializer<GridAdminSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
