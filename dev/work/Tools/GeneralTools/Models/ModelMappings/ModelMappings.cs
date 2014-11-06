using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GeneralTools.Models
{
    public class ModelMappings
    {
        private static readonly Dictionary<string, object> ModelMappingSingletons = new Dictionary<string, object>();

        public static string LastFailedPropertyMapping { get; set; }

        protected static ModelMapping<T1, T2> EnsureSingleton<T1, T2>(Func<ModelMapping<T1, T2>> mappingCreateFunction)
            where T1 : class, new()
            where T2 : class, new()
        {
            if (mappingCreateFunction.Method.ReturnParameter == null) return null;
            var member = mappingCreateFunction.Method.ReturnParameter.Member;
            var dictKey = member.ReflectedType.FullName + "_" + member.Name;
            if (ModelMappingSingletons.ContainsKey(dictKey))
            {
                object storedMapping;
                ModelMappingSingletons.TryGetValue(dictKey, out storedMapping);
                if (storedMapping != null)
                    return (ModelMapping<T1, T2>)storedMapping;
            }

            var mapping = mappingCreateFunction();
            if (ModelMappingSingletons.ContainsKey(dictKey))
                ModelMappingSingletons[dictKey] = mapping;
            else
                ModelMappingSingletons.Add(dictKey, mapping);

            return mapping;
        }

        [Obsolete("Please use the parameterless version instead: ValidateAndRaiseError()")]
        public void ValidateAndRaiseError<T>()
        {
            ValidateAndRaiseError(typeof(T));
        }

        public void ValidateAndRaiseError()
        {
            ValidateAndRaiseError(this.GetType());
        }

        public string GetMappingErrors()
        {
            try
            {
                ValidateAndRaiseError(this.GetType());
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "";
        }

        public void ValidateAndRaiseError(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
            var mappings = properties.Where(p => p.PropertyType.Name.ToLower().StartsWith("modelmapping")).ToList();
            mappings.ForEach(mapping =>
            {
                var validationFunc = mapping.PropertyType.GetMethod("Valid");
                if (validationFunc != null)
                {
                    var isValid = (bool)validationFunc.Invoke(mapping.GetValue(null, new object[0]), new object[0]);
                    if (!isValid)
                        throw new Exception(String.Format("Error {0}.{1}: {2}", this.GetType().Name, mapping.Name, LastFailedPropertyMapping));
                }
            });
        }
    }

}
