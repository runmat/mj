using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GeneralTools.Contracts;

namespace GeneralTools.Models
{
    public class ModelBase : INotifyPropertyChanged
    {
        public static int GetPropertyCount(List<string> properties)
        {
            if (properties == null)
                return 0;

            return properties.Count;
        }

        public static List<string> GetPropertyNames(List<string> properties)
        {
            if (GetPropertyCount(properties) == 0)
                return new List<string>();

            var propertyArray = properties.ToArray();
            for (var i = 0; i < propertyArray.Length; i++)
                propertyArray[i] = propertyArray[i].Trim();

            return propertyArray.ToList();
        }

        public static List<object> GetPropertyValues(object entity, List<string> properties)
        {
            if (GetPropertyCount(properties) == 0)
                return new List<object>();

            var propertyNames = GetPropertyNames(properties);
            var propertyValues = new List<object>();
            var type = entity.GetType();
            var propertiesModels = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pSource in propertiesModels.Where(pS => pS.GetGetMethod() != null).Where(p => propertyNames.Count == 0 || propertyNames.Contains(p.Name)))
                propertyValues.Add(pSource.GetValue(entity, null));

            return propertyValues;
        }

        public static List<string> GetNullValuePropertyNames(object entity, List<string> properties)
        {
            if (GetPropertyCount(properties) == 0)
                return new List<string>();

            var propertyValues = GetPropertyValues(entity, properties);
            var propertyNames = GetPropertyNames(properties);
            var nullPropertyNames = new List<string>();
            var i = 0;
            propertyValues.ForEach(val =>
            {
                if (val is string)
                {
                    if (string.IsNullOrEmpty((string)val))
                        nullPropertyNames.Add(propertyNames[i]);
                }
                else if (val == null)
                    nullPropertyNames.Add(propertyNames[i]);
                else if (val is INullable && (val as INullable).IsNull())
                    nullPropertyNames.Add(propertyNames[i]);

                i++;
            });
            return nullPropertyNames;
        }

        /// <summary>
        /// null or empty items
        /// </summary>
        public static int NullCount(object entity, List<string> properties)
        {
            if (GetPropertyCount(properties) == 0)
                return 0;

            return GetNullValuePropertyNames(entity, properties).Count;
        }

        /// <summary>
        /// NOT null and NOT empty items
        /// </summary>
        public static int NotNullCount(object entity, List<string> properties)
        {
            if (GetPropertyCount(properties) == 0)
                return 0;

            return GetPropertyCount(properties) - NullCount(entity, properties);
        }

        /// <summary>
        /// all items are null or empty
        /// </summary>
        public static bool AllNull(object entity, List<string> properties)
        {
            if (GetPropertyCount(properties) == 0)
                return false;

            return NotNullCount(entity, properties) == 0;
        }

        /// <summary>
        /// all items are NOT null and NOT empty
        /// </summary>
        public static bool AllNotNull(object entity, List<string> properties)
        {
            if (GetPropertyCount(properties) == 0)
                return false;

            return NullCount(entity, properties) == 0;
        }

        /// <summary>
        /// at least one item is null or empty
        /// </summary>
        public static bool AtLeastOneNull(object entity, List<string> properties)
        {
            if (GetPropertyCount(properties) == 0)
                return false;

            return NullCount(entity, properties) > 0;
        }

        /// <summary>
        /// at least one item is NOT null and NOT empty
        /// </summary>
        public static bool AtLeastOneNotNull(object entity, List<string> properties)
        {
            if (GetPropertyCount(properties) == 0)
                return false;

            return NotNullCount(entity, properties) > 0;
        }

        public static bool AtLeastOneRequiredAsGroupPropertiesValid(object entity)
        {
            return !AllNull(entity, GetRequiredAsGroupPropertyNameListToCheck(entity));
        }

        public static List<string> GetRequiredAsGroupPropertyNameListToCheck(object entity)
        {
            return entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(true).OfType<RequiredAsGroupAttribute>().Any()).Select(p => p.Name).ToList();
        }

        public static List<string> GetRequiredButModelOptionalPropertyNameListToCheck(object entity)
        {
            return entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(true).OfType<RequiredButModelOptionalAttribute>().Any()).Select(p => p.Name).ToList();
        }

        public static List<string> GetRequiredButModelOptionalPropertyNamesWithNullValues(object entity, List<string> properties)
        {
            return GetNullValuePropertyNames(entity, properties);
        }

        #region INotifyPropertyChanged

        /// <summary>
        /// Raised when a property on this object has a new value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this ViewModels PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that has a new value</param>
        protected void SendPropertyChanged(string propertyName)
        {
            SendPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this Models PropertyChanged event
        /// </summary>
        /// <param name="e">Arguments detailing the change</param>
        protected virtual void SendPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void SendPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }
            SendPropertyChanged(memberExpression.Member.Name);
        }

        #endregion
    }
}
