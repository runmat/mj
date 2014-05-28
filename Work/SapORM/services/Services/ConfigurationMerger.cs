using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace SapORM.Services
{
    public class ConfigurationMerger
    {
        static public void MergeTestWebConfigAppSettings(string resourcePath = "SapORM.Services.Resources.Web.config", Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            //var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null) return;

            var webConfig = new XmlDocument();
            webConfig.Load(stream);
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
