using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
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

        [HttpPost]
        [CkgApplication]
        public ActionResult Prozessauswahl()
        {
            //if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
            //    model = ViewModel.Auftraggeber;

            return PartialView("Partial/Prozessauswahl", ViewModel.DataMapper.SourceFile);
        }

        [HttpPost]
        [CkgApplication]
        public ActionResult Konfiguration()
        {
            //if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
            //    model = ViewModel.Auftraggeber;

            return PartialView("Partial/Konfiguration", ViewModel);
        }

        [HttpPost]
        [CkgApplication]
        public ActionResult Testimport()
        {
            //if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
            //    model = ViewModel.Auftraggeber;

            return PartialView("Partial/Testimport", ViewModel);
        }

        [HttpPost]
        [CkgApplication]
        public ActionResult Abschluss()
        {
            //if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
            //    model = ViewModel.Auftraggeber;

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

        [HttpPost]
        public JsonResult NewProcessor()
        {
            var processorGuid = ViewModel.DataMapper.AddProcessor();
            return Json(new { NewGuid = processorGuid });
        }

        [HttpPost]
        // public JsonResult NewConnection(DataConnection dataConnection)
        // IdSource: idSource, IdDest: idDest, SourceIsProcessor: sourceIsProcessor, DestIsProcessor:
        public JsonResult NewConnection(string idSource, string idDest, bool sourceIsProcessor, bool destIsProcessor)
        {
            // var result = ViewModel.DataMapper.AddConnection(dataConnection);            
            var result = ViewModel.DataMapper.AddConnection(idSource, idDest, sourceIsProcessor, destIsProcessor);

            var processor = ViewModel.DataMapper.Processors.FirstOrDefault();

            var result2 = ViewModel.DataMapper.GetProcessorResult(processor,1);

            return Json(result);
        }

        [HttpPost]
        public JsonResult RemoveConnection(DataConnection dataConnection)
        {
            var result = ViewModel.DataMapper.RemoveConnection(dataConnection);

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetConnections()
        {
            var result = ViewModel.DataMapper.GetConnections();

            return Json(result);
        }

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

            ViewModel.DataMapper.SourceFile.Filename = file.FileName;

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
            var sdf = ViewModel.DataMapper.SourceFile;

            if (Request["firstRequest"] == "ok")                // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                // model = ViewModel.Upload;
                model = ViewModel.DataMapper.SourceFile;

            if (ModelState.IsValid)
            {
                //if (model.DeleteUploadedPdf)
                //{
                //    ViewModel.Overview.PdfUploaded = null;
                //    ViewModel.Overview.PdfCreateDt = null;
                //}

                //ViewModel.Upload = model;
                ViewModel.DataMapper.SourceFile = model;
            }

            // return PartialView("Partial/Upload", model);
            return PartialView("Partial/Prozessauswahl", model);
        }

        #endregion
    }
}