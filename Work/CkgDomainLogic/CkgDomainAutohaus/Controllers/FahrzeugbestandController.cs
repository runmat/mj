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


        public FahrzeugbestandController(IAppSettings appSettings, ILogonContextDataService logonContext, IFahrzeugAkteBestandDataService fahrzeugbestandDataService, IAdressenDataService adressenDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, fahrzeugbestandDataService);
            InitViewModel(AdressenPflegeViewModel, appSettings, logonContext, adressenDataService);
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();
            //ViewModel.LoadFahrzeugAkteBestand();

            return View(ViewModel);
        }


        #region Fahrzeug Akte / Bestand


        [HttpPost]
        public ActionResult LoadFahrzeugAkteBestand(FahrzeugAkteBestandSelektor model)
        {
            ViewModel.FahrzeugAkteBestandSelektor = model;

            ViewModel.Validate(ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                ViewModel.LoadFahrzeugAkteBestand();
                if (ViewModel.FahrzeugeAkteBestand.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/FahrzeugAkteBestandSuche", ViewModel.FahrzeugAkteBestandSelektor);
        }

        [HttpPost]
        public ActionResult ShowFahrzeugAkteBestand()
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
            AdressenPflegeViewModel.DataInit("KÄUFER", LogonContext.KundenNr);

            return View("TestAdressPflege");
        }

        [CkgApplication]
        public ActionResult H()
        {
            AdressenPflegeViewModel.DataInit("HALTER", LogonContext.KundenNr);

            return View("TestAdressPflege");
        }

        [HttpPost]
        public ActionResult PickPartnerAddress(string partnerKennung)
        {
            AdressenPflegeViewModel.DataInit(partnerKennung, LogonContext.KundenNr);

            return PartialView("Partial/PartnerAdressenGrid");
        }

        [HttpPost]
        public JsonResult PickPartnerAddressFinished(int id)
        {
            var selectedPartner = ViewModel.PickPartnerAddressFinished(AdressenPflegeViewModel.AdressenKennung,
                                                                       AdressenPflegeViewModel.GetItem(id));

            return Json(new
                {
                    partnerKennung = AdressenPflegeViewModel.AdressenKennung,
                    partnerName = selectedPartner.GetAutoSelectString()
                });
        }

        #endregion
    }
}
