// ReSharper disable RedundantUsingDirective
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.Charts.Contracts;
using CkgDomainLogic.Charts.Models;
using CkgDomainLogic.Charts.ViewModels;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using Telerik.Web.Mvc;
// ReSharper restore RedundantUsingDirective

namespace ServicesMvc.Controllers
{
    public class XarisController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<XarisViewModel>(); } }

        public XarisViewModel ViewModel { get { return GetViewModel<XarisViewModel>(); } }


        public XarisController(IAppSettings appSettings, ILogonContextDataService logonContext)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext);
        }


        [CkgApplication]
        public ActionResult Index()
        {
            if (LogonContext.MvcEnforceRawLayout)
                return RedirectPermanent(ViewModel.XarisUrl);

            return View(ViewModel);
        }
    }
}
