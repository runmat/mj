using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace MvcTools.Data
{
    public class EfDataService
    {
        public static T Copy<T>(T src, T dst)
        {
            UpdateRecursivelyWithBaseClasses(src, dst);
            return dst;
        }

        public static T2 Copy<T1, T2>(T1 src, T2 dst, string commaSeparatedProperties)
        {
            UpdateRecursivelyWithBaseClasses(src, dst, commaSeparatedProperties);
            return dst;
        }

        private static void UpdateRecursivelyWithBaseClasses<T1, T2>(T1 src, T2 dst, string commaSeparatedProperties = null)
        {
            var type = src.GetType();
            while (type != null)
            {
                UpdateForType(src, dst, commaSeparatedProperties);
                type = type.BaseType;
            }
        }

        private static void UpdateForType<T1, T2>(T1 source, T2 destination, string commaSeparatedProperties = null)
        {
            var propertyArray = new string[] { };
            if (!string.IsNullOrEmpty(commaSeparatedProperties))
                propertyArray = commaSeparatedProperties.Split(',');
            for (var i = 0; i < propertyArray.Length; i++)
                propertyArray[i] = propertyArray[i].Trim();

            var propertiesSource = typeof(T1).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertiesDest = typeof(T2).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var pSource in propertiesSource.Where(pS => pS.GetSetMethod() != null).Where(p => propertyArray.Length == 0 || propertyArray.Contains(p.Name)))
            {
                var pDest = propertiesDest.FirstOrDefault(pD => (pD.Name == pSource.Name) && (pD.GetSetMethod() != null));
                if (pDest != null)
                    pDest.SetValue(destination, pSource.GetValue(source, null), null);
            }
        }


        public static bool KeyEquals<T>(T first, T second) where T : class
        {
            var type = first.GetType();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in properties)
            {
                var keyAttribute = ((KeyAttribute)(p.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault()));

                if (keyAttribute != null)
                    return (p.GetValue(first, null).ToString() == p.GetValue(second, null).ToString());
            }
            return false;
        }

        public static bool KeyEquals<T>(object idForFirst, T second) where T : class
        {
            var type = second.GetType();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in properties)
            {
                var keyAttribute = ((KeyAttribute)(p.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault()));

                if (keyAttribute != null)
                    return (idForFirst.ToString() == p.GetValue(second, null).ToString());
            }
            return false;
        }
    }
}
