using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Insurance.ViewModels;
using DocumentTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class InsuranceController
    {
        public VertragsverlaengerungViewModel VertragsverlaengerungViewModel { get { return GetViewModel<VertragsverlaengerungViewModel>(); } }

        [CkgApplication]
        public ActionResult Vertragsverlaengerung()
        {
            VertragsverlaengerungViewModel.LoadVertragsdaten(ModelState);

            return View(VertragsverlaengerungViewModel);
        }

        [GridAction]
        public ActionResult VertragsdatenAjaxBinding()
        {
            return View(new GridModel(VertragsverlaengerungViewModel.GridItems));
        }

        [HttpPost]
        public ActionResult SaveVertragsverlaengerungen(string selectedItems)
        {
            VertragsverlaengerungViewModel.SaveChangesToSap(selectedItems, ModelState);

            return PartialView("Vertragsverlaengerung/VertragsdatenGrid", VertragsverlaengerungViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridVertragsdaten(string filterValue, string filterColumns)
        {
            VertragsverlaengerungViewModel.FilterVertragsdaten(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Export

        public ActionResult ExportVertragsdatenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = VertragsverlaengerungViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Vertragsverlaengerung", dt);

            return new EmptyResult();
        }

        public ActionResult ExportVertragsdatenFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = VertragsverlaengerungViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Vertragsverlaengerung", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
