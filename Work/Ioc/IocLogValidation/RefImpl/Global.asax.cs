using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using RefImpl.App_Start;
using RefImpl.Helpers;

namespace RefImpl
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ModelMetadataProviders.Current = new AnnotationsAndConventionsBasedModelMetaDataProvider();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            IocConfig.RegisterIocContainer();
            var logger = LogManager.GetLogger("Global");
            logger.Trace("Application_Start");

        }
    }
}