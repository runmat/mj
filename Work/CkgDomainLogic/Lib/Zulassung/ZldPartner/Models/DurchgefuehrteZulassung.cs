using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.ZldPartner.Models
{
    public class DurchgefuehrteZulassung 
    {
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string Kunde { get; set; }

        [LocalizedDisplay(LocalizeConstants.VoucherNo)]
        public string BelegNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.VoucherPosition)]
        public string BelegPosition { get; set; }

        public string DatensatzId { get { return BelegNr.NotNullOrEmpty().PadLeft(10, '0') + BelegPosition.NotNullOrEmpty().PadLeft(5, '0'); } }

        [LocalizedDisplay(LocalizeConstants.MaterialNo)]
        public string MaterialNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Service)]
        public string MaterialText { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public string LieferDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Halter { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string Zb2Nr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string ZulassungsKreis { get; set; }

        [LocalizedDisplay(LocalizeConstants._Kennzeichen)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public string ZulassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Fee)]
        public string Gebuehr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Price)]
        public string Preis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case "EGG":
                        return "eingegangen";
                    case "IA":
                        return "in Arbeit";
                    case "DGF":
                        return "durchgeführt";
                    case "STO":
                        return "storniert";
                    case "FGS":
                        return "fehlgeschlagen";
                    default:
                        return Status;
                }
            }
        }

        [LocalizedDisplay(LocalizeConstants.FeeRelevant)]
        public bool Gebuehrenrelevant { get; set; }

        [LocalizedDisplay(LocalizeConstants.Origin)]
        public string Herkunft { get; set; }

        [LocalizedDisplay(LocalizeConstants.BillingCreated)]
        public bool AbrechnungErstellt { get; set; }
    }
}
