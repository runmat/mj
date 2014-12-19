using System;
using System.Collections.Generic;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    public abstract class PersistanceBaseService : IPersistanceService
    {
        protected abstract IEnumerable<IPersistableObjectContainer> ReadPersistedObjectContainers(string ownerKey, string groupKey);

        protected abstract void PersistObject(string objectKey, string ownerKey, string groupKey, string userName, string objectType, string objectData);


        public IEnumerable<IPersistableObjectContainer> GetObjectContainers(string ownerKey, string groupKey)
        {
            var objectContainers = ReadPersistedObjectContainers(ownerKey, groupKey).ToListOrEmptyList();

            objectContainers.ForEach(objectContainer =>
                {
                    var type = Type.GetType(objectContainer.ObjectType);
                    if (type == null)
                        return;

                    var o = XmlService.XmlDeserializeFromString(objectContainer.ObjectData, type);
                    if (o is IPersistableObject)
                    {
                        var p = (o as IPersistableObject);
                        p.ObjectKey = objectContainer.ObjectKey;
                        p.EditUser = objectContainer.EditUser;
                        p.EditDate = objectContainer.EditDate;
                    }
                    objectContainer.Object = o;
                });

            return objectContainers;
        }

        public void SaveObject(string objectKey, string ownerKey, string groupKey, string userName, object o)
        {
            var type = o.GetType();
            var typeName = type.GetFullTypeName();
            var objectData = XmlService.XmlSerializeToString(o);

            PersistObject(objectKey, ownerKey, groupKey, userName, typeName, objectData);
        }
    }
}
