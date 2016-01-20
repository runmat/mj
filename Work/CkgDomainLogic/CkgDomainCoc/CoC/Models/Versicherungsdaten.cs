using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.CoC.Models
{
    public class Versicherungsdaten
    {
        [LocalizedDisplay(LocalizeConstants.InsuranceCompany)]
        public string VersicherungsGesellschaft { get; set; }

        [LocalizedDisplay(LocalizeConstants.EvbNumber)]
        [Length(7, true)]
        [Required]
        public string EvbNummer { get; set; }

    
        public string GetSummaryString()
        {
            return new[]
                {
                    EvbNummer.FormatIfNotNull("{0}: {this}", "EVB Nummer"),
                    VersicherungsGesellschaft.FormatIfNotNull("{0}: {this}", Localize.InsuranceCompany)
                }.JoinIfNotNull("<br />");
        }
    }
}
