// ReSharper disable RedundantUsingDirective

using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective

namespace ServicesMvc.Controllers
{
    public partial class SendungenController : CkgDomainController 
    {
        public override sealed string DataContextKey { get { return GetDataContextKey<SendungenViewModel>(); } }

        public SendungenViewModel ViewModel { get { return GetViewModel<SendungenViewModel>(); } }


        public SendungenController(IAppSettings appSettings, ILogonContextDataService logonContext, IZulassungDataService zulassungDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, zulassungDataService);
        }

        [CkgApplication]
        public ActionResult Verfolgung()
        {
            ViewModel.DataMarkForRefresh();

            return View(ViewModel);
        }

        #region grid data export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.SendungenFiltered;
        }

        #endregion
    }
}
