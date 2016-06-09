using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.Remarketing.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Web;

namespace ServicesMvc.Controllers
{
    public partial class RemarketingController
    {
        public FehlendeDatenViewModel FehlendeDatenViewModel { get { return GetViewModel<FehlendeDatenViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportFehlendeDaten()
        {
            FehlendeDatenViewModel.DataInit();

            return View(FehlendeDatenViewModel);
        }

        [HttpPost]
        public ActionResult LoadFehlendeDaten(FehlendeDatenSelektor model)
        {
            if (ModelState.IsValid)
                FehlendeDatenViewModel.LoadFehlendeDaten(model, ModelState.AddModelError);

            return PartialView("FehlendeDaten/Suche", model);
        }

        [HttpPost]
        public ActionResult ShowFehlendeDaten()
        {
            return PartialView("FehlendeDaten/Grid");
        }

        [GridAction]
        public ActionResult FehlendeDatenAjaxBinding()
        {
            return View(new GridModel(FehlendeDatenViewModel.FehlendeDatenFiltered));
        }

        #region Excel Upload

        [HttpPost]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ActionResult ExcelUploadStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!FehlendeDatenViewModel.ExcelUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = Localize.ErrorFileCouldNotBeSaved }, "text/plain");

            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        public FileResult DownloadFehlendeDatenExcelTemplate()
        {
            var templateFileName = (FehlendeDatenViewModel.Selektor.Auswahl == "F" ? "UploadFahrgestellnummern.xls" : "UploadKennzeichen.xls");

            var pfad = Server.MapPath(Url.Content("/ServicesMvc/Documents/Templates/" + templateFileName));
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, templateFileName);
        }

        #endregion

        [HttpPost]
        public ActionResult FilterGridFehlendeDaten(string filterValue, string filterColumns)
        {
            FehlendeDatenViewModel.FilterFehlendeDaten(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportFehlendeDatenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FehlendeDatenViewModel.FehlendeDatenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Remarketing_FehlendeDaten, dt);

            return new EmptyResult();
        }

        public ActionResult ExportFehlendeDatenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = FehlendeDatenViewModel.FehlendeDatenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Remarketing_FehlendeDaten, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
