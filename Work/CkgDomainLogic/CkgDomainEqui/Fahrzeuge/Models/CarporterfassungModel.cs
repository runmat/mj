using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class CarporterfassungModel : Store 
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string CarportId { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarportName)]
        public string CarportName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        [XmlIgnore]
        public string Carport
        {
            get { return CarportName.PrependIfNotNullElse(CarportId + " - ", CarportId); }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

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

        [XmlIgnore]
        public string TmpStatus { get; set; }
    }
}
