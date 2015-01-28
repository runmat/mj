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
    public class FormulareController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<FormulareViewModel>(); } }

        public FormulareViewModel ViewModel 
        { 
            get { return GetViewModel<FormulareViewModel>(); } 
            set { SetViewModel(value); } 
        }

        public FormulareController(IAppSettings appSettings, ILogonContextDataService logonContext, IZulassungDataService zulassungDataService)
            : base(appSettings, logonContext)
        {
            if (IsInitialRequestOf("Index"))
                ViewModel = null;

            InitViewModel(ViewModel, appSettings, logonContext, zulassungDataService);
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadFormulare(FormulareSelektor model)
        {
            ViewModel.Selektor = model;

            if (ModelState.IsValid)
            {
                ViewModel.LoadFormulare(ModelState.AddModelError);
                if (ViewModel.Formulare.None())
                    ModelState.AddModelError(string.Empty, Localize.NoDataFound);
            }

            ViewData.Add("ZulassungskreiseList", ViewModel.Zulassungskreise);
            return PartialView("Partial/Suche", ViewModel.Selektor);
        }

        [HttpPost]
        public ActionResult ShowFormulare()
        {
            return PartialView("Partial/Grid", ViewModel);
        }

        [GridAction]
        public ActionResult FormulareAjaxBinding()
        {
            return View(new GridModel(ViewModel.FormulareFiltered));
        }

        [HttpPost]
        public ActionResult FilterGridFormulare(string filterValue, string filterColumns)
        {
            ViewModel.FilterFormulare(filterValue, filterColumns);

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
            return ViewModel.FormulareFiltered;
        }

        #endregion    
    }
}
