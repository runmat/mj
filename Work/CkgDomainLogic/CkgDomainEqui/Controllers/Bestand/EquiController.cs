using System;
using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using CkgDomainLogic.Equi.Models;

namespace ServicesMvc.Controllers
{
    public partial class EquiController
    {
        public EquiGrunddatenViewModel EquiGrunddatenViewModel { get { return GetViewModel<EquiGrunddatenViewModel>(); }   }

        [CkgApplication]
        public ActionResult ReportFahrzeugbestand()
        {
            EquiGrunddatenViewModel.DataInit();

            return View(EquiGrunddatenViewModel);
        }

        [HttpPost]
        public ActionResult LoadGrunddatenEquis(EquiGrunddatenSelektor model)
        {
            EquiGrunddatenViewModel.Selektor = model;

            EquiGrunddatenViewModel.CheckInput(ModelState.AddModelError);

            if (ModelState.IsValid && !ReportGeneratorMode)
            {
                EquiGrunddatenViewModel.LoadEquis();
                if (EquiGrunddatenViewModel.GrunddatenEquis.None())
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
            }

            return PersistablePartialView("Bestand/EquiSuche", EquiGrunddatenViewModel.Selektor);
        }

        [HttpPost]
        public ActionResult ShowGrunddatenEquis()
        {
            return PartialView("Bestand/EquiGrid", EquiGrunddatenViewModel);
        }

        [GridAction]
        public ActionResult GrunddatenEquiItemsAjaxBinding()
        {
            return View(new GridModel(EquiGrunddatenViewModel.GrunddatenEquisFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridGrunddatenEquiItems(string filterValue, string filterColumns)
        {
            EquiGrunddatenViewModel.FilterEquis(filterValue, filterColumns);

            return new EmptyResult();
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return EquiGrunddatenViewModel.GrunddatenEquisFiltered;
        }

        #endregion
    }
}
