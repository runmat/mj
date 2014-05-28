using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcTools.Controllers;

namespace MvcTools.Web
{
    public static class HttpApplicationExtensions
    {
        static public void HandleError(this HttpApplication app)
        {
            var lastError = app.Server.GetLastError();
            app.Server.ClearError();

            int statusCode;
            if (lastError.GetType() == typeof(HttpException))
            {
                statusCode = ((HttpException)lastError).GetHttpCode();
            }
            else
            {
                // Not an HTTP related error so this is a problem in our code, set status to
                // 500 (internal server error)
                statusCode = 500;
            }

            var originRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
            var originControllerName = "";
            if (originRouteData != null)
                originControllerName = originRouteData.Values["controller"].ToString();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Index");
            routeData.Values.Add("statusCode", statusCode);
            routeData.Values.Add("exception", lastError);
            routeData.Values.Add("originControllerName", originControllerName);

            IController controller = new ErrorController();

            var requestContext = new RequestContext(new HttpContextWrapper(app.Context), routeData);

            // MJE, important for IIS 7.5
            app.Response.TrySkipIisCustomErrors = true;

            controller.Execute(requestContext);
        }

        static public void SetCacheOff(this HttpApplication app)
        {
            var url = HttpContext.Current.Request.Url.ToString().ToLower();
            if (url.Contains(".woff"))
                return;

            //NOTE: Stopping IE from being a caching whore         
            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now);
            HttpContext.Current.Response.Cache.SetValidUntilExpires(true);
        }
    }
}
