﻿using System;
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

        [HttpPost]
        public ActionResult GetFahrzeugHistorieVermieterPartial(string equiNr, string meldungsNr)
        {
            EquipmentHistorieVermieterViewModel.LoadHistorie(equiNr, meldungsNr);

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
