using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.CoC.Models
{
    public class CsvUploadEntityDpmCoc
    {
        [LocalizedDisplay(LocalizeConstants.ReservationNo)]
        public string OrderID { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        [Required]
        [VIN]
        public string VIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public DateTime? DeliveryDate { get; set; }

        [LocalizedDisplay(LocalizeConstants.CountryCode)]
        public string Country { get; set; }

        [LocalizedDisplay(LocalizeConstants.CodeManufacturer)]
        public string CodeManufacturer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CodeTypeVersion)]
        public string CodeTypeVersion { get; set; }

        [LocalizedDisplay(LocalizeConstants.Color)]
        public string Color { get; set; }

        [ScaffoldColumn(false)]
        public string ValidationErrors { get; set; }

        [ScaffoldColumn(false)]
        public string CustomerNo { get; set; }

        [ScaffoldColumn(false)]
        public int TestInt { get; set; }

        public bool MemberHasValidationError(string propertyName = null)
        {
            if (propertyName.IsNullOrEmpty())
                return ValidationErrors.IsNotNullOrEmpty();

            return ValidationErrors.NotNullOrEmpty().Split(',').Contains(propertyName);
        }
    }
}
