using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Models;

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
            InitModelStatics();
        }

        void InitModelStatics()
        {
            FormulareSelektor.GetViewModel = GetViewModel<FormulareViewModel>;
            ZiPoolSelektor.GetViewModel = GetViewModel<FormulareViewModel>;
        }

        [CkgApplication]
        public ActionResult Index()
        {
            ViewModel.DataInit();

            return View(ViewModel);
        }

        [HttpPost]
        [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
        public ActionResult LoadKreisByPlz(string plz)
        {
            var kreis = ViewModel.GetKreisByPlz(plz);

            return Json(new { success = (!string.IsNullOrEmpty(kreis)), kreis = kreis });
        }

        [HttpPost]
        public ActionResult LoadFormulareAndZiPoolDaten(FormulareSelektor model)
        {
            ViewModel.ApplySelection(model);

            ModelState.Clear();
            TryValidateModel(model);

            if (ModelState.IsValid)
                ViewModel.LoadFormulareAndZiPoolDaten(ModelState.AddModelError);

            return PartialView("Partial/Suche", ViewModel.FormulareSelektor);
        }

        [HttpPost]
        public ActionResult ShowFormulareAndZiPoolDaten()
        {
            return PartialView("Partial/Uebersicht", ViewModel);
        }

        public FileContentResult PdfDocumentDownload(string docName)
        {
            var docFullName = Path.Combine(KroschkeZulassungViewModel.PfadAuftragszettel, docName.SlashToBackslash().SubstringTry(1));
            var auftragPdfBytes = System.IO.File.ReadAllBytes(docFullName);

            return new FileContentResult(auftragPdfBytes, "application/pdf") { FileDownloadName = Path.GetFileName(docFullName) };
        }

        [HttpPost]
        public ActionResult RefreshZiPoolListe(bool gewerblich, string dienstleistung)
        {
            ViewModel.FilterZiPool(gewerblich, dienstleistung);

            return PartialView("Partial/Uebersicht", ViewModel);
        }
    }
}
