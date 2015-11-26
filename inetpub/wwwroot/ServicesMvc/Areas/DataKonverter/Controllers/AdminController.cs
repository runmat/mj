using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.DataKonverter.Contracts;
using CkgDomainLogic.DataKonverter.ViewModels;
using CkgDomainLogic.DomainCommon.Models;
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
        public ActionResult Prozessauswahl(KroschkeDataKonverterViewModel.WizardProzessauswahl model)
        {
            //if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
            //    model = ViewModel.Auftraggeber;

            return PartialView("Partial/Prozessauswahl", model);
        }

        [HttpPost]
        [CkgApplication]
        public ActionResult Konfiguration()
        {
            //if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
            //    model = ViewModel.Auftraggeber;

            // ##removeme##
            if (ViewModel.DataMapper.SourceFile.Fields == null)
            {
                // var csvFilename = ViewModel.ConvertExcelToCsv("Testfile.xlsx", Guid.NewGuid() + "-Testfile.csv");
                var csvFilename = @"C:\\dev\\inetpub\\wwwroot\\ServicesMvc\\App_Data\\FileUpload\\Temp\\Testfile3.csv";
                ViewModel.DataMapper.SourceFile.Filename = csvFilename;
                ViewModel.DataMapper.SourceFile = ViewModel.DataKonverterDataService.FillSourceFile(csvFilename, true);
                // ViewModel.DataMapper.DestinationFile = ViewModel.FillDestinationObj("KroschkeOn2.xml");
            }

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
        public JsonResult NewProcessor()
        {
            var processorGuid = ViewModel.DataMapper.AddProcessor();
            return Json(new { NewGuid = processorGuid });
        }

        [HttpPost]
        public JsonResult NewConnection(string idSource, string idDest, bool sourceIsProcessor, bool destIsProcessor)
        {
            var newConnection = ViewModel.DataMapper.AddConnection(idSource, idDest, sourceIsProcessor, destIsProcessor);

            // Alle Prozessoren zur späteren Ausgabe aktualisieren...
            var processorList = ViewModel.DataMapper.RecalcProcessors();

            // Alle DatenRecords der Quellfelder ermitteln...
            var sourceFieldList = ViewModel.DataMapper.RecalcSourceFields();

            // Alle DatenRecords der Zielfelder ermitteln...
            var destFieldList = ViewModel.DataMapper.RecalcDestFields();

            return Json(new { SourceFieldList = sourceFieldList, DestFieldList = destFieldList, ProcessorList = processorList, RecordInfoText = ViewModel.DataMapper.RecordInfoText });

            // return Json(processorList);
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

        [HttpPost]
        public JsonResult RefreshUi(string recordOffset = null)
        {
            switch (recordOffset)
            {
                case "first":
                    ViewModel.DataMapper.RecordNo = 0;
                    break;

                case "-1":
                    ViewModel.DataMapper.RecordNo --;
                    break;

                case "+1":
                    ViewModel.DataMapper.RecordNo++;
                    break;

                case "last":
                    ViewModel.DataMapper.RecordNo = ViewModel.DataMapper.RecordCount;
                    break;
            }

            if (ViewModel.DataMapper.RecordNo < 1)
                ViewModel.DataMapper.RecordNo = 1;

            if (ViewModel.DataMapper.RecordNo > ViewModel.DataMapper.RecordCount )
                ViewModel.DataMapper.RecordNo = ViewModel.DataMapper.RecordCount;
            
            // Alle Prozessoren zur späteren Ausgabe aktualisieren...
            var processorList = ViewModel.DataMapper.RecalcProcessors();

            // Alle DatenRecords der Quellfelder ermitteln...
            var sourceFieldList = new List<Domaenenfestwert>();
            foreach (var field in ViewModel.DataMapper.SourceFile.Fields)
            {
                sourceFieldList.Add(new Domaenenfestwert
                {
                    Wert = field.Guid,
                    Beschreibung = field.Records[ViewModel.DataMapper.RecordNo - 1]
                });
            }

            // Alle DatenRecords der Zielfelder ermitteln...
            var destFieldList = ViewModel.DataMapper.RecalcDestFields();

            return Json(new { SourceFieldList = sourceFieldList, DestFieldList = destFieldList, ProcessorList = processorList, RecordInfoText = ViewModel.DataMapper.RecordInfoText });
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

            if (!ViewModel.UploadFileSave(file.FileName, file.SavePostedFile))
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

        #region Export Xml

        [HttpPost]
        public ActionResult ExportXml()
        {
            //if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
            //    model = ViewModel.Auftraggeber;
            // return PartialView("Partial/Testimport", ViewModel);

            var xmlContent = ViewModel.DataMapper.ExportToXml();

            return Content(xmlContent);
        }
        

        #endregion

    }
}