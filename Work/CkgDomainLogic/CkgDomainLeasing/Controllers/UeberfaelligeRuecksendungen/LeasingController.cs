using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Leasing.Models;
using CkgDomainLogic.Leasing.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;
using CkgDomainLogic.General.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Leasing-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class LeasingController  
    {
        public UeberfaelligeRuecksendungenViewModel UeberfaelligeRuecksendungenViewModel { get { return GetViewModel<UeberfaelligeRuecksendungenViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportUeberfaelligeRuecksendungen()
        {
            return View(UeberfaelligeRuecksendungenViewModel);
        }

        [HttpPost]
        public ActionResult LoadUeberfaelligeRuecksendungen(UeberfaelligeRuecksendungenSuchparameter model)
        {
            if (ModelState.IsValid)
                UeberfaelligeRuecksendungenViewModel.LoadUeberfaelligeRuecksendungen(model, ModelState);

            return PartialView("UeberfaelligeRuecksendungen/Suche", model);
        }

        [HttpPost]
        public ActionResult ShowUeberfaelligeRuecksendungen()
        {
            return PartialView("UeberfaelligeRuecksendungen/Grid", UeberfaelligeRuecksendungenViewModel);
        }

        [GridAction]
        public ActionResult UeberfaelligeRuecksendungenAjaxBinding()
        {
            return View(new GridModel(UeberfaelligeRuecksendungenViewModel.UeberfaelligeRuecksendungenFiltered));
        }

        [HttpPost]
        public ActionResult MemoSpeichern(string equiNr, string memo)
        {
            UeberfaelligeRuecksendungenViewModel.SaveUeberfaelligeRuecksendung(equiNr, memo);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FristVerlaengern(string equiNr)
        {
            UeberfaelligeRuecksendungenViewModel.FristVerlaengern(equiNr);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FilterGridUeberfaelligeRuecksendungen(string filterValue, string filterColumns)
        {
            UeberfaelligeRuecksendungenViewModel.FilterUeberfaelligeRuecksendungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportUeberfaelligeRuecksendungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = UeberfaelligeRuecksendungenViewModel.UeberfaelligeRuecksendungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.OverdueReturns, dt);

            return new EmptyResult();
        }

        public ActionResult ExportUeberfaelligeRuecksendungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = UeberfaelligeRuecksendungenViewModel.UeberfaelligeRuecksendungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.OverdueReturns, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
