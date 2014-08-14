using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    [Table("AHBeauftragteZulassungen")]
    public class BeauftragteZulassung
    {
        [Key]
        [LocalizedDisplay(LocalizeConstants.Id)]
        public int ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string ReferenzNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string ZBIINr { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateTime AuftragsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime ZulassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.UserName)]
        public string WebUser { get; set; }

        public int FahrzeugID { get; set; }
    }
}
