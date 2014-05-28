using System;
using System.Web.Mvc;
using DocumentTools.Services;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// COC Reports
    /// </summary>
    public partial class CocController 
    {
        public ActionResult ReportZulassung()
        {
            return View(CocReportsViewModel);
        }

        public ActionResult ReportZulassungPdfTest()
        {
            return View(CocReportsViewModel);
        }

        [GridAction]
        public ActionResult ReportZulassungAjaxBinding()
        {
            var items = CocReportsViewModel.CocAuftraegeFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridZulassungsAuftraege(string filterValue, string filterColumns)
        {
            CocReportsViewModel.FilterCocAuftraege(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult ReportZulassungFilterGrid(bool? nurAuftragsDatumGesetzt, bool? nurAusliefDatumRange, DateTime? ausliefDatumStart, DateTime? ausliefDatumEnd, string nurLand)
        {
            if (nurAuftragsDatumGesetzt.HasValue)
                CocReportsViewModel.FilterNurAuftragsDatumGesetzt = nurAuftragsDatumGesetzt.GetValueOrDefault();

            if (nurAusliefDatumRange.HasValue)
                CocReportsViewModel.FilterNurAusliefDatumRange = nurAusliefDatumRange.GetValueOrDefault();
            if (ausliefDatumStart.HasValue && ausliefDatumEnd.HasValue)
            {
                CocReportsViewModel.FilterAusliefDatumStart = ausliefDatumStart;
                CocReportsViewModel.FilterAusliefDatumEnd = ausliefDatumEnd;
            }

            if (nurLand.IsNotNullOrEmpty())
                CocReportsViewModel.FilterLand = nurLand;

            CocReportsViewModel.FilterCocAuftraege();

            return new EmptyResult();
        }

        public ActionResult ReportZulassungExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = CocReportsViewModel.CocAuftraegeFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("CocAuftraege", dt);

            return new EmptyResult();
        }

        public ActionResult ReportZulassungExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = CocReportsViewModel.CocAuftraegeFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("CocAuftraege", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
