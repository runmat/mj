// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CkgDomainLogic.General.Database.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Services
{
    public class PersistanceSqlDbContext : DbContext
    {
        public PersistanceSqlDbContext()
            : base(ConfigurationManager.AppSettings["Connectionstring"])
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            System.Data.Entity.Database.SetInitializer<PersistanceSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public IEnumerable<IPersistableObjectContainer> ReadPersistedObjectsFor(string ownerKey, string groupKey)
        {
            return Database.SqlQuery<PersistableObjectContainer>("SELECT * FROM PersistableObjectContainer WHERE OwnerKey = {0} and GroupKey = {1}", ownerKey, groupKey);
        }

        public void PersistObject(string objectKey, string ownerKey, string groupKey, string userName, string objectType, string objectData)
        {
            Database.ExecuteSqlCommand(
                "EXEC PersistableObjectContainerSave {0}, {1}, {2}, {3}, {4}, {5}",
                objectKey, ownerKey, groupKey, userName, objectType, objectData);
        }
    }
}
