﻿using System;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;
using GeneralTools.Models;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController
    {
        public CarporterfassungViewModel CarporterfassungViewModel { get { return GetViewModel<CarporterfassungViewModel>(); } }

        private static string PersistableGroupKey
        {
            get { return "CarporterfassungsListe"; }
        }


        [CkgApplication]
        public ActionResult Carporterfassung()
        {
            _dataContextKey = typeof(CarporterfassungViewModel).Name;

            CarporterfassungViewModel.Init();

            // get shopping cart items
            CarporterfassungViewModel.Fahrzeuge = PersistanceGetObjects<CarporterfassungModel>(PersistableGroupKey);

            return View(CarporterfassungViewModel);
        }

        [HttpPost]
        public ActionResult FahrzeugerfassungForm(CarporterfassungModel model)
        {
            if (ModelState.IsValid)
            {
                model.Kennzeichen = CarporterfassungViewModel.PrepareKennzeichen(model.Kennzeichen);

                // save to shopping cart
                model = (CarporterfassungModel)PersistanceSaveObject(PersistableGroupKey, model.ObjectKey, model);

                CarporterfassungViewModel.AddFahrzeug(model);
            }

            return PartialView("Carporterfassung/FahrzeugerfassungForm", model);
        }

        [HttpPost]
        public ActionResult LoadFahrzeugdaten(string kennzeichen, string bestandsnummer, string fin)
        {
            CarporterfassungViewModel.LoadFahrzeugdaten(kennzeichen, bestandsnummer, fin);

            var fzg = CarporterfassungViewModel.AktuellesFahrzeug;

            return Json(new
            {
                fzg.Kennzeichen, fzg.FahrgestellNr,
                fzg.AuftragsNr, fzg.MvaNr, fzg.CarportName, fzg.Carport,
                Status = fzg.Status.NotNullOrEmpty(),
                TmpStatus = fzg.TmpStatus.NotNullOrEmpty()
            });
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
        public ActionResult FahrzeugDelete(string kennzeichen)
        {
            var objectKey = CarporterfassungViewModel.DeleteFahrzeugModel(kennzeichen);

            // remove from shopping cart
            PersistanceDeleteObject(objectKey);

            return new EmptyResult();
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
            {
                PersistanceDeleteAllObjects(PersistableGroupKey);

                CarporterfassungViewModel.ClearList();
            }

            CarporterfassungViewModel.LoadFahrzeugModel();

            return PartialView("Carporterfassung/FahrzeugerfassungForm", CarporterfassungViewModel.AktuellesFahrzeug);
        }

        public FileContentResult DeliveryNoteAsPdf()
        {
            var pdfBytes = CarporterfassungViewModel.GetLieferschein();

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.DeliveryNote) };
        }

        public ActionResult GenerateUpsShippingOrder()
        {
            var htmlString = CarporterfassungViewModel.GenerateUpsShippingOrderHtml();

            return Content(htmlString);
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
