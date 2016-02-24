using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class StatusverfolgungTrackingInfo
    {
        [LocalizedDisplay(LocalizeConstants.ShippingServiceProvider)]
        public string VersandDienstleister { get; set; }

        [LocalizedDisplay(LocalizeConstants.PartnerRole)]
        public string PartnerRolle { get; set; }

        [LocalizedDisplay(LocalizeConstants.PartnerRole)]
        public string PartnerRolleText
        {
            get
            {
                switch (PartnerRolle)
                {
                    case "ZY":
                        return Localize.Customer;

                    case "ZZ":
                        return Localize.RegistrationServiceZld;

                    case "Z7":
                        return string.Format("{0} 1", Localize.DeliveryAddress);

                    case "Z8":
                        return string.Format("{0} 2", Localize.DeliveryAddress);

                    case "Z9":
                        return string.Format("{0} 3", Localize.DeliveryAddress);

                    default:
                        return PartnerRolle;
                }
            }
        }

        [LocalizedDisplay(LocalizeConstants.TrackingId)]
        public string TrackingId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        public string TrackingLinkHint
        {
            get { return string.Format("{0} {1} {2} {3} ({4} {5})", Localize.ShippingSurvey, Localize.TrackingId, TrackingId, VersandDienstleister, PartnerRolleText, Bemerkung); }
        }

        public string VersandDienstleisterUrl { get; set; }

        public string TrackingUrl
        {
            get { return string.Format(VersandDienstleisterUrl, TrackingId); }
        }
    }
}
