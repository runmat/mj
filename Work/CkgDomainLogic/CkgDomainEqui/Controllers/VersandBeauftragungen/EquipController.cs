using System;
using System.Web.Mvc;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Equi.ViewModels;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class EquipController  
    {
        public VersandBeauftragungenViewModel VersandBeauftragungenViewModel { get { return GetViewModel<VersandBeauftragungenViewModel>(); } }

        [CkgApplication]
        public ActionResult VersandBeauftragungen()
        {
            MainViewModel = VersandBeauftragungenViewModel;
            VersandBeauftragungenViewModel.DataMarkForRefresh();

            return View(VersandBeauftragungenViewModel);
        }

        [GridAction]
        public ActionResult VersandBeauftragungenAjaxBinding()
        {
            return View(new GridModel(VersandBeauftragungenViewModel.VersandBeauftragungenFiltered));
        }

        [HttpPost]
        public JsonResult VersandBeauftragungenSelectionChanged(string id, bool isChecked)
        {
            int allSelectionCount;
            VersandBeauftragungenViewModel.SelectVersandBeauftragung(id, isChecked, out allSelectionCount);

            return Json(new { allSelectionCount });
        }

        [HttpPost]
        public JsonResult VersandBeauftragungenDeleteSelectedRecords()
        {
            var errorMessage = VersandBeauftragungenViewModel.DeleteSelectedVersandBeauftragungen();
            if (errorMessage.IsNotNullOrEmpty())
                throw new Exception(errorMessage);

            VersandBeauftragungenViewModel.DataMarkForRefresh();

            return Json(new {  });
        }

        [HttpPost]
        public ActionResult FilterGridVersandBeauftragungen(string filterValue, string filterColumns)
        {
            VersandBeauftragungenViewModel.FilterVersandBeauftragungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult LoadVersandBeauftragungen(VersandBeauftragungSelektor model)
        {
            VersandBeauftragungenViewModel.VersandBeauftragungSelektor = model;

            if (ModelState.IsValid)
                VersandBeauftragungenViewModel.LoadVersandBeauftragungen(model);

            return PartialView("VersandBeauftragungen/Suche", VersandBeauftragungenViewModel.VersandBeauftragungSelektor);
        }

        [HttpPost]
        public ActionResult ShowVersandBeauftragungen()
        {
            return PartialView("VersandBeauftragungen/Grid", VersandBeauftragungenViewModel);
        }
    }
}
