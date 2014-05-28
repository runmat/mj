using System.Xml.Serialization;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class UeberfuehrungsAuftragsPosition
    {
        public string AuftragsNr { get; set; }

        public string FahrtIndex { get; set; }

        public string Bemerkung { get; set; }

        public bool IsValid { get { return !string.IsNullOrEmpty(AuftragsNr); } }

        [XmlIgnore]
        public string AuftragsNrText
        {
            get
            {
                var auftragsText = AuftragsNr;
                if (!IsValid)
                    auftragsText = string.Format("SAP Fehlermeldung: {0}", Bemerkung);
                return auftragsText;
            }
        }
    }
}
