using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Insurance.ViewModels;
using DocumentTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class InsuranceController
    {
        public BestandsdatenViewModel BestandsdatenViewModel { get { return GetViewModel<BestandsdatenViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportBestandsdatenGesamt()
        {
            BestandsdatenViewModel.LoadBestandsdaten();

            return View("ReportBestandsdaten", BestandsdatenViewModel);
        }

        [CkgApplication]
        public ActionResult ReportBestandsdaten()
        {
            BestandsdatenViewModel.LoadBestandsdaten(true);

            return View(BestandsdatenViewModel);
        }

        [GridAction]
        public ActionResult BestandsdatenAjaxBinding()
        {
            return View(new GridModel(BestandsdatenViewModel.BestandsdatenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridBestandsdaten(string filterValue, string filterColumns)
        {
            BestandsdatenViewModel.FilterBestandsdaten(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Export

        public ActionResult ExportBestandsdatenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = BestandsdatenViewModel.BestandsdatenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Bestandsdaten", dt);

            return new EmptyResult();
        }

        public ActionResult ExportBestandsdatenFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = BestandsdatenViewModel.BestandsdatenFiltered.GetGridFilteredDataTable(orderBy, filterBy, CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Bestandsdaten", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
