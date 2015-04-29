using System.Web.Mvc;
using CkgDomainLogic.Feinstaub.Models;
using CkgDomainLogic.Feinstaub.ViewModels;
using CkgDomainLogic.General.Controllers;
using DocumentTools.Services;
using Telerik.Web.Mvc;

namespace AutohausPortalMvc.Controllers
{
    /// <summary>
    /// Feinstaub-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FeinstaubController  
    {
        public AutohausFeinstaubReportViewModel FeinstaubReportViewModel { get { return GetViewModel<AutohausFeinstaubReportViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportFeinstaubplaketten()
        {
            FeinstaubReportViewModel.LoadStammdaten(ModelState);

            return View(FeinstaubReportViewModel);
        }

        [HttpPost]
        public ActionResult LoadVergabeInfos(FeinstaubSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                FeinstaubReportViewModel.LoadVergabeInfos(model, ModelState);
            }

            return PartialView("Partial/Suche", model);
        }

        [HttpPost]
        public ActionResult ShowVergabeInfos()
        {
            return PartialView("Partial/Grid", FeinstaubReportViewModel);
        }

        [GridAction]
        public ActionResult VergabeInfosAjaxBinding()
        {
            return View(new GridModel(FeinstaubReportViewModel.VergabeInfos));
        }

        public ActionResult ExportVergabeInfosExcel(int page, string orderBy, string filterBy)
        {
            var dt = FeinstaubReportViewModel.VergabeInfos.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Feinstaubplakettenstatistik", dt);

            return new EmptyResult();
        }

        public ActionResult ExportVergabeInfosPDF(int page, string orderBy, string filterBy)
        {
            var dt = FeinstaubReportViewModel.VergabeInfos.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Feinstaubplakettenstatistik", dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
