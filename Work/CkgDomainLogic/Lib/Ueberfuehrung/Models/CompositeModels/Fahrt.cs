using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.Ueberfuehrung.Contracts;
using System.Linq;
using GeneralTools.Models;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class Fahrt
    {
        [XmlIgnore]
        public IUeberfuehrungDataService DataService { get; set; }

        public string TypName { get; set; }

        public string TypNr { get; set; }

        public string Title { get; set; }

        public string FahrtIndex { get; set; }

        [XmlIgnore]
        public string FahrzeugIndex { get { return Fahrzeug == null ? "1" : Fahrzeug.FahrzeugIndex; } }

        [XmlIgnore]
        public string ReihenfolgeTmp { get { return FahrtIndex.PadLeft(6, '0'); } }

        public Fahrzeug Fahrzeug { get; set; }

        public Adresse StartAdresse { get; set; }

        public Adresse ZielAdresse { get; set; }

        public string VorgangsNummer { get; set; }

        public List<Dienstleistung> AvailableDienstleistungen { get; set; }

        public Step Step { get; set; }

        public SubStep DienstleistungsSubStep { get; set; }

        [XmlIgnore]
        public string CustomErrorMessage
        { 
            get { return GetDienstleistungsAuswahl().CustomErrorMessage; }
            set { GetDienstleistungsAuswahl().CustomErrorMessage = value; }
        }

        [XmlIgnore]
        public DateTime? Datum { get { return ZielAdresse == null ? null : ZielAdresse.Datum; } }

        [XmlIgnore]
        public string Uhrzeit { get { return ZielAdresse == null ? "" : ZielAdresse.Uhrzeitwunsch; } }

        [XmlIgnore]
        public string EmptyString { get { return ""; } }

        [XmlIgnore]
        public bool IstZusatzFahrt { get { return Adresse.AlleTransportTypen.Any(at => at.IstZusatzTransport && at.ID == TypNr); } }

        [XmlIgnore]
        public bool IstHauptFahrt { get { return !IstZusatzFahrt; } }

        public void Init()
        {
            DienstleistungsSubStep = new SubStep { Title = Title };

            var thisStepFormsCollection = DienstleistungsSubStep.StepForms;

            if (thisStepFormsCollection.None())
            {
                var index = 0;
                UiModel uiModel;

                index++;
                uiModel = new DienstleistungsAuswahl
                              {
                                  FahrtTyp = TypNr,
                                  FahrtIndex = FahrtIndex,
                                  UiIndex = index,
                                  GroupName = "DIENSTLEISTUNGEN",
                                  Header = string.Format("{0}, Leistungen", Title),
                                  HeaderCssClass = string.Format("step{0}header", 6),

                                  IgnoreSummaryItem = true,
                              };
                thisStepFormsCollection.Add(uiModel);

                index++;
                uiModel = new Bemerkungen
                              {
                                  UiIndex = index,
                                  FahrtIndex = FahrtIndex,
                                  GroupName = "BEMERKUNGEN",
                                  Header = string.Format("{0}, Bemerkungen", Title),
                                  HeaderShort = string.Format("{0}, Ihre Bemerkungen", Title),
                                  HeaderCssClass = string.Format("step{0}header", 7),

                                  IgnoreSummaryItem = true,
                              };
                thisStepFormsCollection.Add(uiModel);

                index++;
                uiModel = new UploadFiles
                              {
                                  UiIndex = index,
                                  FahrtIndex = FahrtIndex,
                                  GroupName = "UPLOAD_FILES",
                                  Header = string.Format("{0}, zusätzliche Dokumente", Title),
                                  HeaderCssClass = string.Format("step{0}header", 8),

                                  UploadGroupNames = string.Join(",", DataService.WebUploadProtokolle.Select(p => p.Protokollart).ToList()),
                              };

                thisStepFormsCollection.Add(uiModel);
            }

            InitAuswahlDienstleistungen();
        }

        public void InitSummary()
        {
            var thisStepFormsCollection = DienstleistungsSubStep.StepForms;
            // merging summary items
            thisStepFormsCollection.OfType<UploadFiles>().First().ParallelSummaryUiModels = new List<UiModel> { thisStepFormsCollection.OfType<DienstleistungsAuswahl>().First() };
        }

        public DienstleistungsAuswahl GetDienstleistungsAuswahl()
        {
            var thisStepFormsCollection = DienstleistungsSubStep.StepForms;

            return thisStepFormsCollection.OfType<DienstleistungsAuswahl>().FirstOrDefault();
        }

        public void InitAuswahlDienstleistungen(bool forceInit=false)
        {
            var dienstleistungsAuswahl = GetDienstleistungsAuswahl();

            if (forceInit || !dienstleistungsAuswahl.DienstleistungenInitialized)
                dienstleistungsAuswahl.InitAuswahlDienstleistungen(AvailableDienstleistungen);
        }
    }
}
