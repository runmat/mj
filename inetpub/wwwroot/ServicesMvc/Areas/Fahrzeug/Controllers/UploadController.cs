using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Fahrzeug.Controllers
{
    public class UploadController : CkgDomainController
    {
        public override string DataContextKey { get { return GetDataContextKey<UploadZb2VersandViewModel>(); } }

        public UploadZb2VersandViewModel UploadZb2VersandViewModel { get { return GetViewModel<UploadZb2VersandViewModel>(); } }

        public UploadController(IAppSettings appSettings, ILogonContextDataService logonContext, IBriefVersandDataService briefVersandDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(UploadZb2VersandViewModel, appSettings, logonContext, briefVersandDataService);
        }

        [CkgApplication]
        public ActionResult Zb2Versand()
        {
            UploadZb2VersandViewModel.Init();

            return View(UploadZb2VersandViewModel);
        }


        #region Zb2Versand

        [HttpPost]
        public ActionResult UploadZb2VersandStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!UploadZb2VersandViewModel.ExcelUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = Localize.ErrorFileCouldNotBeSaved }, "text/plain");


            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult UploadZb2VersandShowGrid()
        {
            UploadZb2VersandViewModel.ValidateUploadItems();

            return PartialView("Zb2Versand/UploadGrid", UploadZb2VersandViewModel);
        }

        [GridAction]
        public ActionResult UploadZb2VersandAjaxBinding()
        {
            return View(new GridModel(UploadZb2VersandViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadZb2VersandAjaxUpdateItem(int lfdNr)
        {
            var item = UploadZb2VersandViewModel.GetDatensatzById(lfdNr);
            if (TryUpdateModel(item))
            {
                UploadZb2VersandViewModel.ApplyChangedData(item);
            }

            return View(new GridModel(UploadZb2VersandViewModel.UploadItems));
        }

        [GridAction]
        public ActionResult UploadZb2VersandAjaxDeleteItem(int lfdNr)
        {
            UploadZb2VersandViewModel.RemoveDatensatzById(lfdNr);

            return View(new GridModel(UploadZb2VersandViewModel.UploadItems));
        }

        [HttpPost]
        public ActionResult UploadZb2VersandSubmit()
        {
            UploadZb2VersandViewModel.SaveUploadItems();

            return Json(new { });
        }

        [HttpPost]
        public ActionResult UploadZb2VersandShowReceipt()
        {
            return PartialView("Zb2Versand/Receipt", UploadZb2VersandViewModel);
        }

        public FileResult DownloadZb2VersandCsvTemplate()
        {
            var pfad = Server.MapPath(Url.Content("/ServicesMvc/Documents/Templates/UploadZb2Versand.csv"));
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, "UploadDokuVersand.csv");
        }

        public FileResult DownloadZb2VersandXlsTemplate()
        {
            var pfad = Server.MapPath(Url.Content("/ServicesMvc/Documents/Templates/UploadZb2Versand.xls"));
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, "UploadDokuVersand.xls");
        }

        #endregion
    }
}
