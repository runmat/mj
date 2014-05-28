using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Archive.Contracts;
using CkgDomainLogic.Archive.ViewModels;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Archive-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class ArchiveController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<EasyAccessViewModel>(); } }

        public ArchiveController(IAppSettings appSettings, ILogonContextDataService logonContext, IEasyAccessDataService dataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(EasyViewModel, appSettings, logonContext, dataService);
        }

        public ActionResult Index(string un, string appID)
        {
            return RedirectToAction("OptischesArchiv", new { un, appID });
        }

    
    }
}
