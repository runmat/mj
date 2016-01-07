using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    public abstract class PersistanceBaseService : IPersistanceService
    {
        protected const string SaveIgnoreHint = "DONT SAVE, PLEASE IGNORE!";

        protected abstract IEnumerable<IPersistableObjectContainer> ReadPersistedObjectContainers(string ownerKey, string groupKey);

        protected abstract void PersistObject(string objectKey, string ownerKey, string groupKey, string userName, string objectType, string objectData, string objectType2, string objectData2);

        protected abstract void DeletePersistedObject(string objectKey);

        public abstract void DeleteAllObjects(string ownerKey, string groupKey, string additionalFilter);


        public IEnumerable<IPersistableObjectContainer> GetObjectContainers(string ownerKey, string groupKey)
        {
            var objectContainers = ReadPersistedObjectContainers(ownerKey, groupKey).ToListOrEmptyList();

            objectContainers.ForEach(objectContainer =>
                {
                    object o;

                    o = TrySetPersistableObject(objectContainer, objectContainer.ObjectType, objectContainer.ObjectData);
                    if (o != null)
                        objectContainer.Object = o;

                    o = TrySetPersistableObject(objectContainer, objectContainer.ObjectType2, objectContainer.ObjectData2);
                    if (o != null)
                        objectContainer.Object2 = o;
                });

            return objectContainers;
        }

        private static object TrySetPersistableObject(IPersistableObjectContainer objectContainer, string objectType, string objectData)
        {
            Type type = null;
            if (objectType != null)
                type = Type.GetType(objectType);
            if (type == null)
                return null;

            var o = XmlService.XmlDeserializeFromString(objectData, type);
            if (o is IPersistableObject)
            {
                var p = (o as IPersistableObject);
                p.ObjectKey = objectContainer.ObjectKey;
                p.EditUser = objectContainer.EditUser;
                if (objectContainer.EditDate != null && objectContainer.EditDate.GetValueOrDefault().Hour > 0 && objectContainer.EditDate.GetValueOrDefault().Minute > 0 && objectContainer.EditDate.GetValueOrDefault().Second > 0)
                    p.EditDate = objectContainer.EditDate;
            }

            return o;
        }

        public IPersistableObject SaveObject(string objectKey, string ownerKey, string groupKey, string userName, IPersistableObject o)
        {
            IPersistableObject o2 = null;
            SaveObject(objectKey, ownerKey, groupKey, userName, ref o, ref o2);
            return o;
        }

        public void SaveObject(string objectKey, string ownerKey, string groupKey, string userName, ref IPersistableObject o, ref IPersistableObject o2)
        {
            o = o ?? new Store { ObjectKey = objectKey, ObjectName = SaveIgnoreHint };
            if (o.EditDate == null) o.EditDate = DateTime.Now;
            var type = o.GetType();
            var typeName = type.GetFullTypeName();
            var objectData = XmlService.XmlSerializeToString(o);

            o2 = o2 ?? new Store { ObjectKey = objectKey, ObjectName = SaveIgnoreHint };
            if (o2.EditDate == null) o2.EditDate = DateTime.Now;
            var type2 = o2.GetType();
            var typeName2 = type2.GetFullTypeName();
            var objectData2 = XmlService.XmlSerializeToString(o2);


            var useDoubleSavingToEnsureObjectKey = (objectKey.IsNullOrEmpty() || o.ObjectKey.IsNullOrEmpty() || o2.ObjectKey.IsNullOrEmpty());

            var persistedContainersBeforeSave = (IEnumerable<IPersistableObjectContainer>)new List<IPersistableObjectContainer>();
            if (useDoubleSavingToEnsureObjectKey )
                persistedContainersBeforeSave = GetObjectContainers(ownerKey, groupKey);


            PersistObject(objectKey, ownerKey, groupKey, userName, typeName, objectData, typeName2, objectData2);


            if (!useDoubleSavingToEnsureObjectKey)
                return;

            var persistedContainersAfterSave = GetObjectContainers(ownerKey, groupKey);

            var validPersistedObjectContainersBeforeSave = persistedContainersBeforeSave.Where(pc => pc.Object is IPersistableObject);
            var validPersistedObjectContainersAfterSave = persistedContainersAfterSave.Where(pc => pc.Object is IPersistableObject);

            var persistedObjectsBeforeSave = validPersistedObjectContainersBeforeSave.Select(pc => pc.Object as IPersistableObject);
            var persistedObjectsAfterSave = validPersistedObjectContainersAfterSave.Select(pc => pc.Object as IPersistableObject);
            var newItem = persistedObjectsAfterSave.Except(persistedObjectsBeforeSave, new IPersistableObjectComparer()).FirstOrDefault();
            if (newItem == null && objectKey.IsNotNullOrEmpty())
                newItem = persistedObjectsAfterSave.FirstOrDefault(n => n != null && n.ObjectKey == objectKey);

            var persistedObjects2BeforeSave = validPersistedObjectContainersBeforeSave.Select(pc => pc.Object2 as IPersistableObject);
            var persistedObjects2AfterSave = validPersistedObjectContainersAfterSave.Select(pc => pc.Object2 as IPersistableObject);
            var newItem2 = persistedObjects2AfterSave.Except(persistedObjects2BeforeSave, new IPersistableObjectComparer()).FirstOrDefault();
            if (newItem2 == null && objectKey.IsNotNullOrEmpty())
                newItem2 = persistedObjects2AfterSave.FirstOrDefault(n => n != null && n.ObjectKey == objectKey);

            if (newItem == null && newItem2 == null)
                return;
            
            objectKey = (newItem ?? newItem2).ObjectKey;

            o = newItem ?? o;
            objectData = XmlService.XmlSerializeToString(o);
            type = o.GetType();
            typeName = type.GetFullTypeName();

            o2 = newItem2 ?? o2;
            objectData2 = XmlService.XmlSerializeToString(o2);
            type2 = o2.GetType();
            typeName2 = type2.GetFullTypeName();


            PersistObject(objectKey, ownerKey, groupKey, userName, typeName, objectData, typeName2, objectData2);


            if (newItem != null)
                o = newItem;

            if (newItem2 != null)
                o2 = newItem2;
        }

        public void DeleteObject(string objectKey)
        {
            DeletePersistedObject(objectKey);
        }


        public List<T> GetObjects<T>(string ownerKey, string groupKey)
        {
            return GetObjectContainers(ownerKey, groupKey)
                .Select(pContainer => (T) pContainer.Object)
                    .ToListOrEmptyList();
        }

        public JsonItemsPackage GetCachedChartItemAsJson(string objectId, string ownerKey, string groupKey, 
                                                                string editUserName, bool clearDataCache,
                                                                Func<JsonItemsPackage, bool> expirationCheckFunc,
                                                                Func<JsonItemsPackage> getItemDataFunc
                                                                )
        {
            var storedDashboardItemExpired = false;
            var itemData = GetObjects<JsonItemsPackage>(ownerKey, groupKey).FirstOrDefault(c => c.ID == objectId);
            if (itemData != null && itemData.dataAsText != null)
            {
                // load cached json data
                itemData.data = new JavaScriptSerializer().DeserializeObject(itemData.dataAsText);

                // check for cached data expiration
                storedDashboardItemExpired = expirationCheckFunc(itemData);
            }

            if (itemData == null || storedDashboardItemExpired || clearDataCache)
            {
                // no cached json data available  or  cached data has expired
                var storedObjectKey = itemData == null ? null : itemData.ObjectKey;

                itemData = getItemDataFunc(); 
                itemData.ID = objectId;
                if (itemData.data != null)
                {
                    itemData.dataAsText = new JavaScriptSerializer().Serialize(itemData.data);
                    SaveObject(storedObjectKey, ownerKey, groupKey, editUserName, itemData);
                }
            }

            return itemData;
        }
    }
}
