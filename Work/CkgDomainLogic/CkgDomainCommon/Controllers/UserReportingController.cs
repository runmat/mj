using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.UserReporting.Contracts;
using CkgDomainLogic.UserReporting.Models;
using CkgDomainLogic.UserReporting.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public class UserReportingController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<UserReportingViewModel>(); } }

        public UserReportingViewModel ViewModel { get { return GetViewModel<UserReportingViewModel>(); } }


        public UserReportingController(IAppSettings appSettings, ILogonContextDataService logonContext, IUserReportingDataService dataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, dataService);
        }

        [CkgApplication]
        public ActionResult UserList()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadUserList(WebUserSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                if (ViewModel.LoadUserList(model))
                    if (ViewModel.Users.None())
                        ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/Suche", ViewModel.Suchparameter);
        }

        [HttpPost]
        public ActionResult ShowUserList()
        {
            return PartialView("Partial/Grid", ViewModel);
        }

        [GridAction]
        public ActionResult UserListAjaxBinding()
        {
            return View(new GridModel(ViewModel.UsersFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridUserList(string filterValue, string filterColumns)
        {
            ViewModel.FilterUsers(filterValue, filterColumns);

            return new EmptyResult();
        }


        #region Export

        public ActionResult ExportUserListFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.UsersFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.UserList, dt);

            return new EmptyResult();
        }

        public ActionResult ExportUserListFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.UsersFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.UserList, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
