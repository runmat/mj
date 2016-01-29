using System;
using System.Data;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Finance.Models;
using CkgDomainLogic.Finance.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class FinanceController 
    {
        public FinanceVersendungenViewModel VersendungenViewModel { get { return GetViewModel<FinanceVersendungenViewModel>(); } }
      
        [CkgApplication]
        public ActionResult ReportVersendungen()
        {
            VersendungenViewModel.Init();

            return View(VersendungenViewModel);
        }

        [CkgApplication]
        public ActionResult ReportVersendungenSumme()
        {
            VersendungenViewModel.Init(true);

            return View("ReportVersendungen", VersendungenViewModel);
        }

        [HttpPost]
        public ActionResult LoadVersendungen(VersendungenSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                VersendungenViewModel.Suchparameter = model;
                VersendungenViewModel.LoadVersendungen(ModelState);
            }

            return PartialView("Versendungen/Suche", model);
        }

        [HttpPost]
        public ActionResult ShowVersendungen()
        {
            return PartialView(String.Format("Versendungen/Grid{0}", VersendungenViewModel.Suchparameter.IsSummaryReport ? "Summe" : ""), VersendungenViewModel);
        }

        [GridAction]
        public ActionResult VersendungenAjaxBinding()
        {
            return View(new GridModel(VersendungenViewModel.VersendungenFiltered));
        }

        [GridAction]
        public ActionResult VersendungenSummiertAjaxBinding()
        {
            return View(new GridModel(VersendungenViewModel.VersendungenSummiert));
        }

        [HttpPost]
        public ActionResult FilterGridVersendungen(string filterValue, string filterColumns)
        {
            return new EmptyResult();
        }

        #region Export
       
        public ActionResult ExportVersendungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Shipments, GetData(orderBy, filterBy));

            return new EmptyResult();
        }

        public ActionResult ExportVersendungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Shipments, GetData(orderBy, filterBy), landscapeOrientation: true);

            return new EmptyResult();
        }

        private DataTable GetData(string orderBy, string filterBy)
        {
            if (VersendungenViewModel.Suchparameter.IsSummaryReport)
                return VersendungenViewModel.VersendungenSummiert.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            
            return VersendungenViewModel.VersendungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
        }

        #endregion
    }   
}
