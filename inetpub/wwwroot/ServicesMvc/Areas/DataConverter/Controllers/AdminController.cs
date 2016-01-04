using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using CkgDomainLogic.DataConverter.Contracts;
using CkgDomainLogic.DataConverter.ViewModels;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using MvcTools.Web;
using ServicesMvc.Areas.DataConverter;
using ServicesMvc.Areas.DataConverter.ActionFilters;

namespace ServicesMvc.DataConverter.Controllers
{
    [DataConverterInjectGlobalData]
    public class AdminController : CkgDomainController
    {
        public override string DataContextKey { get { return GetDataContextKey<DataConverterViewModel>(); } }

        public DataConverterViewModel ViewModel
        {
            get { return GetViewModel<DataConverterViewModel>(); }
            set { SetViewModel(value); }
        }

        public AdminController(IAppSettings appSettings, ILogonContextDataService logonContext, IDataConverterDataService dataConverterDataService)
            : base(appSettings, logonContext)
        {
            if (IsInitialRequestOf("Index"))
                ViewModel = null;

            InitViewModelExpicit(ViewModel, appSettings, logonContext, dataConverterDataService);
        }

        private void InitViewModelExpicit(DataConverterViewModel vm, IAppSettings appSettings, ILogonContextDataService logonContext, IDataConverterDataService dataConverterDataService)
        {
            InitViewModel(vm, appSettings, logonContext, dataConverterDataService);
            InitModelStatics();
        }

        void InitModelStatics()
        {
            //CkgDomainLogic.Autohaus.Models.Zulassungsdaten.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
            //CkgDomainLogic.Autohaus.Models.Fahrzeugdaten.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        [CkgApplication]
        public ActionResult Prozessauswahl(DataConverterViewModel.WizardProzessauswahl model)
        {
            if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model = ViewModel.Prozessauswahl;

            if (ModelState.IsValid)
                ViewModel.Prozessauswahl = model;

            return PartialView("Partial/Prozessauswahl", model);
        }

        [HttpPost]
        [CkgApplication]
        public ActionResult Konfiguration(DataConverterViewModel.WizardKonfiguration model)
        {
            if (Request["firstRequest"] == "ok")          // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model = ViewModel.Konfiguration;

            if (ModelState.IsValid)
                ViewModel.Konfiguration = model;

            ViewModel.DataConverter.DestinationFile.Filename = @"C:\tmp\KroschkeOn2.xml";  // ###removeme### Prozessdatei bis jetzt noch fest verdrahtet
            ViewModel.DataConverter.ReadSourceFile();
            ViewModel.DataConverter.ReadDestinationObj();

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
            ViewModel.DataConverter.AddProcessor();
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
                    ViewModel.DataConverter.RecordNo = 0;
                    break;

                case "-1":
                    ViewModel.DataConverter.RecordNo--;
                    break;

                case "+1":
                    ViewModel.DataConverter.RecordNo++;
                    break;

                case "last":
                    ViewModel.DataConverter.RecordNo = ViewModel.DataConverter.RecordCount;
                    break;
            }

            if (ViewModel.DataConverter.RecordNo < 1)
                ViewModel.DataConverter.RecordNo = 1;

            if (ViewModel.DataConverter.RecordNo > ViewModel.DataConverter.RecordCount)
                ViewModel.DataConverter.RecordNo = ViewModel.DataConverter.RecordCount;
            
            // Alle Prozessoren zur späteren Ausgabe aktualisieren...
            var processorList = ViewModel.DataConverter.RecalcProcessors();

            // Alle DatenRecords der Quellfelder ermitteln...
            var sourceFieldList = new List<Domaenenfestwert>();
            foreach (var field in ViewModel.DataConverter.SourceFile.Fields)
            {
                sourceFieldList.Add(new Domaenenfestwert
                {
                    Wert = field.Guid,
                    Beschreibung = field.Records[ViewModel.DataConverter.RecordNo - 1]
                });
            }

            // Alle DatenRecords der Zielfelder ermitteln...
            var destFieldList = ViewModel.DataConverter.RecalcDestFields();

            return Json(new { SourceFieldList = sourceFieldList, DestFieldList = destFieldList, ProcessorList = processorList, RecordInfoText = ViewModel.DataConverter.RecordInfoText });
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
                uploadFileNameCsv = ViewModel.DataConverter.SourceFile.FilenameCsv
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult Upload(DataConverterViewModel.WizardProzessauswahl model)
        {
            if (Request["firstRequest"] == "ok")                // Wenn Action durch AjaxRequestNextStep aufgerufen wurde, model aus ViewModel übernehmen
                model.SourceFile = ViewModel.DataConverter.SourceFile;

            if (ModelState.IsValid)
                ViewModel.DataConverter.SourceFile = model.SourceFile;

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
            var xmlContent = ViewModel.DataConverter.ExportToXml(@"C:\tmp\TestOutputComplete.xml");

            return Content(xmlContent);
        }

        #endregion
    }
}