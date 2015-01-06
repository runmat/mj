﻿using System.Web.Mvc;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.AutohausFahrzeugdaten.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// AutohausFahrzeugdaten-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class AutohausFahrzeugdatenController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<FahrzeugverwaltungViewModel>(); } }

        public AutohausFahrzeugdatenController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IUploadFahrzeugdatenDataService uploadFahrzeugdatenDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(UploadFahrzeugdatenViewModel, appSettings, logonContext, uploadFahrzeugdatenDataService);
        }

        public ActionResult Index()
        {
            return RedirectToAction("UploadFahrzeugdaten");
        }

    }
}
