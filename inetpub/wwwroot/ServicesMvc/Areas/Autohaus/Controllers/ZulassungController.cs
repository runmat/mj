using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.Partner.Contracts;
using DocumentTools.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Web;
using Telerik.Web.Mvc;

namespace ServicesMvc.Autohaus.Controllers
{
    public class ZulassungController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<KroschkeZulassungViewModel>(); } }

        public KroschkeZulassungViewModel ViewModel 
        { 
            get { return GetViewModel<KroschkeZulassungViewModel>(); } 
            set { SetViewModel(value); } 
        }

        public ZulassungController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IPartnerDataService partnerDataService,
            IZulassungDataService zulassungDataService,
            IFahrzeugAkteBestandDataService fahrzeugbestandDataService)
            : base(appSettings, logonContext)
        {
            if (IsInitialRequestOf("Index"))
                ViewModel = null;

            InitViewModelExpicit(ViewModel, appSettings, logonContext, partnerDataService, zulassungDataService, fahrzeugbestandDataService);
        }

        private void InitViewModelExpicit(KroschkeZulassungViewModel vm, IAppSettings appSettings, ILogonContextDataService logonContext, IPartnerDataService partnerDataService, IZulassungDataService zulassungDataService, IFahrzeugAkteBestandDataService fahrzeugbestandDataService)
        {
            InitViewModel(vm, appSettings, logonContext, partnerDataService, zulassungDataService, fahrzeugbestandDataService);
            InitModelStatics();
        }

        [CkgApplication]
        public ActionResult Index(string fin, string halterNr, string abmeldung)
        {
            ViewModel.SetParamAbmeldung(abmeldung);

            ViewModel.DataInit();

            ViewModel.SetParamFahrzeugAkte(fin);
            ViewModel.SetParamHalter(halterNr);

            ShoppingCartLoadAndCacheItems();
            ShoppingCartTryEditItemAsViewModel();

            //DashboardService.InvokeViewModelForAppUrl("mvc/Autohaus/ZulassungsReport/Index");

            return View("Index", ViewModel);
        }

        [CkgApplication]
        public ActionResult Abmeldung(string fin, string halterNr)
        {
            return Index(fin, halterNr, abmeldung: "1");
        }

        void InitModelStatics()
        {
            CkgDomainLogic.Autohaus.Models.Zulassungsdaten.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
            CkgDomainLogic.Autohaus.Models.Fahrzeugdaten.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
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

            ViewData.Add("KundenList", ViewModel.Kunden);
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
            }

            return PartialView("Partial/FahrzeugdatenForm", model);
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
            var dt = ViewModel.HalterAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("HalterAdressen", dt);

            return new EmptyResult();
        }

        public ActionResult HalterAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.HalterAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns); 
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

            ViewData.Add("MaterialList", ViewModel.Zulassungsarten);
            return PartialView("Partial/ZulassungsdatenForm", model);
        }

        [HttpPost]
        public ActionResult LoadKfzKreisAusHalterAdresse()
        {
            string zulassungsKreis;
            string zulassungsKennzeichen;
            ViewModel.LoadKfzKreisAusHalterAdresse(out zulassungsKreis, out zulassungsKennzeichen);
            
            return Json(new { kfzKreis = zulassungsKreis });
        }

        [HttpPost]
        public ActionResult GetKennzeichenLinkeSeite(string zulassungsKreis)
        {
            string zulassungsKennzeichen;
            ViewModel.LoadKfzKennzeichenFromKreis(zulassungsKreis, out zulassungsKennzeichen);

            return Json(new { kennzeichenLinkeSeite = ViewModel.ZulassungsKennzeichenLinkeSeite(zulassungsKennzeichen) });
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
            ViewModel.Save(new List<Vorgang> { ViewModel.Zulassung }, saveDataToSap: false, saveFromShoppingCart: false);

            ShoppingCartItemSave();

            return PartialView("Partial/Receipt", ViewModel);
        }

        [HttpPost]
        public ActionResult Receipt()
        {
            ViewModel.Save(new List<Vorgang> { ViewModel.Zulassung }, saveDataToSap: true, saveFromShoppingCart: false);
            ShoppingCartItemRemove(ViewModel.ObjectKey);

            return PartialView("Partial/Receipt", ViewModel);
        }

        [HttpPost]
        public ActionResult Summary()
        {
            return PartialView("Partial/Summary", ViewModel.Zulassung.CreateSummaryModel());
        }

        public FileContentResult SummaryAsPdf(string id)
        {
            var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault(z => z.BelegNr == id);
            if (zulassung == null)
                return new FileContentResult(new byte[1], "");

            var summaryHtml = this.RenderPartialViewToString("Partial/SummaryPdf", zulassung.CreateSummaryModel());

            var summaryPdfBytes = PdfDocumentFactory.HtmlToPdf(summaryHtml);

            return new FileContentResult(summaryPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.Overview) };
        }

        public FileContentResult KundenformularAsPdf(string id)
        {
            var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault(z => z.BelegNr == id);
            if (zulassung == null)
                return new FileContentResult(new byte[1], "");

            var formularPdfBytes = zulassung.KundenformularPdf;

            return new FileContentResult(formularPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.CustomerForm) };
        }

        public FileContentResult ZusatzformularAsPdf(string id, string typ)
        {
            var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault(z => z.BelegNr == id);
            if (zulassung == null)
                return new FileContentResult(new byte[1], "");

            var zusatzFormular = zulassung.Zusatzformulare.FirstOrDefault(z => z.Typ == typ);
            if (zusatzFormular == null)
                return new FileContentResult(new byte[1], ""); 

            var auftragPdfBytes = System.IO.File.ReadAllBytes(zusatzFormular.DateiPfad);

            return new FileContentResult(auftragPdfBytes, "application/pdf") { FileDownloadName = Path.GetFileName(zusatzFormular.DateiPfad) };
        }

        public FileContentResult AuftragslisteAsPdf()
        {
            var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault();
            if (zulassung == null)
                return new FileContentResult(new byte[1], "");

            var auftragslisteFormular = zulassung.Zusatzformulare.FirstOrDefault(z => z.IstAuftragsListe);
            if (auftragslisteFormular == null)
                return new FileContentResult(new byte[1], "");
            
            var auftragPdfBytes = System.IO.File.ReadAllBytes(auftragslisteFormular.DateiPfad);

            return new FileContentResult(auftragPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.OrderList) };
        }

        /// <summary>
        /// MaihoferM Generate one PDF file with all downloadable documents
        /// </summary>
        /// <returns></returns>
        public FileContentResult AllDocumentsAsPdf(string id, string typ)
        {
            var foo = PdfDocumentFactory.HtmlToPdf("test");

            // SummaryAsPdf(string id)

            var result = SummaryAsPdf(id);


            //// Merge multiple PDF files into one single PDF 
            //// http://stackoverflow.com/questions/6029142/merging-multiple-pdfs-using-itextsharp-in-c-net
            //// step 1: creation of a document-object
            //iTextSharp document = new Document();
            //// step 2: we create a writer that listens to the document
            //PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
            //if (writer == null)
            //{
            //    return;
            //}
            //// step 3: we open the document
            //document.Open();
            //foreach (string fileName in fileNames)
            //{
            //    // we create a reader for a certain document
            //    PdfReader reader = new PdfReader(fileName);
            //    reader.ConsolidateNamedDestinations();
            //    // step 4: we add content
            //    for (int i = 1; i <= reader.NumberOfPages; i++)
            //    {
            //        PdfImportedPage page = writer.GetImportedPage(reader, i);
            //        writer.AddPage(page);
            //    }
            //    PRAcroForm form = reader.AcroForm;
            //    if (form != null)
            //    {
            //        writer.CopyAcroForm(reader);
            //    }
            //    reader.Close();
            //}
            //// step 5: we close the document and writer
            //writer.Close();
            //document.Close();


            return null;
        }

        #endregion   


        #region Shopping Cart 

        private const string ShoppingCartPersistanceKey = "KroschkeZulassung";

        protected override IEnumerable ShoppingCartLoadItems()
        {
            return ShoppingCartLoadGenericItems<KroschkeZulassungViewModel>(ShoppingCartPersistanceKey)
                    .Where(item => item.ModusAbmeldung == ViewModel.ModusAbmeldung);
        }

        protected void ShoppingCartTryEditItemAsViewModel()
        {
            var objectKey = ShoppingCartPopEditItemKey();
            if (objectKey == null)
                return;

            var vm = ShoppingCartGetItem(objectKey) as KroschkeZulassungViewModel;
            if (vm == null)
                return;

            InitViewModelExpicit(vm, AppSettings, LogonContext, ViewModel.PartnerDataService, ViewModel.ZulassungDataService, ViewModel.FahrzeugAkteBestandDataService);
            vm.DataMarkForRefresh();
            ViewModel = vm;
        }

        private void ShoppingCartItemSave()
        {
            ShoppingCartSaveItem(ShoppingCartPersistanceKey, ViewModel);
        }

        protected override void ShoppingCartFilterItems(string filterValue, string filterProperties)
        {
            ShoppingCartFilterGenericItems<KroschkeZulassungViewModel>(filterValue, filterProperties);
        }

        [HttpPost]
        public override ActionResult ShoppingCartSelectedItemsSubmit()
        {
            var warenkorb = ShoppingCartItems.Cast<KroschkeZulassungViewModel>().Where(item => item.IsSelected).ToListOrEmptyList();
            foreach (var vm in warenkorb)
            {
                InitViewModelExpicit(vm, AppSettings, LogonContext, ViewModel.PartnerDataService, ViewModel.ZulassungDataService, ViewModel.FahrzeugAkteBestandDataService);
                vm.DataMarkForRefresh();
            }

            ViewModel.Save(warenkorb.Select(wk => wk.Zulassung).ToListOrEmptyList(), saveDataToSap: true, saveFromShoppingCart: true);

            if (ViewModel.SaveErrorMessage.IsNullOrEmpty())
            {
                foreach (var vm in warenkorb)
                    ShoppingCartItemRemove(vm.ObjectKey);
            }

            return PartialView("Partial/Receipt", ViewModel);
        }

        #endregion
    }
}
