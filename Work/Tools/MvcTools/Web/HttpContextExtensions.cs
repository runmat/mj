using System;
using System.Web;

namespace MvcTools.Web
{
    public static class HttpContextExtensions
    {
        public static string GetRequestString(this HttpContext context, string key, string defaultValue)
        {
            if (context.Request[key] == null)
                return defaultValue;
            if (context.Request[key].ToString() == "")
                return defaultValue;

            return context.Request[key].ToString();
        }

        public static string GetRequestString(this HttpContext context, string key)
        {
            return context.GetRequestString(key, "");
        }

        public static int GetRequestInt(this HttpContext context, string key, int defaultValue)
        {
            return Int32.Parse(context.GetRequestString(key, defaultValue.ToString()));
        }

        public static int GetRequestInt(this HttpContext context, string key)
        {
            return context.GetRequestInt(key, 0);
        }
    }

    public static class ServerExtensions
    {
        public static string MapToUrl(this HttpServerUtilityBase server, string path)
        {
            var appPath = server.MapPath("/").ToLower();
            return string.Format("/{0}", path.ToLower().Replace(appPath, "").Replace(@"\", "/"));
        }
    }
}
