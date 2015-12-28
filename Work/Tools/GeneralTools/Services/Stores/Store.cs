using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    /// <summary>
    /// Klasse zum Puffern von (vorerst nur) Property Inhalten (später ggfls. auch von anderen Inhalten)
    /// (Architektur durch Matthias Jenzen, 16.07.2013)
    /// 
    /// Erweiterung 19.12.2014: 
    /// Basis-Klasse für persistierbare Objekte, z. B. Warenkorb-Objekte
    /// </summary>
    public class Store : IPersistableObject
    {
        #region Property Store

        #region Get

        /// <summary>
        /// Holt Daten aus dem Cache für eine Property, diese Funktion darf nur aus dem Property Getter selber heraus aufgerufen werden (sonst => Exception) (Matthias Jenzen)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectCreateFunc"></param>
        /// <returns></returns>
        protected T PropertyCacheGet<T>(Func<T> objectCreateFunc) 
        {
            var invocationList = objectCreateFunc.GetInvocationList();
            if (invocationList.None() || invocationList[0].Method == null)
                return objectCreateFunc();

            var uniqueKey = string.Format("{0}", invocationList[0].Method.Name);
            uniqueKey = uniqueKey.Replace("<", "@");
            uniqueKey = uniqueKey.Replace(">", "@");
            var propertyName = uniqueKey.GetPartEnclosedBy('@');
            if (!propertyName.StartsWith("get_"))
                throw new NotSupportedException("Die Function Store.PropertyCacheGet() ohne Angabe einer Property Expression darf nur aus dem Property Getter selber aus aufgerufen werden (Matthias Jenzen)");

            propertyName = propertyName.Replace("get_", "");

            return GetByKey(propertyName, objectCreateFunc);
        }

        /// <summary>
        /// Holt Daten aus dem Cache für eine Property die über "expression" definiert wird
        /// </summary>
        /// <typeparam name="TModel">Entity-Klasse der Property</typeparam>
        /// <typeparam name="T">Property Datentyp</typeparam>
        /// <param name="dummy">nur notwendig damit der Compiler unsere property expression "versteht (auch mit Intellisense)"</param>
        /// <param name="expression">Property Selektor Ausdruck</param>
        /// <param name="objectCreateFunc">Funktion zum Erzeugen der Property Daten (wird nur aufgerufen beim 1. Mal, also wenn der Cache noch leer ist)</param>
        /// <returns>Den Property Datenwert</returns>
        protected T PropertyCacheGet<TModel, T>(TModel dummy, Expression<Func<TModel, object>> expression, Func<T> objectCreateFunc)
        {
            var propertyName = expression.GetPropertyName();
            return GetByKey(propertyName, objectCreateFunc);
        }

        #endregion


        #region Clear

        protected void PropertyCacheClear<TModel>(Expression<Func<TModel, object>> expression)
        {
            var propertyName = expression.GetPropertyName();
            if (_propertyStoreBuffer.ContainsKey(propertyName))
                _propertyStoreBuffer.Remove(propertyName);
        }

        /// <summary>
        /// Leert den Cache für eine Property die über "expression" definiert wird
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dummy">nur notwendig damit der Compiler unsere property expression "versteht (auch mit Intellisense)"</param>
        /// <param name="expression">Property Selektor Ausdruck</param>
        protected void PropertyCacheClear<TModel>(TModel dummy, Expression<Func<TModel, object>> expression)
        {
            PropertyCacheClear(expression);
        }

        #endregion


        #region Set

        internal const string PropertyCacheSetConstraintMessage = "Die Function Store.PropertyCacheSet() ohne Angabe einer Property Expression darf nur aus dem Property Setter selber aus aufgerufen werden (Matthias Jenzen)";

        /// <summary>
        /// Setzt Daten im Cache für eine Property die über "expression" definiert wird
        /// </summary>
        protected void PropertyCacheSet(object value)
        {
            var stackTrace = new StackTrace(Thread.CurrentThread, true);
            var stackFrames = stackTrace.GetFrames();
            if (stackFrames == null || stackFrames.Count() < 2)
                return;

            var propertySetterMethod = stackFrames[1];

            if (propertySetterMethod == null 
                || propertySetterMethod.GetMethod() == null 
                || !propertySetterMethod.GetMethod().Name.StartsWith("set_"))
                throw new NotSupportedException(PropertyCacheSetConstraintMessage);

            var propertyName = propertySetterMethod.GetMethod().Name.Replace("set_", "");

            SetByKey(propertyName, value);
        }

        protected void PropertyCacheSet<TModel>(Expression<Func<TModel, object>> expression, object value)
        {
            var propertyName = expression.GetPropertyName();
            SetByKey(propertyName, value);
        }

        /// <summary>
        /// Setzt Daten im Cache für eine Property die über "expression" definiert wird
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dummy">nur notwendig damit der Compiler unsere property expression "versteht (auch mit Intellisense)"</param>
        /// <param name="expression">Property Selektor Ausdruck</param>
        /// <param name="value">Neuer Datenwert für den Property Cache</param>
        protected void PropertyCacheSet<TModel>(TModel dummy, Expression<Func<TModel, object>> expression, object value)
        {
            PropertyCacheSet(expression, value);
        }

        #endregion


        #region Privates

        private readonly Dictionary<object, object> _propertyStoreBuffer = new Dictionary<object, object>();

        private T GetByKey<T>(object uniqueKey, Func<T> objectCreateFunc) 
        {
            if (_propertyStoreBuffer.ContainsKey(uniqueKey))
                return (T)_propertyStoreBuffer[uniqueKey];

            var storeData = objectCreateFunc();
            _propertyStoreBuffer.Add(uniqueKey, storeData);

            return storeData;
        }

        protected T CacheGet<T>() 
        {
            return GetByKey(typeof(T).Name, () => default(T));
        }

        private void SetByKey<T>(object uniqueKey, T value)
        {
            if (!_propertyStoreBuffer.ContainsKey(uniqueKey))
            {
                _propertyStoreBuffer.Add(uniqueKey, value);
                return;
            }

            _propertyStoreBuffer[uniqueKey] = value;
        }

        protected void CacheSet<T>(T value)
        {
            SetByKey(typeof(T).Name, value);
        }

        #endregion

        #endregion


        #region IPersistableObject

        [FormPersistable]
        public bool IsSelected { get; set; }

        [FormPersistable]
        public string ObjectKey { get; set; }

        [FormPersistable]
        public string ObjectName { get; set; }

        [FormPersistable]
        public DateTime? EditDate { get; set; }

        [FormPersistable]
        public string EditUser { get; set; }

        public bool PersistablePropertiesAvailable
        {
            get
            {
                var type = GetType();

                var propertiesWithFormPersistableAttribute = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(property => property.GetCustomAttributes(typeof(FormPersistableAttribute), true).Any());

                var propertiesNotOfTypeStore = propertiesWithFormPersistableAttribute.Where(pi => pi.DeclaringType != typeof(Store));

                return propertiesNotOfTypeStore.Any();
            }
        }

        #endregion
    }
}
