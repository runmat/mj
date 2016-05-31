using System;
using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Partner.Contracts;
using CkgDomainLogic.Partner.Models;
using CkgDomainLogic.Partner.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;
// ReSharper disable RedundantAnonymousTypePropertyName

namespace ServicesMvc.Autohaus.Controllers
{
    public class PartnerController : CkgDomainController
    {
        public override string DataContextKey { get { return "PartnerViewModel"; } }

        public PartnerViewModel ViewModel { get { return GetViewModel<PartnerViewModel>(); } }

        public override AdressenPflegeViewModel AdressenPflegeViewModel { get { return ViewModel; } }


        public PartnerController(IAppSettings appSettings, ILogonContextDataService logonContext, IPartnerDataService partnerDataService, IZulassungDataService zulassungDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, partnerDataService, zulassungDataService);
            InitModelStatics();
        }

        [CkgApplication]
        public ActionResult Index(string pid = null)
        {
            ViewModel.DataInit(pid);

            return View("Pflege", ViewModel);
        }

        [CkgApplication]
        public ActionResult Pflege(string pid = null)
        {
            ViewModel.DataInit(pid);

            return View(ViewModel);
        }

        void InitModelStatics()
        {
            PartnerSelektor.GetViewModel = GetViewModel<PartnerViewModel>;
        }

        [HttpPost]
        public ActionResult LoadPartners(PartnerSelektor model)
        {
            ViewModel.PartnerSelektor = model;

            ViewModel.ValidateSearch(ModelState.AddModelError);

            if (ModelState.IsValid)
                ViewModel.LoadPartners();

            return PartialView("Partial/PartnerSuche", ViewModel.PartnerSelektor);
        }

        [HttpPost]
        public ActionResult LoadBankdatenAusIban(string iban)
        {
            // ReSharper disable once NotAccessedVariable
            string errorMessage;
            var bankdaten = ViewModel.LoadBankdatenAusIban(iban, (prop, message) => errorMessage = message);

            return Json(new { Swift = bankdaten.Swift, KontoNr = bankdaten.KontoNr, Bankleitzahl = bankdaten.Bankleitzahl, Geldinstitut = bankdaten.Geldinstitut });
        }


        #region Grid

        [HttpPost]
        public ActionResult ShowPartnerGrid()
        {            
            ViewData["ZulassungAvailable"] = ViewModel.PartnerSelektor.PartnerKennung.ToUpper() == "HALTER";
            return PartialView("../Partner/AdressenPflege/AdressenGrid", ViewModel);
        }

        public ActionResult AdressenLoeschen()
        {
            ViewModel.DeleteItems(ModelState);
          
            return PartialView("../Partner/AdressenPflege/AdressenGrid", ViewModel);
        }
        


        [HttpPost]
        public ActionResult ShowAdressenGrid()
        {
            // TODO (ggf. oder das hier löschen) -> dürfen nur Halter Fzg. zulassen? 
            
            // ViewData["ZulassungAvailable"] = ViewModel.PartnerSelektor.PartnerKennung.ToUpper() == "HALTER";

            ViewModel.LoadPartners();

            return PartialView("../Partner/AdressenPflege/AdressenGrid", ViewModel);
        }


        [HttpPost]
        public JsonResult AdsressenAuswahlSelectionChanged(string id, bool isChecked)
        {
            int allSelectionCount, allCount = 0, allFoundCount = 0;
            if (id.IsNotNullOrEmpty())
                ViewModel.SelectAdresse(id, isChecked, out allSelectionCount);
            else                
                ViewModel.SelectAdressen(isChecked, out allSelectionCount, out allCount, out allFoundCount);

            return Json(new { allSelectionCount, allCount, allFoundCount });
        }


        [GridAction]
        public ActionResult PartnerAjaxBinding()
        {
            return View(new GridModel(ViewModel.PartnersFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridPartner(string filterValue, string filterColumns)
        {
            ViewModel.FilterPartners(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.PartnersFiltered;
        }

        #endregion
    }
}
