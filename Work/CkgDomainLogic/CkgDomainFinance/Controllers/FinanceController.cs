using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.ViewModels;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Finance-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FinanceController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<FinanceAktivcheckViewModel>(); } }

        public FinanceController(IAppSettings appSettings, ILogonContextDataService logonContext, IFinanceAktivcheckDataService aktivcheckDataService, 
            IFinanceGebuehrenauslageDataService gebuehrenauslageDataService, IFinanceBewertungDataService bewertungDataService, 
            IFinanceVersandsperreDataService versandsperreDataService, IFinanceVersandsperreReportDataService versandsperreReportDataService, 
            IFinanceTelefonieReportDataService telefonieReportDataService, IFinanceMahnstufenDataService mahnstufenDataService, 
            IFinanceMahnstopDataService mahnstopDataService, IFinanceAnzeigePruefpunkteDataService anzeigePruefpunkteDataService, 
            IFinancePruefschritteDataService pruefschritteDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(AktivcheckViewModel, appSettings, logonContext, aktivcheckDataService);
            InitViewModel(GebuehrenauslageViewModel, appSettings, logonContext, gebuehrenauslageDataService);
            InitViewModel(BewertungViewModel, appSettings, logonContext, bewertungDataService);
            InitViewModel(VersandsperreViewModel, appSettings, logonContext, versandsperreDataService);
            InitViewModel(VersandsperreReportViewModel, appSettings, logonContext, versandsperreReportDataService);
            InitViewModel(TelefonieReportViewModel, appSettings, logonContext, telefonieReportDataService);
            InitViewModel(MahnstufenViewModel, appSettings, logonContext, mahnstufenDataService);
            InitViewModel(MahnstopViewModel, appSettings, logonContext, mahnstopDataService);
            InitViewModel(AnzeigePruefpunkteViewModel, appSettings, logonContext, anzeigePruefpunkteDataService);
            InitViewModel(PruefschritteViewModel, appSettings, logonContext, pruefschritteDataService);
        }

        public ActionResult Index(string un, string appID)
        {
            return RedirectToAction("Aktivcheck", new { un, appID });
        }

    }
}
