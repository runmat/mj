using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Leasing.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Leasing-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class LeasingController : CkgDomainController 
    {
        public LeasingAbmeldungViewModel AbmeldungViewModel { get { return GetViewModel<LeasingAbmeldungViewModel>(); } }

        [CkgApplication]
        public ActionResult BeauftragungAbmeldung()
        {
            AbmeldungViewModel.ResetSubmitState();
            AbmeldungViewModel.LoadAbzumeldendeFzge(ModelState);

            return View(AbmeldungViewModel);
        }

        [GridAction]
        public ActionResult AbzumeldendeFzgAjaxBinding()
        {
            return View(new GridModel(AbmeldungViewModel.GridItems));
        }

        [HttpPost]
        public ActionResult AbmeldungenSelectAll()
        {
            return Json(AbmeldungViewModel.GetFahrgestellnummern(), "text/plain");
        }

        [HttpPost]
        public ActionResult AbmeldungenSwitchToSubmitMode(string selectedItems)
        {
            AbmeldungViewModel.SwitchToSubmitMode(selectedItems);

            return PartialView("Abmeldung/AbzumeldendeFzgGrid", AbmeldungViewModel);
        }

        [HttpPost]
        public ActionResult AbmeldungenSubmitChanges()
        {
            AbmeldungViewModel.SaveChangesToSap(ModelState);

            return PartialView("Abmeldung/AbzumeldendeFzgGrid", AbmeldungViewModel);
        }

        [HttpPost]
        public ActionResult AbmeldungenEditChanges()
        {
            AbmeldungViewModel.ResetSubmitState();

            return PartialView("Abmeldung/AbzumeldendeFzgGrid", AbmeldungViewModel);
        }

        [HttpPost]
        public ActionResult AbmeldungenReinitModel()
        {
            AbmeldungViewModel.ResetSubmitState();
            AbmeldungViewModel.LoadAbzumeldendeFzge(ModelState);

            return PartialView("Abmeldung/AbzumeldendeFzgGrid", AbmeldungViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridAbzumeldendeFzg(string filterValue, string filterColumns)
        {
            AbmeldungViewModel.FilterAbzumeldendeFzge(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportAbzumeldendeFzgFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = AbmeldungViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("AbzumeldendeFzg", dt);

            return new EmptyResult();
        }

        public ActionResult ExportAbzumeldendeFzgFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = AbmeldungViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("AbzumeldendeFzg", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
