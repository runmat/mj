﻿using System.Reflection;
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
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Services;
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
using CkgDomainLogic.Partner.Contracts;
using CkgDomainLogic.Partner.Services;
using CkgDomainLogic.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Services;
using CkgDomainLogic.Strafzettel.Contracts;
using CkgDomainLogic.Strafzettel.Services;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Services;
using CkgDomainLogic.UserReporting.Contracts;
using CkgDomainLogic.UserReporting.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Data;
using MvcTools.Web;
using PortalMvcTools.Services;
using SapORM.Contracts;
using WebTools.Services;
using CkgDomainLogic.AutohausFahrzeugdaten.Services;
using CkgDomainLogic.AutohausFahrzeugdaten.Contracts;

namespace ServicesMvc
{
    public static class IocConfig
    {
        public static void CreateAndRegisterIocContainerToMvc()
        {
            var container = CreateIocContainerAndRegisterTypes();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static IContainer CreateIocContainerAndRegisterTypes(ISapDataService sap = null)
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
            builder.RegisterControllers(Assembly.Load("CkgDomainAutohaus"));
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterModule(new AutofacWebTypesModule());

            // Eigene Interfaces und deren Implementierungen registrieren
            builder.RegisterIocInterfacesAndTypes(sap);

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            // container an MVC übergeben
            var container = builder.Build();
            return container;
        }
        
