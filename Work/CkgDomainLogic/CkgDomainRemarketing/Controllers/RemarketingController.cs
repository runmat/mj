using System.Web.Mvc;
using CkgDomainLogic.Archive.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Remarketing.Contracts;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.Remarketing.ViewModels;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class RemarketingController : CkgDomainController
    {
        public override string DataContextKey { get { return GetDataContextKey<FehlendeDatenViewModel>(); } }

        public RemarketingController(IAppSettings appSettings, 
            ILogonContextDataService logonContext, 
            IFehlendeDatenDataService fehlendeDatenDataService,
            IBelastungsanzeigenDataService belastungsanzeigenDataService,
            IEasyAccessDataService easyAccessDataService
            )
            : base(appSettings, logonContext)
        {
            InitViewModel(FehlendeDatenViewModel, appSettings, logonContext, fehlendeDatenDataService);
            InitViewModel(BelastungsanzeigenViewModel, appSettings, logonContext, belastungsanzeigenDataService, easyAccessDataService);

            InitModelStatics();
        }

        void InitModelStatics()
        {
            FehlendeDatenSelektor.GetViewModel = GetViewModel<FehlendeDatenViewModel>;
            BelastungsanzeigenSelektor.GetViewModel = GetViewModel<BelastungsanzeigenViewModel>;
        }

        public ActionResult Index(string un, string appID)
        {
            return RedirectToAction("ReportFehlendeDaten", new { un, appID });
        }
    }
}
