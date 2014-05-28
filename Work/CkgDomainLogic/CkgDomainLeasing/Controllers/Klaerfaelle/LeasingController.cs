using System;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Leasing.Models;
using CkgDomainLogic.Leasing.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;
using CkgDomainLogic.General.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Leasing-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class LeasingController  
    {
        public LeasingKlaerfaelleViewModel KlaerfaelleViewModel { get { return GetViewModel<LeasingKlaerfaelleViewModel>(); } }

        #region Report Klärfälle P&G

        [CkgApplication]
        public ActionResult ReportKlaerfaellePuG()
        {
            KlaerfaelleViewModel.LoadKlaerfaelle(new KlaerfallSuchparameter{ Klaerfaelle = true });
            if (KlaerfaelleViewModel.Klaerfaelle.Count == 0)
            {
                ModelState.AddModelError(String.Empty, Localize.NoDataFound);
            }

            return View(KlaerfaelleViewModel);
        }

        [HttpPost]
        public ActionResult ShowKlaerfaellePuG()
        {
            return PartialView("Klaerfaelle/KlaerfaellePuGGrid", KlaerfaelleViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridKlaerfaellePuG(string filterValue, string filterColumns)
        {
            KlaerfaelleViewModel.FilterKlaerfaelle(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        #region Report Fehlende Daten P&G

        [CkgApplication]
        public ActionResult ReportFehlendeDatenPuG()
        {
            return View(KlaerfaelleViewModel);
        }

        [HttpPost]
        public ActionResult LoadFehlendeDatenPuG(KlaerfallSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                KlaerfaelleViewModel.LoadKlaerfaelle(model);
                if (KlaerfaelleViewModel.Klaerfaelle.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Klaerfaelle/FehlendeDatenPuGSuche", model);
        }

        [HttpPost]
        public ActionResult ShowFehlendeDatenPuG()
        {
            return PartialView("Klaerfaelle/FehlendeDatenPuGGrid", KlaerfaelleViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridFehlendeDatenPuG(string filterValue, string filterColumns)
        {
            KlaerfaelleViewModel.FilterKlaerfaelle(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        #region Report Anzeige Daten P&G

        [CkgApplication]
        public ActionResult ReportAnzeigeDatenPuG()
        {
            return View(KlaerfaelleViewModel);
        }

        [HttpPost]
        public ActionResult LoadAnzeigeDatenPuG(KlaerfallSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                KlaerfaelleViewModel.LoadKlaerfaelle(model);
                if (KlaerfaelleViewModel.Klaerfaelle.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Klaerfaelle/AnzeigeDatenPuGSuche", model);
        }

        [HttpPost]
        public ActionResult ShowAnzeigeDatenPuG()
        {
            return PartialView("Klaerfaelle/AnzeigeDatenPuGGrid", KlaerfaelleViewModel);
        }

        [HttpPost]
        public ActionResult ShowKlaerfallDetails(string leasingvertragsnummer)
        {
            Klaerfall item = KlaerfaelleViewModel.Klaerfaelle.Find(k => k.Leasingvertragsnummer == leasingvertragsnummer);

            return PartialView("Klaerfaelle/AnzeigeDatenPuGDetails", item);
        }

        [HttpPost]
        public ActionResult FilterGridAnzeigeDatenPuG(string filterValue, string filterColumns)
        {
            KlaerfaelleViewModel.FilterKlaerfaelle(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        [GridAction]
        public ActionResult KlaerfaelleAjaxBinding()
        {
            var items = KlaerfaelleViewModel.KlaerfaelleFiltered; 

            return View(new GridModel(items));
        }

        public ActionResult ExportKlaerfaelleFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = KlaerfaelleViewModel.KlaerfaelleFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Klaerfaelle_PuG", dt);

            return new EmptyResult();
        }

        public ActionResult ExportKlaerfaelleFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = KlaerfaelleViewModel.KlaerfaelleFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Klaerfaelle_PuG", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
