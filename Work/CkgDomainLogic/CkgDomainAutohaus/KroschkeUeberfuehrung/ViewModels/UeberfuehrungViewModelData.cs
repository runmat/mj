using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Ueberfuehrung.Contracts;
using CkgDomainLogic.Ueberfuehrung.Models;
using CkgDomainLogic.Ueberfuehrung.Services;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Ueberfuehrung.ViewModels
{
    public partial class UeberfuehrungViewModel : CkgBaseViewModel
    {
        private int _suppressSaveModelForUiIndex = -1;


        [XmlIgnore]
        public IUeberfuehrungDataService DataService { get { return CacheGet<IUeberfuehrungDataService>(); } }

        [XmlIgnore]
        public GridItemsModel<Adresse> AdressenGridAuswahl { get; private set; }

        public Step[] Steps { get; set; }

        [XmlIgnore]
        public List<UiModel> StepForms
        {
            get { return Steps[Step - 1].CurrentSubStepForms; }
        }

        [XmlIgnore]
        public Step StepModel
        {
            get { return Steps[Step - 1]; }
        }

        public List<UiModel> GetStepForms(int step)
        {
            return Steps[step - 1].CurrentSubStepForms;
        }

        public UiModel GetStepForm(int uiIndex)
        {
            return StepForms[uiIndex - 1];
        }

        public void SetStepForm(int uiIndex, UiModel stepForm)
        {
            StepForms[uiIndex - 1] = stepForm;
        }

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

        [XmlIgnore]
        public bool RequestClearViewModel { get; set; }


        public void DataMarkForRefresh()
        {
            Laender = DataService.Laender;
            Adresse.Laender = Laender;

            RechnungsAdressen = DataService.GetRechnungsAdressen();
            RechnungsAdressen.ForEach(a =>
                {
                    a.TransportTypAvailable = false;
                    a.TransportTyp = "";
                    a.RechnungsAdressen = RechnungsAdressen;
                });

            StepFormsCollectionInit();
        }

        private void LoadDataStep2()
        {
            var fahrzeugStep = Steps.First(s => s.GroupName == "FAHRZEUGE");
            var rgAdresse = fahrzeugStep.AllSubStepForms.OfType<Adresse>().FirstOrDefault(adresse => adresse.SubGroupName == "RG");
            if (rgAdresse != null)
                DataService.AuftragGeber = rgAdresse.KundenNr;

            FahrtAdressen = DataService.GetFahrtAdressen(_addressTypes);
            AdressenGridAuswahl = new GridItemsModel<Adresse>
            {
                Items = FahrtAdressen,
                ControllerName = "Ueberfuehrung",
                AjaxSelectAction = "UeberfuehrungsAdressenAjaxBinding"
            };

            DataService.GetTransportTypenAndDienstleistungen(out _transportTypen, out _dienstleistungen);
            DienstleistungsAuswahl.AlleDienstleistungen = _dienstleistungen;
            Adresse.AlleTransportTypen = _transportTypen.CopyAndInsertAtTop(new TransportTyp { ID = "", Name = "(Bitte wählen)"});

            UploadFiles.WebUploadProtokolle = DataService.WebUploadProtokolle;
        }

        void CalcFahrten()
        {
            if (Fahrten == null)
                Fahrten = new List<Fahrt>();

            var fahrtenStep = Steps.First(s => s.GroupName == "FAHRTEN");
            var allValidAddressModels = fahrtenStep.CurrentSubStepForms.OfType<Adresse>().Where(m => !m.IsEmpty).ToList();
            allValidAddressModels.ForEach(address => address.Fahrten = new List<Fahrt>());

            // Fahrzeug 1: Abholung
            AddRemoveFahrt("FAHRZEUG_1", "START");

            // Fahrzeug 1: Auslieferung + Überführung
            AddRemoveFahrt("FAHRZEUG_1", "ZIEL");

            // Fahrzeug 1: Zusatzfahrt
            AddRemoveFahrt("FAHRZEUG_1", "ZUSATZ");

            // Fahrzeug 2: Rückholung
            AddRemoveFahrt("FAHRZEUG_2", "ZIEL");

            // Fahrzeug 2: Zusatzfahrt
            AddRemoveFahrt("FAHRZEUG_2", "ZUSATZ");

            // alle Fahrten entfernen, zu denen es keine Fahrt-Adresse mehr mit dem korrespondierenden Transport-Typ mehr gibt
            Fahrten.RemoveAll(fahrt => allValidAddressModels.None(address => address.TransportTyp == fahrt.TypNr));

            var dienstleistungsStep = Steps.First(s => s.GroupName == "DIENSTLEISTUNGEN");
            dienstleistungsStep.SubSteps = DienstleistungsFahrten.Select(f => f.DienstleistungsSubStep).ToList();
        }

        void AddRemoveFahrt(string fahrzeug, string fahrtTyp)
        {
            var fahrzeugIndex = fahrzeug.Substring(fahrzeug.Length - 1);
            var fahrtenStep = Steps.First(s => s.GroupName == "FAHRTEN");

            var allValidAddressModels = fahrtenStep.CurrentSubStepForms.OfType<Adresse>().Where(m => !m.IsEmpty);
            var thisValidAddressModels = allValidAddressModels.Where(a => a.GroupName == fahrzeug);

            var addressOfThisFahrt = thisValidAddressModels.FirstOrDefault(a => a.SubGroupName == fahrtTyp);
            if (addressOfThisFahrt == null)
                return;

            if (!addressOfThisFahrt.TransportTypAvailable)
                return;

            var transportTyp = addressOfThisFahrt.TransportTyp;

            var fahrzeugStep = Steps.First(s => s.GroupName == "FAHRZEUGE");
            var fahrzeugModel = fahrzeugStep.CurrentSubStepForms.OfType<Fahrzeug>().FirstOrDefault(fzg => fzg.FahrzeugIndex == fahrzeugIndex);
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

            var startAdresse = allValidAddressModels.FirstOrDefault(m => m.GroupName == startFahrzeug && m.SubGroupName == startFahrtTyp);

            if (startAdresse == null)
                return;

            var fahrt = TryAddFahrt(transportTyp, fahrzeugModel, startAdresse, zielAdresse);

            startAdresse.Fahrten.Add(fahrt);
            zielAdresse.Fahrten.Add(fahrt);
            
            // only to ensure proper sorting of our "Fahrten"
            // ==> remove and then again add to the end of the list:
            Fahrten.Remove(fahrt);
            Fahrten.Add(fahrt);
        }

        Fahrt TryAddFahrt(string transportTyp, Fahrzeug fahrzeug, Adresse startAdresse, Adresse zielAdresse)
        {
            var fahrt = Fahrten.FirstOrDefault(f => f.TypNr == transportTyp);
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

                                FahrtIndex = (Fahrten.Count + 1).ToString(),
                                Title = zielAdresse.FahrtTitleFromAddressType,
                                AvailableDienstleistungen = Dienstleistungen.Where(dl => dl.TransportTyp == transportTyp).ToList(),
                                DataService = DataService,
                                Step = Steps[Step - 1],
                            };
            fahrt.Init();

            Fahrten.Add(fahrt);

            return fahrt;
        }

        /// <summary>
        /// Copies only DERIVED properties from classes deriving from UiModel to object StepForms[i]
        /// (preserves the base properties of class UiModel in the destination object (-> StepForms[i]))
        /// </summary>
        public T SaveSubModelWithPreservingUiModel<T>(T subModel, int uiIndex) where T : UiModel, new()
        {
            if (uiIndex <= 0)
                return null;

            if (_suppressSaveModelForUiIndex == uiIndex)
            {
                _suppressSaveModelForUiIndex = -1;
                return (T)GetStepForm(uiIndex);
            }

            var savedUiModel = ModelMapping.Copy(GetStepForm(uiIndex), new UiModel());

            // Special case "Custom Errormessage": 
            // force Custom Errormessage on destination to empty if source model has an empty Custom Errormessage 
            if (subModel.CustomErrorMessage.IsNullOrEmpty() && savedUiModel.CustomErrorMessage.IsNotNullOrEmpty())
                subModel.CustomErrorMessage = savedUiModel.CustomErrorMessage = "";

            SetStepForm(uiIndex, subModel);
            ModelMapping.Copy(savedUiModel, GetStepForm(uiIndex));

            return (T)GetStepForm(uiIndex);
        }

        public void TryPreassignReToRgAdresse(Adresse selectedRgAdresse, out string reAdresseOpticalCheck)
        {
            reAdresseOpticalCheck = "";
            if (selectedRgAdresse.SubGroupName != "RG")
                return;

            var rgAdresse = RechnungsAdressen.FirstOrDefault(r => r.SubTyp == "RG" && r.ID == selectedRgAdresse.SelectedID);
            if (rgAdresse == null)
                return;

            var savedRgAdresse = (Adresse)GetStepForm(selectedRgAdresse.UiIndex);
            if (savedRgAdresse.ID == rgAdresse.ID)
                // Rg-Adresse didn't change:
                // ==> No preassignment for RE-Adresse here!
                return;

            var newReAdresse = RechnungsAdressen.FirstOrDefault(r => r.SubTyp == "RE" && r.KundenNr == rgAdresse.KundenNr);
            var savedReAdresse = StepForms.OfType<Adresse>().FirstOrDefault(a => a.SubGroupName == "RE");
            if (newReAdresse != null && savedReAdresse != null)
            {
                newReAdresse.SelectedID = newReAdresse.ID;
                SaveSubModelWithPreservingUiModel(newReAdresse, savedReAdresse.UiIndex);
                _suppressSaveModelForUiIndex = savedReAdresse.UiIndex;
                reAdresseOpticalCheck = (newReAdresse.IsEmpty ? "false" : "true");
            }
        }


        void StepFormsCollectionInit()
        {
            if (Steps == null)
            {
                Steps = new Step[StepMax];
                for (var i = 1; i <= Steps.Length; i++)
                {
                    var step = new Step();

                    switch (i)
                    {
                        case 1:
                            step.GroupName = "FAHRZEUGE";
                            break;

                        case 2:
                            step.GroupName = "FAHRTEN";
                            break;

                        case 3:
                            step.GroupName = "DIENSTLEISTUNGEN";
                            break;

                        case 4:
                            step.GroupName = "SUMMARY";
                            break;
                    }

                    Steps[i - 1] = step;
                }
            }

            StepFormsCollectionPrepareStep(1);
        }

        [XmlIgnore]
        public bool Step1Fahrzeug2IsEmpty { get; set; }
        [XmlIgnore]
        public bool Step1Fahrzeug2WasEmpty { get; set; }

        [XmlIgnore]
        public int Step2EmptyAddressCount { get; set; }
        [XmlIgnore]
        public int Step2EmptyAddressPrevCount { get; set; }

        [XmlIgnore]
        [LocalizedDisplay("Zusatzfahrten anzeigen")]
        public bool ShowZusatzfahrten { get; set; }

        void StepFormsCollectionPrepareStep(int stepNew = -1) //, int stepOld = -1)
        {
            //var stepMovingForward = (stepNew > stepOld);
            //var stepMovingBackward = !stepMovingForward;
            

            UiModel uiModel;
            Fahrzeug fahrzeug2;
            var index = 0;

            var thisStep = Steps[stepNew - 1];
            var thisStepFormsCollection = thisStep.CurrentSubStepForms;

            thisStepFormsCollection.ForEach(s => s.ValidationErrorMessage = "");

            switch (stepNew)
            {
                case 1:

                    if (thisStepFormsCollection.None())
                    {
                        index++;
                        uiModel = new Fahrzeug
                            {
                                FahrzeugIndex = "1",

                                UiIndex = index,
                                GroupName = "FAHRZEUGE",
                                SubGroupName = "FAHRZEUG_1",
                                Header = "Fahrzeug 1 (Hinfahrt)",
                                HeaderCssClass = string.Format("step2header"),
                                IsMandatory = true
                            };
                        thisStepFormsCollection.Add(uiModel);

                        index++;
                        uiModel = new Fahrzeug
                            {
                                FahrzeugIndex = "2",

                                UiIndex = index,
                                GroupName = "FAHRZEUGE",
                                SubGroupName = "FAHRZEUG_2",
                                Header = "Fahrzeug 2 (Rückfahrt)",
                                HeaderCssClass = string.Format("step1header"),
                            };
                        thisStepFormsCollection.Add(uiModel);

                        uiModel = RechnungsAdressen.FirstOrDefault(a => a.SubTyp == "RG");
                        if (uiModel != null)
                        {
                            index++;
                            uiModel.UiIndex = index;
                            uiModel.GroupName = "FAHRZEUGE";
                            uiModel.SubGroupName = "RG";
                            uiModel.Header = "Rechnungszahler";
                            uiModel.HeaderCssClass = string.Format("step3header");
                            uiModel.IsMandatory = true;
                            thisStepFormsCollection.Add(uiModel);
                        }

                        uiModel = RechnungsAdressen.FirstOrDefault(a => a.SubTyp == "RE");
                        if (uiModel != null)
                        {
                            index++;
                            uiModel.UiIndex = index;
                            uiModel.GroupName = "FAHRZEUGE";
                            uiModel.SubGroupName = "RE";
                            uiModel.Header = "Abweichende Rechnungsadresse";
                            uiModel.HeaderShort = "Abweich. Rechn.adresse";
                            uiModel.HeaderCssClass = string.Format("step4header");
                            uiModel.IsMandatory = true;
                            thisStepFormsCollection.Add(uiModel);
                        }
                    }

                    fahrzeug2 = thisStepFormsCollection.OfType<Fahrzeug>().FirstOrDefault(f => f.SubGroupName == "FAHRZEUG_2");
                    if (fahrzeug2 != null)
                    {
                        Step1Fahrzeug2WasEmpty = Step1Fahrzeug2IsEmpty;
                        Step1Fahrzeug2IsEmpty = fahrzeug2.IsEmpty;
                    }

                    break;

                case 2:

                    LoadDataStep2();

                    if (thisStepFormsCollection.None())
                    {
                        index++;
                        uiModel = new Adresse
                                      {
                                          TransportTypAvailable = false,

                                          UhrzeitwunschAvailable = true,
                                          Typ = AdressenTyp.FahrtAdresse,
                                          SubTyp = "ABHOLADRESSE",
                                          UiIndex = index,
                                          GroupName = "FAHRZEUG_1",
                                          SubGroupName = "START",
                                          Header = "Abholadresse",
                                          HeaderCssClass = string.Format("step{0}header", 4),
                                          IsMandatory = true,
                                      };
                        thisStepFormsCollection.Add(uiModel);

                        index++;
                        uiModel = new Adresse
                                      {
                                          TransportTypAvailable = true,
                                          TransportTyp = "4",

                                          UhrzeitwunschAvailable = false,
                                          Typ = AdressenTyp.FahrtAdresse,
                                          SubTyp = "AUSLIEFERUNG",
                                          UiIndex = index,
                                          GroupName = "FAHRZEUG_1",
                                          SubGroupName = "ZUSATZ",
                                          Header = "Zusatzfahrt Fahrzeug 1",
                                          HeaderCssClass = string.Format("step{0}header", 2),
                                          FormLayerAdditionalCssClass = "Zusatzfahrt hide",
                                      };
                        thisStepFormsCollection.Add(uiModel);

                        index++;
                        uiModel = new Adresse
                                      {
                                          TransportTypAvailable = true,
                                          TransportTyp = "1",

                                          UhrzeitwunschAvailable = true,
                                          Typ = AdressenTyp.FahrtAdresse,
                                          SubTyp = "AUSLIEFERUNG",
                                          UiIndex = index,
                                          GroupName = "FAHRZEUG_1",
                                          SubGroupName = "ZIEL",
                                          Header = "Zieladresse Fahrzeug 1 (z.B. Hinfahrt)",
                                          HeaderShort = "Zieladresse Fahrzeug 1",
                                          HeaderCssClass = string.Format("step{0}header", 5),
                                          IsMandatory = true,
                                      };
                        thisStepFormsCollection.Add(uiModel);
                    }

                    var fahrzeugModels = Steps.First(s => s.GroupName == "FAHRZEUGE").CurrentSubStepForms;
                    fahrzeug2 = fahrzeugModels.OfType<Fahrzeug>().FirstOrDefault(ui => ui.SubGroupName == "FAHRZEUG_2");

                    if (fahrzeug2 == null || fahrzeug2.IsEmpty)
                    {
                        // delete all Adresses for "Fahrzeug 2"
                        thisStepFormsCollection.RemoveAll(ui => ui.GroupName == "FAHRZEUG_2");
                    }
                    else if (thisStepFormsCollection.None(ui => ui.GroupName == "FAHRZEUG_2"))
                    {
                        index = thisStepFormsCollection.Count;

                        index++;
                        uiModel = new Adresse
                                      {
                                          TransportTypAvailable = true,
                                          UhrzeitwunschAvailable = false,
                                          Typ = AdressenTyp.FahrtAdresse,
                                          SubTyp = "RÜCKHOLUNG",
                                          UiIndex = index,
                                          GroupName = "FAHRZEUG_2",
                                          SubGroupName = "ZUSATZ",
                                          Header = "Zusatzfahrt Fahrzeug 2",
                                          HeaderCssClass = string.Format("step{0}header", 2),
                                          FormLayerAdditionalCssClass = "Zusatzfahrt hide",
                                      };
                        thisStepFormsCollection.Add(uiModel);

                        index++;
                        uiModel = new Adresse
                                      {
                                          TransportTypAvailable = true,
                                          TransportTyp = "3",

                                          UhrzeitwunschAvailable = true,
                                          Typ = AdressenTyp.FahrtAdresse,
                                          SubTyp = "RÜCKHOLUNG",
                                          UiIndex = index,
                                          GroupName = "FAHRZEUG_2",
                                          SubGroupName = "ZIEL",
                                          Header = "Zieladresse Fahrzeug 2 (z.B. Rückfahrt)",
                                          HeaderShort = "Zieladresse Fahrzeug 2",
                                          HeaderCssClass = string.Format("step{0}header", 5),
                                          IsMandatory = true,
                                      };
                        thisStepFormsCollection.Add(uiModel);
                    }

                    fahrzeug2 = thisStepFormsCollection.OfType<Fahrzeug>().FirstOrDefault(f => f.SubGroupName == "FAHRZEUG_2");
                    if (fahrzeug2 != null)
                    {
                        Step1Fahrzeug2WasEmpty = Step1Fahrzeug2IsEmpty;
                        Step1Fahrzeug2IsEmpty = fahrzeug2.IsEmpty;
                    }

                    var fahrzeugMajorChanges = (Step1Fahrzeug2WasEmpty != Step1Fahrzeug2IsEmpty);
                    if (fahrzeugMajorChanges && FromSummaryChangeStep > 0)
                    {
                        FromSummaryMajorChangeOccured = true;

                        var firstModel = thisStepFormsCollection.OfType<Adresse>().First();
                        firstModel.CustomErrorMessage = "Fahrzeugänderungen vorhanden, bitte die Fahrten anpassen!";

                        //ClearFahrten();

                        Step1Fahrzeug2WasEmpty = Step1Fahrzeug2IsEmpty;
                    }

                    Step2EmptyAddressPrevCount = Step2EmptyAddressCount;
                    Step2EmptyAddressCount = thisStepFormsCollection.OfType<Adresse>().Count(f => f.IsEmpty);
                    break;

                case 3:

                    CalcFahrten();

                    var fahrt = DienstleistungsFahrten[SubStep - 1];
                    if (fahrt == null)
                        return;

                    if (SubStep == 1)
                    {
                        var addressesMajorChanges = (Step2EmptyAddressPrevCount != Step2EmptyAddressCount);
                        if (addressesMajorChanges && (FromSummaryChangeStep > 0 && FromSummaryChangeStep != stepNew))
                        {
                            FromSummaryMajorChangeOccured = true;

                            fahrt.CustomErrorMessage = "Anzahl Fahrten geändert, bitte die Dienstleistungen anpassen!";

                            Step2EmptyAddressPrevCount = Step2EmptyAddressCount;
                        }
                    }
                    break;


                case 4:

                    // clear all preceding "from summary" markers, because we are in the summary again here:
                    FromSummaryChangeStep = 0;
                    FromSummaryMajorChangeOccured = false;

                    Step1Fahrzeug2WasEmpty = Step1Fahrzeug2IsEmpty;
                    Step2EmptyAddressPrevCount = Step2EmptyAddressCount;

                    // Init summary for all "drives"
                    Fahrten.ForEach(f => f.InitSummary());

                    thisStepFormsCollection.Clear();

                    index++;
                    var summary = new Summary
                                    {
                                        UiIndex = index,
                                        GroupName = "SUMMARY",
                                        Header = "Ihr Auftrag in der Übersicht",
                                        HeaderCssClass = string.Format("step{0}header", 5),
                                        StepMax = StepMax,
                                    };
                    summary.InitSummaryItems(GetSummaryDescriptionForStep, GetSummaryHeaderForStep);
                    thisStepFormsCollection.Add(summary);
                    break;
            }
        }

        public UiModel CurrentStepFirstModelMandatoryButEmpty
        {
            get
            {
                var stepFormNotValid = StepForms.FirstOrDefault(model => model.IsMandatory && model.IsEmpty);
                if (stepFormNotValid != null)
                {
                    stepFormNotValid.ValidationErrorMessage = "Bitte füllen Sie dieses Formular aus.";
                    return stepFormNotValid;
                }

                return null;
            }
        }

        public UiModel CurrentStepFirstModelWithDependencyError
        {
            get
            {
                switch (Step)
                {
                    case 1:
                        //
                        // "Fahrzeug" Step:
                        //
                        // ... no dependencies here ...
                        break;

                    case 2:
                        //
                        // "Fahrten" Step:
                        //
                        // check destination address of car no. 2:
                        var validAddressModelsFahrzeug2 = StepForms.OfType<Adresse>().Where(ui => ui.GroupName == "FAHRZEUG_2" && !ui.IsEmpty);
                        var invalidModelsFahrzeug2 = StepForms.OfType<Adresse>().Where(ui => ui.GroupName == "FAHRZEUG_2" && ui.IsEmpty);
                        if (validAddressModelsFahrzeug2.Count() == 1 && validAddressModelsFahrzeug2.First().SubGroupName != "ZIEL")
                        {
                            var stepFormNotValid = invalidModelsFahrzeug2.First();
                            if (stepFormNotValid != null)
                            {
                                stepFormNotValid.ValidationErrorMessage = "Fahrzeug 2 - bitte auch die Zieladresse angeben.";
                                return stepFormNotValid;
                            }
                        }

                        // check over all "Fahrten" valid pickup dates:
                        for (var fahrzeugIndex = 1; fahrzeugIndex <= 2; fahrzeugIndex++)
                        {
                            Adresse invalidPickupDateAddressModel;
                            string invalidPickupDateErrorMessage;

                            GetPickupDateValidationErrorFirstAddress(fahrzeugIndex, out invalidPickupDateAddressModel, out invalidPickupDateErrorMessage);
                            if (invalidPickupDateAddressModel != null)
                            {
                                invalidPickupDateAddressModel.ValidationErrorMessage = invalidPickupDateErrorMessage;
                                invalidPickupDateAddressModel.ValidationDependencyErrorFirstProperty = "Datum";
                                return invalidPickupDateAddressModel;
                            }
                        }

                        break;
                    
                    case 3:
                        //
                        // "Dienstleistungs" Step:
                        //
                        // Spezielle Fahrtdatums-Validierungen:
                        // - Bei Vorholung muss vom User explizit(!) die Dienstleistung "Vorholung" gewählt werden:
                        // - Bei Samstag-Liefertermin muss vom User explizit(!) die Dienstleistung "Samstags Auslieferung" gewählt werden:
                        //
                        var invalidDienstleistungsAuswahl = CheckDienstleistungsDatumsWerteSamstagsAuslieferung();
                        if (invalidDienstleistungsAuswahl != null)
                            return invalidDienstleistungsAuswahl;

                        invalidDienstleistungsAuswahl = CheckDienstleistungsDatumsWerteVorholung();
                        if (invalidDienstleistungsAuswahl != null)
                            return invalidDienstleistungsAuswahl;
                        
                        break;
                }

                return null;
            }
        }

        void GetPickupDateValidationErrorFirstAddress(int fahrzeug, out Adresse invalidAddressModel, out string errorMessage)
        {
            invalidAddressModel = null;
            errorMessage = "";

            var validFahrzeugModelsWithDate = StepForms.OfType<Adresse>().Where(ui => !ui.IsEmpty && ui.Datum != null);
            var fahrzeug1ValidAddressModelsWithDate = validFahrzeugModelsWithDate.Where(ui => ui.GroupName == "FAHRZEUG_1");
            var thisFahrzeugValidAddressModelsWithDate = validFahrzeugModelsWithDate.Where(ui => ui.GroupName == "FAHRZEUG_" + fahrzeug);
            
            var addressAbhol = thisFahrzeugValidAddressModelsWithDate.FirstOrDefault(a => a.SubGroupName == "START");
            var labelAbhol = "Abholfahrt";
            if (fahrzeug == 2)
            {
                addressAbhol = fahrzeug1ValidAddressModelsWithDate.FirstOrDefault(a => a.SubGroupName == "ZIEL");
                labelAbhol = "Zielfahrt Fzg. 1";
            }
            var addressZusatz = thisFahrzeugValidAddressModelsWithDate.FirstOrDefault(a => a.SubGroupName == "ZUSATZ");
            var addressZiel = thisFahrzeugValidAddressModelsWithDate.FirstOrDefault(a => a.SubGroupName == "ZIEL");

            if (addressZusatz == null && addressZiel == null)
                return;

            if (addressAbhol != null)
            {
                if (addressZusatz != null)
                    if (addressZusatz.Datum.GetValueOrDefault() < addressAbhol.Datum.GetValueOrDefault())
                    {
                        invalidAddressModel = addressZusatz;
                        errorMessage = string.Format("Bitte Zusatzfahrt Datum nach {0}, {1} terminieren", labelAbhol, addressAbhol.Datum.GetValueOrDefault().ToString("dd.MM.yy"));
                    }
                if (addressZiel != null)
                    if (addressZiel.Datum.GetValueOrDefault() < addressAbhol.Datum.GetValueOrDefault())
                    {
                        invalidAddressModel = addressZiel;
                        errorMessage = string.Format("Bitte Zielfahrt Datum nach {0}, {1} terminieren", labelAbhol, addressAbhol.Datum.GetValueOrDefault().ToString("dd.MM.yy"));
                    }
            }
            if (addressZusatz != null)
            {
                if (addressAbhol != null)
                    if (addressZusatz.Datum.GetValueOrDefault() < addressAbhol.Datum.GetValueOrDefault())
                    {
                        invalidAddressModel = addressZusatz;
                        errorMessage = string.Format("Bitte Zusatzfahrt Datum nach {0}, {1} terminieren", labelAbhol, addressAbhol.Datum.GetValueOrDefault().ToString("dd.MM.yy"));
                    }
                if (addressZiel != null)
                    if (addressZiel.Datum.GetValueOrDefault() < addressZusatz.Datum.GetValueOrDefault())
                    {
                        invalidAddressModel = addressZiel;
                        errorMessage = string.Format("Bitte Zielfahrt Datum nach Zusatzfahrt, {0} terminieren", addressZusatz.Datum.GetValueOrDefault().ToString("dd.MM.yy"));
                    }
            }
        }

        /// <summary>
        /// Bei Samstag-Liefertermin muss vom User explizit(!) die Dienstleistung "Samstags Auslieferung" gewählt werden
        /// </summary>
        DienstleistungsAuswahl CheckDienstleistungsDatumsWerteSamstagsAuslieferung()
        {
            return CheckDienstleistungsDatumsWerte(
                        "Samstagsauslieferung",
                        "Diese Fahrt geht über einen Samstag, bitte wählen Sie die Dienstleistung '{0}'.",
                        dateList => dateList.Any(date => date != null && date.GetValueOrDefault().DayOfWeek == DayOfWeek.Saturday));
        }

        /// <summary>
        /// Bei unterschiedlichen Tagen der Eigenschaft Adresse.Datum aller Fahrt-Adressen eines Fahrzeugs...
        /// ==> muss die Dienstleistung "Vorholung" gewählt werden
        /// </summary>        
        private DienstleistungsAuswahl CheckDienstleistungsDatumsWerteVorholung()
        {
            return CheckDienstleistungsDatumsWerte(
                        "Vorholung",
                        "Diese Fahrt geht über unterschiedliche Tage, bitte wählen Sie die Dienstleistung '{0}'.",
                        dateList => dateList.Where(date => date != null).Distinct().Count() > 1);
        }

        /// <summary>
        /// Bei unterschiedlichen Tagen der Eigenschaft Adresse.Datum aller Fahrt-Adressen eines Fahrzeugs...
        /// ==> muss die Dienstleistung "Vorholung" gewählt werden
        /// </summary>        
        DienstleistungsAuswahl CheckDienstleistungsDatumsWerte(string dienstleistungsNameToCheck, string validationMessage, Func<IEnumerable<DateTime?>, bool> datesInvalidFunc)
        {
            var dienstleistungsAuswahl = StepForms.OfType<DienstleistungsAuswahl>().FirstOrDefault();
            if (dienstleistungsAuswahl == null)
                return null;

            var fahrt = Fahrten.FirstOrDefault(f => f.FahrtIndex == dienstleistungsAuswahl.FahrtIndex);
            if (fahrt == null)
                return null;

            var fahrzeug = fahrt.Fahrzeug;
            var alleFahrtenDiesesFahrzeugs = Fahrten.Where(f => f.FahrzeugIndex == fahrzeug.FahrzeugIndex);

            var hauptFahrtModel = alleFahrtenDiesesFahrzeugs.FirstOrDefault(f => f.IstHauptFahrt);
            if (hauptFahrtModel == null)
                return null;

            var hauptFahrtDienstleistungsAuswahl = hauptFahrtModel.GetDienstleistungsAuswahl();

            var hauptFahrtDienstleistungToCheck = hauptFahrtDienstleistungsAuswahl.AvailableDienstleistungen.FirstOrDefault(d => d.Name.ToLower().StartsWith(dienstleistungsNameToCheck.ToLower()));
            if (hauptFahrtDienstleistungToCheck == null)
                return null;

            var allDatesToCheck = alleFahrtenDiesesFahrzeugs.SelectMany(f => new List<DateTime?> { f.StartAdresse.Datum, f.ZielAdresse.Datum });

            var datesInvalid = datesInvalidFunc(allDatesToCheck);

            if (datesInvalid && hauptFahrtDienstleistungsAuswahl.GewaehlteDienstleistungen.None(dl => dl.ID == hauptFahrtDienstleistungToCheck.ID))
            {
                hauptFahrtDienstleistungsAuswahl.ValidationErrorMessage = string.Format(validationMessage, hauptFahrtDienstleistungToCheck.Name);
                hauptFahrtDienstleistungsAuswahl.ValidationDependencyErrorFirstProperty = string.Format("dl2{0}", hauptFahrtDienstleistungToCheck.ID);
                return hauptFahrtDienstleistungsAuswahl;
            }

            return null;
        }


        #region Summary
        
        public string GetSummaryHeaderForStep(int step)
        {
            return StepBarHeadersAbbreviates[step - 1];
        }

        public List<GeneralEntity> GetSummaryDescriptionForStep(int step)
        {
            var ds = new List<GeneralEntity>();

            var stepModel = Steps[step - 1];
            stepModel.AllSubStepForms.ForEach(ui => TryAddUiModelSummary(ui, ds));

            return ds;
        }

        static void TryAddUiModelSummary(UiModel uiModel, List<GeneralEntity> dict)
        {
            if ((!uiModel.IsEmpty || uiModel.ParallelSummaryUiModels.AnyAndNotNull()) && !uiModel.IgnoreSummaryItem)
                dict.Add(uiModel.SummaryItem);
        }

        #endregion


        public RoutingInfo GetGeoLocationInfo(int uiIndex)
        {
            var adresse = (Adresse)GetStepForm(uiIndex);
            if (adresse == null)
                return null;

            var routingInfo = new RoutingInfo
            {
                StartAdresse = adresse,
                ZielAdresse = null,
            };

            return routingInfo;
        }

        public RoutingInfo GetRoutingInfo(int uiIndex)
        {
            CalcFahrten();

            var zielAdresse = (Adresse)GetStepForm(uiIndex);
            var fahrt = zielAdresse.Fahrten.FirstOrDefault(f => zielAdresse.GroupName == "FAHRZEUG_" + f.FahrzeugIndex && f.ZielAdresse.AdresseAsRouteInfoComplete == zielAdresse.AdresseAsRouteInfoComplete);

            var startAdresse = (fahrt == null ? null : fahrt.StartAdresse);

            if (startAdresse == null)
                return null;

            var routingInfo = new RoutingInfo
            {
                StartAdresse = startAdresse,
                ZielAdresse = zielAdresse,
            };

            return routingInfo;
        }

        [XmlIgnore]
        public bool SaveStarted { get; set; }

        [XmlIgnore]
        public string ReceiptPdfFileName { get; set; }

        [XmlIgnore]
        public string ReceiptErrorMessages { get; set; }

        public bool TryStartSave(int increment)
        {
            if (Step < StepMax || increment < 0)
                return false;

            SaveStarted = true;

            return true;
        }

        private void CopyProtocolFiles(List<UeberfuehrungsAuftragsPosition> auftragsPositionen)
        {
            var dienstleistungsStep = Steps.First(s => s.GroupName == "DIENSTLEISTUNGEN");
            var protocolCollections = dienstleistungsStep.AllSubStepForms.OfType<UploadFiles>().ToList();
            protocolCollections.ForEach(protocolCollection =>
                {
                    for (var i = 0; i < protocolCollection.UploadFileNameArray.Length; i++)
                    {
                        var uploadFileName = protocolCollection.UploadFileNameArray[i];
                                        
                        if (uploadFileName.IsNullOrEmpty())
                            return;

                        var auftrag = auftragsPositionen.FirstOrDefault(ap => ap.FahrtIndex == protocolCollection.FahrtIndex);
                        if (auftrag == null)
                            return;

                        var uploadProtocol = protocolCollection.UploadProtokolle.FirstOrDefault(up => up.Protokollart == protocolCollection.UploadGroupNameArray[i]);
                        if (uploadProtocol == null)
                            return;

                        var srcPath = DataService.GetUploadPathTemp();
                        var srcFullFileName = Path.Combine(srcPath, uploadFileName);

                        var dstPath = Path.Combine(AppSettings.UploadFilePath, LogonContext.KundenNr, auftrag.AuftragsNr.PadLeft(10, '0'), "Vertraege");
                        var dstFileName = string.Format("{0}_{1}_{2}_{3}.pdf", 
                                                auftrag.AuftragsNr, auftrag.FahrtIndex, uploadProtocol.Kategorie, uploadProtocol.Protokollart.Replace(".", ""));
                        var dstFullFileName = Path.Combine(dstPath, dstFileName);

                        if (!FileService.TryDirectoryCreate(dstPath))
                            throw new IOException(string.Format("Fehler, Upload-Zielverzeichnis kann nicht erstellt werden: '{0}'", dstPath));

                        if (!FileService.TryFileCopy(srcFullFileName, dstFullFileName))
                            throw new IOException(string.Format("Fehler, Upload-Datei kann nicht kopiert werden von '{0}' => '{1}'", srcFullFileName, dstFullFileName));

                        //if (!FileService.TryFileDelete(srcFullFileName))
                        //    throw new IOException(string.Format("Fehler, Upload-Quelldatei kann nicht gelöscht werden: '{0}'", srcFullFileName));
                        
                        // ToDo: Enable temp file deletion
                        //FileService.TryFileDelete(srcFullFileName);
                    }
                });
        }

        public void SaveAll()
        {
            //XmlService.XmlSerializeToFile(this, Path.Combine(AppSettings.DataPath, @"vmUeberfuehrung_01.xml"));

            AuftragsPositionen = DataService.Save(Steps, Fahrten);
            CopyProtocolFiles(AuftragsPositionen);
            ReceiptPdfFileName = new ReceiptCreationService(this).CreatePDF();
            
            ReceiptErrorMessages = "";
            var invalidPositions = AuftragsPositionen.Where(ap => !ap.IsValid);
            if (invalidPositions.Any())
                ReceiptErrorMessages = string.Join("<br />", invalidPositions.Select(ap => ap.AuftragsNrText));

            SaveStarted = false;
        }

        public bool SaveUploadFile(HttpPostedFileBase file, string fahrtIndex, out string fileName,
                                   out string errorMessage)
        {
            return DataService.SaveUploadFile(file, fahrtIndex, out fileName, out errorMessage);
        }

        #region Test

        [XmlIgnore]
        static public Fahrzeug DefaultFahrzeug
        {
            get
            {
                return new Fahrzeug
                {
                    FahrzeugIndex = "1",

                    FIN = "WVWZZZ1KZCW072843",
                    KennzeichenOrt = "OD",
                    KennzeichenRest = "J 104",
                    Typ = "Audi A4",
                    Fahrzeugwert = "Z50",

                    FahrzeugZugelassen = "N",
                    ZulassungsauftragAnDAD = "J",
                    Bereifung = "W",
                    Fahrzeugklasse = "PKW",
                };
            }
        }

        [XmlIgnore]
        static public Fahrzeug DefaultFahrzeug2
        {
            get
            {
                return new Fahrzeug
                {
                    FahrzeugIndex = "2",

                    FIN = "WAUZZZ4G2CN156019",
                    KennzeichenOrt = "RZ",
                    KennzeichenRest = "XY 3568",
                    Typ = "Bulli",
                    Fahrzeugwert = "Z00",

                    FahrzeugZugelassen = "J",
                    ZulassungsauftragAnDAD = "N",
                    Bereifung = "S",
                    Fahrzeugklasse = "LKW",
                };
            }
        }

        public void TestRaiseError()
        {
            throw new Exception("Error at ViewModel 'TestRaiseError'");
        }

        #endregion
    }
}