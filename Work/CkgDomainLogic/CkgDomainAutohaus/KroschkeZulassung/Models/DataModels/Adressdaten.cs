using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class Adressdaten
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Plz { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }
    }
}
