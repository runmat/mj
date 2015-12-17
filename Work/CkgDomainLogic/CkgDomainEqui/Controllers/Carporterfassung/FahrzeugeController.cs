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

            var vmStored = (CarporterfassungViewModel)LogonContext.DataContextRestore(typeof(CarporterfassungViewModel).GetFullTypeName());

            CarporterfassungViewModel.Init(PersistanceGetObjects<CarporterfassungModel>(PersistableGroupKey, "ALL"), false, vmStored == null ? null : vmStored.LastOrganizationId, vmStored == null ? null : vmStored.LastCarportId);

            return View(CarporterfassungViewModel);
        }

        [CkgApplication]
        public ActionResult Carportnacherfassung()
        {
            _dataContextKey = typeof(CarporterfassungViewModel).Name;

            var vmStored = (CarporterfassungViewModel)LogonContext.DataContextRestore(typeof(CarporterfassungViewModel).GetFullTypeName());

            CarporterfassungViewModel.Init(new List<CarporterfassungModel>(), true, vmStored == null ? null : vmStored.LastOrganizationId, vmStored == null ? null : vmStored.LastCarportId);

            return View(CarporterfassungViewModel);
        }

        [CkgApplication]
        public ActionResult CarportUpsLabel()
        {
            _dataContextKey = typeof(CarporterfassungViewModel).Name;

            var vmStored = (CarporterfassungViewModel)LogonContext.DataContextRestore(typeof(CarporterfassungViewModel).GetFullTypeName());

            CarporterfassungViewModel.Init(new List<CarporterfassungModel>(), false, vmStored == null ? null : vmStored.LastOrganizationId, vmStored == null ? null : vmStored.LastCarportId);

            return View(CarporterfassungViewModel);
        }

        [HttpPost]
        public ActionResult FahrzeugerfassungForm(CarporterfassungModel model)
        {
            if (model.UiUpdateOnly)
            {
                CarporterfassungViewModel.CarportSelectionModel.SelectedOrganizationId = model.Organisation;
                model.UiUpdateOnly = false;

                ModelState.Clear();
                model.IsValid = false;

                return PartialView("Carporterfassung/FahrzeugerfassungForm", model);
            }

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

                    CarporterfassungViewModel.SetLastOrganizationIdAndCarportId(model.Organisation, model.CarportId);
                    CarporterfassungViewModel.SetSelectedOrganizationIdAndCarportId(model.Organisation, model.CarportId);
                    LogonContext.DataContextPersist(CarporterfassungViewModel);
                }
            }

            model.IsValid = ModelState.IsValid;

            return PartialView("Carporterfassung/FahrzeugerfassungForm", model);
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

            CarporterfassungViewModel.Init(PersistanceGetObjects<CarporterfassungModel>(PersistableGroupKey, "ALL"), false);

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

        #region Nacherfassung

        [HttpPost]
        public ActionResult LoadCarportnacherfassungFahrzeuge(CarportnacherfassungSelektor model)
        {
            if (ModelState.IsValid)
                CarporterfassungViewModel.LoadNacherfassungFahrzeuge(ref model, ModelState);

            return PartialView("Carporterfassung/NacherfassungSuche", model);
        }

        [HttpPost]
        public ActionResult FahrzeugNachbearbeiten(string kennzeichen)
        {
            CarporterfassungViewModel.LoadFahrzeugModel(kennzeichen);

            return PartialView("Carporterfassung/FahrzeugnacherfassungForm", CarporterfassungViewModel.AktuellesFahrzeug);
        }

        [HttpPost]
        public ActionResult FahrzeugnacherfassungForm(CarporterfassungModel model)
        {
            if (ModelState.IsValid)
            {
                CarporterfassungViewModel.PrepareCarportModel(ref model);

                if (ModelState.IsValid)
                {
                    CarporterfassungViewModel.UpdateAndSaveFahrzeug(model, ModelState);

                    CarporterfassungViewModel.SetLastOrganizationIdAndCarportId(model.Organisation, model.CarportId);
                    LogonContext.DataContextPersist(CarporterfassungViewModel);
                }
            }

            return PartialView("Carporterfassung/FahrzeugnacherfassungForm", model);
        }

        #endregion
    }
}
