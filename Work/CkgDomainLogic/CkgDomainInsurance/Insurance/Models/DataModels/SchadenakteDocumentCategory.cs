using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.Insurance.Models
{
    public class SchadenakteDocumentCategory : CustomerDocumentCategory, IValidatableObject
    {
        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(CategoryName))
            {
                yield return new ValidationResult(Localize.AllRequiredInputFieldsMustBeFilled, new[] { "CategoryName" });
            }
        }
    }
}
