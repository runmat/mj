using System.Web.Mvc;
using CkgDomainLogic.Equi.Models;
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
            return View(KlaerfaelleVhcViewModel);
        }

        [HttpPost]
        public ActionResult LoadKlaerfaelleVhc(KlaerfaelleVhcSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                KlaerfaelleVhcViewModel.LoadKlaerfaelleVhc(model, ModelState);
            }

            return PartialView("KlaerfaelleVhc/KlaerfaelleVhcSuche", model);
        }

        [HttpPost]
        public ActionResult ShowKlaerfaelleVhc()
        {
            return PartialView("KlaerfaelleVhc/KlaerfaelleVhcGrid", KlaerfaelleVhcViewModel);
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
            var dt = KlaerfaelleVhcViewModel.KlaerfaelleVhcFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(KlaerfaelleVhcViewModel.ExcelExportListName, dt);

            return new EmptyResult();
        }

        public ActionResult ExportKlaerfaelleVhcFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = KlaerfaelleVhcViewModel.KlaerfaelleVhcFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(KlaerfaelleVhcViewModel.ExcelExportListName, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
