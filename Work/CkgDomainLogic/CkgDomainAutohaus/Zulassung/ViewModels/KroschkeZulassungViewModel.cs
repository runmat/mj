using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.Partner.Contracts;
using CkgDomainLogic.Zulassung.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    [DashboardProviderViewModel]
    public class KroschkeZulassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService ZulassungDataService { get { return CacheGet<IZulassungDataService>(); } }

        [XmlIgnore, ScriptIgnore]
        public IFahrzeugAkteBestandDataService FahrzeugAkteBestandDataService { get { return CacheGet<IFahrzeugAkteBestandDataService>(); } }

        [XmlIgnore, ScriptIgnore]
        public IPartnerDataService PartnerDataService { get { return CacheGet<IPartnerDataService>(); } }

        [ScriptIgnore]
        public Vorgang Zulassung { get; set; }

        [XmlIgnore, ScriptIgnore]
        public List<Vorgang> ZulassungenForReceipt { get; set; }

        [XmlIgnore]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN { get { return Zulassung.Fahrzeugdaten.FahrgestellNr; } }

        #region Für Massenzulassung

        //[XmlIgnore]
        public List<FahrzeugAkteBestand> FinList { get; set; }
        //[XmlIgnore]
        public List<FahrzeugAkteBestand> FinListFiltered { get; set; }

        #endregion

        [XmlIgnore]
        [LocalizedDisplay(LocalizeConstants.Holder)]
        public string HalterDatenAsString { get { return Zulassung.Halter.Adresse.GetAutoSelectString(); } }

        public static string PfadAuftragszettel { get { return GeneralConfiguration.GetConfigValue("KroschkeAutohaus", "PfadAuftragszettel"); } }

        public bool ModusAbmeldung { get; set; }

        public bool ShoppingCartInitialShow { get; set; }

        public bool ModusVersandzulassung { get; set; }

        public bool ModusSonderzulassung { get; set; }

        public bool ModusPartnerportal { get; set; }

        [XmlIgnore]
        public string ApplicationTitle
        {
            get
            {
                if (Zulassung.Zulassungsdaten.IsMassenabmeldung)
                    return Localize.MassCancellation;

                if (ModusAbmeldung)
                    return Localize.Cancellation;

                if (ModusVersandzulassung)
                    return Localize.MailOrderRegistration;

                if (ModusSonderzulassung)
                    return Localize.SpecialRegistration;

                if (Zulassung.Zulassungsdaten.IsMassenzulassung)
                    return Localize.MassRegistration;

                return Localize.Registration;
            }
        }

        [XmlIgnore, ScriptIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() =>
                {
                    var xmlFileName = "StepsKroschkeZulassung.xml";

                    if (ModusAbmeldung)
                        xmlFileName = (Zulassung.Zulassungsdaten.IsSchnellabmeldung ? "StepsKroschkeSchnellabmeldung.xml" : "StepsKroschkeAbmeldung.xml");
                    else if (ModusVersandzulassung)
                        xmlFileName = "StepsKroschkeVersandzulassung.xml";

                    var dict = XmlService.XmlDeserializeFromFile<XmlDictionary<string, string>>(Path.Combine(AppSettings.DataPath, xmlFileName));

                    if (ModusPartnerportal)
                    {
                        var partnerDict = new XmlDictionary<string, string>();
                        dict.ToList().ForEach(entry =>
                        {
                            if (entry.Key == "ZahlerKfzSteuer")
                                return;

                            partnerDict.Add(entry.Key, entry.Value);
                        });

                        return partnerDict;
                    }

                    return dict;
                });
            }
        }

        [XmlIgnore, ScriptIgnore]
        public string[] StepKeys { get { return PropertyCacheGet(() => Steps.Select(s => s.Key).ToArray()); } }

        [XmlIgnore, ScriptIgnore]
        public string[] StepFriendlyNames { get { return PropertyCacheGet(() => Steps.Select(s => s.Value).ToArray()); } }

        [XmlIgnore, ScriptIgnore]
        public string FirstStepPartialViewName
        {
            get { return string.Format("{0}", StepKeys[0]); }
        }

        [XmlIgnore, ScriptIgnore]
        public string SaveErrorMessage { get; set; }

        public FahrzeugAkteBestand ParamFahrzeugAkte { get; set; }

        public void SetParamFahrzeugAkte(string finid)
        {
            ParamFahrzeugAkte = FahrzeugAkteBestandDataService.GetFahrzeuge(new FahrzeugAkteBestandSelektor { FinId = finid.NotNullOrEmpty("-") }).FirstOrDefault();
            if (ParamFahrzeugAkte == null)
                return;

            SetFahrzeugdaten(new Fahrzeugdaten
            {
                FahrgestellNr = ParamFahrzeugAkte.FIN,
                Zb2Nr = ParamFahrzeugAkte.Briefnummer,
            });
            Zulassung.Halter.Adresse = HalterAdressen
                .FirstOrDefault(a => a.KundenNr.NotNullOrEmpty().ToSapKunnr() == ParamFahrzeugAkte.Halter.NotNullOrEmpty().ToSapKunnr()) 
                ?? new Adresse { Typ = "Halter"};
        }

        public void SetParamHalter(string halterNr)
        {
            if (halterNr.IsNullOrEmpty())
                return;

            Zulassung.Halter.Adresse = HalterAdressen.FirstOrDefault(a => a.KundenNr.NotNullOrEmpty().ToSapKunnr() == halterNr.NotNullOrEmpty().ToSapKunnr());
        }

        public void SetParamAbmeldung(string abmeldung)
        {
            ModusAbmeldung = abmeldung.IsNotNullOrEmpty();
        }
        public void SetParamShowShoppingCart(string showShoppingcart)
        {
            ShoppingCartInitialShow = showShoppingcart.NotNullOrEmpty() == "1";
        }

        public void SetParamVersandzulassung(string versandzulassung)
        {
            ModusVersandzulassung = versandzulassung.IsNotNullOrEmpty();
        }

        public void SetParamSonderzulassung(string sonderzulassung)
        {
            ModusSonderzulassung = sonderzulassung.IsNotNullOrEmpty();
        }

        public void SetParamPartnerportal(string partnerportal)
        {
            ModusPartnerportal = partnerportal.IsNotNullOrEmpty();
        }

 
        #region Rechnungsdaten

        [XmlIgnore, ScriptIgnore]
        public List<Kunde> KundenAll { get { return PropertyCacheGet(() => ZulassungDataService.Kunden); } }

        [XmlIgnore, ScriptIgnore]
        public List<Kunde> Kunden
        {
            get
            {
                if (Zulassung.Zulassungsdaten.IsMassenzulassung || Zulassung.Zulassungsdaten.IsMassenabmeldung)
                    return KundenAll.Where(k => !k.Cpdkunde).ToList();

                return KundenAll;
            }
        }

        public void SetRechnungsdaten(Rechnungsdaten model)
        {
            if (Zulassung.Rechnungsdaten.KundenNr != model.KundenNr)
                Zulassung.BankAdressdaten.Bankdaten.Zahlungsart = null;

            Zulassung.Rechnungsdaten.KundenNr = model.KundenNr;

            Zulassung.BankAdressdaten.Cpdkunde = Zulassung.Rechnungsdaten.GetKunde(Kunden).Cpdkunde;
            Zulassung.BankAdressdaten.CpdMitEinzugsermaechtigung = Zulassung.Rechnungsdaten.GetKunde(Kunden).CpdMitEinzugsermaechtigung;

            if (Zulassung.BankAdressdaten.Bankdaten.Zahlungsart.IsNullOrEmpty())
                Zulassung.BankAdressdaten.Bankdaten.Zahlungsart = (Zulassung.BankAdressdaten.CpdMitEinzugsermaechtigung ? "E" : "");

            SkipBankAdressdaten = (!Zulassung.BankAdressdaten.Cpdkunde);
        }

        #endregion


        #region Massenzulassung

        /// <summary>
        /// Überträgt die Liste der anzumeldenden Fahrzeuge in das ViewModel und
        /// sorgt für Vorbelegung der relevanten Formulardaten, falls die entsprechenden 
        /// Fahrzeug-Properties identische Werte haben.
        /// Vorher muss "DataInit" aufgerufen worden sein!
        /// </summary>
        /// <param name="finList"></param>
        /// <param name="keepZulassungsart">wenn true, kein automatisches Setzen des Modus Massenzulassung/-abmeldung</param>
        public void SetFinList(object finList, bool keepZulassungsart = false)
        {
            FinList = ((List<FahrzeugAkteBestand>)finList ?? new List<FahrzeugAkteBestand>());

            #region Halterdaten evtl. vorbelegen, wenn bei allen Fahrzeugen gleich

            if (FinList.Any())
            {
                var firstFahrzeug = FinList.First();
            var isEqual = true;

                foreach (var item in FinList)
            {
                    var fahrzeugAkteBestand = item;

                    var fzgArt = Fahrzeugarten.FirstOrDefault(a => a.Beschreibung.NotNullOrEmpty().ToUpper() == fahrzeugAkteBestand.FahrzeugArt.NotNullOrEmpty().ToUpper());
                    if (fzgArt != null && !string.IsNullOrEmpty(fzgArt.Wert))
                        fahrzeugAkteBestand.ZulassungFahrzeugartId = fzgArt.Wert;
                    else
                        fahrzeugAkteBestand.ZulassungFahrzeugartId = Zulassung.Fahrzeugdaten.FahrzeugartId;

                    if (fahrzeugAkteBestand.SelectedHalter == null || firstFahrzeug.SelectedHalter == null ||
                        ModelMapping.Differences(fahrzeugAkteBestand.SelectedHalter, firstFahrzeug.SelectedHalter).Any())
                {
                    isEqual = false;
                }
            }

                if (isEqual) // Wenn Halterdaten aller Fahrzeuge identisch, soll Vorbelegung erfolgen...
                SetParamHalter(firstFahrzeug.Halter);
            }
            else
            {
                FinList.Add(new FahrzeugAkteBestand { FinID = "001", ZulassungNeuesFzg = true, ZulassungFahrzeugartId = Zulassung.Fahrzeugdaten.FahrzeugartId });
            }

            FinListFiltered = FinList;

            #endregion

            if (!keepZulassungsart)
            {
                Zulassung.Zulassungsdaten.IsMassenzulassung = !ModusAbmeldung;
                Zulassung.Zulassungsdaten.IsMassenabmeldung = ModusAbmeldung;
            }
        }

        /// <summary>
        /// Setzt alle Kennzeichen auf Standardwerte. Nur für Massenzulassung benötigt.
        /// </summary>
        /// <param name="zulassungskreis"></param>
        /// <returns></returns>
        public string SetKreisAll(string zulassungskreis)
        {
            if (!Zulassung.Zulassungsdaten.IsMassenzulassung || zulassungskreis.IsNullOrEmpty())
                return null;
 
            try
            {
                zulassungskreis += "-";
                FinList.ToList().Where(x => x.WunschKennz1.IsNullOrEmpty()).ToList().ForEach(x => x.WunschKennz1 = zulassungskreis);
                FinList.ToList().Where(x => x.WunschKennz2.IsNullOrEmpty()).ToList().ForEach(x => x.WunschKennz2 = zulassungskreis);
                FinList.ToList().Where(x => x.WunschKennz3.IsNullOrEmpty()).ToList().ForEach(x => x.WunschKennz3 = zulassungskreis);
                return null;
            }
            catch (Exception e)
            {
                return e.InnerException.ToString();
            }
        }

        /// <summary>
        /// Setzt die evb für ein einzelnes Fahrzeug oder für alle in der aktuellen FinList
        /// </summary>
        /// <param name="finId"></param>
        /// <param name="evb"></param>
        /// <returns>Null = gespeichert</returns>
        public string SetEvb(string finId, string evb)
        {
            if (!Zulassung.Zulassungsdaten.IsMassenzulassung)   // Funktion nur für Massenzulassung benötigt
                return null;

            if (!string.IsNullOrEmpty(evb) && evb.Length != 7)
                return Localize.EvbNumberLengthMustBe7;

            try
            {
                if (finId.IsNullOrEmpty())
                {
                    // evb für ALLE Fahrzeuge setzen
                    FinList.ToList().ForEach(x => x.Evb = evb);
                }
                else
                {
                    // evb nur für ein Fahrzeug setzen
                    FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.Evb = evb);
                }
                
                return null; 
            }
            catch (Exception e)
            {
                return e.InnerException.ToString(); 
            }
        }

        /// <summary>
        /// Setzt eine Variable für ein angegebenes Fahrzeug
        /// </summary>
        /// <param name="finId"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SetFinValue(string finId, string field, string value)
        {
            try
            {
                switch (field.ToLower())
                {
                    case "wunschkennz1":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.WunschKennz1 = value);
                        break;

                    case "wunschkennz2":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.WunschKennz2 = value);
                        break;

                    case "wunschkennz3":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.WunschKennz3 = value);
                        break;

                    case "kennzeichen":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.Kennzeichen = value);
                        break;

                    case "vorhandeneskennzreservieren":
                        var boolValue = Convert.ToBoolean(value);
                        FinList.Where(x => x.FinID == finId)
                               .ToList()
                               .ForEach(x => x.VorhandenesKennzReservieren = boolValue);
                        break;

                    case "fzgmodell":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.FzgModell = value);
                        break;

                    case "farbe":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.Farbe = value);
                        break;

                    case "reskennz":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.ResKennz = value);
                        break;

                    case "reservationnr":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.ReservationNr = value);
                        break;

                    case "reservationname":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.ReservationName = value);
                        break;

                    case "fin":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.FIN = value);
                        break;

                    case "halter":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.Halter = value);
                        break;

                    case "auftragsnummer":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.AuftragsNummer = value);
                        break;

                    case "bestellnr":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.BestellNr = value);
                        break;

                    case "kostenstelle":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.Kostenstelle = value);
                        break;

                    case "tuevau":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.TuevAu = value);
                        break;

                    case "briefnummer":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.Briefnummer = value);
                        break;

                    case "handelsname":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.HandelsName = value);
                        break;

                    case "zulassungfahrzeugartid":
                        FinList.Where(x => x.FinID == finId).ToList().ForEach(x => x.ZulassungFahrzeugartId = value);
                        break;
                }
                return null;
            }
            catch (Exception e)
            {
                return e.InnerException.ToString();
            }
        }

        #endregion


        #region Halter

        [XmlIgnore, ScriptIgnore]
        public List<Land> LaenderList { get { return ZulassungDataService.Laender; } }

        [XmlIgnore, ScriptIgnore]
        public List<Adresse> HalterAdressen
        {
            // ReSharper disable ConvertClosureToMethodGroup
            get { return PropertyCacheGet(() => GetHalterAdressen()); }
            // ReSharper restore ConvertClosureToMethodGroup
        }

        [XmlIgnore, ScriptIgnore]
        public List<Adresse> HalterAdressenFiltered
        {
            get { return PropertyCacheGet(() => HalterAdressen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterHalterAdressen(string filterValue, string filterProperties)
        {
            HalterAdressenFiltered = HalterAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        List<Adresse> GetHalterAdressen()
        {
            PartnerDataService.AdressenKennung = "HALTER";
            PartnerDataService.MarkForRefreshAdressen();
            var list = PartnerDataService.Adressen;
            list.ForEach(a => a.Typ = "Halter");
            return list;
        }

        public List<string> GetHalterAdressenAsAutoCompleteItems()
        {
            return HalterAdressen.Select(a => a.GetAutoSelectString()).ToList();
        }

        public Adresse GetHalteradresse(string key)
        {
            Adresse adr;

            int id;
            if (Int32.TryParse(key, out id))
                adr = HalterAdressen.FirstOrDefault(v => v.KundenNr.NotNullOrEmpty().ToSapKunnr() == key.NotNullOrEmpty().ToSapKunnr());
            else
                adr = HalterAdressen.FirstOrDefault(a => a.GetAutoSelectString() == key);

            return adr;
        }

        public void SetHalterAdresse(Adresse model)
        {
            Zulassung.Halter.Adresse = model;

            if (!ModusAbmeldung && Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.Name1.IsNullOrEmpty())
            {
                Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse = ModelMapping.Copy(Zulassung.Halter.Adresse);
                Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.Kennung = "ZAHLERKFZSTEUER";
                Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.Typ = "ZahlerKfzSteuer";
                Zulassung.ZahlerKfzSteuer.Bankdaten.Iban = Zulassung.Halter.Adresse.Iban;
            }

            if (Zulassung.BankAdressdaten.Cpdkunde)
            {
                if (Zulassung.BankAdressdaten.Adressdaten.Adresse.Name1.IsNullOrEmpty())
                    Zulassung.BankAdressdaten.Adressdaten.Adresse = ModelMapping.Copy(Zulassung.Halter.Adresse);

                if (Zulassung.BankAdressdaten.Bankdaten.Kontoinhaber.IsNullOrEmpty())
                    Zulassung.BankAdressdaten.Bankdaten.Kontoinhaber = String.Format("{0}{1}", Zulassung.Halter.Adresse.Name1, (Zulassung.Halter.Adresse.Name2.IsNullOrEmpty() ? "" : " " + Zulassung.Halter.Adresse.Name2));
            }

            string zulassungsKreis;
            
            if (Zulassung.Zulassungsdaten.Zulassungskreis.IsNullOrEmpty())
            {
                string zulassungsKennzeichen;

                LoadKfzKreisAusHalterAdresse(out zulassungsKreis, out zulassungsKennzeichen);
                Zulassung.Zulassungsdaten.Zulassungskreis = zulassungsKreis;

                if (!KennzeichenIsValid(Zulassung.Zulassungsdaten.Kennzeichen))
                    Zulassung.Zulassungsdaten.Kennzeichen = ZulassungsKennzeichenLinkeSeite(zulassungsKennzeichen);

                if (!KennzeichenIsValid(Zulassung.Zulassungsdaten.Wunschkennzeichen2))
                    Zulassung.Zulassungsdaten.Wunschkennzeichen2 = ZulassungsKennzeichenLinkeSeite(zulassungsKennzeichen);

                if (!KennzeichenIsValid(Zulassung.Zulassungsdaten.Wunschkennzeichen3))
                    Zulassung.Zulassungsdaten.Wunschkennzeichen3 = ZulassungsKennzeichenLinkeSeite(zulassungsKennzeichen);
            }
            else
            {
                zulassungsKreis = Zulassung.Zulassungsdaten.Zulassungskreis;
            }

            LoadZulassungsAbmeldeArten(zulassungsKreis);
            UpdateZulassungsart();

            // MMA Falls Massenzulassung, dann den Zulassungskreis auch für alle Wunschkennzeichen setzen
            if (Zulassung.Zulassungsdaten.IsMassenzulassung)
            {
                SetKreisAll(zulassungsKreis);

                foreach (var fahrzeugAkteBestand in FinList.Where(x => x.Evb.IsNullOrEmpty())) // 20150731 und EVB für alle Fahrzeuge setzen, sofern leer...
                {
                    fahrzeugAkteBestand.Evb = model.EvbNr;
                }
            }

            // 20150602 MMA Gegebenenfalls verfügbare externe Wunschkennzeichen-Reservierungs-Url ermitteln 
            Zulassung.Zulassungsdaten.WunschkennzeichenReservierenUrl = LoadZulassungsstelleWkzUrl(zulassungsKreis);

            if (Zulassung.Zulassungsdaten.EvbNr.IsNullOrEmpty())
                Zulassung.Zulassungsdaten.EvbNr = model.EvbNr;  // 20150617 MMA EvbNr aus Halteradresse als Vorlage holen
        }

        public string ZulassungsKennzeichenLinkeSeite(string kennzeichen)
        {
            return Zulassungsdaten.ZulassungsKennzeichenLinkeSeite(kennzeichen);
        }

        static bool KennzeichenIsValid(string kennzeichen)
        {
            return Zulassungsdaten.KennzeichenIsValid(kennzeichen);
        }

        public void DataMarkForRefreshHalterAdressen()
        {
            PropertyCacheClear(this, m => m.HalterAdressen);
            PropertyCacheClear(this, m => m.HalterAdressenFiltered);
        }

        public void LoadKfzKreisAusHalterAdresse(out string kreis, out string kennzeichen)
        {
            kreis = "";
            kennzeichen = "";
            if (Zulassung.Halter == null)
                return;

            ZulassungDataService.GetZulassungskreisUndKennzeichen(Zulassung, out kreis, out kennzeichen);
        }

        public void LoadKfzKennzeichenFromKreis(string kreis, out string kennzeichen)
        {
            ZulassungDataService.GetZulassungsKennzeichen(kreis, out kennzeichen);
        }

        /// <summary>
        /// 20150602 MMA 
        /// </summary>
        /// <param name="zulassungsKreis"></param>
        public string LoadZulassungsstelleWkzUrl(string zulassungsKreis)
        {
            return ZulassungDataService.GetZulassungsstelleWkzUrl(zulassungsKreis);
        }

        public void LoadZulassungsAbmeldeArten(string kreis = "", bool forShoppingCartSave = false)
        {
            PropertyCacheClear(this, m => m.ZulassungsAbmeldearten);

            if (Zulassung.Halter == null)
                return;

            Zulassung.Zulassungsdaten.ZulassungsartAutomatischErmitteln = (!forShoppingCartSave && !ModusSonderzulassung && !ModusVersandzulassung && !ModusAbmeldung && !Zulassung.Zulassungsdaten.IsMassenzulassung);

            var ermittelteZulassungsarten = ZulassungDataService.GetZulassungsAbmeldeArten(kreis.NotNullOrEmpty().ToUpper(), Zulassung.Zulassungsdaten.ZulassungsartAutomatischErmitteln, (ModusSonderzulassung && !forShoppingCartSave));
            ZulassungsAbmeldearten = ermittelteZulassungsarten.Where(z => z.IstVersand || !ModusVersandzulassung).ToList();

            Zulassung.Zulassungsdaten.Versandzulassung = (!ModusAbmeldung && Zulassungsarten.Any(z => z.Belegtyp == "AV"));
            Zulassung.Zulassungsdaten.ExpressversandMoeglich = (!ModusAbmeldung && Zulassungsarten.Any(z => z.Belegtyp == "AV" && !z.ZulassungAmFolgetagNichtMoeglich));

            if (!Zulassung.Zulassungsdaten.ExpressversandMoeglich && Zulassung.Zulassungsdaten.Expressversand)
                Zulassung.Zulassungsdaten.Expressversand = false;

            if (String.IsNullOrEmpty(Zulassung.Zulassungsdaten.ZulassungsartMatNr) && Zulassung.Zulassungsdaten.ModusAbmeldung)
            {
                var abmArt = Abmeldearten.FirstOrDefault(z => z.Belegtyp == "AA");
                if (abmArt != null)
                    Zulassung.Zulassungsdaten.ZulassungsartMatNr = abmArt.MaterialNr;
            }
        }

        public void UpdateZulassungsart(string haltereintragVorhanden, bool expressversand)
        {
            Zulassung.Zulassungsdaten.HaltereintragVorhanden = haltereintragVorhanden;
            Zulassung.Zulassungsdaten.Expressversand = expressversand;

            UpdateZulassungsart();
        }

        public void UpdateZulassungsart()
        {
            if (Zulassung.Zulassungsdaten.ZulassungsartAutomatischErmitteln)
            {
                Material zulArt = null;

                if (Zulassung.Zulassungsdaten.Versandzulassung)
                {
                    zulArt = Zulassungsarten.FirstOrDefault(z => z.Belegtyp == "AV" && z.ZulassungAmFolgetagNichtMoeglich != Zulassung.Zulassungsdaten.Expressversand);
                }
                else
                {
                    if (Zulassung.Zulassungsdaten.HaltereintragVorhanden == "J")
                    {
                        zulArt = Zulassungsarten.FirstOrDefault(z => z.Belegtyp == "AG");
                    }
                    else if (Zulassung.Zulassungsdaten.HaltereintragVorhanden == "N")
                    {
                        zulArt = Zulassungsarten.FirstOrDefault(z => z.Belegtyp == "AN");
                    }
                }

                if (zulArt != null)
                    Zulassung.Zulassungsdaten.ZulassungsartMatNr = zulArt.MaterialNr;
            }
        }

        #endregion


        #region Zahler Kfz-Steuer

        [XmlIgnore, ScriptIgnore]
        public List<Adresse> ZahlerKfzSteuerAdressen
        {
            // ReSharper disable ConvertClosureToMethodGroup
            get { return PropertyCacheGet(() => GetZahlerKfzSteuerAdressen()); }
            // ReSharper restore ConvertClosureToMethodGroup
        }

        [XmlIgnore, ScriptIgnore]
        public List<Adresse> ZahlerKfzSteuerAdressenFiltered
        {
            get { return PropertyCacheGet(() => ZahlerKfzSteuerAdressen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterZahlerKfzSteuerAdressen(string filterValue, string filterProperties)
        {
            ZahlerKfzSteuerAdressenFiltered = ZahlerKfzSteuerAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        List<Adresse> GetZahlerKfzSteuerAdressen()
        {
            PartnerDataService.AdressenKennung = "ZAHLERKFZSTEUER";
            PartnerDataService.MarkForRefreshAdressen();
            var list = PartnerDataService.Adressen;
            list.ForEach(a => a.Typ = "ZahlerKfzSteuer");
            return list;
        }

        public List<string> GetZahlerKfzSteuerAdressenAsAutoCompleteItems()
        {
            return ZahlerKfzSteuerAdressen.Select(a => a.GetAutoSelectString()).ToList();
        }

        public Adresse GetZahlerKfzSteueradresse(string key)
        {
            Adresse adr;

            int id;
            if (Int32.TryParse(key, out id))
                adr = ZahlerKfzSteuerAdressen.FirstOrDefault(v => v.KundenNr.NotNullOrEmpty().ToSapKunnr() == key.NotNullOrEmpty().ToSapKunnr());
            else
                adr = ZahlerKfzSteuerAdressen.FirstOrDefault(a => a.GetAutoSelectString() == key);

            return adr;
        }

        public void SetZahlerKfzSteuerAdresse(Adresse model)
        {
            Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse = model;

            // Kontoinhaber aus Adresse übernehmen
            Zulassung.ZahlerKfzSteuer.Bankdaten.Kontoinhaber = String.Format("{0}{1}", model.Name1, (model.Name2.IsNotNullOrEmpty() ? " " + model.Name2 : ""));

            // ggf. Bankdaten aus Zahler Kfz-Steuer übernehmen (muss hier passieren, da die Bank- vor den Adressdaten gespeichert werden)
            if (Zulassung.BankAdressdaten.Cpdkunde
                && Zulassung.Halter.Adresse.Name1 == Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.Name1
                && Zulassung.Halter.Adresse.Name2 == Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.Name2
                && Zulassung.Halter.Adresse.StrasseHausNr == Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.StrasseHausNr
                && Zulassung.Halter.Adresse.PLZ == Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.PLZ
                && Zulassung.Halter.Adresse.Ort == Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.Ort
                && Zulassung.Halter.Adresse.Land == Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.Land
                && Zulassung.BankAdressdaten.Bankdaten.Iban.IsNullOrEmpty())
            {
                Zulassung.BankAdressdaten.Bankdaten.KontoNr = Zulassung.ZahlerKfzSteuer.Bankdaten.KontoNr;
                Zulassung.BankAdressdaten.Bankdaten.Bankleitzahl = Zulassung.ZahlerKfzSteuer.Bankdaten.Bankleitzahl;
                Zulassung.BankAdressdaten.Bankdaten.Iban = Zulassung.ZahlerKfzSteuer.Bankdaten.Iban;
                Zulassung.BankAdressdaten.Bankdaten.Swift = Zulassung.ZahlerKfzSteuer.Bankdaten.Swift;
                Zulassung.BankAdressdaten.Bankdaten.Geldinstitut = Zulassung.ZahlerKfzSteuer.Bankdaten.Geldinstitut;
            }
            else if (!Zulassung.BankAdressdaten.Cpdkunde)
            {
                Zulassung.BankAdressdaten.Bankdaten.KontoNr = "";
                Zulassung.BankAdressdaten.Bankdaten.Bankleitzahl = "";
                Zulassung.BankAdressdaten.Bankdaten.Iban = "";
                Zulassung.BankAdressdaten.Bankdaten.Swift = "";
                Zulassung.BankAdressdaten.Bankdaten.Geldinstitut = "";
            }
        }

        public void SetZahlerKfzSteuerBankdaten(BankAdressdaten model)
        {
            Zulassung.ZahlerKfzSteuer.Bankdaten.Zahlungsart = model.Bankdaten.Zahlungsart;
            Zulassung.ZahlerKfzSteuer.Bankdaten.Iban = model.Bankdaten.Iban.NotNullOrEmpty().ToUpper();
            Zulassung.ZahlerKfzSteuer.Bankdaten.Swift = model.Bankdaten.Swift.NotNullOrEmpty().ToUpper();
            Zulassung.ZahlerKfzSteuer.Bankdaten.KontoNr = model.Bankdaten.KontoNr;
            Zulassung.ZahlerKfzSteuer.Bankdaten.Bankleitzahl = model.Bankdaten.Bankleitzahl;
            Zulassung.ZahlerKfzSteuer.Bankdaten.Geldinstitut = model.Bankdaten.Geldinstitut;
        }

        public void DataMarkForRefreshZahlerKfzSteuerAdressen()
        {
            PropertyCacheClear(this, m => m.ZahlerKfzSteuerAdressen);
            PropertyCacheClear(this, m => m.ZahlerKfzSteuerAdressenFiltered);
        }

        #endregion


        #region Bank-/Adressdaten

        public bool SkipBankAdressdaten { get; set; }

        public void SetBankAdressdaten(BankAdressdaten model)
        {
            Zulassung.BankAdressdaten.Adressdaten = model.Adressdaten;
            Zulassung.BankAdressdaten.Bankdaten.Zahlungsart = model.Bankdaten.Zahlungsart;
            Zulassung.BankAdressdaten.Bankdaten.Kontoinhaber = model.Bankdaten.Kontoinhaber;
            Zulassung.BankAdressdaten.Bankdaten.Iban = model.Bankdaten.Iban.NotNullOrEmpty().ToUpper();
            Zulassung.BankAdressdaten.Bankdaten.Swift = model.Bankdaten.Swift.NotNullOrEmpty().ToUpper();
            Zulassung.BankAdressdaten.Bankdaten.KontoNr = model.Bankdaten.KontoNr;
            Zulassung.BankAdressdaten.Bankdaten.Bankleitzahl = model.Bankdaten.Bankleitzahl;
            Zulassung.BankAdressdaten.Bankdaten.Geldinstitut = model.Bankdaten.Geldinstitut;
        }

        public Bankdaten LoadBankdatenAusIban(string iban)
        {
            return ZulassungDataService.GetBankdaten(iban.NotNullOrEmpty().ToUpper(), delegate {  });
        }

        #endregion


        #region Auslieferadressen

        [XmlIgnore, ScriptIgnore]
        public List<Adresse> AuslieferAdressen
        {
            // ReSharper disable ConvertClosureToMethodGroup
            get { return PropertyCacheGet(() => GetAuslieferAdressen()); }
            // ReSharper restore ConvertClosureToMethodGroup
        }

        [XmlIgnore, ScriptIgnore]
        public List<Adresse> AuslieferAdressenFiltered
        {
            get { return PropertyCacheGet(() => AuslieferAdressen); }
            private set { PropertyCacheSet(value); }
        }

        private string _selectedAuslieferAdressePartnerrolle;
        public string SelectedAuslieferAdressePartnerrolle
        {
            get { return _selectedAuslieferAdressePartnerrolle; }
            set
            {
                _selectedAuslieferAdressePartnerrolle = value;
                SelectedAuslieferAdresse.TmpSelectedPartnerrolle = _selectedAuslieferAdressePartnerrolle;
            }
        }

        public AuslieferAdresse SelectedAuslieferAdresse
        {
            get { return Zulassung.AuslieferAdressen.FirstOrDefault(a => a.Adressdaten.Partnerrolle == SelectedAuslieferAdressePartnerrolle); }
        }

        public string AuslieferAdressenLink { get; set; }

        public AuslieferAdressen GetAuslieferAdressenModel()
        {
            var newModel = new AuslieferAdressen
            {
                AuslieferAdresseZ7 = Zulassung.AuslieferAdressen.FirstOrDefault(x => x.Adressdaten.Partnerrolle == "Z7"),
                AuslieferAdresseZ8 = Zulassung.AuslieferAdressen.FirstOrDefault(x => x.Adressdaten.Partnerrolle == "Z8"),
                AuslieferAdresseZ9 = Zulassung.AuslieferAdressen.FirstOrDefault(x => x.Adressdaten.Partnerrolle == "Z9"),
                Materialien = AuslieferAdresse.AlleMaterialien
            };

            return newModel;
        }
        public AuslieferAdressen SetAuslieferAdressenModel(AuslieferAdressen model)
        {

            var newModel = new AuslieferAdressen
            {
                AuslieferAdresseZ7 = Zulassung.AuslieferAdressen.FirstOrDefault(x => x.Adressdaten.Partnerrolle == "Z7"),
                AuslieferAdresseZ8 = Zulassung.AuslieferAdressen.FirstOrDefault(x => x.Adressdaten.Partnerrolle == "Z8"),
                AuslieferAdresseZ9 = Zulassung.AuslieferAdressen.FirstOrDefault(x => x.Adressdaten.Partnerrolle == "Z9"),
            };

            return newModel;
        }


        public void FilterAuslieferAdressen(string filterValue, string filterProperties)
        {
            AuslieferAdressenFiltered = AuslieferAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        List<Adresse> GetAuslieferAdressen()
        {
            PartnerDataService.AdressenKennung = "HALTER";
            PartnerDataService.MarkForRefreshAdressen();
            var list = PartnerDataService.Adressen;
            list.ForEach(a => a.Typ = "Halter");

            PartnerDataService.AdressenKennung = "KAEUFER";
            PartnerDataService.MarkForRefreshAdressen();
            var listKaeufer = PartnerDataService.Adressen;
            listKaeufer.ForEach(a => a.Typ = "Kaeufer");

            list.AddRange(listKaeufer);
            return list;
        }

        public List<string> GetAuslieferAdressenAsAutoCompleteItems()
        {
            return AuslieferAdressen.Select(a => a.GetAutoSelectString()).ToList();
        }

        public Adresse GetAuslieferadresse(string key)
        {
            Adresse adr;

            int id;
            if (Int32.TryParse(key, out id))
                adr = AuslieferAdressen.FirstOrDefault(v => v.KundenNr.NotNullOrEmpty().ToSapKunnr() == key.NotNullOrEmpty().ToSapKunnr());
            else
                adr = AuslieferAdressen.FirstOrDefault(a => a.GetAutoSelectString() == key);

            return adr;
        }

        public void SetAuslieferAdresse(AuslieferAdresse model)
        {
            var item = Zulassung.AuslieferAdressen.Find(a => a.Adressdaten.Partnerrolle == model.Adressdaten.Partnerrolle);
            ModelMapping.Copy(model, item);

            Zulassung.RefreshAuslieferAdressenMaterialAuswahl();
        }

        public void DataMarkForRefreshAuslieferAdressen()
        {
            PropertyCacheClear(this, m => m.AuslieferAdressen);
            PropertyCacheClear(this, m => m.AuslieferAdressenFiltered);
        }

        #endregion


        #region Fahrzeugdaten

        [XmlIgnore, ScriptIgnore]
        public List<Domaenenfestwert> Fahrzeugarten
        {
            get { return PropertyCacheGet(() => (ZulassungDataService != null ? ZulassungDataService.Fahrzeugarten : new List<Domaenenfestwert>())); }
        }

        public void SetFahrzeugdaten(Fahrzeugdaten model)
        {
            Zulassung.Fahrzeugdaten.AuftragsNr = model.AuftragsNr;
            Zulassung.Fahrzeugdaten.FahrgestellNr = model.FahrgestellNr.NotNullOrEmpty().ToUpper();
            Zulassung.Fahrzeugdaten.Zb2Nr = model.Zb2Nr.NotNullOrEmpty().ToUpper();
            Zulassung.Fahrzeugdaten.FahrzeugartId = model.FahrzeugartId;
            Zulassung.Fahrzeugdaten.VerkaeuferKuerzel = model.VerkaeuferKuerzel;
            Zulassung.Fahrzeugdaten.Kostenstelle = model.Kostenstelle;
            Zulassung.Fahrzeugdaten.BestellNr = model.BestellNr;
            Zulassung.Fahrzeugdaten.TuevAu = model.TuevAu;

            // 20150826 MMA
            Zulassung.Fahrzeugdaten.HasEtikett = model.HasEtikett;
            if (!model.HasEtikett)
            {
                model.FzgModell = null;
                model.Farbe = null;
            }
            Zulassung.Fahrzeugdaten.FzgModell = model.FzgModell;
            Zulassung.Fahrzeugdaten.Farbe = model.Farbe;    

            if (Zulassung.Fahrzeugdaten.IstAnhaenger || Zulassung.Fahrzeugdaten.IstMotorrad)
                Zulassung.OptionenDienstleistungen.NurEinKennzeichen = true;
        }

        public void AddVehicles(int anzFahrzeuge, string fahrzeugartId)
        {
            Zulassung.Fahrzeugdaten.AnzahlHinzuzufuegendeFahrzeuge = anzFahrzeuge;

            for (var i = 0; i < anzFahrzeuge; i++)
            {
                var maxId = FinList.Max(f => f.FinID).ToInt(0);
                var kreisKz = (string.IsNullOrEmpty(Zulassung.Zulassungsdaten.Zulassungskreis) ? "" : string.Format("{0}-", Zulassung.Zulassungsdaten.Zulassungskreis));
                FinList.Add(new FahrzeugAkteBestand
                {
                    FinID = (maxId + 1).ToString("D3"),
                    ZulassungNeuesFzg = true,
                    ZulassungFahrzeugartId = fahrzeugartId,
                    WunschKennz1 = kreisKz,
                    WunschKennz2 = kreisKz,
                    WunschKennz3 = kreisKz,
                    Evb = Zulassung.Zulassungsdaten.EvbNr
                });
            }

            PropertyCacheClear(this, m => m.FinListFiltered);
        }

        public void RemoveVehicle(string finId)
        {
            FinList.RemoveAll(f => f.FinID == finId);

            PropertyCacheClear(this, m => m.FinListFiltered);
        }

        #endregion


        #region Zulassungsdaten

        [XmlIgnore, ScriptIgnore]
        public List<Material> ZulassungsAbmeldearten
        {
            get { return PropertyCacheGet(() => new List<Material>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Material> Zulassungsarten
        {
            get { return ZulassungsAbmeldearten.Where(z => !z.IstAbmeldung).ToList().CopyAndInsertAtTop(new Material { MaterialNr = "", MaterialText = Localize.DropdownDefaultOptionPleaseChoose }); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Material> Abmeldearten
        {
            get { return ZulassungsAbmeldearten.Where(z => z.IstAbmeldung).ToList().CopyAndInsertAtTop(new Material { MaterialNr = "", MaterialText = Localize.DropdownDefaultOptionPleaseChoose, IstAbmeldung = true }); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Domaenenfestwert> Fahrzeugfarben { get { return PropertyCacheGet(() => ZulassungDataService.GetFahrzeugfarben); } }

        [XmlIgnore]
        public ZiPoolDaten ZiPoolDaten
        {
            get { return PropertyCacheGet(() => new ZiPoolDaten()); }
            private set { PropertyCacheSet(value); }
        }

        public string DienstleistungsartZiPool
        {
            get
            {
                switch (Zulassung.Zulassungsdaten.Belegtyp)
                {
                    case "AU":
                        return "UMK";

                    case "AN":
                        return "ZUL";

                    case "AG":
                        return "UMS";

                    case "AV":
                    case "AK":
                    case "AF":
                        return (Zulassung.Zulassungsdaten.HaltereintragVorhanden == "J" ? "UMS" : "ZUL");

                    default:
                        return "XXX";
                }
            }
        }

        public ZiPoolDetaildaten ZiPoolDetails
        {
            get
            {
                if (ZiPoolDaten == null)
                    return new ZiPoolDetaildaten();

                return ZiPoolDaten.Details.FirstOrDefault(d => d.Gewerblich == Zulassung.HalterGewerblich && d.Dienstleistung == DienstleistungsartZiPool);
            }
        }

        public void UpdateZulassungsdatenModel(Zulassungsdaten model)
        {
            var zulDat = Zulassung.Zulassungsdaten;

            zulDat.ZulassungsartMatNr = model.ZulassungsartMatNr;
            zulDat.Zulassungsdatum = model.Zulassungsdatum;
            zulDat.Abmeldedatum = model.Abmeldedatum;
            zulDat.Zulassungskreis = model.Zulassungskreis.NotNullOrEmpty().ToUpper();
            zulDat.ZulassungskreisBezeichnung = model.ZulassungskreisBezeichnung;
            zulDat.EvbNr = model.EvbNr.NotNullOrEmpty().ToUpper();
            zulDat.Versandzulassung = model.Versandzulassung;
            zulDat.ExpressversandMoeglich = model.ExpressversandMoeglich;
            zulDat.HaltereintragVorhanden = model.HaltereintragVorhanden;
            zulDat.Expressversand = model.Expressversand;
            zulDat.VorhandenesKennzeichenReservieren = model.VorhandenesKennzeichenReservieren;
            zulDat.KennzeichenReserviert = model.KennzeichenReserviert;

            if (zulDat.KennzeichenReserviert)
            {
                zulDat.ReservierungsNr = model.ReservierungsNr;
                zulDat.ReservierungsName = model.ReservierungsName;
            }
            else
            {
                zulDat.ReservierungsNr = "";
                zulDat.ReservierungsName = "";
            }

            zulDat.Kennzeichen = model.Kennzeichen;
            zulDat.Wunschkennzeichen2 = model.Wunschkennzeichen2;
            zulDat.Wunschkennzeichen3 = model.Wunschkennzeichen3;
            zulDat.MindesthaltedauerDays = model.MindesthaltedauerDays;  // Identisch mit SAP-Feld HALTE_DAUER
        }

        public void SetZulassungsdaten(Zulassungsdaten model, ModelStateDictionary state)
        {
            UpdateZulassungsdatenModel(model);

            var zulDaten = Zulassung.Zulassungsdaten;

            Zulassung.OptionenDienstleistungen.ZulassungsartMatNr = zulDaten.ZulassungsartMatNr;

            var defaultKg = Zulassung.OptionenDienstleistungen.KennzeichengroesseListForMatNr.FirstOrDefault(k => k.Groesse == "520x114");
            if (defaultKg != null)
            {
                if (Zulassung.OptionenDienstleistungen.KennzeichenGroesseId == 0
                    || Zulassung.OptionenDienstleistungen.KennzeichengroesseListForMatNr.None(k => k.Id == Zulassung.OptionenDienstleistungen.KennzeichenGroesseId))
                {
                    Zulassung.OptionenDienstleistungen.KennzeichenGroesseId = defaultKg.Id;
                }

                Zulassung.OptionenDienstleistungen.KennzeichenSondergroesse = (Zulassung.OptionenDienstleistungen.KennzeichenGroesseId != defaultKg.Id);
            }
            
            // 20150602 MMA
            // Falls Zulassungsdatum gefüllt und firmeneigene Zulassung, dann Datumsfeld "HaltedauerBis" setzen...
            if (zulDaten.MindesthaltedauerDays != null && zulDaten.Zulassungsdatum != null && Zulassungsdaten.IstFirmeneigeneZulassung(Zulassung.OptionenDienstleistungen.ZulassungsartMatNr))
                Zulassung.OptionenDienstleistungen.HaltedauerBis = zulDaten.Zulassungsdatum.Value.AddDays((double)zulDaten.MindesthaltedauerDays);
            else
                Zulassung.OptionenDienstleistungen.HaltedauerBis = null;

            if (ModusVersandzulassung || zulDaten.Zulassungsart.Auf48hVersandPruefen)
            {
                Zulassung.VersandAdresse.Adresse = ZulassungDataService.GetLieferantZuKreis(zulDaten.Zulassungskreis);

                if (ModusVersandzulassung)
                {
                    var tmpLiefNr = Zulassung.VersandAdresse.Adresse.KundenNr.NotNullOrEmpty().TrimStart('0');
                    if (tmpLiefNr.StartsWith("564"))
                        Zulassung.VkBur = tmpLiefNr.Substring(2);
                }

                var checkErg = ZulassungDataService.Check48hExpress(Zulassung);

                if (zulDaten.Zulassungsart.ZulassungAmFolgetagNichtMoeglich && (Zulassung.Ist48hZulassung || !String.IsNullOrEmpty(checkErg)))
                    state.AddModelError("", (String.IsNullOrEmpty(checkErg) ? Localize.RegistrationDateMustBeAtLeast2DaysInTheFuture : checkErg));
                else if (!String.IsNullOrEmpty(checkErg))
                    state.AddModelError("", checkErg);
            }

            if (ModusVersandzulassung)
                zulDaten.HaltereintragVorhanden = (zulDaten.Zulassungsart.Belegtyp == "AN" ? "N" : "J");

            ZiPoolDaten = ZulassungDataService.GetZiPoolDaten(zulDaten.Zulassungskreis, state.AddModelError);
        }

        #endregion


        #region OptionenDienstleistungen

        [XmlIgnore, ScriptIgnore]
        public List<Zusatzdienstleistung> Zusatzdienstleistungen { get { return PropertyCacheGet(() => ZulassungDataService.Zusatzdienstleistungen); } }

        [XmlIgnore, ScriptIgnore]
        public List<Kennzeichengroesse> Kennzeichengroessen { get { return PropertyCacheGet(() => ZulassungDataService.Kennzeichengroessen); } }

        public void SetOptionenDienstleistungen(OptionenDienstleistungen model)
        {
            Zulassung.OptionenDienstleistungen.GewaehlteDienstleistungenString = model.GewaehlteDienstleistungenString;
            Zulassung.OptionenDienstleistungen.NurEinKennzeichen = model.NurEinKennzeichen;

            Zulassung.OptionenDienstleistungen.KennzeichenSondergroesse = model.KennzeichenSondergroesse;
            if (Zulassung.OptionenDienstleistungen.KennzeichenSondergroesse)
            {
                Zulassung.OptionenDienstleistungen.KennzeichenGroesseId = model.KennzeichenGroesseId;
            }
            else
            {
                var tempKg = Zulassung.OptionenDienstleistungen.KennzeichengroesseListForMatNr.FirstOrDefault(k => k.Groesse == "520x114");
                if (tempKg != null)
                    Zulassung.OptionenDienstleistungen.KennzeichenGroesseId = tempKg.Id;
            }

            Zulassung.OptionenDienstleistungen.Saisonkennzeichen = model.Saisonkennzeichen;
            if (Zulassung.OptionenDienstleistungen.Saisonkennzeichen)
            {
                Zulassung.OptionenDienstleistungen.SaisonBeginn = model.SaisonBeginn;
                Zulassung.OptionenDienstleistungen.SaisonEnde = model.SaisonEnde;
            }
            else
            {
                Zulassung.OptionenDienstleistungen.SaisonBeginn = "";
                Zulassung.OptionenDienstleistungen.SaisonEnde = "";
            }

            Zulassung.OptionenDienstleistungen.Bemerkung = model.Bemerkung;
            Zulassung.OptionenDienstleistungen.ZulassungsartMatNr = model.ZulassungsartMatNr;

            if (Zulassungsdaten.IstGebrauchtzulassung(Zulassung.OptionenDienstleistungen.ZulassungsartMatNr))
                Zulassung.OptionenDienstleistungen.KennzeichenVorhanden = model.KennzeichenVorhanden;
            else
                Zulassung.OptionenDienstleistungen.KennzeichenVorhanden = false;

            if (Zulassungsdaten.IstAbmeldung(Zulassung.OptionenDienstleistungen.ZulassungsartMatNr))
                Zulassung.OptionenDienstleistungen.VorhandenesKennzeichenReservieren = model.VorhandenesKennzeichenReservieren;
            else
                Zulassung.OptionenDienstleistungen.VorhandenesKennzeichenReservieren = false;

            if (Zulassungsdaten.IstFirmeneigeneZulassung(Zulassung.OptionenDienstleistungen.ZulassungsartMatNr))
                Zulassung.OptionenDienstleistungen.HaltedauerBis = model.HaltedauerBis;
            else
                Zulassung.OptionenDienstleistungen.HaltedauerBis = null;

            if (Zulassungsdaten.IstUmkennzeichnung(Zulassung.OptionenDienstleistungen.ZulassungsartMatNr))
                Zulassung.OptionenDienstleistungen.AltesKennzeichen = model.AltesKennzeichen.NotNullOrEmpty().ToUpper();
            else
                Zulassung.OptionenDienstleistungen.AltesKennzeichen = "";

            // 20150826 MMA Kennzeichenlabel
            //if (Zulassung.Fahrzeugdaten.HasEtikett)
            //    Zulassung.OptionenDienstleistungen.Kennzeichenlabel = true;
            //else
            //    Zulassung.OptionenDienstleistungen.Kennzeichenlabel = false;
            Zulassung.OptionenDienstleistungen.Kennzeichenlabel = Zulassung.Fahrzeugdaten.HasEtikett;
        }

        #endregion


        #region Versanddaten

        public void SetVersanddaten(Versanddaten model)
        {
            Zulassung.Versanddaten = model;
        }

        #endregion


        #region Misc + Summaries + Savings

        public GeneralSummary ZulassungSummary { get { return Zulassung.CreateSummaryModel(AuslieferAdressenLink); } }

        public bool SaveDataToErpSystem { get; set; }

        public bool AuftragslisteAvailable { get; set; }

        public void DataInit(string zulassungFromShoppingCart = "", string schnellAbmeldung = "")
        {
            if (zulassungFromShoppingCart.IsNullOrEmpty())
            {
                Zulassung = new Vorgang
                    {
                        VkOrg = LogonContext.Customer.AccountingArea.ToString(),
                        VkBur = LogonContext.Organization.OrganizationReference2,
                        Vorerfasser = LogonContext.UserName,
                        WebGroupId = LogonContext.Group.GroupID.ToString(),
                        WebUserId = LogonContext.UserID,
                        VorgangsStatus = "1",
                        Zulassungsdaten = new Zulassungsdaten
                            {
                                ModusAbmeldung = ModusAbmeldung,
                                ModusVersandzulassung = ModusVersandzulassung,
                                ModusSonderzulassung = ModusSonderzulassung,
                                ModusPartnerportal = ModusPartnerportal,
                                ZulassungsartMatNr = null,
                                Zulassungskreis = null,
                            },
                        Fahrzeugdaten = new Fahrzeugdaten
                            {
                                FahrzeugartId = "1",
                                AnzahlHinzuzufuegendeFahrzeuge = 1
                            }
                    };
            }
            else
            {
                ModusAbmeldung = Zulassung.Zulassungsdaten.ModusAbmeldung;
                ModusVersandzulassung = Zulassung.Zulassungsdaten.ModusVersandzulassung;
                ModusSonderzulassung = Zulassung.Zulassungsdaten.ModusSonderzulassung;
                ModusPartnerportal = Zulassung.Zulassungsdaten.ModusPartnerportal;

                var blTyp = Zulassung.Zulassungsdaten.Belegtyp;
                Zulassung.Zulassungsdaten.HaltereintragVorhanden = (blTyp == "AN" ? "N" : (blTyp == "AG" ? "J" : ""));

                Zulassung.Zulassungsdaten.Expressversand = (blTyp == "AV" && !Zulassung.Zulassungsdaten.Zulassungsart.ZulassungAmFolgetagNichtMoeglich);
            }

            if (schnellAbmeldung.IsNotNullOrEmpty())
            {
                Zulassung.Zulassungsdaten.IsSchnellabmeldung = true;
                Zulassung.Zulassungsdaten.AnzahlAbmeldungen = 1;

                if (zulassungFromShoppingCart.IsNullOrEmpty())
                {
                    Zulassung.Zulassungsdaten.AnzahlAbmeldungenAenderbar = true;
                    SetFinList(null, true);
                }
                else
                {
                    Zulassung.Zulassungsdaten.AnzahlAbmeldungenAenderbar = false;
                    SetFinList(new List<FahrzeugAkteBestand>
                    {
                        new FahrzeugAkteBestand
                        {
                            FinID = "001",
                            ZulassungNeuesFzg = true,
                            FahrzeugArt = "PKW",
                            FIN = Zulassung.FahrgestellNr,
                            Kennzeichen = Zulassung.Zulassungsdaten.Kennzeichen,
                            VorhandenesKennzReservieren = Zulassung.Zulassungsdaten.VorhandenesKennzeichenReservieren,
                            Halter = Zulassung.HalterName,
                            AuftragsNummer = Zulassung.Fahrzeugdaten.AuftragsNr,
                            BestellNr = Zulassung.Fahrzeugdaten.BestellNr,
                            Kostenstelle = Zulassung.Fahrzeugdaten.Kostenstelle,
                            TuevAu = Zulassung.Fahrzeugdaten.TuevAu,
                            Briefnummer = Zulassung.Fahrzeugdaten.Zb2Nr
                        }
                    }, true);
                }

                LoadZulassungsAbmeldeArten();
            }

            if (zulassungFromShoppingCart.IsNotNullOrEmpty())
            {
                LoadZulassungsAbmeldeArten(Zulassung.Zulassungsdaten.Zulassungskreis);

                var blTyp = Zulassung.Zulassungsdaten.Belegtyp;
                Zulassung.Zulassungsdaten.HaltereintragVorhanden = (blTyp == "AN" ? "N" : (blTyp == "AG" ? "J" : ""));
                Zulassung.Zulassungsdaten.Expressversand = (blTyp == "AV" && !Zulassung.Zulassungsdaten.Zulassungsart.ZulassungAmFolgetagNichtMoeglich);
            }

            SelectedAuslieferAdressePartnerrolle = Vorgang.AuslieferAdressenPartnerRollen.First().Key;

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.Fahrzeugarten);
            PropertyCacheClear(this, m => m.Kunden);
            PropertyCacheClear(this, m => m.Zusatzdienstleistungen);

            InitZulassung(Zulassung);

            InitKundenauswahlWarenkorb();
            
            Fahrzeugdaten.FahrzeugartList = Fahrzeugarten;
            Adresse.Laender = LaenderList;
            OptionenDienstleistungen.KennzeichengroesseList = Kennzeichengroessen;

            PartnerDataService.MarkForRefreshAdressen();

            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);
        }

        private void InitKundenauswahlWarenkorb()
        {
            WarenkorbSelectedKunnr = "";

            KundenauswahlWarenkorb = new List<Kunde>().Concat(new List<Kunde> { new Kunde("", Localize.Mine) }).ToList();

            if (!WarenkorbNurEigeneAuftraege)
            {
                KundenauswahlWarenkorb.Add(new Kunde("*", Localize.All));
                KundenauswahlWarenkorb.AddRange(KundenAll);
            }
        }

        private void InitZulassung(Vorgang zul)
        {
            zul.Kunden = Kunden;

            zul.OptionenDienstleistungen.InitDienstleistungen(Zusatzdienstleistungen);
        }

        public GeneralSummary CreateSummaryModel(string auslieferAdressenLink)
        {
            return Zulassung.CreateSummaryModel(auslieferAdressenLink);
        }

        public void Save(List<Vorgang> zulassungen, bool saveDataToSap, bool saveFromShoppingCart)
        {
            if (zulassungen.Any(z => !z.Zulassungsdaten.ModusAbmeldung) && Zulassungsarten.None())
            {
                SaveErrorMessage = Localize.NoRegistrationTypesFound;
                return;
            }

            if (zulassungen.Any(z => z.Zulassungsdaten.ModusAbmeldung) && Abmeldearten.None())
            {
                SaveErrorMessage = Localize.NoDeregistrationTypesFound;
                return;
            }

            zulassungen.ForEach(z =>
            {
                    z.Aenderer = LogonContext.UserName;
                    if (z.BeauftragungsArt.IsNullOrEmpty())
                    {
                        z.BeauftragungsArt = (ModusVersandzulassung ? (ModusPartnerportal ? "VERSANDZULASSUNGPARTNER" : "VERSANDZULASSUNG")
                                              : z.Zulassungsdaten.IsMassenzulassung ? "MASSENZULASSUNG"
                                              : z.Zulassungsdaten.IsMassenabmeldung ? "MASSENABMELDUNG"
                                              : z.Zulassungsdaten.IsSchnellabmeldung ? "SCHNELLABMELDUNG"
                                              : ModusAbmeldung ? "ABMELDUNG"
                                              : ModusSonderzulassung ? "SONDERZULASSUNG"
                                              : "ZULASSUNG");
                    }
                });

            var zulassungenToSave = new List<Vorgang>();
           
            if (Zulassung.Zulassungsdaten.IsMassenzulassung || Zulassung.Zulassungsdaten.IsMassenabmeldung || Zulassung.Zulassungsdaten.IsSchnellabmeldung)
            {
                // Alle zuzulassenden Fahrzeuge durchlaufen
                foreach (var fahrzeugAkteBestand in FinListFiltered.Where(x => !string.IsNullOrEmpty(x.FIN) || (Zulassung.Zulassungsdaten.IsSchnellabmeldung && x.IsSchnellabmeldungSpeicherrelevant)))
                {
                    if (Zulassung.Zulassungsdaten.IsSchnellabmeldung && !fahrzeugAkteBestand.IsSchnellabmeldungSpeicherrelevant)
                        continue;

                    var singleZulassung = ModelMapping.Copy(Zulassung);     // Achtung: Kopiert nicht zuverlässig, sondern legt eine Referenz von Zulassung.Zulassungsdaten an
                    singleZulassung.Zulassungsdaten = ModelMapping.Copy(Zulassung.Zulassungsdaten); // Explizit Zulassungsdaten kopieren, damit keine Referenz erzeugt wird
                    singleZulassung.Fahrzeugdaten = ModelMapping.Copy(Zulassung.Fahrzeugdaten);     // Explizit Fahrzeugdaten kopieren, damit keine Referenz erzeugt wird
                    
                    singleZulassung.ZahlerKfzSteuer = ModelMapping.Copy(Zulassung.ZahlerKfzSteuer);
                    singleZulassung.VersandAdresse = ModelMapping.Copy(Zulassung.VersandAdresse);

                    // singleZulassung.AuslieferAdressen    = new List<AuslieferAdresse>();            // ModelMapping.Copy(Zulassung.AuslieferAdressen) gibt Fehlermeldung "Parameteranzahlkonflikt", daher nicht verwendet

                    singleZulassung.AuslieferAdressen = Zulassung.AuslieferAdressen;

                    singleZulassung.Halter = ModelMapping.Copy(Zulassung.Halter);
                    singleZulassung.BankAdressdaten = ModelMapping.Copy(Zulassung.BankAdressdaten);

                    // singleZulassung.Zusatzformulare = ModelMapping.Copy(Zulassung.Zusatzformulare);  // Fehlermeldung "Parameteranzahlkonflikt", daher nicht verwendet
                    singleZulassung.Zusatzformulare = new List<PdfFormular>();

                    singleZulassung.Fahrzeugdaten.FahrgestellNr = fahrzeugAkteBestand.FIN;
                    singleZulassung.Zulassungsdaten.VorhandenesKennzeichenReservieren = fahrzeugAkteBestand.VorhandenesKennzReservieren;

                    if (Zulassung.Zulassungsdaten.IsSchnellabmeldung)
                    {
                        singleZulassung.Zulassungsdaten.Kennzeichen = fahrzeugAkteBestand.Kennzeichen;
                        singleZulassung.Zulassungsdaten.HalterNameSchnellabmeldung = fahrzeugAkteBestand.Halter;
                        singleZulassung.Fahrzeugdaten.TuevAu = fahrzeugAkteBestand.TuevAu;
                        singleZulassung.Fahrzeugdaten.Zb2Nr = fahrzeugAkteBestand.Briefnummer;
                    }
                    else
                    {
                        singleZulassung.Zulassungsdaten.EvbNr = fahrzeugAkteBestand.Evb;
                        singleZulassung.Zulassungsdaten.Kennzeichen = fahrzeugAkteBestand.WunschKennz1;
                        singleZulassung.Zulassungsdaten.Wunschkennzeichen2 = fahrzeugAkteBestand.WunschKennz2;
                        singleZulassung.Zulassungsdaten.Wunschkennzeichen3 = fahrzeugAkteBestand.WunschKennz3;

                        if (!fahrzeugAkteBestand.ResKennz.IsNullOrEmpty() ||
                            !fahrzeugAkteBestand.ReservationNr.IsNullOrEmpty() ||
                            !fahrzeugAkteBestand.ReservationName.IsNullOrEmpty())
                        {
                            singleZulassung.Zulassungsdaten.Kennzeichen = fahrzeugAkteBestand.ResKennz;
                            singleZulassung.Zulassungsdaten.ReservierungsNr = fahrzeugAkteBestand.ReservationNr;
                            singleZulassung.Zulassungsdaten.ReservierungsName = fahrzeugAkteBestand.ReservationName;
                            singleZulassung.Zulassungsdaten.KennzeichenReserviert = true;
                        }

                        // 20150826 MMA                    
                        singleZulassung.Fahrzeugdaten.Farbe = fahrzeugAkteBestand.Farbe;
                        singleZulassung.Fahrzeugdaten.FzgModell = fahrzeugAkteBestand.FzgModell;
                        singleZulassung.Fahrzeugdaten.FahrzeugartId = fahrzeugAkteBestand.ZulassungFahrzeugartId;
                    }

                    zulassungenToSave.Add(singleZulassung);
                }
            }
            else
            {
                zulassungenToSave = zulassungen;
            }
            
            if (saveDataToSap)
            {
                var zulOhneEvb = zulassungenToSave.Where(z => !z.Zulassungsdaten.ModusAbmeldung && string.IsNullOrEmpty(z.Zulassungsdaten.EvbNr));
                if (zulOhneEvb.Any())
                {
                    SaveErrorMessage = string.Join(", ", zulOhneEvb.Select(z => string.Format("{0}: {1}", z.FahrgestellNr, Localize.EvbNumberRequired)));
                    return;
                }
            }
            
            SaveDataToErpSystem = saveDataToSap;
            AuftragslisteAvailable = saveDataToSap;

            ZulassungenForReceipt = new List<Vorgang>();
            
            SaveErrorMessage = ZulassungDataService.SaveZulassungen(zulassungenToSave, saveDataToSap, saveFromShoppingCart, ModusPartnerportal);

            if (SaveErrorMessage.IsNullOrEmpty())
            {
                ZulassungenForReceipt = zulassungenToSave.Select(zulassung => ModelMapping.Copy(zulassung)).ToListOrEmptyList();

                if (ZulassungenForReceipt.ToListOrEmptyList().None() || ZulassungenForReceipt.First().Zusatzformulare.ToListOrEmptyList().None(z => z.IstAuftragsListe))
                    AuftragslisteAvailable = false;
            }
        }

        #endregion


        #region Shopping Cart

        public bool WarenkorbNurEigeneAuftraege { get { return ZulassungDataService.WarenkorbNurEigeneAuftraege; } }

        [LocalizedDisplay(LocalizeConstants.ShowOrders)]
        public string WarenkorbSelectedKunnr { get; set; }

        [XmlIgnore, ScriptIgnore]
        public List<Kunde> KundenauswahlWarenkorb { get; private set; }

        public IEnumerable<Vorgang> LoadZulassungenFromShoppingCart()
        {
            var kundenNummern = new List<string>();

            if (!String.IsNullOrEmpty(WarenkorbSelectedKunnr))
            {
                if (WarenkorbSelectedKunnr == "*")
                    KundenauswahlWarenkorb.ForEach(k => kundenNummern.Add(k.KundenNr));
                else
                    kundenNummern.Add(WarenkorbSelectedKunnr);
            }

            var liste = ZulassungDataService.LoadVorgaengeForShoppingCart(kundenNummern);

            liste.ForEach(InitZulassung);

            return liste;
        }

        public string DeleteShoppingCartVorgang(string belegNr)
        {
            return ZulassungDataService.DeleteVorgangFromShoppingCart(belegNr);
        }

        #endregion

        public void FilterFinList(string filterValue, string filterProperties)
        {
            FinListFiltered = FinList.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void ValidateFahrzeugdatenForm(Action<string, string> addModelError, Fahrzeugdaten fahrzeugdatenModel)
        {
            if ((Zulassung.Zulassungsdaten.IsMassenzulassung && FinList.None(x => !string.IsNullOrEmpty(x.FIN))) && (Zulassung.Zulassungsdaten.IsMassenabmeldung && FinList.None(x => x.IsMassenabmeldungSpeicherrelevant)))
                addModelError(string.Empty, Localize.NoVehicleSelected);

            if (fahrzeugdatenModel.HasEtikett)
            {
                if (Zulassung.Zulassungsdaten.IsMassenzulassung)
                {
                    if (FinList.Any(x => !string.IsNullOrEmpty(x.FIN) && x.Farbe.IsNullOrEmpty()))
                        addModelError(string.Empty, string.Format("{0} {1}", Localize.Color, Localize.Required.ToLower()));

                    if (FinList.Any(x => !string.IsNullOrEmpty(x.FIN) && x.FzgModell.IsNullOrEmpty()))
                        addModelError(string.Empty, string.Format("{0} {1}", Localize.CarModel, Localize.Required.ToLower())); 
                }
                else
                {
                    if (fahrzeugdatenModel.Farbe.IsNullOrEmpty())
                        addModelError("Farbe", string.Format("{0} {1}", Localize.Color, Localize.Required.ToLower()));

                    if (fahrzeugdatenModel.FzgModell.IsNullOrEmpty())
                        addModelError("FzgModell", string.Format("{0} {1}", Localize.CarModel, Localize.Required.ToLower()));
                }
            }

            if (ModusAbmeldung && !Zulassung.Zulassungsdaten.IsMassenabmeldung && !Zulassung.Zulassungsdaten.IsSchnellabmeldung)
            {
                var regexTuevAu = new Regex("^(0[1-9]|1[0-2])[0-9]{2}$");

                if (fahrzeugdatenModel.TuevAu.IsNullOrEmpty())
                    addModelError("TuevAu", string.Format("{0} {1} ({2}: {3})", Localize.TuevAu, Localize.Required.ToLower(), Localize.Format, Localize.DateFormat_MMJJ));
                else if (!regexTuevAu.IsMatch(fahrzeugdatenModel.TuevAu))
                    addModelError(string.Empty, string.Format("{0} {1} ({2}: {3})", Localize.TuevAu, Localize.Invalid.NotNullOrEmpty().ToLower(), Localize.Format, Localize.DateFormat_MMJJ));
            }
        }

        public void ValidateZulassungsdatenForm(Action<string, string> addModelError, Zulassungsdaten zulassungsdatenModel)
        {
            if (Zulassung.Zulassungsdaten.IsMassenzulassung)
            {
                var zulkreis = string.Format("{0}{1}", zulassungsdatenModel.Zulassungskreis, "-");

                var tmpFinList = FinList.Where(x => !string.IsNullOrEmpty(x.FIN)
                                                    && ((!x.WunschKennz1.IsNullOrEmpty() && x.WunschKennz1 != zulkreis) || (!x.WunschKennz2.IsNullOrEmpty() && x.WunschKennz2 != zulkreis) || (!x.WunschKennz3.IsNullOrEmpty() && x.WunschKennz3 != zulkreis))
                                                    && (!x.ResKennz.IsNullOrEmpty() || !x.ReservationNr.IsNullOrEmpty() || !x.ReservationName.IsNullOrEmpty()));
     
                if (tmpFinList.Any())
                {
                    addModelError(string.Empty, Localize.PleaseEnterOnlyPersonalisedLicenseOrReservationInformation);
                }
                else
                {
                    foreach (var item in tmpFinList)
                    {
                        if (!item.ResKennz.IsNullOrEmpty() || !item.ReservationNr.IsNullOrEmpty() || !item.ReservationName.IsNullOrEmpty())
                        {
                            item.WunschKennz1 = null;
                            item.WunschKennz2 = null;
                            item.WunschKennz3 = null;
                        }
                    }
                }
            }
            else if (Zulassung.Zulassungsdaten.IsMassenabmeldung)
            {
                if (FinList.Any(x => x.IsMassenabmeldungSpeicherrelevant && (x.Kennzeichen.IsNullOrEmpty() || x.Kennzeichen.EndsWith("-"))))
                    addModelError(string.Empty, string.Format("{0} {1}", Localize.LicenseNo, Localize.Required.NotNullOrEmpty().ToLower()));
            }
            else if (Zulassung.Zulassungsdaten.IsSchnellabmeldung)
            {
                if (FinList.None(x => x.IsSchnellabmeldungSpeicherrelevant))
                    addModelError(string.Empty, Localize.PleaseChooseOneOrMoreVehicles);

                if (FinList.Any(x => x.IsSchnellabmeldungSpeicherrelevant && (x.Kennzeichen.IsNullOrEmpty() || x.Kennzeichen.EndsWith("-"))))
                    addModelError(string.Empty, string.Format("{0} {1}", Localize.LicenseNo, Localize.Required.NotNullOrEmpty().ToLower()));

                if (FinList.Any(x => x.IsSchnellabmeldungSpeicherrelevant && x.Halter.IsNullOrEmpty()))
                    addModelError(string.Empty, string.Format("{0} {1}", Localize.CarOwner, Localize.Required.NotNullOrEmpty().ToLower()));

                var regexTuevAu = new Regex("^(0[1-9]|1[0-2])[0-9]{2}$");
                if (FinList.Any(x => x.IsSchnellabmeldungSpeicherrelevant && x.TuevAu.IsNotNullOrEmpty() && !regexTuevAu.IsMatch(x.TuevAu)))
                    addModelError(string.Empty, string.Format("{0} {1} ({2}: {3})", Localize.TuevAu, Localize.Invalid.NotNullOrEmpty().ToLower(), Localize.Format, Localize.DateFormat_MMJJ));
            }
            else if (ModusAbmeldung)
            {
                if (zulassungsdatenModel.Kennzeichen.IsNullOrEmpty() || zulassungsdatenModel.Kennzeichen.EndsWith("-"))
                    addModelError(string.Empty, string.Format("{0} {1}", Localize.LicenseNo, Localize.Required.NotNullOrEmpty().ToLower()));
            }
        }

        public bool ValidateAuslieferAdressenForm(Action<string, string> addModelError, AuslieferAdressen model)
        {
            // AuslieferAdresseZ7
            if (model.AuslieferAdresseZ7.HasData && !model.AuslieferAdresseZ7.Adressdaten.AdresseVollstaendig)
                model.ErrorMsgAdresseZ7 = string.Format("{0} & ", Localize.CompleteAddressRequired);

            if (model.AuslieferAdresseZ7.ZugeordneteMaterialien.Contains("Sonstiges") && model.AuslieferAdresseZ7.Adressdaten.Bemerkung.IsNullOrEmpty())
                model.ErrorMsgAdresseZ7 += string.Format("{0} & ", Localize.CommentRequired);

            if (ModusVersandzulassung && model.AuslieferAdresseZ7.HasData && model.AuslieferAdresseZ7.Adressdaten.Adresse.Land != "DE")
                model.ErrorMsgAdresseZ7 += string.Format("{0} & ", Localize.ShippingOnlyPossibleWithinGermany);

            if (model.ErrorMsgAdresseZ7.IsNotNullOrEmpty())
                model.ErrorMsgAdresseZ7 = model.ErrorMsgAdresseZ7.Substring(0, model.ErrorMsgAdresseZ7.Length -2);

            // AuslieferAdresseZ8
            if (model.AuslieferAdresseZ8.HasData && !model.AuslieferAdresseZ8.Adressdaten.AdresseVollstaendig)
                model.ErrorMsgAdresseZ8 = string.Format("{0} & ", Localize.CompleteAddressRequired);

            if (model.AuslieferAdresseZ8.ZugeordneteMaterialien.Contains("Sonstiges") && model.AuslieferAdresseZ8.Adressdaten.Bemerkung.IsNullOrEmpty())
                model.ErrorMsgAdresseZ8 += string.Format("{0} & ", Localize.CommentRequired);

            if (ModusVersandzulassung && model.AuslieferAdresseZ7.HasData && model.AuslieferAdresseZ8.Adressdaten.Adresse.Land != "DE")
                model.ErrorMsgAdresseZ8 += string.Format("{0} & ", Localize.ShippingOnlyPossibleWithinGermany);

            if (model.ErrorMsgAdresseZ8.IsNotNullOrEmpty())
                model.ErrorMsgAdresseZ8 = model.ErrorMsgAdresseZ8.Substring(0, model.ErrorMsgAdresseZ8.Length - 2);

            // AuslieferAdresseZ9
            if (model.AuslieferAdresseZ9.HasData && !model.AuslieferAdresseZ9.Adressdaten.AdresseVollstaendig)
                model.ErrorMsgAdresseZ9 = string.Format("{0} & ", Localize.CompleteAddressRequired);

            if (model.AuslieferAdresseZ9.ZugeordneteMaterialien.Contains("Sonstiges") && model.AuslieferAdresseZ9.Adressdaten.Bemerkung.IsNullOrEmpty())
                model.ErrorMsgAdresseZ9 += string.Format("{0} & ", Localize.CommentRequired);

            if (ModusVersandzulassung && model.AuslieferAdresseZ7.HasData && model.AuslieferAdresseZ9.Adressdaten.Adresse.Land != "DE")
                model.ErrorMsgAdresseZ9 += string.Format("{0} & ", Localize.ShippingOnlyPossibleWithinGermany);

            if (model.ErrorMsgAdresseZ9.IsNotNullOrEmpty())
                model.ErrorMsgAdresseZ9 = model.ErrorMsgAdresseZ9.Substring(0, model.ErrorMsgAdresseZ9.Length - 2);


            if (model.ErrorMsgAdresseZ7.IsNotNullOrEmpty() || model.ErrorMsgAdresseZ8.IsNotNullOrEmpty() ||
                model.ErrorMsgAdresseZ9.IsNotNullOrEmpty())
                return false;

            return true;
        }

        public void ValidateVersanddatenForm(Action<string, string> addModelError, Versanddaten versanddatenModel)
        {
            if (ModusVersandzulassung && string.IsNullOrEmpty(versanddatenModel.VersandDienstleisterId))
                addModelError(string.Empty, Localize.PleaseSelectAShippingServiceProvider);
        }

        public void UpdateAnzahlAbmeldungen(string anzAbmeldungen)
        {
            var anzahlInt = anzAbmeldungen.ToInt(0);

            Zulassung.Zulassungsdaten.AnzahlAbmeldungen = anzahlInt;

            if (FinList.Count != anzahlInt)
            {
                while (FinList.Count < anzahlInt)
                {
                    var maxId = FinList.Max(f => f.FinID).ToInt(0);
                    FinList.Add(new FahrzeugAkteBestand { FinID = (maxId + 1).ToString("D3") });
                }

                while (FinList.Count > anzahlInt)
                {
                    var maxId = FinList.Max(f => f.FinID).ToInt(0);
                    FinList.RemoveAll(f => f.FinID == maxId.ToString("D3"));
                }

                PropertyCacheClear(this, m => m.FinListFiltered);
            }
        }


        #region Dashboard functionality

        static KeyValuePair<int, string> GetChartShoppingCartStackedKey(Vorgang item)
        {
            var date = (item.BeauftragungsArt.NotNullOrEmpty() == "ABMELDUNG" ? item.Abmeldedatum : item.Zulassungsdatum).GetValueOrDefault();
            var today = DateTime.Today;

            if (date < today)
                return new KeyValuePair<int, string>(1, "überfällig");
            if (date.Year == today.Year && date.GetWeekNumber() == today.GetWeekNumber())
                return new KeyValuePair<int, string>(2, "fällig");

            return new KeyValuePair<int, string>(3, "geplant");
        }


        [DashboardItemsLoadMethod("ZulassungShoppingCart")]
        public ChartItemsPackage NameNotRelevant01()
        {
            DataInit();

            var items = LoadZulassungenFromShoppingCart().ToListOrEmptyList();

            items = items.OrderBy(item => GetChartShoppingCartStackedKey(item).Key).ToListOrEmptyList();
            Func<Vorgang, string> xAxisKeyModel = (groupKey => groupKey.BeauftragungsArt.ToLowerFirstUpper());

            return ChartService.GetBarChartGroupedStackedItemsWithLabels(
                    items,
                    xAxisKeyModel,
                    xAxisList => xAxisList.Insert(0, ""),
                    item => GetChartShoppingCartStackedKey(item).Value
                );
        }

        #endregion
    }
}
