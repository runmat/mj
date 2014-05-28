using System.Web.Mvc;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.Mvc;
using FluentValidation;
using FluentValidation.Mvc;
using NLog;
using RefImpl.Filters;
using RefImpl.Helpers;
using RefImpl.Services;
using RefImplBibl.Interfaces;
using RefImplBibl.Logging;
using RefImplBibl.Models;
using RefImplBibl.Services;
using RefImplBibl.Validation;

namespace RefImpl.App_Start
{
    public static class IocConfig
    {
        public static void RegisterIocContainer()
        {
            // IoC container starten
            var builder = new ContainerBuilder();

            // container soll die Controller ermitteln für die Runtime
            builder.RegisterControllers(typeof (MvcApplication).Assembly);
            builder.RegisterModule(new AutofacWebTypesModule());

            // Eigne Interfaces und deren Implementierungen registrieren
            
            builder.RegisterType<Sap>().As<ISap>().EnableInterfaceInterceptors().InstancePerHttpRequest();
            builder.RegisterType<AnwenderInfoProvider>().As<IAnwenderInfoProvider>().InstancePerHttpRequest();

            // Logger
            builder.RegisterType<SapLogger>().As<SapLogger>().InstancePerHttpRequest();
            builder.RegisterType<ErrorLogger>().As<ErrorLogger>().InstancePerHttpRequest();


            builder.Register(c => new SapInterceptor(DependencyResolver.Current.GetService<SapLogger>(), DependencyResolver.Current.GetService<IAnwenderInfoProvider>()));

            // Validatoren registrieren beim IoC -> Wenn eine Model Klasse dem controller präsentiert wird, laufen die Validierungen
            builder.RegisterType<EquiValidator>().As<IValidator<EQUI>>();

            // ActionFilter über AutoFac erstellen lassen, Constructor Injection 
            builder.RegisterFilterProvider();

            builder.RegisterType<CkgHandleErrorAttribute>().As<CkgHandleErrorAttribute>().PropertiesAutowired();

            // container an MVC übergeben
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Validation Modalität in MVC an IoC binden
            ModelValidatorProviders.Providers.Clear();
            var fluentValidationModelValidatorProvider = new FluentValidationModelValidatorProvider(new RefImplValidatorFactory());
            ModelValidatorProviders.Providers.Add(fluentValidationModelValidatorProvider);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            fluentValidationModelValidatorProvider.AddImplicitRequiredValidator = false;
        }
    }
}