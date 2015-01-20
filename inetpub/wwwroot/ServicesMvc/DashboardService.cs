using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using ServicesMvc;

namespace CkgDomainLogic.Services
{
    public class DashboardService
    {
        public static void InvokeViewModelForAppUrl(string appUrl, IContainer iocContainer = null)
        {
            string area, controller, action;
            GetAppUrlParts(appUrl, out area, out controller, out action);

            var servicesMvcAssembly = Assembly.GetAssembly(typeof (MvcApplication));

            var controllerType = GetControllerType(area, controller, servicesMvcAssembly);
            if (controllerType == null)
                return;

            var ctor = controllerType.GetConstructors().First();
            var controllerObject = ctor.Invoke(ctor.GetParameters().Select(p => GetInstanceOf(p.ParameterType, iocContainer)).ToArray());

            var piDashboardProviderViewModel = controllerType.GetPropertyOfClassWithAttribute(typeof (DashboardProviderViewModelAttribute));
            if (piDashboardProviderViewModel == null)
                return;

            var vm = piDashboardProviderViewModel.GetValue(controllerObject, null);
            if (vm == null)
                return;

            //var vieModelProperty = controllerType.GetPropertiesForAttribute()
            Do(vm);
        }

        private static void Do(object vm)
        {
            var vmType = vm.GetType();

            var piDashboardItemSelector = vmType.GetPropertyWithAttribute(typeof(DashboardItemSelectorAttribute));
            if (piDashboardItemSelector == null)
                return;

            var piDashboardItems = vmType.GetPropertyWithAttribute(typeof(DashboardItemsAttribute));
            if (piDashboardItems == null)
                return;

            var piDashboardItemsLoadMethod = vmType.GetMethodWithAttribute(typeof(DashboardItemsLoadMethod));
            if (piDashboardItemsLoadMethod == null)
                return;

            var selector = new ZulassungsReportSelektor
                {
                    ZulassungsDatumRange = new DateRange
                        {
                            IsSelected = true,
                            StartDate = DateTime.Today.AddMonths(-4),
                            EndDate = DateTime.Today.AddMonths(-2),
                        }
                };
            piDashboardItemSelector.SetValue(vm, selector, null);

            piDashboardItemsLoadMethod.Invoke(vm, new object[] { null });
            var items = piDashboardItems.GetValue(vm, null);
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
