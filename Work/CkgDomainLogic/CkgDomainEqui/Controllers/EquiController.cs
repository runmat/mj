using System.Web.Mvc;
using CkgDomainLogic.Archive.Contracts;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class EquiController : CkgDomainController
    {
        public override string DataContextKey { get { return "EquiViewModel"; } }

        public EquiController(
            IAppSettings appSettings, 
            ILogonContextDataService logonContext, 
            IEquiGrunddatenDataService equiGrunddatenDataService, 
            IEquiHistorieDataService equiHistorieDataService, 
            IBriefbestandDataService briefbestandDataService, 
            IAdressenDataService adressenDataService, 
            IBriefVersandDataService briefVersandDataService, 
            IMahnreportDataService mahnreportDataService,
            IDatenOhneDokumenteDataService datenOhneDokumenteDataService, 
            IErweiterterBriefbestandDataService erweiterterBriefbestandDataService, 
            IAbweichungenDataService abweichungenDataService, 
            IDokumenteOhneDatenDataService dokumenteOhneDatenDataService, 
            IMahnsperreDataService mahnsperreDataService, 
            IBriefbestandVhcDataService briefbestandVhcDataService, 
            IKlaerfaelleVhcDataService klaerfaelleVhcDataService,
            IEasyAccessDataService easyAccessDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(EquiGrunddatenViewModel, appSettings, logonContext, equiGrunddatenDataService);
            InitViewModel(EquipmentHistorieViewModel, appSettings, logonContext, equiHistorieDataService, briefbestandDataService, easyAccessDataService);
            InitViewModel(BriefbestandViewModel, appSettings, logonContext, briefbestandDataService);
            InitViewModel(BriefversandViewModel, appSettings, logonContext, briefbestandDataService, adressenDataService, briefVersandDataService);
            InitViewModel(MahnreportViewModel, appSettings, logonContext, mahnreportDataService);
            InitViewModel(DatenOhneDokumenteViewModel, appSettings, logonContext, datenOhneDokumenteDataService);
            InitViewModel(ErweiterterBriefbestandViewModel, appSettings, logonContext, erweiterterBriefbestandDataService);
            InitViewModel(HalterAbweichungenViewModel, appSettings, logonContext, abweichungenDataService);
            InitViewModel(DokumenteOhneDatenViewModel, appSettings, logonContext, dokumenteOhneDatenDataService);
            InitViewModel(MahnsperreViewModel, appSettings, logonContext, mahnsperreDataService);
            InitViewModel(BriefbestandVhcViewModel, appSettings, logonContext, briefbestandVhcDataService);
            InitViewModel(KlaerfaelleVhcViewModel, appSettings, logonContext, klaerfaelleVhcDataService);
            InitViewModel(EquipmentHistorieVermieterViewModel, appSettings, logonContext, equiHistorieDataService);
            InitViewModel(EquipmentHistorieRemarketingViewModel, appSettings, logonContext, equiHistorieDataService, easyAccessDataService);

            InitModelStatics();
        }

        void InitModelStatics()
        {
            CkgDomainLogic.Equi.Models.VersandOptionen.GetViewModel = GetViewModel<BriefversandViewModel>;
            FahrzeugAnforderung.GetViewModel = GetViewModel<EquiHistorieVermieterViewModel>;
            EquiHistorie.GetViewModel = GetViewModel<EquiHistorieViewModel>;
            EquiHistorieInfo.GetViewModel = GetViewModel<EquiHistorieViewModel>;
            EquiGrunddatenSelektor.GetViewModel = GetViewModel<EquiGrunddatenViewModel>;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ReportFahrzeugbestand");
        }
    }
}
