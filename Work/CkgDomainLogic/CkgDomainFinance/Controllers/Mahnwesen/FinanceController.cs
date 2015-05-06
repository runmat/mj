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
        public TempZb2VersandViewModel TempZb2VersandViewModel { get { return GetViewModel<TempZb2VersandViewModel>(); } }

        [CkgApplication]
        public ActionResult TempZb2Versand()
        {
            return View(TempZb2VersandViewModel);
        }

        [HttpPost]
        public ActionResult LoadTempZb2Versand(TempZb2VersandViewModel model)
        {
            if (ModelState.IsValid)
            {                
                if (TempZb2VersandViewModel.TempZb2Versands.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("MahnwesenTempZb2Versand/TempZb2VersandSuche", model);
        }

        [HttpPost]
        public ActionResult ShowTempZb2Versand()
        {
            return PartialView("MahnwesenTempZb2Versand/TempZb2VersandGrid", TempZb2VersandViewModel);
        }

        [GridAction]
        public ActionResult TempZb2VersandAjaxBinding()
        {
            return View(new GridModel(TempZb2VersandViewModel.TempZb2VersandsFiltered));
        }

        [HttpPost]
        public ActionResult FilterTempZb2VersandGrid(string filterValue, string filterColumns)
        {
            TempZb2VersandViewModel.FilterTempZb2Versands(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportTempZb2VersandFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = TempZb2VersandViewModel.TempZb2VersandsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("TempZb2Versand", dt);

            return new EmptyResult();
        }

        public ActionResult ExportTempZb2VersandFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = TempZb2VersandViewModel.TempZb2VersandsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("TempZb2Versand", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
