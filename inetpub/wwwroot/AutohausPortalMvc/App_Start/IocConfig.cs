using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutohausPortalMvc.Services;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Services;
using CkgDomainLogic.Ueberfuehrung.Contracts;
using CkgDomainLogic.Ueberfuehrung.Services;
using CkgDomainLogic.Feinstaub.Contracts;
using CkgDomainLogic.Feinstaub.Services;
using GeneralTools.Contracts;
using GeneralTools.Services;
using MvcTools.Web;
using PortalMvcTools.Services;
using WebTools.Services;

namespace AutohausPortalMvc.App_Start
{
    public static class IocConfig
    {
        public static void RegisterIocContainer()
        {
            // IoC container starten
            var builder = new ContainerBuilder();

            builder.Register(c => S.AP).InstancePerHttpRequest(); 


            // container soll die Controller ermitteln für die Runtime
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterControllers(Assembly.Load("CkgDomainLogic"));
            builder.RegisterControllers(Assembly.Load("CkgDomainCommon"));
            builder.RegisterControllers(Assembly.Load("CkgDomainAutohaus"));

            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterModule(new AutofacWebTypesModule());

            // Eigene Interfaces und deren Implementierungen registrieren
            builder.RegisterIocInterfacesAndTypes();

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            // container an MVC übergeben
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static void RegisterIocInterfacesAndTypes(this ContainerBuilder builder)
        {

            var appSettingsType = typeof(AppSettings);
            builder.RegisterType(appSettingsType).As<IAppSettings>().InstancePerHttpRequest().PropertiesAutowired();
            builder.RegisterType(appSettingsType).As<ISmtpSettings>().InstancePerHttpRequest();

            builder.RegisterType<SmtpMailService>().As<IMailService>().InstancePerHttpRequest();
            builder.RegisterType<WebSecurityService>().As<ISecurityService>().InstancePerHttpRequest();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerHttpRequest();

            var appSettings = new CkgDomainAppSettings();
            var logonSettingsType = (appSettings.IsClickDummyMode ? typeof(LogonContextTestAutohaus) : typeof(LogonContextDataServiceAutohaus));
            builder.RegisterType(logonSettingsType).As<ILogonContextDataService>().InstancePerHttpRequest()
                .PropertiesAutowired();

            builder.RegisterType<AdressenDataServiceSAP>().As<IAdressenDataService>().InstancePerHttpRequest();
            builder.RegisterType<UeberfuehrungDataServiceSAP>().As<IUeberfuehrungDataService>().InstancePerHttpRequest();
            builder.RegisterType<AutohausFeinstaubVergabeDataServiceSAP>().As<IAutohausFeinstaubVergabeDataService>().InstancePerHttpRequest();
            builder.RegisterType<AutohausFeinstaubReportDataServiceSAP>().As<IAutohausFeinstaubReportDataService>().InstancePerHttpRequest();

            ModelMetadataProviders.Current = new AnnotationsAndConventionsBasedModelMetaDataProvider();
        }
    }
}
