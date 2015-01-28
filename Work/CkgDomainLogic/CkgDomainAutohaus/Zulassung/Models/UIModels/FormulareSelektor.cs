using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class FormulareSelektor : Store
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string Zulassungskreis { get; set; }
    }
}
