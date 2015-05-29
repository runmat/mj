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
        public FahrzeugSperrenVerschiebenViewModel SperrenVerschiebenViewModel { get { return GetViewModel<FahrzeugSperrenVerschiebenViewModel>(); } }

        [CkgApplication]
        public ActionResult SperrenVerschieben()
        {
            _dataContextKey = typeof(FahrzeugSperrenVerschiebenViewModel).Name;
            SperrenVerschiebenViewModel.LoadFahrzeuge();

            return View(SperrenVerschiebenViewModel);
        }

        [HttpPost]
        public ActionResult UpdateGridFahrzeugeSperrenVerschieben()
        {
            return PartialView("SperrenVerschieben/Grid", SperrenVerschiebenViewModel);
        }

        [GridAction]
        public ActionResult FzgSperrenVerschiebenAjaxBinding()
        {
            return View(new GridModel(SperrenVerschiebenViewModel.GridItems));
        }

        [HttpPost]
        public ActionResult FilterFzgSperrenVerschiebenData(string auswahl, bool nurMitBemerkung)
        {
            SperrenVerschiebenViewModel.ApplyDatenfilter(auswahl, nurMitBemerkung);
            SperrenVerschiebenViewModel.DataMarkForRefresh();

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FilterGridFzgSperrenVerschieben(string filterValue, string filterColumns)
        {
            SperrenVerschiebenViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult FzgSperrenVerschiebenSelectionChanged(string vin, bool isChecked)
        {
            if (vin.IsNullOrEmpty())
                SperrenVerschiebenViewModel.SelectFahrzeuge(isChecked);
            else
                SperrenVerschiebenViewModel.SelectFahrzeug(vin, isChecked);

            return Json(new { allSelectionCount = SperrenVerschiebenViewModel.SelektierteFahrzeuge.Count });
        }

        [HttpPost]
        public ActionResult FzgSperren(bool sperren)
        {
            if (!SperrenVerschiebenViewModel.SperrenMoeglich(sperren))
                return Json(new { message = Localize.ActionNotPossibleForFewOfSelectedItems });

            return PartialView("SperrenVerschieben/SperrenForm", SperrenVerschiebenViewModel.GetUiModelSperrenVerschieben(sperren));
        }

        [HttpPost]
        public ActionResult FzgSperrenForm(FahrzeugSperrenVerschieben model)
        {
            SperrenVerschiebenViewModel.FahrzeugeSperren(ref model, ModelState);

            return PartialView("SperrenVerschieben/SperrenForm", model);
        }

        [HttpPost]
        public ActionResult FzgVerschieben()
        {
            ViewBag.AllePdis = SperrenVerschiebenViewModel.Pdis;

            return PartialView("SperrenVerschieben/VerschiebenForm", SperrenVerschiebenViewModel.GetUiModelSperrenVerschieben());
        }

        [HttpPost]
        public ActionResult FzgVerschiebenForm(FahrzeugSperrenVerschieben model)
        {
            SperrenVerschiebenViewModel.FahrzeugeVerschieben(ref model);

            ViewBag.AllePdis = SperrenVerschiebenViewModel.Pdis;

            return PartialView("SperrenVerschieben/VerschiebenForm", model);
        }

        [HttpPost]
        public ActionResult FzgTextErfassen()
        {
            return PartialView("SperrenVerschieben/TextErfassenForm", SperrenVerschiebenViewModel.GetUiModelSperrenVerschieben());
        }

        [HttpPost]
        public ActionResult FzgTextErfassenForm(FahrzeugSperrenVerschieben model)
        {
            SperrenVerschiebenViewModel.FahrzeugeTexteErfassen(ref model);

            return PartialView("SperrenVerschieben/TextErfassenForm", model);
        }

        #region Export

        public ActionResult ExportFzgSperrenVerschiebenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = SperrenVerschiebenViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Fahrzeuge_SperrenVerschieben, dt);

            return new EmptyResult();
        }

        public ActionResult ExportFzgSperrenVerschiebenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = SperrenVerschiebenViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Fahrzeuge_SperrenVerschieben, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
