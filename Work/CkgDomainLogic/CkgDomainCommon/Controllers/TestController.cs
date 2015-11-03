using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

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

        [CkgApplication]
        public ActionResult Test()
        {
            return View();
        }
    }
}
