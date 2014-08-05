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
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
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
                model = ViewModel.GetUebfuehrgAdresseFromKey(model.TmpSelectionKey);
                if (model == null)
                    return new EmptyResult();

                ModelState.Clear();
                model.IsValid = false;

                ViewModel.SaveSubModelWithPreservingUiModel(model);

                return GetStepPartialView();
            }

            model.IsValid = ModelState.IsValid;
            if (ModelState.IsValid)
                ViewModel.SaveSubModelWithPreservingUiModel(model);

            return GetStepPartialView();
        }

        [HttpPost]
        public ActionResult UebfuehrgAdressenShowGrid(int uiIndex)
        {
            ViewModel.DataMarkForRefreshUebfuehrgAdressenFiltered();

            ViewModel.StepCurrentIndex = uiIndex;
            ViewModel.FahrtAdressen.ForEach(adresse => adresse.UiIndex = ViewModel.StepCurrentIndex);

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

        public FileContentResult SummaryAsPdf()
        {
            var summaryHtml = this.RenderPartialViewToString("Partial/SummaryPdf", ViewModel.CreateSummaryModel(true, GetSummaryStepDataEditLink));

            var logoPath = AppSettings.LogoPath.IsNotNullOrEmpty() ? Server.MapPath(AppSettings.LogoPath) : "";
            var summaryPdfBytes = PdfDocumentFactory.HtmlToPdf(summaryHtml, logoPath, AppSettings.LogoPdfPosX, AppSettings.LogoPdfPosY);

            return new FileContentResult(summaryPdfBytes, "application/pdf") { FileDownloadName = "Übersicht.pdf" };
        }

        public ActionResult SummaryAsHtml()
        {
            return View("Partial/SummaryPdf", ViewModel.CreateSummaryModel(true, GetSummaryStepDataEditLink));
        }

        //[HttpPost]
        //public ActionResult Receipt()
        //{
        //    LogonContext.DataContextPersist(ViewModel);
        //    ViewModel.Save();

        //    return PartialView(ViewModel);
        //}

        #endregion


        #region Common

        [HttpPost]
        public ActionResult NextStepView()
        {
            ViewModel.MoveToNextStep();

            if (ViewModel.GetStepModel() is CommonSummary)
                ViewModel.SaveSubModelWithPreservingUiModel(ViewModel.CreateSummaryModel(false, GetSummaryStepDataEditLink));

            return PartialView("CurrentStepView", ViewModel);
        }

        [HttpPost]
        public ActionResult CurrentStepView()
        {
            return PartialView(ViewModel);
        }

        private PartialViewResult GetStepPartialView()
        {
            if (ViewModel.ComingFromSummary)
            {
                ViewModel.ComingFromSummary = false;
                ViewModel.MoveToSummaryStep();
            }

            return PartialView(ViewModel.StepCurrentFormPartialViewName, ViewModel.StepCurrentModel);
        }

        #endregion
    }
}
