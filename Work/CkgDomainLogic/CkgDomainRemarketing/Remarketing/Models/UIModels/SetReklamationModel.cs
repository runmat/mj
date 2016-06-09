using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Remarketing.Models
{
    public class SetReklamationModel
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [Required]
        [StringLength(255)]
        [LocalizedDisplay(LocalizeConstants.ClaimText)]
        public string Reklamationstext { get; set; }

        [LocalizedDisplay(LocalizeConstants.Officiale)]
        public string Sachbearbeiter { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string Telefon { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }
    }
}
