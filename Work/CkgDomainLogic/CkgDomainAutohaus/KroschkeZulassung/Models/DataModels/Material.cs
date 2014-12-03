using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class Material
    {
        [SelectListKey]
        [LocalizedDisplay(LocalizeConstants.MaterialNo)]
        public string MaterialNr { get; set; }

        [SelectListText]
        [LocalizedDisplay(LocalizeConstants.Material)]
        public string MaterialText { get; set; }

        public string Belegtyp { get; set; }
    }
}
