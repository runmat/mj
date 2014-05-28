using System;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace MvcTools.Web
{
    public class SessionStore
    {
        public static void SetModel(string key, object value)
        {
            SessionHelper.SetSessionValue(key, value);
        }

        public static object GetModel(string key, Func<object> createFunc)
        {
            return SessionHelper.GetSessionObject(key, createFunc);
        }

        public static ILogonContext GetCurrentLogonContext()
        {
            return (ILogonContext)SessionHelper.GetSessionObject("LogonContext");
        }

        
        #region Session Global Current DataContext 

        public static void SetCurrentDataContextKey(string dataContextKey)
        {
            SessionHelper.SetSessionValue("CurrentDataContextKey", dataContextKey);
        }

        public static string GetCurrentDataContextKey()
        {
            return (string)SessionHelper.GetSessionObject("CurrentDataContextKey");
        }

        public static object GetCurrentDataContext()
        {
            var currentDataContextKey = GetCurrentDataContextKey();
            if (currentDataContextKey.IsNotNullOrEmpty())
                return SessionHelper.GetSessionObject(currentDataContextKey);

            return null;
        }

        #endregion
    }

    public class SessionStore<T> where T : class
    {
        private static string Key { get { return typeof(T).Name; } }

        public static T Model
        {
            get { return (T)SessionHelper.GetSessionObject(Key); }
            set { SessionHelper.SetSessionValue(Key, value); }
        }

        public static T GetModel(Func<T> createFunc)
        {
            return SessionHelper.GetSessionObject(Key, createFunc);
        }
    }

    public class SessionStoreAutoCreate<T> where T : class, new()
    {
        public static string Key { get { return typeof(T).Name; } }

        public static T Model
        {
            get { return SessionHelper.GetSessionObject(Key, () => new T()); }
            set { SessionHelper.SetSessionValue(Key, value); }
        }

        public static T GetModel(Func<T> createFunc)
        {
            return SessionHelper.GetSessionObject(Key, createFunc);
        }

        public static void SetCurrentDataContextKey()
        {
            SessionStore.SetCurrentDataContextKey(Key);
        }
    }
}