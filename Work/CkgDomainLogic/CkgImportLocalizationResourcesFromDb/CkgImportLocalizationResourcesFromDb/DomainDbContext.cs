using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CkgImportLocalizationResourcesFromDb
{
    public class DomainDbContext : DbContext
    {
        public DomainDbContext(string connectionString) : base(connectionString)  { }

        public IEnumerable<string> GetResourceSchluessel()
        {
            return Database.SqlQuery<string>("SELECT Resource FROM TranslatedResource");
        }

        public DbSet<TranslatedResource> TranslatedResources { get; set; }

        public DbSet<TranslatedResourceCustom> TranslatedResourcesCustom { get; set; }

        public DbSet<ConfigAllServers> ConfigsAllServers { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<DomainDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
