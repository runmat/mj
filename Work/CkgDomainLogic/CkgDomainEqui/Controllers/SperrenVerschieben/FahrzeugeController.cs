using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController
    {
        public FahrzeugSperrenVerschiebenViewModel SperrenVerschiebenViewModel { get { return GetViewModel<FahrzeugSperrenVerschiebenViewModel>(); } }

        [CkgApplication]
        public ActionResult SperrenVerschieben()
        {
            _dataContextKey = typeof(FahrzeugSperrenVerschiebenViewModel).Name;
            SperrenVerschiebenViewModel.LoadFahrzeuge();

            return View(SperrenVerschiebenViewModel);
        }

        [GridAction]
        public ActionResult FzgSperrenVerschiebenAjaxBinding()
        {
            return View(new GridModel(SperrenVerschiebenViewModel.FahrzeugeFiltered));
        }

        [HttpPost]
        public ActionResult FilterFzgSperrenVerschiebenData(string auswahl, bool nurMitBemerkung)
        {
            SperrenVerschiebenViewModel.ApplyDatenfilter(auswahl, nurMitBemerkung);
            SperrenVerschiebenViewModel.DataMarkForRefresh();

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FilterGridFzgSperrenVerschieben(string filterValue, string filterColumns)
        {
            SperrenVerschiebenViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        #region Export

        public ActionResult ExportFzgSperrenVerschiebenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = SperrenVerschiebenViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Fahrzeuge_SperrenVerschieben, dt);

            return new EmptyResult();
        }

        public ActionResult ExportFzgSperrenVerschiebenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = SperrenVerschiebenViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Fahrzeuge_SperrenVerschieben, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
