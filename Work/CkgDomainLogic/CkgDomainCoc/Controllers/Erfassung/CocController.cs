using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Models;
using MvcTools.Web;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// COC Erfassung
    /// </summary>
    public partial class CocController
    {
        [CkgApplication]
        public ActionResult Erfassung()
        {
            CocTypenViewModel.DataMarkForRefresh();

            return View(CocTypenViewModel);
        }

        [CkgApplication]
        public ActionResult CsvUpload()
        {
            CocErfassungViewModel.DataMarkForRefresh();

            return View(CocErfassungViewModel);
        }


        #region Coc Erfassung

        [HttpPost]
        public ActionResult NewCocOrder(string typ, string variante, string version)
        {
            ModelState.Clear();

            CocErfassungViewModel.InsertMode = true;

            var cocTyp = CocTypenViewModel.CocTypen.FirstOrDefault(c => c.COC_0_2_TYP == typ && c.COC_0_2_VAR == variante && c.COC_0_2_VERS == version);

            CocErfassungViewModel.CreateNewCocOrder(cocTyp, typ, variante, version);

            return PartialView("Partial/CocDetailsForm", CocErfassungViewModel.CocOrder);
        }

        [HttpPost]
        public ActionResult NewCocOrderDuplicateVinShowDialog()
        {
            return PartialView("Partial/DuplicateVinConfirmationMode", CocErfassungViewModel.CocOrder);
        }

        [HttpPost]
        public ActionResult NewCocOrderDuplicateVinConfirmIgnoreOnSaving()
        {
            CocErfassungViewModel.CocOrder.DuplicateVinIgnoreOnSaving = true;

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult CocOrderNewShowSummary()
        {
            return PartialView("Erfassung/Manuell/CocOrderSummaryForm", CocErfassungViewModel.CocOrder);
        }

        #endregion


        #region CSV Upload

        [HttpPost]
        public ActionResult CsvUploadStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            // Step 1:  Upload the CSV file

            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = "Fehler: Keine Datei angegeben!" }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!CocErfassungViewModel.CsvUploadFileSave(file.FileName, file.SavePostedFile))
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

            CocErfassungViewModel.UploadItemsShowErrorsOnly = showErrorsOnly;

            return PartialView("Erfassung/CsvUpload/ValidationGrid", CocErfassungViewModel);
        }

        [HttpPost]
        public ActionResult CsvUploadSubmit()
        {
            // Step 3:  Save CSV data to data store

            CocErfassungViewModel.SaveUploadItems();

            return PartialView("Erfassung/CsvUpload/Receipt", CocErfassungViewModel);
        }

        #endregion
    }
}