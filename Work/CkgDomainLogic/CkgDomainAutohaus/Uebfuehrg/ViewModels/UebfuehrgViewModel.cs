//#define TESTDATA

// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using CkgDomainLogic.Uebfuehrg.Services;
using GeneralTools.Models;
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

        public string BingMapsLicenseKey { get { return ConfigurationManager.AppSettings["BingMapsLicenseKey"]; } }

        [XmlIgnore]
        public List<CommonUiModel> StepModels { get; private set; }

        [XmlIgnore]
        public string Title
        {
            get { return "Überführung"; }
        }

        public int AnzahlFahrzeugeGewuenscht { get; set; }

        public int AnzahlFahrzeugeGewuenschtCorresponding { get { return AnzahlFahrzeugeGewuenscht == 1 ? 2 : 1; } }

        public bool AnzahlFahrzeugeGewuenschtCorrespondingDisabled { get; set; }

        [XmlIgnore]
        public string TitleSmall
        {
            get { return GetTitleSmall(AnzahlFahrzeugeGewuenscht); }
        }

        [XmlIgnore]
        public string TitleSmallCorresponding
        {
            get { return GetTitleSmall(AnzahlFahrzeugeGewuenschtCorresponding); }
        }

        static string GetTitleSmall(int anzahlFahrzeugeGewuenscht)
        {
            return string.Format("für {0} Fahrzeug{1}", anzahlFahrzeugeGewuenscht, anzahlFahrzeugeGewuenscht > 1 ? "e" : "");
        }

        [XmlIgnore]
        public string ActionCorresponding
        {
            get { return string.Format("Fzg{0}", AnzahlFahrzeugeGewuenschtCorresponding); } 
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
        public string FirstStepErrorHint { get; set; }

        private readonly string[] _addressTypes = {"ABHOLADRESSE", "AUSLIEFERUNG", "RÜCKHOLUNG", "HALTER"};

        [XmlIgnore]
        public List<Adresse> FahrtAdressen { get; private set; }

        [XmlIgnore]
        public List<KundeAusHierarchie> KundenAusHierarchie { get; private set; }

        [XmlIgnore]
        public List<Adresse> RechnungsAdressen { get; private set; }

        [XmlIgnore]
        public List<Land> Laender { get; private set; }

        [XmlIgnore]
        public List<TransportTyp> TransportTypen { get; private set; }

        [XmlIgnore]
        public List<Dienstleistung> Dienstleistungen { get; private set; }

        public List<Fahrt> Fahrten { get; set; }

        public RgDaten RgDaten { get; set; }

        public RgDaten RgDatenFromStepModels { get { return (StepModels.FirstOrDefault() is RgDaten) ? (RgDaten)StepModels.First() : RgDaten; } }

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

        [XmlIgnore]
        public bool SaveStarted { get; set; }

        [XmlIgnore]
        public string ReceiptErrorMessages { get; set; }

        [XmlIgnore]
        public string ReceiptPdfFileName { get; set; }

        [XmlIgnore]
        private bool IstKroschke { get { return (LogonContext.Customer.AccountingArea == 1010); } }


        public void DataInit(int anzahlFahrzeugeGewuenscht, IDictionary<string, string> externalParams = null)
        {
            AnzahlFahrzeugeGewuenscht = anzahlFahrzeugeGewuenscht;
            FirstStepErrorHint = "";

            DataMarkForRefresh();

            InitStepModels();

            TryInitExternalParams(externalParams);
        }

        void TryInitExternalParams(IDictionary<string, string> externalParams)
        {
            AnzahlFahrzeugeGewuenschtCorrespondingDisabled = false;
            if (externalParams == null)
                return;

            AnzahlFahrzeugeGewuenschtCorrespondingDisabled = true;

            // ToDo: Umstellen von "Spiky Zulassungsfahrzeuge SQL" => auf "Kroschke Zulassungsfahrzeuge"
            //List<Autohaus.Models.Fahrzeug> storedFahrzeuge = null;

            foreach (var param in externalParams)
            {
                var fahrzeugIndex = param.Key.SubstringTry(param.Key.Length - 1, 1).ToInt();
                if (fahrzeugIndex < 1) continue;
                
                var fahrzeugModel = StepModels.OfType<Fahrzeug>().FirstOrDefault(f => f.FahrzeugIndex == fahrzeugIndex.ToString());
                if (fahrzeugModel == null) continue;

                var key = param.Key.SubstringTry(0, param.Key.Length - 1).ToLower();
                switch (key)
                {
                    case "vin":
                        fahrzeugModel.FIN = param.Value;
                        break;
                    case "licnr":
                        fahrzeugModel.Kennzeichen = param.Value;
                        break;
                    case "refnr":
                        fahrzeugModel.Referenznummer = param.Value;
                        break;

                    case "id":
                        // ToDo: Umstellen von "Spiky Zulassungsfahrzeuge SQL" => auf "Kroschke Zulassungsfahrzeuge"
                        //if (storedFahrzeuge == null)
                        //    storedFahrzeuge = FahrzeugverwaltungDataService.FahrzeugeGet();
                        //if (storedFahrzeuge == null)
                        //    break;
                        //var storedFahrzeug = storedFahrzeuge.FirstOrDefault(sf => sf.ID == param.Value.ToInt());
                        //if (storedFahrzeug == null)
                        //    break;

                        //fahrzeugModel.FIN = storedFahrzeug.FahrgestellNr;
                        //fahrzeugModel.Kennzeichen = storedFahrzeug.Kennzeichen;
                        //fahrzeugModel.Referenznummer = storedFahrzeug.ReferenzNr;
                        break;
                }
            }
        }

        private void InitStepModels()
        {
            CommonUiModel uiModel;
            var index = 0;

            var list = new List<CommonUiModel>();

            RgDaten = new RgDaten
                {
                    UiIndex = index,
                    GroupName = "RGDATEN",
                    SubGroupName = "-",
                    HeaderShort = "RG-Daten",
                    Header = "Rechnungsdaten",
                    EditFromSummaryDisabled = true,
                    IsMandatory = true,
                    KundenNrUser = LogonContext.KundenNr,
                    KundenNr = LogonContext.KundenNr,

                    ViewName = "RgDaten",
                    GetKundenAusHierarchie = () => KundenAusHierarchie,
                    GetRechnungsAdressen = () => RechnungsAdressen,

#if TESTDATA
                    //ReKundenNr = "0000349980",
                    //RgKundenNr = "0000349980"
#endif
                };

            if ((IstKroschke && RgDaten.KundenAusHierarchie.Count() > 1) || RgDaten.ReAdressen.Count() > 1 || RgDaten.RgAdressen.Count() > 1)
            {
                TryDataContextRestoreUiModel(RgDaten);
                list.Add(RgDaten);
                index++;
            }

            uiModel = new Fahrzeug
                {
                    FahrzeugIndex = "1",

                    UiIndex = index,

                    GroupName = "FAHRZEUGE",
                    SubGroupName = "FAHRZEUG_1",
                    Header = "Fahrzeug" + (AnzahlFahrzeugeGewuenscht > 1 ? " 1 (Hinfahrt)" : ""),
                    HeaderShort = "Fahrzeug" + (AnzahlFahrzeugeGewuenscht > 1 ? " 1" : ""),
                    IsMandatory = true,
                    AnzahlFahrzeugeGewuenscht = AnzahlFahrzeugeGewuenscht,

                    ViewName = "Fahrzeug",

#if TESTDATA
                    FIN = "4711987654",
                    Hersteller = "RENAULT",
                    Modell = "SCENIC",
                    Referenznummer = "#Ref 4710",
                    Kennzeichen = "OD-J104",
                    Fahrzeugklasse = "PKW",
                    Fahrzeugwert = "Z00",
                    FahrzeugZugelassen = true,
                    ZulassungBeauftragt = true,
#endif
                };
            TryDataContextRestoreUiModel(uiModel);
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
                    Header = "Abholadresse" + (AnzahlFahrzeugeGewuenscht > 1 ? " Fahrzeug 1" : ""),
                    HeaderShort = "Start" + (AnzahlFahrzeugeGewuenscht > 1 ? " 1" : ""),
                    Land = "DE",
                    GetAlleTransportTypen = () => TransportTypen,
                    IsMandatory = true,

                    ViewName = "Adresse",

#if TESTDATA
                    Name1 = "AH ABRA",
                    Strasse = "Bevenroder Str.",
                    HausNr = "10",
                    PLZ = "38108",
                    Ort = "Braunschweig",
                    Ansprechpartner = "Herr Schönberg",
                    Telefon = "05312372423",
                    Email = "xxx@xxx.de",
#endif
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
                    Header = "Ziel Hinfahrt (Fahrzeug 1)",
                    HeaderShort = "Ziel" + (AnzahlFahrzeugeGewuenscht > 1 ? " 1" : ""),
                    Land = "DE",
                    GetAlleTransportTypen = () => TransportTypen,
                    IsMandatory = true,

                    ViewName = "Adresse",

#if TESTDATA
                    Name1 = "ATN Autoterminal Neuss GmbH &",
                    Strasse = "Floßhafenstrasse",
                    HausNr = "30",
                    PLZ = "41460",
                    Ort = "Neuss",
                    Ansprechpartner = "Wolfgang Andrée",
                    Telefon = "02131-977-411",
                    Email = "xxx@xxx.de",
#endif
                };
            list.Add(uiModel);
            index++;

            uiModel = new DienstleistungsAuswahl
            {
                FahrtTyp = "1",
                FahrtIndex = "1",

                UiIndex = index,
                GroupName = "DIENSTLEISTUNGEN",
                SubGroupName = "DIENSTLEISTUNGEN",
                Header = "Dienstleistungen" + (AnzahlFahrzeugeGewuenscht > 1 ? " Fahrzeug 1" : ""),
                HeaderShort = "Optionen" + (AnzahlFahrzeugeGewuenscht > 1 ? " 1" : ""),
                IsMandatory = false,

                ViewName = "DienstleistungsAuswahl",
                Bemerkungen = new Bemerkungen(),
            };
            list.Add(uiModel);
            index++;

            if (AnzahlFahrzeugeGewuenscht == 2)
            {
                uiModel = new Fahrzeug
                    {
                        FahrzeugIndex = "2",

                        UiIndex = index,

                        GroupName = "FAHRZEUGE",
                        SubGroupName = "FAHRZEUG_2",
                        Header = "Fahrzeug 2 (Rückfahrt)",
                        HeaderShort = "Fahrzeug 2",
                        IsMandatory = true,
                        AnzahlFahrzeugeGewuenscht = AnzahlFahrzeugeGewuenscht,

                        ViewName = "Fahrzeug",

#if TESTDATA
                        FIN = "9718987654",
                        Hersteller = "AUDI",
                        Modell = "A4",
                        Referenznummer = "#Ref 4711",
                        Kennzeichen = "OD-EZ133",
                        Fahrzeugklasse = "PKW",
                        Fahrzeugwert = "Z00",
                        FahrzeugZugelassen = true,
                        ZulassungBeauftragt = true,
#endif
                    };
                TryDataContextRestoreUiModel(uiModel);
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
                        Header = "Ziel Rückfahrt (Fahrzeug 2)",
                        HeaderShort = "Ziel 2",
                        Land = "DE",
                        GetAlleTransportTypen = () => TransportTypen,
                        IsMandatory = true,

                        ViewName = "Adresse",

#if TESTDATA
                        Name1 = "DWC",
                        Strasse = "Schmalbachstr. 7",
                        HausNr = "7",
                        PLZ = "38112",
                        Ort = "Braunschweig",
                        Ansprechpartner = "Herr Leipnitz",
                        Telefon = "05312124773",
                        Email = "xxx@xxx.de",
#endif
                    };
                list.Add(uiModel);
                index++;

                uiModel = new DienstleistungsAuswahl
                    {
                        FahrtTyp = "2",
                        FahrtIndex = "2",

                        UiIndex = index,
                        GroupName = "DIENSTLEISTUNGEN",
                        SubGroupName = "DIENSTLEISTUNGEN",
                        Header = "Dienstleistungen Fahrzeug 2",
                        HeaderShort = "Optionen 2",
                        IsMandatory = false,

                        ViewName = "DienstleistungsAuswahl",
                        Bemerkungen = new Bemerkungen(),
                    };
                list.Add(uiModel);
                index++;
            }

            uiModel = new CommonSummary
                {
                    UiIndex = index,
                    GroupName = "SUMMARY",
                    SubGroupName = "SUMMARY",
                    HeaderShort = "Übersicht",
                    EditFromSummaryDisabled = true,
                    IsMandatory = false,

                    ViewName = "Summary",
                };
            list.Add(uiModel);
            index++;

            uiModel = new Receipt
                {
                    UiIndex = index,
                    GroupName = "FINISH",
                    SubGroupName = "FINISH",
                    HeaderShort = "Fertig!",
                    EditFromSummaryDisabled = true,
                    IsMandatory = false,

                    ViewName = "Receipt",
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

            KundenAusHierarchie = DataService.KundenAusHierarchie;
            RechnungsAdressen = DataService.GetRechnungsAdressen();
        }

        public void MoveToNextStep()
        {
            StepCurrentIndex++;
            if (StepCurrentIndex >= StepModels.Count)
                StepCurrentIndex = StepModels.Count - 1;

            DataMarkForRefreshUebfuehrgAdressenFiltered();
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

            PrepareFollowingSteps((T)stepModel);

            return (T)stepModel;
        }

        private void PrepareFollowingSteps<T>(T subModel) where T : CommonUiModel
        {
            if (subModel.UiIndex == 0)
            {
                if (subModel is RgDaten)
                    LogonContext.DataContextPersist(subModel as RgDaten);

                PrepareRgDatenFahrtAdressenTransportTypen(subModel is RgDaten ? (subModel as RgDaten) : RgDaten);
            }

            if (subModel is Fahrzeug)
                SaveFahrzeug(subModel as Fahrzeug);

            if (subModel is DienstleistungsAuswahl)
                SaveDienstleistungen(subModel as DienstleistungsAuswahl);

            if (subModel is Adresse)
            {
                SaveAdresse(subModel as Adresse);
                CalcFahrten();
            }

            if (subModel is CommonSummary)
                CalcFahrten();

            if (subModel is Receipt)
                SaveAll();
        }

        private void TryDataContextRestoreUiModel(CommonUiModel subModel)
        {
            if (subModel == null)
                return;

            if (subModel is RgDaten)
            {
                var storedRgDaten = (RgDaten)LogonContext.DataContextRestore(typeof(RgDaten).GetFullTypeName());
                if (storedRgDaten != null)
                {
                    var subModelRgDaten = subModel as RgDaten;

                    if (subModelRgDaten.RgAdressen.Any(rg => rg.KundenNr.NotNullOrEmpty().TrimStart('0') == storedRgDaten.RgKundenNr.NotNullOrEmpty().TrimStart('0')))
                        subModelRgDaten.RgKundenNr = storedRgDaten.RgKundenNr;

                    if (subModelRgDaten.ReAdressen.Any(re => re.KundenNr.NotNullOrEmpty().TrimStart('0') == storedRgDaten.ReKundenNr.NotNullOrEmpty().TrimStart('0')))
                        subModelRgDaten.ReKundenNr = storedRgDaten.ReKundenNr;
                }
            }

            if (subModel is Fahrzeug)
            {
                var storedFahrzeug = (Fahrzeug)LogonContext.DataContextRestore(typeof(Fahrzeug).GetFullTypeName());
                if (storedFahrzeug != null)
                {
                    var subModelFahrzeug = subModel as Fahrzeug;

                    subModelFahrzeug.Fahrzeugklasse = storedFahrzeug.Fahrzeugklasse;
                    subModelFahrzeug.Fahrzeugwert = storedFahrzeug.Fahrzeugwert;
                    subModelFahrzeug.FahrzeugZugelassen = storedFahrzeug.FahrzeugZugelassen;
                    subModelFahrzeug.ZulassungBeauftragt = storedFahrzeug.ZulassungBeauftragt;
                }
            }
        }

        private void PrepareRgDatenFahrtAdressenTransportTypen(RgDaten rgDaten)
        {
            if (IstKroschke && !String.IsNullOrEmpty(rgDaten.AgKundenNr))
            {
                rgDaten.KundenNr = rgDaten.AgKundenNr;

                if (rgDaten.KundenNr != DataService.KundenNr)
                {
                    DataService.KundenNr = rgDaten.AgKundenNr;
                    RechnungsAdressen = DataService.GetRechnungsAdressen();
                    rgDaten.MarkForRefreshRgReKundenNr();
                }
            }

            rgDaten.GetKundenAusHierarchie = () => KundenAusHierarchie;
            rgDaten.GetRechnungsAdressen = () => RechnungsAdressen;

            if (IstKroschke)
            {
                if (String.IsNullOrEmpty(rgDaten.RgKundenNr))
                    rgDaten.RgKundenNr = rgDaten.RgAdressen.First().KundenNr;

                if (String.IsNullOrEmpty(rgDaten.ReKundenNr))
                    rgDaten.ReKundenNr = rgDaten.ReAdressen.First().KundenNr;

                if (StepCurrentModel is RgDaten)
                {
                    (StepCurrentModel as RgDaten).KundenNr = rgDaten.KundenNr;
                    (StepCurrentModel as RgDaten).RgKundenNr = rgDaten.RgKundenNr;
                    (StepCurrentModel as RgDaten).ReKundenNr = rgDaten.ReKundenNr;
                }
            }

            DataService.AuftragGeber = rgDaten.RgKundenNr;

            FahrtAdressen = DataService.GetFahrtAdressen(_addressTypes);

            List<Dienstleistung> dienstleistungen;
            List<TransportTyp> transportTypen;
            DataService.GetTransportTypenAndDienstleistungen(out transportTypen, out dienstleistungen);
            Dienstleistungen = dienstleistungen;
            TransportTypen = transportTypen;

            StepModels.OfType<DienstleistungsAuswahl>()
                      .ToList()
                      .ForEach(dl => dl.InitDienstleistungen(Dienstleistungen, null, true));
        }

        public void SaveDienstleistungen(DienstleistungsAuswahl model)
        {
            model.InitDienstleistungen(Dienstleistungen, model.FahrtTyp);
            model.Bemerkungen.FahrtIndex = model.FahrtIndex;
        }



        #region Fahrzeug

        public void SaveFahrzeug(Fahrzeug model)
        {
            LogonContext.DataContextPersist(model);

            if (AnzahlFahrzeugeGewuenscht == 2 && model.FahrzeugIndex == "1")
            {
                var fahrzeug2 = StepModels.OfType<Fahrzeug>().LastOrDefault();
                TryDataContextRestoreUiModel(fahrzeug2);
            }
        }

        #endregion


        #region Adressen

        public void CheckAdressDatum(Adresse model, DateTime? savedDatum, Action<string, string> addModelError, Action failAction)
        {
            if (ComingFromSummary && model.Datum.GetValueOrDefault() != savedDatum.GetValueOrDefault())
            {
                addModelError("Datum", "Das Überführungsdatum darf aus der Übersicht heraus nicht mehr geändert werden. Stattdessen wählen Sie bitte aus der Überischt heraus die Zurück Buttons.");
                if (failAction != null)
                    failAction();
            }

            if (model.Datum != null)
            {
                var formerAddresses = StepModels.OfType<Adresse>().Where(a => a.UiIndex < model.UiIndex);
                if (formerAddresses.Any())
                {
                    var formerAddressMaxDatum = formerAddresses.Max(a => a.Datum.GetValueOrDefault());
                    if (model.Datum.GetValueOrDefault() < formerAddressMaxDatum)
                        addModelError("Datum", string.Format("Das Überführungsdatum darf nicht vor dem größten Datum Ihrer bisher gewählten Fahrten liegen: {0:dd.MM.yyyy}", formerAddressMaxDatum));
                }
            }
        }

        public void SaveAdresse(Adresse model)
        {
            model.GetAlleTransportTypen = () => TransportTypen;

            if (model.TransportTypAvailable && !ComingFromSummary)
            {
                var correspondingDienstleistungsAuswahl = StepModels.Skip(StepModels.IndexOf(model)).OfType<DienstleistungsAuswahl>().FirstOrDefault();
                if (correspondingDienstleistungsAuswahl != null)
                {
                    correspondingDienstleistungsAuswahl.InitDienstleistungen(Dienstleistungen, model.TransportTyp, true);
                }
            }
        }

        [XmlIgnore]
        public List<Adresse> UebfuehrgAdressen
        {
            get { return FahrtAdressen.ToListOrEmptyList().Where(a => a.SubTyp == UebfuehrgAdressenSubTypCurrent.NotNullOrEmpty()).ToListOrEmptyList(); }
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

        string UebfuehrgAdressenSubTypCurrent { get; set; }    

        public void FilterUebfuehrgAdressen(string filterValue, string filterProperties)
        {
            UebfuehrgAdressenFiltered = UebfuehrgAdressen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void DataMarkForRefreshUebfuehrgAdressenFiltered()
        {
            PropertyCacheClear(this, m => m.UebfuehrgAdressenFiltered);

            UebfuehrgAdressenSubTypCurrent = "";

            var currentAdresse = GetStepModel() as Adresse;
            if (currentAdresse == null) return;

            UebfuehrgAdressenSubTypCurrent = currentAdresse.SubTyp;
        }

        public Adresse GetUebfuehrgAdresseFromKey(string key)
        {
            if (key.ToInt() > 0)
                return FahrtAdressen.FirstOrDefault(adresse => adresse.ID == key.ToInt());

            return FahrtAdressen.FirstOrDefault(adresse => adresse.GetAutoSelectString() == key);
        }

        List<Adresse> GetValidAddressModels()
        {
            return StepModels.OfType<Adresse>().ToList();
        } 

        #endregion


        #region DienstleistungsAuswahl

        public void CheckDienstleistungsAuswahl(DienstleistungsAuswahl dienstleistungsAuswahl, Action<string, string> addModelError)
        {
            dienstleistungsAuswahl.InitDienstleistungen(Dienstleistungen);
            CheckDienstleistungsDatumsWerteSamstagsAuslieferung(dienstleistungsAuswahl, addModelError);
            CheckDienstleistungsDatumsWerteVorholung(dienstleistungsAuswahl, addModelError);
        }

        /// <summary>
        /// Bei Samstag-Liefertermin muss vom User explizit(!) die Dienstleistung "Samstags Auslieferung" gewählt werden
        /// </summary>
        private void CheckDienstleistungsDatumsWerteSamstagsAuslieferung(DienstleistungsAuswahl dienstleistungsAuswahl, Action<string, string> addModelError)
        {
            CheckDienstleistungsDatumsWerte(dienstleistungsAuswahl, addModelError,
                "Samstagsauslieferung",
                "Diese Fahrt geht über einen Samstag, bitte wählen Sie die Dienstleistung '{0}'.",
                dateList =>
                dateList.Any(date => date != null && date.GetValueOrDefault().DayOfWeek == DayOfWeek.Saturday));
        }

        /// <summary>
        /// Bei unterschiedlichen Tagen der Eigenschaft Adresse.Datum aller Fahrt-Adressen eines Fahrzeugs...
        /// ==> muss die Dienstleistung "Vorholung" gewählt werden
        /// </summary>        
        private void CheckDienstleistungsDatumsWerteVorholung(DienstleistungsAuswahl dienstleistungsAuswahl, Action<string, string> addModelError)
        {
            CheckDienstleistungsDatumsWerte(dienstleistungsAuswahl, addModelError,
                "Vorholung",
                "Diese Fahrt geht über unterschiedliche Tage, bitte wählen Sie die Dienstleistung '{0}'.",
                dateList => dateList.Where(date => date != null).Distinct().Count() > 1);
        }

        private void CheckDienstleistungsDatumsWerte(DienstleistungsAuswahl dienstleistungsAuswahl, Action<string, string> addModelError, 
                                                                       string dienstleistungsNameToCheck,
                                                                       string validationMessage,
                                                                       Func<IEnumerable<DateTime?>, bool> datesInvalidFunc)
        {
            if (dienstleistungsAuswahl == null)
                return;

            var fahrt = Fahrten.FirstOrDefault(f => f.FahrtIndex == dienstleistungsAuswahl.FahrtIndex);
            if (fahrt == null)
                return;

            var fahrzeug = fahrt.Fahrzeug;
            var alleFahrtenDiesesFahrzeugs = Fahrten.Where(f => f.FahrzeugIndex == fahrzeug.FahrzeugIndex);

            var hauptFahrtModel = alleFahrtenDiesesFahrzeugs.FirstOrDefault(f => f.IstHauptFahrt);
            if (hauptFahrtModel == null)
                return;

            var hauptFahrtDienstleistungsAuswahl = StepModels.OfType<DienstleistungsAuswahl>().FirstOrDefault(dl => dl.FahrtTyp == hauptFahrtModel.TypNr);
            if (hauptFahrtDienstleistungsAuswahl == null)
                return;

            var hauptFahrtDienstleistungToCheck =
                hauptFahrtDienstleistungsAuswahl.AvailableDienstleistungen.FirstOrDefault(d => d.Name.ToLower().StartsWith(dienstleistungsNameToCheck.ToLower()));
            if (hauptFahrtDienstleistungToCheck == null)
                return;

            var allDatesToCheck =
                alleFahrtenDiesesFahrzeugs.SelectMany(
                    f => new List<DateTime?> {f.StartAdresse.Datum, f.ZielAdresse.Datum});

            var datesInvalid = datesInvalidFunc(allDatesToCheck);

            if (datesInvalid &&
                hauptFahrtDienstleistungsAuswahl.GewaehlteDienstleistungen.None(dl => dl.ID == hauptFahrtDienstleistungToCheck.ID))
            {
                addModelError(hauptFahrtDienstleistungToCheck.Name, string.Format(validationMessage, hauptFahrtDienstleistungToCheck.Name));
            }
        }

        #endregion


        #region Summary

        public CommonSummary CreateSummaryModel(bool cacheOriginItems, Func<CommonUiModel, string> getSummaryStepDataEditLinkFunction)
        {
            var summaryItems = StepModels
                .Select(step => new GeneralEntity
                    {
                        Title = step.Header,
                        Body = step.GetSummaryString() + (step.EditFromSummaryDisabled ? "" : getSummaryStepDataEditLinkFunction(step))
                    }).ToListOrEmptyList();

            var summaryModel = new CommonSummary
                {
                    Header = "Auftragsübersicht Überführung",
                    Items = summaryItems,
                };

            return summaryModel;

        }

        public void SaveAll()
        {
            ReceiptErrorMessages = "";
            try
            {
                AuftragsPositionen = DataService.Save(RgDatenFromStepModels, StepModels, Fahrten).ToListOrEmptyList();
                ReceiptPdfFileName = new ReceiptCreationService(this).CreatePDF();
            }
            catch (Exception e)
            {
                ReceiptErrorMessages = e.Message;
                SaveStarted = false;
                return;
            }
            
            var invalidPositions = AuftragsPositionen.Where(ap => !ap.IsValid);
            if (invalidPositions.Any())
                ReceiptErrorMessages = string.Join("<br />", invalidPositions.Select(ap => ap.AuftragsNrText));

            SaveStarted = false;
        }

        #endregion


        #region Fahrten

        private void CalcFahrten()
        {
            if (Fahrten == null)
                Fahrten = new List<Fahrt>();

            var allValidAddressModels = GetValidAddressModels();
            allValidAddressModels.ForEach(address => address.Fahrten = new List<Fahrt>());

            Fahrten.RemoveAll(f => true);

            // Fahrzeug 1: Abholung
            AddRemoveFahrt("FAHRZEUG_1", "START", 0);

            // Fahrzeug 1: Auslieferung + Überführung
            AddRemoveFahrt("FAHRZEUG_1", "ZIEL", 1);

            // Fahrzeug 1: Zusatzfahrt
            AddRemoveFahrt("FAHRZEUG_1", "ZUSATZ", 2);

            // Fahrzeug 2: Rückholung
            AddRemoveFahrt("FAHRZEUG_2", "ZIEL", 3);

            // Fahrzeug 2: Zusatzfahrt
            AddRemoveFahrt("FAHRZEUG_2", "ZUSATZ", 4);

            var fahrtIndex = 0;
            Fahrten.ForEach(fahrt => fahrt.FahrtIndex = (fahrtIndex++).ToString());
        }

        private void AddRemoveFahrt(string fahrzeug, string fahrtTyp, int sort)
        {
            var fahrzeugIndex = fahrzeug.Substring(fahrzeug.Length - 1);
            var allValidAddressModels = GetValidAddressModels();
            var thisValidAddressModels = allValidAddressModels.Where(a => a.GroupName == fahrzeug);

            var addressOfThisFahrt = thisValidAddressModels.FirstOrDefault(a => a.SubGroupName == fahrtTyp);
            if (addressOfThisFahrt == null)
                return;

            var transportTyp = addressOfThisFahrt.TransportTyp;

            var fahrzeugModel = StepModels.OfType<Fahrzeug>().FirstOrDefault(fzg => fzg.FahrzeugIndex == fahrzeugIndex);
            if (fahrzeugModel == null)
                return;

            var zielAdresse = addressOfThisFahrt;
            var startFahrtTyp = "START";
            var startFahrzeug = fahrzeug;
            var zusatzFahrtVorhanden = thisValidAddressModels.Any(m => m.SubGroupName == "ZUSATZ");

            if (fahrzeug == "FAHRZEUG_1")
                if (zusatzFahrtVorhanden && fahrtTyp == "ZIEL")
                    startFahrtTyp = "ZUSATZ";

            if (fahrzeug == "FAHRZEUG_2")
                if (zusatzFahrtVorhanden && fahrtTyp == "ZIEL")
                    startFahrtTyp = "ZUSATZ";
                else
                {
                    // Start-Adresse für 2. Fahrzeug Adressen => u. U. Ziel-Adresse des 1. Fahrzeugs
                    startFahrzeug = "FAHRZEUG_1";
                    startFahrtTyp = "ZIEL";
                }

            var startAdresse =
                allValidAddressModels.FirstOrDefault(
                    m => m.GroupName == startFahrzeug && m.SubGroupName == startFahrtTyp);

            if (startAdresse == null)
                return;

            var fahrt = TryAddFahrt(transportTyp, fahrzeugModel, startAdresse, zielAdresse, sort);

            startAdresse.Fahrten.Add(fahrt);
            zielAdresse.Fahrten.Add(fahrt);
            zielAdresse.StartAdresseAsRouteInfo = startAdresse.AdresseAsRouteInfo;

            // only to ensure proper sorting of our "Fahrten"
            // ==> remove and then again add to the end of the list:
            Fahrten.Remove(fahrt);
            Fahrten.Add(fahrt);
        }

        private Fahrt TryAddFahrt(string transportTyp, Fahrzeug fahrzeug, Adresse startAdresse, Adresse zielAdresse, int sort)
        {
            var fahrt = Fahrten.FirstOrDefault(f => f.TypNr == transportTyp && f.FahrzeugIndex == fahrzeug.FahrzeugIndex && f.Sort == sort);
            if (fahrt != null)
            {
                fahrt.StartAdresse = startAdresse;
                fahrt.ZielAdresse = zielAdresse;

                fahrt.Fahrzeug = fahrzeug;

                return fahrt;
            }

            var transportTypModel = TransportTypen.FirstOrDefault(tt => tt.ID == transportTyp);

            fahrt = new Fahrt
                {
                    TypName = (transportTypModel == null ? "" : transportTypModel.Name),
                    TypNr = transportTyp,

                    Fahrzeug = fahrzeug,
                    StartAdresse = startAdresse,
                    ZielAdresse = zielAdresse,

                    //FahrtIndex = (Fahrten.Count + 1).ToString(),
                    Sort = sort,

                    Title = zielAdresse.FahrtTitleFromAddressType,
                    AvailableDienstleistungen = Dienstleistungen.Where(dl => dl.TransportTyp == transportTyp).ToList(),
                };
            Fahrten.Add(fahrt);

            return fahrt;
        }

        #endregion

    }
}
