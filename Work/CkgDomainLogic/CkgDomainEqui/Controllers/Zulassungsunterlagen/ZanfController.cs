using System;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Zanf.Models;
using CkgDomainLogic.Zanf.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class ZanfController  
    {
        public ZulassungsunterlagenViewModel ZulassungsunterlagenViewModel { get { return GetViewModel<ZulassungsunterlagenViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportZulassungsunterlagen()
        {
            return View(ZulassungsunterlagenViewModel);
        }

        [HttpPost]
        public ActionResult LoadZulassungsUnterlagen(ZulassungsUnterlagenSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                ZulassungsunterlagenViewModel.LoadZulassungsUnterlagen(model);
                if (ZulassungsunterlagenViewModel.ZulassungsUnterlagen.Count == 0)
                {
                    ModelState.AddModelError(String.Empty, Localize.NoDataFound);
                }
            }

            return PartialView("Zulassungsunterlagen/Suche", model);
        }

        [HttpPost]
        public ActionResult ShowZulassungsUnterlagen()
        {
            return PartialView("Zulassungsunterlagen/Grid", ZulassungsunterlagenViewModel);
        }

        [GridAction]
        public ActionResult ZulassungsUnterlagenAjaxBinding()
        {
            return View(new GridModel(ZulassungsunterlagenViewModel.ZulassungsUnterlagenFiltered));
        }

        [HttpPost]
        public ActionResult ShowZulassungsUnterlagenEdit(string halterId)
        {
            var zulUnt = ZulassungsunterlagenViewModel.ZulassungsUnterlagen.Find(z => z.HalterId == halterId);

            return PartialView("Zulassungsunterlagen/Edit", zulUnt);
        }

        [HttpPost]
        public ActionResult SaveZulassungsUnterlagen(ZulassungsUnterlagen model)
        {
            if (ModelState.IsValid)
            {
                ZulassungsunterlagenViewModel.SaveZulassungsUnterlagen(model, ModelState);
            }

            return PartialView("Zulassungsunterlagen/Edit", model);
        }

        public FileContentResult PdfDocumentDownload(string docId)
        {
            var pdfBytes = ZulassungsunterlagenViewModel.GetUnterlagenAsPdf(docId);

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.RegistrationDocuments) };
        }

        [HttpPost]
        public ActionResult FilterGridZulassungsUnterlagen(string filterValue, string filterColumns)
        {
            ZulassungsunterlagenViewModel.FilterZulassungsUnterlagen(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportZulassungsUnterlagenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungsunterlagenViewModel.ZulassungsUnterlagenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.RegistrationDocuments, dt);

            return new EmptyResult();
        }

        public ActionResult ExportZulassungsUnterlagenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ZulassungsunterlagenViewModel.ZulassungsUnterlagenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.RegistrationDocuments, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
