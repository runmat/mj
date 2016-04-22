using System;
using System.Web.Mvc;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Services;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Equi-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class EquiController  
    {
        public ErweiterterBriefbestandViewModel ErweiterterBriefbestandViewModel { get { return GetViewModel<ErweiterterBriefbestandViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportErweiterterBriefbestand()
        {
            return View(ErweiterterBriefbestandViewModel);
        }

        [HttpPost]
        public ActionResult LoadFahrzeugbriefeErweitert(FahrzeugbriefSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                ErweiterterBriefbestandViewModel.LoadFahrzeugbriefe(model);
                if (ErweiterterBriefbestandViewModel.Fahrzeugbriefe.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("ErweiterterBriefbestand/FahrzeugbriefErweitertSuche", model);
        }

        [HttpPost]
        public ActionResult ShowFahrzeugbriefeErweitert()
        {
            return PartialView("ErweiterterBriefbestand/FahrzeugbriefeErweitertGrid", ErweiterterBriefbestandViewModel);
        }

        [GridAction]
        public ActionResult FahrzeugbriefeErweitertAjaxBinding()
        {
            var items = ErweiterterBriefbestandViewModel.FahrzeugbriefeFiltered; 

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult ItemEdit(string fin)
        {
            ModelState.Clear();
            return PartialView("ErweiterterBriefbestand/DetailsForm", ErweiterterBriefbestandViewModel.GetItem(fin));
        }

        [HttpPost]
        public ActionResult DetailsFormSave(FahrzeugbriefErweitert model)
        {
            if (ModelState.IsValid)
                model.SAPError = ErweiterterBriefbestandViewModel.SaveItem(model);

            return PartialView("ErweiterterBriefbestand/DetailsForm", model);
        }

        #region Filter & Export

        [HttpPost]
        public ActionResult FilterGridFahrzeugbriefeErweitert(string filterValue, string filterColumns)
        {
            ErweiterterBriefbestandViewModel.FilterFahrzeugbriefe(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugbriefeErweitertFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ErweiterterBriefbestandViewModel.FahrzeugbriefeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Briefbestand", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugbriefeErweitertFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ErweiterterBriefbestandViewModel.FahrzeugbriefeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Briefbestand", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

    }
}
