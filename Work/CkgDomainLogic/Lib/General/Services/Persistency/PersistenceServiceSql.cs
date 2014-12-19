using System.Collections.Generic;
using GeneralTools.Contracts;

namespace CkgDomainLogic.General.Services
{
    public class PersistenceServiceSql : IPersistenceService 
    {
        public IPersistenceOwnerKeyProvider OwnerKeyProvider { get; set; }

        public IEnumerable<IPersistableObjectContainer> GetObjectContainers(string groupKey)
        {
            var ct = CreateDbContext();

            var dbItems = ct.GetPersistableObjectsFor(OwnerKeyProvider.PersistenceKey, groupKey);

            return dbItems;
        }

        public IPersistableObjectContainer ReadObjectContainer(string groupKey, string objectKey)
        {
            return null;
        }

        public void WriteObjectContainer(string groupKey, string objectKey, IPersistableObjectContainer objecContainer)
        {
        }

        private static PersistenceSqlDbContext CreateDbContext()
        {
            return new PersistenceSqlDbContext();
        }
    }
}
