using System;
using System.Collections.Generic;
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
        public static IEnumerable<Assembly> Assemblies { get; set; }

        public static void RegisterAssemblies(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies;
        }

        public static ChartItemsPackage InvokeViewModelForAppUrl(string appUrl, string key)
        {
            string area, controller, action;
            GetAppUrlParts(appUrl, out area, out controller, out action);

            var controllerType = GetControllerType(area, controller);
            if (controllerType == null)
                return null;

            var ctor = controllerType.GetConstructors().First();
            var controllerObject = ctor.Invoke(ctor.GetParameters().Select(p => GetInstanceOf(p.ParameterType)).ToArray());

            var piDashboardProviderViewModel = controllerType.GetPropertyOfClassWithAttribute(typeof (DashboardProviderViewModelAttribute));
            if (piDashboardProviderViewModel == null)
                return null;

            var vm = piDashboardProviderViewModel.GetValue(controllerObject, null);
            if (vm == null)
                return null;

            return GetItems(vm, key);
        }

        private static ChartItemsPackage GetItems(object vm, string key)
        {
            var vmType = vm.GetType();

            var piDashboardItemsLoadMethod = vmType.GetMethodWithAttribute<DashboardItemsLoadMethodAttribute>(attr => attr.Key == key);
            if (piDashboardItemsLoadMethod == null)
                return null;

            var data = (ChartItemsPackage)piDashboardItemsLoadMethod.Invoke(vm, piDashboardItemsLoadMethod.GetParameters().Select(p => (object)null).ToArray());
            return data;
        }

        private static object GetInstanceOf(Type type)
        {
            return DependencyResolver.Current.GetService(type);
        }

        private static Type GetControllerType(string area, string controller)
        {
            if (Assemblies == null)
                return null;

            Type controllerType = null;
            foreach (var assembly in Assemblies)
            {
                var controllerTypeName = string.Format("servicesmvc.{0}controllers.{1}controller", (area.IsNullOrEmpty() ? "" : area + "."), controller);
                controllerType = assembly.GetType(controllerTypeName, throwOnError: false, ignoreCase: true);
                if (controllerType != null)
                    break;
            }

            return controllerType;
        }

        private static void GetAppUrlParts(string appUrl, out string area, out string controller, out string action)
        {
            appUrl = appUrl.NotNullOrEmpty().ToLower();
            if (appUrl.StartsWith("mvc/"))
                appUrl = appUrl.SubstringTry(4);

            var urlParts = appUrl.Split('/');
            area = "";
            var index = -1;
            if (urlParts.Count() > 2)
                area = urlParts[++index];
            controller = urlParts[++index];
            action = urlParts[++index];
        }
    }
}
