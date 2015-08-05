using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
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
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.Autohaus.ViewModels
{
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

        public bool ModusVersandzulassung { get; set; }

        [XmlIgnore]
        public string ApplicationTitle
        {
            get
            {
                if (ModusAbmeldung)
                    return Localize.Cancellation;

                if (ModusVersandzulassung)
                    return Localize.MailOrderRegistration;

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
                    var dict = XmlService.XmlDeserializeFromFile<XmlDictionary<string, string>>(Path.Combine(AppSettings.DataPath, @"StepsKroschkeZulassung.xml"));

                    if (!ModusAbmeldung)
                        return dict;

                    var abmeldungsDict = new XmlDictionary<string, string>();
                    dict.ToList().ForEach(entry =>
                        {
                            if (entry.Key == "Zulassungsdaten")
                            {
                                abmeldungsDict.Add(entry.Key, Localize.Cancellation);
                                return;
                            }

                            if (entry.Key == "OptionenDienstleistungen" || entry.Key == "ZahlerKfzSteuer" || entry.Key == "AuslieferAdressen")
                                return;

                            abmeldungsDict.Add(entry.Key, entry.Value);
                        });

                    return abmeldungsDict;
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

        public bool FahrzeugdatenKostenstelleIsVisible
        {
            get { return GetApplicationConfigValueForCustomer("AhZulassungKostenstelleAnzeigen").ToBool(); }
        }

        public void SetParamFahrzeugAkte(string fin)
        {
            ParamFahrzeugAkte = FahrzeugAkteBestandDataService.GetFahrzeugeAkteBestand(new FahrzeugAkteBestandSelektor { FIN = fin.NotNullOrEmpty("-") }).FirstOrDefault();
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

        public void SetParamVersandzulassung(string versandzulassung)
        {
            ModusVersandzulassung = versandzulassung.IsNotNullOrEmpty();
        }
 
 
        #region Rechnungsdaten

        [XmlIgnore, ScriptIgnore]
        public List<Kunde> Kunden
        {
            get
            {
                if (Zulassung.Zulassungsdaten.IsMassenzulassung)
                    return ZulassungDataService.Kunden.Where(k => !k.Cpdkunde).ToList();

                return ZulassungDataService.Kunden;
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
        /// </summary>
        /// <param name="finList"></param>
        /// <returns>True, wenn FinList mit Fahrzeugen vorhanden</returns>
        public bool SetFinList(object finList)
        {
            FinList = (List<FahrzeugAkteBestand>)finList;
            if (FinList == null)
                return false;

            FinList.ToList().ForEach(x => x.IsSelected = true);
            FinListFiltered = FinList;

            var firstFahrzeug = FinList.FirstOrDefault();
            if (firstFahrzeug == null) 
                return false;

            #region Halterdaten evtl. vorbelegen, wenn bei allen Fahrzeugen gleich
            var isEqual = true;
            foreach (var fahrzeugAkteBestand in FinList)
            {
                if (ModelMapping.Differences(fahrzeugAkteBestand.SelectedHalter, firstFahrzeug.SelectedHalter).Any())
                {
                    isEqual = false;
                    break;
                }
            }

            if (isEqual)    // Wenn Halterdaten aller Fahrzeuge identisch, soll Vorbelegung erfolgen...
                SetParamHalter(firstFahrzeug.Halter);

            #endregion

            Zulassung.Zulassungsdaten.IsMassenzulassung = true;

            return true;
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
                FinList.ToList().ForEach(x => x.WunschKennz1 = zulassungskreis);
                FinList.ToList().ForEach(x => x.WunschKennz2 = zulassungskreis);
                FinList.ToList().ForEach(x => x.WunschKennz3 = zulassungskreis);
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
        /// <param name="fin"></param>
        /// <param name="evb"></param>
        /// <returns>Null = gespeichert</returns>
        public string SetEvb(string fin, string evb)
        {
            if (!Zulassung.Zulassungsdaten.IsMassenzulassung)   // Funktion nur für Massenzulassung benötigt
                return null;

            if (!string.IsNullOrEmpty(evb) && evb.Length != 7)
                return Localize.EvbNumberLengthMustBe7;

            try
            {
                if (fin.IsNullOrEmpty())
                {
                    // evb für ALLE Fahrzeuge setzen
                    FinList.ToList().ForEach(x => x.Evb = evb);
                }
                else
                {
                    // evb nur für ein Fahrzeug setzen
                    FinList.Where(x => x.FIN == fin).ToList().ForEach(x => x.Evb = evb);
                }
                
                return null; 
            }
            catch (Exception e)
            {
                return e.InnerException.ToString(); 
            }
        }

        /// <summary>
        /// Setzt das Wunschkennzeichen für ein angegebenes Fahrzeug
        /// </summary>
        /// <param name="fin"></param>
        /// <param name="field"></param>
        /// <param name="kennz"></param>
        /// <returns>Null = gespeichert</returns>
        public string SetWunschKennz(string fin, string field, string kennz)
        {
            try
            {
                switch (field.ToLower())
                {
                    case "wunschkennz1":
                        FinList.Where(x => x.FIN == fin).ToList().ForEach(x => x.WunschKennz1 = kennz);
                        break;

                    case "wunschkennz2":
                        FinList.Where(x => x.FIN == fin).ToList().ForEach(x => x.WunschKennz2 = kennz);
                        break;

                    case "wunschkennz3":
                        FinList.Where(x => x.FIN == fin).ToList().ForEach(x => x.WunschKennz3 = kennz);
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

            if (adr != null)
                adr.Strasse = adr.StrasseHausNr;

            return adr;
        }

        public void SetHalterAdresse(Adresse model)
        {
            Zulassung.Halter.Adresse = model;

            if (Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.Name1.IsNullOrEmpty())
            {
                Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse = ModelMapping.Copy(Zulassung.Halter.Adresse);
                Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.Kennung = "ZAHLERKFZSTEUER";
                Zulassung.ZahlerKfzSteuer.Adressdaten.Adresse.Typ = "ZahlerKfzSteuer";
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

            // MMA Falls Massenzulassung, dann den Zulassungskreis auch für alle Wunschkennzeichen setzen
            if (Zulassung.Zulassungsdaten.IsMassenzulassung)
            {
                this.SetKreisAll(zulassungsKreis);
            }

            // 20150602 MMA Gegebenenfalls verfügbare externe Wunschkennzeichen-Reservierungs-Url ermitteln 
            Zulassung.Zulassungsdaten.WunschkennzeichenReservierenUrl = LoadZulassungsstelleWkzUrl(zulassungsKreis);

            Zulassung.Zulassungsdaten.EvbNr = model.EvbNr;  // 20150617 MMA EvbNr aus Halteradresse als Vorlage holen
        }

        public string ZulassungsKennzeichenLinkeSeite(string kennzeichen)
        {
            return Zulassungsdaten.ZulassungsKennzeichenLinkeSeite(kennzeichen);
        }

        static bool KennzeichenIsValid(string kennnzeichen)
        {
            return Zulassungsdaten.KennzeichenIsValid(kennnzeichen);
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

            if (adr != null)
                adr.Strasse = adr.StrasseHausNr;

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
            return ZulassungDataService.GetBankdaten(iban.NotNullOrEmpty().ToUpper());
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

        public AuslieferAdresse SelectedAuslieferAdresse { get { return Zulassung.AuslieferAdressen.FirstOrDefault(a => a.Adressdaten.Partnerrolle == SelectedAuslieferAdressePartnerrolle); } }

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

            if (adr != null)
                adr.Strasse = adr.StrasseHausNr;

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
        public List<Domaenenfestwert> Fahrzeugarten { get { return ZulassungDataService.Fahrzeugarten; } }

        public void SetFahrzeugdaten(Fahrzeugdaten model)
        {
            Zulassung.Fahrzeugdaten.AuftragsNr = model.AuftragsNr;
            Zulassung.Fahrzeugdaten.FahrgestellNr = model.FahrgestellNr.NotNullOrEmpty().ToUpper();
            Zulassung.Fahrzeugdaten.Zb2Nr = model.Zb2Nr.NotNullOrEmpty().ToUpper();
            Zulassung.Fahrzeugdaten.FahrzeugartId = model.FahrzeugartId;
            Zulassung.Fahrzeugdaten.VerkaeuferKuerzel = model.VerkaeuferKuerzel;
            Zulassung.Fahrzeugdaten.Kostenstelle = model.Kostenstelle;
            Zulassung.Fahrzeugdaten.BestellNr = model.BestellNr;

            if (Zulassung.Fahrzeugdaten.IstAnhaenger || Zulassung.Fahrzeugdaten.IstMotorrad)
                Zulassung.OptionenDienstleistungen.NurEinKennzeichen = true;
        }

        #endregion


        #region Zulassungsdaten

        [XmlIgnore, ScriptIgnore]
        public List<Material> Zulassungsarten { get { return PropertyCacheGet(() => ZulassungDataService.Zulassungsarten.Where(z => !ModusVersandzulassung || z.IstVersand).ToList()); } }

        [XmlIgnore, ScriptIgnore]
        public List<Material> Abmeldearten { get { return PropertyCacheGet(() => ZulassungDataService.Abmeldearten); } }

        public void SetZulassungsdaten(Zulassungsdaten model, ModelStateDictionary state)
        {
            var zulDat = Zulassung.Zulassungsdaten;

            zulDat.ZulassungsartMatNr = model.ZulassungsartMatNr;
            zulDat.Zulassungsdatum = model.Zulassungsdatum;
            zulDat.Abmeldedatum = model.Abmeldedatum;
            zulDat.Zulassungskreis = model.Zulassungskreis.NotNullOrEmpty().ToUpper();
            zulDat.ZulassungskreisBezeichnung = model.ZulassungskreisBezeichnung;
            zulDat.EvbNr = model.EvbNr.NotNullOrEmpty().ToUpper();

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

            Zulassung.OptionenDienstleistungen.ZulassungsartMatNr = zulDat.ZulassungsartMatNr;

            var tempKg = Zulassung.OptionenDienstleistungen.KennzeichengroesseListForMatNr.FirstOrDefault(k => k.Groesse == "520x114");
            if (tempKg != null)
                Zulassung.OptionenDienstleistungen.KennzeichenGroesseId = tempKg.Id;

            // 20150602 MMA
            Zulassung.Zulassungsdaten.MindesthaltedauerDays = model.MindesthaltedauerDays;  // Identisch mit SAP-Feld HALTE_DAUER

            // Falls Zulassungsdatum gefüllt und firmeneigene Zulassung, dann Datumsfeld "HaltedauerBis" setzen...
            if (model.MindesthaltedauerDays != null && model.Zulassungsdatum != null && Zulassungsdaten.IstFirmeneigeneZulassung(Zulassung.OptionenDienstleistungen.ZulassungsartMatNr))
                Zulassung.OptionenDienstleistungen.HaltedauerBis = model.Zulassungsdatum.Value.AddDays((double)model.MindesthaltedauerDays);
            else
                Zulassung.OptionenDienstleistungen.HaltedauerBis = null;


            if (ModusVersandzulassung || zulDat.Zulassungsart.Auf48hVersandPruefen)
            {
                Zulassung.VersandAdresse.Adresse = ZulassungDataService.GetLieferantZuKreis(zulDat.Zulassungskreis);

                if (ModusVersandzulassung)
                {
                    var tmpLiefNr = Zulassung.VersandAdresse.Adresse.KundenNr.NotNullOrEmpty().TrimStart('0');
                    if (tmpLiefNr.StartsWith("564"))
                        Zulassung.VkBur = tmpLiefNr.Substring(2);
                }

                var checkErg = ZulassungDataService.Check48hExpress(Zulassung);

                if (Zulassung.Zulassungsdaten.Zulassungsart.ZulassungAmFolgetagNichtMoeglich && (Zulassung.Ist48hZulassung || !String.IsNullOrEmpty(checkErg)))
                    state.AddModelError("", Localize.RegistrationDateMustBeAtLeast2DaysInTheFuture);
                else if (!String.IsNullOrEmpty(checkErg))
                    state.AddModelError("", checkErg);
            }
        }

        #endregion


        #region OptionenDienstleistungen

        [XmlIgnore, ScriptIgnore]
        public List<Kennzeichengroesse> Kennzeichengroessen { get { return ZulassungDataService.Kennzeichengroessen; } }

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
        }

        #endregion


        #region Misc + Summaries + Savings

        public bool SaveDataToErpSystem { get; set; }

        public bool AuftragslisteAvailable { get; set; }

        public void DataInit(string zulassungFromShoppingCart = "")
        {
            if (zulassungFromShoppingCart.IsNullOrEmpty())
            {
                Zulassung = new Vorgang
                    {
                        VkOrg = LogonContext.Customer.AccountingArea.ToString(),
                        VkBur = LogonContext.Organization.OrganizationReference2,
                        Vorerfasser = LogonContext.UserName,
                        VorgangsStatus = "1",
                        Zulassungsdaten = new Zulassungsdaten
                            {
                                ModusAbmeldung = ModusAbmeldung,
                                ModusVersandzulassung = ModusVersandzulassung,
                                ZulassungsartMatNr =
                                    (!ModusAbmeldung || Abmeldearten.None() ? null : Abmeldearten.First().MaterialNr),
                                Zulassungskreis = null,
                            },
                        Fahrzeugdaten = new Fahrzeugdaten
                            {
                                FahrzeugartId = "1",
                            }
                    };
            }
            else
            {
                ModusAbmeldung = (Zulassung.BeauftragungsArt == "ABMELDUNG");
                ModusVersandzulassung = (Zulassung.BeauftragungsArt == "VERSANDZULASSUNG");
                Zulassung.Zulassungsdaten.ModusAbmeldung = ModusAbmeldung;
                Zulassung.Zulassungsdaten.ModusVersandzulassung = ModusVersandzulassung;
            }

            SelectedAuslieferAdressePartnerrolle = Vorgang.AuslieferAdressenPartnerRollen.First().Key;

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            InitZulassung(Zulassung);

            Fahrzeugdaten.FahrzeugartList = Fahrzeugarten;
            Adresse.Laender = LaenderList;
            OptionenDienstleistungen.KennzeichengroesseList = Kennzeichengroessen;

            PartnerDataService.MarkForRefreshAdressen();

            PropertyCacheClear(this, m => m.Zulassungsarten);
            PropertyCacheClear(this, m => m.Abmeldearten);
            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);
        }

        private void InitZulassung(Vorgang zul)
        {
            zul.Kunden = Kunden;

            ZulassungDataService.MarkForRefresh();
            zul.OptionenDienstleistungen.InitDienstleistungen(ZulassungDataService.Zusatzdienstleistungen);
        }

        public void Save(List<Vorgang> zulassungen, bool saveDataToSap, bool saveFromShoppingCart)
        {
            if (!ModusAbmeldung && Zulassungsarten.None())
                return;

            if (ModusAbmeldung && Abmeldearten.None())
                return;

            zulassungen.ForEach(z =>
                {
                    z.WebGroupId = LogonContext.Group.GroupID.ToString();
                    z.WebUserId = LogonContext.UserID;
                });

            if (!saveFromShoppingCart)
                zulassungen.ForEach(z => z.BeauftragungsArt = (ModusVersandzulassung ? "VERSANDZULASSUNG" : ModusAbmeldung ? "ABMELDUNG" : z.Zulassungsdaten.IsMassenzulassung ? "MASSENZULASSUNG" : "ZULASSUNG"));

            var zulassungenToSave = new List<Vorgang>();
           
            if (Zulassung.Zulassungsdaten.IsMassenzulassung)
            {
                // Alle zuzulassenden Fahrzeuge durchlaufen
                foreach (var fahrzeugAkteBestand in FinListFiltered.Where(x => x.IsSelected))
                {
                    var singleZulassung = ModelMapping.Copy(Zulassung); // Achtung: Kopiert nicht, sondern legt eine Referenz von Zulassung.Zulassungsdaten an
                    singleZulassung.Zulassungsdaten = ModelMapping.Copy(Zulassung.Zulassungsdaten); // Explizit Zulassungsdaten kopieren, damit keine Referenz erzeugt wird
                    singleZulassung.Fahrzeugdaten = ModelMapping.Copy(Zulassung.Fahrzeugdaten);     // Explizit Fahrzeugdaten kopieren, damit keine Referenz erzeugt wird
                    
                    // 20150723
                    singleZulassung.ZahlerKfzSteuer = ModelMapping.Copy(Zulassung.ZahlerKfzSteuer);
                    singleZulassung.VersandAdresse = ModelMapping.Copy(Zulassung.VersandAdresse);

                    // 20150722
                    singleZulassung.AuslieferAdressen    = new List<AuslieferAdresse>();            // ModelMapping.Copy(Zulassung.AuslieferAdressen) gibt Fehlermeldung "Parameteranzahlkonflikt", daher nicht verwendet
                    singleZulassung.Halter = ModelMapping.Copy(Zulassung.Halter);
                    singleZulassung.BankAdressdaten = ModelMapping.Copy(Zulassung.BankAdressdaten);

                    // singleZulassung.Zusatzformulare = ModelMapping.Copy(Zulassung.Zusatzformulare);  // Fehlermeldung "Parameteranzahlkonflikt", daher nicht verwendet
                    singleZulassung.Zusatzformulare = new List<PdfFormular>();

                    singleZulassung.Fahrzeugdaten.FahrgestellNr = fahrzeugAkteBestand.FIN;

                    singleZulassung.Zulassungsdaten.EvbNr = fahrzeugAkteBestand.Evb;
                    singleZulassung.Zulassungsdaten.Kennzeichen = fahrzeugAkteBestand.WunschKennz1;
                    singleZulassung.Zulassungsdaten.Wunschkennzeichen2 = fahrzeugAkteBestand.WunschKennz2;
                    singleZulassung.Zulassungsdaten.Wunschkennzeichen3 = fahrzeugAkteBestand.WunschKennz3;

                    zulassungenToSave.Add(singleZulassung);
                }
            }
            else
            {
                zulassungenToSave = zulassungen;
            }
            
            SaveDataToErpSystem = saveDataToSap;
            AuftragslisteAvailable = saveDataToSap;

            ZulassungenForReceipt = new List<Vorgang>();
            
            SaveErrorMessage = ZulassungDataService.SaveZulassungen(zulassungenToSave, saveDataToSap, saveFromShoppingCart, ModusAbmeldung, ModusVersandzulassung);

            if (SaveErrorMessage.IsNullOrEmpty())
            {
                ZulassungenForReceipt = zulassungenToSave.Select(zulassung => ModelMapping.Copy(zulassung)).ToListOrEmptyList();

                if (ZulassungenForReceipt.ToListOrEmptyList().None() || ZulassungenForReceipt.First().Zusatzformulare.ToListOrEmptyList().None(z => z.IstAuftragsListe))
                    AuftragslisteAvailable = false;
            }
        }

        #endregion


        #region Shopping Cart

        public IEnumerable<Vorgang> LoadZulassungenFromShoppingCart()
        {
            var liste = ZulassungDataService.LoadVorgaengeForShoppingCart();

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
    }
}
