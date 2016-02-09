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
        public override string DataContextKey { get { return GetDataContextKey<EsdAnforderungViewModel>(); } }

        public EsdAnforderungViewModel ViewModel 
        { 
            get { return GetViewModel<EsdAnforderungViewModel>(); } 
            set { SetViewModel(value); } 
        }

        public EsdController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IZulassungDataService zulassungDataService,
            IEsdAnforderungDataService esdAnforderungDataService
            )
            : base(appSettings, logonContext)
        {
            if (IsInitialRequestOf("Index"))
                ViewModel = null;

            InitViewModel(ViewModel, appSettings, logonContext, zulassungDataService, esdAnforderungDataService);
            InitModelStatics();
        }

        private void InitModelStatics()
        {
            EsdAnforderung.GetViewModel = GetViewModel<EsdAnforderungViewModel>;
        }

        [CkgApplication]
        public ActionResult AuslandsBeauftragung()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        //[HttpPost]
        //public ActionResult ApplyKopfdaten(EsdAnforderungKopfdaten model)
        //{
        //    if (ModelState.IsValid)
        //        ViewModel.ApplyKopfdaten(model);

        //    return PartialView("Partial/FormKopf", model);
        //}

        [HttpPost]
        public ActionResult ShowDetails()
        {
            return PartialView("Partial/FormDetails", ViewModel.EsdAnforderung);
        }

        [HttpPost]
        public ActionResult EsdAnfordern(EsdAnforderung model)
        {
            if (ModelState.IsValid)
                ViewModel.EsdAnforderungAbsenden(model, ModelState.AddModelError);

            return PartialView("Partial/FormDetails", model);
        }

        [HttpPost]
        public ActionResult ShowReceipt()
        {
            return PartialView("Partial/Receipt", ViewModel);
        }
    }
}
