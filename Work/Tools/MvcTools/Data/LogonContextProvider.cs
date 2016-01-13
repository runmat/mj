using System;
using System.Web;
using System.Web.SessionState;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace MvcTools.Data
{
    public class LogonContextProvider : ILogonContextProvider
    {
        public ILogonContext GetLogonContext()
        {
            return GetSessionObject<ILogonContext>("LogonContext");
        }

        public T GetSessionObject<T>(string key, Func<T> createFunc = null) where T : class
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

        private static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }
    }
}
