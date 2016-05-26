using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Remarketing.Models
{
    public class Schadensmeldung
    {
        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string Vermieter { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.SequenceNo)]
        public string LaufendeNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamageAmount)]
        public decimal? Schadensbetrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamageDate)]
        public DateTime? Schadensdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DescriptionBeschreibung)]
        public string Beschreibung { get; set; }

        [LocalizedDisplay(LocalizeConstants.InventoryNo)]
        public string InventarNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.TakeInCenter)]
        public string Hereinnahmecenter { get; set; }

        [LocalizedDisplay(LocalizeConstants.HcReceipt)]
        public DateTime? HcEingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelName)]
        public string ModellBezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string Modell { get; set; }

        public string Aktion { get; set; }
    }
}
