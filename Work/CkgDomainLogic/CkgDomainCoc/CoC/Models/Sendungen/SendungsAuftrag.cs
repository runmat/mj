using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.CoC.Models
{
    public class SendungsAuftrag
    {
        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? AnlageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.InvoiceNo)]
        public string RechnungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? ZulassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingID)]
        public string VersandID { get; set; }
    }
}
