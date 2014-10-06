// ReSharper disable RedundantUsingDirective
using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.CoC.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;
// ReSharper restore RedundantUsingDirective

namespace ServicesMvc.Controllers
{
    public partial class CocSendungController : CkgDomainController 
    {
        public override sealed string DataContextKey { get { return GetDataContextKey<SendungenViewModel>(); } }

        public SendungenViewModel ViewModel { get { return GetViewModel<SendungenViewModel>(); } }


        public CocSendungController(IAppSettings appSettings, ILogonContextDataService logonContext, IZulassungDataService zulassungDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, zulassungDataService);
        }
    }
}
