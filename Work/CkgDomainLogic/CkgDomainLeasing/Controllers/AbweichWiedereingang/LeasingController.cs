using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Models.UIModels;
using CkgDomainLogic.Leasing.ViewModels;
using DocumentTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public partial class LeasingController : CkgDomainController
    {

        public LeasingAbweichWiedereingangViewModel AbweichWiedereingangViewModel { get { return GetViewModel<LeasingAbweichWiedereingangViewModel>(); } }

        [CkgApplication]
        public ActionResult AbweichWiedereingang()
        {
            return View(AbweichWiedereingangViewModel);
        }

        [HttpPost]
        public ActionResult LoadAbweichendeWiedereingaenge(AbweichWiedereingangSelektor model)
        {
            if (ModelState.IsValid)
                AbweichWiedereingangViewModel.LoadWiedereingaenge(model, ModelState);

            return PartialView("abweichwiedereingang/Suche", model);
        }

        [HttpPost]
        public ActionResult ShowAbweichendeWiedereingaenge()
        {
            return PartialView("abweichwiedereingang/Grid", AbweichWiedereingangViewModel);
        }


        [GridAction]
        public ActionResult AbweichendeWiedereingaengejaxBinding()
        {
            return View(new GridModel(AbweichWiedereingangViewModel.WiedereingaengeFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridAbweichendeWiedereingaenge(string filterValue, string filterColumns)
        {
            AbweichWiedereingangViewModel.FilterAbweichendeWiedereingaenge(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportAbweichendeWiedereingaengeFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = AbweichWiedereingangViewModel.WiedereingaengeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.OverdueReturns, dt);

            return new EmptyResult();
        }

        public ActionResult ExportAbweichendeWiedereingaengeFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = AbweichWiedereingangViewModel.WiedereingaengeFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.OverdueReturns, dt, landscapeOrientation: true);

            return new EmptyResult();
        }




    }
}