        public static void RegisterIocInterfacesAndTypes(this ContainerBuilder builder, ISapDataService sap = null)
        {
            builder.Register(c => sap ?? S.AP).InstancePerLifetimeScope();

            var appSettings = new CkgDomainAppSettings();
            builder.RegisterType(appSettings.GetType()).As<IAppSettings>().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType(appSettings.GetType()).As<ISmtpSettings>().InstancePerLifetimeScope();

            builder.RegisterType<SmtpMailService>().As<IMailService>().InstancePerLifetimeScope();
            builder.RegisterType<WebSecurityService>().As<ISecurityService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerLifetimeScope();


            var logonSettingsType = (appSettings.IsClickDummyMode ? typeof(LogonContextTest) : typeof(LogonContextDataServiceDadServices));

            // Persistance (Warenkorb, etc)
            builder.RegisterType<PersistanceServiceSql>().As<IPersistanceService>().InstancePerLifetimeScope().PropertiesAutowired();

            builder.RegisterType(logonSettingsType).As<ILogonContextDataService>().InstancePerLifetimeScope().PropertiesAutowired();


            var grunddatenEquiBestandDataService = (appSettings.IsClickDummyMode ? typeof(EquiGrunddatenDataServiceTest) : typeof(EquiGrunddatenDataServiceSAP));
            builder.RegisterType(grunddatenEquiBestandDataService).As<IEquiGrunddatenDataService>().InstancePerLifetimeScope();
            builder.RegisterType<EquiHistorieDataServiceSAP>().As<IEquiHistorieDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FahrzeugeDataServiceSAP>().As<IFahrzeugeDataService>().InstancePerLifetimeScope();
            builder.RegisterType<StrafzettelDataServiceSAP>().As<IStrafzettelDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FehlteilEtikettenDataServiceSAP>().As<IFehlteilEtikettenDataService>().InstancePerLifetimeScope();
            
            builder.RegisterType<CocTypenDataServiceSAP>().As<ICocTypenDataService>().InstancePerLifetimeScope();
            builder.RegisterType<CocErfassungDataServiceSAP>().As<ICocErfassungDataService>().InstancePerLifetimeScope();
            builder.RegisterType<AdressenDataServiceSAP>().As<IAdressenDataService>().InstancePerLifetimeScope();
            builder.RegisterType<BriefVersandDataServiceSAP>().As<IBriefVersandDataService>().InstancePerLifetimeScope();
            builder.RegisterType<CkgDomainLogic.CoC.Services.ZulassungDataServiceSAP>().As<CkgDomainLogic.CoC.Contracts.IZulassungDataService>().InstancePerLifetimeScope();
            builder.RegisterType<InfoCenterDataService>().As<IInfoCenterDataService>().InstancePerLifetimeScope();
            builder.RegisterType<LeasingZB1KopienDataServiceSAP>().As<ILeasingZB1KopienDataService>().InstancePerLifetimeScope();
            builder.RegisterType<LeasingUnzugelFzgDataServiceSAP>().As<ILeasingUnzugelFzgDataService>().InstancePerLifetimeScope();
            builder.RegisterType<LeasingAbmeldungDataServiceSAP>().As<ILeasingAbmeldungDataService>().InstancePerLifetimeScope();
            builder.RegisterType<LeasingKlaerfaelleDataServiceSAP>().As<ILeasingKlaerfaelleDataService>().InstancePerLifetimeScope();
            builder.RegisterType<EasyAccessDataService>().As<IEasyAccessDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FinanceAktivcheckDataServiceSAP>().As<IFinanceAktivcheckDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FahrerDataServiceSAP>().As<IFahrerDataService>().InstancePerLifetimeScope();
            builder.RegisterType<LeasingCargateCsvUploadDataServiceSAP>().As<ILeasingCargateCsvUploadDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FinanceGebuehrenauslageDataServiceSAP>().As<IFinanceGebuehrenauslageDataService>().InstancePerLifetimeScope();
            builder.RegisterType<LeasingSicherungsscheineDataService>().As<ILeasingSicherungsscheineDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FinanceBewertungDataServiceSAP>().As<IFinanceBewertungDataService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerDocumentDataService>().As<ICustomerDocumentDataService>().InstancePerLifetimeScope();
            builder.RegisterType<UploadBestandsdatenDataServiceSap>().As<IUploadBestandsdatenDataService>().InstancePerLifetimeScope();
            builder.RegisterType<BestandsdatenDataServiceSap>().As<IBestandsdatenDataService>().InstancePerLifetimeScope();
            builder.RegisterType<VertragsverlaengerungDataServiceSap>().As<IVertragsverlaengerungDataService>().InstancePerLifetimeScope();
            builder.RegisterType<BriefbestandDataServiceSAP>().As<IBriefbestandDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FinanceVersandsperreDataServiceSAP>().As<IFinanceVersandsperreDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FinanceVersandsperreReportDataServiceSAP>().As<IFinanceVersandsperreReportDataService>().InstancePerLifetimeScope();
	        builder.RegisterType<FinanceTelefonieReportDataServiceSAP>().As<IFinanceTelefonieReportDataService>().InstancePerLifetimeScope();
            builder.RegisterType<UploadFahrzeugeinsteuerungDataServiceSAP>().As<IUploadFahrzeugeinsteuerungDataService>().InstancePerLifetimeScope();
            builder.RegisterType<MahnreportDataServiceSAP>().As<IMahnreportDataService>().InstancePerLifetimeScope();
            builder.RegisterType<DatenOhneDokumenteDataServiceSAP>().As<IDatenOhneDokumenteDataService>().InstancePerLifetimeScope();
            builder.RegisterType<ErweiterterBriefbestandDataServiceSAP>().As<IErweiterterBriefbestandDataService>().InstancePerLifetimeScope();
            builder.RegisterType<AbweichungenDataServiceSAP>().As<IAbweichungenDataService>().InstancePerLifetimeScope();
            builder.RegisterType<DokumenteOhneDatenDataServiceSAP>().As<IDokumenteOhneDatenDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FinanceMahnstufenDataServiceSAP>().As<IFinanceMahnstufenDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FinanceMahnstopDataServiceSAP>().As<IFinanceMahnstopDataService>().InstancePerLifetimeScope();
            builder.RegisterType<MahnsperreDataServiceSAP>().As<IMahnsperreDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FinanceMahnungenVorErsteingangDataServiceSAP>().As<IFinanceMahnungenVorErsteingangDataService>().InstancePerLifetimeScope();

            builder.RegisterType<TranslationFormatService>().As<ITranslationFormatService>().InstancePerLifetimeScope();
            builder.RegisterType<SessionDataHelper>().As<ISessionDataHelper>().InstancePerLifetimeScope();
            builder.RegisterType<LogsDataServiceSql>().As<ILogsDataService>().InstancePerLifetimeScope();
            builder.RegisterType<ChartsDataServiceSql>().As<IChartsDataService>().InstancePerLifetimeScope();

            builder.RegisterType<SchadenakteDataServiceSAP>().As<ISchadenakteDataService>().InstancePerLifetimeScope();
            if (System.Configuration.ConfigurationManager.AppSettings["CsiVersEventsDataServiceSql"].NotNullOrEmpty().ToLower() == "true")
                builder.RegisterType<VersEventsDataServiceSQL>().As<IVersEventsDataService>().InstancePerLifetimeScope();
            else
                builder.RegisterType<VersEventsDataServiceSAP>().As<IVersEventsDataService>().InstancePerLifetimeScope();

            builder.RegisterType<UebfuehrgDataServiceSAP>().As<IUebfuehrgDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FahrzeugAkteBestandDataServiceSAP>().As<IFahrzeugAkteBestandDataService>().InstancePerLifetimeScope();
            builder.RegisterType<PartnerDataServiceSAP>().As<IPartnerDataService>().InstancePerLifetimeScope();

            builder.RegisterType<FinanceAnzeigePruefpunkteDataServiceSAP>().As<IFinanceAnzeigePruefpunkteDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FinancePruefschritteDataServiceSAP>().As<IFinancePruefschritteDataService>().InstancePerLifetimeScope();

            builder.RegisterType<CkgDomainLogic.Autohaus.Services.ZulassungDataServiceTest>().As<CkgDomainLogic.Autohaus.Contracts.IZulassungDataService>().InstancePerLifetimeScope();
            builder.RegisterType<UploadFahrzeugdatenDataServiceSap>().As<IUploadFahrzeugdatenDataService>().InstancePerHttpRequest();

            builder.RegisterType<UserReportingDataServiceSql>().As<IUserReportingDataService>().InstancePerLifetimeScope();

            builder.RegisterType<DashboardDataServiceSql>().As<IDashboardDataService>().InstancePerLifetimeScope();

            ModelMetadataProviders.Current = new AnnotationsAndConventionsBasedModelMetaDataProvider();
        }
    }
}
