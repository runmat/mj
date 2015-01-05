﻿using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.UploadFahrzeugdaten.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Autohaus-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class AutohausController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<FahrzeugverwaltungViewModel>(); } }

        public AutohausController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IFahrzeugverwaltungDataService fahrzeugverwaltungDataService,
            IFahrzeugakteDataService fahrzeugakteDataService,
            ICustomerDocumentDataService customerDocumentDataService,
            IAutohausZulassungDataService autohausZulassungDataService,
            IAdressenDataService adressenDataService,
            IUploadFahrzeugdatenDataService uploadFahrzeugdatenDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(FahrzeugverwaltungViewModel, appSettings, logonContext, fahrzeugverwaltungDataService);

            InitViewModel(FahrzeugakteViewModel, appSettings, logonContext, fahrzeugakteDataService);

            if (FahrzeugakteViewModel.DocsViewModel == null)
                FahrzeugakteViewModel.DocsViewModel = new FahrzeugakteDocsViewModel();
            InitViewModel(FahrzeugakteViewModel.DocsViewModel, appSettings, logonContext, customerDocumentDataService);

            InitViewModel(ZulassungViewModel, appSettings, logonContext, fahrzeugakteDataService, adressenDataService, autohausZulassungDataService);

            InitViewModel(UploadFahrzeugdatenViewModel, appSettings, logonContext, uploadFahrzeugdatenDataService);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Fahrzeugverwaltung");
        }

    }
}
