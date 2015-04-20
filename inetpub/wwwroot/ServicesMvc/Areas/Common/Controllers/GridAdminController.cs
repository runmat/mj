// ReSharper disable RedundantUsingDirective
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using System.Linq;
using GeneralTools.Models;

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
            ViewModel.DataInit();

            return PartialView("Partial/Edit", columnMember);
        }
    }
}
