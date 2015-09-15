using System;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController
    {
        public CarporterfassungViewModel CarporterfassungViewModel { get { return GetViewModel<CarporterfassungViewModel>(); } }

        [CkgApplication]
        public ActionResult Carporterfassung()
        {
            _dataContextKey = typeof(CarporterfassungViewModel).Name;
            CarporterfassungViewModel.Init();

            return View(CarporterfassungViewModel);
        }

        [HttpPost]
        public ActionResult FahrzeugerfassungForm(CarporterfassungModel model)
        {
            if (ModelState.IsValid)
                CarporterfassungViewModel.AddFahrzeug(model);

            return PartialView("Carporterfassung/FahrzeugerfassungForm", model);
        }

        [HttpPost]
        public ActionResult LoadFahrzeugdatenZuKennzeichen(string kennzeichen)
        {
            CarporterfassungViewModel.LoadFahrzeugdaten(kennzeichen);

            var fzg = CarporterfassungViewModel.AktuellesFahrzeug;

// ReSharper disable RedundantAnonymousTypePropertyName
            return Json(new { FahrgestellNr = fzg.FahrgestellNr, AuftragsNr = fzg.AuftragsNr, MvaNr = fzg.MvaNr, CarportName = fzg.CarportName, Carport = fzg.Carport, Status = fzg.Status });
// ReSharper restore RedundantAnonymousTypePropertyName
        }

        [HttpPost]
        public ActionResult ListeAnzeigen()
        {
            return PartialView("Carporterfassung/Grid", CarporterfassungViewModel);
        }

        [GridAction]
        public ActionResult CarporterfassungAjaxBinding()
        {
            return View(new GridModel(CarporterfassungViewModel.FahrzeugeFiltered));
        }

        [HttpPost]
        public ActionResult FahrzeugBearbeiten(string kennzeichen)
        {
            CarporterfassungViewModel.LoadFahrzeugModel(kennzeichen);

            return PartialView("Carporterfassung/FahrzeugerfassungForm", CarporterfassungViewModel.AktuellesFahrzeug);
        }

        [HttpPost]
        public ActionResult FahrzeugeSpeichern()
        {
            CarporterfassungViewModel.SaveFahrzeuge();

            return PartialView("Carporterfassung/Grid", CarporterfassungViewModel);
        }

        [HttpPost]
        public ActionResult NeuesFahrzeugErfassen(bool clearList)
        {
            if (clearList)
                CarporterfassungViewModel.ClearList();

            CarporterfassungViewModel.LoadFahrzeugModel();

            return PartialView("Carporterfassung/FahrzeugerfassungForm", CarporterfassungViewModel.AktuellesFahrzeug);
        }

        public FileContentResult DeliveryNoteAsPdf()
        {
            var pdfBytes = CarporterfassungViewModel.GetLieferschein();

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.DeliveryNote) };
        }

        [HttpPost]
        public ActionResult FilterGridCarporterfassung(string filterValue, string filterColumns)
        {
            CarporterfassungViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Export

        public ActionResult ExportCarporterfassungFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = CarporterfassungViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Fahrzeuge_Carporterfassung, dt);

            return new EmptyResult();
        }

        public ActionResult ExportCarporterfassungFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = CarporterfassungViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Fahrzeuge_Carporterfassung, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
