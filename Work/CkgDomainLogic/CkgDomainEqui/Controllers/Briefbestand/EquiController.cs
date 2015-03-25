using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Equi.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Equi-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class EquiController  
    {
        public BriefbestandViewModel BriefbestandViewModel { get { return GetViewModel<BriefbestandViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportBriefbestand()
        {
            BriefbestandViewModel.LoadFahrzeugbriefe();

            return View(BriefbestandViewModel);
        }

        [GridAction]
        public ActionResult FahrzeugbriefeAjaxBinding()
        {
            var items = BriefbestandViewModel.FahrzeugbriefeFiltered; 

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterBriefbestandData(bool SelektionsfilterLagerbestand, bool SelektionsfilterTempVersendete)
        {
            BriefbestandViewModel.ApplyDatenfilter(SelektionsfilterLagerbestand, SelektionsfilterTempVersendete);
            BriefbestandViewModel.MarkForRefreshFahrzeugbriefeFiltered();

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugbriefe(string filterValue, string filterColumns)
        {
            BriefbestandViewModel.FilterFahrzeugbriefe(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugbriefeFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = BriefbestandViewModel.FahrzeugbriefeFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Briefbestand", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugbriefeFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = BriefbestandViewModel.FahrzeugbriefeFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Briefbestand", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
