using System.Collections;
using System.IO;
using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Telerik.Web.Mvc;

namespace ServicesMvc.Autohaus.Controllers
{
    public class StatusverfolgungZulassungController : CkgDomainController
    {
        public override string DataContextKey { get { return GetDataContextKey<StatusverfolgungZulassungViewModel>(); } }

        public StatusverfolgungZulassungViewModel ViewModel 
        {
            get { return GetViewModel<StatusverfolgungZulassungViewModel>(); } 
            set { SetViewModel(value); } 
        }

        public StatusverfolgungZulassungController(IAppSettings appSettings, ILogonContextDataService logonContext,
                                          IZulassungDataService zulassungDataService)
            : base(appSettings, logonContext)
        {
            if (IsInitialRequestOf("Index"))
                ViewModel = null;

            InitViewModel(ViewModel, appSettings, logonContext, zulassungDataService);

            InitModelStatics();
        }

        void InitModelStatics()
        {
            StatusverfolgungZulassungSelektor.GetViewModel = GetViewModel<StatusverfolgungZulassungViewModel>;
        }

        [CkgApplication]
        public ActionResult Index(string fin, string halterNr)
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult LoadData(StatusverfolgungZulassungSelektor model)
        {
            ViewModel.Selektor = model;

            if (ModelState.IsValid)
                ViewModel.LoadData(ModelState.AddModelError);

            return PartialView("Partial/Suche", ViewModel.Selektor);
        }

        [HttpPost]
        public ActionResult ShowData()
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
            ViewModel.FilterData(filterValue, filterColumns);

            return new EmptyResult();
        }

        public FileContentResult PdfDocumentDownload(string docName)
        {
            var docFullName = Path.Combine(KroschkeZulassungViewModel.PfadAuftragszettel, docName.SlashToBackslash().SubstringTry(1));
            var auftragPdfBytes = System.IO.File.ReadAllBytes(docFullName);

            return new FileContentResult(auftragPdfBytes, "application/pdf") { FileDownloadName = Path.GetFileName(docFullName) };
        }

        [HttpPost]
        public ActionResult ShowDetails(string belegNr)
        {
            ViewModel.LoadStatusverfolgungDetails(belegNr, ModelState.AddModelError);

            return PartialView("StatusverfolgungZulassungDetails", ViewModel.StatusverfolgungDetails);
        }

        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return ViewModel.ItemsFiltered;
        }

        #endregion
    }
}
