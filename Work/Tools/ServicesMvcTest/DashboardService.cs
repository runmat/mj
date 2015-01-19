using System;
using System.Linq;
using System.Reflection;
using Autofac;
using GeneralTools.Models;
using ServicesMvc;

namespace CkgDomainLogic.Services
{
    public class DashboardService
    {
        public static void InvokeViewModelForAppUrl(string appUrl, IContainer iocContainer)
        {
            string area, controller, action;
            GetAppUrlParts(appUrl, out area, out controller, out action);

            var servicesMvcAssembly = Assembly.GetAssembly(typeof (MvcApplication));

            var controllerType = GetControllerType(area, controller, servicesMvcAssembly);
            if (controllerType == null)
                return;

            var ctor = controllerType.GetConstructors().First();
            var controllerObject = ctor.Invoke(ctor.GetParameters().Select(p =>
                {
                    var xxx = iocContainer.Resolve(p.ParameterType);
                    return xxx;
                }).ToArray());
        }

        private static Type GetControllerType(string area, string controller, Assembly servicesMvcAssembly)
        {
            var controllerTypeName = string.Format("servicesmvc.{0}controllers.{1}controller", (area.IsNullOrEmpty() ? "" : area + "."), controller);
            var controllerType = servicesMvcAssembly.GetType(controllerTypeName, throwOnError: false, ignoreCase: true);

            return controllerType;
        }

        private static string GetAppUrlParts(string appUrl, out string area, out string controller, out string action)
        {
            appUrl = appUrl.NotNullOrEmpty().ToLower();
            if (appUrl.StartsWith("mvc/"))
                appUrl = appUrl.SubstringTry(4);

            var urlParts = appUrl.Split('/');
            area = "";
            var index = -1;
            if (urlParts.Count() > 1)
                area = urlParts[++index];
            controller = urlParts[++index];
            action = urlParts[++index];

            return appUrl;
        }
    }
}
