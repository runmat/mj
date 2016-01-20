using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using GeneralTools.Contracts;

namespace GeneralTools.Models
{
    public class ModelMapping
    {
        /// <summary>
        /// Kopiert Properties mit gleichen Namen automatisch zwischen zwei Model Klassen
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="onInit"></param>
        /// <returns></returns>
        public static T2 Copy<T1, T2>(T1 source, T2 destination, Action<T1, T2> onInit = null)
            where T1 : class
            where T2 : class
        {
            if (source == null || destination == null)
                return null;

            UpdateRecursivelyWithBaseClasses(source, destination);
            if (onInit != null)
                onInit(source, destination);
                
            return destination;
        }

        public static T2 Copy<T1, T2>(T1 source, Action<T1, T2> onInit = null)
            where T1 : class
            where T2 : class, new()
        {
            if (source == null)
                return null;

            var destination = new T2();

            UpdateRecursivelyWithBaseClasses(source, destination);
            if (onInit != null)
                onInit(source, destination);

            return destination;
        }

        public static T Copy<T>(T source, Action<T, T> onInit = null)
            where T : class, new()
        {
            if (source == null)
                return null;

            var destination = new T();
            UpdateRecursivelyWithBaseClasses(source, destination);
            if (onInit != null)
                onInit(source, destination);

            return destination;
        }

        public static void CopyPropertiesTo<T>(T source, T destination, Action<T, T> onInit = null)
            where T : class, new()
        {
            if (source == null || destination == null)
                return;

            UpdateRecursivelyWithBaseClasses(source, destination);
            if (onInit != null)
                onInit(source, destination);
        }

        public static T CopyOnlyPersistableProperties<T>(T source, Action<T, T> onInit = null)
            where T : class, new()
        {
            if (source == null)
                return null;

            var destination = new T();
            UpdateRecursivelyWithBaseClasses(source, destination, 'X',
                (propertySrc, propertyDst) =>
                    propertySrc.GetCustomAttributes(true).OfType<FormPersistableAttribute>().Any() &&
                    propertyDst.GetCustomAttributes(true).OfType<FormPersistableAttribute>().Any()
                );
            if (onInit != null)
                onInit(source, destination);

            return destination;
        }

        public static IEnumerable<T> CopyList<T>(IEnumerable<T> srcEntities, Action<T, T> onInit = null)
            where T : class, new()
        {
            return srcEntities.Select(src => Copy(src, new T(), onInit));
        }

        public static IEnumerable<T2> Copy<T1, T2>(IEnumerable<T1> srcEntities, Action<T1, T2> onInit = null)
            where T1 : class
            where T2 : class, new()
        {
            return srcEntities.Select(src => Copy(src, new T2(), onInit));
        }

        public static bool Copy(DataRow srcRow, DataRow dstRow)
        {
            var colsFound = false;

            for (var i = 0; i < srcRow.Table.Columns.Count; i++)
            {
                var colName = srcRow.Table.Columns[i].ColumnName;

                if (dstRow.Table.Columns.Contains(colName) && dstRow.Table.Columns[colName].DataType == srcRow.Table.Columns[colName].DataType)
                {
                    colsFound = true;
                    dstRow[colName] = srcRow[colName];
                }
            }

            return colsFound;
        }

        public static DataTable Copy(DataTable srcTable, DataTable dstTable)
        {
            foreach (DataRow row in srcTable.Rows)
            {
                var newRow = dstTable.NewRow();

                if (Copy(row, newRow))
                    dstTable.Rows.Add(newRow);
            }

            return dstTable;
        }

        private static void UpdateRecursivelyWithBaseClasses<T1, T2>(T1 source, T2 destination, char booleanStringConvertCharacter = 'X', Func<PropertyInfo, PropertyInfo, bool> additionalCopyConditionFunc = null)
            where T1 : class
            where T2 : class
        {
            var propertiesSrc = typeof(T1).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertiesDst = typeof(T2).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertySrc in propertiesSrc)
            {
                var propertyDst = propertiesDst.FirstOrDefault(pDst => pDst.Name.ToLower() == propertySrc.Name.ToLower());
                if (propertyDst != null && propertyDst.GetSetMethod() != null
                    && propertySrc.GetCustomAttributes(true).OfType<ModelMappingCopyIgnoreAttribute>().None()
                    && propertyDst.GetCustomAttributes(true).OfType<ModelMappingCopyIgnoreAttribute>().None()
                    && (additionalCopyConditionFunc == null || additionalCopyConditionFunc(propertySrc, propertyDst)))
                {
                    propertyDst.SetValue(destination, TryConvertValue(propertySrc, propertyDst, propertySrc.GetValue(source, null), booleanStringConvertCharacter), null);
                }
            }

