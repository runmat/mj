using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using CkgDomainLogic.DataConverter.Contracts;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.DataConverter.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Web;
using ServicesMvc.Areas.DataConverter.ActionFilters;
using Telerik.Web.Mvc;

namespace ServicesMvc.DataConverter.Controllers
{
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
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
            InitViewModel(ViewModel, appSettings, logonContext, dataConverterDataService);
            InitModelStatics();
        }

        void InitModelStatics()
        {
            DataMappingSelektor.GetViewModel = GetViewModel<DataConverterViewModel>;
            NewDataMappingSelektor.GetViewModel = GetViewModel<DataConverterViewModel>;
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadDataMappings(DataMappingSelektor model)
        {
            ViewModel.Selektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadDataMappings(ModelState);

            return PartialView("Partial/Suche", ViewModel.Selektor);
        }

        [HttpPost]
        public ActionResult ShowDataMappings()
        {
            return PartialView("Partial/Grid", ViewModel);
        }

        [GridAction]
        public ActionResult DataMappingsAjaxBinding()
        {
            return View(new GridModel(ViewModel.DataMappingsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridDataMappings(string filterValue, string filterColumns)
        {
            ViewModel.FilterDataMappings(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Grid-Export

        public ActionResult ExportDataMappingsFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DataMappingsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.DataConverter, dt);

            return new EmptyResult();
        }

        public ActionResult ExportDataMappingsFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DataMappingsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.DataConverter, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        [HttpPost]
        public ActionResult ShowProzessauswahl()
        {
            return PartialView("Partial/Prozessauswahl", ViewModel.NewMappingSelektor);
        }

        [HttpPost]
        public ActionResult Prozessauswahl(NewDataMappingSelektor model)
        {
            if (ModelState.IsValid)
                ViewModel.NewMappingSelektor = model;

            return PartialView("Partial/Prozessauswahl", model);
        }

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

        #endregion

        [HttpPost]
        public ActionResult ShowKonfiguration(int mappingId)
        {
            ViewModel.InitMapping(mappingId);

            return PartialView("Partial/Konfiguration", ViewModel.DataConverter);
        }

        #region Show Xml as div content

        public ActionResult ShowDestinationDiv(XmlDocument destXmlDocument)
        {
            var divContent = ViewModel.GetDestinationDiv(destXmlDocument);

            return Content(divContent);
        }

        #endregion

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
        [StoreUi]
        public JsonResult LoadUiData()
        {
            return RefreshUi();
        }

        [HttpPost]
        [StoreUi]
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
            var sourceFieldList = ViewModel.DataConverter.RecalcSourceFields();

            // Alle DatenRecords der Zielfelder ermitteln...
            var destFieldList = ViewModel.DataConverter.RecalcDestFields();

            return Json(new { SourceFieldList = sourceFieldList, DestFieldList = destFieldList, ProcessorList = processorList, RecordInfoText = ViewModel.DataConverter.RecordInfoText });
        }

        [HttpPost]
        [StoreUi]
        public ActionResult SaveKonfiguration()
        {
            if (ModelState.IsValid)
                ViewModel.SaveMapping(ModelState);

            return PartialView("Partial/Konfiguration", ViewModel.DataConverter);
        }

        #region Export Xml

        [StoreUi]
        public ActionResult TestExportXml()
        {
            var brrr = JSon.Serialize(ViewModel.DataConverter.DataMapping);

            return Content(ViewModel.GenerateXmlResultStructure(), "text/xml");
        }

        #endregion
    }
}