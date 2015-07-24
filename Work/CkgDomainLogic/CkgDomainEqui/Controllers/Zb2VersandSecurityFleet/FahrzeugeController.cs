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
        public Zb2BestandSecurityFleetViewModel Zb2BestandSecurityFleetViewModel { get { return GetViewModel<Zb2BestandSecurityFleetViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportZb2BestandSecurityFleet()
        {          
            Zb2BestandSecurityFleetViewModel.DataInit();
            return View(Zb2BestandSecurityFleetViewModel);
        }

        [HttpPost]
        public ActionResult LoadZb2BestandSecurityFleet(Zb2BestandSecurityFleetSelektor model)
        {
            Zb2BestandSecurityFleetViewModel.Zb2BestandSecurityFleetSelektor = model;

            Zb2BestandSecurityFleetViewModel.Validate(AddModelError);

            if (ModelState.IsValid)
            {
                Zb2BestandSecurityFleetViewModel.LoadZb2BestandSecurityFleet();
                if (Zb2BestandSecurityFleetViewModel.Zb2BestandSecurityFleets.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Zb2BestandSecurityFleet/SucheZb2BestandSecurityFleet", Zb2BestandSecurityFleetViewModel.Zb2BestandSecurityFleetSelektor);
        }

        [HttpPost]
        public ActionResult ShowZb2BestandSecurityFleet()
        {
            return PartialView("Zb2BestandSecurityFleet/Zb2BestandSecurityFleetGrid", Zb2BestandSecurityFleetViewModel);
        }

        [GridAction]
        public ActionResult Zb2BestandSecurityFleetAjaxBinding()
        {
            return View(new GridModel(Zb2BestandSecurityFleetViewModel.Zb2BestandSecurityFleetsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridZb2BestandSecurityFleet(string filterValue, string filterColumns)
        {
            Zb2BestandSecurityFleetViewModel.FilterZb2BestandSecurityFleet(filterValue, filterColumns);

            return new EmptyResult();
        }
       
        #region Export

        public ActionResult ExportZb2BestandSecurityFleetFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = Zb2BestandSecurityFleetViewModel.Zb2BestandSecurityFleetsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.RegistrationDocuments, dt);

            return new EmptyResult();
        }

        public ActionResult ExportZb2BestandSecurityFleetFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = Zb2BestandSecurityFleetViewModel.Zb2BestandSecurityFleetsFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.RegistrationDocuments, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
