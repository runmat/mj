using System.Web.Mvc;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Equi.ViewModels;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class EquipController  
    {
        public EinAusgaengeViewModel EinAusgaengeViewModel { get { return GetViewModel<EinAusgaengeViewModel>(); } }

        [CkgApplication]
        public ActionResult EinAusgaenge()
        {
            MainViewModel = EinAusgaengeViewModel;
            EinAusgaengeViewModel.DataMarkForRefresh();

            return View(EinAusgaengeViewModel);
        }

        [GridAction]
        public ActionResult EinAusgaengeAjaxBinding()
        {
            return View(new GridModel(EinAusgaengeViewModel.EinAusgaengeFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridEinAusgaenge(string filterValue, string filterColumns)
        {
            EinAusgaengeViewModel.FilterEinAusgaenge(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult LoadEinAusgaenge(EinAusgangSelektor model)
        {
            EinAusgaengeViewModel.EinAusgangSelektor = model;

            if (ModelState.IsValid)
                EinAusgaengeViewModel.LoadEinAusgaenge(model, ModelState.AddModelError);

            return PartialView("EinAusgaenge/Suche", EinAusgaengeViewModel.EinAusgangSelektor);
        }

        [HttpPost]
        public ActionResult ShowEinAusgaenge()
        {
            return PartialView("EinAusgaenge/Grid", EinAusgaengeViewModel);
        }
    }
}
