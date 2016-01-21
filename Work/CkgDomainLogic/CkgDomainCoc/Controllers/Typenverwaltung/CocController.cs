using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.CoC.Services;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using SapORM.Services;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// COC Typenverwaltung (Stammdatenverwaltung)
    /// </summary>
    public partial class CocController 
    {
        [CkgApplication]
        public ActionResult TypenVerwaltung()
        {
            CocTypenViewModel.DataMarkForRefresh();

            return View(CocTypenViewModel);
        }

        [GridAction]
        public ActionResult CocItemsAjaxBinding()
        {
            var items = CocTypenViewModel.CocTypenFiltered;

            return View(new GridModel(items));
        }

        [HttpGet]
        public ActionResult GetCocAsPdf(string ag, string vorgangsnummer, string vorlage)
        {
            var sapDataService = new SapDataServiceTestSystemNoCacheFactory().Create();

            sapDataService.InitExecute("Z_DPM_Print_Coc_01", "I_AG,I_VORG_NR,I_VKZ,I_PDF", ag, vorgangsnummer, vorlage.ToUpper(),"X");
            var pdfAsByteArray = sapDataService.GetExportParameterByte("E_PDF");
            var subrc = sapDataService.GetExportParameter("E_SUBRC");

            if (subrc != "0")
            {
                return new ContentResult{ Content = "Datei konnte nicht ermittelt werden"};
            }

            return new FileContentResult(pdfAsByteArray, "application/pdf");
        }

        [HttpGet]
        public ActionResult GetCocAsPdfTest(string ag, string vorgangsnummer, string vorlage)
        {
            return new FileContentResult(FileService.GetBytesFromFile(Path.Combine(AppSettings.DataPath, "TestCOC.pdf")), "application/pdf");
        }


        [HttpPost]
        public JsonResult RefreshCascadedFilter(int level, string searchCascadedTyp, string searchCascadedVar, string searchCascadedVers)
        {
            if (level == 1)
                searchCascadedVar = searchCascadedVers = null;
            if (level == 2)
                searchCascadedVers = null;

            CocTypenViewModel.SetCascadedFilter(searchCascadedTyp, searchCascadedVar, searchCascadedVers);

            var json = Json(new
                        {
                            searchCascadedTypGroups = CocTypenViewModel.FilterCocTypGroups,
                            searchCascadedVarGroups = CocTypenViewModel.FilterCocVarGroups,
                            searchCascadedVersGroups = CocTypenViewModel.FilterCocVersGroups,
                        }
                );

            return json;
        }

        [HttpPost]
        public ActionResult EditCocTyp(int id)
        {
            CocTypenViewModel.InsertMode = false;
            ModelState.Clear();
            return PartialView("Partial/CocDetailsForm", CocTypenViewModel.GetItem(id).SetInsertMode(CocTypenViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult DuplicateCocTyp(int id)
        {
            CocTypenViewModel.InsertMode = true;

            // very important here, because we duplicate the one item and modelstate should distinguish between original and new item
            ModelState.Clear();

            return PartialView("Partial/CocDetailsForm", CocTypenViewModel.DuplicateItem(id).SetInsertMode(CocTypenViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult NewCocTyp()
        {
            CocTypenViewModel.InsertMode = true;
            ModelState.Clear();
            return PartialView("Partial/CocDetailsForm", CocTypenViewModel.NewItem().SetInsertMode(CocTypenViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult DeleteCocTyp(int id)
        {
            CocTypenViewModel.RemoveItem(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult CocDetailsFormSave(CocEntity model)
        {
            // Avoid ModelState clearing on saving!!!
            // => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();
            
            TestDelayLong();

            if (model.NoSaveButUiRefreshOnly)
            {
                ModelState.SetModelValue(model, m => m.NoSaveButUiRefreshOnly, false);
                return PartialView("Partial/CocDetailsForm", model);
            }

            ConvertPropertyModelErrors();

            var viewModel = (model.IsCocOrder ? (ICocEntityViewModel)CocErfassungViewModel : CocTypenViewModel);

            viewModel.ValidateModel(model, viewModel.InsertMode, AddModelError);

            if (ModelState.IsValid)
            {
                if (viewModel.InsertMode)
                    viewModel.AddItem(model);

                model = viewModel.SaveItem(model, AddModelError);
            }

            model.IsValid = ModelState.IsValid;
            model.InsertModeTmp = viewModel.InsertMode;

            return PartialView("Partial/CocDetailsForm", model);
        }

        private void ConvertPropertyModelErrors()
        {
            var modelErros = ModelState.Where(keyValuePair => keyValuePair.Value.Errors.Any());
                
            modelErros.ToList().ForEach(modelState =>
                {
                    var errorMessage = modelState.Value.Errors.Select(f => f.ErrorMessage).First();
                    modelState.Value.Errors.Clear();
                    AddModelError(modelState.Key, errorMessage);
                });
        }

        private void AddModelError(Expression<Func<CocEntity, object>> expression, string errorMessage)
        {
            AddModelError(expression.GetPropertyName(), errorMessage);
        }

        private void AddModelError(string key, string errorMessage)
        {
            var groupName = CocTypenLayoutService.GetPropertyGroupName(key);

            if (errorMessage.IsNotNullOrEmpty())
                errorMessage = string.Format("Block {0}: {1}", groupName, errorMessage);

            ModelState.AddModelError(key, new Exception (errorMessage.NotNullOrEmpty()) { HelpLink = groupName.Replace('.','_') });
        }

        public new ActionResult ExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = CocTypenViewModel.CocTypenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("CocTypen", dt);

            return new EmptyResult();
        }

        public new ActionResult ExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = CocTypenViewModel.CocTypenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("CocTypen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
