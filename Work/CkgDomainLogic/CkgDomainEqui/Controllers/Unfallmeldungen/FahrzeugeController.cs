using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController : CkgDomainController
    {
        public UnfallmeldungenViewModel UnfallmeldungenViewModel { get { return GetViewModel<UnfallmeldungenViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportUnfallmeldungen()
        {
            _dataContextKey = typeof(UnfallmeldungenViewModel).Name;
            UnfallmeldungenViewModel.DataInit();

            return View(UnfallmeldungenViewModel);
        }

        [CkgApplication]
        public ActionResult Unfallmeldungen()
        {
            _dataContextKey = typeof(UnfallmeldungenViewModel).Name;
            UnfallmeldungenViewModel.DataInit();
            UnfallmeldungenViewModel.LoadAllUnfallmeldungen();

            return View(UnfallmeldungenViewModel);
        }

        [HttpPost]
        public ActionResult LoadUnfallmeldungen(UnfallmeldungenSelektor model)
        {
            UnfallmeldungenViewModel.UnfallmeldungenSelektor = model;

            UnfallmeldungenViewModel.Validate(AddModelError);

            if (ModelState.IsValid)
            {
                UnfallmeldungenViewModel.LoadUnfallmeldungen();
                if (UnfallmeldungenViewModel.Unfallmeldungen.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Unfallmeldungen/UnfallmeldungenSuche", UnfallmeldungenViewModel.UnfallmeldungenSelektor);
        }

        [HttpPost]
        public ActionResult ShowUnfallmeldungen()
        {
            return PartialView("Unfallmeldungen/UnfallmeldungenGrid", UnfallmeldungenViewModel);
        }

        [GridAction]
        public ActionResult UnfallmeldungenAjaxBinding()
        {
            return View(new GridModel(UnfallmeldungenViewModel.UnfallmeldungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridUnfallmeldungen(string filterValue, string filterColumns)
        {
            UnfallmeldungenViewModel.FilterUnfallmeldungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult UnfallmeldungenSelectionChanged(string vin, bool isChecked)
        {
            int allSelectionCount, allCount = 0, allFoundCount = 0;
            if (vin.IsNullOrEmpty())
                UnfallmeldungenViewModel.SelectUnfallmeldungen(isChecked, f => f.IsValidForCancellation, out allSelectionCount, out allCount, out allFoundCount);
            else
                UnfallmeldungenViewModel.SelectUnfallmeldung(vin, isChecked, out allSelectionCount);

            return Json(new { allSelectionCount, allCount, allFoundCount });
        }

       
        #region Export
       
        public ActionResult ExportUnfallmeldungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = UnfallmeldungenViewModel.UnfallmeldungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.RegistrationRequests, dt);

            return new EmptyResult();
        }

        public ActionResult ExportUnfallmeldungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = UnfallmeldungenViewModel.UnfallmeldungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.RegistrationRequests, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
