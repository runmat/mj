
namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class Zusatzdienstleistung
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string BelegNr { get; set; }

        public string PositionsNr { get; set; }

        public string Menge { get; set; }

        public bool IstGewaehlt { get; set; }

        public string SelectedAsString { get { return IstGewaehlt ? "selected" : ""; } }
    }
}
