using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using MvcTools.Web;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController : CkgDomainController
    {
        public FehlteilEtikettenViewModel FehlteilEtikettenViewModel { get { return GetViewModel<FehlteilEtikettenViewModel>(); } }

        [CkgApplication]
        public ActionResult FehlteilEtikettenCsvUpload()
        {
            _dataContextKey = typeof(FehlteilEtikettenViewModel).Name;
            FehlteilEtikettenViewModel.DataInit();

            return View(FehlteilEtikettenViewModel);
        }

        [CkgApplication]
        public ActionResult FehlteilEtikettenEditAndPrint()
        {
            _dataContextKey = typeof(FehlteilEtikettenViewModel).Name;
            FehlteilEtikettenViewModel.DataInit();

            return View(FehlteilEtikettenViewModel);
        }


        #region FehlteilEtikettenCsvUpload

        [HttpPost]
        public ActionResult CsvUploadStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            // Step 1:  Upload the CSV file

            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = "Fehler: Keine Datei angegeben!" }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!FehlteilEtikettenViewModel.CsvUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = "Fehler: CSV Datei konnte nicht gespeichert werden!" }, "text/plain");

            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult CsvUploadShowGrid(bool showErrorsOnly)
        {
            // Step 2:  Show CSV data in a grid for user validation

            FehlteilEtikettenViewModel.UploadItemsShowErrorsOnly = showErrorsOnly;
            if (showErrorsOnly && FehlteilEtikettenViewModel.UploadItemsFiltered.None())
                FehlteilEtikettenViewModel.UploadItemsShowErrorsOnly = false;

            return PartialView("FehlteilEtikettenCsvUpload/ValidationGrid", FehlteilEtikettenViewModel);
        }
        
        [GridAction]
        public ActionResult FehlteilEtikettenAjaxSelect()
        {
            return View(new GridModel(FehlteilEtikettenViewModel.UploadItemsFiltered));
        }
        
        [HttpPost]
        [GridAction]
        public ActionResult FehlteilEtikettenAjaxUpdate(int id)
        {
            var itemToUpdate = FehlteilEtikettenViewModel.UploadItemsFiltered.FirstOrDefault(p => p.ID == id);
            TryUpdateModel(itemToUpdate);
            FehlteilEtikettenViewModel.ValidateUploadItems();

            return View(new GridModel(FehlteilEtikettenViewModel.UploadItemsFiltered));
        }
        
        [HttpPost]
        public ActionResult CsvUploadSubmit()
        {
            // Step 3:  Save CSV data to data store

            FehlteilEtikettenViewModel.SaveUploadItems();

            return PartialView("FehlteilEtikettenCsvUpload/Receipt", FehlteilEtikettenViewModel);
        }

        #endregion


        #region FehlteilEtikettPrintEdit

        [HttpPost]
        public ActionResult LoadFehlteilEtikett(FehlteilEtikettSelektor model)
        {
            FehlteilEtikettenViewModel.FehlteilEtikettSelektor = model;

            if (ModelState.IsValid)
            {
                FehlteilEtikettenViewModel.LoadFehlteilEtikett();
                if (FehlteilEtikettenViewModel.FehlteilEtikett == null)
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("FehlteilEtikettenEditAndPrint/Suche", FehlteilEtikettenViewModel.FehlteilEtikettSelektor);
        }

        [HttpPost]
        public ActionResult ShowFehlteilEtikett()
        {
            return PartialView("FehlteilEtikettenEditAndPrint/DetailsForm", FehlteilEtikettenViewModel.FehlteilEtikett);
        }

        [HttpPost]
        public ActionResult LabelPositionSave(int labelPos)
        {
            FehlteilEtikettenViewModel.FehlteilEtikett.LayoutPosition = labelPos;
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DetailsFormSave(FehlteilEtikett model)
        {
            // Avoid ModelState clearing on saving 
            // => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();

            FehlteilEtikettenViewModel.ValidateFehlteilEtikett(model, ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                model = FehlteilEtikettenViewModel.SaveItem(model, ModelState.AddModelError);
            }

            model.IsValid = ModelState.IsValid;

            return PartialView("FehlteilEtikettenEditAndPrint/DetailsForm", model);
        }

        [HttpGet]
        public ActionResult GetEtikettAsPdf(string vin)
        {
            string errorMessage;
            byte[] pdfBytes;
            FehlteilEtikettenViewModel.GetEtikettAsPdf(vin, out errorMessage, out pdfBytes);

            if (pdfBytes == null || errorMessage.IsNotNullOrEmpty())
                return new ContentResult{ Content = errorMessage.IsNotNullOrEmpty() ? errorMessage : "Fehler, PDF Datei konnte nicht erzeugt werden." };
            
            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = "Fehlteiletikett.pdf" };
        }

        #endregion
    }
}
