using System;
using System.Collections.Generic;
using GeneralTools.Models;

namespace GeneralTools.Contracts
{
    public interface IPersistanceService
    {
        IEnumerable<IPersistableObjectContainer> GetObjectContainers(string ownerKey, string groupKey);

        List<T> GetObjects<T>(string ownerKey, string groupKey);

        IPersistableObject SaveObject(string objectKey, string ownerKey, string groupKey, string userName, IPersistableObject o);
        void SaveObject(string objectKey, string ownerKey, string groupKey, string userName, ref IPersistableObject o, ref IPersistableObject o2);

        void DeleteObject(string objectKey);

        void DeleteAllObjects(string ownerKey, string groupKey, string additionalFilter);

        JsonItemsPackage GetCachedChartItemAsJson(string objectId, string ownerKey, string groupKey,  
                                                        string editUserName, bool clearDataCache,
                                                        Func<JsonItemsPackage, bool> expirationCheckFunc,
                                                        Func<JsonItemsPackage> getItemDataFunc);
    }
}
