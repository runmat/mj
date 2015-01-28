using System.Collections.Generic;
using GeneralTools.Contracts;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Services
{
    public class PersistanceServiceSql : PersistanceBaseService 
    {
        protected override IEnumerable<IPersistableObjectContainer> ReadPersistedObjectContainers(string ownerKey, string groupKey)
        {
            var ct = CreateDbContext();

            var dbItems = ct.ReadPersistedObjectsFor(ownerKey, groupKey);

            return dbItems;
        }

        protected override void PersistObject(string objectKey, string ownerKey, string groupKey, string userName, string objectType, string objectData)
        {
            var ct = CreateDbContext();

            ct.PersistObject(objectKey, ownerKey, groupKey, userName, objectType, objectData);
        }

        protected override void DeletePersistedObject(string objectKey)
        {
            var ct = CreateDbContext();

            ct.DeletePersistedObject(objectKey);
        }


        private static PersistanceSqlDbContext CreateDbContext()
        {
            return new PersistanceSqlDbContext();
        }
    }
}
