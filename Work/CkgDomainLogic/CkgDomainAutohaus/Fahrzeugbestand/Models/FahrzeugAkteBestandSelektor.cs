using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeugbestand.Models
{
    public class FahrzeugAkteBestandSelektor
    {
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.FactoryName)]
        public string FabrikName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Holder)]
        public string Halter { get; set; }

        [LocalizedDisplay(LocalizeConstants.Buyer)]
        public string Kaeufer { get; set; }
    }
}
