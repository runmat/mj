using System;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class CarporterfassungModel
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string CarportId { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarportName)]
        public string CarportName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string Carport
        {
            get
            {
                if (!String.IsNullOrEmpty(CarportName))
                    return String.Format("{0} - {1}", CarportId, CarportName);

                return CarportId;
            }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.InventoryNumber)]
        public string MvaNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Barcode)]
        public string Barcode { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfLicensePlates)]
        public string AnzahlKennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.DisassemblyDate)]
        public DateTime? DemontageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Deregistered)]
        public bool Abgemeldet { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb1Available)]
        public bool Zb1Vorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb2Available)]
        public bool Zb2Vorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.CocAvailable)]
        public bool CocVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.ServiceRecordAvailable)]
        public bool ServiceheftVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.HuAuReportAvailable)]
        public bool HuAuBerichtVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.SpareKeyAvailable)]
        public bool ZweitschluesselVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.NaviCdAvailable)]
        public bool NaviCdVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryNoteNo)]
        public string LieferscheinNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        public string Action { get; set; }
    }
}
