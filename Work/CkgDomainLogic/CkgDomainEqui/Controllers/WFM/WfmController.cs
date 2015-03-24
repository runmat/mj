using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.WFM.Models;
using CkgDomainLogic.WFM.ViewModels;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class WfmController 
    {
        public WfmViewModel ViewModel { get { return GetViewModel<WfmViewModel>(); } }

        [CkgApplication]
        public ActionResult Abmeldevorgaenge()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadAuftraege(WfmAuftragSelektor model)
        {
            ViewModel.Selektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadAuftraege(ModelState);

            return PartialView("Partial/AuftraegeSuche", ViewModel.Selektor);
        }

        [HttpPost]
        public ActionResult ShowAuftraege()
        {
            return PartialView("Partial/AuftraegeGrid", ViewModel);
        }

        [GridAction]
        public ActionResult AuftraegeAjaxBinding()
        {
            return View(new GridModel(ViewModel.AuftraegeFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridAuftraege(string filterValue, string filterColumns)
        {
            ViewModel.FilterAuftraege(filterValue, filterColumns);

            return new EmptyResult();
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.AuftraegeFiltered;
        }

        #endregion
    }
}
