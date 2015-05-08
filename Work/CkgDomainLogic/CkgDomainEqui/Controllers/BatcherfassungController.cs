using System.Collections;
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

namespace ServicesMvc.Controllers
{
    public class BatcherfassungController : CkgDomainController
    {

        public override string DataContextKey
        {
            get { return "BatcherfassungDataContext"; }
        }

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
            Batcherfassung.GetViewModel = GetViewModel<BatcherfassungViewModel>; 
            BatcherfassungSelektor.GetViewModel = GetViewModel<BatcherfassungViewModel>;
        }


        [CkgApplication]
        public ActionResult Verwaltung()
        {
            ViewModel.DataInit();
            ViewModel.Init();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadBatcherfassung(BatcherfassungSelektor model)
        {
            ViewModel.BatcherfassungSelektor = model;

            //ViewModel.Validate(AddModelError);

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
        public ActionResult LoadDataByModelId(string modelId)
        {
            return PartialView("Partial/DetailsForm", ViewModel.ModifyItemWithModelData(modelId));
        }
              
        [HttpPost]
        public ActionResult EditBatcherfassung(string id)
        {
            ViewModel.InsertMode = false;
            ModelState.Clear();
            return PartialView("Partial/DetailsForm", ViewModel.GetItem(id).SetInsertMode(ViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult NewBatcherfassung(string idToDuplicate)
        {
            ViewModel.InsertMode = true;
            ModelState.Clear();
            return PartialView("Partial/DetailsForm", ViewModel.NewItem(idToDuplicate).SetInsertMode(ViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult BatcherfassungDetailsFormSave(Batcherfassung model)
        {
            var viewModel = ViewModel;

            viewModel.ValidateModel(model, viewModel.InsertMode, ModelState.AddModelError);

            if (ModelState.IsValid)
            {
                if (viewModel.InsertMode)
                    viewModel.AddItem(model);

                viewModel.SaveItem(model, ModelState.AddModelError);
            }

            model.InsertModeTmp = viewModel.InsertMode;

            return PartialView("Partial/DetailsForm", model);
        }

        [HttpPost]
        public ActionResult FilterGridBatcherfassung(string filterValue, string filterColumns)
        {
            ViewModel.FilterBatcherfassungs(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.BatcherfassungsFiltered;
        }

        #endregion

        #region Excel Upload
      
        [HttpPost]
        public ActionResult ExcelUploadStart(IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            // Step 1:  Upload the CSV file

            if (uploadFiles == null || uploadFiles.None())
                return Json(new { success = false, message = "Fehler: Keine Datei angegeben!" }, "text/plain");

            // because we are uploading in async mode, our "e.files" collection always has exact 1 entry:
            var file = uploadFiles.ToArray()[0];

            if (!ViewModel.CsvUploadFileSave(file.FileName, file.SavePostedFile))
                return Json(new { success = false, message = "Fehler: CSV Datei konnte nicht gespeichert werden!" }, "text/plain");

            return Json(new
            {
                success = true,
                message = "ok",
                uploadFileName = file.FileName,
            }, "text/plain");
        }

        [HttpPost]
        public ActionResult ExcelUploadShowData(bool showErrorsOnly)
        {
            // Step 2:  Prepare data for user validation

            ViewModel.UploadItemsShowErrorsOnly = showErrorsOnly;

            ViewModel.PrepareUploadItems();

            return PartialView("Partial/DetailsForm", ViewModel.SelectedItem);
        }

       

        #endregion



        #region Unitnummern

        [HttpPost]
        public ActionResult ShowGridUnitNumbers(string batchId)
        {
            return PartialView("Partial/GridUnitNumbers", ViewModel.LoadUnitnummerByBatchId(batchId));
        }

        #endregion


    }
}
