// ReSharper disable RedundantUsingDirective
using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using CkgDomainLogic.Uebfuehrg.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective

namespace ServicesMvc.Controllers
{
    public class UebfuehrgController : CkgDomainController
    {
        public override string DataContextKey { get { return "UebfuehrgViewModel"; } }

        public UebfuehrgViewModel ViewModel { get { return GetViewModel<UebfuehrgViewModel>(); } }


        public UebfuehrgController(IAppSettings appSettings, ILogonContextDataService logonContext, IUebfuehrgDataService uebfuehrgDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, uebfuehrgDataService);
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }
    }
}
