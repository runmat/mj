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
        public IUebfuehrgDataService DataService { get { return CacheGet<IUebfuehrgDataService>(); } }

        public List<CommonUiModel> StepModels { get; private set; }

        [XmlIgnore]
        public string Title { get { return "Überführung"; } }

        [XmlIgnore]
        public string TitleSmall { get { return "für 1 Fahrzeug"; } }


        public string[] StepFriendlyNames { get { return PropertyCacheGet(() => StepModels.Select(s => s.HeaderShort).ToArray()); } }

        public int StepCurrentIndex { get; set; }

        public CommonUiModel StepCurrentModel { get { return StepModels[StepCurrentIndex]; } }

        public string StepCurrentFormPartialViewName
        {
            get { return GetFormPartialViewName(StepCurrentModel.ViewName); }
        }

        public string GetFormPartialViewName(string stepKey)
        {
            return string.Format("Forms/{0}", stepKey);
        }

        public string FirstStepErrorHint { get { return null; } }

        readonly string[] _addressTypes = { "ABHOLADRESSE", "AUSLIEFERUNG", "RÜCKHOLUNG", "HALTER" };

        [XmlIgnore]
        public List<Adresse> FahrtAdressen { get; private set; }

        [XmlIgnore]
        public List<Adresse> RechnungsAdressen { get; private set; }

        [XmlIgnore]
        public List<Land> Laender { get; private set; }

        private List<TransportTyp> _transportTypen;
        [XmlIgnore]
        public List<TransportTyp> TransportTypen
        {
            get { return _transportTypen; }
        }

        private List<Dienstleistung> _dienstleistungen;

        [XmlIgnore]
        public List<Dienstleistung> Dienstleistungen
        {
            get { return _dienstleistungen; }
        }

        public List<Fahrt> Fahrten { get; set; }

        [XmlIgnore]
        public List<Fahrt> DienstleistungsFahrten { get { return Fahrten.Where(f => f.TypNr.IsNotNullOrEmpty()).ToList(); } }

        private List<UeberfuehrungsAuftragsPosition> _auftragsPositionen = new List<UeberfuehrungsAuftragsPosition>();

        [XmlIgnore]
        public List<UeberfuehrungsAuftragsPosition> AuftragsPositionen
        {
            get { return _auftragsPositionen; }
            private set { _auftragsPositionen = value; }
        }

        public void DataInit()
        {
            DataMarkForRefresh();

            InitStepModels();
        }

        void InitStepModels()
        {
            CommonUiModel uiModel;
            var index = 0; 
            
            var list = new List<CommonUiModel>();

            uiModel = new RgDaten
            {
                UiIndex = index,
                GroupName = "RGDATEN",
                SubGroupName = "-",
                Header = "Rg",
                IsMandatory = true,

                ViewName = "RgDaten",

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
                IsMandatory = false,

                ViewName = "DienstleistungsAuswahl",
            };
            list.Add(uiModel);
            index++;

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
                IsMandatory = false,

                ViewName = "DienstleistungsAuswahl",
            };
            list.Add(uiModel);
            index++;

            uiModel = new CommonUiModel
            {
                UiIndex = index,
                GroupName = "SUMMARY",
                SubGroupName = "SUMMARY",
                HeaderShort = "Üb",
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

            Laender = DataService.Laender;
            DomainCommon.Models.Adresse.Laender = Laender;

            RechnungsAdressen = DataService.GetRechnungsAdressen();
            RechnungsAdressen.ForEach(a =>
            {
                a.TransportTypAvailable = false;
                a.TransportTyp = "";
            });
            RgDaten.RechnungsAdressen = RechnungsAdressen;
        }

        public void MoveToNextStep()
        {
            StepCurrentIndex++;
            if (StepCurrentIndex >= StepModels.Count)
                StepCurrentIndex = StepModels.Count - 1;
        }

        public void Validate(Action<string, string> addModelError)
        {
        }

        public CommonUiModel GetStepModel(int uiIndex=-1)
        {
            if (uiIndex == -1)
                uiIndex = StepCurrentIndex;

            return StepModels[uiIndex];
        }

        public void SetStepModel(CommonUiModel stepForm, int uiIndex=-1)
        {
            if (uiIndex == -1)
                uiIndex = StepCurrentIndex;
            
            StepModels[uiIndex] = stepForm;
        }

        /// <summary>
        /// Copies only DERIVED properties from classes deriving from UiModel to object StepForms[i]
        /// (preserves the base properties of class UiModel in the destination object (-> StepForms[i]))
        /// </summary>
        public T SaveSubModelWithPreservingUiModel<T>(T subModel, int uiIndex=-1) where T : CommonUiModel, new()
        {
            if (uiIndex == -1)
                uiIndex = StepCurrentIndex;

            var savedUiModel = ModelMapping.Copy(GetStepModel(uiIndex), new CommonUiModel());

            SetStepModel(subModel, uiIndex);
            ModelMapping.Copy(savedUiModel, GetStepModel(uiIndex));

            PrepareFollowingSteps(subModel);

            return (T)GetStepModel(uiIndex);
        }

        private void PrepareFollowingSteps<T>(T subModel) where T : CommonUiModel
        {
            if (subModel is RgDaten)
                PrepareFahrtAdressenAndTransportTypen(subModel as RgDaten);

            if (subModel is DienstleistungsAuswahl)
                SaveDienstleistungen(subModel as DienstleistungsAuswahl);
        }

        private void PrepareFahrtAdressenAndTransportTypen(RgDaten rgDaten)
        {
            DataService.AuftragGeber = rgDaten.RgKundenNr;

            FahrtAdressen = DataService.GetFahrtAdressen(_addressTypes);

            DataService.GetTransportTypenAndDienstleistungen(out _transportTypen, out _dienstleistungen);
            DienstleistungsAuswahl.AlleDienstleistungen = _dienstleistungen;

            Adresse.AlleTransportTypen = _transportTypen.CopyAndInsertAtTop(new TransportTyp { ID = "", Name = Localize.TranslateResourceKey(LocalizeConstants.DropdownDefaultOptionPleaseChoose) });
        }


        #region Dienstleistungen

        public void SaveDienstleistungen(DienstleistungsAuswahl model)
        {
            var dienstleistungsAuswahl = (GetStepModel() as DienstleistungsAuswahl);
            if (dienstleistungsAuswahl != null)
                dienstleistungsAuswahl.GewaehlteDienstleistungenString = model.GewaehlteDienstleistungenString;
        }

        #endregion
    }
}
