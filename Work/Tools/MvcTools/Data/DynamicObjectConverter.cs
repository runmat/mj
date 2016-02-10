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
            return ConvertDynamicObjectToDestinationObject<T>(inputObject);
        }

        public static List<T> MapDynamicObjectListToDestinationObjectList<T>(IEnumerable<dynamic> inputObjects, Action<T> afterMappingAction = null)
            where T : class, new()
        {
            var destinationList = new List<T>();

            if (inputObjects.Any())
            {
                foreach (var item in inputObjects)
                {
                    var dstObject = ConvertDynamicObjectToDestinationObject<T>(item);

                    if (afterMappingAction != null)
                        afterMappingAction(dstObject);

                    destinationList.Add(dstObject);
                }
            }

            return destinationList;
        }

        private static T ConvertDynamicObjectToDestinationObject<T>(dynamic inputObject)
            where T : class, new()
        {
            var destination = new T();

            var propertiesDst = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyDst in propertiesDst)
            {
                if (propertyDst.GetSetMethod() != null && propertyDst.GetCustomAttributes(true).OfType<ModelMappingCopyIgnoreAttribute>().None() && ((IDictionary<string, object>)inputObject).ContainsKey(propertyDst.Name))
                {
                    var valueSrc = ((IDictionary<string, object>)inputObject)[propertyDst.Name];
                    if (valueSrc != null)
                    {
                        if (PropertyHasSimpleType(propertyDst))
                        {
                            // "einfache" Property
                            propertyDst.SetValue(destination, valueSrc.ToString().TryConvertToDestinationType(propertyDst, true), null);
                        }
                        else
                        {
                            // Property einer enthaltenen Klasse
                            var valueDst = propertyDst.GetValue(destination, null);
                            var valueDstProperties = valueDst.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite).ToList();

                            foreach (var valueDstProperty in valueDstProperties)
                            {
                                if (((IDictionary<string, object>)valueSrc).ContainsKey(valueDstProperty.Name))
                                {
                                    var subValueSrc = ((IDictionary<string, object>)valueSrc)[valueDstProperty.Name];
                                    if (subValueSrc != null)
                                        valueDstProperty.SetValue(valueDst, subValueSrc.ToString().TryConvertToDestinationType(valueDstProperty, true), null);
                                }
                            }
                        }
                    }
                }
            }

            return destination;
        }

        private static bool PropertyHasSimpleType(PropertyInfo prop)
        {
            return ((prop.PropertyType.IsPrimitive)
                    || (prop.PropertyType == typeof (string))
                    || (prop.PropertyType == typeof (int?))
                    || (prop.PropertyType == typeof (long?))
                    || (prop.PropertyType == typeof (decimal))
                    || (prop.PropertyType == typeof (decimal?))
                    || (prop.PropertyType == typeof (float?))
                    || (prop.PropertyType == typeof (double?))
                    || (prop.PropertyType == typeof (DateTime))
                    || (prop.PropertyType == typeof (DateTime?))
                    || (prop.PropertyType == typeof (bool?)));
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
