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
        public FinanceGebuehrenauslageViewModel GebuehrenauslageViewModel { get { return GetViewModel<FinanceGebuehrenauslageViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportGebuehrenauslage()
        {
            return View(GebuehrenauslageViewModel);
        }

        [HttpPost]
        public ActionResult LoadAuftraege(AuftragSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                GebuehrenauslageViewModel.LoadAuftraege(model);
                if (GebuehrenauslageViewModel.Auftraege.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Gebuehrenauslage/AuftragSuche", model);
        }

        [HttpPost]
        public ActionResult ShowAuftraege()
        {
            return PartialView("Gebuehrenauslage/AuftraegeGrid", GebuehrenauslageViewModel);
        }

        [GridAction]
        public ActionResult AuftraegeAjaxBinding()
        {
            return View(new GridModel(GebuehrenauslageViewModel.AuftraegeFiltered));
        }

        [HttpPost]
        public ActionResult ShowAuftragEdit(string auftragsnr)
        {
            var auftr = GebuehrenauslageViewModel.GetAuftrag(auftragsnr);

            return PartialView("Gebuehrenauslage/AuftragEdit", auftr);
        }

        [HttpPost]
        public ActionResult EditAuftrag(Auftrag model)
        {
            GebuehrenauslageViewModel.SaveAuftragChanges(model);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FilterGridAuftraege(string filterValue, string filterColumns)
        {
            GebuehrenauslageViewModel.FilterAuftraege(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportAuftraegeFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = GebuehrenauslageViewModel.AuftraegeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Auftraege", dt);

            return new EmptyResult();
        }

        public ActionResult ExportAuftraegeFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = GebuehrenauslageViewModel.AuftraegeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Auftraege", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
