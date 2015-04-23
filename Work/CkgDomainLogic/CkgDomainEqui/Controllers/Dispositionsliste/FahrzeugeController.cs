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
        public DispositionslisteViewModel DispositionslisteViewModel { get { return GetViewModel<DispositionslisteViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportDispositionsliste()
        {          
            DispositionslisteViewModel.DataInit();
            return View(DispositionslisteViewModel);
        }

        [HttpPost]
        public ActionResult LoadDispositionsliste(DispositionslisteSelektor model)
        {
            DispositionslisteViewModel.DispositionslisteSelektor = model;

            DispositionslisteViewModel.Validate(AddModelError);

            if (ModelState.IsValid)
            {
                DispositionslisteViewModel.LoadDispositionsliste();
                if (DispositionslisteViewModel.Dispositionslistes.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Dispositionsliste/DispositionslisteSuche", DispositionslisteViewModel.DispositionslisteSelektor);
        }

        [HttpPost]
        public ActionResult ShowDispositionsliste()
        {
            return PartialView("Dispositionsliste/DispositionslisteGrid", DispositionslisteViewModel);
        }

        [GridAction]
        public ActionResult DispositionslisteAjaxBinding()
        {
            return View(new GridModel(DispositionslisteViewModel.DispositionslistesFiltered));
        }

        [HttpPost]
        public ActionResult FilterDispositionslisteGrid(string filterValue, string filterColumns)
        {
            DispositionslisteViewModel.FilterDispositionsliste(filterValue, filterColumns);

            return new EmptyResult();
        }
       
        #region Export

        public ActionResult ExportDispositionslisteFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = DispositionslisteViewModel.DispositionslistesFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.RegistrationDocuments, dt);

            return new EmptyResult();
        }

        public ActionResult ExportDispositionslisteFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = DispositionslisteViewModel.DispositionslistesFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.RegistrationDocuments, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
