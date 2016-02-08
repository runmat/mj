using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.ViewModels;
using GeneralTools.Contracts;

namespace ServicesMvc.Autohaus.Controllers
{
    public class EsdController : CkgDomainController
    {
        public override string DataContextKey { get { return GetDataContextKey<CocAnforderungViewModel>(); } }

        public CocAnforderungViewModel ViewModel 
        { 
            get { return GetViewModel<CocAnforderungViewModel>(); } 
            set { SetViewModel(value); } 
        }

        public EsdController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IZulassungDataService zulassungDataService,
            ICocAnforderungDataService cocAnforderungDataService
            )
            : base(appSettings, logonContext)
        {
            if (IsInitialRequestOf("Index"))
                ViewModel = null;

            InitViewModel(ViewModel, appSettings, logonContext, zulassungDataService, cocAnforderungDataService);
            InitModelStatics();
        }

        private void InitModelStatics()
        {
            CocAnforderungKopfdaten.GetViewModel = GetViewModel<CocAnforderungViewModel>;
            CkgDomainLogic.Autohaus.Models.CocAnforderung.GetViewModel = GetViewModel<CocAnforderungViewModel>;
        }

        [CkgApplication]
        public ActionResult AuslandsAnforderung()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult ApplyKopfdaten(CocAnforderungKopfdaten model)
        {
            if (ModelState.IsValid)
                ViewModel.ApplyKopfdaten(model);

            return PartialView("Partial/FormKopf", model);
        }

        [HttpPost]
        public ActionResult ShowDetails()
        {
            return PartialView("Partial/FormDetails", ViewModel.CocAnforderung);
        }

        [HttpPost]
        public ActionResult CocAnfordern(CocAnforderung model)
        {
            if (ModelState.IsValid)
                ViewModel.CocAnforderungAbsenden(model, ModelState.AddModelError);

            return PartialView("Partial/FormDetails", model);
        }

        [HttpPost]
        public ActionResult ShowReceipt()
        {
            return PartialView("Partial/Receipt", ViewModel);
        }
    }
}
