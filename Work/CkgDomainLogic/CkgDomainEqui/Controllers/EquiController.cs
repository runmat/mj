﻿using System.Web.Mvc;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class EquiController : CkgDomainController
    {
        public override string DataContextKey { get { return "EquiViewModel"; } }


        public EquiController(IAppSettings appSettings, ILogonContextDataService logonContext, IEquiGrunddatenDataService equiGrunddatenDataService, IEquiHistorieDataService equiHistorieDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(EquiGrunddatenEquiViewModel, appSettings, logonContext, equiGrunddatenDataService);
            InitViewModel(EquipmentHistorieViewModel, appSettings, logonContext, equiHistorieDataService);
        }

        public ActionResult Index()
        {
            return RedirectToAction("ReportFahrzeugbestand");
        }
    }
}
