// ReSharper disable RedundantUsingDirective

using System;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using System.Linq;
using GeneralTools.Models;
using MvcTools.Web;
using ServicesMvc.DomainCommon.Models;
using WebTools.Services;

namespace ServicesMvc.Common.Controllers
{
    [AllowAnonymous]
    public class GridAdminController : CkgDomainController
    {
        public override string DataContextKey { get { return "GridAdminViewModel"; } }

        public GridAdminViewModel ViewModel
        {
            get { return GetViewModel<GridAdminViewModel>(); }
            set { SetViewModel(value); }
        }

        public GridAdminController(IAppSettings appSettings, ILogonContextDataService logonContext, IGridAdminDataService gridAdminDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, gridAdminDataService);
        }


        [HttpPost]
        public ActionResult EditTranslations(string modelTypeName, string propertyName, string partialViewContext)
        {
            if (partialViewContext.IsNotNullOrEmpty())
                SessionHelper.SetSessionValue("PartialViewContextCurrent", partialViewContext); 

            ViewModel.Mode = (modelTypeName == null ? GridAdminMode.GridColumns : GridAdminMode.FormControls);

            var currentModelType = ViewModel.Mode == GridAdminMode.GridColumns
                                            ? (SessionHelper.GetSessionObject("Telerik_Grid_CurrentModelTypeForAutoPersistColumns", () => null) as Type)
                                            : Type.GetType(modelTypeName);
            if (currentModelType == null)
                return new EmptyResult();

            if (!ViewModel.DataInit(currentModelType, propertyName))
                return PartialView("Partial/NoTranslationAvailable", ViewModel);

            return PartialView("Partial/EditTranslations", ViewModel);
        }

        [HttpPost]
        public ActionResult EditTranslationsForm(GridAdminViewModel model)
        {
            if (model.TmpSwitchGlobalFlag)
            {
                ViewModel.IsGlobal = model.IsGlobal;
                ViewModel.TmpSwitchGlobalFlag = model.TmpSwitchGlobalFlag;
                ViewModel.LoadTranslatedResourcesForProperty();
                ModelState.Clear();
                return PartialView("Partial/EditTranslations", ViewModel);
            }

            if (!ModelState.IsValid)
                return PartialView("Partial/EditTranslations", ViewModel);

            ModelMapping.CopyPropertiesTo(model, ViewModel);

            ViewModel.DataSave();

            if (ViewModel.TmpDeleteCustomerTranslation)
                ModelState.Clear();

            return PartialView("Partial/EditTranslations", ViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult OnCustomerChanged(int customerId)
        {
            ViewModel.LoadUserForCustomer(customerId);
            if (ViewModel.Users.None())
                return Json(new { success = false });

            return Json(new
            {
                success = true,
                users = ViewModel.Users.Select(user => new
                {
                    ID = user.UserID,
                    Name = user.Username
                })
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult OnUserChanged(int userId)
        {
            ViewModel.SetCurrentUser(userId);

            return new EmptyResult();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ReportSolutionReroute()
        {
            var currentValidUser = ViewModel.GetCurrentValidUser();

            TryUserLogon(currentValidUser.Username);

            return Json(new { url = ViewModel.GetRelativeAppUrl() });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ReportSolution(string un)
        {
            TryUserLogoff();

            if (!ViewModel.TrySetReportSettings(CryptoMd5.Decrypt(un)))
                LogonContext.MvcEnforceRawLayout = true;
            
            return View(ViewModel);
        }

        private void TryUserLogoff()
        {
            TryUserLogonLogoffInner(UrlLogOff);
        }

        private void TryUserLogon(string userName)
        {
            TryUserLogonLogoffInner(() => UrlLogOn(userName, null, null));
            GridSettingsAdminMode = true;
        }

        void TryUserLogonLogoffInner(Action func)
        {
            var orgDataService = ViewModel.DataService;
            var orgAppSettings = ViewModel.AppSettings;

            var vmOrg = ViewModel;

            func();

            ViewModel = vmOrg;

            LogonContext.UserNameForDisplay = "Report-Solution";
            LogonContext.MvcEnforceRawLayout = false;

            InitViewModel(ViewModel, orgAppSettings, LogonContext, orgDataService);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult TestStrafzettel()
        {
            TryUserLogoff();

            ViewModel.ReportSettings = new ReportSolution
            {
                AdminIsAuthorized = true,
                AdminUserName = "JenzenM",
                AppID = 1731,
                AppFriendlyName = "Strafzettel-Report"
            };

            return View("Test", ViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult TestZulassung()
        {
            TryUserLogoff();

            ViewModel.ReportSettings = new ReportSolution
            {
                AdminIsAuthorized = true,
                AdminUserName = "JenzenM",
                AppID = 1806,
                AppFriendlyName = "Autohaus-Zulassung"
            };

            return View("Test", ViewModel);
        }

    }
}
