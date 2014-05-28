using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;

namespace CkgDomainLogic.Feinstaub.Models
{
    public class FeinstaubCheck
    {
        [Required]
        [StringLength(4)]
        [LocalizedDisplay("Fahrzeugklasse")]
        public string Fahrzeugklasse { get; set; }

        [Required]
        [StringLength(4)]
        [LocalizedDisplay("Kraftstoffcode")]
        public string Kraftstoffcode { get; set; }

        [Required]
        [StringLength(4)]
        [LocalizedDisplay("Code Aufbau")]
        public string CodeAufbau { get; set; }

        [Required]
        [StringLength(2)]
        [LocalizedDisplay("Emissionsschlüsselnummer")]
        public string Emissionsschluesselnummer { get; set; }
    }
}
