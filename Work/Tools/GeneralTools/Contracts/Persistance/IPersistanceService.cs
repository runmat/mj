using System.Collections.Generic;

namespace GeneralTools.Contracts
{
    public interface IPersistanceService
    {
        IEnumerable<IPersistableObjectContainer> GetObjectContainers(string ownerKey, string groupKey);

        void SaveObject(string objectKey, string ownerKey, string groupKey, string userName, ref IPersistableObject o, ref IPersistableObject o2);

        void DeleteObject(string objectKey);
    }
}
