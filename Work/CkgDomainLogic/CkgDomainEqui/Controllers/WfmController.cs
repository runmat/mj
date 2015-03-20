using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.WFM.Contracts;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class WfmController : CkgDomainController
    {
        public override string DataContextKey { get { return "WflViewModel"; } }

        public WfmController(IAppSettings appSettings, ILogonContextDataService logonContext, IWfmDataService wflDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, wflDataService);
        }
    }
}
