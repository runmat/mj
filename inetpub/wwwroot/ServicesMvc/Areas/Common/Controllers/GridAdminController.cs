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
        public ActionResult Edit(string columnMember)
        {
            ViewModel.CurrentCustomerID = LogonContext.KundenNr.ToInt();

            var gridCurrentModelType = (SessionHelper.GetSessionObject("Telerik_Grid_CurrentModelTypeForAutoPersistColumns", () => null) as Type);
            if (gridCurrentModelType == null)
                return new EmptyResult();

            if (!ViewModel.DataInit(gridCurrentModelType, columnMember))
                return new EmptyResult();

            return PartialView("Partial/Edit", ViewModel);
        }

        [HttpPost]
        public ActionResult EditGridColumnTranslations(GridAdminViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("Partial/Edit", model);

            ViewModel.DataSave(model);
            if (model.TmpDeleteCustomerTranslation)
                ModelState.Clear();

            return PartialView("Partial/Edit", model);
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
        public ActionResult Test()
        {
            ViewModel.ReportSettings = new ReportSolution
                {
                    AdminIsAuthorized = true,
                    AdminUserName = "JenzenM",
                    AppID = 1731,
                };

            return View(ViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ReportSolution(string un)
        {
            ViewModel.TrySetReportSettings(CryptoMd5.Decrypt(un));
            
            return View(ViewModel);
        }

        void TryUserLogon(string userName)
        {
            var orgDataService = ViewModel.DataService;
            var orgAppSettings = ViewModel.AppSettings;

            var vmOrg = ViewModel;
            UrlLogOn(userName, null, null);
            ViewModel = vmOrg;

            InitViewModel(ViewModel, orgAppSettings, LogonContext, orgDataService);
        }
    }
}
