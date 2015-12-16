using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Xml;
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
using ServicesMvc.Areas.DataKonverter.ActionFilters;

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

            // var csvFilename = ViewModel.ConvertExcelToCsv("Import1 Excel2007.xlsx", Guid.NewGuid() + "-Testfile.csv");
            // var csvFilename = ViewModel.ConvertExcelToCsv(  "Import1 Excel2007.xlsx", Guid.NewGuid() + "-Testfile.csv");
            var destFilename = "";

            // ViewModel.DataMapper.SourceFile.Filename = csvFilename; // ViewModel.DataKonverterDataService.FillSourceFile(csvFilename, true);
            // ViewModel.DataMapper.DestinationFile.Filename = @"C:\tmp\KroschkeOn2.xml";  //  ViewModel.FillDestinationObj("KroschkeOn.xsd");
            var csvFilename = "";

            // ViewModel.DataMapper.Init(ViewModel.GetUploadPathTemp(), csvFilename, true, ';', @"C:\tmp\KroschkeOn2.xml", null, null);

            return View(ViewModel);
        }

        [HttpPost]
        [CkgApplication]
        public ActionResult Prozessauswahl(KroschkeDataKonverterViewModel.WizardProzessauswahl model)
        {
            var firstRequest = Request["firstRequest"];

            if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model = ViewModel.Prozessauswahl;

            if (ModelState.IsValid)
            {
                ViewModel.Prozessauswahl = model;
            }

            var test0 = ViewModel;

            var test = model.SourceFile.FilenameOrig;

            return PartialView("Partial/Prozessauswahl", model);
        }

        [HttpPost]
        [CkgApplication]
        public ActionResult Konfiguration(KroschkeDataKonverterViewModel.WizardKonfiguration model)
        {
            var firstRequest = Request["firstRequest"];

            if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model = ViewModel.Konfiguration;

            if (ModelState.IsValid)
            {
                ViewModel.Konfiguration = model;
            }

            ViewModel.DataMapper.DestinationFile.Filename = @"C:\tmp\KroschkeOn2.xml";  // ###removeme### Prozessdatei bis jetzt noch fest verdrahtet
            ViewModel.DataMapper.ReadSourceFile();
            ViewModel.DataMapper.ReadDestinationObj();

            return PartialView("Partial/Konfiguration", ViewModel);
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
        [StoreUi]
        public JsonResult AddProcessor(string processors, string connections)
        {
            ViewModel.DataMapper.AddProcessor();
            return RefreshUi();
        }

        [HttpPost]
        [StoreUi]
        public JsonResult SyncUiData()
        {
            return RefreshUi();
        }

        [HttpPost]
        public JsonResult LoadUiData()
        {
            return RefreshUi();
        }

        [HttpPost]
        public JsonResult SetProcessorSettings(string processorId, Operation processorType, string processorPara1, string processorPara2)
        {
            ViewModel.SetProcessorSettings(processorId, processorType, processorPara1, processorPara2);
            return RefreshUi();
        }

        [HttpPost]
        [StoreUi]
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

            if (!ViewModel.UploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = Localize.ErrorFileCouldNotBeSaved }, "text/plain");

            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
                uploadFileNameCsv = ViewModel.DataMapper.SourceFile.FilenameCsv
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult Upload(SourceFile model)
        {
            var sdf = ViewModel.DataMapper.SourceFile;

            if (Request["firstRequest"] == "ok")                // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model = ViewModel.DataMapper.SourceFile;

            if (ModelState.IsValid)
            {
                ViewModel.DataMapper.SourceFile = model;
            }

            return PartialView("Partial/Prozessauswahl", model);
        }

        #endregion

        #region Show Xml as div content

        public ActionResult ShowDestinationDiv(XmlDocument destXmlDocument)
        {
            var divContent = ViewModel.GetDestinationDiv(destXmlDocument);

            return Content(divContent);
        }

        #endregion

        #region Export Xml

        [HttpPost]
        public ActionResult ExportXml()
        {
            var xmlContent = ViewModel.DataMapper.ExportToXml(@"C:\tmp\TestOutputComplete.xml");

            return Content(xmlContent);
        }

        #endregion
    }
}