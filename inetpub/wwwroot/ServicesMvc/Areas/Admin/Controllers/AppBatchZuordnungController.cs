using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.Admin.Contracts;
using CkgDomainLogic.Admin.Models;
using CkgDomainLogic.Admin.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Database.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Admin.Controllers
{
    public class AppBatchZuordnungController : CkgDomainController
    {
        public override string DataContextKey { get { return "AppBatchZuordnungViewModel"; } }

        public AppBatchZuordnungViewModel ViewModel { get { return GetViewModel<AppBatchZuordnungViewModel>(); } }

        public AppBatchZuordnungController(IAppSettings appSettings, ILogonContextDataService logonContext, IAppBatchZuordnungDataService appBatchZuordnungDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, appBatchZuordnungDataService);

            InitModelStatics();
        }

        void InitModelStatics()
        {
            AppBatchZuordnungSelektor.GetViewModel = GetViewModel<AppBatchZuordnungViewModel>;
        }

        [CkgApplication(AdminLevel.Master)]
        public ActionResult Index() 
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadAppZuordnungen(AppBatchZuordnungSelektor model)
        {
            ViewModel.AppBatchZuordnungSelektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadAppZuordnungen();

            return PersistablePartialView("Partial/Suche", ViewModel.AppBatchZuordnungSelektor);
        }

        [HttpPost]
        public ActionResult ShowAppZuordnungen()
        {
            return PartialView("Partial/Grid", ViewModel);
        }

        [GridAction]
        public ActionResult AppZuordnungenAjaxBinding()
        {
            return View(new GridModel(ViewModel.AppZuordnungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridAppZuordnungen(string filterValue, string filterColumns)
        {
            ViewModel.FilterAppZuordnungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AppZuordnungenSelectionChanged(string id, bool isChecked)
        {
            if (id.IsNullOrEmpty())
                ViewModel.ChangeZuordnungen(isChecked);
            else
                ViewModel.ChangeZuordnung(id, isChecked);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveAppZuordnungen()
        {
            string message;

            var success = ViewModel.SaveZuordnungen(out message);

            return Json(new { message, success });
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.AppZuordnungenFiltered;
        }

        #endregion
    }
}
