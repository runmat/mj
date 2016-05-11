using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using PortalMvcTools.Web;

namespace ServicesMvc
{
    public class MvcApplication : HttpApplication
    {
        public static MvcApplication Instance { get; private set; }

        private IEnumerable<Assembly> _ckgAssemblies;
        private IEnumerable<Assembly> CkgAssemblies { get { return (_ckgAssemblies ?? (_ckgAssemblies = GetCkgAssemblies())); } }

        private IEnumerable<Assembly> GetCkgAssemblies()
        {
            yield return typeof(MvcApplication).Assembly;

            yield return Assembly.Load("CkgDomainLogic");
            yield return Assembly.Load("CkgDomainCommon");
            yield return Assembly.Load("CkgDomainCoc");
            yield return Assembly.Load("CkgDomainFahrzeug");
            yield return Assembly.Load("CkgDomainLeasing");
            yield return Assembly.Load("CkgDomainArchive");
            yield return Assembly.Load("CkgDomainFinance");
            yield return Assembly.Load("CkgDomainInsurance");
            yield return Assembly.Load("CkgDomainFahrer");
            yield return Assembly.Load("CkgDomainAutohaus");
            yield return Assembly.Load("CkgDomainInternal");
            yield return Assembly.Load("CkgDomainRemarketing");
        }

        protected void Application_Start()
        {
            Instance = this;

            // register theme supporting web form view engine
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngineThemed());

            // Register Areas (auto register)
            AreaAutoRegistration.RegisterAreasFolder(Assembly.GetAssembly(typeof(MvcApplication)));

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // needed for RangeSlider
            ModelBinders.Binders.Add(typeof(int[]), new ArrayModelBinder());

            // Customized general validation messages
            DefaultModelBinder.ResourceClassKey = "ValidationMessages";
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(LocalizedRequiredAttributeAdapter));
            ModelValidatorProviders.Providers.Add(new CustomModelValidatorsProvider());

            //
            // views and controllers provided by external assemblies:
            //
            MvcTools.MvcSettings.RegisterRoutes(RouteTable.Routes, typeof(PortalHtmlHelperExtensions).Assembly);

            //
            // Autofac / IoC Integration:
            //
            IocConfig.CreateAndRegisterIocContainerToMvc(CkgAssemblies);
            DashboardAppUrlService.RegisterAssemblies(CkgAssemblies);

            //
            // combine our appsettings in our web.config with a "parent" web.config (i. e. of a ASP.NET WebForms Application)
            //
            MvcTools.MvcSettings.MergeWebConfigAppSettings();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest()
        {
            var context = HttpContext.Current;
            var request = context.Request;
            var url = request.Url;
            var urlReferrer = request.UrlReferrer;
            if (request.HttpMethod.NotNullOrEmpty().ToUpper().Contains("POST"))
            {
                var x = urlReferrer;
            }
        }

        protected void Application_AcquireRequestState()
        {
            SetCulture();

            //RazorViewEngineThemed.EnforcePartialViewMarkerMode();
            RazorViewEngineThemed.TrySetThemeFromAppsettingsToSession();

            RazorViewEngineThemed.TrySetPartialViewMarkerModeFromRequestToSession();
            RazorViewEngineThemed.TrySetThemeFromRequestToSession();
        }

        static void SetCulture()
        {
            var sessionCulture = SessionHelper.GetSessionValue("UserCulture", "de-DE").NotNullOrEmpty();
            var culture = sessionCulture.IsNotNullOrEmpty() ? sessionCulture : ConfigurationManager.AppSettings["UserCulture"] ?? "de-DE";
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
        }

        public static IHtmlString GetRawXmlString(string xml)
        {
            return new HtmlString(xml.Replace('\n', ' '));
        }

        public static ILogonContextDataService LogonContext
        {
            get { return (ILogonContextDataService)SessionHelper.GetSessionObject("LogonContext"); }
        }

        public static bool IsActiveMenuItem(string appName)
        {
            if (LogonContext == null)
                return false;

            var httpContext = HttpContext.Current;
            if (httpContext == null)
                return false;

            var contextUrl = httpContext.Request.RawUrl.ToLower();
            if (contextUrl.NotNullOrEmpty().ToLower().EndsWith(appName.NotNullOrEmpty().ToLower()))
                return true;

            var allMenuItems = LogonContext.GetMenuItems();
            var currentMenuItem = allMenuItems.FirstOrDefault(item => item.AppName == appName);
            if (currentMenuItem == null)
                return false;

            var appUrl = currentMenuItem.AppURL.ToLower();

            var isActive = contextUrl.Contains(appUrl);
            return isActive;
        }

        public static bool IsActiveMenuGroup(string appType)
        {
            if (LogonContext == null)
                return false;

            var httpContext = HttpContext.Current;
            if (httpContext == null)
                return false;

            var contextUrl = httpContext.Request.RawUrl.ToLower();

            var allMenuItems = LogonContext.GetMenuItems();
            var menuItemsOfThisGroup = allMenuItems.Where(item => item.AppType == appType);
            if (menuItemsOfThisGroup.None())
                return false;

            var isActive = menuItemsOfThisGroup.Any(item => contextUrl.Contains(item.AppURL.ToLower()));
            return isActive;
        }

        public static string GetActiveMenuItemCssClass(string appName = null)
        {
            if (appName.IsNullOrEmpty())
            {
                appName = "/ServicesMvc/";
                if (LogonContext.Customer != null && LogonContext.Customer.MvcSelectionUrl.IsNotNullOrEmpty())
                    appName += LogonContext.Customer.MvcSelectionUrl.Replace("~/","");
            }

            return IsActiveMenuItem(appName) ? "active" : "";
        }

        public static string GetActiveMenuGroupCssClass(string appName)
        {
            return IsActiveMenuGroup(appName) ? "active" : "";
        }

        //
        // Application_Error is obsolete now, because we use "MvcTools.Web.CustomHandleErrorAttribute" "registered in App_Start.FilterConfig"
        // Application_Error nutzen um die Ausnahmen abzufangen die sich ausßerhalb der MVC Pipeline ereignen.  MVC Ausnahmen werden in der CustomHandleErrorAttribute abgefangen
        //
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var dataContext = SessionStore.GetCurrentDataContext();
            var logonContext = SessionStore.GetCurrentLogonContext();

            if (exception.Message.NotNullOrEmpty().Contains("__browserLink/requestData"))
                // ignore this error
                return;

            this.HandleError();

            var logService = new LogService(string.Empty, string.Empty);
            logService.LogElmahError(exception, logonContext, dataContext);
        }
    }
}

