// ReSharper disable RedundantUsingDirective
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController
    {
        public FahrzeugSperrenEntsperrenViewModel SperrenEntsperrenViewModel { get { return GetViewModel<FahrzeugSperrenEntsperrenViewModel>(); } }

        [CkgApplication]
        public ActionResult SperrenEntsperren()
        {
            _dataContextKey = typeof(FahrzeugSperrenEntsperrenViewModel).Name;
            SperrenEntsperrenViewModel.Init();
            SperrenEntsperrenViewModel.LoadFahrzeuge();

            return View(SperrenEntsperrenViewModel);
        }

        [HttpPost]
        public ActionResult UpdateGridFahrzeugeSperrenEntsperren()
        {
            return PartialView("SperrenEntsperren/Grid", SperrenEntsperrenViewModel);
        }

        [GridAction]
        public ActionResult FzgSperrenEntsperrenAjaxBinding()
        {
            return View(new GridModel(SperrenEntsperrenViewModel.GridItems));
        }

        [HttpPost]
        public ActionResult FilterFzgSperrenEntsperrenData(string auswahl)
        {
            SperrenEntsperrenViewModel.ApplyFilter(auswahl);
            SperrenEntsperrenViewModel.DataMarkForRefresh();

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FilterGridFzgSperrenEntsperren(string filterValue, string filterColumns)
        {
            SperrenEntsperrenViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult FzgSperrenEntsperrenSelectionChanged(string vin, bool isChecked)
        {
            if (vin.IsNullOrEmpty())
                SperrenEntsperrenViewModel.SelectFahrzeuge(isChecked);
            else
                SperrenEntsperrenViewModel.SelectFahrzeug(vin, isChecked);

            return Json(new { allSelectionCount = SperrenEntsperrenViewModel.SelektierteFahrzeuge.Count });
        }

        //[HttpPost]
        //public ActionResult FzgSperrenEntsperren(bool sperren)
        //{
        //    if (!SperrenEntsperrenViewModel.SperrenMoeglich(sperren))
        //        return Json(new { message = Localize.ActionNotPossibleForFewOfSelectedItems });

        //    return PartialView("SperrenEntsperren/SperrenForm", SperrenEntsperrenViewModel.GetUiModelSperrenEntsperren(sperren));
        //}

        //[HttpPost]
        //public ActionResult FzgSperrenForm(FahrzeugSperrenEntsperren model)
        //{
        //    SperrenEntsperrenViewModel.FahrzeugeSperren(ref model, ModelState);

        //    return PartialView("SperrenEntsperren/SperrenForm", model);
        //}

        #region Export

        public ActionResult ExportFzgSperrenEntsperrenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = SperrenEntsperrenViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Versandauftraege", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFzgSperrenEntsperrenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = SperrenEntsperrenViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Versandauftraege", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
