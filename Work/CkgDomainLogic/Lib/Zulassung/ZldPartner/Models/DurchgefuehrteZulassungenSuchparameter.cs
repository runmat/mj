using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.ZldPartner.Models
{
    public class DurchgefuehrteZulassungenSuchparameter : Store
    {
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string Kunde { get; set; }

        public string Kunden { get { return ",;1010,Kroschke;1510,DAD"; } }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateRange ZulassungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string Auswahl { get; set; }

        public string AuswahlOptionen { get { return "A,Alle Aufträge;D,Durchgeführte Aufträge;O,Offene Aufträge"; } }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Halter { get; set; }
    }
}
