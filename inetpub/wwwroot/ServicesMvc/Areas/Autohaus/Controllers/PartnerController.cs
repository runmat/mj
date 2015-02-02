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
using CkgDomainLogic.Partner.Contracts;
using CkgDomainLogic.Partner.Models;
using CkgDomainLogic.Partner.ViewModels;
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
    public class PartnerController : CkgDomainController
    {
        public override string DataContextKey { get { return "PartnerViewModel"; } }

        public PartnerViewModel ViewModel { get { return GetViewModel<PartnerViewModel>(); } }

        public override AdressenPflegeViewModel AdressenPflegeViewModel { get { return ViewModel; } }


        public PartnerController(IAppSettings appSettings, ILogonContextDataService logonContext, IPartnerDataService partnerDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, partnerDataService);
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
            {
                ViewModel.LoadPartners();
                if (ViewModel.Partners.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/PartnerSuche", ViewModel.PartnerSelektor);
        }


        #region Grid

        [HttpPost]
        public ActionResult ShowPartnerGrid()
        {
            ViewData["ZulassungAvailable"] = ViewModel.PartnerSelektor.PartnerKennung.ToUpper() == "HALTER";
            return PartialView("../Partner/AdressenPflege/AdressenGrid", ViewModel);
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
