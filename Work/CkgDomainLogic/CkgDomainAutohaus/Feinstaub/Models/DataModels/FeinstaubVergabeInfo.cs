using System;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Feinstaub.Models
{
    public class FeinstaubVergabeInfo
    {
        [LocalizedDisplay("Kennzeichen")]
        [Kennzeichen]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay("Erfassungsdatum")]
        public DateTime Erfassungsdatum { get; set; }

        [LocalizedDisplay("Plakettenart")]
        public string Plakettenart { get; set; }

        [LocalizedDisplay("Erfasser")]
        public string Erfasser { get; set; }

    }
}
