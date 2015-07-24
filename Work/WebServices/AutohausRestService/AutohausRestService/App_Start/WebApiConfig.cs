using System.Web.Http;
using AutohausRestService.Models;
using AutohausRestService.Services;

namespace AutohausRestService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MessageHandlers.Add(new RestLoggingHandler());

            config.Filters.Add(new RestExceptionFilterAttribute());
        }
    }
}
