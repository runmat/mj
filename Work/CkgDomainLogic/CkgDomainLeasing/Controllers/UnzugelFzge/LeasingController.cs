using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Leasing.Models;
using CkgDomainLogic.Leasing.ViewModels;
using Newtonsoft.Json;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Leasing-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class LeasingController  
    {
        public LeasingBriefeOhneLVNrViewModel BriefeOhneLVNrViewModel { get { return GetViewModel<LeasingBriefeOhneLVNrViewModel>(); } }

        [CkgApplication]
        public ActionResult FzgBriefeOhneLVNr()
        {
            BriefeOhneLVNrViewModel.LoadUnzugelFzge(ModelState);

            return View(BriefeOhneLVNrViewModel);
        }

        [GridAction]
        public ActionResult UnzugelFzgAjaxBinding()
        {
            return View(new GridModel(BriefeOhneLVNrViewModel.GridItems));
        }

        [GridAction]
        public ActionResult UnzugelFzgAjaxSaveChanges([Bind(Prefix = "inserted")]IEnumerable<UnzugelFzg> insertedFzg,
            [Bind(Prefix = "updated")]IEnumerable<UnzugelFzg> updatedFzg,
            [Bind(Prefix = "deleted")]IEnumerable<UnzugelFzg> deletedFzg)
        {
            BriefeOhneLVNrViewModel.ApplyLVNummern(updatedFzg);

            return View(new GridModel(BriefeOhneLVNrViewModel.GridItems));
        }

        [HttpPost]
        public ActionResult BriefeOhneLVNrSwitchToSubmitMode()
        {
            BriefeOhneLVNrViewModel.SwitchToSubmitMode();

            return PartialView("UnzugelFzg/UnzugelFzgGrid", BriefeOhneLVNrViewModel);
        }

        [HttpPost]
        public ActionResult BriefeOhneLVNrSubmitChanges()
        {
            BriefeOhneLVNrViewModel.SaveChangesToSap(ModelState);

            return PartialView("UnzugelFzg/UnzugelFzgGrid", BriefeOhneLVNrViewModel);
        }

        [HttpPost]
        public ActionResult BriefeOhneLVNrEditChanges()
        {
            BriefeOhneLVNrViewModel.ResetSubmitState();

            return PartialView("UnzugelFzg/UnzugelFzgGrid", BriefeOhneLVNrViewModel);
        }

        [HttpPost]
        public ActionResult BriefeOhneLVNrReinitModel()
        {
            BriefeOhneLVNrViewModel.ResetSubmitState();
            BriefeOhneLVNrViewModel.LoadUnzugelFzge(ModelState);

            return PartialView("UnzugelFzg/UnzugelFzgGrid", BriefeOhneLVNrViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridUnzugelFzg(string filterValue, string filterColumns)
        {
            BriefeOhneLVNrViewModel.FilterUnzugelFzge(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportUnzugelFzgFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = BriefeOhneLVNrViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("UnzugelFzg", dt);

            return new EmptyResult();
        }

        public ActionResult ExportUnzugelFzgFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = BriefeOhneLVNrViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("UnzugelFzg", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
