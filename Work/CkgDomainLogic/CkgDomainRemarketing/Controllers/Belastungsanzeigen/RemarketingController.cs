using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.Remarketing.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;
using GeneralTools.Models;
using MvcTools.Web;

namespace ServicesMvc.Controllers
{
    public partial class RemarketingController
    {
        public BelastungsanzeigenViewModel BelastungsanzeigenViewModel { get { return GetViewModel<BelastungsanzeigenViewModel>(); } }

        [CkgApplication]
        public ActionResult Belastungsanzeigen()
        {
            BelastungsanzeigenViewModel.DataInit();

            return View(BelastungsanzeigenViewModel);
        }

        [HttpPost]
        public ActionResult LoadBelastungsanzeigen(BelastungsanzeigenSelektor model)
        {
            if (ModelState.IsValid)
                BelastungsanzeigenViewModel.LoadBelastungsanzeigen(model, ModelState.AddModelError);

            return PartialView("Belastungsanzeigen/Suche", model);
        }

        [HttpPost]
        public ActionResult ShowBelastungsanzeigen()
        {
            return PartialView("Belastungsanzeigen/Grid", BelastungsanzeigenViewModel);
        }

        [GridAction]
        public ActionResult BelastungsanzeigenAjaxBinding()
        {
            return View(new GridModel(BelastungsanzeigenViewModel.BelastungsanzeigenFiltered));
        }

        [HttpPost]
        public JsonResult BelastungsanzeigenSelectionChanged(string id, bool isChecked)
        {
            int allSelectionCount;
            if (id.IsNullOrEmpty())
                BelastungsanzeigenViewModel.SelectBelastungsanzeigen(isChecked, f => true, out allSelectionCount);
            else
                BelastungsanzeigenViewModel.SelectBelastungsanzeige(id, isChecked, out allSelectionCount);

            return Json(new { allSelectionCount });
        }

        [HttpPost]
        public ActionResult BelastungsanzeigeSetReklamation(string fahrgestellNr)
        {
            var model = BelastungsanzeigenViewModel.GetSetReklamationModel(fahrgestellNr);

            return PartialView("Belastungsanzeigen/Partial/SetReklamationForm", model);
        }

        [HttpPost]
        public ActionResult SetReklamationForm(SetReklamationModel model)
        {
            if (ModelState.IsValid)
                BelastungsanzeigenViewModel.SetReklamation(model, ModelState.AddModelError);

            return PartialView("Belastungsanzeigen/Partial/SetReklamationForm", model);
        }

        [HttpPost]
        public ActionResult BelastungsanzeigeShowReklamation(string fahrgestellNr)
        {
            var model = BelastungsanzeigenViewModel.GetReklamationstext(fahrgestellNr);

            return PartialView("Belastungsanzeigen/Partial/InfoForm", model);
        }

        [HttpPost]
        public ActionResult BelastungsanzeigeShowBlockade(string fahrgestellNr)
        {
            var model = BelastungsanzeigenViewModel.GetBlockadetext(fahrgestellNr);

            return PartialView("Belastungsanzeigen/Partial/InfoForm", model);
        }

        [HttpPost]
        public ActionResult BelastungsanzeigeShowGutachten(string fahrgestellNr)
        {
            BelastungsanzeigenViewModel.LoadGutachten(fahrgestellNr, ModelState.AddModelError);

            return PartialView("Belastungsanzeigen/GridGutachten", BelastungsanzeigenViewModel);
        }

        [GridAction]
        public ActionResult GutachtenAjaxBinding()
        {
            return View(new GridModel(BelastungsanzeigenViewModel.GutachtenFiltered));
        }

        public ActionResult GetBelastungsanzeigePdf(string fin)
        {
            var pdfBytes = BelastungsanzeigenViewModel.GetBelastungsanzeigePdf(fin);

            if (pdfBytes == null)
                return new ContentResult { Content = Localize.NoDocumentsFound };

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = string.Format("{0}.pdf", Localize.DebitNote) };
        }

        public ActionResult GetReparaturKalkulationPdf(string fin)
        {
            var pdfBytes = BelastungsanzeigenViewModel.GetReparaturKalkulationPdf(fin);

            if (pdfBytes == null)
                return new ContentResult { Content = Localize.NoDocumentsFound };

            return new FileContentResult(pdfBytes, "application/pdf") { FileDownloadName = string.Format("{0}.pdf", Localize.RepairCalculation) };
        }

        [HttpPost]
        public ActionResult BelastungsanzeigeSetBlock()
        {
            if (BelastungsanzeigenViewModel.BelastungsanzeigenSelected.None())
                return Json(new { message = Localize.NoVehicleSelected });

            var model = BelastungsanzeigenViewModel.GetSetBlockadeModel();

            return PartialView("Belastungsanzeigen/Partial/SetBlockForm", model);
        }

        [HttpPost]
        public ActionResult SetBlockForm(SetBlockadeModel model)
        {
            if (ModelState.IsValid)
                BelastungsanzeigenViewModel.SetBlockade(model, ModelState.AddModelError);

            return PartialView("Belastungsanzeigen/Partial/SetBlockForm", model);
        }

        [HttpPost]
        public ActionResult BelastungsanzeigeSetNoBlock()
        {
            if (BelastungsanzeigenViewModel.BelastungsanzeigenSelected.None())
                return Json(new { message = Localize.NoVehicleSelected });

            if (ModelState.IsValid)
                BelastungsanzeigenViewModel.ResetBlockade(ModelState.AddModelError);

            return ShowBelastungsanzeigen();
        }

        [HttpPost]
        public ActionResult BelastungsanzeigeSetInArbeit()
        {
            if (BelastungsanzeigenViewModel.BelastungsanzeigenSelected.None())
                return Json(new { message = Localize.NoVehicleSelected });

            if (ModelState.IsValid)
                BelastungsanzeigenViewModel.SetInBearbeitung(ModelState.AddModelError);

            return ShowBelastungsanzeigen();
        }

        [HttpPost]
        public ActionResult BelastungsanzeigeSetOpen()
        {
            if (BelastungsanzeigenViewModel.BelastungsanzeigenSelected.None())
                return Json(new { message = Localize.NoVehicleSelected });

            if (ModelState.IsValid)
                BelastungsanzeigenViewModel.SetOffen(ModelState.AddModelError);

            return ShowBelastungsanzeigen();
        }

        [HttpPost]
        public ActionResult FilterGridBelastungsanzeigen(string filterValue, string filterColumns)
        {
            BelastungsanzeigenViewModel.FilterBelastungsanzeigen(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FilterGridGutachten(string filterValue, string filterColumns)
        {
            BelastungsanzeigenViewModel.FilterGutachten(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportBelastungsanzeigenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = BelastungsanzeigenViewModel.BelastungsanzeigenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Remarketing_Belastungsanzeigen, dt);

            return new EmptyResult();
        }

        public ActionResult ExportBelastungsanzeigenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = BelastungsanzeigenViewModel.BelastungsanzeigenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Remarketing_Belastungsanzeigen, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        public ActionResult ExportGutachtenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = BelastungsanzeigenViewModel.GutachtenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Surveys, dt);

            return new EmptyResult();
        }

        public ActionResult ExportGutachtenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = BelastungsanzeigenViewModel.GutachtenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Surveys, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
