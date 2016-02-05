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
        public ActionResult MultiVerfolgung()
        {
            ViewModel.DataMarkForRefreshMulti();

            return View(ViewModel);
        }


        #region Sendungen, Suche nach ID

        [GridAction]
        public ActionResult SendungenIdAjaxBinding()
        {
            return View(new GridModel(ViewModel.SendungenIdFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridCocSendungenId(string filterValue, string filterColumns)
        {
            ViewModel.FilterSendungenId(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult LoadSendungenId(SendungsAuftragIdSelektor model)
        {
            ViewModel.SendungsAuftragIdSelektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadSendungenId(model, ModelState.AddModelError);

            return PartialView("VerfolgungMulti/Id/Suche", ViewModel.SendungsAuftragIdSelektor);
        }

        [HttpPost]
        public ActionResult ShowSendungenId()
        {
            return PartialView("VerfolgungMulti/Id/Grid", ViewModel);
        }

        #endregion


        #region Sendungen, Suche nach Docs

        [GridAction]
        public ActionResult SendungenDocsAjaxBinding()
        {
            return View(new GridModel(ViewModel.SendungenDocsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridCocSendungenDocs(string filterValue, string filterColumns)
        {
            ViewModel.FilterSendungenDocs(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult LoadSendungenDocs(SendungsAuftragDocsSelektor model)
        {
            ViewModel.SendungsAuftragDocsSelektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadSendungenDocs(model, ModelState.AddModelError);

            return PartialView("VerfolgungMulti/Docs/Suche", ViewModel.SendungsAuftragDocsSelektor);
        }

        [HttpPost]
        public ActionResult LoadSendungenFin(SendungsAuftragFinSelektor model)
        {
            ViewModel.SendungsAuftragFinSelektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadSendungenFin(model, ModelState.AddModelError);

            return PartialView("VerfolgungMulti/Fin/Suche", ViewModel.SendungsAuftragFinSelektor);
        }


        [HttpPost]
        public ActionResult ShowSendungenDocs()
        {
            return PartialView("VerfolgungMulti/Docs/Grid", ViewModel);
        }

        #endregion

    }
}
