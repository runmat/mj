using System.Web.Mvc;
using CkgDomainLogic.Finance.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Finance.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Finance-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FinanceController  
    {
        public FinanceMahnstopViewModel MahnstopViewModel { get { return GetViewModel<FinanceMahnstopViewModel>(); } }

        [CkgApplication]
        public ActionResult Mahnstop()
        {
            return View(MahnstopViewModel);
        }

        [HttpPost]
        public ActionResult LoadMahnstops(MahnstopSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                MahnstopViewModel.LoadMahnstops(model, ModelState);
            }

            return PartialView("Mahnstop/MahnstopSuche", model);
        }

        [HttpPost]
        public ActionResult ShowMahnstops()
        {
            return PartialView("Mahnstop/MahnstopGrid", MahnstopViewModel);
        }

        [GridAction]
        public ActionResult MahnstopsAjaxBinding()
        {
            return View(new GridModel(MahnstopViewModel.GridItems));
        }

        [HttpPost]
        public ActionResult MahnstopEdit(string paid, string equinr, string matnr)
        {
            var model = MahnstopViewModel.GetMahnstopEditModel(paid, equinr, matnr);

            return PartialView("Mahnstop/Partial/MahnstopEditForm", model);
        }

        [HttpPost]
        public ActionResult MahnstopEditForm(MahnstopEdit model)
        {
            if (ModelState.IsValid)
            {
                MahnstopViewModel.EditMahnstop(model, ModelState);
            }

            return PartialView("Mahnstop/Partial/MahnstopEditForm", model);
        }

        [HttpPost]
        public ActionResult MahnstopEditComplete()
        {
            return PartialView("Mahnstop/MahnstopGrid", MahnstopViewModel);
        }

        [HttpPost]
        public ActionResult SaveMahnstops()
        {
            MahnstopViewModel.SaveMahnstops(ModelState);

            return PartialView("Mahnstop/MahnstopGrid", MahnstopViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridMahnstops(string filterValue, string filterColumns)
        {
            MahnstopViewModel.FilterMahnstops(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportMahnstopsFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = MahnstopViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Mahnstop", dt);

            return new EmptyResult();
        }

        public ActionResult ExportMahnstopsFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = MahnstopViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Mahnstop", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
