using System.Web.Mvc;
using CkgDomainLogic.Feinstaub.Contracts;
using CkgDomainLogic.Feinstaub.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace AutohausPortalMvc.Controllers
{
    /// <summary>
    /// Feinstaub-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FeinstaubController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<AutohausFeinstaubVergabeViewModel>(); } }

        public FeinstaubController(IAppSettings appSettings, ILogonContextDataServiceAutohaus logonContext, IAutohausFeinstaubVergabeDataService feinstaubVergabeDataService, 
            IAutohausFeinstaubReportDataService feinstaubReportDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(FeinstaubVergabeViewModel, appSettings, logonContext, feinstaubVergabeDataService);
            InitViewModel(FeinstaubReportViewModel, appSettings, logonContext, feinstaubReportDataService);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Feinstaubplaketten");
        }

    }
}
