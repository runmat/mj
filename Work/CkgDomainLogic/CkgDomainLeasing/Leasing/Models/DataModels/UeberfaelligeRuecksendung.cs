using System;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class UeberfaelligeRuecksendung
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquiNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LeasingContractNo)]
        public string LeasingvertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string Zb2Nr { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDispatch)]
        public DateTime? VersandDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningLevel)]
        public string Mahnstufe { get; set; }

        [StringLength(200)]
        [LocalizedDisplay(LocalizeConstants.Memo)]
        public string Memo { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string Versandgrund { get; set; }

        [LocalizedDisplay(LocalizeConstants.DueDate)]
        public DateTime? FaelligkeitsDatum { get; set; }

        public bool FristVerlaengert { get; set; }

        public bool FaelligkeitsDatumGesetzt { get { return ((FaelligkeitsDatum.HasValue && FaelligkeitsDatum.Value.Year > 1900) || FristVerlaengert); } }

        [LocalizedDisplay(LocalizeConstants.SalesUnit)]
        public string Vertriebseinheit { get; set; }

        [LocalizedDisplay(LocalizeConstants.SearchnameCustomer)]
        public string SuchnameKunde { get; set; }

        [LocalizedDisplay(LocalizeConstants.Requester)]
        public string Anforderer { get; set; }
    }
}
