using System;
using System.Collections.Generic;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiHistorieVermieter
    {
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string Modell { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string ModellAsText { get { return string.Format("{0}{1}", Modell, (Einsteuerungsdaten != null ? Einsteuerungsdaten.ModellName.PrependIfNotNull(" ") : "")); } }

        [LocalizedDisplay(LocalizeConstants.Color)]
        public string Farbe { get; set; }

        [LocalizedDisplay(LocalizeConstants.ColorCode)]
        public string Farbcode { get; set; }

        [LocalizedDisplay(LocalizeConstants.Color)]
        public string FarbeText
        {
            get
            {
                if (string.IsNullOrEmpty(Farbcode))
                    return Farbe;

                return string.Format("{0} ({1})", Farbe, Farbcode);
            }
        }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string BriefNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? Erstzulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Cancellation)]
        public DateTime? Abmeldedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo_VehicleHistory)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1_VehicleHistory)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2_VehicleHistory)]
        public string Referenz2 { get; set; }

        public EquiHistorieVermieterInfo HistorieInfo { get; set; }

        public EquiEinsteuerung Einsteuerungsdaten { get; set; }

        public EquiAussteuerung Aussteuerungsdaten { get; set; }

        public EquiTypdaten Typdaten { get; set; }

        public List<EquiMeldungsdaten> LebenslaufZb2 { get; set; }

        public List<EquiMeldungsdaten> LebenslaufFsm { get; set; }

        public List<EquiTueteninhalt> InhalteFsm { get; set; }

        public List<FahrzeugAnforderung> FahrzeugAnforderungen { get; set; }

        public bool FahrzeugAnforderungenAnzeigen { get; set; }
    }
}
