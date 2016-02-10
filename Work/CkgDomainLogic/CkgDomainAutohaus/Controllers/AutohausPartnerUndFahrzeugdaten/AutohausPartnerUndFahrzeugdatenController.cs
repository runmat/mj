using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.ViewModels;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class AutohausPartnerUndFahrzeugdatenController
    {
        public UploadPartnerUndFahrzeugdatenViewModel ViewModel { get { return GetViewModel<UploadPartnerUndFahrzeugdatenViewModel>(); } }

        [CkgApplication]
        public ActionResult MappedUploadPartnerdaten()
        {
            ViewModel.InitViewModel(UploadPartnerUndFahrzeugdatenViewModel.UploadModus.PartnerUpload);

            return View("Index", ViewModel);
        }

        [CkgApplication]
        public ActionResult MappedUploadFahrzeugdaten()
        {
            ViewModel.InitViewModel(UploadPartnerUndFahrzeugdatenViewModel.UploadModus.FahrzeugUpload);

            return View("Index", ViewModel);
        }

        [CkgApplication]
        public ActionResult MappedUploadPartnerUndFahrzeugdaten()
        {
            ViewModel.InitViewModel(UploadPartnerUndFahrzeugdatenViewModel.UploadModus.PartnerUndFahrzeugUpload);

            return View("Index", ViewModel);
        }

        #region MappingSelection

        [HttpPost]
        public ActionResult MappingSelection()
        {
            return PartialView("Partial/MappingSelection", ViewModel);
        }

        [HttpPost]
        public ActionResult MappingSelectionForm(MappedUploadMappingSelectionModel model)
        {
            if (ModelState.IsValid)
                ViewModel.MappingSelectionModel.MappingId = model.MappingId;

            return PartialView("Partial/MappingSelectionForm", model);
        }

        #endregion

        #region Upload

        [HttpPost]
        public ActionResult FileUpload()
        {
            return PartialView("Partial/FileUpload", ViewModel);
        }

        [HttpPost]
        public ActionResult UploadStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            ModelState.Clear();

            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            ViewModel.ExcelUploadFileSave(file.FileName, file.SavePostedFile, ModelState);

            if (!ModelState.IsValid)
                return Json(new { success = false, message = string.Format("{0} ({1})", Localize.ErrorFileCouldNotBeSaved, 
                                                                                        ModelState.Values.First(m => m.Errors.Any()).Errors.First().ErrorMessage) }, "text/plain");

            return Json(new { success = true, message = "ok", uploadFileName = file.FileName }, "text/plain");
        }

        [HttpPost]
        public ActionResult ShowGrid()
        {
            return PartialView("Partial/Grid", ViewModel);
        }

        [GridAction]
        public ActionResult UploadPartnerUndFahrzeugdatenAjaxBinding()
        {
            return View(new GridModel(ViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadSavePartnerUndFahrzeugdatenAjaxBinding()
        {
            return View(new GridModel(ViewModel.UploadItemsFiltered));
        }

        [GridAction]
        public ActionResult UploadPartnerUndFahrzeugdatenAjaxUpdateItem(int DatensatzNr)
        {
            var item = ViewModel.GetDatensatzById(DatensatzNr);

            if (item is UploadPartnerdaten)
                UpdateModel((item as UploadPartnerdaten));
            else if (item is UploadFahrzeugdaten)
                UpdateModel((item as UploadFahrzeugdaten));
            else if (item is UploadPartnerUndFahrzeugdaten)
                UpdateModel((item as UploadPartnerUndFahrzeugdaten));

            return View(new GridModel(ViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadPartnerUndFahrzeugdatenAjaxDeleteItem(int DatensatzNr)
        {
            ViewModel.RemoveDatensatzById(DatensatzNr);

            return View(new GridModel(ViewModel.UploadItems));
        }

        [HttpPost]
        public ActionResult CheckUpload()
        {
            ViewModel.ValidateUploadItems();

            return PartialView("Partial/Grid", ViewModel);
        }

        [HttpPost]
        public ActionResult ResetSubmitMode()
        {
            ViewModel.ResetSubmitMode();

            return PartialView("Partial/Grid", ViewModel);
        }

        [HttpPost]
        public ActionResult SubmitUpload()
        {
            ViewModel.SaveUploadItems();

            return PartialView("Partial/Receipt", ViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridSaveUploadPartnerUndFahrzeugdaten(string filterValue, string filterColumns)
        {
            ViewModel.FilterUploadItems(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        #region Export

        public ActionResult ExportUploadItemsFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(ViewModel.ModusName, dt);

            return new EmptyResult();
        }

        public ActionResult ExportUploadItemsFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(ViewModel.ModusName, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
