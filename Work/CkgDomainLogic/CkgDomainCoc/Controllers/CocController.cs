using System.Web.Mvc;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class CocController : CkgDomainController
    {
        public override string DataContextKey { get { return "CocViewModel"; } }

        public CocTypenViewModel CocTypenViewModel { get { return GetViewModel<CocTypenViewModel>(); } }

        public CocErfassungViewModel CocErfassungViewModel { get { return GetViewModel<CocErfassungViewModel>(); } }

        public CocReportsViewModel CocReportsViewModel { get { return GetViewModel<CocReportsViewModel>(); } }
        


        public CocController(IAppSettings appSettings, ILogonContextDataService logonContext, ICocTypenDataService typenDataService, ICocErfassungDataService erfassungsDataService) 
            : base(appSettings, logonContext)
        {
            InitViewModel(CocTypenViewModel, appSettings, logonContext, typenDataService);
            InitViewModel(CocErfassungViewModel, appSettings, logonContext, erfassungsDataService);
            InitViewModel(CocReportsViewModel, appSettings, logonContext, erfassungsDataService);
        }


        public ActionResult Index(string un, string appID)
        {
            return RedirectToAction("TypenVerwaltung", new { un, appID });
        }
    }
}
