using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class KennzeichenSerie
    {
        [SelectListKey]
        [LocalizedDisplay(LocalizeConstants.Id)]
        public string ID { get; set; }

        [SelectListText]
        [LocalizedDisplay(LocalizeConstants.LicenseNoSeries)]
        public string Name { get; set; }


        public string Art { get; set; }

        public string OrtsKennzeichen { get; set; }

        public string BuchstabenTeilMin { get; set; }

        public string BuchstabenTeilMax { get; set; }

        public string NummernTeilMin { get; set; }

        public string NummernTeilMax { get; set; }

        public string SonderSerie { get; set; }
    }
}
