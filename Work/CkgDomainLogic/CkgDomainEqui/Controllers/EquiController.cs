using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class EquiController : CkgDomainController
    {
        public override string DataContextKey { get { return "EquiViewModel"; } }


        public EquiController(IAppSettings appSettings, ILogonContextDataService logonContext, IEquiGrunddatenDataService equiGrunddatenDataService, IEquiHistorieDataService equiHistorieDataService, 
            IBriefbestandDataService briefbestandDataService, IAdressenDataService adressenDataService, IBriefVersandDataService briefVersandDataService, IMahnreportDataService mahnreportDataService,
            IDatenOhneDokumenteDataService datenOhneDokumenteDataService, IErweiterterBriefbestandDataService erweiterterBriefbestandDataService, IAbweichungenDataService abweichungenDataService, 
            IDokumenteOhneDatenDataService dokumenteOhneDatenDataService, IMahnsperreDataService mahnsperreDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(EquiGrunddatenEquiViewModel, appSettings, logonContext, equiGrunddatenDataService);
            InitViewModel(EquipmentHistorieViewModel, appSettings, logonContext, equiHistorieDataService);
            InitViewModel(BriefbestandViewModel, appSettings, logonContext, briefbestandDataService);
            InitViewModel(BriefversandViewModel, appSettings, logonContext, briefbestandDataService, adressenDataService, briefVersandDataService);
            InitViewModel(MahnreportViewModel, appSettings, logonContext, mahnreportDataService);
            InitViewModel(DatenOhneDokumenteViewModel, appSettings, logonContext, datenOhneDokumenteDataService);
            InitViewModel(ErweiterterBriefbestandViewModel, appSettings, logonContext, erweiterterBriefbestandDataService);
            InitViewModel(HalterAbweichungenViewModel, appSettings, logonContext, abweichungenDataService);
            InitViewModel(DokumenteOhneDatenViewModel, appSettings, logonContext, dokumenteOhneDatenDataService);
            InitViewModel(MahnsperreViewModel, appSettings, logonContext, mahnsperreDataService);

            InitModelStatics();
        }

        void InitModelStatics()
        {
            CkgDomainLogic.Equi.Models.VersandOptionen.GetViewModel = GetViewModel<BriefversandViewModel>;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ReportFahrzeugbestand");
        }
    }
}
