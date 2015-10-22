using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;
using GeneralTools.Models;
using SapORM.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController
    {
        public CarporterfassungViewModel CarporterfassungViewModel { get { return GetViewModel<CarporterfassungViewModel>(); } }

        private string PersistableGroupKey
        {
            get { return string.Format("CarporterfassungsListe_{0}", LogonContext.KundenNr.ToSapKunnr()); }
        }


        [CkgApplication]
        public ActionResult Carporterfassung()
        {
            _dataContextKey = typeof(CarporterfassungViewModel).Name;

            LastCarportIdInit();
            LoadPersistedObjects();

            return View(CarporterfassungViewModel);
        }

        [CkgApplication]
        public ActionResult CarportUpsLabel()
        {
            _dataContextKey = typeof(CarporterfassungViewModel).Name;

            LastCarportIdInit();
            CarporterfassungViewModel.Init(new List<CarporterfassungModel>());

            return View(CarporterfassungViewModel);
        }

        void LastCarportIdInit()
        {
            var vmStored = (CarporterfassungViewModel)LogonContext.DataContextRestore(typeof(CarporterfassungViewModel).GetFullTypeName());
            CarporterfassungViewModel.LastCarportIdInit(vmStored == null ? null : vmStored.LastCarportId);
        }

        void LoadPersistedObjects()
        {
            // get shopping cart items
            var fahrzeugePersisted = PersistanceGetObjects<CarporterfassungModel>(PersistableGroupKey, "ALL");
            CarporterfassungViewModel.Init(fahrzeugePersisted);
        }

        [HttpPost]
        public ActionResult FahrzeugerfassungForm(CarporterfassungModel model)
        {
            if (ModelState.IsValid)
            {
                CarporterfassungViewModel.PrepareCarportModel(ref model);
                CarporterfassungViewModel.CheckFahrgestellnummer(model, ModelState);

                if (ModelState.IsValid)
                {
                    var objKey = model.ObjectKey;

                    // save to shopping cart
                    model = (CarporterfassungModel)PersistanceSaveObject(PersistableGroupKey, model.ObjectKey, model);

                    if (String.IsNullOrEmpty(objKey))
                        CarporterfassungViewModel.AddFahrzeug(model);
                    else
                        CarporterfassungViewModel.UpdateFahrzeug(model);

                    CarporterfassungViewModel.LastCarportIdInit(model.CarportId);
                    LogonContext.DataContextPersist(CarporterfassungViewModel);
                }
            }

            return PartialView("Carporterfassung/FahrzeugerfassungForm", model);
        }

        //[HttpPost]
        //public ActionResult LoadFahrzeugdaten(string kennzeichen, string bestandsnummer, string fin, string finPruefziffer)
        //{
        //    if (!String.IsNullOrEmpty(fin) || !String.IsNullOrEmpty(finPruefziffer))
        //    {
        //        var pruefErg = CarporterfassungViewModel.CheckFahrgestellnummer(fin.NotNullOrEmpty().ToUpper(), finPruefziffer);
        //        if (!String.IsNullOrEmpty(pruefErg))
        //            return Json(new { Status = pruefErg });
        //    }
            
        //    CarporterfassungViewModel.LoadFahrzeugdaten(kennzeichen, bestandsnummer, fin);

        //    var fzg = CarporterfassungViewModel.AktuellesFahrzeug;

        //    return Json(new
        //    {
        //        fzg.Kennzeichen, fzg.FahrgestellNr,
        //        fzg.AuftragsNr, fzg.BestandsNr, fzg.CarportName, 
        //        Status = fzg.Status.NotNullOrEmpty(),
        //        TmpStatus = fzg.TmpStatus.NotNullOrEmpty()
        //    });
        //}

        [HttpPost]
        public ActionResult ListeAnzeigen()
        {
            CarporterfassungViewModel.SetCarportIdPersisted(CarporterfassungViewModel.LastCarportId);

            return PartialView("Carporterfassung/Grid", CarporterfassungViewModel);
        }

        [GridAction]
        public ActionResult CarporterfassungAjaxBinding()
        {
            return View(new GridModel(CarporterfassungViewModel.FahrzeugeFiltered));
        }

        [GridAction]
        public ActionResult CarporterfassungAjaxBindingForConfirmation()
        {
            return View(new GridModel(CarporterfassungViewModel.FahrzeugeForConfirmationFiltered));
        }

        [HttpPost]
        public ActionResult CarportSelectionForm(CarporterfassungModel model)
        {
            CarporterfassungViewModel.SaveCarportSelectionModel(model);

            return PartialView("Carporterfassung/CarportSelectionForm", model);
        }

        [HttpPost]
        public ActionResult CarportUpsLabelSelectionForm(CarporterfassungModel model)
        {
            CarporterfassungViewModel.AktuellesFahrzeug = model;
            CarporterfassungViewModel.LastCarportId = model.CarportId;

            return PartialView("Carporterfassung/CarportSelectionUpsLabelForm", model);
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
            var saveErg = CarporterfassungViewModel.SaveFahrzeuge((ownerMultiKey, additionalFilter) => PersistanceDeleteAllObjects(PersistableGroupKey, ownerMultiKey, additionalFilter));

            if (!String.IsNullOrEmpty(saveErg))
                return Json(new { message = saveErg });

            LoadPersistedObjects();

            return PartialView("Carporterfassung/GridForConfirmation", CarporterfassungViewModel);
        }

        [HttpPost]
        public ActionResult NeuesFahrzeugErfassen(bool clearList)
        {
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

        [HttpPost]
        public ActionResult FilterGridCarporterfassungForConfirmation(string filterValue, string filterColumns)
        {
            CarporterfassungViewModel.FilterFahrzeugeForConfirmation(filterValue, filterColumns);

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
        public ActionResult ExportCarporterfassungFilteredExcelForConfirmation(int page, string orderBy, string filterBy)
        {
            var dt = CarporterfassungViewModel.FahrzeugeForConfirmationFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Fahrzeuge_Carporterfassung, dt);

            return new EmptyResult();
        }

        public ActionResult ExportCarporterfassungFilteredPDFForConfirmation(int page, string orderBy, string filterBy)
        {
            var dt = CarporterfassungViewModel.FahrzeugeForConfirmationFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Fahrzeuge_Carporterfassung, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
