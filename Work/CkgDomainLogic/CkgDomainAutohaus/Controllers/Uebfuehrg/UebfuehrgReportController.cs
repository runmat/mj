// ReSharper disable RedundantUsingDirective

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
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

namespace ServicesMvc.Controllers
{
    public partial class UebfuehrgController 
    {
        [CkgApplication]
        public ActionResult Report()
        {
            ReportViewModel.DataInit();
            return View(ReportViewModel);
        }

        [HttpPost]
        public ActionResult LoadHistoryAuftraege(HistoryAuftragSelector model)
        {
            model.GetKundenAusHierarchie = ReportViewModel.HistoryAuftragSelector.GetKundenAusHierarchie;
            ReportViewModel.HistoryAuftragSelector = model;

            if (ModelState.IsValid && !PersistableMode)
            {
                bool dateRangeResetOccurred;
                ReportViewModel.Validate(ModelState.AddModelError, out dateRangeResetOccurred);

                ReportViewModel.LoadHistoryAuftraege();
                if (ReportViewModel.HistoryAuftraege.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
                else
                    if (dateRangeResetOccurred)
                        ModelState.Clear();
            }

            return PersistablePartialView("Partial/HistoryAuftragSuche", ReportViewModel.HistoryAuftragSelector);
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
        public ActionResult LoadHistoryAuftragDetails(string auftragsNr, string fahrt)
        {
            ReportViewModel.LoadHistoryAuftragDetails(auftragsNr, fahrt, Server.MapToUrl);

            return PartialView("Partial/HistoryAuftragDetailsWithDocuments", ReportViewModel);
        }

        [HttpPost]
        public ActionResult HistoryAuftragShowBigImage(int tour, int fileNr)
        {
            ReportViewModel.CopySingleBigImage(tour, fileNr);

            return PartialView("Partial/HistoryAuftragDetailsBigImageDialog", ReportViewModel.GetImageFileNameForIndex(tour, fileNr));
        }

        public ActionResult HistoryDownloadPdfFiles()
        {
            var zipFileName = ReportViewModel.GetPdfFilesAsZip();
            if (zipFileName.IsNullOrEmpty())
                return new EmptyResult();

            return new FileContentResult(FileService.GetBytesFromFile(zipFileName), "application/zip")
            {
                FileDownloadName = FileService.PathGetFileName(zipFileName)
            };
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
