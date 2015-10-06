using System;
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
            CarporterfassungViewModel.LastCarportIdInit(vmStored == null ? null : vmStored.LastCarportId);

            LoadPersistedObjects();

            return View(CarporterfassungViewModel);
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

                // save to shopping cart
                model = (CarporterfassungModel)PersistanceSaveObject(PersistableGroupKey, model.ObjectKey, model);

                CarporterfassungViewModel.AddFahrzeug(model);

                CarporterfassungViewModel.LastCarportIdInit(model.CarportId);
                LogonContext.DataContextPersist(CarporterfassungViewModel);
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
                fzg.AuftragsNr, fzg.MvaNr, fzg.CarportName, 
                Status = fzg.Status.NotNullOrEmpty(),
                TmpStatus = fzg.TmpStatus.NotNullOrEmpty()
            });
        }

        [HttpPost]
        public ActionResult ListeAnzeigen()
        {
            CarporterfassungViewModel.TryAvoidNullValueForCarportIdPersisted(CarporterfassungViewModel.LastCarportId);

            return PartialView("Carporterfassung/Grid", CarporterfassungViewModel);
        }

        [GridAction]
        public ActionResult CarporterfassungAjaxBinding()
        {
            return View(new GridModel(CarporterfassungViewModel.FahrzeugeFiltered));
        }

        [HttpPost]
        public ActionResult CarportSelectionForm(CarporterfassungModel model)
        {
            CarporterfassungViewModel.SaveCarportSelectionModel(model);

            return PartialView("Carporterfassung/CarportSelectionForm", model);
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
                CarporterfassungViewModel.ClearList((ownerMultiKey, additionalFilter) => PersistanceDeleteAllObjects(PersistableGroupKey, ownerMultiKey, additionalFilter));
                LoadPersistedObjects();
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
