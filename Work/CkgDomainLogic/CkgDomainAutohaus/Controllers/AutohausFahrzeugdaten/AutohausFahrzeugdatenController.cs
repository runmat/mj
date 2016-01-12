using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.AutohausFahrzeugdaten.ViewModels;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class AutohausFahrzeugdatenController
    {
        public UploadFahrzeugdatenViewModel UploadFahrzeugdatenViewModel { get { return GetViewModel<UploadFahrzeugdatenViewModel>(); } }

        [CkgApplication]
        public ActionResult UploadFahrzeugdaten()
        {
            UploadFahrzeugdatenViewModel.InitViewModel();

            return View(UploadFahrzeugdatenViewModel);
        }

        [CkgApplication]
        public ActionResult MappedUploadFahrzeugdaten()
        {
            UploadFahrzeugdatenViewModel.InitViewModel(true);

            return View("UploadFahrzeugdaten", UploadFahrzeugdatenViewModel);
        }

        #region MappingSelection

        [HttpPost]
        public ActionResult MappingSelection()
        {
            return PartialView("UploadFahrzeugdaten/MappingSelection", UploadFahrzeugdatenViewModel);
        }

        [HttpPost]
        public ActionResult MappingSelectionForm(MappedUploadMappingSelectionModel model)
        {
            if (ModelState.IsValid)
                UploadFahrzeugdatenViewModel.MappingSelectionModel.MappingId = model.MappingId;

            return PartialView("UploadFahrzeugdaten/MappingSelectionForm", model);
        }

        #endregion

        #region CsvUpload

        [HttpPost]
        public ActionResult FileUpload()
        {
            return PartialView("UploadFahrzeugdaten/FileUpload", UploadFahrzeugdatenViewModel);
        }

        [HttpPost]
        public ActionResult UploadFahrzeugdatenStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            ModelState.Clear();

            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            UploadFahrzeugdatenViewModel.ExcelUploadFileSave(file.FileName, file.SavePostedFile, ModelState);

            if (!ModelState.IsValid)
                return Json(new { success = false, message = string.Format("{0} ({1})", Localize.ErrorFileCouldNotBeSaved, 
                                                                                        ModelState.Values.First(m => m.Errors.Any()).Errors.First().ErrorMessage) }, "text/plain");

            return Json(new { success = true, message = "ok", uploadFileName = file.FileName }, "text/plain");
        }

        [HttpPost]
        public ActionResult UploadFahrzeugdatenShowGrid()
        {
            return PartialView("UploadFahrzeugdaten/UploadFahrzeugdatenGrid", UploadFahrzeugdatenViewModel);
        }

        [GridAction]
        public ActionResult UploadFahrzeugdatenAjaxBinding()
        {
            return View(new GridModel(UploadFahrzeugdatenViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadSaveFahrzeugdatenAjaxBinding()
        {
            return View(new GridModel(UploadFahrzeugdatenViewModel.UploadItemsFiltered));
        }

        [GridAction]
        public ActionResult UploadFahrzeugdatenAjaxUpdateItem(int DatensatzNr)
        {
            var item = UploadFahrzeugdatenViewModel.GetDatensatzById(DatensatzNr);
            if (TryUpdateModel(item))
            {
                UploadFahrzeugdatenViewModel.ApplyChangedData(item);
            }

            return View(new GridModel(UploadFahrzeugdatenViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadFahrzeugdatenAjaxDeleteItem(int DatensatzNr)
        {
            UploadFahrzeugdatenViewModel.RemoveDatensatzById(DatensatzNr);

            return View(new GridModel(UploadFahrzeugdatenViewModel.UploadItems));
        }

        [HttpPost]
        public ActionResult UploadFahrzeugdatenCheck()
        {
            UploadFahrzeugdatenViewModel.ValidateUploadItems();

            return PartialView("UploadFahrzeugdaten/UploadFahrzeugdatenGrid", UploadFahrzeugdatenViewModel);
        }

        [HttpPost]
        public ActionResult UploadFahrzeugdatenResetSubmitMode()
        {
            UploadFahrzeugdatenViewModel.ResetSubmitMode();

            return PartialView("UploadFahrzeugdaten/UploadFahrzeugdatenGrid", UploadFahrzeugdatenViewModel);
        }

        [HttpPost]
        public ActionResult UploadFahrzeugdatenSubmit()
        {
            UploadFahrzeugdatenViewModel.SaveUploadItems();

            return PartialView("UploadFahrzeugdaten/Receipt", UploadFahrzeugdatenViewModel);
        }

        public FileResult DownloadFahrzeugdatenCsvTemplate()
        {
            var pfad = Server.MapPath(Url.Content("/ServicesMvc/Documents/Templates/UploadFahrzeugdaten.csv"));
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, "UploadFahrzeugdaten.csv");
        }

        public FileResult DownloadFahrzeugdatenXlsTemplate()
        {
            var pfad = Server.MapPath(Url.Content("/ServicesMvc/Documents/Templates/UploadFahrzeugdaten.xls"));
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, "UploadFahrzeugdaten.xls");
        }

        [HttpPost]
        public ActionResult FilterGridUploadSaveFahrzeugdaten(string filterValue, string filterColumns)
        {
            UploadFahrzeugdatenViewModel.FilterUploadItems(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        #region Export

        public ActionResult ExportUploadSaveFahrzeugdatenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = UploadFahrzeugdatenViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("UploadFahrzeugdaten", dt);

            return new EmptyResult();
        }

        public ActionResult ExportUploadSaveFahrzeugdatenFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = UploadFahrzeugdatenViewModel.UploadItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("UploadFahrzeugdaten", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
