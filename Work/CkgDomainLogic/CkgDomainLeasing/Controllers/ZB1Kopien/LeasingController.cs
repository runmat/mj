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
        public LeasingZB1KopienViewModel ZB1KopienViewModel { get { return GetViewModel<LeasingZB1KopienViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportZB1Kopien()
        {
            return View(ZB1KopienViewModel);
        }

        [HttpPost]
        public ActionResult LoadZB1Kopien(ZB1KopieSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                ZB1KopienViewModel.LoadZB1Kopien(model);
                if (ZB1KopienViewModel.ZB1Kopien.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("ZB1Kopien/ZB1KopieSuche", model);
        }

        [HttpPost]
        public ActionResult ShowZB1Kopien()
        {
            return PartialView("ZB1Kopien/ZB1KopieGrid", ZB1KopienViewModel);
        }

        [GridAction]
        public ActionResult ZB1KopienAjaxBinding()
        {
            var items = ZB1KopienViewModel.ZB1KopienFiltered; 

            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterGridZB1Kopien(string filterValue, string filterColumns)
        {
            ZB1KopienViewModel.FilterZB1Kopien(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportZB1KopienFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ZB1KopienViewModel.ZB1KopienFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("ZB1Kopien", dt);

            return new EmptyResult();
        }

        public ActionResult ExportZB1KopienFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ZB1KopienViewModel.ZB1KopienFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("ZB1Kopien", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
