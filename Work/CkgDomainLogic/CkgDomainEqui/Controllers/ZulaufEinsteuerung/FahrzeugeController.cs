using System.Collections;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController : CkgDomainController
    {
        public ZulaufEinsteuerungViewModel ZulaufEinsteuerungViewModel { get { return GetViewModel<ZulaufEinsteuerungViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportZulaufEinsteuerung()
        {          
            ZulaufEinsteuerungViewModel.DataInit();          
            return View(ZulaufEinsteuerungViewModel);
        }

        [HttpPost]
        public ActionResult LoadZulaufEinsteuerung()
        {                     
            if (ModelState.IsValid)
            {
                ZulaufEinsteuerungViewModel.LoadZulaufEinsteuerung();
                if (ZulaufEinsteuerungViewModel.ZulaufEinsteuerungs.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("ZulaufEinsteuerung/ZulaufEinsteuerungSuche", ZulaufEinsteuerungViewModel);
        }

        [HttpPost]
        public ActionResult ShowZulaufEinsteuerung()
        {
            return PartialView("ZulaufEinsteuerung/ZulaufEinsteuerungGrid", ZulaufEinsteuerungViewModel);
        }

        [GridAction]
        public ActionResult ZulaufEinsteuerungAjaxBinding()
        {
            return View(new GridModel(ZulaufEinsteuerungViewModel.ZulaufEinsteuerungsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridZulaufEinsteuerung(string filterValue, string filterColumns)
        {
            ZulaufEinsteuerungViewModel.FilterZulaufEinsteuerung(filterValue, filterColumns);

            return new EmptyResult();
        }
       
        #region Export

        public ActionResult ExportZulaufEinsteuerungFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ZulaufEinsteuerungViewModel.ZulaufEinsteuerungsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.RegistrationDocuments, dt);

            return new EmptyResult();
        }

        public ActionResult ExportZulaufEinsteuerungFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ZulaufEinsteuerungViewModel.ZulaufEinsteuerungsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.RegistrationDocuments, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
