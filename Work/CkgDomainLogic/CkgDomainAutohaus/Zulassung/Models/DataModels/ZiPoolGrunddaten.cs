using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class ZiPoolGrunddaten
    {
        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string Kreis { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChangeDate)]
        public DateTime? ZuletztGeaendertAm { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChangedBy)]
        public string ZuletztGeaendertVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationOffice)]
        public string UrlZulst { get; set; }

        [LocalizedDisplay(LocalizeConstants.Forms)]
        public string UrlZulstFormulare { get; set; }

        [LocalizedDisplay(LocalizeConstants.FeeInformation)]
        public string UrlZulstGebuehreninformation { get; set; }

        [LocalizedDisplay(LocalizeConstants.PersonalisedNumberPlate)]
        public string UrlZulstWunschkennzeichen { get; set; }
    }
}
