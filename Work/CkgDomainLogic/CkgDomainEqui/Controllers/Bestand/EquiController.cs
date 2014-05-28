using System;
using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using Telerik.Web.Mvc;
using CkgDomainLogic.Equi.Models;

namespace ServicesMvc.Controllers
{
    public partial class EquiController
    {
        public EquiGrunddatenViewModel EquiGrunddatenEquiViewModel { get { return GetViewModel<EquiGrunddatenViewModel>(); }   }

        [CkgApplication]
        public ActionResult ReportFahrzeugbestand()
        {
            EquiGrunddatenEquiViewModel.DataInit();

            return View(EquiGrunddatenEquiViewModel);
        }

        [HttpPost]
        public ActionResult LoadGrunddatenEquis(GrunddatenEquiSuchparameter model)
        {
            EquiGrunddatenEquiViewModel.Suchparameter = model;

            EquiGrunddatenEquiViewModel.CheckInput(ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                EquiGrunddatenEquiViewModel.LoadEquis();
                if (EquiGrunddatenEquiViewModel.GrunddatenEquis.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Bestand/EquiSuche", EquiGrunddatenEquiViewModel.Suchparameter);
        }

        [HttpPost]
        public ActionResult ShowGrunddatenEquis()
        {
            return PartialView("Bestand/EquiGrid", EquiGrunddatenEquiViewModel);
        }

        [GridAction]
        public ActionResult GrunddatenEquiItemsAjaxBinding()
        {
            return View(new GridModel(EquiGrunddatenEquiViewModel.GrunddatenEquisFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridGrunddatenEquiItems(string filterValue, string filterColumns)
        {
            EquiGrunddatenEquiViewModel.FilterEquis(filterValue, filterColumns);

            return new EmptyResult();
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return EquiGrunddatenEquiViewModel.GrunddatenEquisFiltered;
        }

        #endregion
    }
}
