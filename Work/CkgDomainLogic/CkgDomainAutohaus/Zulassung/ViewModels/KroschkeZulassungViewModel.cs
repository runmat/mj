using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.General.Contracts;
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

        [XmlIgnore]
        [LocalizedDisplay(LocalizeConstants.Holder)]
        public string HalterDatenAsString { get { return HalterAdresse.GetAutoSelectString(); } }

        public static string PfadAuftragszettel { get { return GeneralConfiguration.GetConfigValue("KroschkeAutohaus", "PfadAuftragszettel"); } }
        public bool ModusAbmeldung { get; set; }


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

                            if (entry.Key == "OptionenDienstleistungen")
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

        string GetApplicationConfigValueForCustomer(string configValue)
        {
            if (LogonContext == null || LogonContext.Customer == null)
                return "";

            var userCustomerId = LogonContext.Customer.CustomerID;
            var userGroupId = 0;
            var appId = HttpContextService.TryGetAppIdFromUrlOrSession();

            return ApplicationConfiguration.GetApplicationConfigValue(configValue, appId.ToString(), userCustomerId, userGroupId);
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
            HalterAdresse = HalterAdressen.FirstOrDefault(a => a.KundenNr.NotNullOrEmpty().ToSapKunnr() == ParamFahrzeugAkte.Halter.NotNullOrEmpty().ToSapKunnr());
        }

        public void SetParamHalter(string halterNr)
        {
            if (halterNr.IsNullOrEmpty())
                return;

            HalterAdresse = HalterAdressen.FirstOrDefault(a => a.KundenNr.NotNullOrEmpty().ToSapKunnr() == halterNr.NotNullOrEmpty().ToSapKunnr());
        }

        public void SetParamAbmeldung(string abmeldung)
        {
            ModusAbmeldung = abmeldung.IsNotNullOrEmpty();
        }


        #region Rechnungsdaten

        [XmlIgnore, ScriptIgnore]
        public List<Kunde> Kunden { get { return ZulassungDataService.Kunden; } }


        public void SetRechnungsdaten(Rechnungsdaten model)
        {
            if (Zulassung.Rechnungsdaten.KundenNr != model.KundenNr)
                Zulassung.BankAdressdaten.Zahlungsart = null;

            Zulassung.Rechnungsdaten.KundenNr = model.KundenNr;

            Zulassung.BankAdressdaten.Cpdkunde = Zulassung.Rechnungsdaten.GetKunde(Kunden).Cpdkunde;
            Zulassung.BankAdressdaten.CpdMitEinzugsermaechtigung = Zulassung.Rechnungsdaten.GetKunde(Kunden).CpdMitEinzugsermaechtigung;

            if (Zulassung.BankAdressdaten.Zahlungsart.IsNullOrEmpty())
                Zulassung.BankAdressdaten.Zahlungsart = (Zulassung.BankAdressdaten.CpdMitEinzugsermaechtigung ? "E" : "");
        }

        #endregion


        #region Bank-/Adressdaten

        public bool SkipBankAdressdaten { get; set; }

        public void CheckCpd()
        {
            SkipBankAdressdaten = !Zulassung.Rechnungsdaten.GetKunde(Kunden).Cpdkunde;
        }

        public void SetBankAdressdaten(ref BankAdressdaten model)
        {
            Zulassung.BankAdressdaten.Rechnungsempfaenger = model.Rechnungsempfaenger;
            Zulassung.BankAdressdaten.Zahlungsart = model.Zahlungsart;
            Zulassung.BankAdressdaten.Kontoinhaber = model.Kontoinhaber;
            Zulassung.BankAdressdaten.Iban = model.Iban.NotNullOrEmpty().ToUpper();

            if (model.Swift.NotNullOrEmpty().ToUpper() == Localize.WillBeFilledAutomatically.ToUpper())
                Zulassung.BankAdressdaten.Swift = "";
            else
                Zulassung.BankAdressdaten.Swift = model.Swift.NotNullOrEmpty().ToUpper();

            Zulassung.BankAdressdaten.KontoNr = model.KontoNr;
            Zulassung.BankAdressdaten.Bankleitzahl = model.Bankleitzahl;

            if (model.Geldinstitut.NotNullOrEmpty().ToUpper() == Localize.WillBeFilledAutomatically.ToUpper())
                Zulassung.BankAdressdaten.Geldinstitut = "";
            else
                Zulassung.BankAdressdaten.Geldinstitut = model.Geldinstitut;
        }

        public Bankdaten LoadBankdatenAusIban(string iban)
        {
            return ZulassungDataService.GetBankdaten(iban.NotNullOrEmpty().ToUpper());
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


        #region HalterAdresse

        [XmlIgnore, ScriptIgnore]
        public List<Land> LaenderList { get { return ZulassungDataService.Laender; } }

        [XmlIgnore, ScriptIgnore]
        public Adresse HalterAdresse
        {
            get { return Zulassung.Halterdaten; }
            set { Zulassung.Halterdaten = value; }
        }

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
            int id;
            if (Int32.TryParse(key, out id))
                return HalterAdressen.FirstOrDefault(v => v.KundenNr.NotNullOrEmpty().ToSapKunnr() == key.NotNullOrEmpty().ToSapKunnr());

            return HalterAdressen.FirstOrDefault(a => a.GetAutoSelectString() == key);
        }

        public void SetHalterAdresse(Adresse model)
        {
            HalterAdresse = model;

            var zulassungsKreis = LoadKfzKreisAusHalterAdresse();
            Zulassung.Zulassungsdaten.Zulassungskreis = zulassungsKreis;

            if (!KennzeichenIsValid(Zulassung.Zulassungsdaten.Kennzeichen))
                Zulassung.Zulassungsdaten.Kennzeichen = ZulassungskreisToKennzeichenLinkeSeite(zulassungsKreis);

            if (!KennzeichenIsValid(Zulassung.Zulassungsdaten.Wunschkennzeichen2))
                Zulassung.Zulassungsdaten.Wunschkennzeichen2 = ZulassungskreisToKennzeichenLinkeSeite(zulassungsKreis);

            if (!KennzeichenIsValid(Zulassung.Zulassungsdaten.Wunschkennzeichen3))
                Zulassung.Zulassungsdaten.Wunschkennzeichen3 = ZulassungskreisToKennzeichenLinkeSeite(zulassungsKreis);
        }

        public string ZulassungskreisToKennzeichenLinkeSeite(string zulassungsKreis)
        {
            return Zulassungsdaten.ZulassungskreisToKennzeichenLinkeSeite(zulassungsKreis);
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

        public string LoadKfzKreisAusHalterAdresse()
        {
            return HalterAdresse == null ? "" : ZulassungDataService.GetZulassungskreis(Zulassung);
        }

        #endregion


        #region Zulassungsdaten

        [XmlIgnore, ScriptIgnore]
        public List<Material> Zulassungsarten { get { return PropertyCacheGet(() => ZulassungDataService.Zulassungsarten); } }

        [XmlIgnore, ScriptIgnore]
        public List<Material> Abmeldearten { get { return ZulassungDataService.Abmeldearten; } }

        public void SetZulassungsdaten(Zulassungsdaten model)
        {
            Zulassung.Zulassungsdaten.ZulassungsartMatNr = model.ZulassungsartMatNr;
            Zulassung.Zulassungsdaten.Zulassungsdatum = model.Zulassungsdatum;
            Zulassung.Zulassungsdaten.Abmeldedatum = model.Abmeldedatum;
            Zulassung.Zulassungsdaten.Zulassungskreis = model.Zulassungskreis.NotNullOrEmpty().ToUpper();
            Zulassung.Zulassungsdaten.ZulassungskreisBezeichnung = model.ZulassungskreisBezeichnung;
            Zulassung.Zulassungsdaten.EvbNr = model.EvbNr.NotNullOrEmpty().ToUpper();

            Zulassung.Zulassungsdaten.KennzeichenReserviert = model.KennzeichenReserviert;

            if (Zulassung.Zulassungsdaten.KennzeichenReserviert)
            {
                Zulassung.Zulassungsdaten.ReservierungsNr = model.ReservierungsNr;
                Zulassung.Zulassungsdaten.ReservierungsName = model.ReservierungsName;
            }
            else
            {
                Zulassung.Zulassungsdaten.ReservierungsNr = "";
                Zulassung.Zulassungsdaten.ReservierungsName = "";
            }

            Zulassung.Zulassungsdaten.Kennzeichen = model.Kennzeichen;
            Zulassung.Zulassungsdaten.Wunschkennzeichen2 = model.Wunschkennzeichen2;
            Zulassung.Zulassungsdaten.Wunschkennzeichen3 = model.Wunschkennzeichen3;

            Zulassung.OptionenDienstleistungen.ZulassungsartMatNr = Zulassung.Zulassungsdaten.ZulassungsartMatNr;

            var tempKg = Zulassung.OptionenDienstleistungen.KennzeichengroesseListForMatNr.FirstOrDefault(k => k.Groesse == "520x114");
            if (tempKg != null)
                Zulassung.OptionenDienstleistungen.KennzeichenGroesseId = tempKg.Id;
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

        public void DataInit()
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
                        ZulassungsartMatNr = (!ModusAbmeldung || Abmeldearten.None() ? null : Abmeldearten.First().MaterialNr),
                        Zulassungskreis = (!ModusAbmeldung  ? null : "-"),
                    },
                Fahrzeugdaten = new Fahrzeugdaten
                    {
                        FahrzeugartId = "1",
                    }
            };

            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            Zulassung.Kunden = Kunden;

            ZulassungDataService.MarkForRefresh();
            Zulassung.OptionenDienstleistungen.InitDienstleistungen(ZulassungDataService.Zusatzdienstleistungen);

            Fahrzeugdaten.FahrzeugartList = Fahrzeugarten;
            Adresse.Laender = LaenderList;
            OptionenDienstleistungen.KennzeichengroesseList = Kennzeichengroessen;

            PartnerDataService.MarkForRefreshAdressen();

            PropertyCacheClear(this, m => m.Zulassungsarten);
            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);
        }

        public void Save(List<Vorgang> zulassungen, bool saveDataToSap, bool saveFromShoppingCart)
        {
            SaveDataToErpSystem = saveDataToSap;
            AuftragslisteAvailable = saveDataToSap;

            ZulassungenForReceipt = new List<Vorgang>();

            SaveErrorMessage = ZulassungDataService.SaveZulassungen(zulassungen, saveDataToSap, saveFromShoppingCart);

            if (SaveErrorMessage.IsNullOrEmpty())
            {
                ZulassungenForReceipt = zulassungen.Select(zulassung => ModelMapping.Copy(zulassung)).ToListOrEmptyList();

                if (ZulassungenForReceipt.ToListOrEmptyList().None() || ZulassungenForReceipt.First().Zusatzformulare.ToListOrEmptyList().None(z => z.IstAuftragsListe))
                    AuftragslisteAvailable = false;
            }
        }

        #endregion
    }
}
