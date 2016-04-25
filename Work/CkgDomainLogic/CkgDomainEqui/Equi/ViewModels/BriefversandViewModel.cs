using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class BriefversandViewModel : CkgBaseViewModel
    {
        public BriefversandModus VersandModus
        {
            get { return PropertyCacheGet(() => BriefversandModus.Brief); } 
            set
            {
                PropertyCacheSet(value);
                BriefbestandDataService.FahrzeugbriefeZumVersandModus = value;
            }
        }  

        [XmlIgnore]
        public IBriefbestandDataService BriefbestandDataService { get { return CacheGet<IBriefbestandDataService>(); } }

        [XmlIgnore]
        public IAdressenDataService AdressenDataService { get { return CacheGet<IAdressenDataService>(); } }

        [XmlIgnore]
        public IBriefVersandDataService BriefVersandDataService { get { return CacheGet<IBriefVersandDataService>(); } }

        [XmlIgnore]
        public IDictionary<string, string> Steps
        {
            get
            {
                return PropertyCacheGet(() =>
                {
                    var dict = XmlService.XmlDeserializeFromFile<XmlDictionary<string, string>>(Path.Combine(AppSettings.DataPath, @"StepsBriefversand.xml"));

                    if (VersandModus != BriefversandModus.Stueckliste)
                    {
                        dict.Remove("EquiSuche");
                        dict.Remove("Stueckliste");
                    }

                    if (ParamVins.IsNotNullOrEmpty())
                        dict.Remove("FahrzeugAuswahl");

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
                if (ParamVins.IsNotNullOrEmpty() && SelectedFahrzeuge.None())
                    return string.Format("Die Fahrzeug VIN(s) {0} ist/sind in unserem System nicht vorhanden.", ParamVins);

                return null;
            }
        }

        public string AppTitle
        {
            get
            {
                switch (VersandModus)
                {
                    case BriefversandModus.Brief:
                        return Localize.Equi_Briefversand;
                    
                    case BriefversandModus.Schluessel:
                        return Localize.Equi_Schluesselversand;

                    case BriefversandModus.BriefMitSchluessel:
                        return Localize.Equi_BriefSchluesselversand;

                    case BriefversandModus.Stueckliste:
                        return Localize.Equi_Stuecklistenversand;
                }

                return "";
            }
        }

        [XmlIgnore]
        public string ParamVins { get; private set; }

        public int CurrentAppID { get; set; }

        public bool TechnIdentnummerIsVisible { get { return VersandModus != BriefversandModus.Schluessel; } }

        
        #region Stuecklistenversand

        public EquiPartlistSelektor EquiPartlistSelektor
        {
            get { return PropertyCacheGet(() => new EquiPartlistSelektor()); }
            set { PropertyCacheSet(value); }
        }

        public List<Fahrzeugbrief> FahrzeugeForPartList { get; set; }

        public void LoadFahrzeugeForPartlist(Action<string, string> addModelError)
        {
            var fahrzeugBriefForSearch = EquiPartlistSelektor.ToFahrzeugbrief();
            var fahrzeugbriefe = BriefbestandDataService.GetFahrzeugBriefe(fahrzeugBriefForSearch)
                                                          .Where((f => !f.IsMissing))
                                                          .ToListOrEmptyList();
            if (fahrzeugbriefe.None())
            {
                addModelError("", Localize.NoDataFound);
                return;
            }

            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
            FahrzeugeForPartList = fahrzeugbriefe;

            if (FahrzeugeForPartList.Count == 1)
                FahrzeugeForPartList.First().IsSelected = true;
        }

        public List<StuecklistenKomponente> Stueckliste { get; set; }

        [XmlIgnore]
        public List<StuecklistenKomponente> SelectedStueckliste { get { return Stueckliste == null ? new List<StuecklistenKomponente>() : Stueckliste.Where(sl => sl.IsSelected).OrderBy(sl => sl.Fahrgestellnummer).ToListOrEmptyList(); } }

        public List<StuecklistenKomponente> StuecklisteFiltered
        {
            get { return PropertyCacheGet(() => Stueckliste); }
            set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefreshStueckliste()
        {
            PropertyCacheClear(this, m => m.StuecklisteFiltered);
        }

        public void LoadStueckliste()
        {
            Stueckliste = BriefbestandDataService.GetStuecklistenKomponenten(SelectedFahrzeuge.Select(f => f.Fahrgestellnummer)).ToListOrEmptyList();

            DataMarkForRefreshStueckliste();
        }

        public void FilterStueckliste(string filterValue, string filterProperties)
        {
            StuecklisteFiltered = Stueckliste.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void SelectStuecklistenEintrag(string id, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var sl = Stueckliste.FirstOrDefault(f => f.UniqueId == id);
            if (sl == null)
                return;

            sl.IsSelected = select;
            allSelectionCount = Stueckliste.Count(c => c.IsSelected);
        }

        public void SelectStueckliste(bool select, out int allSelectionCount, out int allCount, out int allFoundCount)
        {
            // Stueckliste.ToListOrEmptyList().ForEach(sl => (sl.IsSelected) = select);

            foreach (var sl in Stueckliste.ToListOrEmptyList())            
                sl.IsSelected = !sl.EntgueltigVersandt ? select : false;
                                    
            allSelectionCount = Stueckliste.Count(c => c.IsSelected);
            allCount = Stueckliste.Count;
            allFoundCount = Stueckliste.Count;
        }

        #endregion


        #region Step "Fahrzeugwahl"

        [XmlIgnore]
        public List<Fahrzeugbrief> Fahrzeuge { get { return FahrzeugeForPartList ?? FahrzeugeMergedWithCsvUpload ?? BriefbestandDataService.FahrzeugbriefeZumVersand; } }

        [XmlIgnore]
        private List<Fahrzeugbrief> FahrzeugeMergedWithCsvUpload { get; set; }

        [XmlIgnore]
        public List<Fahrzeugbrief> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Land> LaenderList { get { return BriefVersandDataService.Laender; } }

        [XmlIgnore]
        public List<VersandOption> VersandOptionenList
        {
            get
            {
                return BriefVersandDataService.VersandOptionen
                    .Where(vo => vo.IstEndgueltigerVersand == VersandartOptionen.IstEndgueltigerVersand && vo.MaterialCode != "ZZABMELD")
                    .OrderBy(w => w.Name)
                    .ToList();
            }
        }

        [XmlIgnore]
        public List<VersandGrund> VersandGruendeList { get { return BriefVersandDataService.GetVersandgruende(VersandartOptionen.IstEndgueltigerVersand); } }

        [XmlIgnore]
        public List<Fahrzeugbrief> SelectedFahrzeuge { get { return Fahrzeuge.Where(c => c.IsSelected).OrderBy(c => c.Fahrgestellnummer).ToList(); } }

        [XmlIgnore]
        public string SelectedFahrzeugeAsString
        {
            get
            {
                if (SelectedFahrzeuge.Count > 10)
                    return string.Format("{0} Fahrzeug{1}", SelectedFahrzeuge.Count, SelectedFahrzeuge.Count == 1 ? "" : "e");

                return string.Join(", ", SelectedFahrzeuge.Select(c => c.Fahrgestellnummer));
            }
        }

        [XmlIgnore]
        private string PrevSelectedFahrzeugeAsString { get; set; }

        [XmlIgnore]
        public string SaveErrorMessage { get; private set; }

        public string FahrzeugAuswahlTitleHint { get { return Localize.PleaseChooseOneOrMoreVehicles; } }
        public string StuecklistenAuswahlTitleHint { get { return Localize.PleaseChooseOneOrMoreComponents; } }

        [XmlIgnore]
        public string CsvUploadFileName { get; private set; }
        [XmlIgnore]
        public string CsvUploadServerFileName { get; private set; }
        [XmlIgnore]
        public bool UploadItemsSuccessfullyStored { get; set; }
        [XmlIgnore]
        public List<FahrzeugCsvUploadEntity> UploadItems { get; private set; }

        public string CsvTemplateFileName
        {
            get { return (VersandModus == BriefversandModus.Schluessel ? "UploadSchluesselversand.csv" : "UploadBriefversand.csv"); } 
        }

        #endregion


        public string BeauftragungBezeichnung
        {
            get
            {
                return string.Format("{0}: {1}", PrevSelectedFahrzeugeAsString.Contains(",") ? "FIN's" : "FIN", PrevSelectedFahrzeugeAsString);
            }
        }


        #region Step "Versandadresse"

        [XmlIgnore]
        public List<Adresse> VersandAdressen
        {
            get { return AdressenDataService.Adressen.Where(a => a.Kennung == "VERSANDADRESSE").ToListOrEmptyList(); }
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
            get { return PropertyCacheGet(() => new Adresse { Land = "DE", Kennung = "VERSANDADRESSE" }); }
            set { PropertyCacheSet(value); }
        }

        public Adresse GetVersandAdresseFromKey(string key)
        {
            int id;
            Adresse adr = null;
            if (Int32.TryParse(key, out id))
                adr = VersandAdressen.FirstOrDefault(v => v.ID == id) ?? ZulassungAdressen.FirstOrDefault(v => v.ID == id);

            if (adr == null)
                // note: skip "Zulassungsstellen" in "auto select" mode
                adr = VersandAdressen.FirstOrDefault(a => a.GetAutoSelectString() == key);

            if (adr != null)
                if (adr.Land.IsNullOrEmpty())
                    adr.Land = "DE";

            return adr;
        }

        [XmlIgnore]
        public List<Adresse> ZulassungAdressen
        {
            get { return AdressenDataService.ZulassungsStellen; }
        }

        [XmlIgnore]
        public List<string> ZulassungAdressenAsAutoCompleteItems
        {
            get { return ZulassungAdressen.Select(a => a.GetAutoSelectString()).ToList(); }
        }

        [XmlIgnore]
        public List<Adresse> ZulassungAdressenFiltered
        {
            get { return PropertyCacheGet(() => ZulassungAdressen); }
            private set { PropertyCacheSet(value); }
        }

        public Adresse GetZulassungAdresseFromKey(string key)
        {
            int id;
            if (Int32.TryParse(key, out id))
                return ZulassungAdressen.FirstOrDefault(v => v.ID == id);

            return ZulassungAdressen.FirstOrDefault(a => a.GetAutoSelectString() == key);
        }

        #endregion


        #region Step "Versand"

        public bool VersandartOptionenEndgültigerVersandAusgeblendet
        {
            get { return GetApplicationConfigBoolValueForCustomer("VersandartOptionenEndgültigerVersandAusgeblendet", true); }
        }

        public VersandartOptionen VersandartOptionen
        {
            get
            {
                return PropertyCacheGet(() => new VersandartOptionen
                {
                    IstEndgueltigerVersand = !VersandartOptionenEndgültigerVersandAusgeblendet,
                    EndgueltigerVersandAusgeblendet = VersandartOptionenEndgültigerVersandAusgeblendet
                });
            }
            set { PropertyCacheSet(value); }
        }

        public VersandOptionen VersandOptionen
        {
            get { return PropertyCacheGet(() => new VersandOptionen { AufAbmeldungWarten = VersandOptionAufAbmeldungWartenInitialChecked }); }
            set { PropertyCacheSet(value); }
        }

        public bool VersandOptionAufAbmeldungWartenAvailable
        {
            get
            {
                return VersandartOptionen.IstEndgueltigerVersand && GetApplicationConfigBoolValueForCustomer("OptionAufAbmeldungWarten", true);
            }
        }

        public bool VersandOptionAufAbmeldungWartenInitialChecked
        {
            get
            {
                return VersandOptionAufAbmeldungWartenAvailable && GetApplicationConfigBoolValueForCustomer("OptionAufAbmeldungWarten_Checked", true);
            }
        }

        public void InitVersandOptionen()
        {
            VersandOptionen.IstPostfachAdresse = (VersandAdresse != null && VersandAdresse.Strasse.NotNullOrEmpty().ToUpper().Contains("POSTFACH"));
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
                                         LogonContext.KundenNr,
                                         LogonContext.Customer.Customername)
                };
            }
        }

        private GeneralEntity SummaryBeauftragungsHeader
        {
            get
            {
                return new GeneralEntity
                {
                    Title = "Ihre Beauftragung für " + AppTitle,
                    Body = BeauftragungBezeichnung,
                    Tag = "SummaryMainItem"
                };
            }
        }

        public void DataMarkForRefresh(string vins)
        {
            GetCurrentAppID();

            Adresse.Laender = LaenderList;

            BriefbestandDataService.MarkForRefreshFahrzeugbriefe();
            AdressenDataService.MarkForRefreshAdressen();

            // reset filtered data
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
            DataMarkForRefreshVersandAndZulassungAdressenFiltered();

            // reset CSV Upload (merged) data
            FahrzeugeMergedWithCsvUpload = null;

            // reset data for partlist delivery
            FahrzeugeForPartList = null;

            ParamVins = vins;
            if (ParamVins.IsNotNullOrEmpty())
                ParamVins.Split(',').ToList().ForEach(vin => TrySelectFahrzeugVIN(vin.Trim()));
            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);

            PropertyCacheClear(this, m => m.VersandartOptionen);
            PropertyCacheClear(this, m => m.VersandOptionen);

            PropertyCacheClear(this, m => m.EquiPartlistSelektor);
        }

        public void DataMarkForRefreshVersandAndZulassungAdressenFiltered()
        {
            PropertyCacheClear(this, m => m.VersandAdressenFiltered);
            PropertyCacheClear(this, m => m.ZulassungAdressenFiltered);
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterVersandAdressen(string filterValue, string filterProperties)
        {
            VersandAdressenFiltered = VersandAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterZulassungAdressen(string filterValue, string filterProperties)
        {
            ZulassungAdressenFiltered = ZulassungAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void TrySelectFahrzeugVIN(string vin)
        {
            var fzg = Fahrzeuge.FirstOrDefault(f => f.Fahrgestellnummer.NotNullOrEmpty().ToLower() == vin.NotNullOrEmpty().ToLower());
            if (fzg == null)
                return;

            fzg.IsSelected = true;
        }

        public string SelectFahrzeug(string vin, bool select, out int allSelectionCount, out bool mustBeConfirmed)
        {
            string sperrvermerk = "";
            
            allSelectionCount = 0;
            mustBeConfirmed = false;
            var fzg = Fahrzeuge.FirstOrDefault(f => f.Fahrgestellnummer == vin);
            if (fzg == null)
                return sperrvermerk;

            fzg.IsSelected = select;
            allSelectionCount = Fahrzeuge.Count(c => c.IsSelected);

            if (fzg.Referenz1.IsNotNullOrEmpty())
            {
                var fzb = new FahrzeugbriefErweitert();
                mustBeConfirmed = fzb.SperrvermerkListe.Contains(fzg.Referenz1);
                sperrvermerk = fzb.SperrvermerkListe.FirstOrDefault(c => c == fzg.Referenz1);
            }
            return sperrvermerk;
        }

        public void SelectFahrzeuge(bool select, Predicate<Fahrzeugbrief> filter, out int allSelectionCount, out int allCount, out int allFoundCount, out bool mustBeConfirmed)
        {
            mustBeConfirmed = false;
            Fahrzeuge.Where(f => filter(f)).ToListOrEmptyList().ForEach(f => f.IsSelected = select);

            allSelectionCount = Fahrzeuge.Count(c => c.IsSelected);
            allCount = Fahrzeuge.Count;
            allFoundCount = Fahrzeuge.Count(c => filter(c));

            var fzb = new FahrzeugbriefErweitert();
            mustBeConfirmed = Fahrzeuge.Any(c => c.IsSelected && c.Referenz1.IsNotNullOrEmpty() && fzb.SperrvermerkListe.Contains(c.Referenz1));
        }

        VersandAuftragsAnlage CreateVersandAuftrag(string vin, string stuecklistenCode, bool briefVersand, bool schluesselVersand, bool schluesselKombiVersand)
        {
            var versandAuftrag = new VersandAuftragsAnlage();

            // Mapping der Versandadress-Daten muss vor der Zuweisung der weiteren Properties passieren, weil sonst ggf. die falschen Daten überschrieben werden
            ModelMapping.Copy(VersandAdresse, versandAuftrag);

            versandAuftrag.KundenNr = BriefbestandDataService.ToDataStoreKundenNr(LogonContext.KundenNr);
            versandAuftrag.VIN = vin;
            versandAuftrag.BriefVersand = briefVersand;
            versandAuftrag.SchluesselVersand = schluesselVersand;
            versandAuftrag.SchluesselKombiVersand = schluesselKombiVersand;
            versandAuftrag.StuecklistenKomponente = stuecklistenCode;
            versandAuftrag.AbmeldeKennzeichen = (!VersandOptionen.AufAbmeldungWartenAvailable || !VersandOptionen.AufAbmeldungWarten);
            versandAuftrag.AbcKennzeichen = VersandartOptionen.Versandart;
            versandAuftrag.MaterialNr = VersandOptionen.VersandOption.MaterialCode;
            versandAuftrag.DadAnforderungsDatum = DateTime.Today;
            versandAuftrag.ErfassungsUserName = LogonContext.UserName;
            versandAuftrag.Bemerkung = VersandOptionen.BemerkungAsString;
            versandAuftrag.Versandgrund = VersandOptionen.VersandGrund.Code;
            versandAuftrag.Mahnverfahren = (VersandAdresse.Kennung == "ZULASSUNG" ? "0001" : "0002");

            return versandAuftrag;
        }

        public void Save()
        {
            SaveErrorMessage = "";

            // 1. Versandauftrags-Datensätze anlegen
            var versandAuftraege = new List<VersandAuftragsAnlage>();

            SelectedFahrzeuge.ForEach(fzg =>
                {
                    if (VersandModus == BriefversandModus.Brief)
                        versandAuftraege.Add(CreateVersandAuftrag(fzg.Fahrgestellnummer, "", briefVersand: true, schluesselVersand: false, schluesselKombiVersand: false));
                    
                    if (VersandModus == BriefversandModus.Schluessel)
                        versandAuftraege.Add(CreateVersandAuftrag(fzg.Fahrgestellnummer, "", briefVersand: false, schluesselVersand: true, schluesselKombiVersand: false));

                    if (VersandModus == BriefversandModus.BriefMitSchluessel)
                    {
                        versandAuftraege.Add(CreateVersandAuftrag(fzg.Fahrgestellnummer, "", briefVersand: true, schluesselVersand: false, schluesselKombiVersand: false));
                        versandAuftraege.Add(CreateVersandAuftrag(fzg.Fahrgestellnummer, "", briefVersand: false, schluesselVersand: true, schluesselKombiVersand: true));
                    }

                    if (VersandModus == BriefversandModus.Stueckliste)
                        SelectedStueckliste.Where(sl => sl.Fahrgestellnummer == fzg.Fahrgestellnummer)
                            .ToListOrEmptyList().ForEach(sl =>
                                versandAuftraege.Add(CreateVersandAuftrag(sl.Fahrgestellnummer, sl.Nr, briefVersand: true, schluesselVersand: false, schluesselKombiVersand: false)));
                });

            BriefVersandDataService.SaveVersandBeauftragung(versandAuftraege, true, 
                (fin, errorMessage) =>
                {
                    errorMessage = errorMessage.NotNullOrEmpty().Replace(":", ",");

                    var error = string.Format("FIN {0}: {1}", fin, errorMessage);

                    SaveErrorMessage += SaveErrorMessage.ReplaceIfNotNull("; ") + error;
                });
        }

        public GeneralSummary CreateSummaryModel(bool cacheOriginItems)
        {
            return CreateSummaryForVersand(cacheOriginItems);
        }

        private GeneralSummary CreateSummaryForVersand(bool pdfMode)
        {
            if (!pdfMode)
                PrevSelectedFahrzeugeAsString = SelectedFahrzeugeAsString;

            var summaryModel = new GeneralSummary
            {
                Header = "Auftragsübersicht",
                Items = new ListNotEmpty<GeneralEntity>
                        (
                            SummaryBeauftragungsHeader,

                            GetStuecklisteForSummary(),

                            new GeneralEntity
                            {
                                Title = Localize.DispatchType,
                                Body = VersandartOptionen.GetSummaryString(),
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

        private GeneralEntity GetStuecklisteForSummary()
        {
            if (VersandModus != BriefversandModus.Stueckliste)
                return null;

            return new GeneralEntity
            {
                Title = Localize.Partlist,
                Body = string.Join("<br />", SelectedStueckliste.Select(sl => string.Format("- {0}, {1}, {2}", sl.Fahrgestellnummer, sl.Kennzeichen, sl.Bezeichnung))),
            };
        }

        private void GetCurrentAppID()
        {
            CurrentAppID = LogonContext.GetAppIdCurrent();
        }

        #endregion


        #region CSV Upload

        public bool CsvUploadFileSaveForPrefilter(string fileName, Func<string, bool> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".csv");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            var commaSeparatedAutoPropertyNamesToIgnore = (VersandModus != BriefversandModus.Schluessel ? "" : "ZBII");
            var list = new ExcelDocumentFactory().ReadToDataTable<FahrzeugCsvUploadEntity>(CsvUploadServerFileName, true, commaSeparatedAutoPropertyNamesToIgnore).ToList();

            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list;
            MergeCsvUploadItems();

            return true;
        }

        private void MergeCsvUploadItems()
        {
            FahrzeugeMergedWithCsvUpload = null;

            var mergedList = new List<Fahrzeugbrief>();
            var fahrzeugeFromUploadItems = UploadItems.Select(uploadItem => new Fahrzeugbrief
                {
                    Fahrgestellnummer = uploadItem.FIN,
                    Kennzeichen = uploadItem.Kennzeichen,
                    TechnIdentnummer = uploadItem.ZBII,
                    Vertragsnummer = uploadItem.LizenzNr,
                    Referenz1 = uploadItem.Referenz1,
                    Referenz2 = uploadItem.Referenz2,
                    IsMissing = true,
                }).ToListOrEmptyList();

            fahrzeugeFromUploadItems.ForEach(uploadFahrzeug =>
                {
                    var fahrzeugImBestand = GetFahrzeugImBestandMatchesPropertiesOf(uploadFahrzeug,
                                                                                    p => p.Fahrgestellnummer,
                                                                                    p => p.Kennzeichen,
                                                                                    p => p.TechnIdentnummer,
                                                                                    p => p.Vertragsnummer,
                                                                                    p => p.Referenz1,
                                                                                    p => p.Referenz2);
                    mergedList.Add(fahrzeugImBestand ?? uploadFahrzeug);
                }
             );

            SetFahrzeugeMergedWithCsvUpload(mergedList);
        }

        public void SetFahrzeugeMergedWithCsvUpload(List<Fahrzeugbrief> list)
        {
            FahrzeugeMergedWithCsvUpload = list;
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
        }

        Fahrzeugbrief GetFahrzeugImBestandMatchesPropertiesOf(Fahrzeugbrief uploadFahrzeug, params Expression<Func<Fahrzeugbrief, string>>[] propertyExpressions)
        {
            foreach (var propertyExpression in propertyExpressions)
            {
                var propertyMethod = propertyExpression.Compile();

                var fahrzeugImBestandFound = Fahrzeuge.FirstOrDefault(fahrzeugImBestand =>
                                                                      propertyMethod(uploadFahrzeug).IsNotNullOrEmpty() &&
                                                                      propertyMethod(fahrzeugImBestand).IsNotNullOrEmpty() &&
                                                                      propertyMethod(uploadFahrzeug) == propertyMethod(fahrzeugImBestand));
                if (fahrzeugImBestandFound != null)
                    return fahrzeugImBestandFound;
            }

            return null;
        }

        #endregion    
    }
}
