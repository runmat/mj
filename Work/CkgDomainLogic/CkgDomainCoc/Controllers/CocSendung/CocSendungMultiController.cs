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
        public ActionResult VerfolgungMulti()
        {
            ViewModel.DataMarkForRefreshMulti();

            return View(ViewModel);
        }


        #region Sendungen, Suche nach ID

        [GridAction]
        public ActionResult SendungenIdAjaxBinding()
        {
            return View(new GridModel(ViewModel.SendungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridCocSendungenId(string filterValue, string filterColumns)
        {
            ViewModel.FilterSendungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult LoadSendungenId(SendungsAuftragIdSelektor model)
        {
            ViewModel.SendungsAuftragIdSelektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadSendungenId(model, ModelState.AddModelError);

            return PartialView("VerfolgungMulti/Suche", ViewModel.SendungsAuftragIdSelektor);
        }

        [HttpPost]
        public ActionResult ShowSendungenId()
        {
            return PartialView("VerfolgungMulti/Grid", ViewModel);
        }

        #endregion

    }
}
