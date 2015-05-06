using System;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Services;
using Telerik.Web.Mvc;
using DocumentTools.Services;


namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Fahrzeug-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FahrzeugeController : CkgDomainController
    {
        public TreuhandbestandViewModel TreuhandbestandViewModel { get { return GetViewModel<TreuhandbestandViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportTreuhandbestand()
        {
            _dataContextKey = typeof(TreuhandbestandViewModel).Name;
            TreuhandbestandViewModel.DataMarkForRefresh();
            
            return View(TreuhandbestandViewModel);            
        }

        [HttpPost]
        public ActionResult LoadTreuhandbestand()
        {
            if (ModelState.IsValid)
            {
                TreuhandbestandViewModel.LoadTreuhandbestand();

                if (TreuhandbestandViewModel.Treuhandbestands.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Treuhandbestand/TreuhandbestandSuche", TreuhandbestandViewModel);
        }

        [HttpPost]
        public ActionResult ShowTreuhandbestand()
        {
            return PartialView("Treuhandbestand/TreuhandbestandGrid", TreuhandbestandViewModel);
        }

        [GridAction]
        public ActionResult TreuhandbestandAjaxBinding()
        {
            return View(new GridModel(TreuhandbestandViewModel.TreuhandbestandsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridTreuhandbestand(string filterValue, string filterColumns)
        {
            TreuhandbestandViewModel.FilterTreuhandbestands(filterValue, filterColumns);
            
            return new EmptyResult();
        }

        public ActionResult ExportTreuhandbestandFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = TreuhandbestandViewModel.TreuhandbestandsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Treuhandbestand", dt);

            return new EmptyResult();
        }

        public ActionResult ExportTreuhandbestandFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = TreuhandbestandViewModel.TreuhandbestandsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Treuhandbestand", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
