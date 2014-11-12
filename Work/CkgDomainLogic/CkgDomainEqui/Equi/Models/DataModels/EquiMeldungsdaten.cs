using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiMeldungsdaten
    {
        [LocalizedDisplay(LocalizeConstants.MessageNo)]
        public string Meldungsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ExecutionDate)]
        public DateTime? Durchfuehrungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.AcquisitionDate)]
        public DateTime? Erfassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Process)]
        public string Vorgang { get; set; }

        [LocalizedDisplay(LocalizeConstants.InstructedBy)]
        public string BeauftragtDurch { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Strasse { get; set; }

        public string Hausnummer { get; set; }

        public string Plz { get; set; }

        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingAddress)]
        public string Versandadresse
        {
            get
            {
                return String.Format("{0} {1}" + (String.IsNullOrEmpty(Strasse) && String.IsNullOrEmpty(Hausnummer) ? "" : Environment.NewLine)
                    + "{2} {3}" + (String.IsNullOrEmpty(Plz) && String.IsNullOrEmpty(Ort) ? "" : Environment.NewLine)
                    + "{4} {5}",
                    Name1, Name2,
                    Strasse, Hausnummer,
                    Plz, Ort);
            }
        }

        [LocalizedDisplay(LocalizeConstants.DispatchType)]
        public string Versandart { get; set; }
    }
}
