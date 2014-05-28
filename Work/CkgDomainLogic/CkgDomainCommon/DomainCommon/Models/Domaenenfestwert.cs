
namespace CkgDomainLogic.DomainCommon.Models
{
    public class Domaenenfestwert
    {
        public string Wert { get; set; }

        public string Beschreibung { get; set; }

        public string BeschreibungMitWert { get { return Wert + " " + Beschreibung; } }
    }
}
