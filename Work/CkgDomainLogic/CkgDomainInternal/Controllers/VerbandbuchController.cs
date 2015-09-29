using System;
using System.Web.Mvc;
using CkgDomainInternal.Verbandbuch.Contracts;
using CkgDomainInternal.Verbandbuch.Models;
using CkgDomainInternal.Verbandbuch.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;
using SapORM.Models;
using CkgDomainInternal.Verbandbuch.Services;
using DocumentTools.Services;
using Telerik.Web.Mvc;


namespace ServicesMvc.Controllers
{
    public class VerbandbuchController : CkgDomainController
    {
        [CkgApplication]
        public ActionResult Erfassung(string vkbur = null)
        {
            return View(ViewModel.GetVerbandBuchModel(vkbur));
        }
        [HttpPost]
        public ActionResult SaveVorfall(VerbandbuchModel vbModel)
        {

            if (ModelState.IsValid)
                ViewModel.SaveVorfall(vbModel, ModelState);


            return PartialView("Partial/FormErfassung", vbModel);
        }

        [CkgApplication]
        public ActionResult Report(string vkbur = null)
        {
            ViewModel.DataInit();
            ViewModel.GetVerbandbuchEntries(vkbur);
            return View(ViewModel);
        }

        [GridAction]
        public ActionResult VerbandbuchAjaxBinding()
        {
            return View(new GridModel(ViewModel.VerbandbuchFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridVerbandbuch(string filterValue, string filterColumns)
        {
            ViewModel.FilterVerbandbuch(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportVerbandbuchFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.VerbandbuchFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Verbandbuch", dt);

            return new EmptyResult();
        }

        public ActionResult ExportVerbandbuchFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.VerbandbuchFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Verbandbuch", dt, landscapeOrientation: true);

            return new EmptyResult();
        }




        public VerbandbuchController(IAppSettings appSettings, ILogonContextDataService logonContext, IVerbandbuchDataService verbandbuchDataService) : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, verbandbuchDataService);
        }

        public override string DataContextKey { get { return "VerbandbuchViewModel"; } }

        public VerbandbuchViewModel ViewModel { get { return GetViewModel<VerbandbuchViewModel>(); } }
    }
}
