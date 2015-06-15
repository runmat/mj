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
    public partial class FahrzeugeController
    {
        public UnfallmeldungenViewModel UnfallmeldungenViewModel { get { return GetViewModel<UnfallmeldungenViewModel>(); } }

        [CkgApplication]
        public ActionResult ReportUnfallmeldungen()
        {
            _dataContextKey = typeof(UnfallmeldungenViewModel).Name;
            UnfallmeldungenViewModel.DataInit(true);

            return View(UnfallmeldungenViewModel);
        }

        [CkgApplication]
        public ActionResult Unfallmeldungen()
        {
            _dataContextKey = typeof(UnfallmeldungenViewModel).Name;
            UnfallmeldungenViewModel.DataInit(false);
            UnfallmeldungenViewModel.LoadUnfallmeldungen();

            return View(UnfallmeldungenViewModel);
        }

        [HttpPost]
        public ActionResult LoadUnfallmeldungen(UnfallmeldungenSelektor model)
        {
            UnfallmeldungenViewModel.UnfallmeldungenSelektor = model;

            UnfallmeldungenViewModel.Validate(AddModelError);

            if (ModelState.IsValid)
            {
                UnfallmeldungenViewModel.LoadUnfallmeldungen();
                if (UnfallmeldungenViewModel.Unfallmeldungen.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            return PartialView("Unfallmeldungen/UnfallmeldungenSuche", UnfallmeldungenViewModel.UnfallmeldungenSelektor);
        }

        [HttpPost]
        public ActionResult ShowUnfallmeldungen()
        {
            return PartialView("Unfallmeldungen/UnfallmeldungenGrid", UnfallmeldungenViewModel);
        }

        [GridAction]
        public ActionResult UnfallmeldungenAjaxBinding()
        {
            return View(new GridModel(UnfallmeldungenViewModel.UnfallmeldungenFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridUnfallmeldungen(string filterValue, string filterColumns)
        {
            UnfallmeldungenViewModel.FilterUnfallmeldungen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult UnfallmeldungenSelectionChanged(string unfallNr, bool isChecked)
        {
            int allSelectionCount, allCount = 0, allFoundCount = 0;
            if (unfallNr.IsNullOrEmpty())
                UnfallmeldungenViewModel.SelectUnfallmeldungen(isChecked, f => f.IsValidForCancellation, out allSelectionCount, out allCount, out allFoundCount);
            else
                UnfallmeldungenViewModel.SelectUnfallmeldung(unfallNr, isChecked, out allSelectionCount);

            return Json(new { allSelectionCount, allCount, allFoundCount });
        }

        [HttpPost]
        public JsonResult UnfallmeldungenCancel(string cancelText)
        {
            string errorMessage; int cancelCount;
            UnfallmeldungenViewModel.UnfallmeldungenCancel(cancelText, out cancelCount, out errorMessage);

            var cancelCountMessage = (cancelCount == 0 ? "" : string.Format("{0} {1} {2} {3}", cancelCount, (cancelCount == 1 ? Localize.Vehicle : Localize.Vehicles), Localize.Successful.ToLower(), Localize.Cancelled.ToLower()));

            return Json(new { cancelCountMessage, errorMessage});
        }

        [HttpPost]
        public ActionResult MeldungCreateSearch(Unfallmeldung model)
        {
            if (model == null || model.MeldungTyp.IsNullOrEmpty())
            {
                UnfallmeldungenViewModel.MeldungForCreate = new Unfallmeldung { MeldungTyp = "U" };
                return PartialView("Unfallmeldungen/MeldungCreateSearch", UnfallmeldungenViewModel.MeldungForCreate);
            }

            UnfallmeldungenViewModel.MeldungForCreate = model;

            UnfallmeldungenViewModel.ValidateMeldungCreationSearch(ModelState.AddModelError);

            if (ModelState.IsValid)
                UnfallmeldungenViewModel.MeldungCreateTryLoadEqui(ModelState.AddModelError);

            return PartialView("Unfallmeldungen/MeldungCreateSearch", UnfallmeldungenViewModel.MeldungForCreate);
        }

        [HttpPost]
        public ActionResult MeldungCreateInit()
        {
            return PartialView("Unfallmeldungen/MeldungCreateEdit", UnfallmeldungenViewModel.MeldungForCreate);
        }

        [HttpPost]
        public ActionResult MeldungCreateEdit(Unfallmeldung model)
        {
            UnfallmeldungenViewModel.ValidateMeldungCreationEdit(ModelState.AddModelError, model);

            if (ModelState.IsValid)
                UnfallmeldungenViewModel.MeldungCreate(ModelState.AddModelError, model);

            return PartialView("Unfallmeldungen/MeldungCreateEdit", UnfallmeldungenViewModel.MeldungForCreate);
        }
       
        #region Export
       
        public ActionResult ExportUnfallmeldungenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = UnfallmeldungenViewModel.UnfallmeldungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.RegistrationRequests, dt);

            return new EmptyResult();
        }

        public ActionResult ExportUnfallmeldungenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = UnfallmeldungenViewModel.UnfallmeldungenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.RegistrationRequests, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}
