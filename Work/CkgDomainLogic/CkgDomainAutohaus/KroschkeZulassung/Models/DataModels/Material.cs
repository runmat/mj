using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class Material
    {
        [LocalizedDisplay(LocalizeConstants.MaterialNo)]
        public string MaterialNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Material)]
        public string MaterialText { get; set; }

        public string Belegtyp { get; set; }
    }
}
