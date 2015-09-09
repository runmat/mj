using System;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
	public class UploadAvisdaten
	{
        public int DatensatzNr { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        [Required]
        public string FahrgestellNr { get; set; }

        [StringLength(15)]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [StringLength(15)]
        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string Carport { get; set; }

        [StringLength(15)]
        [LocalizedDisplay(LocalizeConstants.CarportName)]
        public string CarportName { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.MvaNumber)]
        public string MvaNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
	    public string ValidationStatus
	    {
	        get
	        {
                if (!String.IsNullOrEmpty(ValidationErrorsJson) && ValidationErrorsJson != "[]")
                    return Localize.Error;

                return Localize.OK;
	        }
	    }

        public string ValidationErrorsJson { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string SaveStatus { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool ValidationOk { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get { return ValidationOk; } }
	}
}
