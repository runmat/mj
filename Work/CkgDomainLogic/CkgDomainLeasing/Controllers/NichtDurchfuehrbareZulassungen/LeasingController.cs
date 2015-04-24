using System;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Leasing.ViewModels;
using GeneralTools.Models;
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
        public NichtDurchfuehrbZulViewModel NichtDurchfuehrbZulViewModel { get { return GetViewModel<NichtDurchfuehrbZulViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportNichtDurchfuehrbareZulassungen()
        {
            NichtDurchfuehrbZulViewModel.LoadNichtDurchfuehrbareZulassungen();

            if (NichtDurchfuehrbZulViewModel.NichtDurchfuehrbareZulassungen.None())
                ModelState.AddModelError(String.Empty, Localize.NoDataFound);

            return View(NichtDurchfuehrbZulViewModel);
        }

        [HttpPost]
        public ActionResult ShowNichtDurchfuehrbareZulassungen()
        {
            return PartialView("NichtDurchfuehrbareZulassungen/Grid", NichtDurchfuehrbZulViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridNichtDurchfuehrbareZulassungen(string filterValue, string filterColumns)
        {
            NichtDurchfuehrbZulViewModel.FilterNichtDurchfuehrbareZulassungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [GridAction]
        public ActionResult NichtDurchfuehrbareZulassungenAjaxBinding()
        {
            var items = NichtDurchfuehrbZulViewModel.NichtDurchfuehrbareZulassungenFiltered; 

            return View(new GridModel(items));
        }

        public ActionResult ExportNichtDurchfuehrbareZulassungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = NichtDurchfuehrbZulViewModel.NichtDurchfuehrbareZulassungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Leasing_NichtDurchfuehrbareZulassungen, dt);

            return new EmptyResult();
        }

        public ActionResult ExportNichtDurchfuehrbareZulassungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = NichtDurchfuehrbZulViewModel.NichtDurchfuehrbareZulassungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Leasing_NichtDurchfuehrbareZulassungen, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
