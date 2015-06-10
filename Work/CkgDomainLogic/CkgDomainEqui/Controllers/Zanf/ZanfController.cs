using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Zanf.Models;
using CkgDomainLogic.Zanf.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class ZanfController  
    {
        public ZanfReportViewModel ZanfReportViewModel { get { return GetViewModel<ZanfReportViewModel>(); } }

        [CkgApplication]
        public ActionResult ZanfReport()
        {
            return View(ZanfReportViewModel);
        }

        [HttpPost]
        public ActionResult LoadZulassungsAnforderungen(ZulassungsAnforderungSuchparameter model)
        {
            if (ModelState.IsValid)
                ZanfReportViewModel.LoadZulassungsAnforderungen(model, ModelState);

            return PartialView("ZanfReport/ZanfSuche", model);
        }

        [HttpPost]
        public ActionResult ShowZulassungsAnforderungen()
        {
            return PartialView("ZanfReport/ZanfGrid", ZanfReportViewModel);
        }

        [GridAction]
        public ActionResult ZulassungsAnforderungenAjaxBinding()
        {
            return View(new GridModel(ZanfReportViewModel.ZulassungsAnforderungenForGrid));
        }

        [HttpPost]
        public ActionResult ShowZanfKlaerfalltext(string anforderungsNr, string hauptpositionsNr, string auftragsNr)
        {
            return PartialView("ZanfReport/ZanfKlaerfalltext", ZanfReportViewModel.GetItem(anforderungsNr, hauptpositionsNr, auftragsNr));
        }

        [HttpPost]
        public ActionResult FilterGridZulassungsAnforderungen(string filterValue, string filterColumns)
        {
            ZanfReportViewModel.FilterZulassungsAnforderungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportZulassungsAnforderungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ZanfReportViewModel.ZulassungsAnforderungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.RegistrationRequests, dt);

            return new EmptyResult();
        }

        public ActionResult ExportZulassungsAnforderungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ZanfReportViewModel.ZulassungsAnforderungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.RegistrationRequests, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
