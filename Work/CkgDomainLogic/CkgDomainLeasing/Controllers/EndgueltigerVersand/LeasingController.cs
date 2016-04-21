using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Models.UIModels;
using CkgDomainLogic.Leasing.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;
using GeneralTools.Models;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Leasing-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class LeasingController : CkgDomainController
    {
        public LeasingEndgueltigerVersandViewModel EndgueltigerVersandViewModel { get { return GetViewModel<LeasingEndgueltigerVersandViewModel>(); } }

        [CkgApplication]
        public ActionResult EndgueltigerVersand()
        {
            return View(EndgueltigerVersandViewModel);
        }



        [HttpPost]
        public ActionResult ShowEndgueltigerVersand()
        {
            return PartialView("EndgueltigerVersand/EndgueltigerVersandGrid", EndgueltigerVersandViewModel);
        }

        [HttpPost]
        public ActionResult GetTempVersand(EndgueltigerVersandSuchParameter model)
        {

            EndgueltigerVersandViewModel.EndgueltigerVersandSelektor = model;


            if (ModelState.IsValid)
            {
                EndgueltigerVersandViewModel.GetTemporaryMarkedVersand();
               
            }

            return PartialView("EndgueltigerVersand/EndgueltigerVersandGrid", EndgueltigerVersandViewModel.EndgueltigerVersandSelektor);
        }

        [GridAction]
        public ActionResult EndgueltigerVersandAjaxBinding()
        {
            return View(new GridModel(EndgueltigerVersandViewModel.EndgueltigerVersandInfosFiltered));
        }

        [HttpPost]
        public ActionResult SendFinally()
        {

            string error = EndgueltigerVersandViewModel.VersendeSelektierteEndgueltig();

            if (error.IsNotNullOrEmpty())
                ModelState.AddModelError("Error", error);
            return PartialView("EndgueltigerVersand/EndgueltigerVersandGrid", EndgueltigerVersandViewModel);
        }


        #region MultiSelect

        [HttpPost]
        public JsonResult VorgangAuswahlSelectionChanged(string id, bool isChecked)
        {
            int allSelectionCount, allCount = 0;
            if (id.IsNullOrEmpty())
                EndgueltigerVersandViewModel.SelectVersandDatensaetze(isChecked, f => true, out allSelectionCount, out allCount);
            else
                EndgueltigerVersandViewModel.SelectVersandDatensatz(id, isChecked, out allSelectionCount);

            return Json(new
            {
                allSelectionCount,
                allCount
            }); 
        }

        #endregion


        #region Filter

        [HttpPost]
        public ActionResult FilterGridEndgueltigerVersand(string filterValue, string filterColumns)
        {
            EndgueltigerVersandViewModel.FilterEndgueltigerVersandInfos(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion


        #region Export

        public ActionResult ExportEndgueltigerVersandFilteredExcel (int page, string orderBy, string filterBy)
        {
            var dt = EndgueltigerVersandViewModel.EndgueltigerVersandInfosFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.RegistrationDocuments, dt);

            return new EmptyResult();
        }

        public ActionResult ExportEndgueltigerVersandFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = EndgueltigerVersandViewModel.EndgueltigerVersandInfosFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.RegistrationDocuments, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

    }
}