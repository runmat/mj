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
        public FinanceVersandsperreViewModel VersandsperreViewModel { get { return GetViewModel<FinanceVersandsperreViewModel>(); } }

        [CkgApplication]
        public ActionResult Versandsperre()
        {
            VersandsperreViewModel.FillVertragsarten();

            return View(VersandsperreViewModel);
        }

        [HttpPost]
        public ActionResult LoadVersandsperreVorgaenge(VorgangVersandperreSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                VersandsperreViewModel.LoadVorgaenge(model, ModelState);
            }

            model.AuswahlVertragsart = VersandsperreViewModel.DataService.Suchparameter.AuswahlVertragsart;

            return PartialView("Versandsperre/VersandsperreSuche", model);
        }

        [HttpPost]
        public ActionResult ShowVersandsperreVorgaenge()
        {
            return PartialView("Versandsperre/VersandsperreGrid", VersandsperreViewModel);
        }

        [GridAction]
        public ActionResult VersandsperreAjaxBinding()
        {
            return View(new GridModel(VersandsperreViewModel.GridItems));
        }

        [HttpPost]
        public ActionResult SaveVersandsperren(string selectedItems)
        {
            VersandsperreViewModel.SaveVersandsperren(selectedItems, ModelState);

            return PartialView("Versandsperre/VersandsperreGrid", VersandsperreViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridVorgaengeVersandsperre(string filterValue, string filterColumns)
        {
            VersandsperreViewModel.FilterVorgaenge(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportVorgaengeVersandsperreFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = VersandsperreViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Versandsperre", dt);

            return new EmptyResult();
        }

        public ActionResult ExportVorgaengeVersandsperreFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = VersandsperreViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Versandsperre", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
