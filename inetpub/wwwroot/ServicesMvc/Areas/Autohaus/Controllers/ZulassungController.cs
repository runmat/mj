using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
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

        #region Massenzulassung

        // ##MMA##
        [CkgApplication]
        public ActionResult IndexMultiReg()
        {
            ViewModel.SetParamAbmeldung(null);
            ViewModel.DataInit();

            if (ViewModel.SetFinList(TempData["SelectedFahrzeuge"]) == false)
            {
                return RedirectToAction("Index");
            }

            var firstFahrzeug = ViewModel.FinList.FirstOrDefault();
            if (firstFahrzeug == null)
            {
                return Content("Kein Fahrzeug ausgewählt.");
            }
            
            ShoppingCartLoadAndCacheItems();
            ShoppingCartTryEditItemAsViewModel();

            return View("Index", ViewModel);
        }

        [HttpPost]
        public ActionResult FahrzeugShowGrid()
        {
            ViewModel.DataMarkForRefreshHalterAdressen();

            return PartialView("Partial/FahrzeugAuswahlGrid", ViewModel.FinList);
        }

        public ActionResult FahrzeugAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.FinList.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Holder, dt);

            return new EmptyResult();
        }

        public ActionResult FahrzeugAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.FinList.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Holder, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        // 20150618 MMA ITA8076 Massenzulassung
        public void SelectFahrzeuge(bool select, Predicate<FahrzeugAkteBestand> filter, out int allSelectionCount, out int allCount)
        {
            //FahrzeugeAkteBestandFiltered.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.IsSelected = select);
            //allSelectionCount = FahrzeugeAkteBestandFiltered.Count(c => c.IsSelected);
            //allCount = FahrzeugeAkteBestandFiltered.Count();

            ViewModel.FinList.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.IsSelected = select);
            allSelectionCount = ViewModel.FinList.Count(c => c.IsSelected);
            allCount = ViewModel.FinList.Count();

            // ##MMA## FinListFiltered oder FinList?!
            //ViewModel.FinListFiltered.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.IsSelected = select);
            //allSelectionCount = ViewModel.FinListFiltered.Count(c => c.IsSelected);
            //allCount = ViewModel.FinListFiltered.Count();
        }

        public void SelectFahrzeug(string vin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = ViewModel.FinList.FirstOrDefault(f => f.FIN == vin);
            if (fzg == null)
                return;
            fzg.IsSelected = select;
            allSelectionCount = ViewModel.FinList.Count(c => c.IsSelected);
        }

        [GridAction]
        public ActionResult FahrzeugAuswahlAjaxBinding()
        {
            var items = ViewModel.FinList;
            return View(new GridModel(items));
        }

        [GridAction]
        public ActionResult FahrzeugAuswahlSelectedAjaxBinding()
        {
            var items = ViewModel.FinList.Where(x => x.IsSelected);
            return View(new GridModel(items));
        }

        [HttpPost]
        public JsonResult FahrzeugAuswahlSelectionChanged(string vin, bool isChecked)
        {
            int allSelectionCount, allCount = 0;
            if (vin.IsNullOrEmpty())
                SelectFahrzeuge(isChecked, f => true, out allSelectionCount, out allCount);
            else
                SelectFahrzeug(vin, isChecked, out allSelectionCount);
            return Json(new
            {
                allSelectionCount,
                allCount,
                // zulassungenAnzahlPdiTotal = ViewModel.FinListFiltered.Count, //  11,     // ViewModel.FahrzeugeSelected,
                zulassungenAnzahlPdiTotal = ViewModel.FinList.Count(x => x.IsSelected), //  11,     // ViewModel.FahrzeugeSelected,
                zulassungenAnzahlGesamtTotal = ViewModel.FinList.Count   // ViewModel.FahrzeugeTotal,
            });
        }

        [HttpPost]
        public JsonResult SetKreisAll(string zulassungsKreis)
        {
            var result = ViewModel.SetKreisAll(zulassungsKreis);
            return Json(result == null ? new { ok = true, message = Localize.SaveSuccessful } : new { ok = false, message = string.Format("{0}: {1}", Localize.SaveFailed, result) });
        }

        [HttpPost]
        public JsonResult SetEvb(string fin, string evb)
        {
            var result = ViewModel.SetEvb(fin, evb);
            return Json(result == null ? new {ok = true, message = Localize.SaveSuccessful} : new { ok = false, message = string.Format("{0}: {1}", Localize.SaveFailed, result) });
        }

        [HttpPost]
        public JsonResult SetWunschKennz(string fin, string field, string kennz)
        {
            var result = ViewModel.SetWunschKennz(fin, field, kennz);
            return Json(result == null ? new { ok = true, message = Localize.SaveSuccessful } : new { ok = false, message = string.Format("{0}: {1}", Localize.SaveFailed, result) });
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugAuswahl(string filterValue, string filterColumns)
        {
            ViewModel.FilterFinList(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

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

        #region Halter

        [HttpPost]
        public ActionResult HalterAdresse()
        {
            return PartialView("Partial/HalterAdresse", ViewModel.Zulassung.Halter.Adresse);
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
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.Holder, dt);

            return new EmptyResult();
        }

        public ActionResult HalterAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.HalterAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, GridCurrentColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.Holder, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region Zahler Kfz-Steuer

        [HttpPost]
        public ActionResult ZahlerKfzSteuer()
        {
            return PartialView("Partial/ZahlerKfzSteuer", ViewModel.Zulassung.ZahlerKfzSteuer);
        }

        [HttpPost]
        public JsonResult ZahlerKfzSteuerAdresseGetAutoCompleteItems()
        {
            return Json(new { items = ViewModel.GetZahlerKfzSteuerAdressenAsAutoCompleteItems() });
        }

        [HttpPost]
        public ActionResult ZahlerKfzSteuerForm(BankAdressdaten model)
        {
            ViewModel.SetZahlerKfzSteuerBankdaten(model);

            if (model.Adressdaten.Adresse.TmpSelectionKey.IsNotNullOrEmpty())
            {
                model.Adressdaten.Adresse = ViewModel.GetZahlerKfzSteueradresse(model.Adressdaten.Adresse.TmpSelectionKey);
                if (model.Adressdaten.Adresse == null)
                    return new EmptyResult();

                ModelState.Clear();
                model.Adressdaten.Adresse.IsValid = false;
                return PartialView("Partial/ZahlerKfzSteuerForm", model);
            }

            if (ModelState.IsValid)
                ViewModel.SetZahlerKfzSteuerAdresse(model.Adressdaten.Adresse);

            model.Adressdaten.Adresse.IsValid = ModelState.IsValid;

            return PartialView("Partial/ZahlerKfzSteuerForm", model);
        }

        [GridAction]
        public ActionResult ZahlerKfzSteuerAdressenAjaxBinding()
        {
            var items = ViewModel.ZahlerKfzSteuerAdressenFiltered;
            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterZahlerKfzSteuerAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterZahlerKfzSteuerAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult ZahlerKfzSteuerAdressenShowGrid()
        {
            ViewModel.DataMarkForRefreshZahlerKfzSteuerAdressen();

            return PartialView("Partial/ZahlerKfzSteuerAdressenAuswahlGrid");
        }

        public ActionResult ZahlerKfzSteuerAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.ZahlerKfzSteuerAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.CarTaxPayer, dt);

            return new EmptyResult();
        }

        public ActionResult ZahlerKfzSteuerAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.ZahlerKfzSteuerAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.CarTaxPayer, dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion

        #region Bank-/Adressdaten

        [HttpPost]
        public ActionResult BankAdressdaten()
        {
            return PartialView("Partial/BankAdressdaten", ViewModel);
        }

        [HttpPost]
        public ActionResult BankAdressdatenForm(BankAdressdaten model)
        {
            ViewModel.SetBankAdressdaten(model);

            if (ViewModel.SkipBankAdressdaten)
                ModelState.Clear();

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

        #region Auslieferadressen

        [HttpPost]
        public ActionResult AuslieferAdressen()
        {
            return PartialView("Partial/AuslieferAdressen", ViewModel.SelectedAuslieferAdresse);
        }

        [HttpPost]
        public JsonResult AuslieferAdresseGetAutoCompleteItems()
        {
            return Json(new { items = ViewModel.GetAuslieferAdressenAsAutoCompleteItems() });
        }

        [HttpPost]
        public ActionResult AuslieferAdressenForm(AuslieferAdresse model)
        {
            if (model.Adressdaten.Adresse.TmpSelectionKey.IsNotNullOrEmpty())
            {
                ViewModel.SelectedAuslieferAdresse.ZugeordneteMaterialien = model.ZugeordneteMaterialien;
                ViewModel.SelectedAuslieferAdresse.Adressdaten.Bemerkung = model.Adressdaten.Bemerkung;
                ViewModel.SelectedAuslieferAdresse.Adressdaten.Adresse = ViewModel.GetAuslieferadresse(model.Adressdaten.Adresse.TmpSelectionKey);
                if (ViewModel.SelectedAuslieferAdresse.Adressdaten.Adresse == null)
                    return new EmptyResult();

                ModelState.Clear();
                ViewModel.SelectedAuslieferAdresse.IsValid = false;
                return PartialView("Partial/AuslieferAdressenForm", ViewModel.SelectedAuslieferAdresse);
            }

            if (model.TmpSelectedPartnerrolle != model.Adressdaten.Partnerrolle)
            {
                ViewModel.SelectedAuslieferAdressePartnerrolle = model.TmpSelectedPartnerrolle;
                ModelState.Clear();
                ViewModel.SelectedAuslieferAdresse.IsValid = false;
                return PartialView("Partial/AuslieferAdressenForm", ViewModel.SelectedAuslieferAdresse);
            }

            if (ModelState.IsValid)
                ViewModel.SetAuslieferAdresse(model);

            // Auslieferadressen sind optional
            if (!model.HasData)
                ModelState.Clear();

            model.IsValid = (ModelState.IsValid && !model.TmpSaveAddressOnly);
            model.Materialien = ViewModel.SelectedAuslieferAdresse.Materialien;

            ModelState.SetModelValue("TmpSaveAddressOnly", false);
            //model.TmpSaveAddressOnly = false;

            return PartialView("Partial/AuslieferAdressenForm", model);
        }

        [GridAction]
        public ActionResult AuslieferAdressenAjaxBinding()
        {
            var items = ViewModel.AuslieferAdressenFiltered;
            return View(new GridModel(items));
        }

        [HttpPost]
        public ActionResult FilterAuslieferAdressenAuswahlGrid(string filterValue, string filterColumns)
        {
            ViewModel.FilterAuslieferAdressen(filterValue, filterColumns);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AuslieferAdressenShowGrid()
        {
            ViewModel.DataMarkForRefreshAuslieferAdressen();

            return PartialView("Partial/AuslieferAdressenAuswahlGrid");
        }

        public ActionResult AuslieferAdressenAuswahlExportFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.AuslieferAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse(Localize.DeliveryAddresses, dt);

            return new EmptyResult();
        }

        public ActionResult AuslieferAdressenAuswahlExportFilteredPDF(int page, string orderBy, string filterBy)
        {
            var dt = ViewModel.AuslieferAdressenFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse(Localize.DeliveryAddresses, dt, landscapeOrientation: true);

            return new EmptyResult();
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
            //var viewModel = AdressenPflegeViewModel;
            //viewModel.ValidateModel(model, viewModel.InsertMode, ModelState.AddModelError);
            //if (ModelState.IsValid)
            //    model = viewModel.SaveItem(model, ModelState.AddModelError);
            //model.IsValid = ModelState.IsValid;
            //model.InsertModeTmp = viewModel.InsertMode;

            if (!ViewModel.FinList.Any(x => x.IsSelected))
            {
                ModelState.AddModelError(string.Empty, "Kein Fahrzeug gewählt");   // Localize.NoDataFound
            }

            if (ModelState.IsValid)
            {
                ViewModel.SetFahrzeugdaten(model);
            }

            return PartialView("Partial/FahrzeugdatenForm", model);
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
