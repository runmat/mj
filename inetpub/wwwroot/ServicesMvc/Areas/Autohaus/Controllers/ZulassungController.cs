using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.Partner.Contracts;
using CkgDomainLogic.Zulassung.Models;
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
        public ActionResult Index(string finid, string halterNr, string abmeldung = "", string versandzulassung = "", string zulassungFromShoppingCart = "", string sonderzulassung = "", string schnellabmeldung = "", string showShoppingcart = "")
        {
            ViewModel.SetParamShowShoppingCart(showShoppingcart);
            ViewModel.SetParamAbmeldung(abmeldung);
            ViewModel.SetParamVersandzulassung(versandzulassung);
            ViewModel.SetParamSonderzulassung(sonderzulassung);

            ViewModel.DataInit(zulassungFromShoppingCart, schnellabmeldung);

            ViewModel.SetParamFahrzeugAkte(finid);
            ViewModel.SetParamHalter(halterNr);

            ShoppingCartLoadAndCacheItems();

            TempData["KundenauswahlWarenkorb"] = ViewModel.KundenauswahlWarenkorb;

            return View("Index", ViewModel);
        }

        #region Massenzulassung

        // ##MMA##
        [CkgApplication]
        public ActionResult IndexMultiReg()
        {
            ViewModel.SetParamAbmeldung("");
            ViewModel.SetParamVersandzulassung("");
            ViewModel.SetParamSonderzulassung("");

            ViewModel.DataInit();
            ViewModel.SetFinList(TempData["SelectedFahrzeuge"]);

            ShoppingCartLoadAndCacheItems();

            TempData["KundenauswahlWarenkorb"] = ViewModel.KundenauswahlWarenkorb;

            return View("Index", ViewModel);
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

        [GridAction]
        public ActionResult FahrzeugAuswahlAjaxBinding()
        {
            var items = ViewModel.FinListFiltered;
            return View(new GridModel(items));
        }

        [GridAction]
        public ActionResult FahrzeugAuswahlSelectedAjaxBinding()
        {
            var items = ViewModel.FinList.Where(x => !string.IsNullOrEmpty(x.FIN) || (ViewModel.Zulassung.Zulassungsdaten.IsSchnellabmeldung && x.IsSchnellabmeldungSpeicherrelevant));
            return View(new GridModel(items));
        }

        [HttpPost]
        public JsonResult SetKreisAll(string zulassungsKreis)
        {
            var result = ViewModel.SetKreisAll(zulassungsKreis);
            return Json(result == null ? new { ok = true, message = Localize.SaveSuccessful } : new { ok = false, message = string.Format("{0}: {1}", Localize.SaveFailed, result) });
        }

        [HttpPost]
        public JsonResult SetEvb(string finId, string evb)
        {
            var result = ViewModel.SetEvb(finId, evb);
            return Json(result == null ? new {ok = true, message = Localize.SaveSuccessful} : new { ok = false, message = string.Format("{0}: {1}", Localize.SaveFailed, result) });
        }

        [HttpPost]
        public JsonResult SetFinValue(string finId, string field, string value)
        {
            var result = ViewModel.SetFinValue(finId, field, value);
            return Json(result == null ? new { ok = true, message = Localize.SaveSuccessful } : new { ok = false, message = string.Format("{0}: {1}", Localize.SaveFailed, result) });
        }

        [HttpPost]
        public ActionResult FilterGridFahrzeugAuswahl(string filterValue, string filterColumns)
        {
            ViewModel.FilterFinList(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        #region Massenabmeldung

        [CkgApplication]
        public ActionResult IndexMultiCancellation()
        {
            ViewModel.SetParamAbmeldung("x");
            ViewModel.SetParamVersandzulassung("");
            ViewModel.SetParamSonderzulassung("");

            ViewModel.DataInit();
            ViewModel.SetFinList(TempData["SelectedFahrzeuge"]);

            ShoppingCartLoadAndCacheItems();

            TempData["KundenauswahlWarenkorb"] = ViewModel.KundenauswahlWarenkorb;

            return View("Index", ViewModel);
        }

        #endregion

        [CkgApplication]
        public ActionResult Abmeldung(string finid, string halterNr)
        {
            return Index(finid, halterNr, abmeldung: "1");
        }

        [CkgApplication]
        public ActionResult Shoppingcart()
        {
            return Index("", "", showShoppingcart: "1");
        }

        protected override void ShoppingCartOnShow()
        {
            ViewModel.SetParamShowShoppingCart(null);
        }

        [CkgApplication]
        public ActionResult Versandzulassung(string finid, string halterNr)
        {
            return Index(finid, halterNr, versandzulassung: "1");
        }

        [CkgApplication]
        public ActionResult Sonderzulassung(string finid, string halterNr)
        {
            return Index(finid, halterNr, sonderzulassung: "1");
        }

        [CkgApplication]
        public ActionResult Schnellabmeldung()
        {
            return Index("", "", abmeldung: "1", schnellabmeldung: "1");
        }

        void InitModelStatics()
        {
            CkgDomainLogic.Autohaus.Models.Zulassungsdaten.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
            CkgDomainLogic.Autohaus.Models.Fahrzeugdaten.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
            FahrzeugAkteBestand.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
            VersandDienstleister.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
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

                model.Bankdaten.Iban = model.Adressdaten.Adresse.Iban;

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
            return PartialView("Partial/AuslieferAdressen", ViewModel.GetAuslieferAdressenModel());
        }

        [HttpPost]
        public JsonResult AuslieferAdresseGetAutoCompleteItems()
        {
            return Json(new { items = ViewModel.GetAuslieferAdressenAsAutoCompleteItems() });
        }

        [HttpPost]
        public ActionResult AuslieferAdressenForm(AuslieferAdressen model)
        {
            var selectedPartnerRolle = Request["SelectedPartnerRolle"];
            var selectedAddressId = Request["SelectedAddressId"];

            ViewModel.SetAuslieferAdresse(model.AuslieferAdresseZ7);
            ViewModel.SetAuslieferAdresse(model.AuslieferAdresseZ8);
            ViewModel.SetAuslieferAdresse(model.AuslieferAdresseZ9);

            if (selectedPartnerRolle.IsNotNullOrEmpty() && selectedAddressId.IsNotNullOrEmpty())
            {

                var tmpAdresse = ViewModel.GetAuslieferadresse(selectedAddressId);

                if (tmpAdresse == null)
                    return new EmptyResult();

                ModelState.Clear();

                switch (selectedPartnerRolle)
                {
                    case "Z7":
                        model.AuslieferAdresseZ7.Adressdaten.Adresse = tmpAdresse;
                        ViewModel.SetAuslieferAdresse(model.AuslieferAdresseZ7);
                        break;

                    case "Z8":
                        model.AuslieferAdresseZ8.Adressdaten.Adresse = tmpAdresse;
                        ViewModel.SetAuslieferAdresse(model.AuslieferAdresseZ8);
                        break;

                    case "Z9":
                        model.AuslieferAdresseZ9.Adressdaten.Adresse = tmpAdresse;
                        ViewModel.SetAuslieferAdresse(model.AuslieferAdresseZ9);
                        break;
                }

                ViewModel.Zulassung.RefreshAuslieferAdressenMaterialAuswahl();

                return PartialView("Partial/AuslieferAdressenForm", ViewModel.GetAuslieferAdressenModel()); 
            }

            ModelState.Clear();

            ViewModel.Zulassung.RefreshAuslieferAdressenMaterialAuswahl();
            model.Materialien = AuslieferAdresse.AlleMaterialien;

            // Validierung
            model.IsValid = ViewModel.ValidateAuslieferAdressenForm(ModelState.AddModelError, model);

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

        private string GetAuslieferAdressenLink()
        {
            return this.RenderPartialViewToString("Partial/SummaryAuslieferAdressenLink");
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
            ViewModel.ValidateFahrzeugdatenForm(ModelState.AddModelError, model);

            if (ModelState.IsValid)
            {
                ViewModel.SetFahrzeugdaten(model);

                // 20150826 MMA Falls kein Kennzeichenlabel, etwaig gesetzte Werte auf null setzen...
                if (!model.HasEtikett)
                {
                    model.Farbe = null;
                    model.FzgModell = null;
                }
            }

            ViewData["IsMassenzulassung"] = ViewModel.Zulassung.Zulassungsdaten.IsMassenzulassung;
            ViewData["IsMassenabmeldung"] = ViewModel.Zulassung.Zulassungsdaten.IsMassenabmeldung;
            ViewData["ModusAbmeldung"] = ViewModel.ModusAbmeldung;
            ViewData["FahrzeugfarbenList"] = ViewModel.Fahrzeugfarben;

            return PartialView("Partial/FahrzeugdatenForm", model);
        }

        [HttpPost]
        public ActionResult VehicleAdd(int anzFahrzeuge, string fahrzeugartId)
        {
            ViewModel.AddVehicles(anzFahrzeuge, fahrzeugartId);

            return Json(new { ok = true });
        }

        [HttpPost]
        public ActionResult VehicleRemove(string finId)
        {
            ViewModel.RemoveVehicle(finId);

            return Json(new { ok = true });
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
            if (model.UiUpdateOnly)
            {
                ViewModel.UpdateZulassungsdatenModel(model);
                model.UiUpdateOnly = false;

                ModelState.Clear();
                model.IsValid = false;

                ViewData.Add("MaterialList", (ViewModel.ModusAbmeldung ? ViewModel.Abmeldearten : ViewModel.Zulassungsarten));
                return PartialView("Partial/ZulassungsdatenForm", model);
            }

            ViewModel.ValidateZulassungsdatenForm(ModelState.AddModelError, model);

            if (ModelState.IsValid)
                ViewModel.SetZulassungsdaten(model, ModelState);

            model.IsValid = ModelState.IsValid;

            ViewData.Add("MaterialList", (ViewModel.ModusAbmeldung ? ViewModel.Abmeldearten : ViewModel.Zulassungsarten));
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
        [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
        public ActionResult GetKennzeichenLinkeSeite(string zulassungsKreis)
        {
            string zulassungsKennzeichen;
            ViewModel.LoadKfzKennzeichenFromKreis(zulassungsKreis, out zulassungsKennzeichen);

            ViewModel.LoadZulassungsAbmeldeArten(zulassungsKreis);
            ViewModel.UpdateZulassungsart();

            var url = ViewModel.LoadZulassungsstelleWkzUrl(zulassungsKreis);

            // 20150502 MMA Zusätzlich Zulassungsstellen-Url für Wunschkennzeichenreservierung zurückgeben
            return Json(new
                {
                    kennzeichenLinkeSeite = ViewModel.ZulassungsKennzeichenLinkeSeite(zulassungsKennzeichen),
                    zulassungsstelleUrl = url,
                    Versandzulassung = ViewModel.Zulassung.Zulassungsdaten.Versandzulassung,
                    ExpressversandMoeglich = ViewModel.Zulassung.Zulassungsdaten.ExpressversandMoeglich,
                    ZulassungsartMatNr = ViewModel.Zulassung.Zulassungsdaten.ZulassungsartMatNr
                });
        }

        [HttpPost]
        [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
        public ActionResult UpdateZulassungsart(string haltereintragVorhanden, bool expressversand)
        {
            ViewModel.UpdateZulassungsart(haltereintragVorhanden, expressversand);

            return Json(new
            {
                ZulassungsartMatNr = ViewModel.Zulassung.Zulassungsdaten.ZulassungsartMatNr
            });
        }

        [HttpPost]
        public ActionResult CheckKennzeichenReserviert(bool kennzeichenReserviert)
        {
            ViewModel.Zulassung.Zulassungsdaten.KennzeichenReserviert = kennzeichenReserviert;

            ViewData.Add("MaterialList", (ViewModel.ModusAbmeldung ? ViewModel.Abmeldearten : ViewModel.Zulassungsarten));
            return PartialView("Partial/ZulassungsdatenForm", ViewModel.Zulassung.Zulassungsdaten);
        }

        [HttpPost]
        public ActionResult UpdateAnzahlAbmeldungen(string anzAbmeldungen)
        {
            ViewModel.UpdateAnzahlAbmeldungen(anzAbmeldungen);

            return Json(new { ok = true });
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

        #region Versanddaten

        [HttpPost]
        public ActionResult Versanddaten()
        {
            // bis auf weiteres DHL als default vorauswählen
            ViewModel.Zulassung.Versanddaten.VersandDienstleisterId = "DHL";

            return PartialView("Partial/Versanddaten", ViewModel.Zulassung.Versanddaten);
        }

        [HttpPost]
        public ActionResult VersanddatenForm(Versanddaten model)
        {
            ViewModel.ValidateVersanddatenForm(ModelState.AddModelError, model);

            if (ModelState.IsValid)
                ViewModel.SetVersanddaten(model);

            return PartialView("Partial/VersanddatenForm", ViewModel.Zulassung.Versanddaten);
        }

        #endregion

        #region Summary + Receipt

        [HttpPost]
        public ActionResult Save()
        {
            ViewModel.Save(new List<Vorgang> { ViewModel.Zulassung }, saveDataToSap: false, saveFromShoppingCart: false);

            ShoppingCartLoadAndCacheItems();

            return PartialView("Partial/Receipt", ViewModel);
        }

        [HttpPost]
        public ActionResult Receipt()
        {
            ViewModel.Save(new List<Vorgang> { ViewModel.Zulassung }, saveDataToSap: true, saveFromShoppingCart: false);

            ShoppingCartLoadAndCacheItems();

            return PartialView("Partial/Receipt", ViewModel);
        }

        [HttpPost]
        public ActionResult Summary()
        {
            ViewModel.AuslieferAdressenLink = GetAuslieferAdressenLink();

            return PartialView("Partial/Summary", ViewModel);
        }

        #region PDF-Formulare

        public FileContentResult KundenformularAsPdf(string id)
        {
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

        public FileContentResult VersandlabelAsPdf(string id)
        {
            var formularPdfBytes = VersandlabelAsPdfGetPdfBytes(id);

            return new FileContentResult(formularPdfBytes, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", Localize.ShippingLabel) };
        }

        /// <summary>
        /// 20150528 MMA PDF-Generierung ausgelagert, damit auch von AllDocumentsAsPdf nutzbar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private byte[] VersandlabelAsPdfGetPdfBytes(string id)
        {
            var zulassung = ViewModel.ZulassungenForReceipt.FirstOrDefault(z => z.BelegNr == id);
            if (zulassung == null)
                return PdfDocumentFactory.HtmlToPdf(Localize.NoDataFound);

            var formularPdfBytes = zulassung.VersandlabelPdf;

            return formularPdfBytes;
        }

        public FileContentResult ZusatzformularAsPdf(string id, string typ)
        {
            string dateiPfad;
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

        public FileContentResult AuftragslisteAsPdf()
        {
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

        /// <summary>
        /// 20150527 MMA Generate one PDF file with all downloadable documents
        /// </summary>
        /// <returns></returns>
        public FileContentResult AllDocumentsAsPdf()
        {
            var pdfsToMerge = new List<byte[]>();

            // Anhand des ViewModels ermitteln, welche Dokumente verfügbar sind (analog Buttons-Anzeige in Receipt.cshtml)
            // Sollten neue Buttons bzw. Berichte im View eingebunden werden, muss dies hier ggfl. berücksichtigt werden.

            foreach (var zulassung in ViewModel.ZulassungenForReceipt)
            {
                if (zulassung.KundenformularPdf != null)
                    pdfsToMerge.Add(KundenformularAsPdfGetPdfBytes(zulassung.BelegNr));

                foreach (var pdfFormular in zulassung.Zusatzformulare.Where(p => !p.IstAuftragsListe))
                {
                    string dateiPfad;
                    pdfsToMerge.Add(ZusatzformularAsPdfGetPdfBytes(pdfFormular.Belegnummer, pdfFormular.Typ, out dateiPfad));
                }

                if (zulassung.VersandlabelPdf != null)
                    pdfsToMerge.Add(VersandlabelAsPdfGetPdfBytes(zulassung.BelegNr));
            }

            var mergedPdf = PdfDocumentFactory.MergePdfDocuments(pdfsToMerge);

            return new FileContentResult(mergedPdf, "application/pdf") { FileDownloadName = String.Format("{0}.pdf", "Alle Zulassungsdateien") };
        }

        #endregion   

        #endregion

        #region Shopping Cart 

        protected override IEnumerable ShoppingCartLoadItems()
        {
            return ViewModel.LoadZulassungenFromShoppingCart();
        }

        protected override void ShoppingCartFilterItems(string filterValue, string filterProperties)
        {
            ShoppingCartFilterGenericItems<Vorgang>(filterValue, filterProperties);
        }

        public override void ShoppingCartItemSelect(string objectKey, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var item = ShoppingCartItems.Cast<Vorgang>().FirstOrDefault(f => f.BelegNr == objectKey);
            if (item == null)
                return;

            item.IsSelected = select;
            allSelectionCount = ShoppingCartItems.Cast<Vorgang>().Count(c => c.IsSelected);
        }

        public override void ShoppingCartItemsSelect(bool select, out int allSelectionCount, out int allCount)
        {
            ShoppingCartItems.Cast<Vorgang>().ToList().ForEach(f => f.IsSelected = select);

            allSelectionCount = ShoppingCartItems.Cast<Vorgang>().Count(c => c.IsSelected);
            allCount = ShoppingCartItems.Cast<Vorgang>().Count();
        }

        [HttpPost]
        public ActionResult ShoppingCartZulassungItemEdit(string id)
        {
            var item = ShoppingCartItems.Cast<Vorgang>().FirstOrDefault(v => v.BelegNr == id);

            if (item == null)
                return Json(new { ok = false, message = string.Format("{0}: {1}", Localize.Error, Localize.RecordNotFound) });

            ViewModel.Zulassung = item;

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult ShoppingCartZulassungItemRemove(string id)
        {
            var erg = ViewModel.DeleteShoppingCartVorgang(id);

            ShoppingCartLoadAndCacheItems();

            if (erg.IsNotNullOrEmpty())
                return Json(new { ok = false, message = string.Format("{0}: {1}", Localize.Error, erg) });

            return new EmptyResult();
        }

        [HttpPost]
        public override ActionResult ShoppingCartSelectedItemsSubmit()
        {
            var warenkorb = ShoppingCartItems.Cast<Vorgang>().Where(item => item.IsSelected).ToListOrEmptyList();

            ViewModel.LoadZulassungsAbmeldeArten(forShoppingCartSave: true);
            ViewModel.Save(warenkorb, saveDataToSap: true, saveFromShoppingCart: true);

            return PartialView("Partial/Receipt", ViewModel);
        }

        [CkgApplication]
        public ActionResult ZulassungFromShoppingCart(string id, string versandzulassung = "", string sonderzulassung = "", string schnellabmeldung = "", string abmeldung = "")
        {
            return Index("", "", zulassungFromShoppingCart: "1", versandzulassung: versandzulassung, sonderzulassung: sonderzulassung, schnellabmeldung: schnellabmeldung, abmeldung: abmeldung);
        }

        [HttpPost]
        public ActionResult WarenkorbSelectedKunnrChanged(string kunnr)
        {
            ViewModel.WarenkorbSelectedKunnr = kunnr;

            ShoppingCartLoadAndCacheItems();

            return new EmptyResult();
        }

        #endregion
    }
}
