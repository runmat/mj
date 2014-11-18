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
    public partial class UebfuehrgController 
    {
        [CkgApplication]
        public ActionResult Report()
        {
            ReportViewModel.DataInit();
            return View();
        }
        [HttpPost]
        public ActionResult LoadHistoryAuftraege(HistoryAuftragSelector model)
        {
            ReportViewModel.HistoryAuftragSelector = model;

            ReportViewModel.Validate(ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                ReportViewModel.LoadHistoryAuftraege();
                if (ReportViewModel.HistoryAuftraege.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/SucheHistoryAuftrag", ReportViewModel.HistoryAuftragSelector);
        }

        [HttpPost]
        public ActionResult ShowHistoryAuftraege()
        {
            return PartialView("Partial/HistoryAuftragGrid", ReportViewModel);
        }

        [GridAction]
        public ActionResult HistoryAuftraegeAjaxBinding()
        {
            return View(new GridModel(ReportViewModel.HistoryAuftraegeFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridHistoryAuftrag(string filterValue, string filterColumns)
        {
            ReportViewModel.FilterHistoryAuftraege(filterValue, filterColumns);

            return new EmptyResult();
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ReportViewModel.HistoryAuftraegeFiltered;
        }

        #endregion    
    }
}
