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


        public FahrzeugbestandController(IAppSettings appSettings, ILogonContextDataService logonContext, IFahrzeugbestandDataService fahrzeugbestandDataService, IAdressenDataService adressenDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, fahrzeugbestandDataService);
            InitViewModel(AdressenPflegeViewModel, appSettings, logonContext, adressenDataService);
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

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
            var selectedPartner = ViewModel.PickPartnerAddressFinished(AdressenPflegeViewModel.AdressenKennung, AdressenPflegeViewModel.GetItem(id));

            return Json(new
                {
                    partnerKennung = AdressenPflegeViewModel.AdressenKennung,
                    partnerName = selectedPartner.GetAutoSelectString()
                });
        }
    }
}
