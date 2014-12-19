using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CkgDomainLogic.General.Database.Models;
using GeneralTools.Contracts;

namespace CkgDomainLogic.General.Services
{
    public class PersistenceSqlDbContext : DbContext
    {
        public PersistenceSqlDbContext()
            : base(ConfigurationManager.AppSettings["Connectionstring"])
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            System.Data.Entity.Database.SetInitializer<PersistenceSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public IEnumerable<IPersistableObjectContainer> GetPersistableObjectsFor(string ownerKey, string groupKey)
        {
            return Database.SqlQuery<PersistableObject>("SELECT * FROM PersistableObject WHERE OwnerKey = {0} and GroupKey = {1}", ownerKey, groupKey);
        }
    }
}
