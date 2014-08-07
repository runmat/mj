using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using PortalMvcTools.Web;
using ServicesMvc.App_Start;

namespace ServicesMvc
{
    // Hinweis: Anweisungen zum Aktivieren des klassischen Modus von IIS6 oder IIS7 
    // finden Sie unter "http://go.microsoft.com/?LinkId=9394801".

    public class MvcApplication : HttpApplication
    {
        public static MvcApplication Instance { get; private set; }

        //private IAppSettings _appSettings;
        //public IAppSettings AppSettings { get { return (_appSettings ?? (_appSettings = CkgDomainAppSettings.DefaultInstanceDAD)); } }

        //private ILogService _logService;
        //public ILogService LogService
        //{
        //    get { return (_logService ?? (_logService = new LogService(AppSettings.AppName, Path.Combine(AppSettings.DataPath, "log.xml")))); }
        //}


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


            //throw new DirectoryNotFoundException("Test: Application_Start");

            //
            // views and controllers provided by external assemblies:
            //
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
            // S.AP.GetLogService = () => LogService;

            //
            // DB Tier to Middle Tier Model mapping validation:
            // validate model mappings between our de-coupled SAP and Web Models
            //
            new CkgDomainLogic.General.Models.AppModelMappings().ValidateAndRaiseError();
            new CkgDomainLogic.CoC.Models.AppModelMappings().ValidateAndRaiseError();
            new CkgDomainLogic.Equi.Models.AppModelMappings().ValidateAndRaiseError();
            new CkgDomainLogic.Fahrzeuge.Models.AppModelMappings().ValidateAndRaiseError();
            new CkgDomainLogic.Strafzettel.Models.AppModelMappings().ValidateAndRaiseError();
            new CkgDomainLogic.Leasing.Models.AppModelMappings().ValidateAndRaiseError();
            new CkgDomainLogic.Ueberfuehrung.Models.AppModelMappings().ValidateAndRaiseError();
            new CkgDomainLogic.Uebfuehrg.Models.AppModelMappings().ValidateAndRaiseError();
            new CkgDomainLogic.Zulassung.MobileErfassung.Models.AppModelMappings().ValidateAndRaiseError();
            new CkgDomainLogic.Finance.Models.AppModelMappings().ValidateAndRaiseError();
            new CkgDomainLogic.Fahrer.Models.AppModelMappings().ValidateAndRaiseError();
            //new CkgDomainLogic.DomainCommon.Models.AppModelMappings().ValidateAndRaiseError();
        }

        protected void Application_BeginRequest()
        {
            this.SetCacheOff();
        }

        protected void Application_AcquireRequestState()
        {
            SetCulture();
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

            this.HandleError();

            var logService = new LogService(string.Empty, string.Empty);
            logService.LogElmahError(exception, logonContext, dataContext);
        }
    }
}

