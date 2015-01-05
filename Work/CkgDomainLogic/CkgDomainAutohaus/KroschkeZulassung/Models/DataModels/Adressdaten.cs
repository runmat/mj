using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class Adressdaten
    {
        public string Kennung { get; set; }

        public string BelegNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Plz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        public bool AdresseVollstaendig { get { return (Name1.IsNotNullOrEmpty() && Strasse.IsNotNullOrEmpty() && Plz.IsNotNullOrEmpty() && Ort.IsNotNullOrEmpty()); } }

        public Adressdaten AdresseToAdressdaten(string belegNr, string kennung, Adresse adresse)
        {
            BelegNr = belegNr;
            Name1 = adresse.Name1;
            Name2 = adresse.Name2;
            Strasse = adresse.StrasseHausNr;
            Plz = adresse.PLZ;
            Ort = adresse.Ort;

            return this;
        }
    }
}
