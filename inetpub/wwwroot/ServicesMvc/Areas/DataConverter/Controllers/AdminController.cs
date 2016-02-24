using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using CkgDomainLogic.DataConverter.ActionFilters;
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
            ViewModel.DataConverterInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadDataMappings(DataMappingSelektor model)
        {
            ViewModel.MappingSelektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadDataMappings(ModelState);

            return PartialView("Partial/Suche", ViewModel.MappingSelektor);
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
            ViewModel.ClearNewMappingSelektor();

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
                uploadFileNameCsv = ViewModel.MappingModel.SourceFile.FilenameCsv
            }, "text/plain");
        }

        #endregion

        [HttpPost]
        public ActionResult ShowKonfiguration(int mappingId)
        {
            ViewModel.InitMapping(mappingId);

            return PartialView("Partial/Konfiguration", ViewModel);
        }

        [HttpPost]
        public ActionResult DeleteKonfiguration(int mappingId)
        {
            ViewModel.DeleteMapping(mappingId);

            return new EmptyResult();
        }

        #region Show Xml as div content

        public ActionResult ShowDestinationDiv()
        {
            return Content(ViewModel.GetDestinationDiv());
        }

        #endregion

        [HttpPost]
        [StoreUi]
        public JsonResult AddProcessor(string processors, string connections)
        {
            ViewModel.AddProcessor();
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
                    ViewModel.MappingModel.RecordNo = 0;
                    break;

                case "-1":
                    ViewModel.MappingModel.RecordNo--;
                    break;

                case "+1":
                    ViewModel.MappingModel.RecordNo++;
                    break;

                case "last":
                    ViewModel.MappingModel.RecordNo = ViewModel.MappingModel.RecordCount;
                    break;
            }

            if (ViewModel.MappingModel.RecordNo < 1)
                ViewModel.MappingModel.RecordNo = 1;

            if (ViewModel.MappingModel.RecordNo > ViewModel.MappingModel.RecordCount)
                ViewModel.MappingModel.RecordNo = ViewModel.MappingModel.RecordCount;

            // Alle DatenRecords der Quellfelder ermitteln...
            var sourceFieldList = ViewModel.RecalcSourceFields();

            // Alle DatenRecords der Zielfelder ermitteln...
            var destFieldList = ViewModel.RecalcDestFields();

            // Alle Prozessoren zur späteren Ausgabe aktualisieren...
            var processorList = ViewModel.RecalcProcessors();

            return Json(new { SourceFieldList = sourceFieldList, DestFieldList = destFieldList, ConnectionList = ViewModel.MappingModel.DataConnections, ProcessorList = processorList, RecordInfoText = ViewModel.MappingModel.RecordInfoText });
        }

        [HttpPost]
        [StoreUi]
        public ActionResult ApplyFileContainsHeadings(bool fileContainsHeadings)
        {
            ViewModel.FileContainsHeadings = fileContainsHeadings;
            return RefreshUi("first");
        }

        [HttpPost]
        [StoreUi]
        public ActionResult SaveKonfiguration()
        {
            var success = ViewModel.SaveMapping();

            return Json(new { success = success, message = (success ? Localize.SaveSuccessful : Localize.SaveFailed) });
        }

        #region Export Xml

        [StoreUi]
        public ActionResult TestExportXml()
        {
            return Content(ViewModel.GenerateXmlResultStructure().ToString(), "text/xml");
        }

        #endregion
    }
}