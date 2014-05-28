using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FehlteilEtikettSelektor
    {
        [LocalizedDisplay(LocalizeConstants.VIN)]
        [Required]
        [VIN]
        public string VIN { get; set; }
    }
}
