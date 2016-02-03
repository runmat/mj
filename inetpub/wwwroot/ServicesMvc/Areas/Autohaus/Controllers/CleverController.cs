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

        public CleverViewModel ViewModel { get { return GetViewModel<CleverViewModel>(); } }

        public CleverController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IAdressenDataService adressenDataService,
            IZulassungDataService zulassungDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, adressenDataService, zulassungDataService);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Search");
        }

        [CkgApplication]
        public ActionResult Search()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult PerformSearch(string anfrageText, string anfrageTyp)
        {
            ViewModel.PerformSearch(anfrageText, anfrageTyp, ModelState.AddModelError);

            return PartialView("Partial/SearchResult", ViewModel);
        }
    }
}