            var modelMappingAppliedItem = (destination as IModelMappingApplied);
            if (modelMappingAppliedItem != null)
                modelMappingAppliedItem.OnModelMappingApplied();
        }

        public static object TryConvertValue(PropertyInfo propertySrc, PropertyInfo propertyDst, object value, char booleanStringConvertCharacter)
        {
            if (propertySrc.PropertyType == propertyDst.PropertyType)
                return TryProcessValidationAttributeWithModelMapping(propertyDst, value);

            value = TryConvertBooleanValue(propertySrc, propertyDst, value, booleanStringConvertCharacter);
            value = TryConvertIntegerValue(propertySrc, propertyDst, value);
            value = TryConvertDateTimeValue(propertySrc, propertyDst, value);

            var validationAttributeWithModelMapping = propertyDst.GetCustomAttributes(true).OfType<IAttributeWithModelMappingConvert>().FirstOrDefault();
            if (validationAttributeWithModelMapping != null)
                value = validationAttributeWithModelMapping.ModelMappingConvert(value);

            return TryProcessValidationAttributeWithModelMapping(propertyDst, value);
        }

        static object TryProcessValidationAttributeWithModelMapping(PropertyInfo propertyDst, object value)
        {
            var validationAttributeWithModelMapping = propertyDst.GetCustomAttributes(true).OfType<IAttributeWithModelMappingConvert>().FirstOrDefault();
            if (validationAttributeWithModelMapping != null)
                value = validationAttributeWithModelMapping.ModelMappingConvert(value);

            return value;
        }

        static object TryConvertBooleanValue(PropertyInfo propertySrc, PropertyInfo propertyDst, object value, char booleanStringConvertCharacter)
        {
            if (propertySrc.PropertyType == typeof(string) && propertyDst.PropertyType == typeof(bool))
                value = ((string)value).NotNullOrEmpty() == booleanStringConvertCharacter.ToString();

            if (propertyDst.PropertyType == typeof(string) && propertySrc.PropertyType == typeof(bool))
                value = ((bool)value) ? booleanStringConvertCharacter.ToString() : "";

            return value;
        }

        static object TryConvertIntegerValue(PropertyInfo propertySrc, PropertyInfo propertyDst, object value)
        {
            if (propertySrc.PropertyType == typeof(string) && propertyDst.PropertyType == typeof(int?))
                value = TryConvertValue(propertyDst, value);

            if (propertyDst.PropertyType == typeof(string) && propertySrc.PropertyType == typeof(int?))
                value = (value == null ? "" : ((int?)value).GetValueOrDefault().ToString());

            return value;
        }

        static object TryConvertDateTimeValue(PropertyInfo propertySrc, PropertyInfo propertyDst, object value)
        {
            if (propertySrc.PropertyType == typeof(string) && propertyDst.PropertyType == typeof(DateTime?))
                value = TryConvertValue(propertyDst, value);

            if (propertyDst.PropertyType == typeof(string) && propertySrc.PropertyType == typeof(DateTime?))
                value = (value == null ? null : ((DateTime?)value).GetValueOrDefault().ToString());

            return value;
        }

        public static object TryConvertValue(PropertyInfo property, object value, string locale = "de")
        {
            if (property.PropertyType == typeof(DateTime?))
            {
                var sValue = value.ToString();
                if (locale != "de")
                {
                    sValue = sValue.Replace('.', '/');
                    sValue = sValue.Replace(" 00:00:00", "");
                }

                DateTime tmpVal;
                // englische Datumsschreibweise berücksichtigen
                if (sValue.Contains("/") && DateTime.TryParseExact(sValue, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmpVal)) 
                {
                    value = tmpVal;
                }
                else if (DateTime.TryParse(sValue, out tmpVal))
                {
                    value = tmpVal;
                }
                else
                {
                    value = null;
                }
            }
            else if (property.PropertyType == typeof(bool?))
            {
                bool tmpVal;
                if (bool.TryParse(value.ToString(), out tmpVal))
                    value = tmpVal; else value = null;
            }
            else if (property.PropertyType == typeof(int?))
            {
                int tmpVal;
                if (int.TryParse(value.ToString(), out tmpVal))
                    value = tmpVal; else value = null;
            }

