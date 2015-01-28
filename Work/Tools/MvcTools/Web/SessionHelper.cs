using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using GeneralTools.Models;

namespace MvcTools.Web
{
    public static class SessionHelper
    {
        private static HttpContext HttpContext
        {
            get { return HttpContext.Current; }
        }

        private static HttpSessionState Session
        {
            get { return HttpContext == null ? null : HttpContext.Session; }
        }


        public static void ClearSessionObject(string key)
        {
            if (Session == null)
                return;

            Session[key] = null;
        }

        public static void SetSessionValue<T>(string key, T model)
        {
            if (Session == null)
                return;

            Session[key] = model;
        }

        public static void SetSessionObject(string key, object model)
        {
            if (Session == null)
                return;

            Session[key] = model;
        }

        public static T GetSessionValue<T>(string key, T defaultVal)
        {
            if (Session == null)
                return defaultVal;

            if (key.IsNullOrEmpty())
                return defaultVal;

            T result;
            if (Session[key] != null)
                result = (T) Session[key];
            else
                // Session store for value type objects
                Session[key] = result = defaultVal;

            return result;
        }

        public static T GetSessionObject<T>(string key, Func<T> createFunc = null) where T : class
        {
            if (Session == null)
                return null;

            if (key.IsNullOrEmpty())
                return null;

            if (createFunc == null)
                createFunc = () => null;

            T result;
            if (Session[key] != null)
                result = (T)Session[key];
            else
                // Session store for reference type objects
                Session[key] = result = createFunc();

            return result;
        }

        public static object GetSessionObject(string key, Func<object> createFunc = null)
        {
            if (Session == null)
                return null;

            if (key.IsNullOrEmpty())
                return null;

            if (createFunc == null)
                createFunc = () => null;

            object result;
            if (Session[key] != null)
                result = Session[key];
            else
                // Session store for reference type objects
                Session[key] = result = createFunc();

            return result;
        }

        public static List<T> GetSessionList<T>(string key, Func<List<T>> createFunc)
        {
            return GetSessionObject(key, createFunc);
        }

        public static string GetSessionString(string key)
        {
            return (string)GetSessionObject(key);
        }
    }
}