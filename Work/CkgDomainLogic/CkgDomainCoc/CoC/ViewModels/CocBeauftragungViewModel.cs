using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.CoC.ViewModels
{
    public class CocBeauftragungViewModel : CkgBaseViewModel
    {
        public CocBeauftragungMode Mode { get { return PropertyCacheGet(() => CocBeauftragungMode.Versand); } set { PropertyCacheSet(value); } }

        [XmlIgnore]
        public ICocErfassungDataService CocErfassungDataService { get { return CacheGet<ICocErfassungDataService>(); } }

        [XmlIgnore]
        public IAdressenDataService AdressenDataService { get { return CacheGet<IAdressenDataService>(); } }

        [XmlIgnore]
        public IBriefVersandDataService BriefVersandDataService { get { return CacheGet<IBriefVersandDataService>(); } }

        [XmlIgnore]
        public IZulassungDataService ZulassungDataService { get { return CacheGet<IZulassungDataService>(); } }

        [XmlIgnore]
        public IDictionary<string, string> Steps
        {
            get { return PropertyCacheGet(() => new Dictionary<string, string>()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public IDictionary<string, string> StepsVersandBeauftragung
        {
            get
            {
                return PropertyCacheGet(() =>
                {
                    var dict = XmlService.XmlDeserializeFromFile<XmlDictionary<string, string>>(Path.Combine(AppSettings.DataPath, @"StepsVersandBeauftragung.xml"));
                    if (ParamVins.IsNotNullOrEmpty())
                        dict.Remove("FahrzeugAuswahl");

                    return dict;
                });
            }
        }

        [XmlIgnore]
        public IDictionary<string, string> StepsZulassung
        {
            get
            {
                return PropertyCacheGet(() =>
                {
                    var dict = XmlService.XmlDeserializeFromFile<XmlDictionary<string, string>>(Path.Combine(AppSettings.DataPath, @"StepsZulassung.xml"));
                    if (ParamVins.IsNotNullOrEmpty())
                        dict.Remove("FahrzeugAuswahl");

                    return dict;
                });
            }
        }

        [XmlIgnore]
        public IDictionary<string, string> StepsFreieZulassung
        {
            get
            {
                return PropertyCacheGet(() =>
                {
                    var dict = XmlService.XmlDeserializeFromFile<XmlDictionary<string, string>>(Path.Combine(AppSettings.DataPath, @"StepsZulassungFrei.xml"));

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

        public string FirstStepErrorHint
        {
            get
            {
                if (ParamVins.IsNotNullOrEmpty() && SelectedCocAuftraege.None())
                    return string.Format("Die Fahrzeug VIN(s) {0} ist/sind in unserem System nicht vorhanden.", ParamVins);

                return null;
            }
        }

        [XmlIgnore]
        public string ParamVins { get; private set; }

        [XmlIgnore]
        [DisplayName("Spaltenmodus")]
        public string UserLogonLevelAsString { get { return UserLogonLevel.ToString("F"); } }

        [XmlIgnore]
        public LogonLevel UserLogonLevel { get { return LogonContext.UserLogonLevel; } }

        [XmlIgnore]
        public string Title
        {
            get
            {
                switch (Mode)
                {
                    case CocBeauftragungMode.Versand:
                        return "CoC / ZBII";
                    case CocBeauftragungMode.VersandDuplikat:
                        return "CoC";
                    case CocBeauftragungMode.Zulassung:
                        return "CoC / ZBII";
                    case CocBeauftragungMode.FreieZulassung:
                        return "Freie Zulassung";
                }
                return "";
            }
        }

        [XmlIgnore]
        public string TitleSmall
        {
            get
            {
                switch (Mode)
                {
                    case CocBeauftragungMode.Versand:
                        return "Versandbeauftragung";
                    case CocBeauftragungMode.VersandDuplikat:
                        return "Druck Duplikat";
                    case CocBeauftragungMode.Zulassung:
                        return "Zulassung";
                    case CocBeauftragungMode.FreieZulassung:
                        return "";
                }
                return "";
            }
        }


        #region Step "Fahrzeugwahl" (Auswahl Coc-Auftrag)

        [XmlIgnore]
        public List<CocEntity> CocAuftraege
        {
            get
            {
                Predicate<CocEntity> cocSelector = c =>
                    {
                        if (Mode == CocBeauftragungMode.VersandDuplikat)
                            return c.AUFTRAG_DAT != null && (c.COC_DRUCK_ORIG.IsNotNullOrEmpty() || c.COC_KD_ORIG.IsNotNullOrEmpty());

                        return c.AUFTRAG_DAT == null;
                    };

                return CocErfassungDataService.CocAuftraege.Where(c => cocSelector(c)).ToList();
            }
        }

        [XmlIgnore]
        public List<CocEntity> CocAuftraegeFiltered
        {
            get { return PropertyCacheGet(() => CocAuftraege); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Land> LaenderList { get { return CocErfassungDataService.Laender; } }

        [XmlIgnore]
        public List<VersandOption> VersandOptionenList { get { return CocErfassungDataService.VersandOptionen; } }

        [XmlIgnore]
        public List<ZulassungsOption> ZulassungsOptionenList { get { return CocErfassungDataService.ZulassungsOptionen; } }

        [XmlIgnore]
        public List<CocEntity> SelectedCocAuftraege { get { return CocAuftraege.Where(c => c.IsSelected).ToList(); } }

        [XmlIgnore]
        public string SelectedCocAuftraegeAsString { get { return string.Join(", ", SelectedCocAuftraege.Select(c => c.VIN)); } }
        
        [XmlIgnore]
        private string PrevSelectedCocAuftraegeAsString { get; set; }

        [XmlIgnore]
        public string SaveErrorMessage { get; private set; }

        public string FahrzeugAuswahlTitleHint { get { return (Mode == CocBeauftragungMode.VersandDuplikat ? Localize.PleaseChooseOneVehicle : Localize.PleaseChooseOneOrMoreVehicles); } }

        public bool FahrzeugAuswahlSingleSelection { get { return (Mode == CocBeauftragungMode.VersandDuplikat); } }

        #endregion


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
                if (Mode == CocBeauftragungMode.FreieZulassung)
                    return (Wunschkennzeichen != null && Wunschkennzeichen.WunschkennzeichenList != null && Wunschkennzeichen.WunschkennzeichenList.Any() 
                                ? Wunschkennzeichen.WunschkennzeichenList.First().UniqueKey : "");

                return string.Format("{0}: {1}", PrevSelectedCocAuftraegeAsString.Contains(",") ? "FIN's" : "FIN", PrevSelectedCocAuftraegeAsString);
            }
        }

        public void SetFreieZulassungsOption(FreieZulassung model)
        {
            FreieZulassungsOption = model;
            DataMarkForRefreshWunschkennzeichen();
        }

        #endregion


        #region Step "Druck"

        public DruckOptionen DruckOptionen
        {
            get { return PropertyCacheGet(() => new DruckOptionen { FolgeDienstleistungen = "versand" }); }
            set { PropertyCacheSet(value); }
        }

        #endregion


        #region Step "Versandadresse"

        [XmlIgnore]
        public List<Adresse> VersandAdressen
        {
            get { return AdressenDataService.Adressen.Where(a => a.Kennung == GetAdressKennung("Versand")).ToListOrEmptyList(); }
        }

        [XmlIgnore]
        public List<string> VersandAdressenAsAutoCompleteItems
        {
            get { return VersandAdressen.Select(a => a.GetAutoSelectString()).ToList(); }
        }

        [XmlIgnore]
        public List<Adresse> VersandAdressenFiltered
        {
            get { return PropertyCacheGet(() => VersandAdressen); }
            private set { PropertyCacheSet(value); }
        }

        public Adresse VersandAdresse
        {
            get { return PropertyCacheGet(() => new Adresse { Land = "DE", Kennung = GetAdressKennung("Versand") }); }
            set { PropertyCacheSet(value); }
        }

        public Adresse GetVersandAdresseFromKey(string key)
        {
            int id;
            if (Int32.TryParse(key, out id))
                return VersandAdressen.FirstOrDefault(v => v.ID == id);

            return VersandAdressen.FirstOrDefault(a => a.GetAutoSelectString() == key);
        }

        [XmlIgnore]
        public string LastCocPdfFileName { get; private set; }

        [XmlIgnore]
        public Dictionary<string, DateTime?> CocSelbstDruckAuftraege { get { return PropertyCacheGet(() => new Dictionary<string, DateTime?>()); } }

        public byte[] GetCocAsPdf(string vnr, string vorlage)
        {
            var cocAuftrag = CocAuftraege.FirstOrDefault(c => c.VORG_NR == vnr);
            if (cocAuftrag == null)
                return null;

            if (CocSelbstDruckAuftraege.ContainsKey(vnr))
                CocSelbstDruckAuftraege.Remove(vnr);

            LastCocPdfFileName = string.Format("COC_{0}.pdf", cocAuftrag.VIN);

            var pdfBytes = CocErfassungDataService.GetCocAsPdf(cocAuftrag.VORG_NR, vorlage);
            if (pdfBytes != null)
            {
                if (Mode == CocBeauftragungMode.VersandDuplikat)
                    CocErfassungDataService.SaveVersandDuplikatDruckBeauftragung(new List<CocEntity> { cocAuftrag }, DruckOptionen);
                else
                    CocErfassungDataService.StoreCocPdfSelbstDruck(cocAuftrag, vorlage);

                CocSelbstDruckAuftraege.Add(vnr, DateTime.Now);
            }

            return pdfBytes;
        }

        public DateTime? CocSelbstDruckAuftragsDatum(string vnr)
        {
            if (!CocSelbstDruckAuftraege.ContainsKey(vnr))
                return null;

            return CocSelbstDruckAuftraege[vnr];
        }

        #endregion

        
        #region Step "Versand"

        public VersandOptionen VersandOptionen
        {
            get { return PropertyCacheGet(() => new VersandOptionen()); }
            set { PropertyCacheSet(value); }
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

        public void SetCocBeauftragungMode(CocBeauftragungMode mode)
        {
            Mode = mode;
            if (mode == CocBeauftragungMode.Versand || Mode == CocBeauftragungMode.VersandDuplikat)
                Steps = StepsVersandBeauftragung;
            if (mode == CocBeauftragungMode.Zulassung)
                Steps = StepsZulassung;
            if (mode == CocBeauftragungMode.FreieZulassung)
                Steps = StepsFreieZulassung;

            DruckOptionen.SetBeauftragungMode(Mode);
        }

        public void DataMarkForRefresh(string vins)
        {
            ZulassungDataService.AuftragsNummer = null;

            Adresse.Laender = LaenderList;
            VersandOptionen.OptionenList = VersandOptionenList;
            ZulassungsOptionen.OptionenList = ZulassungsOptionenList;

            CocErfassungDataService.MarkForRefreshCocOrders();
            AdressenDataService.MarkForRefreshAdressen();

            // reset filtered data
            PropertyCacheClear(this, m => m.CocAuftraegeFiltered);
            DataMarkForRefreshVersandAdressenFiltered();

            ParamVins = vins;
            if (ParamVins.IsNotNullOrEmpty())
                ParamVins.Split(',').ToList().ForEach(vin => TrySelectCocVIN(vin.Trim()));
            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepsVersandBeauftragung);
            PropertyCacheClear(this, m => m.StepsZulassung);
            PropertyCacheClear(this, m => m.StepsFreieZulassung);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);

            DataMarkForRefreshWunschkennzeichen();
            PropertyCacheClear(this, m => m.ZulassungsDienstleistungen);

            PropertyCacheClear(this, m => m.FreieZulassungsOption);

            //XmlService.XmlSerializeToFile(StepsZulassung, Path.Combine(AppSettings.DataPath, @"StepsZulassung.xml"));
            //XmlService.XmlSerializeToFile(StepsCocBeauftragung, Path.Combine(AppSettings.DataPath, @"StepsCocBeauftragung.xml"));
        }

        public void DataMarkForRefreshWunschkennzeichen()
        {
            PropertyCacheClear(this, m => m.Wunschkennzeichen);
        }

        public void DataMarkForRefreshVersandAdressenFiltered()
        {
            PropertyCacheClear(this, m => m.VersandAdressenFiltered);
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            CocAuftraegeFiltered = CocAuftraege.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterVersandAdressen(string filterValue, string filterProperties)
        {
            VersandAdressenFiltered = VersandAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void TrySelectCocVIN(string vin)
        {
            var cocAuftrag = CocAuftraege.FirstOrDefault(f => f.VIN.NotNullOrEmpty().ToLower() == vin.NotNullOrEmpty().ToLower());
            if (cocAuftrag == null)
                return;

            cocAuftrag.IsSelected = true;
        }

        public void SelectCocAuftrag(int fahrzeugID, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var cocAuftrag = CocAuftraege.FirstOrDefault(f => f.ID == fahrzeugID);
            if (cocAuftrag == null)
                return;

            allSelectionCount = CocAuftraege.Count(c => c.IsSelected);

            // enforce single selection:
            if (FahrzeugAuswahlSingleSelection)
                if (select && allSelectionCount == 1)
                    CocAuftraege.ForEach(c => c.IsSelected = false);

            cocAuftrag.IsSelected = select;
            allSelectionCount = CocAuftraege.Count(c => c.IsSelected);
        }

        VersandAuftragsAnlage CreateVersandAuftrag(string vin, string stuecklistenCode)
        {
            var versandAuftrag = new VersandAuftragsAnlage
            {
                KundenNr = CocErfassungDataService.ToDataStoreKundenNr(LogonContext.KundenNr),
                VIN = vin,
                BriefVersand = true,
                SchluesselVersand = false,
                StuecklistenKomponente = stuecklistenCode,
                AbmeldeKennzeichen = true,
                AbcKennzeichen = "2",
                MaterialNr = VersandOptionen.VersandOption.MaterialCode,
                DadAnforderungsDatum = DateTime.Today,
                ErfassungsUserName = LogonContext.UserName,
                Bemerkung = VersandOptionen.Bemerkung,
            };

            ModelMapping.Copy(VersandAdresse, versandAuftrag);

            return versandAuftrag;
        }

        public void Save()
        {
            SaveErrorMessage = "";

            if (Mode == CocBeauftragungMode.Versand || Mode == CocBeauftragungMode.VersandDuplikat)
                SaveVersandBeauftragung();

            if (Mode == CocBeauftragungMode.Zulassung || Mode == CocBeauftragungMode.FreieZulassung)
                SaveZulassung();
        }

        private void SaveZulassung()
        {
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

            var errorMessageCoc = "";
            if (Mode != CocBeauftragungMode.FreieZulassung && errorMessageZulassung.IsNullOrEmpty())
                // 2. Zulassungs-Info und Auftragsdatum in COC_01 Tabelle setzen:
                errorMessageCoc = CocErfassungDataService.SaveZulassung(SelectedCocAuftraege, DruckOptionen, HalterAdresse.Land);

            // 3. Fehlermeldungen kombinieren + im ViewModel persistieren
            SaveErrorMessage = string.Join(";;", new StringListNotEmpty(errorMessageZulassung, errorMessageCoc));
        }

        private void SaveVersandBeauftragung()
        {
            // 1. Versandauftrags-Datensätze anlegen (Druck-Datensätze)
            var versandAuftraege = new List<VersandAuftragsAnlage>();
            var errorMessageBriefVersand = "";

            if (!DruckOptionen.ModusEigenDruck)
            {
                SelectedCocAuftraege.ForEach(cocAuftrag =>
                    {
                        if (DruckOptionen.DruckCoc)
                            versandAuftraege.Add(CreateVersandAuftrag(cocAuftrag.VIN, "720"));

                        if (DruckOptionen.DruckZBII)
                            versandAuftraege.Add(CreateVersandAuftrag(cocAuftrag.VIN, "722"));
                    });

                errorMessageBriefVersand = BriefVersandDataService.SaveVersandBeauftragung(versandAuftraege);
            }
 
           
            // 2. Druck-Info und Auftragsdatum in COC_01 Tabelle setzen:
            var errorMessageCoc = "";
            if (errorMessageBriefVersand.IsNullOrEmpty())
                if (Mode == CocBeauftragungMode.VersandDuplikat)
                    errorMessageCoc = CocErfassungDataService.SaveVersandDuplikatDruckBeauftragung(SelectedCocAuftraege, DruckOptionen);
                else
                    errorMessageCoc = CocErfassungDataService.SaveVersandBeauftragung(SelectedCocAuftraege, DruckOptionen);

            
            // 3. Fehlermeldungen kombinieren + im ViewModel persistieren
            SaveErrorMessage = string.Join(";;", new StringListNotEmpty(errorMessageBriefVersand, errorMessageCoc));
        }

        public GeneralSummary CreateSummaryModel(bool cacheOriginItems, Func<string, string> getAdressenSelectionLinkFunction)
        {
            if (Mode == CocBeauftragungMode.Versand || Mode == CocBeauftragungMode.VersandDuplikat)
                return CreateSummaryForVersand(cacheOriginItems);

            return CreateSummaryForZulassung(cacheOriginItems, getAdressenSelectionLinkFunction);
        }

        private GeneralSummary CreateSummaryForZulassung(bool pdfMode, Func<string, string> getAdressenSelectionLinkFunction)
        {
            if (!pdfMode)
                PrevSelectedCocAuftraegeAsString = SelectedCocAuftraegeAsString;

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
                            
                            (Mode == CocBeauftragungMode.FreieZulassung ? null : 
                                new GeneralEntity
                                    {
                                        Title = Localize.PrintOptions,
                                        Body = DruckOptionen.GetSummaryString(),
                                    }),
                            
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

        private GeneralSummary CreateSummaryForVersand(bool pdfMode)
        {
            if (!pdfMode)
                PrevSelectedCocAuftraegeAsString = SelectedCocAuftraegeAsString;

            var summaryModel = new GeneralSummary
            {
                Header = "Auftragsübersicht CoC / ZBII Beauftragung",
                Items = new ListNotEmpty<GeneralEntity>
                        (
                            SummaryBeauftragungsHeader,

                            new GeneralEntity
                            {
                                Title = Localize.PrintOptions,
                                Body = DruckOptionen.GetSummaryString(),
                            },

                            new GeneralEntity
                            {
                                Title = Localize.ShippingAddress,
                                Body = VersandAdresse.GetPostLabelString(),
                            },
                            
                            new GeneralEntity
                                {
                                    Title = Localize.ShippingOptions, 
                                    Body = VersandOptionen.GetSummaryString(),
                                },
                            
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
                    dienstleistungen.InitDienstleistungen(CocErfassungDataService.ZulassungsDienstleistungen);
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
                    WunschkennzeichenList = 
                        (Mode == CocBeauftragungMode.FreieZulassung
                            ? new List<VinWunschkennzeichen>
                            { 
                                new VinWunschkennzeichen
                                {
                                    VIN = FreieZulassungsOption.VIN,
                                    ZBII = FreieZulassungsOption.ZBII,
                                    AuftragsReferenz = FreieZulassungsOption.AuftragsReferenz,
                                }
                            } 
                            : SelectedCocAuftraege.Select(cocAuftrag => new VinWunschkennzeichen { VIN = cocAuftrag.VIN }).ToList()
                        )
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
        public Adresse ReguliererAdresse            // default = AuftraggeberAdresse
        {
            get { return PropertyCacheGet(() => CopyAdresse(AuftraggeberAdresse, "Regulierer")); }
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
        public Adresse RechnungsEmpfaengerAdresse            // default = AuftraggeberAdresse
        {
            get { return PropertyCacheGet(() => CopyAdresse(AuftraggeberAdresse, "RechnungsEmpfaenger")); }
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
