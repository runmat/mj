using GeneralTools.Models;

namespace CkgDomainLogic.Equi.Models
{
    public class StuecklistenKomponente
    {
        public string Equipmentnummer { get; set; }

        public string Fahrgestellnummer { get; set; }

        public string Kennzeichen { get; set; }

        [SelectListKey]
        public string ID { get; set; }

        [SelectListText]
        public string Bezeichnung { get; set; }

        public string ErsatzteilKennung { get; set; }

        public string ErsatzteilKennungText { get; set; }

        public bool IstLagernd { get; set; }
    }
}
