using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLeasing.Leasing.Models.DataModels
{
	public class LeasingCargateCsvUploadModel
	{
        [LocalizedDisplay(LocalizeConstants.VIN17)]
        [Required]
	    public string Fin { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        [Required]
	    public string Standort { get; set; }

        [LocalizedDisplay(LocalizeConstants._EingangFahrzeugBlg)]
        [Required]
	    public string EingangFahrzeugBlg { get; set; }

        [LocalizedDisplay(LocalizeConstants._BereitstellungFahrzeugBlg)]
	    public string BereitstellungFahrzeugBlg { get; set; }

        [LocalizedDisplay(LocalizeConstants._FertigmeldungAufbereitungFahrzeugBlg)]
	    public string FertigmeldungAufbereitungFahrzeugBlg { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<ValidationResult> ValidationErrors { get; set; }

        public bool MemberHasValidationError(string propertyName = null)
        {
            if (propertyName.IsNullOrEmpty())
                return false;

            return ValidationErrors.Any(x => x.MemberNames.Contains(propertyName));
        }
	}
}
