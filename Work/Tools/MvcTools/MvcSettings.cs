using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using BoC.Web.Mvc.PrecompiledViews;
using GeneralTools.Services;
using GeneralTools.Models;
using WebTools.Services;

namespace MvcTools
{
    public class MvcSettings
    {
        private static readonly object LockObj = new Object();

        public static void RegisterRoutes(RouteCollection routes, params Assembly[] precompiledAssemblies)
        {
            routes.MapRoute("HelpControl", "HelpControl/{action}/{id}",
                            new { controller = "HelpControl", action = "", id = UrlParameter.Optional });
            routes.MapRoute("Shared", "Shared/{action}/{id}",
                            new { controller = "Shared", action = "", id = UrlParameter.Optional });

            ApplicationPartRegistry.Register(typeof(MvcSettings).Assembly);

            if (precompiledAssemblies != null && precompiledAssemblies.Length > 0)
                precompiledAssemblies.ToList().ForEach(ApplicationPartRegistry.Register);
        }

        public static void MergeWebConfigAppSettings()
        {
            var appRootFolder = HttpContext.Current.Server.MapPath("~/");
            var parentWebConfigFolder = appRootFolder;

            var pathRoot = HttpContext.Current.Server.MapPath("/");
            var pathRootApp = HttpContext.Current.Server.MapPath("~/");
            var pathRelApp = pathRootApp.ToLower().Replace(pathRoot.ToLower(), "");
            var rawUrlFolders = pathRelApp.Split('\\');

            var parentWebConfigExistsInFolder = false;

            if (rawUrlFolders.Length > 0)
            {
                //
                // Variante 1: 
                //   Suche solange in den Parent-Foldern bis eine "nicht MVC" Variante als Sub-Folder erkannt wird
                //   Beispiel: 
                //       Quelle: c:\inetpub\wwwroot\irgendwas\nochirgendwas\[ServicesMvc]
                //       Ziel:   c:\inetpub\wwwroot\[Services]
                //

                var appNameMvc = rawUrlFolders[0].ToLower();
                if (appNameMvc.IsNullOrEmpty() || !appNameMvc.EndsWith("mvc"))
                    return;
                var appNameWebForms = appNameMvc.Replace("mvc", "");


                var count = 0;
                do
                {
                    parentWebConfigFolder += @"..\";
                    parentWebConfigExistsInFolder = (null != Directory.GetDirectories(parentWebConfigFolder).FirstOrDefault(f => f.ToLower().EndsWith(appNameWebForms)));
                    if (count++ > 20)
                        break;

                } while (!parentWebConfigExistsInFolder);

                if (parentWebConfigExistsInFolder)
                    parentWebConfigFolder += appNameWebForms;
            }

            if (!parentWebConfigExistsInFolder)
            {
                //
                // Variante 2: 
                //   Suche solange in den Parent-Foldern bis dort eine beliebige Web.Config erkannt wird
                //
                var count = 0;
                do
                {
                    parentWebConfigFolder += @"..\";
                    parentWebConfigExistsInFolder = (null != Directory.GetFiles(parentWebConfigFolder).FirstOrDefault(f => f.ToLower().Contains("web.config")));
                    if (count++ > 20)
                        return;
                } while (!parentWebConfigExistsInFolder);
            }

            var parentWebConfigFilePath = Path.Combine(parentWebConfigFolder, "web.config");
            var webConfig = new XmlDocument();
            webConfig.Load(parentWebConfigFilePath);
            var xmlRoot = webConfig.DocumentElement;
            if (xmlRoot == null) return;

            var parentXmlAppSettings =
                xmlRoot.ChildNodes.OfType<XmlNode>().FirstOrDefault(n => n.Name.ToLower() == "appsettings");
            if (parentXmlAppSettings == null) return;
            var appSettings = parentXmlAppSettings.ChildNodes.OfType<XmlNode>().ToList();

            try
            {
                if (appSettings.Count == 1)
                {
                    // genau 1 Eintrag in den AppSettings 
                    // ==> Achtung "Verschlüsselung": 
                    //      Wir gehen davon aus, dass die AppSettings zu genau einem Eintrag verschlüsselt sind:
                    //      ==> Wir meiden die verschlüsselte Web.Config und verwenden stattdessen unsere eigene Config "App_Data\AppSettings.config"
                    var fileName = Path.Combine(parentWebConfigFolder, "App_Data", "AppSettings.config");
                    if (File.Exists(fileName))
                    {
                        var xmlDict = XmlService.XmlDeserializeFromFile<XmlDictionary<string, string>>(fileName);
                        xmlDict.ToList().ForEach(xmlEntry =>
                        {
                            var key = CryptoMd5.Decrypt(xmlEntry.Key);
                            var val = CryptoMd5.Decrypt(xmlEntry.Value);
                            TryThreadSaveSetAppSettingsKey(key, val);
                        });
                    }
                }
                else
                    appSettings.ForEach(s =>
                    {
                        if (s.Attributes == null || s.Attributes["key"] == null)
                            return;

                        var key = s.Attributes["key"].InnerText;
                        TryThreadSaveSetAppSettingsKey(key, s.Attributes["value"].InnerText);
                    });
            }
            catch
            {
                // empty catch ist ok here, weil wir nicht deterministischen Fehler abfangen wollen ("Key already exists...")
            }
        }

        private static void TryThreadSaveSetAppSettingsKey(string key, string value)
        {
            if (ConfigurationManager.AppSettings.Get(key) == null)
            {
                lock (LockObj)
                {
                    if (ConfigurationManager.AppSettings.Get(key) == null)
                        ConfigurationManager.AppSettings.Set(key, value);
                }
            }
        }
    }
}
