using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CkgImportLocalizationResourcesFromDb
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

        public static IEnumerable<T> Copy<T>(IEnumerable<T> srcEntities, Action<T, T> onInit = null)
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

        private static void UpdateRecursivelyWithBaseClasses<T1, T2>(T1 source, T2 destination)
            where T1 : class
            where T2 : class
        {
            var propertiesSrc = typeof(T1).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertiesDst = typeof(T2).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertySrc in propertiesSrc)
            {
                var propertyDst = propertiesDst.FirstOrDefault(pDst => pDst.Name.ToLower() == propertySrc.Name.ToLower());
                if (propertyDst != null && propertyDst.GetSetMethod() != null)
                {
                    propertyDst.SetValue(destination, propertySrc.GetValue(source, null), null);
                }
            }
        }
    }
}
