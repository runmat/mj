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
        public MahnreportViewModel MahnreportViewModel { get { return GetViewModel<MahnreportViewModel>(); } }

        [CkgApplication]
        public ActionResult Mahnreport()
        {
            MahnreportViewModel.LoadFahrzeuge();

            return View(MahnreportViewModel);
        }

        [GridAction]
        public ActionResult FahrzeugeMahnAjaxBinding()
        {
            var items = MahnreportViewModel.FahrzeugeFiltered; 

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugeMahn(string filterValue, string filterColumns)
        {
            MahnreportViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugeMahnFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = MahnreportViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Briefbestand", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugeMahnFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = MahnreportViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Briefbestand", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
