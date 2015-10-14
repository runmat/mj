using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Archive.ViewModels;

namespace ServicesMvc.Controllers
{
    public partial class ArchiveController : CkgDomainController 
    {
        public PdfAnzeigeViewModel PdfAnzeigeViewModel { get { return GetViewModel<PdfAnzeigeViewModel>(); } }

        [CkgApplication]
        public ActionResult PdfAnzeige()
        {
            PdfAnzeigeViewModel.Init();

            return View(PdfAnzeigeViewModel);
        }

        [CkgApplication]
        public ActionResult UpsVersandlabel()
        {
            PdfAnzeigeViewModel.Init();

            return View("PdfAnzeige", PdfAnzeigeViewModel);
        }

        public ActionResult ShowPdf()
        {
            var pdfBytes = PdfAnzeigeViewModel.PdfDatei;

            return new FileContentResult(pdfBytes, "application/pdf");
        }
    }
}
