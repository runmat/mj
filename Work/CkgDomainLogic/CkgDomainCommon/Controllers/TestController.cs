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
    public class TestController : CkgDomainController 
    {
        public override string DataContextKey { get { return "Test"; } }

        public TestController(IAppSettings appSettings, ILogonContextDataService logonContext)
            : base(appSettings, logonContext)
        {
        }

        [CkgApplication]
        public ActionResult Cerberus()
        {
            return View();
        }
    }
}
