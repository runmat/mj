using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.ZldPartner.Models
{
    public class DurchgefuehrteZulassung 
    {
        [LocalizedDisplay(LocalizeConstants.VoucherNo)]
        public string BelegNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.VoucherPosition)]
        public string BelegPosition { get; set; }

        [LocalizedDisplay(LocalizeConstants.MaterialNo)]
        public string MaterialNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.MaterialText)]
        public string MaterialText { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public DateTime? LieferDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Halter { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string Zb2Nr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string ZulassungsKreis { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? ZulassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderPosition)]
        public string AuftragsPosition { get; set; }

        [LocalizedDisplay(LocalizeConstants.Fee)]
        public decimal? Gebuehr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Price)]
        public decimal? Preis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        public string StatusWerte { get { return ",;IA,in Arbeit;DGF,durchgeführt;STO,storniert;FGS,fehlgeschlagen"; } }

        [LocalizedDisplay(LocalizeConstants.FeeRelevant)]
        public bool Gebuehrenrelevant { get; set; }

        [LocalizedDisplay(LocalizeConstants.Origin)]
        public string Herkunft { get; set; }

        [LocalizedDisplay(LocalizeConstants.VoucherFeePosition)]
        public string BelegGebuehrenPosition { get; set; }
    }
}
