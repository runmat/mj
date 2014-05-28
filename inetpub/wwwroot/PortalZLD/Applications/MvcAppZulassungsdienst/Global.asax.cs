using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml;
using BoC.Web.Mvc.PrecompiledViews;
using MvcTools;
using MvcTools.Web;

namespace MvcAppZulassungsdienst
{
    // Hinweis: Anweisungen zum Aktivieren des klassischen Modus von IIS6 oder IIS7 
    // finden Sie unter "http://go.microsoft.com/?LinkId=9394801".

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MvcSettings.RegisterRoutes(RouteTable.Routes);
            ApplicationPartRegistry.Register(typeof(HtmlHelperExtensions).Assembly);

            MergeWebConfigAppSettings();
        }

        /// <summary>
        /// MJE, Important to avoid confusing output because of browser chaching:
        /// </summary>
        protected void Application_BeginRequest()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
        }

        static public void MergeWebConfigAppSettings()
        {
            var appRootFolder = HttpContext.Current.Server.MapPath("~/");

            var parentWebConfigFolder = appRootFolder;
            bool parentWebConfigExistsInFolder;
            do
            {
                parentWebConfigFolder += @"..\";
                parentWebConfigExistsInFolder = (null != Directory.GetFiles(parentWebConfigFolder).FirstOrDefault(f => f.ToLower().Contains("web.config")));
            } while (!parentWebConfigExistsInFolder);

            var parentWebConfigFilePath = Path.Combine(parentWebConfigFolder, "web.config");
            var webConfig = new XmlDocument();
            webConfig.Load(parentWebConfigFilePath);
            var xmlRoot = webConfig.DocumentElement;
            if (xmlRoot == null) return;

            var parentXmlAppSetings =
                xmlRoot.ChildNodes.OfType<XmlNode>().FirstOrDefault(n => n.Name.ToLower() == "appsettings");
            if (parentXmlAppSetings == null) return;
            var appSettings = parentXmlAppSetings.ChildNodes.OfType<XmlNode>().ToList();

            appSettings.ForEach(s =>
                                    {
                                        if (s.Attributes != null && s.Attributes["key"] != null)
                                            ConfigurationManager.AppSettings.Set(s.Attributes["key"].InnerText, s.Attributes["value"].InnerText);
                                    });
        }
    }
}