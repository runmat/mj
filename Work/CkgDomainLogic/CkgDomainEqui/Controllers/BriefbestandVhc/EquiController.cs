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
        public BriefbestandVhcViewModel BriefbestandVhcViewModel { get { return GetViewModel<BriefbestandVhcViewModel>(); } }

        [CkgApplication]
        public ActionResult BriefbestandVhc()
        {
            BriefbestandVhcViewModel.LoadFahrzeugbriefeVhc();

            return View(BriefbestandVhcViewModel);
        }

        [GridAction]
        public ActionResult FahrzeugbriefeVhcAjaxBinding()
        {
            var items = BriefbestandVhcViewModel.FahrzeugbriefeVhcFiltered; 

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugbriefeVhc(string filterValue, string filterColumns)
        {
            BriefbestandVhcViewModel.FilterFahrzeugbriefeVhc(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugbriefeVhcFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = BriefbestandVhcViewModel.FahrzeugbriefeVhcFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("FahrzeugbriefeVhc", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugbriefeVhcFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = BriefbestandVhcViewModel.FahrzeugbriefeVhcFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("FahrzeugbriefeVhc", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
