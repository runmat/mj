using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.Remarketing.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class RemarketingController
    {
        public GemeldeteVorschaedenViewModel GemeldeteVorschaedenViewModel { get { return GetViewModel<GemeldeteVorschaedenViewModel>(); } }

        [CkgApplication]
        public ActionResult GemeldeteVorschaeden()
        {
            GemeldeteVorschaedenViewModel.DataInit();

            return View(GemeldeteVorschaedenViewModel);
        }

        [HttpPost]
        public ActionResult LoadGemeldeteVorschaeden(GemeldeteVorschaedenSelektor model)
        {
            if (ModelState.IsValid)
                GemeldeteVorschaedenViewModel.LoadGemeldeteVorschaeden(model, ModelState.AddModelError);

            return PartialView("GemeldeteVorschaeden/Suche", model);
        }

        [HttpPost]
        public ActionResult ShowGemeldeteVorschaeden()
        {
            return PartialView("GemeldeteVorschaeden/Grid", GemeldeteVorschaedenViewModel);
        }

        [GridAction]
        public ActionResult GemeldeteVorschaedenAjaxBinding()
        {
            return View(new GridModel(GemeldeteVorschaedenViewModel.GemeldeteVorschaedenFiltered));
        }

        [HttpPost]
        public ActionResult EditVorschaden(string fahrgestellNr, string kennzeichem, string laufendeNr)
        {
            var model = GemeldeteVorschaedenViewModel.GetEditVorschadenModel(fahrgestellNr, kennzeichem, laufendeNr);

            return PartialView("GemeldeteVorschaeden/Partial/SetReklamationForm", model);
        }

        [HttpPost]
        public ActionResult EditVorschadenForm(EditVorschadenModel model)
        {
            if (ModelState.IsValid)
                GemeldeteVorschaedenViewModel.UpdateVorschaden(model, ModelState.AddModelError);

            return PartialView("GemeldeteVorschaeden/Partial/EditVorschadenForm", model);
        }

        [HttpPost]
        public ActionResult FilterGridGemeldeteVorschaeden(string filterValue, string filterColumns)
        {
            GemeldeteVorschaedenViewModel.FilterGemeldeteVorschaeden(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportGemeldeteVorschaedenFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = GemeldeteVorschaedenViewModel.GemeldeteVorschaedenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Remarketing_GemeldeteVorschaeden, dt);

            return new EmptyResult();
        }

        public ActionResult ExportGemeldeteVorschaedenFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = GemeldeteVorschaedenViewModel.GemeldeteVorschaedenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Remarketing_GemeldeteVorschaeden, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
