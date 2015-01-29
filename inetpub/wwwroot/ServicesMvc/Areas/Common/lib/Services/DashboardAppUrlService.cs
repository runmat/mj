using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using ServicesMvc;

namespace CkgDomainLogic.General.Services
{
    public class DashboardAppUrlService
    {
        public static DashboardItemsPackage InvokeViewModelForAppUrl(string appUrl, string key, IContainer iocContainer = null)
        {
            string area, controller, action;
            GetAppUrlParts(appUrl, out area, out controller, out action);

            var servicesMvcAssembly = Assembly.GetAssembly(typeof (MvcApplication));

            var controllerType = GetControllerType(area, controller, servicesMvcAssembly);
            if (controllerType == null)
                return null;

            var ctor = controllerType.GetConstructors().First();
            var controllerObject = ctor.Invoke(ctor.GetParameters().Select(p => GetInstanceOf(p.ParameterType, iocContainer)).ToArray());

            var piDashboardProviderViewModel = controllerType.GetPropertyOfClassWithAttribute(typeof (DashboardProviderViewModelAttribute));
            if (piDashboardProviderViewModel == null)
                return null;

            var vm = piDashboardProviderViewModel.GetValue(controllerObject, null);
            if (vm == null)
                return null;

            return GetItems(vm, key);
        }

        private static DashboardItemsPackage GetItems(object vm, string key)
        {
            var vmType = vm.GetType();

            var piDashboardItemsLoadMethod = vmType.GetMethodWithAttribute<DashboardItemsLoadMethodAttribute>(attr => attr.Key == key);
            if (piDashboardItemsLoadMethod == null)
                return null;

            var data = (DashboardItemsPackage)piDashboardItemsLoadMethod.Invoke(vm, piDashboardItemsLoadMethod.GetParameters().Select(p => (object)null).ToArray());
            return data;
        }

        private static object GetInstanceOf(Type type, IContainer iocContainer)
        {
            if (iocContainer != null)
                // use custom resolver
                return iocContainer.Resolve(type);

            // use MVC resolver
            return DependencyResolver.Current.GetService(type);
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
