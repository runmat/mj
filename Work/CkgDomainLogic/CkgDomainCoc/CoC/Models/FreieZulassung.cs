using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.CoC.Models
{
    public class FreieZulassung : IValidatableObject 
    {
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsReferenz { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        [VIN]
        public string VIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string ZBII { get; set; }


        public bool IsEmpty
        {
            get { return AuftragsReferenz.IsNullOrEmpty() && VIN.IsNullOrEmpty() && ZBII.IsNullOrEmpty(); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsEmpty)
                yield return new ValidationResult(Localize.ProvideAtLeastOneOption, new[] { "AuftragsReferenz", "VIN", "ZBII" });
        }
    }
}
