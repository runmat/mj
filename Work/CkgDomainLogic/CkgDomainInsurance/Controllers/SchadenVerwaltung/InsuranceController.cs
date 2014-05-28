using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.ViewModels;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Insurance-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class InsuranceController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<VersEventsViewModel>(); } }

        public InsuranceController(IAppSettings appSettings, ILogonContextDataService logonContext, 
            ICustomerDocumentDataService customerDocumentDataService, IVersEventsDataService versEventsDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(EventsViewModel, appSettings, logonContext, versEventsDataService);
            InitModelStatics();

            InitViewModel(SchadenakteViewModel, appSettings, logonContext, versEventsDataService);
            
            if (SchadenakteViewModel.DocsViewModel == null)
                SchadenakteViewModel.DocsViewModel = new SchadenakteDocsViewModel();
            InitViewModel(SchadenakteViewModel.DocsViewModel, appSettings, logonContext, customerDocumentDataService);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Konfigurator");
        }
    }
}
