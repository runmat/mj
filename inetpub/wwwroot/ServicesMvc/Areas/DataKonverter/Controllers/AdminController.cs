using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.DataKonverter.Contracts;
using CkgDomainLogic.DataKonverter.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using ServicesMvc.Areas.DataKonverter.Models;
using MvcTools.Web;
using ServicesMvc.Areas.DataKonverter;

namespace ServicesMvc.DataKonverter.Controllers
{
    [DataKonverterInjectGlobalData]
    public class AdminController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<KroschkeDataKonverterViewModel>(); } }

        public KroschkeDataKonverterViewModel ViewModel
        {
            get { return GetViewModel<KroschkeDataKonverterViewModel>(); }
            set { SetViewModel(value); }
        }

        public AdminController(IAppSettings appSettings, ILogonContextDataService logonContext, IDataKonverterDataService dataKonverterDataService)
            : base(appSettings, logonContext)
        {
            if (IsInitialRequestOf("Index"))
                ViewModel = null;

            InitViewModelExpicit(ViewModel, appSettings, logonContext, dataKonverterDataService);
        }

        private void InitViewModelExpicit(KroschkeDataKonverterViewModel vm, IAppSettings appSettings, ILogonContextDataService logonContext, IDataKonverterDataService dataKonverterDataService)
        {
            InitViewModel(vm, appSettings, logonContext, dataKonverterDataService);
            InitModelStatics();
        }

        void InitModelStatics()
        {
        }

        [CkgApplication]
        public ActionResult Index()
        {

            ViewModel.DataInit();

            //var csvFilename = ViewModel.ConvertExcelToCsv("Testfile.xlsx", Guid.NewGuid() + "-Testfile.csv");
            //var destFilename = "";
            //ViewModel.SourceFile = ViewModel.DataKonverterDataService.FillSourceFile(csvFilename, true);
            //ViewModel.DestinationFile = ViewModel.FillDestinationObj("KroschkeOn.xsd");

            return View(ViewModel);
        }

        [CkgApplication]
        [HttpPost]
        public ActionResult Prozessauswahl()
        {
            return PartialView("Partial/Prozessauswahl", ViewModel.SourceFile);
        }

        [CkgApplication]
        [HttpPost]
        public ActionResult Konfiguration()
        {
            return PartialView("Partial/Konfiguration", ViewModel);
        }

        [CkgApplication]
        [HttpPost]
        public ActionResult Testimport()
        {
            return PartialView("Partial/Testimport", ViewModel);
        }

        [CkgApplication]
        [HttpPost]
        public ActionResult Abschluss()
        {
            return PartialView("Partial/Abschluss", ViewModel);
        }


        #region Ajax

        [HttpPost]
        public JsonResult LiveTransform(string input, string func)
        {
            var output = "";
            output = string.Format("#{0}", input);

            return Json(new { Output = output });
        }

        #endregion

        #region Upload

        [HttpPost]
        public ActionResult UploadStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            //if (!ViewModel.PdfUploadFileSave(file.FileName, file.SavePostedFile))
            //    return Json(new { success = false, message = Localize.ErrorFileCouldNotBeSaved }, "text/plain");

            if (!ViewModel.PdfUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = Localize.ErrorFileCouldNotBeSaved }, "text/plain");

            ViewModel.SourceFile.Filename = file.FileName;

            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        //public ActionResult Upload(Upload model)
        public ActionResult Upload(SourceFile model)
        {
            var sdf = ViewModel.SourceFile;

            if (Request["firstRequest"] == "ok")                // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                // model = ViewModel.Upload;
                model = ViewModel.SourceFile;

            if (ModelState.IsValid)
            {
                //if (model.DeleteUploadedPdf)
                //{
                //    ViewModel.Overview.PdfUploaded = null;
                //    ViewModel.Overview.PdfCreateDt = null;
                //}

                //ViewModel.Upload = model;
                ViewModel.SourceFile = model;
            }

            // return PartialView("Partial/Upload", model);
            return PartialView("Partial/Prozessauswahl", model);
        }

        #endregion
    }
}