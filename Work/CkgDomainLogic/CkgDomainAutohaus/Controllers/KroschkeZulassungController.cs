using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.KroschkeZulassung.Contracts;
using CkgDomainLogic.KroschkeZulassung.Models;
using CkgDomainLogic.KroschkeZulassung.ViewModels;
using CkgDomainLogic.Partner.Contracts;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    public class KroschkeZulassungController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<KroschkeZulassungViewModel>(); } }

        public KroschkeZulassungViewModel ViewModel 
        { 
            get { return GetViewModel<KroschkeZulassungViewModel>(); } 
            set { SetViewModel(value); } 
        }

        public KroschkeZulassungController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IPartnerDataService partnerDataService,
            IKroschkeZulassungDataService zulassungDataService,
            IFahrzeugAkteBestandDataService fahrzeugbestandDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, partnerDataService, zulassungDataService, fahrzeugbestandDataService);
            InitModelStatics();
        }

        [CkgApplication]
        public ActionResult Index(string fin, string halterNr)
        {
            ViewModel.DataInit();

            ViewModel.SetParamFahrzeugAkte(fin);
            
            if (halterNr.IsNotNullOrEmpty())
                ViewModel.SetParamHalter(halterNr);

            return View(ViewModel);
        }

        void InitModelStatics()
        {
            Vorgang.GetViewModel = GetViewModel<KroschkeZulassungViewModel>;
        }


        #region Rechnungsdaten

        [HttpPost]
        public ActionResult Rechnungsdaten()
        {
            return PartialView("Partial/Rechnungsdaten", ViewModel);
        }

        [HttpPost]
        public ActionResult RechnungsdatenForm(Rechnungsdaten model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.SetRechnungsdaten(model);
            }

            return PartialView("Partial/RechnungsdatenForm", model);
        }

        #endregion

        #region Bank-/Adressdaten

        [HttpPost]
        public ActionResult BankAdressdaten()
        {
            ViewModel.CheckCpd();

            return PartialView("Partial/BankAdressdaten", ViewModel);
        }

        [HttpPost]
        public ActionResult BankAdressdatenForm(BankAdressdaten model)
        {
            ViewModel.SetBankAdressdaten(ref model);

            return PartialView("Partial/BankAdressdatenForm", model);
        }

        [HttpPost]
        public ActionResult LoadBankdatenAusIban(string iban)
        {
            var bankdaten = ViewModel.LoadBankdatenAusIban(iban);

// ReSharper disable RedundantAnonymousTypePropertyName
            return Json(new { Swift = bankdaten.Swift, KontoNr = bankdaten.KontoNr, Bankleitzahl = bankdaten.Bankleitzahl, Geldinstitut = bankdaten.Geldinstitut });
// ReSharper restore RedundantAnonymousTypePropertyName
        }

        #endregion

        #region Fahrzeugdaten

        [HttpPost]
        public ActionResult Fahrzeugdaten()
        {
            return PartialView("Partial/Fahrzeugdaten", ViewModel);
        }

        [HttpPost]
        public ActionResult FahrzeugdatenForm(Fahrzeugdaten model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.SetFahrzeugdaten(model);

                Test();
            }

            return PartialView("Partial/FahrzeugdatenForm", model);
        }

        void Test()
        {
            var path = Path.Combine(AppSettings.DataPath, @"AhZulassung_01.xml");
            XmlService.XmlSerializeToFile(ViewModel, path);
            
            var savedVm = XmlService.XmlDeserializeFromFile<KroschkeZulassungViewModel>(path);
            InitViewModel(savedVm, AppSettings, LogonContext, ViewModel.PartnerDataService, ViewModel.ZulassungDataService, ViewModel.FahrzeugAkteBestandDataService);
            savedVm.DataMarkForRefresh();
        }

        #endregion

        #region HalterAdresse

        [HttpPost]
        public ActionResult HalterAdresse()
        {
            return PartialView("Partial/HalterAdresse", ViewModel);
        }

        [HttpPost]
        public JsonResult HalterAdresseGetAutoCompleteItems()
        {
            return Json(new { items = ViewModel.GetHalterAdressenAsAutoCompleteItems() });
        }

        [HttpPost]
        public ActionResult HalterAdresseForm(Adresse model)
        {
            if (model.TmpSelectionKey.IsNotNullOrEmpty())
            {
                model = ViewModel.GetHalteradresse(model.TmpSelectionKey);
                if (model == null)
                    return new EmptyResult();

                ModelState.Clear();
                model.IsValid = false;
                return PartialView("Partial/HalterAdresseForm", model);
            }

            if (ModelState.IsValid)
                ViewModel.SetHalterAdresse(model);

            model.IsValid = ModelState.IsValid;

            return PartialView("Partial/HalterAdresseForm", model);
        }

        [GridAction]
        public ActionResult HalterAdressenAjaxBinding()
        {
            var items = ViewModel.HalterAdressenFiltered;
            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterHalterAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterHalterAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult HalterAdressenShowGrid()
        {
            ViewModel.DataMarkForRefreshHalterAdressen();

            return PartialView("Partial/HalterAdressenAuswahlGrid");
        }

        public ActionResult HalterAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.HalterAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("HalterAdressen", dt);

            return new EmptyResult();
        }

        public ActionResult HalterAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.HalterAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("HalterAdressen", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
        
        #region Zulassungsdaten

        [HttpPost]
        public ActionResult Zulassungsdaten()
        {
            return PartialView("Partial/Zulassungsdaten", ViewModel);
        }

        [HttpPost]
        public ActionResult ZulassungsdatenForm(Zulassungsdaten model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.SetZulassungsdaten(model);
            }

            return PartialView("Partial/ZulassungsdatenForm", model);
        }

        [HttpPost]
        public ActionResult LoadKfzKreisAusHalterAdresse()
        {
            return Json(new { kfzKreis = ViewModel.LoadKfzKreisAusHalterAdresse() });
        }

        #endregion

        #region OptionenDienstleistungen

        [HttpPost]
        public ActionResult OptionenDienstleistungen()
        {
            ViewModel.Zulassung.OptionenDienstleistungen.InitDienstleistungen();

            return PartialView("Partial/OptionenDienstleistungen", ViewModel);
        }

        [HttpPost]
        public ActionResult OptionenDienstleistungenForm(OptionenDienstleistungen model)
        {
            if (ModelState.IsValid)
            {
                ViewModel.SetOptionenDienstleistungen(model);
            }

            return PartialView("Partial/OptionenDienstleistungenForm", ViewModel.Zulassung.OptionenDienstleistungen);
        }

        #endregion

        #region Summary + Receipt

        [HttpPost]
        public ActionResult Save()
        {
            PersistanceSaveObject("KroschkeZulassung", ViewModel);
            ViewModel.Save(false);

            return PartialView("Partial/Receipt", ViewModel);
        }

        [HttpPost]
        public ActionResult Receipt()
        {
            ViewModel.Save(true);

            return PartialView("Partial/Receipt", ViewModel);
        }

        [HttpPost]
        public ActionResult Summary()
        {
            return PartialView("Partial/Summary", ViewModel.CreateSummaryModel());
        }

        public FileContentResult SummaryAsPdf()
        {
            var summaryHtml = this.RenderPartialViewToString("Partial/SummaryPdf", ViewModel.CreateSummaryModel());

            var summaryPdfBytes = PdfDocumentFactory.HtmlToPdf(summaryHtml);

            return new FileContentResult(summaryPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.Overview) };
        }

        public FileContentResult KundenformularAsPdf()
        {
            var formularPdfBytes = ViewModel.Zulassung.KundenformularPdf;

            return new FileContentResult(formularPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.CustomerForm) };
        }

        public FileContentResult AuftragszettelAsPdf()
        {
            var auftragPdfBytes = System.IO.File.ReadAllBytes(ViewModel.Zulassung.AuftragszettelPdfPfad);

            return new FileContentResult(auftragPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.OrderForm) };
        }

        #endregion   


        #region Shopping Cart 

        protected override IEnumerable ShoppingCartGetItems()
        {
            var list = PersistanceGetObjects<KroschkeZulassungViewModel>("KroschkeZulassung");

            // <Warenkorb Test>
            var objectKey = ConfigurationManager.AppSettings["TestPersistanceObjectKey"];
            var vm = list.FirstOrDefault(o => o.ObjectKey == objectKey);
            if (vm != null)
            {
                InitViewModel(vm, AppSettings, LogonContext, ViewModel.PartnerDataService, ViewModel.ZulassungDataService, ViewModel.FahrzeugAkteBestandDataService);
                vm.DataMarkForRefresh();
                ViewModel = vm;
            }
            // </Warenkorb Test>

            return list;
        }

        protected override void ShoppingCartFilterItems(string filterValue, string filterProperties)
        {
            ShoppingCartFilterGenericItems<KroschkeZulassungViewModel>(filterValue, filterProperties);
        }

        #endregion
    }
}
