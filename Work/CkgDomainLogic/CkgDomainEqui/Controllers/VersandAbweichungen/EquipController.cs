using System;
using System.Linq;
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
        public VersandAbweichungenViewModel VersandAbweichungenViewModel { get { return GetViewModel<VersandAbweichungenViewModel>(); } }

        [CkgApplication]
        public ActionResult VersandAbweichungen()
        {
            MainViewModel = VersandAbweichungenViewModel;
            VersandAbweichungenViewModel.DataMarkForRefresh();

            return View(VersandAbweichungenViewModel);
        }

        [GridAction]
        public ActionResult VersandAbweichungenAjaxBinding()
        {
            return View(new GridModel(VersandAbweichungenViewModel.VersandAbweichungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridVersandAbweichungen(string filterValue, string filterColumns)
        {
            VersandAbweichungenViewModel.FilterVersandAbweichungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult LoadVersandAbweichungen(VersandAbweichungSelektor model)
        {
            VersandAbweichungenViewModel.VersandAbweichungSelektor = model;

            if (ModelState.IsValid)
                VersandAbweichungenViewModel.LoadVersandAbweichungen(ModelState.AddModelError);

            return PartialView("VersandAbweichungen/Suche", VersandAbweichungenViewModel.VersandAbweichungSelektor);
        }

        [HttpPost]
        public ActionResult ShowVersandAbweichungen()
        {
            return PartialView("VersandAbweichungen/Grid", VersandAbweichungenViewModel);
        }

        [HttpPost]
        public ActionResult VersandAbweichungMemoEditFormShow(string id)
        {
            var model = VersandAbweichungenViewModel.VersandAbweichungenFiltered.FirstOrDefault(a => a.Equipmentnummer == id);
            if (model == null) 
                return new EmptyResult();

            return PartialView("VersandAbweichungen/MemoEditForm", model);
        }

        [HttpPost]
        public ActionResult VersandAbweichungMemoEditFormSave(Fahrzeugbrief model)
        {
            if (ModelState.IsValid)
                VersandAbweichungenViewModel.SaveMemo(model, ModelState.AddModelError);

            return PartialView("VersandAbweichungen/MemoEditForm", model);
        }

        [HttpPost]
        public ActionResult VersandAbweichungSaveAsErledigt(string id)
        {
            var errorMessage = "";
            VersandAbweichungenViewModel.SaveAsErledigt(id, (key, error) => errorMessage = error);
            if (errorMessage.IsNotNullOrEmpty())
                throw new Exception(errorMessage);

            return new EmptyResult();
        }
    }
}
