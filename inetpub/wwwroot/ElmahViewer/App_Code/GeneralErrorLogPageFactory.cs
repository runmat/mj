using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using Elmah;

namespace Elmahreader.App_Code
{
    public class GeneralErrorLogPageFactory : ErrorLogPageFactory
    {
        public override IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            const string testserver = "vms026";
            const string localhost = "localhost";
            string host = context.Request.Url.Host.ToUpper();
            var legal = new List<string> {testserver.ToUpper(), localhost.ToUpper()};

            var isNotLegalRequest = !legal.Any(host.Contains);


            if (isNotLegalRequest)
            {
                throw new Exception("Illegal Access");
            }

            var appname = context.Request.Params["app"];
            if (String.IsNullOrEmpty(appname))
            {
                // appname was not found - let's check the referrer
                if (context.Request.UrlReferrer != null && !"/stylesheet".Equals(context.Request.PathInfo, StringComparison.OrdinalIgnoreCase))
                {
                    appname = HttpUtility.ParseQueryString(context.Request.UrlReferrer.Query)["app"];
                    context.Response.Redirect(String.Format("{0}{1}app={2}", context.Request.RawUrl, context.Request.Url.Query.Length > 0 ? "&" : "?", appname));
                }
            }

            IHttpHandler handler = base.GetHandler(context, requestType, url, pathTranslated);
            var err = new MySqlErrorLog(WebConfigurationManager.ConnectionStrings["MySqlTraceConnString"].ConnectionString);
            err.ApplicationName = appname ?? "Not found";

            // we need to fool elmah completely
            object v = typeof(ErrorLog).GetField("_contextKey", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            context.Items[v] = err;

            return handler;
        }
    }
}