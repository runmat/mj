using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.KroschkeZulassung.Contracts;
using CkgDomainLogic.KroschkeZulassung.Models;
using CkgDomainLogic.Partner.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.KroschkeZulassung.ViewModels
{
    public class KroschkeZulassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IKroschkeZulassungDataService ZulassungDataService { get { return CacheGet<IKroschkeZulassungDataService>(); } }

        [XmlIgnore]
        public IFahrzeugAkteBestandDataService FahrzeugAkteBestandDataService { get { return CacheGet<IFahrzeugAkteBestandDataService>(); } }

        [XmlIgnore]
        public IPartnerDataService PartnerDataService { get { return CacheGet<IPartnerDataService>(); } }

        public Vorgang Zulassung { get; set; }
        //public Vorgang Zulassung { get { return ZulassungDataService.Zulassung; } }

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
        public string SaveErrorMessage { get; set; }

        public FahrzeugAkteBestand ParamFahrzeugAkte { get; set; }


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
            HalterAdresse = HalterAdressen.FirstOrDefault(a => a.KundenNr.NotNullOrEmpty().ToSapKunnr() == halterNr.NotNullOrEmpty().ToSapKunnr());
        }


        #region Rechnungsdaten

        [XmlIgnore]
        public List<Kunde> Kunden { get { return ZulassungDataService.Kunden; } }

        public void SetRechnungsdaten(Rechnungsdaten model)
        {
            Zulassung.Rechnungsdaten.KundenNr = model.KundenNr;

            Zulassung.BankAdressdaten.Cpdkunde = Zulassung.Rechnungsdaten.Kunde.Cpdkunde;
            Zulassung.BankAdressdaten.CpdMitEinzugsermaechtigung = Zulassung.Rechnungsdaten.Kunde.CpdMitEinzugsermaechtigung;

            Zulassung.BankAdressdaten.Zahlungsart = (Zulassung.BankAdressdaten.CpdMitEinzugsermaechtigung ? "E" : "");
        }

        #endregion


        #region Bank-/Adressdaten

        public bool SkipBankAdressdaten { get; set; }

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
        public List<Adresse> HalterAdressen
        {
            // ReSharper disable ConvertClosureToMethodGroup
            get { return PropertyCacheGet(() => GetHalterAdressen()); }
            // ReSharper restore ConvertClosureToMethodGroup
        }

        [XmlIgnore]
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

            Zulassung.Zulassungsdaten.Zulassungskreis = LoadKfzKreisAusHalterAdresse();

            if (String.IsNullOrEmpty(Zulassung.Zulassungsdaten.Kennzeichen) || Zulassung.Zulassungsdaten.Kennzeichen.EndsWith("-"))
                Zulassung.Zulassungsdaten.Kennzeichen = String.Format("{0}-", Zulassung.Zulassungsdaten.Zulassungskreis);

            if (String.IsNullOrEmpty(Zulassung.Zulassungsdaten.Wunschkennzeichen2) || Zulassung.Zulassungsdaten.Wunschkennzeichen2.EndsWith("-"))
                Zulassung.Zulassungsdaten.Wunschkennzeichen2 = String.Format("{0}-", Zulassung.Zulassungsdaten.Zulassungskreis);

            if (String.IsNullOrEmpty(Zulassung.Zulassungsdaten.Wunschkennzeichen3) || Zulassung.Zulassungsdaten.Wunschkennzeichen3.EndsWith("-"))
                Zulassung.Zulassungsdaten.Wunschkennzeichen3 = String.Format("{0}-", Zulassung.Zulassungsdaten.Zulassungskreis);
        }

        public void DataMarkForRefreshHalterAdressen()
        {
            PropertyCacheClear(this, m => m.HalterAdressen);
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

            var kennz = model.Kennzeichen.NotNullOrEmpty().ToUpper();
            if (kennz != String.Format("{0}-", Zulassung.Zulassungsdaten.Zulassungskreis))
                Zulassung.Zulassungsdaten.Kennzeichen = kennz;
            else
                Zulassung.Zulassungsdaten.Kennzeichen = "";

            Zulassung.Zulassungsdaten.KennzeichenReserviert = model.KennzeichenReserviert;

            if (Zulassung.Zulassungsdaten.KennzeichenReserviert)
            {
                Zulassung.Zulassungsdaten.ReservierungsNr = model.ReservierungsNr;
                Zulassung.Zulassungsdaten.ReservierungsName = model.ReservierungsName;
                Zulassung.Zulassungsdaten.Wunschkennzeichen2 = "";
                Zulassung.Zulassungsdaten.Wunschkennzeichen3 = "";
            }
            else
            {
                Zulassung.Zulassungsdaten.ReservierungsNr = "";
                Zulassung.Zulassungsdaten.ReservierungsName = "";

                var wkz2 = model.Wunschkennzeichen2.NotNullOrEmpty().ToUpper();
                if (wkz2 != String.Format("{0}-", Zulassung.Zulassungsdaten.Zulassungskreis))
                    Zulassung.Zulassungsdaten.Wunschkennzeichen2 = wkz2;
                else
                    Zulassung.Zulassungsdaten.Wunschkennzeichen2 = "";

                var wkz3 = model.Wunschkennzeichen3.NotNullOrEmpty().ToUpper();
                if (wkz3 != String.Format("{0}-", Zulassung.Zulassungsdaten.Zulassungskreis))
                    Zulassung.Zulassungsdaten.Wunschkennzeichen3 = wkz3;
                else
                    Zulassung.Zulassungsdaten.Wunschkennzeichen3 = "";
            }

            Zulassung.OptionenDienstleistungen.ZulassungsartMatNr = Zulassung.Zulassungsdaten.ZulassungsartMatNr;

            var tempKg = Zulassung.OptionenDienstleistungen.KennzeichengroesseListForMatNr.FirstOrDefault(k => k.Groesse == "520x114");
            if (tempKg != null)
                Zulassung.OptionenDienstleistungen.KennzeichenGroesseId = tempKg.Id;
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

        public void DataMarkForRefresh()
        {
            ZulassungDataService.MarkForRefresh();
            Zulassung = new Vorgang
            {
                VkOrg = LogonContext.Customer.AccountingArea.ToString(),
                VkBur = LogonContext.Organization.OrganizationReference2,
                Vorerfasser = LogonContext.UserName,
                VorgangsStatus = "1"
            };
            Zulassung.OptionenDienstleistungen.InitDienstleistungen(ZulassungDataService.Zusatzdienstleistungen);

            Rechnungsdaten.KundenList = Kunden;
            Fahrzeugdaten.FahrzeugartList = Fahrzeugarten;
            Adresse.Laender = LaenderList;
            Zulassungsdaten.MaterialList = Zulassungsarten;
            OptionenDienstleistungen.KennzeichengroesseList = Kennzeichengroessen;

            PartnerDataService.MarkForRefreshAdressen();

            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);
        }

        public void Save(bool saveDataInSap)
        {
            SaveErrorMessage = ZulassungDataService.SaveZulassung(saveDataInSap);
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

        [XmlIgnore]
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
                            },

                            new GeneralEntity
                            {
                                Title = Localize.DataForEndCustomerInvoice,
                                Body = Zulassung.BankAdressdaten.GetSummaryString(),
                            }
                        )
            };

            return summaryModel;
        }

        #endregion
    }
}
