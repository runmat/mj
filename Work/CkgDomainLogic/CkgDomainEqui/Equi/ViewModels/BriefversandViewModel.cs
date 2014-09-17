using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
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

        [XmlIgnore]
        public string ParamVins { get; private set; }

        [XmlIgnore]
        [DisplayName("Spaltenmodus")]
        public string UserLogonLevelAsString { get { return UserLogonLevel.ToString("F"); } }

        [XmlIgnore]
        public LogonLevel UserLogonLevel { get { return LogonContext.UserLogonLevel; } }

        public int CurrentAppID { get; set; }


        #region Step "Fahrzeugwahl"

        [XmlIgnore]
        public List<Fahrzeugbrief> Fahrzeuge { get { return BriefbestandDataService.FahrzeugbriefeZumVersand; } }

        public List<Fahrzeugbrief> FahrzeugeMergedWithCsvUpload { get; private set; }

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
        public List<Fahrzeugbrief> SelectedFahrzeuge { get { return Fahrzeuge.Where(c => c.IsSelected).ToList(); } }

        [XmlIgnore]
        public string SelectedFahrzeugeAsString { get { return string.Join(", ", SelectedFahrzeuge.Select(c => c.Fahrgestellnummer)); } }

        [XmlIgnore]
        private string PrevSelectedFahrzeugeAsString { get; set; }

        [XmlIgnore]
        public string SaveErrorMessage { get; private set; }

        public string FahrzeugAuswahlTitleHint { get { return Localize.PleaseChooseOneOrMoreVehicles; } }

        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }
        public bool UploadItemsSuccessfullyStored { get; set; }
        public List<FahrzeugCsvUploadEntity> UploadItems { get; private set; }

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
            if (Int32.TryParse(key, out id))
                return VersandAdressen.FirstOrDefault(v => v.ID == id);

            return VersandAdressen.FirstOrDefault(a => a.GetAutoSelectString() == key);
        }

        #endregion


        #region Step "Versand"

        public VersandartOptionen VersandartOptionen
        {
            get { return PropertyCacheGet(() => new VersandartOptionen{ IstEndgueltigerVersand = true }); }
            set { PropertyCacheSet(value); }
        }

        public VersandOptionen VersandOptionen
        {
            get { return PropertyCacheGet(() => new VersandOptionen()); }
            set { PropertyCacheSet(value); }
        }

        public bool VersandOptionAufAbmeldungWartenAvailable
        {
            get
            {
                if (VersandartOptionen.IstEndgueltigerVersand && CurrentAppID > 0 &&
                    LogonContext.UserApps.Any(a => a.AppID == CurrentAppID && a.BerechtigungsLevel.ContainsKey("7")))
                {
                    return true;
                }
                return false;
            }
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

        public void DataMarkForRefresh(string vins)
        {
            GetCurrentAppID();

            Adresse.Laender = LaenderList;

            BriefbestandDataService.MarkForRefreshFahrzeugbriefe();
            AdressenDataService.MarkForRefreshAdressen();

            // reset filtered data
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
            DataMarkForRefreshVersandAdressenFiltered();

            ParamVins = vins;
            if (ParamVins.IsNotNullOrEmpty())
                ParamVins.Split(',').ToList().ForEach(vin => TrySelectFahrzeugVIN(vin.Trim()));
            PropertyCacheClear(this, m => m.Steps);
            PropertyCacheClear(this, m => m.StepKeys);
            PropertyCacheClear(this, m => m.StepFriendlyNames);
        }

        public void DataMarkForRefreshVersandAdressenFiltered()
        {
            PropertyCacheClear(this, m => m.VersandAdressenFiltered);
        }

        public void DataMarkForRefreshVersandoptionen()
        {
            VersandOptionen.OptionenList = VersandOptionenList;
        }

        public void DataMarkForRefreshVersandgruende()
        {
            VersandOptionen.GruendeList = VersandGruendeList;
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void FilterVersandAdressen(string filterValue, string filterProperties)
        {
            VersandAdressenFiltered = VersandAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void TrySelectFahrzeugVIN(string vin)
        {
            var fzg = Fahrzeuge.FirstOrDefault(f => f.Fahrgestellnummer.NotNullOrEmpty().ToLower() == vin.NotNullOrEmpty().ToLower());
            if (fzg == null)
                return;

            fzg.IsSelected = true;
        }

        public void SelectFahrzeug(string vin, bool select, out int allSelectionCount)
        {
            allSelectionCount = 0;
            var fzg = Fahrzeuge.FirstOrDefault(f => f.Fahrgestellnummer == vin);
            if (fzg == null)
                return;

            fzg.IsSelected = select;
            allSelectionCount = Fahrzeuge.Count(c => c.IsSelected);
        }

        VersandAuftragsAnlage CreateVersandAuftrag(string vin, string stuecklistenCode)
        {
            var versandAuftrag = new VersandAuftragsAnlage
            {
                KundenNr = BriefbestandDataService.ToDataStoreKundenNr(LogonContext.KundenNr),
                VIN = vin,
                BriefVersand = true,
                SchluesselVersand = false,
                StuecklistenKomponente = stuecklistenCode,
                AbmeldeKennzeichen = (!VersandOptionen.AufAbmeldungWartenAvailable || !VersandOptionen.AufAbmeldungWarten),
                AbcKennzeichen = VersandartOptionen.Versandart,
                MaterialNr = VersandOptionen.VersandOption.MaterialCode,
                DadAnforderungsDatum = DateTime.Today,
                ErfassungsUserName = LogonContext.UserName,
                Bemerkung = VersandOptionen.Bemerkung,
                Versandgrund = VersandOptionen.VersandGrund.Code
            };

            ModelMapping.Copy(VersandAdresse, versandAuftrag);

            return versandAuftrag;
        }

        public void Save()
        {
            SaveErrorMessage = "";

            // 1. Versandauftrags-Datensätze anlegen
            var versandAuftraege = new List<VersandAuftragsAnlage>();

            SelectedFahrzeuge.ForEach(fzg => versandAuftraege.Add(CreateVersandAuftrag(fzg.Fahrgestellnummer, "")));

            SaveErrorMessage = BriefVersandDataService.SaveVersandBeauftragung(versandAuftraege);
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

        private void GetCurrentAppID()
        {
            int tmpAppId;
            int tmpUserId;
            int tmpCustomerId;
            int tmpKunnr;
            int tmpPortalType;

            HttpContextService.TryGetUserDataFromUrlOrSession(out tmpAppId, out tmpUserId, out tmpCustomerId, out tmpKunnr, out tmpPortalType);

            CurrentAppID = tmpAppId;
        }

        #endregion


        #region CSV Upload

        public bool CsvUploadFileSaveForPrefilter(string fileName, Func<string, bool> fileSaveAction)
        {
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".csv");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            var list = new ExcelDocumentFactory().ReadToDataTable<FahrzeugCsvUploadEntity>(CsvUploadServerFileName, true).ToList();
            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list;
            MergeCsvUploadItems();

            return true;
        }

        void MergeCsvUploadItems()
        {
            var fahrzeugeFromUploadItems = UploadItems.Select(uploadItem => new Fahrzeugbrief
                {
                    Fahrgestellnummer = uploadItem.FIN,
                    Kennzeichen = uploadItem.Kennzeichen,
                    TechnIdentnummer = uploadItem.ZBII,
                    Vertragsnummer = uploadItem.LizenzNr,
                    Referenz1 = uploadItem.Referenz1,
                    Referenz2 = uploadItem.Referenz2,
                    IsValid = false,
                }).ToListOrEmptyList();

            fahrzeugeFromUploadItems.ForEach(uploadFahrzeug =>
                {
                    if (FahrzeugeImBestandMatchesProperty(uploadFahrzeug, p => p.Fahrgestellnummer))
                        uploadFahrzeug.IsValid = true;
                    if (FahrzeugeImBestandMatchesProperty(uploadFahrzeug, p => p.Kennzeichen))
                        uploadFahrzeug.IsValid = true;
                    if (FahrzeugeImBestandMatchesProperty(uploadFahrzeug, p => p.TechnIdentnummer))
                        uploadFahrzeug.IsValid = true;
                    if (FahrzeugeImBestandMatchesProperty(uploadFahrzeug, p => p.Vertragsnummer))
                        uploadFahrzeug.IsValid = true;
                    if (FahrzeugeImBestandMatchesProperty(uploadFahrzeug, p => p.Referenz1))
                        uploadFahrzeug.IsValid = true;
                    if (FahrzeugeImBestandMatchesProperty(uploadFahrzeug, p => p.Referenz2))
                        uploadFahrzeug.IsValid = true;
                });


        }

        bool FahrzeugeImBestandMatchesProperty(Fahrzeugbrief uploadFahrzeug, Expression<Func<Fahrzeugbrief, string>> propertyExpression)
        {
            var propertyMethod = propertyExpression.Compile();

            return Fahrzeuge.Any(fahrzeugImBestand =>
                                 propertyMethod(uploadFahrzeug).IsNotNullOrEmpty() &&
                                 propertyMethod(fahrzeugImBestand).IsNotNullOrEmpty() &&
                                 propertyMethod(uploadFahrzeug) == propertyMethod(fahrzeugImBestand));
        }

        #endregion    
    }
}
