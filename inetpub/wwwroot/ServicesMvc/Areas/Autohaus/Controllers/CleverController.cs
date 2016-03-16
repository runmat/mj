using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Autohaus.Controllers
{
    public class CleverController : CkgDomainController
    {
        public override string DataContextKey { get { return GetDataContextKey<CleverViewModel>(); } }

        public CleverViewModel CleverViewModel { get { return GetViewModel<CleverViewModel>(); } }

        public StatusverfolgungZulassungViewModel StatusverfolgungViewModel { get { return GetViewModel<StatusverfolgungZulassungViewModel>(); } }

        public CleverController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IAdressenDataService adressenDataService,
            IZulassungDataService zulassungDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(CleverViewModel, appSettings, logonContext, adressenDataService, zulassungDataService);
            InitViewModel(StatusverfolgungViewModel, appSettings, logonContext, zulassungDataService);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Search");
        }

        [CkgApplication]
        public ActionResult Search()
        {
            CleverViewModel.DataInit();
            StatusverfolgungViewModel.DataInit();

            return View(CleverViewModel);
        }

        [HttpPost]
        public ActionResult PerformSearch(string anfrageText, string anfrageTyp)
        {
            CleverViewModel.PerformSearch(anfrageText, anfrageTyp, ModelState.AddModelError);

            return PartialView("Partial/SearchResult", CleverViewModel);
        }

        [HttpPost]
        public ActionResult ShowStatusverfolgung(string belegNr)
        {
            var zulDaten = CleverViewModel.GetZulassungsReportItem(belegNr);
            var detailDaten = StatusverfolgungViewModel.GetStatusverfolgungDetails(zulDaten, ModelState.AddModelError);

            return PartialView("StatusverfolgungZulassungDetails", detailDaten);
        }
    }
}
