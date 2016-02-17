using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.ZldPartner.Contracts;
using CkgDomainLogic.ZldPartner.Models;
using CkgDomainLogic.ZldPartner.ViewModels;
using DocumentTools.Services;
using GeneralTools.Contracts;
using Telerik.Web.Mvc;

namespace ServicesMvc.ZldPartner.Controllers
{
    public class ZldPartnerController : CkgDomainController
    {
        public override string DataContextKey { get { return GetDataContextKey<ZldPartnerZulassungenViewModel>(); } }

        public ZldPartnerZulassungenViewModel ViewModel { get { return GetViewModel<ZldPartnerZulassungenViewModel>(); } }

        public ZldPartnerController(IAppSettings appSettings, ILogonContextDataService logonContext, IZldPartnerZulassungenDataService partnerZulassungenDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, partnerZulassungenDataService);
            InitModelStatics();
        }

        void InitModelStatics()
        {
            OffeneZulassung.GetViewModel = GetViewModel<ZldPartnerZulassungenViewModel>;
        }


        [CkgApplication]
        public ActionResult DurchzufuehrendeZulassungen()
        {
            ViewModel.DataInit();
            ViewModel.LoadOffeneZulassungen();

            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult ReportDurchgefuehrteZulassungen()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }


        #region Durchzuführende Zulassungen

        [GridAction]
        public ActionResult OffeneZulassungenAjaxBinding()
        {
            return View(new GridModel(ViewModel.OffeneZulassungenGridItemsFiltered));
        }

        [HttpPost]
        public ActionResult UpdateOffeneZulassung(string datensatzId, string property, string value)
        {
            ViewModel.ApplyChangedData(datensatzId, property, value);

            return Json(new { showSendButton = ViewModel.SendingEnabled });
        }

        [HttpPost]
        public ActionResult UpdateOffeneZulassungStorno(string datensatzId, string status)
        {
            var model = new StornoModel { DatensatzId = datensatzId, Status = status };

            return PartialView("DurchzufuehrendeZulassungen/StornoDialogForm", model);
        }

        [HttpPost]
        public ActionResult StornoDialogForm(StornoModel model)
        {
            if (ModelState.IsValid)
                ViewModel.ApplyChangedData(model.DatensatzId, "Status", model.Status, model.GrundId, model.Bemerkung);

            return PartialView("DurchzufuehrendeZulassungen/StornoDialogForm", model);
        }

        [HttpPost]
        public ActionResult OffeneZulassungenSpeichern(bool absenden)
        {
            ViewModel.SaveOffeneZulassungen(!absenden, ModelState);

            return PartialView("DurchzufuehrendeZulassungen/Grid", ViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridOffeneZulassungen(string filterValue, string filterColumns)
        {
            ViewModel.FilterOffeneZulassungenGridItems(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportOffeneZulassungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.OffeneZulassungenGridItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("DurchzufuehrendeZulassungen", dt);

            return new EmptyResult();
        }

        public ActionResult ExportOffeneZulassungenFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.OffeneZulassungenGridItemsFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("DurchzufuehrendeZulassungen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion


        #region Report durchgeführte Zulassungen

        [HttpPost]
        public ActionResult LoadDurchgefuehrteZulassungen(DurchgefuehrteZulassungenSuchparameter model)
        {
            ViewModel.DurchgefuehrteZulassungenSelektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadDurchgefuehrteZulassungen(ModelState);

            return PartialView("ReportDurchgefuehrteZulassungen/Suche", ViewModel.DurchgefuehrteZulassungenSelektor);
        }

        [HttpPost]
        public ActionResult ShowDurchgefuehrteZulassungen()
        {
            return PartialView("ReportDurchgefuehrteZulassungen/Grid", ViewModel);
        }

        [GridAction]
        public ActionResult DurchgefuehrteZulassungenAjaxBinding()
        {
            return View(new GridModel(ViewModel.DurchgefuehrteZulassungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridDurchgefuehrteZulassungen(string filterValue, string filterColumns)
        {
            ViewModel.FilterDurchgefuehrteZulassungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportDurchgefuehrteZulassungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DurchgefuehrteZulassungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("DurchgefuehrteZulassungen", dt);

            return new EmptyResult();
        }

        public ActionResult ExportDurchgefuehrteZulassungenFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.DurchgefuehrteZulassungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("DurchgefuehrteZulassungen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
