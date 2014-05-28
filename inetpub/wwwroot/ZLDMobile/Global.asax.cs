using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using MvcTools.Data;
using ZLDMobile.Services;
using MvcTools.Web;
using System.Threading;
using System.Configuration;

namespace ZLDMobile
{
    // Hinweis: Anweisungen zum Aktivieren des klassischen Modus von IIS6 oder IIS7 
    // finden Sie unter "http://go.microsoft.com/?LinkId=9394801".

    public class MvcApplication : System.Web.HttpApplication
    {
        private static IAppSettings _appSettings;
        public static IAppSettings AppSettings { get { return (_appSettings ?? (_appSettings = new AppSettings { AppCopyRight = "© " + DateTime.Now.Year + " Christoph Kroschke GmbH" })); } }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Precompiled Views mit JS-Hilfsfunktionen einbinden
            MvcTools.MvcSettings.RegisterRoutes(RouteTable.Routes, typeof (HtmlHelperExtensions).Assembly);
        }

        protected void Application_BeginRequest()
        {
            System.Diagnostics.Debug.WriteLine(HttpContext.Current.Request.ContentLength);
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");

            this.SetCacheOff();
        }

        protected void Application_Error()
        {
            Exception ex;

            Exception lastError = Server.GetLastError();

            if (lastError == null)
            {
                return;
            }

            if (lastError.InnerException == null)
            {
                ex = lastError;
            }
            else
            {
                ex = lastError.GetBaseException();
            }

            ErrorLogging.WriteErrorToLogFile(ex);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Dummy-Zugriff auf die Session, um Probleme mit dem Session-State zu vermeiden
            string sessionId = Session.SessionID;
        }
    }
}