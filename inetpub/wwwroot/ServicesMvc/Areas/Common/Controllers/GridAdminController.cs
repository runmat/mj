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

namespace ServicesMvc.Common.Controllers
{
    public class GridAdminController : CkgDomainController
    {
        public override string DataContextKey { get { return "GridAdminViewModel"; } }

        public GridAdminViewModel ViewModel { get { return GetViewModel<GridAdminViewModel>(); } }


        public GridAdminController(IAppSettings appSettings, ILogonContextDataService logonContext, IGridAdminDataService gridAdminDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, gridAdminDataService);
        }


        [HttpPost]
        public ActionResult Edit(string columnMember)
        {
            // ToDo: Change this to the administrated(!) Customer ID
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
    }
}
