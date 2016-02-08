using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using CkgDomainInternal.Verbandbuch.Contracts;
using CkgDomainInternal.Verbandbuch.Services;
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
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Services;
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

using CkgDomainLogic.AppUserOverview.Contracts; // MaihoferM
using CkgDomainLogic.AppUserOverview.Services;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Services;
// MaihoferM

using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Services;
using CkgDomainLogic.UserReporting.Contracts;
using CkgDomainLogic.UserReporting.Services;
using CkgDomainLogic.WFM.Contracts;
using CkgDomainLogic.WFM.Services;
using CkgDomainLogic.Zanf.Contracts;
using CkgDomainLogic.Zanf.Services;
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
using Telerik.Web.Mvc.Infrastructure;
using ILocalizationService = GeneralTools.Contracts.ILocalizationService;
using IZulassungDataService = CkgDomainLogic.CoC.Contracts.IZulassungDataService;
using ZulassungDataServiceSAP = CkgDomainLogic.CoC.Services.ZulassungDataServiceSAP;

namespace ServicesMvc
{
    public static class IocConfig
    {
        public static void CreateAndRegisterIocContainerToMvc(IEnumerable<Assembly> assemblies)
        {
            var container = CreateIocContainerAndRegisterTypes(assemblies);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static IContainer CreateIocContainerAndRegisterTypes(IEnumerable<Assembly> assemblies, ISapDataService sap = null)
        {
            // IoC container starten
            var builder = new ContainerBuilder();

            // container soll die Controller ermitteln für die Runtime
            assemblies.ToListOrEmptyList().ForEach(asm => builder.RegisterControllers(asm));

            builder.RegisterControllers(Assembly.Load("CkgDomainInternal"));
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
        
        public static void RegisterTelerikLocalizationAdapterServiceFactory()
        {
            DI.Current.Register<ILocalizationServiceFactory>(() => new TelerikLocalizationAdapterServiceFactory());
        }

        public static void RegisterIocInterfacesAndTypes(this ContainerBuilder builder, ISapDataService sap = null)
        {
            RegisterTelerikLocalizationAdapterServiceFactory();

            builder.Register(c => sap ?? S.AP).InstancePerLifetimeScope();

            var appSettings = new CkgDomainAppSettings();
            builder.RegisterType(appSettings.GetType()).As<IAppSettings>().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType(appSettings.GetType()).As<ISmtpSettings>().InstancePerLifetimeScope();

            builder.RegisterType<SmtpMailService>().As<IMailService>().InstancePerLifetimeScope();
            builder.RegisterType<WebSecurityService>().As<ISecurityService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerLifetimeScope();
            builder.RegisterType<SessionBasedKeyValueStore>().As<IKeyValueStore<string>>().InstancePerLifetimeScope();


            var logonSettingsType = (appSettings.IsClickDummyMode ? typeof(LogonContextTest) : typeof(LogonContextDataServiceDadServices));

            // Persistance (Warenkorb, etc)
            builder.RegisterType<PersistanceServiceSql>().As<IPersistanceService>().InstancePerLifetimeScope().PropertiesAutowired();

            builder.RegisterType(logonSettingsType).As<ILogonContextDataService>().InstancePerLifetimeScope().PropertiesAutowired();

            builder.RegisterType<ApplicationConfiguration>().As<IApplicationConfigurationProvider>().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType<ApplicationConfiguration>().As<ICustomerConfigurationProvider>().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType<GeneralConfiguration>().As<IGeneralConfigurationProvider>().InstancePerLifetimeScope().PropertiesAutowired();


            var grunddatenEquiBestandDataService = (appSettings.IsClickDummyMode ? typeof(EquiGrunddatenDataServiceTest) : typeof(EquiGrunddatenDataServiceSAP));
            builder.RegisterType(grunddatenEquiBestandDataService).As<IEquiGrunddatenDataService>().InstancePerLifetimeScope();
            builder.RegisterType<EquiHistorieDataServiceSAP>().As<IEquiHistorieDataService>().InstancePerLifetimeScope();
            builder.RegisterType<FahrzeugeDataServiceSAP>().As<IFahrzeugeDataService>().InstancePerLifetimeScope();

            builder.RegisterType<HolBringServiceDataServiceSAP>().As<IHolBringServiceDataService>().InstancePerLifetimeScope(); // ITA8163

            builder.RegisterType<StrafzettelDataServiceSAP>().As<IStrafzettelDataService>().InstancePerLifetimeScope();

            builder.RegisterType<AppUserOverviewDataService>().As<IAppUserOverviewDataService>().InstancePerLifetimeScope(); // MaihoferM

            builder.RegisterType<FehlteilEtikettenDataServiceSAP>().As<IFehlteilEtikettenDataService>().InstancePerLifetimeScope();
            
            builder.RegisterType<CocTypenDataServiceSAP>().As<ICocTypenDataService>().InstancePerHttpRequest();
            builder.RegisterType<CocErfassungDataServiceSAP>().As<ICocErfassungDataService>().InstancePerHttpRequest();
            builder.RegisterType<AdressenDataServiceSAP>().As<IAdressenDataService>().InstancePerHttpRequest();
            builder.RegisterType<BriefVersandDataServiceSAP>().As<IBriefVersandDataService>().InstancePerHttpRequest();
            builder.RegisterType<ZulassungDataServiceSAP>().As<IZulassungDataService>().InstancePerHttpRequest();

            builder.RegisterType<HolBringServiceDataServiceSAP>().As<IHolBringServiceDataService>().InstancePerHttpRequest();   // ITA6183

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
	        builder.RegisterType<FinanceTelefonieReportDataServiceSAP>().As<IFinanceTelefonieReportDataService>().InstancePerHttpRequest();
            builder.RegisterType<UploadFahrzeugeinsteuerungDataServiceSAP>().As<IUploadFahrzeugeinsteuerungDataService>().InstancePerHttpRequest();
            builder.RegisterType<MahnreportDataServiceSAP>().As<IMahnreportDataService>().InstancePerHttpRequest();
            builder.RegisterType<DatenOhneDokumenteDataServiceSAP>().As<IDatenOhneDokumenteDataService>().InstancePerHttpRequest();
            builder.RegisterType<ErweiterterBriefbestandDataServiceSAP>().As<IErweiterterBriefbestandDataService>().InstancePerHttpRequest();
            builder.RegisterType<AbweichungenDataServiceSAP>().As<IAbweichungenDataService>().InstancePerHttpRequest();
            builder.RegisterType<DokumenteOhneDatenDataServiceSAP>().As<IDokumenteOhneDatenDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceMahnstufenDataServiceSAP>().As<IFinanceMahnstufenDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceMahnstopDataServiceSAP>().As<IFinanceMahnstopDataService>().InstancePerHttpRequest();
            builder.RegisterType<MahnsperreDataServiceSAP>().As<IMahnsperreDataService>().InstancePerHttpRequest();
            builder.RegisterType<BriefbestandVhcDataServiceSAP>().As<IBriefbestandVhcDataService>().InstancePerHttpRequest();
            builder.RegisterType<KlaerfaelleVhcDataServiceSAP>().As<IKlaerfaelleVhcDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceMahnungenVorErsteingangDataServiceSAP>().As<IFinanceMahnungenVorErsteingangDataService>().InstancePerHttpRequest();

            builder.RegisterType<TranslationService>().As<ITranslationService>().InstancePerLifetimeScope();
            builder.RegisterType<LogonContextProvider>().As<ILogonContextProvider>().InstancePerLifetimeScope();
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

            builder.RegisterType<CkgDomainLogic.Autohaus.Services.ZulassungDataServiceSAP>().As<CkgDomainLogic.Autohaus.Contracts.IZulassungDataService>().InstancePerLifetimeScope();
            builder.RegisterType<UploadFahrzeugdatenDataServiceSap>().As<IUploadFahrzeugdatenDataService>().InstancePerHttpRequest();
            builder.RegisterType<DashboardDataServiceSql>().As<IDashboardDataService>().InstancePerLifetimeScope();
            builder.RegisterType<GridAdminDataServiceSql>().As<IGridAdminDataService>().InstancePerLifetimeScope(); 

            builder.RegisterType<UserReportingDataServiceSql>().As<IUserReportingDataService>().InstancePerLifetimeScope();
            builder.RegisterType<ZanfReportDataServiceSAP>().As<IZanfReportDataService>().InstancePerHttpRequest();
            builder.RegisterType<ZulassungsunterlagenDataServiceSap>().As<IZulassungsunterlagenDataService>().InstancePerHttpRequest();
            builder.RegisterType<NichtDurchfuehrbZulDataServiceSAP>().As<INichtDurchfuehrbZulDataService>().InstancePerHttpRequest();
            builder.RegisterType<EquiHistorieVermieterDataServiceSAP>().As<IEquiHistorieVermieterDataService>().InstancePerHttpRequest();
            builder.RegisterType<FahrzeugzulaeufeDataServiceSAP>().As<IFahrzeugzulaeufeDataService>().InstancePerHttpRequest();

            builder.RegisterType<WfmDataServiceSAP>().As<IWfmDataService>().InstancePerHttpRequest();

            builder.RegisterType<ModellIdDataServiceSAP>().As<IModellIdDataService>().InstancePerHttpRequest();

            builder.RegisterType<FinanceTempZb2VersandDataServiceSAP>().As<IFinanceTempZb2VersandDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceTempVersandZweitschluesselDataServiceSAP>().As<IFinanceTempZb2VersandZweitschluesselDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceFehlendeSchluesseltueteDataServiceSAP>().As<IFinanceFehlendeSchluesseltueteDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceCarporteingaengeOhneEHDataServiceSAP>().As<IFinanceCarporteingaengeOhneEHDataService>().InstancePerHttpRequest();
            builder.RegisterType<TreuhandDataServiceSAP>().As<ITreuhandDataService>().InstancePerHttpRequest();
            builder.RegisterType<FahrzeugvoravisierungDataServiceSAP>().As<IFahrzeugvoravisierungDataService>().InstancePerHttpRequest();
            builder.RegisterType<DispositionslisteDataServiceSAP>().As<IDispositionslisteDataService>().InstancePerHttpRequest();
            builder.RegisterType<HaendlerAdressenDataServiceSAP>().As<IHaendlerAdressenDataService>().InstancePerHttpRequest(); 
            builder.RegisterType<ZulaufEinsteuerungDataServiceSAP>().As<IZulaufEinsteuerungDataService>().InstancePerHttpRequest();           
            builder.RegisterType<FahrzeuguebersichtDataServiceSAP>().As<IFahrzeuguebersichtDataService>().InstancePerHttpRequest();
            builder.RegisterType<BatcherfassungDataServiceSAP>().As<IBatcherfassungDataService>().InstancePerHttpRequest();             
            builder.RegisterType<FahrzeugSperrenVerschiebenDataServiceSAP>().As<IFahrzeugSperrenVerschiebenDataService>().InstancePerHttpRequest();
            builder.RegisterType<StatusEinsteuerungDataServiceSAP>().As<IStatusEinsteuerungDataService>().InstancePerHttpRequest();
            builder.RegisterType<UeberfaelligeRuecksendungenDataServiceSAP>().As<IUeberfaelligeRuecksendungenDataService>().InstancePerHttpRequest();
            builder.RegisterType<FinanceVersendungenDataServiceSAP>().As<IFinanceVersendungenDataService>().InstancePerHttpRequest();
            builder.RegisterType<UploadAvislisteDataServiceSap>().As<IUploadAvislisteDataService>().InstancePerHttpRequest();
            builder.RegisterType<CarporterfassungDataServiceSAP>().As<ICarporterfassungDataService>().InstancePerHttpRequest();
            builder.RegisterType<DateiDownloadDataService>().As<IDateiDownloadDataService>().InstancePerHttpRequest();
            builder.RegisterType<PdfAnzeigeDataService>().As<IPdfAnzeigeDataService>().InstancePerHttpRequest();

            builder.RegisterType<VerbandbuchDataServiceSAP>().As<IVerbandbuchDataService>().InstancePerHttpRequest(); // ITA 8249 (Verbandbuch) RehrA

            builder.RegisterType<CocAnforderungDataServiceSAP>().As<ICocAnforderungDataService>().InstancePerHttpRequest();
            builder.RegisterType<EsdAnforderungDataServiceSAP>().As<IEsdAnforderungDataService>().InstancePerHttpRequest();

            ModelMetadataProviders.Current = new AnnotationsAndConventionsBasedModelMetaDataProvider();
        }
    }
}
