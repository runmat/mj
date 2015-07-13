using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using System.Linq;
using CkgDomainLogic.FzgModelle.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.FzgModelle.ViewModels;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Contracts;
using Telerik.Web.Mvc;
using MvcTools.Web;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public class BatcherfassungController : CkgDomainController
    {
        public override string DataContextKey { get { return "BatcherfassungDataContext"; } }

        public BatcherfassungViewModel ViewModel { get { return GetViewModel<BatcherfassungViewModel>(); } }

        public BatcherfassungController(IAppSettings appSettings, ILogonContextDataService logonContext, IBatcherfassungDataService modellIdDataService
                                        , IFahrzeugeDataService fahrzeugeDataService
                                        )
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, modellIdDataService);            
            InitViewModel(ViewModel, appSettings, logonContext, fahrzeugeDataService);
            InitModelStatics();
        }

        void InitModelStatics()
        {
            BatcherfassungEdit.GetViewModel = GetViewModel<BatcherfassungViewModel>; 
            BatcherfassungSelektor.GetViewModel = GetViewModel<BatcherfassungViewModel>;
        }

        [CkgApplication]
        public ActionResult Verwaltung()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadBatcherfassung(BatcherfassungSelektor model)
        {
            ViewModel.BatcherfassungSelektor = model;

            if (ModelState.IsValid)
            {
                ViewModel.LoadBatches();
                if (ViewModel.Batcherfassungs.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Partial/Suche", ViewModel.BatcherfassungSelektor);
        }

        [HttpPost]
        public ActionResult ShowBatcherfassung()
        {
            return PartialView("Partial/GridBatcherfassung", ViewModel);
        }

        [GridAction]
        public ActionResult BatcherfassungAjaxBinding()
        {
            return View(new GridModel(ViewModel.BatcherfassungsFiltered));
        }

        [HttpPost]
        public JsonResult LoadDataByModelId(string modelId)
        {
            return Json(new { ModelData = ViewModel.GetModelData(modelId) });
        }
              
        [HttpPost]
        public ActionResult EditBatcherfassung(string id)
        {
            var item = ViewModel.GetEditItem(id).SetInsertMode(ViewModel.InsertMode);

            if(!item.Batch.StatusNeu)
                return PartialView("Partial/DetailsFormReadOnly", item);

            ViewModel.InsertMode = false;
            ModelState.Clear();
            return PartialView("Partial/DetailsForm", item);
        }

        [HttpPost]
        public ActionResult NewBatcherfassung(string idToDuplicate)
        {
            ViewModel.InsertMode = true;
            ModelState.Clear();
            return PartialView("Partial/DetailsForm", ViewModel.NewEditItem(idToDuplicate).SetInsertMode(ViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult BatcherfassungDetailsFormSave(BatcherfassungEdit model)
        {
            if (ModelState.IsValid)
            {
                if (ViewModel.InsertMode)
                    ViewModel.AddItem(model.Batch);

                ViewModel.SaveEditItem(model, ModelState.AddModelError);
            }

            model.InsertModeTmp = ViewModel.InsertMode;

            return PartialView("Partial/DetailsForm", model);
        }

        [HttpPost]
        public ActionResult FilterGridBatcherfassung(string filterValue, string filterColumns)
        {
            ViewModel.FilterBatcherfassungs(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Export
       
        public ActionResult ExportBatcherfassungFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.BatcherfassungsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Batcherfassung", dt);

            return new EmptyResult();
        }

        public ActionResult ExportBatcherfassungFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.BatcherfassungsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Batcherfassung", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region Excel Upload
      
        public FileResult DownloadBatcherfassungXlsTemplate()
        {
            var pfad = Server.MapPath(Url.Content("/ServicesMvc/Documents/Templates/UploadUnitnummern.xls"));
            return File(pfad, System.Net.Mime.MediaTypeNames.Application.Octet, "UploadUnitnummern.xls");
        }

        [HttpPost]
        public ActionResult ExcelUploadStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = Localize.ErrorNoFileSelected }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!ViewModel.CsvUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = Localize.ErrorFileCouldNotBeSaved }, "text/plain");

            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public JsonResult ExcelUploadShowData()
        {
            ViewModel.PrepareUploadItems();

            return Json(new { ViewModel.SelectedItem.Batch.Unitnummern });
        }

        #endregion

        #region Unitnummern
           
        [HttpPost]
        public JsonResult CalculateUnitNumbers(string unitnumberFrom, string unitnumberUntil, string count)
        {
            ViewModel.CalculateUnitNumbers(unitnumberFrom, unitnumberUntil, count);

            return Json(new { ViewModel.SelectedItem.Batch.Unitnummern,  ViewModel.SelectedItem.ValidationError });
        }

        [HttpPost]
        public JsonResult UnitnumberSelectionChanged(string unitnummer, bool isChecked)
        {
            int allSelectionCount = 0, allCount = 0, allFoundCount = 0;
            
            if (unitnummer.IsNullOrEmpty())
                ViewModel.SelectUnitnummern(isChecked, out allSelectionCount, out allCount, out allFoundCount);
            else
                ViewModel.SelectUnitnummer(unitnummer, isChecked, out allSelectionCount);
            
            return Json(new { allSelectionCount, allCount, allFoundCount });
        }

        [HttpPost]
        public ActionResult ShowGridUnitNumbers(string batchId)
        {
            ViewModel.LoadUnitnummerByBatchId(batchId);
            return PartialView("Partial/GridUnitNumbers", ViewModel.SelectedItem);
        }

        [HttpPost]
        public ActionResult UnitnummerFreigebenSperren()
        {            
            ViewModel.FreigebenSperren(ModelState.AddModelError);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult UpdateSperrvermerk(string unitnummer, string sperrvermerk)
        {
            ViewModel.UpdateSperrvermerk(unitnummer, sperrvermerk);
            
            return new EmptyResult();
        }

        [GridAction]
        public ActionResult UnitnummerAjaxBinding()
        {
            return View(new GridModel(ViewModel.UnitnummernFiltered));
        }
     
        [HttpPost]
        public ActionResult FilterGridUnitnumbers(string filterValue, string filterColumns)
        {
            ViewModel.FilterUnitnummern(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportUnitnummerFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.UnitnummernFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Batcherfassung", dt);

            return new EmptyResult();
        }

        public ActionResult ExportUnitnummerFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.UnitnummernFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Batcherfassung", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
         
        #endregion
    }
}
