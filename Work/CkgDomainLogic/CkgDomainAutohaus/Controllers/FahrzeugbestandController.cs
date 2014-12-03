// ReSharper disable RedundantUsingDirective
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.Fahrzeugbestand.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using Telerik.Web.Mvc;
using System.Linq;
using Adresse = CkgDomainLogic.DomainCommon.Models.Adresse;

namespace ServicesMvc.Controllers
{
    public class FahrzeugbestandController : CkgDomainController
    {
        public override string DataContextKey { get { return "FahrzeugbestandViewModel"; } }

        public FahrzeugbestandViewModel ViewModel { get { return GetViewModel<FahrzeugbestandViewModel>(); } }

        public override AdressenPflegeViewModel AdressenPflegeViewModel { get { return ViewModel; } }


        public FahrzeugbestandController(IAppSettings appSettings, ILogonContextDataService logonContext, IFahrzeugAkteBestandDataService fahrzeugbestandDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, fahrzeugbestandDataService);
            InitModelStatics();
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult Partner()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        void InitModelStatics()
        {
            FahrzeugAkteBestand.GetViewModel = GetViewModel<FahrzeugbestandViewModel>;
            FahrzeugAkteBestandSelektor.GetViewModel = GetViewModel<FahrzeugbestandViewModel>;
        }


        #region Fahrzeug Akte / Bestand

        [HttpPost]
        public ActionResult LoadFahrzeugAkteBestand(FahrzeugAkteBestandSelektor model)
        {
            ViewModel.FahrzeugAkteBestandSelektor = model;

            ViewModel.ValidateSearch(ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                ViewModel.LoadFahrzeuge();
                if (ViewModel.FahrzeugeAkteBestand.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/FahrzeugAkteBestandSuche", ViewModel.FahrzeugAkteBestandSelektor);
        }


        [HttpPost]
        public ActionResult FinSearchFormSubmit(FahrzeugAkteBestandSelektor model)
        {
            ViewModel.FinSearchSelektor = model;
            ViewModel.ValidateFinSearch(ModelState.AddModelError);
            if (!ModelState.IsValid)
                return PartialView("Partial/FahrzeugAkteBestandDetailsFinSuche", model);

            return PartialView("Partial/FahrzeugAkteBestandDetailsFinSuche", ViewModel.FinSearchSelektor);
        }


        [HttpPost]
        public ActionResult ShowFahrzeugAkteBestandDetails(string finToLoad)
        {
            if (finToLoad.IsNullOrEmpty() || finToLoad == "null")
                return PartialView("Partial/FahrzeugAkteBestandDetailsFinSuche", ViewModel.FinSearchSelektor);

            if (finToLoad == "useModelFin")
                finToLoad = ViewModel.FinSearchSelektor.FIN;

            ViewModel.LoadFahrzeugDetailsUsingFin(finToLoad);

            return PartialView("Partial/FahrzeugAkteBestandDetails", ViewModel.CurrentFahrzeug);
        }

        [HttpPost]
        public ActionResult UpdateFahrzeugDetails(FahrzeugAkteBestand model)
        {
            if (!ModelState.IsValid)
                return PartialView("Partial/FahrzeugAkteBestandDetails", model);

            ViewModel.UpdateFahrzeugDetails(model, ModelState.AddModelError);

            return PartialView("Partial/FahrzeugAkteBestandDetails", ViewModel.CurrentFahrzeug);
        }


        #region Grid

        [HttpPost]
        public ActionResult ShowFahrzeugAkteBestandGrid()
        {
            return PartialView("Partial/FahrzeugAkteBestandGrid", ViewModel);
        }

        [GridAction]
        public ActionResult FahrzeugAkteBestandAjaxBinding()
        {
            return View(new GridModel(ViewModel.FahrzeugeAkteBestandFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugAkteBestand(string filterValue, string filterColumns)
        {
            ViewModel.FilterFahrzeugeAkteBestand(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.FahrzeugeAkteBestandFiltered;
        }

        #endregion

        #endregion


        #region Partner Adressen

        [CkgApplication]
        public ActionResult K()
        {
            AdressenPflegeViewModel.AdressenDataInit("KAEUFER", LogonContext.KundenNr);

            return View("TestAdressPflege");
        }

        [CkgApplication]
        public ActionResult H()
        {
            AdressenPflegeViewModel.AdressenDataInit("HALTER", LogonContext.KundenNr);

            return View("TestAdressPflege");
        }

        [HttpPost]
        public ActionResult PickPartnerAddress(string partnerKennung)
        {
            AdressenPflegeViewModel.AdressenDataInit(partnerKennung, LogonContext.KundenNr);

            return PartialView("Partial/PartnerAdressenGrid");
        }

        [HttpPost]
        public JsonResult PickPartnerAddressFinished(int id)
        {
            var selectedPartner = ViewModel.PickPartnerAddressFinished(id);

            return Json(new
                {
                    partnerKennung = AdressenPflegeViewModel.AdressenKennung,
                    partnerID = selectedPartner.KundenNr,
                    partnerName = selectedPartner.GetAutoSelectString()
                });
        }

        #endregion
    }
}
