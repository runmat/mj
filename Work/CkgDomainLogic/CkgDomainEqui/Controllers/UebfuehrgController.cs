// ReSharper disable RedundantUsingDirective
using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using CkgDomainLogic.Uebfuehrg.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using Telerik.Web.Mvc;
using System.Linq;
using Adresse = CkgDomainLogic.Uebfuehrg.Models.Adresse;
using Fahrzeug = CkgDomainLogic.Uebfuehrg.Models.Fahrzeug;

// ReSharper restore RedundantUsingDirective

namespace ServicesMvc.Controllers
{
    public class UebfuehrgController : CkgDomainController
    {
        public override string DataContextKey { get { return "UebfuehrgViewModel"; } }

        public UebfuehrgViewModel ViewModel { get { return GetViewModel<UebfuehrgViewModel>(); } }


        public UebfuehrgController(IAppSettings appSettings, ILogonContextDataService logonContext, IUebfuehrgDataService uebfuehrgDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, uebfuehrgDataService);
        }

        [CkgApplication]
        public ActionResult Fzg1()
        {
            return Fzg(1);
        }

        [CkgApplication]
        public ActionResult Fzg2()
        {
            return Fzg(2);
        }

        private ActionResult Fzg(int anzahlFahrzeugeGewuenscht)
        {
            ViewModel.DataInit(anzahlFahrzeugeGewuenscht);

            return View("Fzg", ViewModel);
        }


        #region RgDaten

        [HttpPost]
        public ActionResult RgDatenForm(RgDaten model)
        {
            ViewModel.StepCurrentIndex = model.UiIndex;

            if (ModelState.IsValid)
            {
                ViewModel.SaveSubModelWithPreservingUiModel(model);
                //LogonContext.DataContextPersist(ViewModel);
            }

            return GetStepPartialView();
        }

        #endregion


        #region Fahrzeug

        [HttpPost]
        public ActionResult FahrzeugForm(Fahrzeug model)
        {
            ViewModel.StepCurrentIndex = model.UiIndex;

            if (ModelState.IsValid)
            {
                ViewModel.SaveSubModelWithPreservingUiModel(model);
            }

            return GetStepPartialView();
        }

        #endregion


        #region Adresse

        [HttpPost]
        public ActionResult AdresseForm(Adresse model)
        {
            ViewModel.StepCurrentIndex = model.UiIndex;
            
            if (model.TmpSelectionKey.IsNotNullOrEmpty())
            {
                var savedModel = ViewModel.GetUebfuehrgAdresseFromKey(model.TmpSelectionKey);
                if (savedModel == null)
                    return new EmptyResult();

                savedModel.TransportTypAvailable = model.TransportTypAvailable;
                savedModel.TransportTyp = model.TransportTyp;

                ModelState.Clear();
                savedModel.IsValid = false;

                ViewModel.SaveSubModelWithPreservingUiModel(savedModel);

                return GetStepPartialView();
            }

            model.IsValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                var savedModel = ((Adresse)ViewModel.GetStepModel());
                var savedDatum = savedModel.Datum;
                ViewModel.SaveSubModelWithPreservingUiModel(model);
                ViewModel.CheckAdressDatum(model, savedDatum, ModelState.AddModelError, () =>
                    {
                        ModelState.SetModelValue("Datum", savedDatum == null ? null : savedDatum.GetValueOrDefault().ToString("dd.MM.yyyy"));
                        savedModel.Datum = savedDatum;
                    });
            }

            return GetStepPartialView();
        }

        [HttpPost]
        public ActionResult UebfuehrgAdressenShowGrid(int uiIndex)
        {
            ViewModel.StepCurrentIndex = uiIndex;
            ViewModel.FahrtAdressen.ForEach(adresse => adresse.UiIndex = ViewModel.StepCurrentIndex);

            ViewModel.DataMarkForRefreshUebfuehrgAdressenFiltered();

            return PartialView("Partial/AdressenAuswahlGrid");
        }

        [GridAction]
        public ActionResult UebfuehrgAdressenAjaxBinding()
        {
            var items = ViewModel.UebfuehrgAdressenFiltered;

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterUebfuehrgAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterUebfuehrgAdressen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult UebfuehrgAdresseGetAutoCompleteItems()
        {
            ViewModel.DataMarkForRefreshUebfuehrgAdressenFiltered();

            return Json(new { items = ViewModel.UebfuehrgAdressenAsAutoCompleteItems });
        }

        #endregion


        #region DienstleistungsAuswahl

        [HttpPost]
        public ActionResult DienstleistungsAuswahlForm(DienstleistungsAuswahl model)
        {
            ViewModel.StepCurrentIndex = model.UiIndex;

            if (ModelState.IsValid)
            {
                ViewModel.SaveSubModelWithPreservingUiModel(model);
                ViewModel.CheckDienstleistungsAuswahl(model, ModelState.AddModelError);
            }

            return GetStepPartialView();
        }

        #endregion


        #region Summary + Receipt


        string GetSummaryStepDataEditLink(CommonUiModel model)
        {
            return this.RenderPartialViewToString("Forms/SummaryStepDataEditLink", model);
        }

        [HttpPost]
        public ActionResult SummaryStepDataEdit(int uiIndex)
        {
            ViewModel.ComingFromSummary = true;

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult SaveAllStart()
        {
            ViewModel.SaveAll();

            return Json(new { receiptErrorMessages = ViewModel.ReceiptErrorMessages });
        }

        public FileContentResult DownloadPdf()
        {
            return new FileContentResult(FileService.GetBytesFromFile(ViewModel.ReceiptPdfFileName), "application/pdf")
            {
                FileDownloadName = "Ueberfuehrung.pdf"
            };
        }

        #endregion


        #region Common

        [HttpPost]
        public ActionResult NextStepView()
        {
            ViewModel.MoveToNextStep();
            if (ViewModel.GetStepModel() is CommonSummary)
                ViewModel.SaveSubModelWithPreservingUiModel(ViewModel.CreateSummaryModel(false, GetSummaryStepDataEditLink));

            if (ViewModel.ComingFromSummary)
            {
                ViewModel.ComingFromSummary = false;
                ViewModel.MoveToSummaryStep();
                ViewModel.SaveSubModelWithPreservingUiModel(ViewModel.CreateSummaryModel(false, GetSummaryStepDataEditLink));
            }

            return PartialView("CurrentStepView", ViewModel);
        }

        [HttpPost]
        public ActionResult CurrentStepView()
        {
            return PartialView(ViewModel);
        }

        private PartialViewResult GetStepPartialView()
        {
            return PartialView(ViewModel.StepCurrentFormPartialViewName, ViewModel.StepCurrentModel);
        }

        #endregion
    }
}
