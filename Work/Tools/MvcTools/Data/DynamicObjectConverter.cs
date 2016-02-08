using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using GeneralTools.Models;

namespace MvcTools.Data
{
    public class DynamicObjectConverter
    {
        public static T MapDynamicObjectToDestinationObject<T>(dynamic inputObject)
            where T : class, new()
        {
            var propertiesDst = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return ConvertDynamicObjectToDestinationObject<T>(inputObject, propertiesDst);
        }

        public static List<T> MapDynamicObjectListToDestinationObjectList<T>(IEnumerable<dynamic> inputObjects, Action<T> afterMappingAction = null)
            where T : class, new()
        {
            var destinationList = new List<T>();

            if (inputObjects.Any())
            {
                var propertiesDst = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var item in inputObjects)
                {
                    var dstObject = ConvertDynamicObjectToDestinationObject<T>(item, propertiesDst);

                    if (afterMappingAction != null)
                        afterMappingAction(dstObject);

                    destinationList.Add(dstObject);
                }
            }

            return destinationList;
        }

        private static T ConvertDynamicObjectToDestinationObject<T>(dynamic inputObject, PropertyInfo[] destinationProperties)
            where T : class, new()
        {
            var destination = new T();

            var propertyNamesSrc = ((IDictionary<string, object>)inputObject).Keys.ToList();

            foreach (var propertyNameSrc in propertyNamesSrc)
            {
                var propertyDst = destinationProperties.FirstOrDefault(pDst => pDst.Name.ToLower() == propertyNameSrc.ToLower());
                if (propertyDst != null && propertyDst.GetSetMethod() != null && propertyDst.GetCustomAttributes(true).OfType<ModelMappingCopyIgnoreAttribute>().None())
                {
                    var inputValue = ((IDictionary<string, object>) inputObject)[propertyNameSrc];
                    if (inputValue != null)
                        propertyDst.SetValue(destination, inputValue.ToString().TryConvertToDestinationType(propertyDst, true), null);
                }
            }

            return destination;
        }

        public static dynamic CreateDynamicObjectFromDatarow(DataRow row)
        {
            var item = new ExpandoObject();

            for (var i = 0; i < row.Table.Columns.Count; i++)
            {
                ((IDictionary<string, object>)item).Add(string.Format("Column{0}", i), row[i].ToString());
            }

            return item;
        }
    }
}
