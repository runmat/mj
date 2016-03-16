using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models
{
    public class EvbInfo 
    {
        [LocalizedDisplay(LocalizeConstants.EvbNumber)]
        public string EvbNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.InsuranceNo)]
        public string VsuNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Insurance)]
        public string Versicherung { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Plz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        public string Land { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string HausNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string Telefon { get; set; }

        [LocalizedDisplay(LocalizeConstants.Fax)]
        public string Fax { get; set; }
    }
}
