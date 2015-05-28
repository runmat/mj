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

        #region SummaryAsPdf
        public FileContentResult SummaryAsPdf(string id)
        {
            //var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault(z => z.BelegNr == id);
            //if (zulassung == null)
            //    return new FileContentResult(new byte[1], "");
            //var summaryHtml = this.RenderPartialViewToString("Partial/SummaryPdf", zulassung.CreateSummaryModel());
            //var summaryPdfBytes = PdfDocumentFactory.HtmlToPdf(summaryHtml);

            var summaryPdfBytes = SummaryAsPdfGetPdfBytes(id);

            return new FileContentResult(summaryPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.Overview) };
        }
        /// <summary>
        /// 20150528 MMA PDF-Generierung ausgelagert, damit auch von AllDocumentsAsPdf nutzbar
        /// </summary>
        /// <returns></returns>
        private byte[] SummaryAsPdfGetPdfBytes(string id)
        {
            var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault(z => z.BelegNr == id);

            if (zulassung == null)
                return PdfDocumentFactory.HtmlToPdf(Localize.NoDataFound); 

            var summaryHtml = this.RenderPartialViewToString("Partial/SummaryPdf", zulassung.CreateSummaryModel());

            var summaryPdfBytes = PdfDocumentFactory.HtmlToPdf(summaryHtml);

            return summaryPdfBytes;
        }
        #endregion

        #region KundenformularAsPdf
        public FileContentResult KundenformularAsPdf(string id)
        {
            // 20150528 MMA Folgender Block auskommentiert...
            //var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault(z => z.BelegNr == id);
            //if (zulassung == null)
            //    return new FileContentResult(new byte[1], "");
            //var formularPdfBytes = zulassung.KundenformularPdf;

            var formularPdfBytes = KundenformularAsPdfGetPdfBytes(id);

            return new FileContentResult(formularPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.CustomerForm) };
        }
        /// <summary>
        /// 20150528 MMA PDF-Generierung ausgelagert, damit auch von AllDocumentsAsPdf nutzbar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private byte[] KundenformularAsPdfGetPdfBytes(string id)
        {
            var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault(z => z.BelegNr == id);
            if (zulassung == null)
                return PdfDocumentFactory.HtmlToPdf(Localize.NoDataFound); 

            var formularPdfBytes = zulassung.KundenformularPdf;

            return formularPdfBytes;
        }
        #endregion

        #region ZusatzformularAsPdf
        public FileContentResult ZusatzformularAsPdf(string id, string typ)
        {
            // 20150528 MMA Folgender Block auskommentiert...
            //var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault(z => z.BelegNr == id);
            //if (zulassung == null)
            //    return new FileContentResult(new byte[1], "");
            //var zusatzFormular = zulassung.Zusatzformulare.FirstOrDefault(z => z.Typ == typ);
            //if (zusatzFormular == null)
            //    return new FileContentResult(new byte[1], ""); 
            //var auftragPdfBytes = System.IO.File.ReadAllBytes(zusatzFormular.DateiPfad);
            // var dateiPfad = "";
            // var zusatzformularPdfBytes = ZusatzformularAsPdfGetPdfBytes(id, typ, out dateiPfad);
            // return new FileContentResult(zusatzformularPdfBytes, "application/pdf") { FileDownloadName = Path.GetFileName(zusatzFormular.DateiPfad) };

            var dateiPfad = "";
            var zusatzformularPdfBytes = ZusatzformularAsPdfGetPdfBytes(id, typ, out dateiPfad);

            return new FileContentResult(zusatzformularPdfBytes, "application/pdf") { FileDownloadName = Path.GetFileName(dateiPfad) };
        }

        /// <summary>
        /// 20150528 MMA PDF-Generierung ausgelagert, damit auch von AllDocumentsAsPdf nutzbar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typ"></param>
        /// <param name="dateiPfad"></param>
        /// <returns></returns>
        private byte[] ZusatzformularAsPdfGetPdfBytes(string id, string typ, out string dateiPfad)
        {
            dateiPfad = "Dummy";

            var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault(z => z.BelegNr == id);
            if (zulassung == null)
                return PdfDocumentFactory.HtmlToPdf(Localize.NoDataFound); 

            var zusatzFormular = zulassung.Zusatzformulare.FirstOrDefault(z => z.Typ == typ);
            if (zusatzFormular == null)
                return PdfDocumentFactory.HtmlToPdf(Localize.NoDataFound); 

            var zusatzformularPdfBytes = System.IO.File.ReadAllBytes(zusatzFormular.DateiPfad);
            
            dateiPfad = zusatzFormular.DateiPfad;

            return zusatzformularPdfBytes;
        }
        #endregion

        #region AuftragslisteAsPdf
        public FileContentResult AuftragslisteAsPdf()
        {
            // 20150528 MMA Folgender Block auskommentiert...
            //var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault();
            //if (zulassung == null)
            //    return new FileContentResult(new byte[1], "");
            //var auftragslisteFormular = zulassung.Zusatzformulare.FirstOrDefault(z => z.IstAuftragsListe);
            //if (auftragslisteFormular == null)
            //    return new FileContentResult(new byte[1], "");
            //var auftragPdfBytes = System.IO.File.ReadAllBytes(auftragslisteFormular.DateiPfad);

            var auftragPdfBytes = AuftragslisteGetPdfBytes();

            return new FileContentResult(auftragPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.OrderList) };
        }

        /// <summary>
        /// 20150528 MMA
        /// </summary>
        /// <returns></returns>
        private byte[] AuftragslisteGetPdfBytes()
        {
            var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault();
            if (zulassung == null)
                return PdfDocumentFactory.HtmlToPdf(Localize.NoDataFound); 

            var auftragslisteFormular = zulassung.Zusatzformulare.FirstOrDefault(z => z.IstAuftragsListe);
            if (auftragslisteFormular == null)
                return PdfDocumentFactory.HtmlToPdf(Localize.NoDataFound); 

            var auftragPdfBytes = System.IO.File.ReadAllBytes(auftragslisteFormular.DateiPfad);

            return auftragPdfBytes;
        }
        #endregion

        /// <summary>
        /// 20150527 MMA Generate one PDF file with all downloadable documents
        /// </summary>
        /// <returns></returns>
        // public FileContentResult AllDocumentsAsPdf(string id, string typ)
        public FileContentResult AllDocumentsAsPdf()
        {
            var pdfsToMerge = new List<byte[]>();

            // Anhand des ViewModels ermitteln, welche Dokumente verfügbar sind (analog Buttons-Anzeige in Receipt.cshtml)
            // Sollten neue Buttons bzw. Berichte im View eingebunden werden, muss dies hier ggfl. berücksichtigt werden.

            // Falls Auftragsliste soll nicht im PDF stehen, daher deaktiviert...
            //if (ViewModel.AuftragslisteAvailable)
            //{
            //    pdfsToMerge.Add(AuftragslisteGetPdfBytes());
            //}

            foreach (var zulassung in ViewModel.ZulassungenForReceipt)
            {
                pdfsToMerge.Add(SummaryAsPdfGetPdfBytes(zulassung.BelegNr));                                                    // <a href="SummaryAsPdf?id=@zulassung.BelegNr" class="btn blue tooltips margin-right-5 margin-bottom-5" data-original-title='@Localize.YourOrderAsPdfFile' data-placement="top"> @Localize.PdfDownload <i class="icon-download-alt"></i></a>

                if (zulassung.KundenformularPdf != null)
                {
                    pdfsToMerge.Add(KundenformularAsPdfGetPdfBytes(zulassung.BelegNr));                                         // <a href="KundenformularAsPdf?id=@zulassung.BelegNr" class="btn blue tooltips margin-right-5 margin-bottom-5" data-original-title='@Localize.CustomerFormAsPdfFile' data-placement="top"> @Localize.CustomerForm <i class="icon-download-alt"></i></a>
                }

                foreach (var pdfFormular in zulassung.Zusatzformulare.Where(p => !p.IstAuftragsListe))
                {
                    var dateiPfad = "";
                    pdfsToMerge.Add(ZusatzformularAsPdfGetPdfBytes(pdfFormular.Belegnummer, pdfFormular.Typ, out dateiPfad));   // <a href="ZusatzformularAsPdf?id=@pdfFormular.Belegnummer&typ=@pdfFormular.Typ" class="btn blue tooltips margin-right-5 margin-bottom-5"> @pdfFormular.LabelForGui <i class="icon-download-alt"></i></a>
                }
            }

            var mergedPdf = PdfDocumentFactory.MergePdfDocuments(pdfsToMerge);

            return new FileContentResult(mergedPdf, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", "Alle Zulassungsdateien") };
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
