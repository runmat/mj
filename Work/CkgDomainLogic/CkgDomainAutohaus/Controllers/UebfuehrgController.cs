// ReSharper disable RedundantUsingDirective
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using CkgDomainLogic.Uebfuehrg.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using Telerik.Web.Mvc;
using System.Linq;
using Adresse = CkgDomainLogic.Uebfuehrg.Models.Adresse;
using Fahrzeug = CkgDomainLogic.Uebfuehrg.Models.Fahrzeug;

// ReSharper restore RedundantUsingDirective

namespace ServicesMvc.Controllers
{
    public partial class UebfuehrgController : CkgDomainController
    {
        public override string DataContextKey { get { return "UebfuehrgViewModel"; } }

        public UebfuehrgViewModel ViewModel { get { return GetViewModel<UebfuehrgViewModel>(); } }
        public UebfuehrgReportViewModel ReportViewModel { get { return GetViewModel<UebfuehrgReportViewModel>(); } }


        public UebfuehrgController(IAppSettings appSettings, ILogonContextDataService logonContext, IUebfuehrgDataService uebfuehrgDataService, IFahrzeugverwaltungDataService fahrzeugverwaltungDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, uebfuehrgDataService, fahrzeugverwaltungDataService);
            InitViewModel(ReportViewModel, appSettings, logonContext, uebfuehrgDataService);
        }
    }
}
