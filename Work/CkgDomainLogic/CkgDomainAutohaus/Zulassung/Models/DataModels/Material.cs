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

        public bool IstAbmeldung { get; set; }

        public bool IstVersand { get; set; }

        public bool Auf48hVersandPruefen { get; set; }

        public bool ZulassungAmFolgetagNichtMoeglich { get; set; }

        public bool SimuliereVersand { get; set; }
    }
}
