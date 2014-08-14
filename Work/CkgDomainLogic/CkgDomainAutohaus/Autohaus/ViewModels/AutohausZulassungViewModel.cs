using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    public class AutohausZulassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IAdressenDataService AdressenDataService { get { return CacheGet<IAdressenDataService>(); } }

        [XmlIgnore]
        public IFahrzeugakteDataService FahrzeugakteDataService { get { return CacheGet<IFahrzeugakteDataService>(); } }

        [XmlIgnore]
        public IAutohausZulassungDataService ZulassungDataService { get { return CacheGet<IAutohausZulassungDataService>(); } }

        [XmlIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() =>
                {
                    var dict = XmlService.XmlDeserializeFromFile<XmlDictionary<string, string>>(Path.Combine(AppSettings.DataPath, @"StepsAutohausZulassung.xml"));

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
        [DisplayName("Spaltenmodus")]
        public string UserLogonLevelAsString { get { return UserLogonLevel.ToString("F"); } }

        [XmlIgnore]
        public LogonLevel UserLogonLevel { get { return LogonContext.UserLogonLevel; } }

        [XmlIgnore]
        public List<Land> LaenderList { get { return ZulassungDataService.Laender; } }

        [XmlIgnore]
        public List<ZulassungsOption> ZulassungsOptionenList { get { return ZulassungDataService.ZulassungsOptionen; } }

        [XmlIgnore]
        public string SaveErrorMessage { get; private set; }


        #region Freie Zulassungs-Optionen

        public FreieZulassung FreieZulassungsOption
        {
            get { return PropertyCacheGet(() => new FreieZulassung()); }
            set { PropertyCacheSet(value); }
        }

        public string BeauftragungBezeichnung
        {
            get
            {
                return (Wunschkennzeichen != null && Wunschkennzeichen.WunschkennzeichenList != null && Wunschkennzeichen.WunschkennzeichenList.Any()
                                ? Wunschkennzeichen.WunschkennzeichenList.First().UniqueKey : "");
            }
        }

        public void SetFreieZulassungsOption(FreieZulassung model)
        {
            FreieZulassungsOption = model;
            DataMarkForRefreshWunschkennzeichen();
        }

// ReSharper disable RedundantNameQualifier
        public void SetFreieZulassungsOption(Autohaus.Models.Fahrzeug fahrzeugDaten)
// ReSharper restore RedundantNameQualifier
        {
            FreieZulassungsOption = new FreieZulassung{ FahrzeugID = fahrzeugDaten.ID, VIN = fahrzeugDaten.FahrgestellNr, AuftragsReferenz = fahrzeugDaten.ReferenzNr };
            DataMarkForRefreshWunschkennzeichen();
        }

        #endregion


        #region Misc + Summaries + Savings

        private GeneralEntity SummaryFooterUserInformation
        {
            get
            {
                return new GeneralEntity
                {
                    Title = "Datum, User, Kunde",
                    Body = string.Format("{0}<br/>{1} (#{2})<br/>{3}",
                                         DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                                         LogonContext.UserName,
                                         LogonContext.Customer.Customername, LogonContext.KundenNr)
                };
            }
        }

        private GeneralEntity SummaryBeauftragungsHeader
        {
            get
            {
                return new GeneralEntity
                    {
                        Title = "Ihre Beauftragung",
                        Body = BeauftragungBezeichnung,
                        Tag = "SummaryMainItem"
                    };
            }
        }

        public void DataMarkForRefresh()
        {
            ZulassungDataService.AuftragsNummer = null;

            Adresse.Laender = LaenderList;
            ZulassungsOptionen.OptionenList = ZulassungsOptionenList;

            AdressenDataService.MarkForRefreshAdressen();

            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);

            DataMarkForRefreshWunschkennzeichen();
            PropertyCacheClear(this, m => m.ZulassungsDienstleistungen);

            PropertyCacheClear(this, m => m.FreieZulassungsOption);
        }

        public void DataMarkForRefreshWunschkennzeichen()
        {
            PropertyCacheClear(this, m => m.Wunschkennzeichen);
        }

        public void Save()
        {
            SaveErrorMessage = "";

            // 1. Zulassungs-Prozess anstoßen 
            var errorMessageZulassung = ZulassungDataService.SaveZulassung(
                    AuftraggeberAdresse,    
                    HalterAdresse,
                    ReguliererAdresse,
                    RechnungsEmpfaengerAdresse,
                    VersicherungsNehmerAdresse,
                    VersandScheinSchilderAdresse,
                    VersandZb2CocAdresse,

                    ZulassungsOption,
                    ZulassungsDienstleistungen,
                    Versicherungsdaten,
                    Wunschkennzeichen
                );

            // 2. ggf. Beauftragte Zulassung in SQL speichern
            if (FreieZulassungsOption.FahrzeugID > 0 && String.IsNullOrEmpty(errorMessageZulassung))
            {
                FahrzeugakteDataService.BeauftragteZulassungSave(FreieZulassungsOption.FahrzeugID, FreieZulassungsOption.AuftragsReferenz, FreieZulassungsOption.VIN,
                    FreieZulassungsOption.ZBII, ZulassungsOption.AuslieferDatum);
            }

            // 3. Fehlermeldungen kombinieren + im ViewModel persistieren
            SaveErrorMessage = errorMessageZulassung;
        }

        public GeneralSummary CreateSummaryModel(bool pdfMode, Func<string, string> getAdressenSelectionLinkFunction)
        {
            var summaryModel = new GeneralSummary
            {
                Header = "Auftragsübersicht Fahrzeug Zulassung",
                Items = new ListNotEmpty<GeneralEntity>
                        (
                            (ZulassungDataService.AuftragsNummer.IsNullOrEmpty() ? null:
                                new GeneralEntity
                                    {
                                        Title = Localize.OrderID, 
                                        Body = ZulassungDataService.AuftragsNummer,
                                    }),

                            SummaryBeauftragungsHeader,

                            null,
                            
                            new GeneralEntity
                                {
                                    Title = Localize.Holder, 
                                    Body = HalterAdresse.GetPostLabelString(),
                                },
                            
                            new GeneralEntity
                                {
                                    Title = Localize.RegistrationOptions, 
                                    Body = ZulassungsOption.GetSummaryString(),
                                },

                            (Versicherungsdaten.GetSummaryString().IsNullOrEmpty() ? null :
                                new GeneralEntity
                                    {
                                        Title = Localize.InsuranceData, 
                                        Body = Versicherungsdaten.GetSummaryString(),
                                    }),

                            (ZulassungsDienstleistungen.GetSummaryString().IsNullOrEmpty() ? null :
                                new GeneralEntity
                                    {
                                        Title = Localize.Services, 
                                        Body = ZulassungsDienstleistungen.GetSummaryString(),
                                    }),
                            
                            new GeneralEntity
                                {
                                    Title = Localize.Regulator, 
                                    Body = ReguliererAdresse.GetPostLabelString() + (pdfMode ? "" : getAdressenSelectionLinkFunction("Regulierer")),
                                },
                            
                            new GeneralEntity
                                {
                                    Title = Localize.InvoiceRecipient, 
                                    Body = RechnungsEmpfaengerAdresse.GetPostLabelString() + (pdfMode ? "" : getAdressenSelectionLinkFunction("RechnungsEmpfaenger")),
                                },
                            
                            new GeneralEntity
                                {
                                    Title = Localize.InsuranceRecipient, 
                                    Body = VersicherungsNehmerAdresse.GetPostLabelString() + (pdfMode ? "" : getAdressenSelectionLinkFunction("VersicherungsNehmer")),
                                },
                            
                            new GeneralEntity
                                {
                                    Title = Localize.DeliveryZB1andSigns, 
                                    Body = VersandScheinSchilderAdresse.GetPostLabelString() + (pdfMode ? "" : getAdressenSelectionLinkFunction("VersandScheinSchilder")),
                                },
                            
                            new GeneralEntity
                                {
                                    Title = Localize.DeliveryZB2andCoc, 
                                    Body = VersandZb2CocAdresse.GetPostLabelString() + (pdfMode ? "" : getAdressenSelectionLinkFunction("VersandZb2Coc")),
                                },

                            (Wunschkennzeichen.GetSummaryString().IsNullOrEmpty() ? null :
                                new GeneralEntity
                                {
                                    Title = Localize.RequestSign,
                                    Body = Wunschkennzeichen.GetSummaryString(),
                                }),
                            
                            SummaryFooterUserInformation
                        )
            };

            return summaryModel;
        }

        #endregion


        #region Zulassung, Halter

        public ZulassungsOptionen ZulassungsOption
        {
            get { return PropertyCacheGet(() => new ZulassungsOptionen { Versicherung = new Versicherungsdaten() }); }
            set { PropertyCacheSet(value); }
        }

        public ZulassungsDienstleistungen ZulassungsDienstleistungen
        {
            get
            {
                return PropertyCacheGet(() =>
                {
                    var dienstleistungen = new ZulassungsDienstleistungen();
                    dienstleistungen.InitDienstleistungen(ZulassungDataService.ZulassungsDienstleistungen);
                    return dienstleistungen;
                });
            }
            set { PropertyCacheSet(value); }
        }

        public Versicherungsdaten Versicherungsdaten
        {
            get { return ZulassungsOption.Versicherung; }
            set { ZulassungsOption.Versicherung = value; } 
        }

        public void DataMarkForRefreshAdressenFiltered()
        {
            PropertyCacheClear(this, m => m.HalterAdressenFiltered);
        }

        public string LoadKfzKreisAusHalterAdresse()
        {
            return HalterAdresse == null ? "" : AdressenDataService.GetZulassungskreisFromPostcodeAndCity(HalterAdresse.PLZ, HalterAdresse.Ort);
        }

        public void SetHalterAdresse(Adresse model)
        {
            HalterAdresse = model;

            // Default Adressen vorgeben
            VersicherungsNehmerAdresse = CopyAdresse(HalterAdresse, "VersicherungsNehmer");
            VersandScheinSchilderAdresse = CopyAdresse(HalterAdresse, "VersandScheinSchilder");
            VersandZb2CocAdresse = CopyAdresse(HalterAdresse, "VersandZb2Coc");

            ZulassungsOption.ZulassungsKreis = LoadKfzKreisAusHalterAdresse();

            Wunschkennzeichen.SetZulassungsKreis(ZulassungsOption.ZulassungsKreis);
        }

        Adresse CopyAdresse(Adresse model, string adressTyp)
        {
            return ModelMapping.Copy(model, (source, destination) =>
            {
                destination.Typ = adressTyp;
                destination.Kennung = GetAdressKennung(adressTyp);
            });
        }

        #endregion  


        #region Wunschkennzeichen

        [XmlIgnore]
        public WunschkennzeichenOptionen Wunschkennzeichen
        {
            get 
            { 
                return PropertyCacheGet(() => new WunschkennzeichenOptionen
                {
                    WunschkennzeichenList = new List<VinWunschkennzeichen>
                            { 
                                new VinWunschkennzeichen
                                {
                                    VIN = FreieZulassungsOption.VIN,
                                    ZBII = FreieZulassungsOption.ZBII,
                                    AuftragsReferenz = FreieZulassungsOption.AuftragsReferenz,
                                }
                            }
                });  
            }
            set { PropertyCacheSet(value); }
        }

        #endregion


        #region Dienstleistungen

        public void SaveZulassungsDienstleistungen(ZulassungsDienstleistungen model)
        {
            ZulassungsDienstleistungen.GewaehlteDienstleistungenString = model.GewaehlteDienstleistungenString;
        }

        #endregion

        
        #region Versicherungsdaten

        private const string VersicherungsAdressenKennung = "VERSICHERER";

        [XmlIgnore]
        public List<Adresse> VersicherungsAdressen
        {
            get { return AdressenDataService.Adressen.Where(a => a.Kennung == VersicherungsAdressenKennung).ToListOrEmptyList(); }
        }

        [XmlIgnore]
        public List<string> VersicherungsAdressenAsAutoCompleteItems
        {
            get { return VersicherungsAdressen.Select(a => a.Name1).ToList(); }
        }

        public void SaveVersicherungsdaten(Versicherungsdaten model)
        {
            Versicherungsdaten = model;
        }

        #endregion


        #region Adressen Typen

        [XmlIgnore]
        public Dictionary<string, string> AdressTypen = new Dictionary<string, string>
            {
                {"Versand", "VERSANDADRESSE"},
                
                {"VersandScheinSchilder", "VERSANDADRESSE"},
                {"VersandZb2Coc", "VERSANDADRESSE"},
                {"Halter", "HALTER"},
                {"Versicherer", "VERSICHERER"},
                {"VersicherungsNehmer", "SSCHILDER"},

                {"RechnungsEmpfaenger", "RE"},
                {"Regulierer", "RG"},
            };

        //
        // AuftraggeberAdresse
        //
        [XmlIgnore]
        public Adresse AuftraggeberAdresse
        {
            get { return AdressenDataService.AgAdresse; }
        }

        //
        // HalterAdresse
        //
        [XmlIgnore]
        public Adresse HalterAdresse
        {
            get { return PropertyCacheGet(() => new Adresse { Land = "DE", Kennung = GetAdressKennung("Halter") }); }
            set { PropertyCacheSet(value); }
        }
        [XmlIgnore]
        public List<Adresse> HalterAdressenFiltered
        {
            get { return PropertyCacheGet(() => GetAdressen("Halter")); }
            private set { PropertyCacheSet(value); }
        }
        public void FilterHalterAdressen(string filterValue, string filterProperties)
        {
            HalterAdressenFiltered = GetAdressen("Halter").SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        //
        // ReguliererAdresse
        //
        [XmlIgnore]
        public Adresse ReguliererAdresse            // default = default aus SAP, sonst AuftraggeberAdresse
        {
            get { return PropertyCacheGet(() => (ReguliererAdressenFiltered.Any(a => a.IsDefaultPartner) ? ReguliererAdressenFiltered.First(a => a.IsDefaultPartner) : CopyAdresse(AuftraggeberAdresse, "Regulierer"))); }
            set { PropertyCacheSet(value); }
        }
        [XmlIgnore]
        public List<Adresse> ReguliererAdressenFiltered
        {
            get { return PropertyCacheGet(() => AdressenDataService.RgAdressen); }
            private set { PropertyCacheSet(value); }
        }
        public void FilterReguliererAdressen(string filterValue, string filterProperties)
        {
            ReguliererAdressenFiltered = AdressenDataService.RgAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        //
        // RechnungsEmpfaengerAdresse
        //
        [XmlIgnore]
        public Adresse RechnungsEmpfaengerAdresse            // default = default aus SAP, sonst AuftraggeberAdresse
        {
            get { return PropertyCacheGet(() => (RechnungsEmpfaengerAdressenFiltered.Any(a => a.IsDefaultPartner) ? RechnungsEmpfaengerAdressenFiltered.First(a => a.IsDefaultPartner) : CopyAdresse(AuftraggeberAdresse, "RechnungsEmpfaenger"))); }
            set { PropertyCacheSet(value); }
        }
        [XmlIgnore]
        public List<Adresse> RechnungsEmpfaengerAdressenFiltered
        {
            get { return PropertyCacheGet(() => AdressenDataService.ReAdressen); }
            private set { PropertyCacheSet(value); }
        }
        public void FilterRechnungsEmpfaengerAdressen(string filterValue, string filterProperties)
        {
            RechnungsEmpfaengerAdressenFiltered = AdressenDataService.ReAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        //
        // VersicherungsNehmerAdresse
        //
        [XmlIgnore]
        public Adresse VersicherungsNehmerAdresse   // default = Halter
        {
            get { return PropertyCacheGet(() => CopyAdresse(HalterAdresse, "VersicherungsNehmer")); }
            set { PropertyCacheSet(value); }
        }

        //
        // VersandScheinSchilderAdresse
        //
        [XmlIgnore]
        public Adresse VersandScheinSchilderAdresse
        {
            get { return PropertyCacheGet(() => new Adresse { Land = "DE", Kennung = GetAdressKennung("VersandScheinSchilder") }); }
            set { PropertyCacheSet(value); }
        }
        [XmlIgnore]
        public List<Adresse> VersandScheinSchilderAdressenFiltered
        {
            get { return PropertyCacheGet(() => GetAdressen("VersandScheinSchilder")); }
            private set { PropertyCacheSet(value); }
        }
        public void FilterVersandScheinSchilderAdressen(string filterValue, string filterProperties)
        {
            VersandScheinSchilderAdressenFiltered = GetAdressen("VersandScheinSchilder").SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        //
        // VersandZb2CocAdresse
        //
        [XmlIgnore]
        public Adresse VersandZb2CocAdresse
        {
            get { return PropertyCacheGet(() => new Adresse { Land = "DE", Kennung = GetAdressKennung("VersandZb2Coc") }); }
            set { PropertyCacheSet(value); }
        }
        [XmlIgnore]
        public List<Adresse> VersandZb2CocAdressenFiltered
        {
            get { return PropertyCacheGet(() => GetAdressen("VersandZb2Coc")); }
            private set { PropertyCacheSet(value); }
        }
        public void FilterVersandZb2CocAdressen(string filterValue, string filterProperties)
        {
            VersandZb2CocAdressenFiltered = GetAdressen("VersandZb2Coc").SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
        
        #endregion

        
        #region Adressen Helpers

        List<Adresse> GetAdressen(string adressenTyp)
        {
            var list = AdressenDataService.Adressen.Where(a => a.Kennung == GetAdressKennung(adressenTyp)).ToListOrEmptyList();
            list.ForEach(a => a.Typ = GetAdressTyp(a.Kennung));
            return list;
        }

        public List<string> GetAdressenAsAutoCompleteItems(string adressenTyp)
        {
            return GetAdressen(adressenTyp).Select(a => a.GetAutoSelectString()).ToList(); 
        }

        public Adresse GetAdresseFromKey(string adressenTyp, string key)
        {
            int id;
            if (Int32.TryParse(key, out id))
                return GetAdressen(adressenTyp).FirstOrDefault(v => v.ID == id);

            return GetAdressen(adressenTyp).FirstOrDefault(a => a.GetAutoSelectString() == key);
        }

        public void SetAdresse(Adresse model)
        {
            ModelMapping.Copy(model, GetAdresseFromType(model.Typ), (source, destination) => { });
        }

        public void SummaryUpdateAddressFromGrid(int id, string addressType)
        {
            switch (addressType)
            {
                case "Regulierer":
                    ReguliererAdresse = AdressenDataService.RgAdressen.FirstOrDefault(a => a.ID == id);
                    break;
                case "RechnungsEmpfaenger":
                    RechnungsEmpfaengerAdresse = AdressenDataService.ReAdressen.FirstOrDefault(a => a.ID == id);
                    break;

                case "VersandScheinSchilder":
                    VersandScheinSchilderAdresse = CopyAdresse(GetAdressen(addressType).FirstOrDefault(a => a.ID == id), addressType);
                    break;

                case "VersandZb2Coc":
                    VersandZb2CocAdresse = CopyAdresse(GetAdressen(addressType).FirstOrDefault(a => a.ID == id), addressType);
                    break;
            }
        }

        public Adresse GetAdresseFromType(string addressType)
        {
            switch (addressType)
            {
                case "Halter":
                    return HalterAdresse;

                case "Regulierer":
                    return ReguliererAdresse;

                case "RechnungsEmpfaenger":
                    return RechnungsEmpfaengerAdresse;

                case "VersicherungsNehmer":
                    return VersicherungsNehmerAdresse;

                case "VersandScheinSchilder":
                    return VersandScheinSchilderAdresse;

                case "VersandZb2Coc":
                    return VersandZb2CocAdresse;
            }

            return null;
        }

        public string GetAdressKennung(string adressTyp)
        {
            return AdressTypen[adressTyp];
        }

        public string GetAdressTyp(string adressKennung)
        {
            var adressTypen = AdressTypen.Where(t => t.Value == adressKennung);
            if (adressTypen.None())
                return "";
            
            return adressTypen.First().Key;
        }

        public bool SummaryAdressEditAvailable(string addressType)
        {
            switch (addressType)
            {
                case "VersicherungsNehmer":
                case "VersandScheinSchilder":
                case "VersandZb2Coc":
                    return true;
            }

            return false;
        }

        public bool SummaryAdressSelectionAvailable(string addressType)
        {
            switch (addressType)
            {
                case "VersicherungsNehmer":
                    return false;
            }

            return true;
        }

        #endregion
    }
}
