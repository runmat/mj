// ReSharper disable RedundantUsingDirective
using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.CoC.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;
// ReSharper restore RedundantUsingDirective

namespace ServicesMvc.Controllers
{
    public partial class CocSendungController 
    {
        [CkgApplication]
        public ActionResult Verfolgung()
        {
            ViewModel.DataMarkForRefresh();

            return View(ViewModel);
        }

        [GridAction]
        public ActionResult SendungenAjaxBinding()
        {
            return View(new GridModel(ViewModel.SendungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridCocSendungen(string filterValue, string filterColumns)
        {
            ViewModel.FilterSendungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult LoadSendungen(SendungsAuftragSelektor model)
        {
            ViewModel.SendungsAuftragSelektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadSendungen(model, ModelState.AddModelError);

            return PartialView("Verfolgung/Suche", ViewModel.SendungsAuftragSelektor);
        }

        [HttpPost]
        public ActionResult ShowSendungen()
        {
            return PartialView("Verfolgung/Grid", ViewModel);
        }


        #region grid data export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.SendungenFiltered;
        }

        #endregion
    }
}
