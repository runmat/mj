using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// Note:  NameSpace is correct: GeneralTools.Services
namespace GeneralTools.Services
{
    public class ValidationService
    {
        /// <summary>
        /// Data Annotation Validation, process all ValidationAttributes on all properties of an object
        /// (also "VIN" validation will be processed here, use the "VIN" attribute in CkgDomainLogic library)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<ValidationResult> ValidateDataAnnotations(object item)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(item, null, null);
            if (!Validator.TryValidateObject(item, validationContext, validationResults, true))
                return validationResults.ToList();

            return new List<ValidationResult>();
        }
    }
}
