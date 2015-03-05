using System.Web.Mvc;
using CkgDomainLogic.Zanf.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class ZanfController : CkgDomainController
    {
        public override string DataContextKey { get { return "ZanfReportViewModel"; } }

        public ZanfController(
            IAppSettings appSettings,
            ILogonContextDataService logonContext,
            IZanfReportDataService zanfReportDataService,
            IZulassungsunterlagenDataService zulassungsunterlagenDataService
            )
            : base(appSettings, logonContext)
        {
            InitViewModel(ZanfReportViewModel, appSettings, logonContext, zanfReportDataService);
            InitViewModel(ZulassungsunterlagenViewModel, appSettings, logonContext, zulassungsunterlagenDataService);
        }

        public ActionResult Index()
        {
            return RedirectToAction("ZanfReport");
        }
    }
}
