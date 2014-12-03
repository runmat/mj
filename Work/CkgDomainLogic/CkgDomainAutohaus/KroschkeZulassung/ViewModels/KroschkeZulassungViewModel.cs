using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.KroschkeZulassung.Contracts;
using CkgDomainLogic.KroschkeZulassung.Models;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.KroschkeZulassung.ViewModels
{
    public class KroschkeZulassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IAdressenDataService AdressenDataService { get { return CacheGet<IAdressenDataService>(); } }

        [XmlIgnore]
        public IKroschkeZulassungDataService ZulassungDataService { get { return CacheGet<IKroschkeZulassungDataService>(); } }

        public Vorgang Zulassung { get { return ZulassungDataService.Zulassung; } }

        [XmlIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() =>
                {
                    var dict = XmlService.XmlDeserializeFromFile<XmlDictionary<string, string>>(Path.Combine(AppSettings.DataPath, @"StepsKroschkeZulassung.xml"));

                    return dict;
                });
            }
        }

        public string[] StepKeys { get { return PropertyCacheGet(() => Steps.Select(s => s.Key).ToArray()); } }

        public string[] StepFriendlyNames { get { return PropertyCacheGet(() => Steps.Select(s => s.Value).ToArray()); } }

        public string FirstStepPartialViewName
        {
            get { return string.Format("{0}", StepKeys[0]); }
        }

        [XmlIgnore]
        public string SaveErrorMessage { get; private set; }


        #region Rechnungsdaten

        [XmlIgnore]
        public List<Kunde> Kunden { get { return ZulassungDataService.Kunden; } }

        public void SetRechnungsdaten(Rechnungsdaten model)
        {
            Zulassung.Rechnungsdaten.KundenNr = model.KundenNr;

            Zulassung.BankAdressdaten.Cpdkunde = Zulassung.Rechnungsdaten.Kunde.Cpdkunde;
            Zulassung.BankAdressdaten.CpdMitEinzugsermaechtigung = Zulassung.Rechnungsdaten.Kunde.CpdMitEinzugsermaechtigung;
        }

        #endregion


        #region Bank-/Adressdaten

        public bool SkipBankAdressdaten { get; private set; }

        public void CheckCpd()
        {
            SkipBankAdressdaten = !Zulassung.Rechnungsdaten.Kunde.Cpdkunde;
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

        [XmlIgnore]
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

        [XmlIgnore]
        public List<Land> LaenderList { get { return ZulassungDataService.Laender; } }

        [XmlIgnore]
        public Adresse HalterAdresse
        {
            get { return Zulassung.Halterdaten; }
            set { Zulassung.Halterdaten = value; }
        }

        [XmlIgnore]
        public List<Adresse> HalterAdressenFiltered
        {
            get { return PropertyCacheGet(() => GetHalterAdressen()); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterHalterAdressen(string filterValue, string filterProperties)
        {
            HalterAdressenFiltered = GetHalterAdressen().SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        List<Adresse> GetHalterAdressen()
        {
            var list = AdressenDataService.Adressen.Where(a => a.Kennung == "HALTER").ToListOrEmptyList();
            list.ForEach(a => a.Typ = "Halter");
            return list;
        }

        public List<string> GetHalterAdressenAsAutoCompleteItems()
        {
            return GetHalterAdressen().Select(a => a.GetAutoSelectString()).ToList();
        }

        public Adresse GetHalteradresse(string key)
        {
            int id;
            if (Int32.TryParse(key, out id))
                return GetHalterAdressen().FirstOrDefault(v => v.ID == id);

            return GetHalterAdressen().FirstOrDefault(a => a.GetAutoSelectString() == key);
        }

        public void SetHalterAdresse(Adresse model)
        {
            HalterAdresse = model;
        }

        public void DataMarkForRefreshHalterAdressenFiltered()
        {
            PropertyCacheClear(this, m => m.HalterAdressenFiltered);
        }

        public string LoadKfzKreisAusHalterAdresse()
        {
            return HalterAdresse == null ? "" : ZulassungDataService.GetZulassungskreis();
        }

        #endregion


        #region Zulassungsdaten

        [XmlIgnore]
        public List<Material> Zulassungsarten { get { return ZulassungDataService.Zulassungsarten; } }

        public void SetZulassungsdaten(Zulassungsdaten model)
        {
            Zulassung.Zulassungsdaten.ZulassungsartMatNr = model.ZulassungsartMatNr;
            Zulassung.Zulassungsdaten.Zulassungsdatum = model.Zulassungsdatum;
            Zulassung.Zulassungsdaten.Zulassungskreis = model.Zulassungskreis.NotNullOrEmpty().ToUpper();
            Zulassung.Zulassungsdaten.ZulassungskreisBezeichnung = model.ZulassungskreisBezeichnung;
            Zulassung.Zulassungsdaten.EvbNr = model.EvbNr.NotNullOrEmpty().ToUpper();
            Zulassung.Zulassungsdaten.Kennzeichen = model.Kennzeichen.NotNullOrEmpty().ToUpper();
            Zulassung.Zulassungsdaten.Wunschkennzeichen = model.Wunschkennzeichen;
            Zulassung.Zulassungsdaten.Wunschkennzeichen2 = model.Wunschkennzeichen2.NotNullOrEmpty().ToUpper();
            Zulassung.Zulassungsdaten.Wunschkennzeichen3 = model.Wunschkennzeichen3.NotNullOrEmpty().ToUpper();
            Zulassung.Zulassungsdaten.KennzeichenReservieren = model.KennzeichenReservieren;
            Zulassung.Zulassungsdaten.ReservierungsNr = model.ReservierungsNr;
            Zulassung.Zulassungsdaten.ReservierungsName = model.ReservierungsName;

            Zulassung.OptionenDienstleistungen.ZulassungsartMatNr = Zulassung.Zulassungsdaten.ZulassungsartMatNr;
        }

        #endregion


        #region OptionenDienstleistungen

        [XmlIgnore]
        public List<Kennzeichengroesse> Kennzeichengroessen { get { return ZulassungDataService.Kennzeichengroessen; } }

        public void SetOptionenDienstleistungen(OptionenDienstleistungen model)
        {
            Zulassung.OptionenDienstleistungen.GewaehlteDienstleistungenString = model.GewaehlteDienstleistungenString;
            Zulassung.OptionenDienstleistungen.NurEinKennzeichen = model.NurEinKennzeichen;
            Zulassung.OptionenDienstleistungen.KennzeichenSondergroesse = model.KennzeichenSondergroesse;
            Zulassung.OptionenDienstleistungen.KennzeichenGroesseId = model.KennzeichenGroesseId;
            Zulassung.OptionenDienstleistungen.Saisonkennzeichen = model.Saisonkennzeichen;
            Zulassung.OptionenDienstleistungen.SaisonBeginn = model.SaisonBeginn;
            Zulassung.OptionenDienstleistungen.SaisonEnde = model.SaisonEnde;
            Zulassung.OptionenDienstleistungen.Bemerkung = model.Bemerkung;
            Zulassung.OptionenDienstleistungen.ZulassungsartMatNr = model.ZulassungsartMatNr;

            if (Zulassung.OptionenDienstleistungen.IstGebrauchtzulassung)
                Zulassung.OptionenDienstleistungen.KennzeichenVorhanden = model.KennzeichenVorhanden;
            else
                Zulassung.OptionenDienstleistungen.KennzeichenVorhanden = false;

            if (Zulassung.OptionenDienstleistungen.IstAbmeldung)
                Zulassung.OptionenDienstleistungen.VorhandenesKennzeichenReservieren = model.VorhandenesKennzeichenReservieren;
            else
                Zulassung.OptionenDienstleistungen.VorhandenesKennzeichenReservieren = false;

            if (Zulassung.OptionenDienstleistungen.IstFirmeneigeneZulassung)
                Zulassung.OptionenDienstleistungen.HaltedauerBis = model.HaltedauerBis;
            else
                Zulassung.OptionenDienstleistungen.HaltedauerBis = null;

            if (Zulassung.OptionenDienstleistungen.IstUmkennzeichnung)
                Zulassung.OptionenDienstleistungen.AltesKennzeichen = model.AltesKennzeichen.NotNullOrEmpty().ToUpper();
            else
                Zulassung.OptionenDienstleistungen.AltesKennzeichen = "";
        }

        #endregion


        #region Misc + Summaries + Savings

        public void DataMarkForRefresh()
        {
            ZulassungDataService.MarkForRefresh();

            Rechnungsdaten.KundenList = Kunden;
            Fahrzeugdaten.FahrzeugartList = Fahrzeugarten;
            Adresse.Laender = LaenderList;
            Zulassungsdaten.MaterialList = Zulassungsarten;
            OptionenDienstleistungen.KennzeichengroesseList = Kennzeichengroessen;

            AdressenDataService.MarkForRefreshAdressen();

            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);
        }

        public void Save(bool saveDataInSap)
        {
            SaveErrorMessage = ZulassungDataService.SaveZulassung(saveDataInSap, true);
        }

        public string BeauftragungBezeichnung
        {
            get
            {
                return String.Format("{0}: {1}, {2}, {3}, {4}",
                    Zulassung.Fahrzeugdaten.AuftragsNr,
                    Zulassung.Rechnungsdaten.Kunde.KundenNameNr,
                    Zulassung.Zulassungsdaten.Zulassungsart.MaterialText,
                    Zulassung.Halter,
                    Zulassung.Zulassungsdaten.Kennzeichen);
            }
        }

        private GeneralEntity SummaryBeauftragungsHeader
        {
            get
            {
                return new GeneralEntity
                {
                    Title = Localize.YourOrder,
                    Body = BeauftragungBezeichnung,
                    Tag = "SummaryMainItem"
                };
            }
        }

        public GeneralSummary CreateSummaryModel()
        {
            var summaryModel = new GeneralSummary
            {
                Header = Localize.OrderSummaryVehicleRegistration,
                Items = new ListNotEmpty<GeneralEntity>
                        (
                            SummaryBeauftragungsHeader,

                            new GeneralEntity
                            {
                                Title = Localize.InvoiceData,
                                Body = Zulassung.Rechnungsdaten.GetSummaryString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.BankDataAndAddressForEndCustomerInvoice,
                                Body = Zulassung.BankAdressdaten.GetSummaryString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.VehicleData,
                                Body = Zulassung.Fahrzeugdaten.GetSummaryString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.Holder,
                                Body = HalterAdresse.GetPostLabelString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.Registration,
                                Body = Zulassung.Zulassungsdaten.GetSummaryString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.RegistrationOptions,
                                Body = Zulassung.OptionenDienstleistungen.GetSummaryString(),
                            }
                        )
            };

            return summaryModel;
        }

        #endregion
    }
}
