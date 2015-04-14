using System.Web.Mvc;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Equi.ViewModels;
using Telerik.Web.Mvc;
using DocumentTools.Services;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Equi-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class EquiController
    {
        public MahnsperreViewModel MahnsperreViewModel { get { return GetViewModel<MahnsperreViewModel>(); } }

        [CkgApplication]
        public ActionResult Mahnsperre()
        {
            return View(MahnsperreViewModel);
        }

        [HttpPost]
        public ActionResult LoadMahnsperreEquis(MahnsperreSuchparameter model)
        {
            if (ModelState.IsValid)
            {
                MahnsperreViewModel.LoadMahnsperreEquis(model, ModelState);
            }

            return PartialView("Mahnsperre/MahnsperreSuche", model);
        }

        [HttpPost]
        public ActionResult ShowMahnsperreEquis()
        {
            return PartialView("Mahnsperre/MahnsperreGrid", MahnsperreViewModel);
        }

        [GridAction]
        public ActionResult MahnsperreEquisAjaxBinding()
        {
            return View(new GridModel(MahnsperreViewModel.GridItems));
        }

        [HttpPost]
        public ActionResult MahnsperreEquisSelectAll()
        {
            return Json(MahnsperreViewModel.GetDatensatzIds(), "text/plain");
        }

        [HttpPost]
        public ActionResult BeginEditMahnsperre(string selectedItems)
        {
            MahnsperreViewModel.SelectMahnsperreEquis(selectedItems, ModelState);

            return PartialView("Mahnsperre/MahnsperreGrid", MahnsperreViewModel);
        }

        [HttpPost]
        public ActionResult DeleteMahnsperre(string selectedItems)
        {
            MahnsperreViewModel.SelectMahnsperreEquis(selectedItems, ModelState);
            MahnsperreViewModel.DeleteMahnsperre(ModelState);

            return PartialView("Mahnsperre/MahnsperreGrid", MahnsperreViewModel);
        }

        [HttpPost]
        public ActionResult MahnsperreCreate()
        {
            var model = MahnsperreViewModel.GetMahnsperreCreateModel();

            return PartialView("Mahnsperre/Partial/MahnsperreEditForm", model);
        }

        [HttpPost]
        public ActionResult MahnsperreEdit()
        {
            var model = MahnsperreViewModel.GetMahnsperreEditModel();

            return PartialView("Mahnsperre/Partial/MahnsperreEditForm", model);
        }

        [HttpPost]
        public ActionResult MahnsperreEditForm(MahnsperreEdit model)
        {
            if (ModelState.IsValid)
            {
                MahnsperreViewModel.EditOrCreateMahnsperre(model, ModelState);
            }

            return PartialView("Mahnsperre/Partial/MahnsperreEditForm", model);
        }

        [HttpPost]
        public ActionResult MahnsperreEditComplete()
        {
            return PartialView("Mahnsperre/MahnsperreGrid", MahnsperreViewModel);
        }

        [HttpPost]
        public ActionResult FilterGridMahnsperreEquis(string filterValue, string filterColumns)
        {
            MahnsperreViewModel.FilterMahnsperreEquis(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult ExportMahnsperreEquisFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = MahnsperreViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Mahnsperre", dt);

            return new EmptyResult();
        }

        public ActionResult ExportMahnsperreEquisFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = MahnsperreViewModel.GridItems.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Mahnsperre", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

    }
}
