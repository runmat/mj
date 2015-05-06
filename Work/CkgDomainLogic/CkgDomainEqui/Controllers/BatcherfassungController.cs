using System.Collections;
using System.Web.Mvc;
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
            Batcherfassung.GetViewModel = GetViewModel<BatcherfassungViewModel>; // TODO -> entf., wenn nicht nötig
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
            return PartialView("Partial/Grid", ViewModel);
        }

        [GridAction]
        public ActionResult BatcherfassungAjaxBinding()
        {
            return View(new GridModel(ViewModel.BatcherfassungsFiltered));
        }

        [HttpPost]
        public ActionResult LoadDataByModelId(string modelId)
        {
            return PartialView("Partial/DetailsForm", ViewModel.GetItemWithModelData(modelId));
        }

        //LoadDataByModelId
      

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

        
    }
}
