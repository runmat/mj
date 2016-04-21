using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.ViewModels;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Leasing-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class LeasingController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<LeasingZB1KopienViewModel>(); } }

        public LeasingCargateCsvUploadViewModel LeasingCargateCsvUploadViewModel { get { return GetViewModel<LeasingCargateCsvUploadViewModel>(); } }

        public LeasingController(IAppSettings appSettings, 
            ILogonContextDataService logonContext, 
            ILeasingZB1KopienDataService zB1KopienDataService, 
            ILeasingUnzugelFzgDataService unzugelFzgDataService, 
            ILeasingAbmeldungDataService abmeldungDataService, 
            ILeasingKlaerfaelleDataService klaerfaelleDataService,
            ILeasingCargateCsvUploadDataService cargateCsvUploadService, 
            ILeasingSicherungsscheineDataService sicherungsscheineDataService,
            INichtDurchfuehrbZulDataService nichtDurchfuehrbZulDataService, 
            IUeberfaelligeRuecksendungenDataService ueberfaelligeRuecksendungenDataService,
            IUnzugelasseneFahrzeugeDataService unzugelasseneFahrzeugeDataService,
            ILeasingEndgueltigerVersandDataService endgueltigerVersandDataService
            )
            : base(appSettings, logonContext)
            {
            InitViewModel(ZB1KopienViewModel, appSettings, logonContext, zB1KopienDataService);
            InitViewModel(BriefeOhneLVNrViewModel, appSettings, logonContext, unzugelFzgDataService);
            InitViewModel(AbmeldungViewModel, appSettings, logonContext, abmeldungDataService);
            InitViewModel(KlaerfaelleViewModel, appSettings, logonContext, klaerfaelleDataService);
            InitViewModel(LeasingCargateCsvUploadViewModel, appSettings, logonContext, cargateCsvUploadService);  // Dataservice Initialisierung folgt noch
            InitViewModel(SicherungsscheineViewModel, appSettings, logonContext, sicherungsscheineDataService);
            InitViewModel(NichtDurchfuehrbZulViewModel, appSettings, logonContext, nichtDurchfuehrbZulDataService);
            InitViewModel(UeberfaelligeRuecksendungenViewModel, appSettings, logonContext, ueberfaelligeRuecksendungenDataService);
            InitViewModel(UnzugelasseneFahrzeugeViewModel, appSettings, logonContext, unzugelasseneFahrzeugeDataService);
            InitViewModel(EndgueltigerVersandViewModel, appSettings, logonContext, endgueltigerVersandDataService);
            }

        public ActionResult Index(string un, string appID)
        {
            return RedirectToAction("ReportZB1Kopien", new { un, appID });
        }

    }
}
