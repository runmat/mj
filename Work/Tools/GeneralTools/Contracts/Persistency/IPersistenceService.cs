using System.Collections.Generic;

namespace GeneralTools.Contracts
{
    public interface IPersistenceService
    {
        IPersistenceOwnerKeyProvider OwnerKeyProvider { get; set; }

        IEnumerable<IPersistableObjectContainer> GetObjectContainers(string groupKey);
    }
}
