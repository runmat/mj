using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using CkgDomainLogic.Archive.Contracts;
using CkgDomainLogic.Archive.Services;
using CkgDomainLogic.Charts.Contracts;
using CkgDomainLogic.Charts.Services;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Services;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Services;
using CkgDomainLogic.Fahrer.Contracts;
using CkgDomainLogic.Fahrer.Services;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Services;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Services;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Services;
using CkgDomainLogic.Logs.Contracts;
using CkgDomainLogic.Logs.Services;
using CkgDomainLogic.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Services;
using CkgDomainLogic.Strafzettel.Contracts;
using CkgDomainLogic.Strafzettel.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Data;
using MvcTools.Web;
using PortalMvcTools.Services;
using WebTools.Services;

namespace ServicesMvc.App_Start
{
    public static class IocConfig
    {
        public static void RegisterIocContainer()
        {
            // IoC container starten
            var builder = new ContainerBuilder();

            // container soll die Controller ermitteln für die Runtime
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterControllers(Assembly.Load("CkgDomainLogic"));
            builder.RegisterControllers(Assembly.Load("CkgDomainCommon"));
            builder.RegisterControllers(Assembly.Load("CkgDomainCoc"));
            builder.RegisterControllers(Assembly.Load("CkgDomainFahrzeug"));
            builder.RegisterControllers(Assembly.Load("CkgDomainLeasing"));
            builder.RegisterControllers(Assembly.Load("CkgDomainArchive"));
            builder.RegisterControllers(Assembly.Load("CkgDomainFinance"));
            builder.RegisterControllers(Assembly.Load("CkgDomainInsurance"));
            builder.RegisterControllers(Assembly.Load("CkgDomainFahrer"));
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
            builder.Register(c => S.AP).InstancePerHttpRequest(); 

            var appSettings = new CkgDomainAppSettings();
            builder.RegisterType(appSettings.GetType()).As<IAppSettings>().InstancePerHttpRequest().PropertiesAutowired();
            builder.RegisterType(appSettings.GetType()).As<ISmtpSettings>().InstancePerHttpRequest();

            builder.RegisterType<SmtpMailService>().As<IMailService>().InstancePerHttpRequest();
            builder.RegisterType<WebSecurityService>().As<ISecurityService>().InstancePerHttpRequest();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerHttpRequest();

            var logonSettingsType = (appSettings.IsClickDummyMode ? typeof(LogonContextTest) : typeof(LogonContextDataServiceDadServices));
            builder.RegisterType(logonSettingsType).As<ILogonContextDataService>().InstancePerHttpRequest()
                .PropertiesAutowired();

            var grunddatenEquiBestandDataService = (appSettings.IsClickDummyMode ? typeof(EquiGrunddatenDataServiceTest) : typeof(EquiGrunddatenDataServiceSAP));
            builder.RegisterType(grunddatenEquiBestandDataService).As<IEquiGrunddatenDataService>().InstancePerHttpRequest();
            builder.RegisterType<EquiHistorieDataServiceSAP>().As<IEquiHistorieDataService>().InstancePerHttpRequest();
            builder.RegisterType<FahrzeugeDataServiceSAP>().As<IFahrzeugeDataService>().InstancePerHttpRequest();
            builder.RegisterType<StrafzettelDataServiceSAP>().As<IStrafzettelDataService>().InstancePerHttpRequest();
            builder.RegisterType<FehlteilEtikettenDataServiceSAP>().As<IFehlteilEtikettenDataService>().InstancePerHttpRequest();
            
            builder.RegisterType<CocTypenDataServiceSAP>().As<ICocTypenDataService>().InstancePerHttpRequest();
            builder.RegisterType<CocErfassungDataServiceSAP>().As<ICocErfassungDataService>().InstancePerHttpRequest();
            builder.RegisterType<AdressenDataServiceSAP>().As<IAdressenDataService>().InstancePerHttpRequest();
            builder.RegisterType<BriefVersandDataServiceSAP>().As<IBriefVersandDataService>().InstancePerHttpRequest();
            builder.RegisterType<ZulassungDataServiceSAP>().As<IZulassungDataService>().InstancePerHttpRequest();
            builder.RegisterType<InfoCenterDataService>().As<IInfoCenterDataService>().InstancePerHttpRequest();
            builder.RegisterType<LeasingZB1KopienDataServiceSAP>().As<ILeasingZB1KopienDataService>().InstancePerHttpRequest();
            builder.RegisterType<LeasingUnzugelFzgDataServiceSAP>().As<ILeasingUnzugelFzgDataService>().InstancePerHttpRequest();
            builder.RegisterType<LeasingAbmeldungDataServiceSAP>().As<ILeasingAbmeldungDataService>().InstancePerHttpRequest();
            builder.RegisterType<LeasingKlaerfaelleDataServiceSAP>().As<ILeasingKlaerfaelleDataService>().InstancePerHttpRequest();
            builder.RegisterType<EasyAccessDataService>().As<IEasyAccessDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceAktivcheckDataServiceSAP>().As<IFinanceAktivcheckDataService>().InstancePerHttpRequest();
            builder.RegisterType<FahrerDataServiceSAP>().As<IFahrerDataService>().InstancePerHttpRequest();
            builder.RegisterType<LeasingCargateCsvUploadDataServiceSAP>().As<ILeasingCargateCsvUploadDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceGebuehrenauslageDataServiceSAP>().As<IFinanceGebuehrenauslageDataService>().InstancePerHttpRequest();
            builder.RegisterType<LeasingSicherungsscheineDataService>().As<ILeasingSicherungsscheineDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceBewertungDataServiceSAP>().As<IFinanceBewertungDataService>().InstancePerHttpRequest();
            builder.RegisterType<CustomerDocumentDataService>().As<ICustomerDocumentDataService>().InstancePerHttpRequest();
            builder.RegisterType<UploadBestandsdatenDataServiceSap>().As<IUploadBestandsdatenDataService>().InstancePerHttpRequest();
            builder.RegisterType<BestandsdatenDataServiceSap>().As<IBestandsdatenDataService>().InstancePerHttpRequest();
            builder.RegisterType<VertragsverlaengerungDataServiceSap>().As<IVertragsverlaengerungDataService>().InstancePerHttpRequest();
            builder.RegisterType<BriefbestandDataServiceSAP>().As<IBriefbestandDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceVersandsperreDataServiceSAP>().As<IFinanceVersandsperreDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceVersandsperreReportDataServiceSAP>().As<IFinanceVersandsperreReportDataService>().InstancePerHttpRequest();
            builder.RegisterType<UploadFahrzeugeinsteuerungDataServiceSAP>().As<IUploadFahrzeugeinsteuerungDataService>().InstancePerHttpRequest();
            builder.RegisterType<ErweiterterBriefbestandDataServiceSAP>().As<IErweiterterBriefbestandDataService>().InstancePerHttpRequest();

            builder.RegisterType<TranslationFormatService>().As<ITranslationFormatService>().InstancePerHttpRequest();
            builder.RegisterType<SessionDataHelper>().As<ISessionDataHelper>().InstancePerHttpRequest();
            builder.RegisterType<LogsDataServiceSql>().As<ILogsDataService>().InstancePerHttpRequest();
            builder.RegisterType<ChartsDataServiceSql>().As<IChartsDataService>().InstancePerHttpRequest();

            builder.RegisterType<SchadenakteDataServiceSAP>().As<ISchadenakteDataService>().InstancePerHttpRequest();
            if (System.Configuration.ConfigurationManager.AppSettings["CsiVersEventsDataServiceSql"].NotNullOrEmpty().ToLower() == "true")
                builder.RegisterType<VersEventsDataServiceSQL>().As<IVersEventsDataService>().InstancePerHttpRequest();
            else
                builder.RegisterType<VersEventsDataServiceSAP>().As<IVersEventsDataService>().InstancePerHttpRequest();

            ModelMetadataProviders.Current = new AnnotationsAndConventionsBasedModelMetaDataProvider();
        }
    }
}
