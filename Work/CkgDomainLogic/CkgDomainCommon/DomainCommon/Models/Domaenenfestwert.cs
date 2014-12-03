
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class Domaenenfestwert
    {
        [SelectListKey]
        public string Wert { get; set; }

        [SelectListText]
        public string Beschreibung { get; set; }

        public string BeschreibungMitWert { get { return Wert + " " + Beschreibung; } }
    }
}
