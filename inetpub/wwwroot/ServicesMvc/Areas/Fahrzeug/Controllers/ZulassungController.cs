using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Contracts;

namespace ServicesMvc.Fahrzeug.Controllers
{
    public class ZulassungController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<ZulassungViewModel>(); } }

        public ZulassungViewModel ViewModel 
        { 
            get { return GetViewModel<ZulassungViewModel>(); } 
            set { SetViewModel(value); } 
        }

        public ZulassungController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IFahrzeugeDataService fahrzeugeDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, fahrzeugeDataService);
            InitModelStatics();
        }

        [CkgApplication]
        public ActionResult Index()
        {
            return View(ViewModel);
        }

        static void InitModelStatics()
        {
        }
    }
}
