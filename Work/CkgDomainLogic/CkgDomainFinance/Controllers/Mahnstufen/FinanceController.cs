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
        public FinanceMahnstufenViewModel MahnstufenViewModel { get { return GetViewModel<FinanceMahnstufenViewModel>(); } }

        [CkgApplication]
        public ActionResult Mahnstufen()
        {
            return View(MahnstufenViewModel);
        }

        [HttpPost]
        public ActionResult LoadMahnungen(MahnungSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                MahnstufenViewModel.LoadMahnungen(model);
                if (MahnstufenViewModel.Mahnungen.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Mahnstufen/MahnungenSuche", model);
        }

        [HttpPost]
        public ActionResult ShowMahnungen()
        {
            return PartialView("Mahnstufen/MahnungenGrid", MahnstufenViewModel);
        }

        [GridAction]
        public ActionResult MahnungenAjaxBinding()
        {
            return View(new GridModel(MahnstufenViewModel.MahnungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridMahnungen(string filterValue, string filterColumns)
        {
            MahnstufenViewModel.FilterMahnungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportMahnungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = MahnstufenViewModel.MahnungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Mahnstufen", dt);

            return new EmptyResult();
        }

        public ActionResult ExportMahnungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = MahnstufenViewModel.MahnungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Mahnstufen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
