using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Remarketing.Models
{
    public class FehlendeDaten
    {
        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string VermieterId { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string VermieterName { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string Vermieter { get { return string.Format("{0} {1}", VermieterId, VermieterName); } }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb2Receipt)]
        public DateTime? Zb2Eingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarportArrival)]
        public DateTime? CarportEingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquiNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.BuyBackBill)]
        public DateTime? Rechnungsuebermittlung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Survey)]
        public DateTime? Eingangsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Deactivation)]
        public DateTime? Stilllegungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.HandOverTuev)]
        public DateTime? DatumHcUebergabeTuevSued { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreationDebitNote)]
        public DateTime? DatumErstellungBelastungsanzeige { get; set; }
    }
}
