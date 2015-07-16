using System;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class EquiController
    {
        public EquiHistorieVermieterViewModel EquipmentHistorieVermieterViewModel { get { return GetViewModel<EquiHistorieVermieterViewModel>(); } }

        [CkgApplication]
        public ActionResult FahrzeugHistorieVermieter()
        {
            return View(EquipmentHistorieVermieterViewModel);
        }
               
        public ActionResult GetHistorieVermieterByFinPartial(string fahrgestellnummer)
        {
            var model = new EquiHistorieSuchparameter { FahrgestellNr = fahrgestellnummer };
            EquipmentHistorieVermieterViewModel.LoadHistorieInfos(ref model, ModelState);

            if (EquipmentHistorieVermieterViewModel.HistorieInfos != null && EquipmentHistorieVermieterViewModel.HistorieInfos.Any())
                EquipmentHistorieVermieterViewModel.LoadHistorie(EquipmentHistorieVermieterViewModel.HistorieInfos[0].FahrgestellNr);
        
            return PartialView("Historie/HistorieVermieterDetail", EquipmentHistorieVermieterViewModel.EquipmentHistorie);
        }

        [HttpPost]
        public ActionResult GetFahrzeugHistorieVermieterPartial(string fahrgestellNr)
        {
            EquipmentHistorieVermieterViewModel.LoadHistorie(fahrgestellNr);

            return PartialView("Historie/HistorieVermieterDetail", EquipmentHistorieVermieterViewModel.EquipmentHistorie);
        }

        [HttpPost]
        public ActionResult LoadHistorieVermieterEquis(EquiHistorieSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                EquipmentHistorieVermieterViewModel.LoadHistorieInfos(ref model, ModelState);
            }

            return PartialView("Historie/HistorieVermieterSuche", model);
        }

        [HttpPost]
        public ActionResult ShowHistorieVermieterEquis()
        {
            return PartialView("Historie/HistorieVermieterGrid", EquipmentHistorieVermieterViewModel);
        }

        [GridAction]
        public ActionResult EquiHistorieInfosVermieterAjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieVermieterViewModel.HistorieInfosFiltered));
        }

        [GridAction]
        public ActionResult EquiHistorieVermieterLebenslaufZb2AjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieVermieterViewModel.EquipmentHistorie.LebenslaufZb2));
        }

        [GridAction]
        public ActionResult EquiHistorieVermieterLebenslaufFsmAjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieVermieterViewModel.EquipmentHistorie.LebenslaufFsm));
        }

        [GridAction]
        public ActionResult EquiHistorieVermieterTueteninhaltFsmAjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieVermieterViewModel.EquipmentHistorie.InhalteFsm));
        }

        [GridAction]
        public ActionResult EquiHistorieVermieterFahrzeugAnforderungenAjaxBinding()
        {
            return View(new GridModel(EquipmentHistorieVermieterViewModel.EquipmentHistorie.FahrzeugAnforderungen));
        }

        [HttpPost]
        public ActionResult FahrzeugAnforderungNew()
        {
            ModelState.Clear();
            return PartialView("Historie/Partial/FahrzeugAnforderungDetailsForm", EquipmentHistorieVermieterViewModel.FahrzeugAnforderungNew());
        }

        [HttpPost]
        public ActionResult FahrzeugAnforderungDetailsFormSave(FahrzeugAnforderung model)
        {
            var viewModel = EquipmentHistorieVermieterViewModel;

            if (ModelState.IsValid)
                viewModel.FahrzeugAnforderungSave(model, ModelState.AddModelError);

            return PartialView("Historie/Partial/FahrzeugAnforderungDetailsForm", model);
        }


        public FileContentResult FahrzeughistorieVermieterPdf()
        {
            var formularPdfBytes = EquipmentHistorieVermieterViewModel.GetHistorieAsPdf();

            return new FileContentResult(formularPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}_{1}.pdf", Localize.VehicleHistory, EquipmentHistorieVermieterViewModel.EquipmentHistorie.HistorieInfo.FahrgestellNr) };
        }

        [HttpPost]
        public ActionResult FilterGridEquiHistorieInfosVermieter(string filterValue, string filterColumns)
        {
            EquipmentHistorieVermieterViewModel.FilterHistorieInfos(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportEquiHistorieInfosVermieterFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = EquipmentHistorieVermieterViewModel.HistorieInfosFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.VehicleHistory, dt);

            return new EmptyResult();
        }

        public ActionResult ExportEquiHistorieInfosVermieterFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = EquipmentHistorieVermieterViewModel.HistorieInfosFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.VehicleHistory, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