            return TryProcessValidationAttributeWithModelMapping(property, value);
        }

        /// <summary>
        /// Clears all properties of an object, but only properties with attribute "ModelMappingClearable"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemToClear"></param>
        public static void Clear<T>(T itemToClear)
            where T : class
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property != null && property.GetSetMethod() != null && property.GetCustomAttributes(true).OfType<ModelMappingClearableAttribute>().Any())
                {
                    var type = property.PropertyType;
                    var resetValue = type.IsValueType ? Activator.CreateInstance(type) : null;
                    property.SetValue(itemToClear, resetValue, null);    
                }
            }
        }

        public static List<string> Differences<T1, T2>(T1 source, T2 destination, char booleanStringConvertCharacter = 'X')
            where T1 : class
            where T2 : class
        {
            var differencesList = new List<string>();

            var propertiesSrc = typeof(T1).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertiesDst = typeof(T2).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertySrc in propertiesSrc)
            {
                var propertyDst = propertiesDst.FirstOrDefault(pDst => pDst.Name == propertySrc.Name);
                if (propertyDst != null
                    && propertySrc.GetCustomAttributes(true).OfType<ModelMappingCompareIgnoreAttribute>().None()
                    && propertyDst.GetCustomAttributes(true).OfType<ModelMappingCompareIgnoreAttribute>().None())
                {
                    var srcObject = propertySrc.GetValue(source, null);
                    var dstObject = propertyDst.GetValue(destination, null);
                    var srcVal = srcObject == null ? "" : TryConvertValue(propertySrc, propertyDst, srcObject, booleanStringConvertCharacter).ToString();
                    var dstVal = dstObject == null ? "" : dstObject.ToString();

                    if (propertyDst.PropertyType == typeof (int?) && propertySrc.PropertyType == typeof (int?))
                    {
                        if (srcVal == "0") srcVal = "";
                        if (dstVal == "0") dstVal = "";
                    }

                    if (srcVal != dstVal)
                        differencesList.Add(propertySrc.Name);
                }
            }

            return differencesList;
        }

        public static T CloneBySerializing<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
// ReSharper disable LocalizableElement
                throw new ArgumentException("The type must be serializable.", "source");
