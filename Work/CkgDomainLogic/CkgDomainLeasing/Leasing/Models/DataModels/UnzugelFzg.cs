using System;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class UnzugelFzg
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquipmentnummerShow { get { return Equipmentnummer; } }
        public string Equipmentnummer { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.DateOfRegistrationReceipt)]
        public DateTime BriefeingangShow { get { return Briefeingang; } }
        public DateTime Briefeingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellnummerShow { get { return Fahrgestellnummer; } }
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Dealer)]
        public string HaendlernameShow { get { return Haendlername; } }
        public string Haendlername { get; set; }

        [LocalizedDisplay(LocalizeConstants.DealerCity)]
        public string HaendlerortShow { get { return Haendlerort; } }
        public string Haendlerort { get; set; }

        [LocalizedDisplay(LocalizeConstants.LeasingContractNo)]
        [StringLength(20, MinimumLength=7)]
        [RegularExpression("^[0-9]*$")]
        public string Leasingvertragsnummer { get; set; }
        [LocalizedDisplay(LocalizeConstants.LeasingContractNo)]
        public string LeasingvertragsnummerShow { get { return Leasingvertragsnummer; } }

        [LocalizedDisplay(LocalizeConstants.RegistrationNo)]
        public string BriefnummerShow { get { return Briefnummer; } }
        public string Briefnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string StatusShow { get { return Status; } }
        public string Status { get; set; }

    }
}
