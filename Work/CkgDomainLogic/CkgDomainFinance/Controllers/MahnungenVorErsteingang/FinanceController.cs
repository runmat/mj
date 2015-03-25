using System;
using System.Web.Mvc;
using CkgDomainLogic.Finance.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Finance.ViewModels;
using CkgDomainLogic.General.Services;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Finance-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FinanceController  
    {
        public FinanceMahnungenVorErsteingangViewModel MahnungenVorErsteingangViewModel { get { return GetViewModel<FinanceMahnungenVorErsteingangViewModel>(); } }

        [CkgApplication]
        public ActionResult MahnungenVorErsteingang()
        {
            MahnungenVorErsteingangViewModel.FillVertragsarten();

            return View(MahnungenVorErsteingangViewModel);
        }

        [HttpPost]
        public ActionResult LoadMahnungenVorErsteingang(MahnungVorErsteingangSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                MahnungenVorErsteingangViewModel.LoadMahnungen(model);
                if (MahnungenVorErsteingangViewModel.Mahnungen.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            model.AuswahlVertragsart = MahnungenVorErsteingangViewModel.DataService.Suchparameter.AuswahlVertragsart;

            return PartialView("MahnungenVorErsteingang/MahnungenVorErsteingangSuche", model);
        }

        [HttpPost]
        public ActionResult ShowMahnungenVorErsteingang()
        {
            return PartialView("MahnungenVorErsteingang/MahnungenVorErsteingangGrid", MahnungenVorErsteingangViewModel);
        }

        [GridAction]
        public ActionResult MahnungenVorErsteingangAjaxBinding()
        {
            return View(new GridModel(MahnungenVorErsteingangViewModel.MahnungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridMahnungenVorErsteingang(string filterValue, string filterColumns)
        {
            MahnungenVorErsteingangViewModel.FilterMahnungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportMahnungenVorErsteingangFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = MahnungenVorErsteingangViewModel.MahnungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Mahnungen", dt);

            return new EmptyResult();
        }

        public ActionResult ExportMahnungenVorErsteingangFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = MahnungenVorErsteingangViewModel.MahnungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Mahnungen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
