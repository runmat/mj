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
        public KlaerfaelleVhcViewModel KlaerfaelleVhcViewModel { get { return GetViewModel<KlaerfaelleVhcViewModel>(); } }

        [CkgApplication]
        public ActionResult KlaerfaelleVhc()
        {
            KlaerfaelleVhcViewModel.LoadKlaerfaelleVhc();

            return View(KlaerfaelleVhcViewModel);
        }

        [GridAction]
        public ActionResult KlaerfaelleVhcAjaxBinding()
        {
            var items = KlaerfaelleVhcViewModel.KlaerfaelleVhcFiltered; 

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridKlaerfaelleVhc(string filterValue, string filterColumns)
        {
            KlaerfaelleVhcViewModel.FilterKlaerfaelleVhc(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportKlaerfaelleVhcFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = KlaerfaelleVhcViewModel.KlaerfaelleVhcFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("KlaerfaelleVhc", dt);

            return new EmptyResult();
        }

        public ActionResult ExportKlaerfaelleVhcFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = KlaerfaelleVhcViewModel.KlaerfaelleVhcFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("KlaerfaelleVhc", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
