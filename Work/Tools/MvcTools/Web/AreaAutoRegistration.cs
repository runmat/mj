using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTools.Web
{
    public class AreaAutoRegistration
    {
        internal static void CreateContextAndRegister(Assembly assembly, string areaName)
        {
            var context = new AreaRegistrationContext(areaName, RouteTable.Routes, null);

            var thisNamespace = assembly.GetName().Name;
            if (thisNamespace != null)
                context.Namespaces.Add(thisNamespace + ".*");

            // register area:
            context.MapRoute(
                areaName + "_default",
                areaName + "/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }, namespaces: new[] { thisNamespace + "." + areaName + ".Controllers" }
            );
        }

        public static void RegisterAreasFolder(Assembly assembly)
        {
            var rootPath = HttpContext.Current.Server.MapPath("~/");
            var areaPath = Path.Combine(rootPath, "Areas"); 

            foreach (var directory in Directory.GetDirectories(areaPath)) 
                CreateContextAndRegister(assembly, Path.GetFileName(directory));
        }
    }
}