// ReSharper restore LocalizableElement
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    public class ModelMapping<T1, T2>
        where T1 : class, new()
        where T2 : class, new()
    {
        private IDictionary<string, string> _propertyMappings;

        private Action<T1, T2> _onInit;
        private Action<T2, T1> _onInitBack;

        public ModelMapping(IDictionary<string, string> propertyMappings, Action<T1, T2> onInit = null, Action<T2, T1> onInitBack = null)
        {
            Init(propertyMappings, onInit, onInitBack);
        }

        public ModelMapping(Action<T1, T2> onInit = null, Action<T2, T1> onInitBack = null)
        {
            Init(null, onInit, onInitBack);
        }

        void Init(IDictionary<string, string> propertyMappings, Action<T1, T2> onInit = null, Action<T2, T1> onInitBack = null)
        {
            _propertyMappings = propertyMappings ?? new Dictionary<string, string>();

            _onInit = onInit;
            _onInitBack = onInitBack;
        }

        #region Copy functions

        public IEnumerable<T2> Copy(IEnumerable<T1> srcEntities, Action<T1, T2> onInit = null)
        {
            return srcEntities.Select(e => Copy(e, onInit));
        }

        public IEnumerable<T1> CopyBack(IEnumerable<T2> srcEntities, Action<T2, T1> onInitBack = null)
        {
            return srcEntities.Select(e => CopyBack(e, onInitBack));
        }

        private static PropertyInfo[] GetPropertiesRecursivelyWithBaseClasses(IReflect type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public T2 Copy(T1 src, Action<T1, T2> onInit = null)
        {
            return Copy(src, new T2(), onInit);
        }

        public T2 Copy(T1 src, T2 dst, Action<T1, T2> onInit = null)
        {
            return Copy(src, dst, _propertyMappings, onInit);
        }

        private T2 Copy(T1 src, T2 dst, IDictionary<string, string> propertyMappings, Action<T1, T2> onInit = null)
        {
            UpdateRecursivelyWithBaseClasses(src, dst, propertyMappings, false);
            if (_onInit != null)
                _onInit(src, dst);
            if (onInit != null)
                onInit(src, dst);

            return dst;
        }

        public T1 CopyBack(T2 src, Action<T2, T1> onInitBack = null)
        {
            return CopyBack(src, new T1(), onInitBack);
        }

        public T1 CopyBack(T2 src, T1 dst, Action<T2, T1> onInitBack = null)
        {
            return CopyBack(src, dst, _propertyMappings, onInitBack);
        }

        private T1 CopyBack(T2 src, T1 dst, IDictionary<string, string> propertyMappings, Action<T2, T1> onInitBack = null)
        {
            UpdateRecursivelyWithBaseClasses(src, dst, propertyMappings, true);
            if (_onInitBack != null)
                _onInitBack(src, dst);
            if (onInitBack != null)
                onInitBack(src, dst);

            return dst;
        }

        private static void UpdateRecursivelyWithBaseClasses<TS, TD>(TS source, TD destination, IDictionary<string, string> propertyMappings, bool reverse, char booleanStringConvertCharacter = 'X')
        {
            var propertiesSrc = GetPropertiesRecursivelyWithBaseClasses(typeof(TS));
            var propertiesDst = GetPropertiesRecursivelyWithBaseClasses(typeof(TD));

            foreach (var propertyMapping in propertyMappings)
            {
                var propertNameSrc = reverse ? propertyMapping.Value : propertyMapping.Key;
                var propertNameDst = reverse ? propertyMapping.Key : propertyMapping.Value;

                var pSrc = propertiesSrc.FirstOrDefault(pS => (pS.Name.ToLower() == propertNameSrc.ToLower()));
                var pDst = propertiesDst.FirstOrDefault(pD => (pD.Name.ToLower() == propertNameDst.ToLower()));
                if (pSrc != null && pDst != null && pDst.GetSetMethod() != null
                    && pSrc.GetCustomAttributes(true).OfType<ModelMappingCopyIgnoreAttribute>().None()
                    && pDst.GetCustomAttributes(true).OfType<ModelMappingCopyIgnoreAttribute>().None())
                {
                    pDst.SetValue(destination, ModelMapping.TryConvertValue(pSrc, pDst, pSrc.GetValue(source, null), booleanStringConvertCharacter), null);
                }
            }

            var modelMappingAppliedItem = (destination as IModelMappingApplied);
            if (modelMappingAppliedItem != null)
                modelMappingAppliedItem.OnModelMappingApplied();
        }

        #endregion

        #region Validation

        public bool Valid()
        {
            return Valid<T1, T2>(_propertyMappings);
        }

        private static bool Valid<TS, TD>(IDictionary<string, string> propertyMappings)
        {
            return ValidRecursivelyWithBaseClasses<TS, TD>(propertyMappings);
        }

        private static bool ValidRecursivelyWithBaseClasses<TS, TD>(IDictionary<string, string> propertyMappings)
        {
            var propertiesSrc = GetPropertiesRecursivelyWithBaseClasses(typeof(TS));
            var propertiesDst = GetPropertiesRecursivelyWithBaseClasses(typeof(TD));

            foreach (var propertyMapping in propertyMappings)
            {
                var propertNameSrc = propertyMapping.Key;
                var propertNameDst = propertyMapping.Value;

                var pSrc = propertiesSrc.FirstOrDefault(pS => (pS.Name.ToLower() == propertNameSrc.ToLower())); // && (pS.GetSetMethod() != null));
                var pDst = propertiesDst.FirstOrDefault(pD => (pD.Name.ToLower() == propertNameDst.ToLower())); // && (pD.GetSetMethod() != null));
                if (pSrc == null || pDst == null)
                {
                    ModelMappings.LastFailedPropertyMapping = string.Format("{0} <=> {1}", propertyMapping.Key, propertyMapping.Value);
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
