using System;
using System.Collections.Generic;
using System.Linq;
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
                p.EditDate = objectContainer.EditDate;
            }

            return o;
        }

        public object SaveObject(string objectKey, string ownerKey, string groupKey, string userName, object o, object o2)
        {
            if (o == null)
                o = new Store { ObjectKey = SaveIgnoreHint };
            var type = o.GetType();
            var typeName = type.GetFullTypeName();
            var objectData = XmlService.XmlSerializeToString(o);

            if (o2 == null)
                o2 = new Store { ObjectKey = SaveIgnoreHint };
            var type2 = o2.GetType();
            var typeName2 = type2.GetFullTypeName();
            var objectData2 = XmlService.XmlSerializeToString(o2);

            var persistedContainersBeforeSave = (IEnumerable<IPersistableObjectContainer>)null;
            if (objectKey.IsNullOrEmpty())
                persistedContainersBeforeSave = GetObjectContainers(ownerKey, groupKey);


            PersistObject(objectKey, ownerKey, groupKey, userName, typeName, objectData, typeName2, objectData2);


            var persistedContainersAfterSave = (IEnumerable<IPersistableObjectContainer>)null;
            if (objectKey.IsNullOrEmpty())
                persistedContainersAfterSave = GetObjectContainers(ownerKey, groupKey);

            if (persistedContainersBeforeSave == null || persistedContainersAfterSave == null)
                return o;

            var validPersistedObjectContainersBeforeSave = persistedContainersBeforeSave.Where(pc => pc.Object as IPersistableObject != null);
            var validPersistedObjectContainersAfterSave = persistedContainersAfterSave.Where(pc => pc.Object as IPersistableObject != null);

            var persistedObjectsBeforeSave = validPersistedObjectContainersBeforeSave.Select(pc => pc.Object as IPersistableObject);
            var persistedObjectsAfterSave = validPersistedObjectContainersAfterSave.Select(pc => pc.Object as IPersistableObject);
            var persistedObjects2BeforeSave = validPersistedObjectContainersBeforeSave.Select(pc => pc.Object2 as IPersistableObject);
            var persistedObjects2AfterSave = validPersistedObjectContainersAfterSave.Select(pc => pc.Object2 as IPersistableObject);

            var newItem = persistedObjectsAfterSave.Except(persistedObjectsBeforeSave, new IPersistableObjectComparer()).FirstOrDefault();
            var newItem2 = persistedObjects2AfterSave.Except(persistedObjects2BeforeSave, new IPersistableObjectComparer()).FirstOrDefault();
            if (newItem != null || newItem2 != null)
            {
                objectKey = (newItem ?? newItem2).ObjectKey;

                objectData = XmlService.XmlSerializeToString(newItem ?? new Store { ObjectKey = SaveIgnoreHint });

                objectData2 = XmlService.XmlSerializeToString(newItem2 ?? new Store { ObjectKey = SaveIgnoreHint });

                PersistObject(objectKey, ownerKey, groupKey, userName, typeName, objectData, typeName2, objectData2);
            }

            return newItem;
        }

        public void DeleteObject(string objectKey)
        {
            DeletePersistedObject(objectKey);
        }
    }
}
