using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class StatusverfolgungZulassungModel
    {
        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? StatusDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string StatusUhrzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public DateTime? StatusDatumUhrzeit
        {
            get
            {
                if (!StatusDatum.HasValue || string.IsNullOrEmpty(StatusUhrzeit) || StatusUhrzeit.Length < 6)
                    return StatusDatum;

                return StatusDatum.Value.AddHours(StatusUhrzeit.Substring(0, 2).ToInt(0)).AddMinutes(StatusUhrzeit.Substring(2, 2).ToInt(0)).AddSeconds(StatusUhrzeit.Substring(4, 2).ToInt(0));
            }
        }

        [LocalizedDisplay(LocalizeConstants.PartnerRole)]
        public string PartnerRolle { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingServiceProvider)]
        public string VersandDienstleister { get; set; }

        [LocalizedDisplay(LocalizeConstants.TrackingId)]
        public string TrackingId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }
    }
}
