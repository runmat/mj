using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
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

        public bool IstAbmeldung { get { return Belegtyp.NotNullOrEmpty().ToUpper() == "AA"; } }
    }
}
