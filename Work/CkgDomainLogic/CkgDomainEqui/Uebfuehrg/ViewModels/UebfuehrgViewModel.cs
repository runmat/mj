// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Resources;
using GeneralTools.Services;
using Adresse = CkgDomainLogic.Uebfuehrg.Models.Adresse;
using Fahrzeug = CkgDomainLogic.Uebfuehrg.Models.Fahrzeug;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Uebfuehrg.ViewModels
{
    public class UebfuehrgViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IUebfuehrgDataService DataService
        {
            get { return CacheGet<IUebfuehrgDataService>(); }
        }

        [XmlIgnore]
        public List<CommonUiModel> StepModels { get; private set; }

        [XmlIgnore]
        public string Title
        {
            get { return "Überführung"; }
        }

        [XmlIgnore]
        public string TitleSmall
        {
            get { return "für 1 Fahrzeug"; }
        }


        [XmlIgnore]
        public string[] StepFriendlyNames
        {
            get { return PropertyCacheGet(() => StepModels.Select(s => s.HeaderShort).ToArray()); }
        }

        public int StepCurrentIndex { get; set; }

        [XmlIgnore]
        public CommonUiModel StepCurrentModel
        {
            get { return StepModels[StepCurrentIndex]; }
        }

        [XmlIgnore]
        public string StepCurrentFormPartialViewName
        {
            get { return GetFormPartialViewName(StepCurrentModel.ViewName); }
        }

        public string GetFormPartialViewName(string stepKey)
        {
            return string.Format("Forms/{0}", stepKey);
        }

        [XmlIgnore]
        public string FirstStepErrorHint
        {
            get { return null; }
        }

        private readonly string[] _addressTypes = {"ABHOLADRESSE", "AUSLIEFERUNG", "RÜCKHOLUNG", "HALTER"};

        [XmlIgnore]
        public List<Adresse> FahrtAdressen { get; private set; }

        [XmlIgnore]
        public List<Adresse> RechnungsAdressen { get; private set; }

        [XmlIgnore]
        public List<Land> Laender { get; private set; }

        [XmlIgnore]
        public List<TransportTyp> TransportTypen { get; private set; }

        [XmlIgnore]
        public List<Dienstleistung> Dienstleistungen { get; private set; }

        public List<Fahrt> Fahrten { get; set; }

        [XmlIgnore]
        public List<Fahrt> DienstleistungsFahrten
        {
            get { return Fahrten == null ? new List<Fahrt>() : Fahrten.Where(f => f.TypNr.IsNotNullOrEmpty()).ToList(); }
        }

        private List<UeberfuehrungsAuftragsPosition> _auftragsPositionen = new List<UeberfuehrungsAuftragsPosition>();

        [XmlIgnore]
        public List<UeberfuehrungsAuftragsPosition> AuftragsPositionen
        {
            get { return _auftragsPositionen; }
            private set { _auftragsPositionen = value; }
        }

        public bool ComingFromSummary { get; set; }


        public void DataInit()
        {
            DataMarkForRefresh();

            InitStepModels();
        }

        private void InitStepModels()
        {
            CommonUiModel uiModel;
            var index = 0;

            var list = new List<CommonUiModel>();

            uiModel = new RgDaten
                {
                    UiIndex = index,
                    GroupName = "RGDATEN",
                    SubGroupName = "-",
                    HeaderShort = "Rg",
                    Header = "Rechnungsdaten",
                    IgnoreEditFromSummary = true,
                    IsMandatory = true,

                    ViewName = "RgDaten",
                    GetRechnungsAdressen = () => RechnungsAdressen,

                    // TEST
                    ReKundenNr = "4711",
                    RgKundenNr = "9876"
                };
            list.Add(uiModel);
            index++;

            uiModel = new Fahrzeug
                {
                    FahrzeugIndex = "1",

                    UiIndex = index,

                    GroupName = "FAHRZEUGE",
                    SubGroupName = "FAHRZEUG_1",
                    Header = "Fahrzeug 1 (Hinfahrt)",
                    HeaderShort = "Fzg1",
                    IsMandatory = true,

                    ViewName = "Fahrzeug",

                    // TEST
                    FIN = "4711987654",
                    Fahrzeugklasse = "PKW",
                    Kennzeichen = "OD-J104",
                    Fahrzeugwert = "1000",
                    Typ = "Renault",
                    FahrzeugZugelassen = "J",
                    ZulassungsauftragAnDAD = "N",
                    Bereifung = "Winterreifen",
                };
            list.Add(uiModel);
            index++;

#if NO_TEST
            uiModel = new Adresse
                {
                    TransportTypAvailable = false,
                    TransportTyp = "-",

                    UhrzeitwunschAvailable = true,
                    AdressTyp = AdressenTyp.FahrtAdresse,
                    SubTyp = "ABHOLADRESSE",
                    UiIndex = index,
                    GroupName = "FAHRZEUG_1",
                    SubGroupName = "START",
                    Header = "Abholadresse (Fzg. 1)",
                    HeaderShort = "Ab",
                    Land = "DE",
                    GetAlleTransportTypen = () => TransportTypen,
                    IsMandatory = true,

                    ViewName = "Adresse",

                    // TEST
                    Name1 = "Walter Zabel",
                    Strasse = "Teststraße",
                    HausNr = "3",
                    PLZ = "22926",
                    Ort = "Ahrensburg",
                    Ansprechpartner = "...",
                    Telefon = "...",
                    Email = "xxx@xxx.de",
                };
            list.Add(uiModel);
            index++;
#endif

            uiModel = new Adresse
                {
                    TransportTypAvailable = true,
                    TransportTyp = "1",

                    UhrzeitwunschAvailable = true,
                    AdressTyp = AdressenTyp.FahrtAdresse,
                    SubTyp = "AUSLIEFERUNG",
                    UiIndex = index,
                    GroupName = "FAHRZEUG_1",
                    SubGroupName = "ZIEL",
                    Header = "Ziel Hinfahrt (Fzg. 1)",
                    HeaderShort = "Ziel",
                    Land = "DE",
                    GetAlleTransportTypen = () => TransportTypen,
                    IsMandatory = true,

                    ViewName = "Adresse",

                    // TEST
                    Name1 = "Göster Halmacken",
                    Strasse = "Willistraße",
                    HausNr = "42",
                    PLZ = "22941",
                    Ort = "Bargteheide",
                    Ansprechpartner = "...",
                    Telefon = "...",
                    Email = "xxx@xxx.de",
                };
            list.Add(uiModel);
            index++;

            uiModel = new DienstleistungsAuswahl
                {
                    FahrtTyp = "1",

                    UiIndex = index,
                    GroupName = "DIENSTLEISTUNGEN",
                    SubGroupName = "DIENSTLEISTUNGEN",
                    HeaderShort = "DL1",
                    Header = "Dienstleistungen (Fzg. 1)",
                    IsMandatory = false,

                    ViewName = "DienstleistungsAuswahl",
                };
            list.Add(uiModel);
            index++;

#if NO_TEST
            uiModel = new Fahrzeug
                {
                    FahrzeugIndex = "2",

                    UiIndex = index,

                    GroupName = "FAHRZEUGE",
                    SubGroupName = "FAHRZEUG_2",
                    Header = "Fahrzeug 2 (Rückfahrt)",
                    HeaderShort = "Fzg2",
                    IsMandatory = true,

                    ViewName = "Fahrzeug",

                    // TEST
                    FIN = "4711987654",
                    Fahrzeugklasse = "PKW",
                    Kennzeichen = "OD-EZ133",
                    Fahrzeugwert = "900",
                    Typ = "AUDI",
                    FahrzeugZugelassen = "J",
                    ZulassungsauftragAnDAD = "N",
                    Bereifung = "Sommerreifen",
                };
            list.Add(uiModel);
            index++;

            uiModel = new Adresse
                {
                    TransportTypAvailable = true,
                    TransportTyp = "2",

                    UhrzeitwunschAvailable = true,
                    AdressTyp = AdressenTyp.FahrtAdresse,
                    SubTyp = "RÜCKHOLUNG",
                    UiIndex = index,
                    GroupName = "FAHRZEUG_2",
                    SubGroupName = "ZIEL",
                    Header = "Ziel Rückfahrt (Fzg. 2)",
                    HeaderShort = "Rück",
                    Land = "DE",
                    GetAlleTransportTypen = () => TransportTypen,
                    IsMandatory = true,

                    ViewName = "Adresse",

                    // TEST
                    Name1 = "Gundulbert Hammer",
                    Strasse = "Herbertstraße ",
                    HausNr = "42",
                    PLZ = "22941",
                    Ort = "Bargteheide",
                    Ansprechpartner = "...",
                    Telefon = "...",
                    Email = "xxx@xxx.de",
                };
            list.Add(uiModel);
            index++;

            uiModel = new DienstleistungsAuswahl
                {
                    FahrtTyp = "2",

                    UiIndex = index,
                    GroupName = "DIENSTLEISTUNGEN",
                    SubGroupName = "DIENSTLEISTUNGEN",
                    HeaderShort = "DL2",
                    Header = "Dienstleistungen (Fzg. 2)",
                    IsMandatory = false,

                    ViewName = "DienstleistungsAuswahl",
                };
            list.Add(uiModel);
            index++;
#endif

            uiModel = new CommonSummary
                {
                    UiIndex = index,
                    GroupName = "SUMMARY",
                    SubGroupName = "SUMMARY",
                    HeaderShort = "Üb",
                    IgnoreEditFromSummary = true,
                    IsMandatory = false,

                    ViewName = "Summary",
                };
            list.Add(uiModel);
            index++;

            uiModel = new CommonUiModel
                {
                    UiIndex = index,
                    GroupName = "FINISH",
                    SubGroupName = "FINISH",
                    HeaderShort = "OK!",
                    IgnoreEditFromSummary = true,
                    IsMandatory = false,

                    ViewName = "Finish",
                };
            list.Add(uiModel);


            StepModels = list;
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.StepFriendlyNames);
            StepCurrentIndex = 0;
            ComingFromSummary = false;

            Laender = DataService.Laender;
            DomainCommon.Models.Adresse.Laender = Laender;

            RechnungsAdressen = DataService.GetRechnungsAdressen();
        }

        public void MoveToNextStep()
        {
            StepCurrentIndex++;
            if (StepCurrentIndex >= StepModels.Count)
                StepCurrentIndex = StepModels.Count - 1;
        }

        public void MoveToStep(int uiIndex)
        {
            StepCurrentIndex = uiIndex;
        }

        public void MoveToSummaryStep()
        {
            MoveToStep(StepModels.OfType<CommonSummary>().First().UiIndex);
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        public CommonUiModel GetStepModel(int uiIndex = -1)
        {
            if (uiIndex == -1)
                uiIndex = StepCurrentIndex;

            return StepModels[uiIndex];
        }

        /// <summary>
        /// Copies only DERIVED properties from classes deriving from UiModel to object StepForms[i]
        /// (preserves the base properties of class UiModel in the destination object (-> StepForms[i]))
        /// </summary>
        public T SaveSubModelWithPreservingUiModel<T>(T subModel, int uiIndex = -1) where T : CommonUiModel, new()
        {
            if (uiIndex == -1)
                uiIndex = StepCurrentIndex;

            var stepModel = GetStepModel(uiIndex);

            var savedUiModel = ModelMapping.Copy(stepModel, new CommonUiModel());
            ModelMapping.Copy(subModel, (T) stepModel);
            ModelMapping.Copy(savedUiModel, (T) stepModel);

            PrepareFollowingSteps(subModel);

            return (T) GetStepModel(uiIndex);
        }

        private void PrepareFollowingSteps<T>(T subModel) where T : CommonUiModel
        {
            if (subModel is RgDaten)
                PrepareRgDatenFahrtAdressenTransportTypen(subModel as RgDaten);

            if (subModel is DienstleistungsAuswahl)
                SaveDienstleistungen(subModel as DienstleistungsAuswahl);

            if (subModel is Adresse)
                SaveAdresse(subModel as Adresse);
        }

        private void PrepareRgDatenFahrtAdressenTransportTypen(RgDaten rgDaten)
        {
            rgDaten.GetRechnungsAdressen = () => RechnungsAdressen;
            DataService.AuftragGeber = rgDaten.RgKundenNr;

            FahrtAdressen = DataService.GetFahrtAdressen(_addressTypes);
            // TEST
            if (FahrtAdressen.None())
            {
                FahrtAdressen =
                    XmlService.XmlDeserializeFromFile<List<Adresse>>(Path.Combine(AppSettings.DataPath,
                                                                                  @"FahrtAdressen.xml"));
                FahrtAdressen.ForEach(item =>
                    {
                        item.GetAlleTransportTypen = () => TransportTypen;
                        if (item.Land.IsNullOrEmpty())
                            item.Land = "DE";
                    });
            }

            List<Dienstleistung> dienstleistungen;
            List<TransportTyp> transportTypen;
            DataService.GetTransportTypenAndDienstleistungen(out transportTypen, out dienstleistungen);
            Dienstleistungen = dienstleistungen;
            TransportTypen = transportTypen;

            StepModels.OfType<DienstleistungsAuswahl>()
                      .ToList()
                      .ForEach(dl => dl.InitDienstleistungen(Dienstleistungen, true));
        }

        public void SaveDienstleistungen(DienstleistungsAuswahl model)
        {
            var dienstleistungsAuswahl = (GetStepModel() as DienstleistungsAuswahl);
            if (dienstleistungsAuswahl != null)
            {
                dienstleistungsAuswahl.InitDienstleistungen(Dienstleistungen);
                dienstleistungsAuswahl.GewaehlteDienstleistungenString = model.GewaehlteDienstleistungenString;
            }
        }


        #region Adressen

        public void SaveAdresse(Adresse model)
        {
            model.GetAlleTransportTypen = () => TransportTypen;
        }

        [XmlIgnore]
        public List<Adresse> UebfuehrgAdressen
        {
            get { return FahrtAdressen.ToListOrEmptyList(); }
        }

        [XmlIgnore]
        public List<string> UebfuehrgAdressenAsAutoCompleteItems
        {
            get { return UebfuehrgAdressen.Select(a => a.GetAutoSelectString()).ToList(); }
        }

        [XmlIgnore]
        public List<Adresse> UebfuehrgAdressenFiltered
        {
            get { return PropertyCacheGet(() => UebfuehrgAdressen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterUebfuehrgAdressen(string filterValue, string filterProperties)
        {
            UebfuehrgAdressenFiltered = UebfuehrgAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void DataMarkForRefreshUebfuehrgAdressenFiltered()
        {
            PropertyCacheClear(this, m => m.UebfuehrgAdressenFiltered);
        }

        public Adresse GetUebfuehrgAdresseFromKey(string key)
        {
            if (key.ToInt() > 0)
                return FahrtAdressen.FirstOrDefault(adresse => adresse.KundenNr == key);

            return FahrtAdressen.FirstOrDefault(adresse => adresse.GetAutoSelectString() == key);
        }

        #endregion


        #region Summary

        public CommonSummary CreateSummaryModel(bool cacheOriginItems, Func<CommonUiModel, string> getSummaryStepDataEditLinkFunction)
        {
            var pdfMode = false;

            var summaryItems = StepModels
                .Select(step => new GeneralEntity
                    {
                        Title = step.Header,
                        Body = step.GetSummaryString() + (pdfMode || step.IgnoreEditFromSummary ? "" : getSummaryStepDataEditLinkFunction(step))
                    }).ToListOrEmptyList();

            var summaryModel = new CommonSummary
                {
                    Header = "Auftragsübersicht Überführung",
                    Items = summaryItems,
                };

            return summaryModel;

        }

        #endregion
    }
}
