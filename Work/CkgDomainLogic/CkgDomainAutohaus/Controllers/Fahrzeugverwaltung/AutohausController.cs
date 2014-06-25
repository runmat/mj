using System.Web.Mvc;
using System.Web.Routing;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Autohaus.ViewModels;
using DocumentTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Autohaus-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class AutohausController
    {
        public FahrzeugverwaltungViewModel FahrzeugverwaltungViewModel { get { return GetViewModel<FahrzeugverwaltungViewModel>(); } }

        [CkgApplication]
        public ActionResult Fahrzeugverwaltung()
        {
            FahrzeugverwaltungViewModel.DataInit();

            return View(FahrzeugverwaltungViewModel);
        }

        [GridAction]
        public ActionResult FahrzeugeAjaxBinding()
        {
            return View(new GridModel(FahrzeugverwaltungViewModel.FahrzeugeFiltered));
        }

        [HttpPost]
        public ActionResult FilterFahrzeugGrid(string filterValue, string filterColumns)
        {
            FahrzeugverwaltungViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FahrzeugCreate()
        {
            FahrzeugverwaltungViewModel.InsertMode = true;
            ModelState.Clear();

            return PartialView("Fahrzeugverwaltung/FahrzeugDetailsForm", FahrzeugverwaltungViewModel.FahrzeugCreate().SetInsertMode(FahrzeugverwaltungViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult FahrzeugEdit(int id)
        {
            FahrzeugverwaltungViewModel.InsertMode = false;
            ModelState.Clear();

            return PartialView("Fahrzeugverwaltung/FahrzeugDetailsForm", FahrzeugverwaltungViewModel.FahrzeugGet(id).SetInsertMode(FahrzeugverwaltungViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult FahrzeugDelete(int id)
        {
            FahrzeugverwaltungViewModel.FahrzeugDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FahrzeugDetailsFormSave(Fahrzeug model)
        {
            // Avoid ModelState clearing on saving 
            // => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();
            ModelState.SetModelValue("ID", null);

            if (ModelState.IsValid)
                model = FahrzeugverwaltungViewModel.FahrzeugSave(model, ModelState.AddModelError);

            model.InsertModeTmp = FahrzeugverwaltungViewModel.InsertMode;

            return PartialView("Fahrzeugverwaltung/FahrzeugDetailsForm", model);
        }

        #region Export

        public ActionResult ExportFahrzeugeFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugverwaltungViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Fahrzeuge", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugeFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugverwaltungViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Fahrzeuge", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
