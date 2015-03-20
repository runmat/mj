using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.WFL.Contracts;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class WflController : CkgDomainController
    {
        public override string DataContextKey { get { return "WflViewModel"; } }

        public WflController(IAppSettings appSettings, ILogonContextDataService logonContext, IWflDataService wflDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, wflDataService);
        }
    }
}
