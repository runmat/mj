// ReSharper disable RedundantUsingDirective
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
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

namespace ServicesMvc.Autohaus.Controllers
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
                ViewModel.LoadFahrzeuge();

            return PartialView("Partial/FahrzeugAkteBestandSuche", ViewModel.FahrzeugAkteBestandSelektor);
        }


        [HttpPost]
        public ActionResult FinSearchFormSubmit(FahrzeugAkteBestandSelektor model)
        {
            if (model.TmpEnforcePartnerDropdownRefresh)
            {
                ViewModel.DataMarkForRefreshPartnerAdressen();
                return PartialView("Partial/FahrzeugAkteBestandDetailsFinSuche", model);
            }

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

            ViewModel.TryLoadFahrzeugDetailsUsingFin(finToLoad);

            return PartialView("Partial/FahrzeugAkteBestandDetails", ViewModel.CurrentFahrzeug);
        }

        [HttpPost]
        public ActionResult UpdateFahrzeugDetails(FahrzeugAkteBestand model)
        {
            if (!ModelState.IsValid)
                return PartialView("Partial/FahrzeugAkteBestandDetails", model);

            ViewModel.UpdateFahrzeugDetails(model, model.FIN, ModelState.AddModelError);

            if (ModelState.IsValid)
                ModelState.Clear();
                
            return PartialView("Partial/FahrzeugAkteBestandDetails", ViewModel.CurrentFahrzeug);
        }

        [HttpPost]
        public ActionResult GetTypDaten(string herstellerSchluessel, string typSchluessel, string vvsSchluessel)
        {
            var model = ViewModel.GetTypDaten(herstellerSchluessel, typSchluessel, vvsSchluessel);

            return Json(new
                {
                    success = (model != null),
                    fabrikName = (model ?? new FahrzeugAkteBestand()).FabrikName,
                    handelsName = (model ?? new FahrzeugAkteBestand()).HandelsName
                });
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

        [HttpPost]
        public JsonResult FahrzeugAuswahlSelectionChanged(string vin, bool isChecked)  
        {
            int allSelectionCount, allCount = 0;
            if (vin.IsNullOrEmpty())
                ViewModel.SelectFahrzeuge(isChecked, f => true, out allSelectionCount, out allCount);
            else
                ViewModel.SelectFahrzeug(vin, isChecked, out allSelectionCount);

            return Json(new
            {
                allSelectionCount,
                allCount,
                zulassungenAnzahlPdiTotal = ViewModel.FahrzeugeSelected,    
                zulassungenAnzahlGesamtTotal = ViewModel.FahrzeugeTotal,   
            });
        }

        #endregion


        public ActionResult MultiReg()
        {
            var selectedFahrzeuge = ViewModel.FahrzeugeAkteBestandFiltered.Where(x => x.IsSelected).Select(x => x.FIN).ToArray();

            TempData["SelectedFahrzeuge"] = selectedFahrzeuge;

            return RedirectToAction("IndexMultiReg", "Zulassung");

            // return RedirectToAction("IndexMultiReg", "Zulassung", new { vin = selectedFahrzeuge });
            // vorhanden...
            // IAppSettings appSettings, 
            // ILogonContextDataService logonContext, 
            // IFahrzeugAkteBestandDataService fahrzeugbestandDataService
            // erwartet...
            // IAppSettings appSettings, 
            // ILogonContextDataService logonContext, 
            // IPartnerDataService partnerDataService, 
            // IZulassungDataService zulassungDataService, 
            // IFahrzeugAkteBestandDataService fahrzeugbestandDataService
        }

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

            return PartialView("Partial/PartnerAdressenGrid", ViewModel);
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
