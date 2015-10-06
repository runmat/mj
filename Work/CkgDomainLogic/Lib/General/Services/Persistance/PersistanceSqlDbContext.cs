// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CkgDomainLogic.General.Database.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;

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
            var sql = "SELECT * FROM PersistableObjectContainer WHERE GroupKey = {0}";
            if (ownerKey.IsNotNullOrEmpty())
                sql += " and OwnerKey = {1}";

            return Database.SqlQuery<PersistableObjectContainer>(sql, groupKey, ownerKey);
        }

        public void PersistObject(string objectKey, string ownerKey, string groupKey, string userName, string objectType, string objectData, string objectType2, string objectData2)
        {
            Database.ExecuteSqlCommand(
                "EXEC PersistableObjectContainerSave {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                objectKey, ownerKey, groupKey, userName, objectType, objectData, objectType2, objectData2);
        }

        public void DeletePersistedObject(string objectKey)
        {
            Database.ExecuteSqlCommand("delete from PersistableObjectContainer where ID = {0}", objectKey);
        }

        public void DeleteAllObjects(string ownerKey, string groupKey)
        {
            Database.ExecuteSqlCommand("delete from PersistableObjectContainer where OwnerKey = {0} and GroupKey = {1}", ownerKey, groupKey);
        }
    }
}
