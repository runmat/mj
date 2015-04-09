using System.Web.Mvc;
using CkgDomainLogic.Finance.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Finance.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Finance-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FinanceController  
    {
        public FinanceAktivcheckViewModel AktivcheckViewModel { get { return GetViewModel<FinanceAktivcheckViewModel>(); } }

        [CkgApplication]
        public ActionResult Aktivcheck()
        {
            AktivcheckViewModel.LoadTreffer();
            return View(AktivcheckViewModel);
        }

        [GridAction]
        public ActionResult AktivcheckTrefferAjaxBinding()
        {
            return View(new GridModel(AktivcheckViewModel.TrefferFiltered));
        }

        [HttpPost]
        public ActionResult ShowAktivcheckEdit(string vorgangId)
        {
            var vorgang = AktivcheckViewModel.GetVorgang(vorgangId);

            return PartialView("Aktivcheck/AktivcheckEdit", new AktivcheckEdit{ AktivcheckItem = vorgang, AuswahlKlassifizierung = AktivcheckViewModel.Klassifizierungen });
        }

        [HttpPost]
        public ActionResult EditAktivcheck(AktivcheckEdit model)
        {
            AktivcheckViewModel.SaveChanges(model.AktivcheckItem);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult EmailAktivcheck(string vorgangId)
        {
            if (AktivcheckViewModel.SendRequestMail(vorgangId))
            {
                return Json("OK");
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FilterGridAktivcheckTreffer(string filterValue, string filterColumns)
        {
            AktivcheckViewModel.FilterTreffer(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportAktivcheckTrefferFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = AktivcheckViewModel.TrefferFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Aktivcheck-Treffer", dt);

            return new EmptyResult();
        }

        public ActionResult ExportAktivcheckTrefferFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = AktivcheckViewModel.TrefferFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Aktivcheck-Treffer", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
