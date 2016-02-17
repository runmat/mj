using GeneralTools.Models;

namespace CkgDomainLogic.Equi.Models
{
    public class StuecklistenKomponente
    {
        public string Equipmentnummer { get; set; }

        public string Fahrgestellnummer { get; set; }

        public string Kennzeichen { get; set; }

        public string Nr { get; set; }

        [SelectListText]
        public string Bezeichnung { get; set; }

        public string ErsatzteilKennung { get; set; }

        public string ErsatzteilKennungText { get; set; }

        public string Status { get; set; }

        public bool IstLagernd { get; set; }

        [ModelMappingCompareIgnore]
        public bool EntgueltigVersandt { get; set; }

        [SelectListKey]
        public string UniqueId { get { return string.Format("{0}-{1}",  Fahrgestellnummer, Nr); } }

        public bool IsSelected { get; set; }
    }
}
