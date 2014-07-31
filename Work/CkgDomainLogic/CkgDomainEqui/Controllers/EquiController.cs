using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.Equi.Contracts;
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
            IDatenOhneDokumenteDataService datenOhneDokumenteDataService, IErweiterterBriefbestandDataService erweiterterBriefbestandDataService, IHalterabweichungenDataService halterabweichungenDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(EquiGrunddatenEquiViewModel, appSettings, logonContext, equiGrunddatenDataService);
            InitViewModel(EquipmentHistorieViewModel, appSettings, logonContext, equiHistorieDataService);
            InitViewModel(BriefbestandViewModel, appSettings, logonContext, briefbestandDataService);
            InitViewModel(BriefversandViewModel, appSettings, logonContext, briefbestandDataService, adressenDataService, briefVersandDataService);
            InitViewModel(MahnreportViewModel, appSettings, logonContext, mahnreportDataService);
            InitViewModel(DatenOhneDokumenteViewModel, appSettings, logonContext, datenOhneDokumenteDataService);
            InitViewModel(ErweiterterBriefbestandViewModel, appSettings, logonContext, erweiterterBriefbestandDataService);
            InitViewModel(HalterabweichungenViewModel, appSettings, logonContext, halterabweichungenDataService);
        }

        public ActionResult Index()
        {
            return RedirectToAction("ReportFahrzeugbestand");
        }
    }
}
