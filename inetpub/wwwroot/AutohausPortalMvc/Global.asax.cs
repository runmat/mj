using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutohausPortalMvc.App_Start;
using AutohausPortalMvc.Services;
using GeneralTools.Contracts;
using GeneralTools.Services;
using MvcTools.Web;
using PortalMvcTools.Services;
using PortalMvcTools.Web;

namespace AutohausPortalMvc
{
    // Hinweis: Anweisungen zum Aktivieren des klassischen Modus von IIS6 oder IIS7 
    // finden Sie unter "http://go.microsoft.com/?LinkId=9394801".

    public class MvcApplication : HttpApplication
    {
        public static MvcApplication Instance { get; private set; }

        private IAppSettings _appSettings;
        public IAppSettings AppSettings
        {
            get { return (_appSettings ?? (_appSettings = new AppSettings())); }
        }

        private ILogService _logService;
        public ILogService LogService
        {
            get { return (_logService ?? (_logService = new LogService(AppSettings.AppName, Path.Combine(AppSettings.DataPath, "log.xml")))); }
        }

        public static string GoogleMapsScriptUrl
        {
            get { return string.Format("{0}://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false", HttpContext.Current.Request.Url.Scheme); }
        }


        protected void Application_Start()
        {
            Instance = this;

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // needed for RangeSlider
            ModelBinders.Binders.Add(typeof(int[]), new ArrayModelBinder());

            // Customized general validation messages
            DefaultModelBinder.ResourceClassKey = "ValidationMessages";
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(LocalizedRequiredAttributeAdapter));

            //
            // views and controllers provided by external assemblies:
            //
            //MvcTools.MvcSettings.RegisterRoutes(RouteTable.Routes, typeof(HtmlHelperExtensions).Assembly);
            MvcTools.MvcSettings.RegisterRoutes(RouteTable.Routes, typeof(PortalHtmlHelperExtensions).Assembly);

            // Autofac / IoC Integration:
            IocConfig.RegisterIocContainer();

            //
            // combine our appsettings in our web.config with a "parent" web.config (i. e. of a ASP.NET WebForms Application)
            //
            MvcTools.MvcSettings.MergeWebConfigAppSettings();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //
            // Connecting our Logger to our SapDataService
            //
            S.AP.GetLogService = () => LogService;

            //
            // SAP to Web Model mapping validation:
            // validate model mappings between our de-coupled SAP and Web Models
            //
            //new AppModelMappings().ValidateAndRaiseError();
        }

        protected void Application_BeginRequest()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");

            this.SetCacheOff();
        }


        //
        // Application_Error is obsolete now, because we use "MvcTools.Web.CustomHandleErrorAttribute" "registered in App_Start.FilterConfig"
        //
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    var exception = Server.GetLastError();
        //    var logonContext = SessionStore.GetCurrentLogonContext();
        //    var dataContext = SessionStore.GetCurrentDataContext();

        //    this.HandleError();

        //    LogService.LogError(exception, logonContext, dataContext);
        //}
    }
}