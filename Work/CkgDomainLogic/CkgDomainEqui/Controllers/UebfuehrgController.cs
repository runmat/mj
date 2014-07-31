// ReSharper disable RedundantUsingDirective
using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using CkgDomainLogic.Uebfuehrg.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Models;
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

            if (ModelState.IsValid)
            {
                ViewModel.SaveSubModelWithPreservingUiModel(model);
            }

            return GetStepPartialView();
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


        #region Common

        [HttpPost]
        public ActionResult NextStepView()
        {
            ViewModel.MoveToNextStep();

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
