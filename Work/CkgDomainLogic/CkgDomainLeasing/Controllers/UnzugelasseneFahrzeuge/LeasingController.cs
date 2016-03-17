using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Models;
using CkgDomainLogic.Leasing.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    public partial class LeasingController  
    {
        public UnzugelasseneFahrzeugeViewModel UnzugelasseneFahrzeugeViewModel { get { return GetViewModel<UnzugelasseneFahrzeugeViewModel>(); } }

        [CkgApplication]
        public ActionResult UnzugelasseneFahrzeuge()
        {
            UnzugelasseneFahrzeugeViewModel.DataInit();

            return View(UnzugelasseneFahrzeugeViewModel);
        }

        [GridAction]
        public ActionResult UnzugelasseneFahrzeugeAjaxBinding()
        {
            return View(new GridModel(UnzugelasseneFahrzeugeViewModel.UnzugelasseneFahrzeugeFiltered));
        }

        [HttpPost]
        public ActionResult BemerkungErfassenAendern(string equiNr)
        {
            var model = UnzugelasseneFahrzeugeViewModel.GetEquiBemerkungErfassenModel(equiNr);

            return PartialView("UnzugelasseneFahrzeuge/BemerkungErfassenAendernForm", model);
        }

        [HttpPost]
        public ActionResult BemerkungErfassenAendernForm(EquiBemerkungErfassenModel model)
        {
            if (ModelState.IsValid)
                UnzugelasseneFahrzeugeViewModel.SaveBemerkung(model);

            return PartialView("UnzugelasseneFahrzeuge/BemerkungErfassenAendernForm", model);
        }

        [HttpPost]
        public ActionResult FilterGridUnzugelasseneFahrzeuge(string filterValue, string filterColumns)
        {
            UnzugelasseneFahrzeugeViewModel.FilterUnzugelasseneFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportUnzugelasseneFahrzeugeFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = UnzugelasseneFahrzeugeViewModel.UnzugelasseneFahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Leasing_UnzugelasseneFahrzeuge, dt);

            return new EmptyResult();
        }

        public ActionResult ExportUnzugelasseneFahrzeugeFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = UnzugelasseneFahrzeugeViewModel.UnzugelasseneFahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Leasing_UnzugelasseneFahrzeuge, dt, landscapeOrientation: true);

            return new EmptyResult();
        }
    }
}
