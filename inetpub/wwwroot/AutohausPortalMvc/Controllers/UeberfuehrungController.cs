using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CkgDomainLogic.General.Controllers;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Ueberfuehrung.Contracts;
using CkgDomainLogic.Ueberfuehrung.Models;
using CkgDomainLogic.Ueberfuehrung.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using Telerik.Web.Mvc;

namespace AutohausPortalMvc.Controllers
{
    public class UeberfuehrungController : CkgDomainController
    {
        protected override bool NeedsDefaultIndexActionInUrl { get { return false; } }

        public UeberfuehrungViewModel ViewModel
        {
            get { return GetViewModel<UeberfuehrungViewModel>(); }
        }

        #region LogonCapableController overrides

        #region DataContextCapableController overrides

        public override string DataContextKey { get { return "UeberfuehrungViewModel"; } }

        #endregion

        protected static ILogonContext CreateLogonContext()
        {
            //if (AppSettings.IsClickDummyMode)
            //    return new LogonContextTest(new Loc1lize()) { KundenNr = "0010000649" };

            return new LogonContextDataServiceAutohaus();
        }

        #endregion 


        public UeberfuehrungController(IAppSettings appSettings, ILogonContextDataServiceAutohaus logonContext, IUeberfuehrungDataService ueberfuehrungDataService) : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, ueberfuehrungDataService);
        }

        public ActionResult Index()
        {
            ViewModel.DataMarkForRefresh();

            return View(ViewModel);
        }

        public ActionResult TestRaiseError()
        {
            ViewModel.TestRaiseError();
            return null;
        }

        [HttpPost]
        public ActionResult TestRaiseErrorAsPost()
        {
            ViewModel.TestRaiseError();
            return null;
        }


        #region Sonstiges

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BemerkungenEdit(Bemerkungen model)
        {
            ModelState.Clear();

            model = ViewModel.SaveSubModelWithPreservingUiModel(model, model.UiIndex);

            return PartialView(model.ViewName, model);
        }

        #endregion


        #region Dienstleistungsauswahl

        [HttpPost]
        public ActionResult DienstleistungsAuswahlEdit(DienstleistungsAuswahl model)
        {
            ModelState.Clear();

            model = ViewModel.SaveSubModelWithPreservingUiModel(model, model.UiIndex);

            return PartialView(model.ViewName, model);
        }

        #endregion


        #region Fahrzeug

        [HttpPost]
        public ActionResult FahrzeugEdit(Fahrzeug model)
        {
            ModelState.Clear();

            if (model.RequestClearModel)
            {
                model.RequestClearModel = false;
                //model = ViewModel.ClearSubModelWithPreservingUiModel<Fahrzeug>(model.UiIndex, a =>
                //{
                //    a.FahrzeugIndex = model.FahrzeugIndex;
                //});
                ModelMapping.Clear(model);
                model.IsValidCustom = true;
                return PartialView(model.ViewName, model);
            }

            ViewModel.DataService.TryLoadFahrzeugFromFIN(ref model);

            model = ViewModel.SaveSubModelWithPreservingUiModel(model, model.UiIndex);

            TryModelValidation(model, "Bitte vervollständigen Sie die Fahrzeugdaten");

            return PartialView(model.ViewName, model);
        }

        #endregion


        #region Rechnungsadressen

        [HttpPost]
        public ActionResult AdressenSelectableEdit(Adresse model)
        {
            ModelState.Clear();

            var addressIndex = model.UiIndex;

            string reAdresseOpticalCheck;
            ViewModel.TryPreassignReToRgAdresse(model, out reAdresseOpticalCheck);

            model = ViewModel.RechnungsAdressen.FirstOrDefault(a => a.ID == model.SelectedID) ?? new Adresse();
            model.ReAdresseOpticalCheck = reAdresseOpticalCheck;
            model = ViewModel.SaveSubModelWithPreservingUiModel(model, addressIndex);

            return PartialView(model.ViewName, model);
        }

        #endregion


        #region Fahrtadressen


        #region AutoComplete

        public ActionResult AdressenAutocompleteAjaxBinding(int uiIndex)
        {
            var stepModel = (ViewModel.GetStepForm(uiIndex) as Adresse);
            if (stepModel == null)
                return new EmptyResult();

            return Json(ViewModel.AdressenGridAuswahl.Items.Where(item => item.SubTyp == stepModel.SubTyp).Select(i => i.FirmaOrt), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdressenAutocompleteTextboxOnChange(int uiIndex, string textboxValue)
        {
            var stepModel = (ViewModel.GetStepForm(uiIndex) as Adresse);
            if (stepModel == null)
                return Json(new { status = false });

            var matchingAddress = ViewModel.AdressenGridAuswahl.Items.FirstOrDefault(item => item.SubTyp == stepModel.SubTyp && item.FirmaOrt == textboxValue);
            if (matchingAddress == null)
                return Json(new { status = false });

            ViewModel.AdressenGridAuswahl.FormID = uiIndex;
            ViewModel.AdressenGridAuswahl.ItemID = matchingAddress.ID;

            return Json(new { status = true, foundAddressID = matchingAddress.ID, uiIndex });
        }

        #endregion


        [GridAction]
        public ActionResult AdressenAjaxBinding()
        {
            var stepModel = (ViewModel.GetStepForm(ViewModel.AdressenGridAuswahl.FormID.GetValueOrDefault()) as Adresse);
            if (stepModel == null)
                return new EmptyResult();

            return View(new GridModel(ViewModel.AdressenGridAuswahl.Items.Where(item => item.SubTyp == stepModel.SubTyp)));
        }

        [HttpPost]
        public ActionResult AdressenEdit(Adresse model)
        {
            ModelState.Clear();

            if (model.RequestClearModel)
            {
                model.RequestClearModel = false;
                ModelMapping.Clear(model);
                model.IsValidCustom = true;
                return PartialView(model.ViewName, model);
            }

            int adressIndex;
            if (ViewModel.AdressenGridAuswahl.FormID != null)
            {
                //
                // address picking via grid dialog
                //
                adressIndex = ViewModel.AdressenGridAuswahl.FormID.GetValueOrDefault();
                var addressID = ViewModel.AdressenGridAuswahl.ItemID.GetValueOrDefault();

                ViewModel.AdressenGridAuswahl.FormID = null;
                ViewModel.AdressenGridAuswahl.ItemID = null;

                var item = ViewModel.AdressenGridAuswahl.Items.FirstOrDefault(i => i.ID == addressID);
                if (item != null)
                {
                    CkgDomainLogic.Ueberfuehrung.Models.AppModelMappings.Adresse_To_Adresse.Copy(item, model);
                    model.IsValidCustom = true;
                }
            }
            else
            {
                //
                // address update via form submit
                //
                adressIndex = model.UiIndex;
                model = ViewModel.SaveSubModelWithPreservingUiModel(model, adressIndex);

                model.IsValidCustom = model.RequestGeoMapInfo;
                if (!model.RequestGeoMapInfo)
                    TryModelValidation(model, "Bitte vervollständigen Sie die Adresse");

                model.RequestGeoMapInfo = false;
            }

            model.UiIndex = adressIndex;

            return PartialView(model.ViewName, model);
        }

        [HttpPost]
        public ActionResult PickAddress(int id, int fahrtAdressIndex)
        {
            ViewModel.AdressenGridAuswahl.FormID = fahrtAdressIndex;
            ViewModel.AdressenGridAuswahl.ItemID = id;

            return Json(new {id, fahrtAdressIndex});
        }

        [HttpPost]
        public ActionResult ShowGridAdressen(int id, int fahrtAdressIndex)
        {
            ViewModel.AdressenGridAuswahl.FormID = fahrtAdressIndex;
            ViewModel.AdressenGridAuswahl.ItemID = id;

            return PartialView("Partial/AdressenSelection", ViewModel.AdressenGridAuswahl);
        }

        [HttpPost]
        public ActionResult ShowGeoLocationInfo(int uiIndex)
        {
            var routingInfo = ViewModel.GetGeoLocationInfo(uiIndex);
            if (routingInfo == null)
                return new EmptyResult();

            return PartialView("Partial/LocationInfo", routingInfo);
        }

        [HttpPost]
        public ActionResult ShowRouteInfo(int uiIndex)
        {
            var routingInfo = ViewModel.GetRoutingInfo(uiIndex);
            if (routingInfo == null)
                return new EmptyResult();

            return PartialView("Partial/RouteInfo", routingInfo);
        }

        #endregion


        #region Upload Files

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadFilesEdit(UploadFiles model)
        {
            ModelState.Clear();

            model = ViewModel.SaveSubModelWithPreservingUiModel(model, model.UiIndex);

            return PartialView(model.ViewName, model);
        }

        [HttpPost]
        public ActionResult UploadFilesSave(IEnumerable<HttpPostedFileBase> attachments)
        {
            var model = ViewModel.StepForms.OfType<UploadFiles>().FirstOrDefault();
            if (model == null)
                return Json(new { success = false, message = "Fehler: Das UI-Model muss vom Typ 'UploadFiles' sein!" }, "text/plain");

            if (attachments == null || attachments.None())
                return Json(new { success = false, message = "Fehler: Kein Dateiname angegeben!" }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = attachments.ToArray()[0];

            //if (AppSettings.IsTestEnvironment)
            //    return Json(new
            //                {
            //                    success = true, 
            //                    message = "ok",
            //                    uploadFileName = file.FileName,
            //                }, "text/plain");

            string fileName;

            //if (file.FileName.Length > 20)
            //    return Json(new { success = false, message = "Es gab ein Problem beim Speichern Ihrer Datei! Der Dateiname ist zu lang." }, "text/plain");

            // Model.SaveSubModelWithPreservingUiModel(model, model.UiIndex);
            
            //
            // save file here:
            //
            string errorMessage;
            if (!ViewModel.SaveUploadFile(file, model.FahrtIndex, out fileName, out errorMessage))
                return Json(new { success = false, message = errorMessage }, "text/plain");

            return Json(new
                            {
                                success = true, 
                                message = "ok",
                                uploadFileName = fileName,
                            }, "text/plain");
        }

        #endregion


        #region Summary

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SummaryEdit(Summary model)
        {
            ModelState.Clear();

            model = ViewModel.SaveSubModelWithPreservingUiModel(model, model.UiIndex);

            return PartialView(model.ViewName, model);
        }

        [HttpPost]
        public ActionResult StartSave()
        {
            if (AppSettings.IsClickDummyMode)
            {
                ViewModel.ReceiptPdfFileName = Path.Combine(AppSettings.DataPath, "Überführung_Dummyauftrag.pdf");
                Thread.Sleep(6000);
            }
            else
                ViewModel.SaveAll();

            ViewModel.RequestClearViewModel = true;

            return Json(new { receiptErrorMessages = ViewModel.ReceiptErrorMessages });
        }

        public FileContentResult DownloadPdf()
        {
            return new FileContentResult(FileService.GetBytesFromFile(ViewModel.ReceiptPdfFileName), "application/pdf")
            {
                FileDownloadName = "Ueberfuehrung.pdf"
            };
        }

        public ActionResult StartNewOrder()
        {
            //ViewModelLoadOrClear();

            return RedirectToAction("Index");
        }

        #endregion


        #region Step Navigation


        [HttpPost]
        public ActionResult MoveToStep(int increment)
        {
            //if (increment < 0)
            //    throw new Exception("Bitte nicht zurück navigieren!");

            if (ViewModel.TryStartSave(increment))
                return PartialView("Partial/SubmitReceipt", ViewModel);
            
            if (!ViewModel.HandleStepIncrement(increment))
                return new EmptyResult();

            return PartialView(ViewModel.StepPartialViewName, ViewModel);
        }

        [HttpPost]
        public ActionResult NavigateToStep(int step)
        {
            if (!ViewModel.NavigateToStep(step))
                return new EmptyResult();

            return PartialView(ViewModel.StepPartialViewName, ViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TryNavigateToStepFromSummary(int step)
        {
            ViewModel.FromSummaryChangeStep = step;

            return new EmptyResult();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TryResetNavigateToStepFromSummary()
        {
            ViewModel.FromSummaryChangeStep = 0;

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult CurrentStepValidation()
        {
            //
            // first, validation at sub-form level:
            //
            var currentStepFirstErrorModel = ViewModel.CurrentStepFirstModelMandatoryButEmpty;

            if (currentStepFirstErrorModel != null)
                return Json(new
                {
                    firstValidationErrorForm = currentStepFirstErrorModel.UiIndex,
                    firstValidationErrorMessage = currentStepFirstErrorModel.ValidationErrorMessage,
                });

            //
            // second, validation at step level:
            //
            currentStepFirstErrorModel = ViewModel.CurrentStepFirstModelWithDependencyError;

            if (currentStepFirstErrorModel != null)
                return Json(new
                {
                    firstValidationErrorForm = currentStepFirstErrorModel.UiIndex,
                    firstValidationErrorMessage = currentStepFirstErrorModel.ValidationErrorMessage,
                    firstValidationDependencyErrorProperty = currentStepFirstErrorModel.ValidationDependencyErrorFirstProperty,
                });

            return new EmptyResult();
        }

        #endregion


        #region Misc

        void TryModelValidation(UiModel model, string generalValidationMessage)
        {
            if (!model.IsValid)
            {
                // check required but empty properties (also maybe allow a whole empty form)
                model.RequiredButModelOptionalPropertiesWithNullValues.ForEach(nullProperty => ModelState.AddModelError(nullProperty, ""));
                if (model.RequiredButModelOptionalPropertiesWithNullValues.Any())
                    model.ValidationErrorMessage = generalValidationMessage;
                else 
                    if (model.ValidationAdditionalErrorProperties.Any())
                    {
                        // check additional validation errors on properties 
                        var firstValidationAdditionalErrorProperty = model.ValidationAdditionalErrorProperties.First();
                        ModelState.AddModelError(firstValidationAdditionalErrorProperty.Key, "");
                        model.ValidationErrorMessage = firstValidationAdditionalErrorProperty.Value;
                    }
            }
        }

        #endregion
    }
}
