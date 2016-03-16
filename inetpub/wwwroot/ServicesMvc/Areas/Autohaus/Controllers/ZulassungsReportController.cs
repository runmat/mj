using System.Collections;
using System.IO;
using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Autohaus.Controllers
{
    public class ZulassungsReportController : CkgDomainController
    {
        public override string DataContextKey { get { return GetDataContextKey<ZulassungsReportViewModel>(); } }

        public ZulassungsReportViewModel ViewModel 
        { 
            get { return GetViewModel<ZulassungsReportViewModel>(); } 
            set { SetViewModel(value); } 
        }

        public ZulassungsReportController(IAppSettings appSettings, ILogonContextDataService logonContext,
                                          IZulassungDataService zulassungDataService)
            : base(appSettings, logonContext)
        {
            if (IsInitialRequestOf("Index"))
                ViewModel = null;

            InitViewModel(ViewModel, appSettings, logonContext, zulassungDataService);
        }

        [CkgApplication]
        public ActionResult Index(string fin, string halterNr)
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadZulassungsReport(ZulassungsReportSelektor model)
        {
            ViewModel.Selektor = model;

            ViewModel.Validate(ModelState.AddModelError);

            if (ModelState.IsValid && !PersistableMode)
            {
                ViewModel.LoadZulassungsReport(ModelState.AddModelError);
                if (ViewModel.Items.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            ViewData.Add("KundenList", ViewModel.Kunden);

            return PersistablePartialView("Partial/Suche", ViewModel.Selektor);
        }

        [HttpPost]
        public ActionResult ShowZulassungsReport()
        {
            return PartialView("Partial/Grid", ViewModel);
        }

        [GridAction]
        public ActionResult ItemsAjaxBinding()
        {
            return View(new GridModel(ViewModel.ItemsFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridZulassungen(string filterValue, string filterColumns)
        {
            ViewModel.FilterZulassungsReport(filterValue, filterColumns);

            return new EmptyResult();
        }

        public FileContentResult PdfDocumentDownload(string docName)
        {
            var docFullName = Path.Combine(KroschkeZulassungViewModel.PfadAuftragszettel, docName.SlashToBackslash().SubstringTry(1));
            var auftragPdfBytes = System.IO.File.ReadAllBytes(docFullName);

            return new FileContentResult(auftragPdfBytes, "application/pdf") { FileDownloadName = Path.GetFileName(docFullName) };
        }

        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.ItemsFiltered;
        }

        #endregion
    }
}
