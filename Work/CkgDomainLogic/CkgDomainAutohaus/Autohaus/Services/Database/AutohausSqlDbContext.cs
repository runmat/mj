using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using CkgDomainLogic.Autohaus.Models;

namespace CkgDomainLogic.Autohaus.Services
{
    public class AutohausSqlDbContext : DbContext
    {
        public DbSet<Fahrzeug> Fahrzeuge { get; set; }

        public DbSet<BeauftragteZulassung> BeauftragteZulassungen { get; set; }

        public AutohausSqlDbContext()
            : base(ConfigurationManager.AppSettings["Connectionstring"])
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<AutohausSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        protected override DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            // Let our ViewModels and Models take full control over all validation issues ;-)
            return null;
        }
    }
}
