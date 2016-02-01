using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    [GridColumnsAutoPersist]
    public class StuecklistenKomponente
    {
        public string Equipmentnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        public string Nr { get; set; }

        [SelectListText]
        [LocalizedDisplay(LocalizeConstants.Description)]
        public string Bezeichnung { get; set; }

        public string ErsatzteilKennung { get; set; }

        public string ErsatzteilKennungText { get; set; }

        public bool IstLagernd { get; set; }


        [SelectListKey]
        public string UniqueId { get { return string.Format("{0}-{1}",  Fahrgestellnummer, Nr); } }

        public bool IsSelected { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActionDot)]
        public string Action { get; set; }
    }
}
