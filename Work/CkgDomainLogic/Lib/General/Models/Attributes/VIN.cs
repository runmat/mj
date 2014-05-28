using System.ComponentModel.DataAnnotations;
using System.Linq;
using GeneralTools.Contracts;
using GeneralTools.Resources;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Models
{
    public class VINAttribute : ValidationAttribute, IAttributeWithModelMappingConvert 
    {
        public VINAttribute()
        {
            this.ErrorMessageResourceType = typeof(CommonResources);
            this.ErrorMessageResourceName = "VinInvalid";
        }

        public override bool IsValid(object value)
        {
            value = ModelMappingConvert(value);

            var s = value as string;
            if (s == null || s.IsNullOrEmpty())
                // Note: 
                // Regardless that a VIN may be required in most cases, we should let the "Required" Attribute to do this job!
                // Also, sometimes we want to accept empty values for a VIN
                return true;

            // characters "I" and "O" are not valid for VIN's
            if (s.Contains("I") || s.Contains("O") || s.Contains(" ") || s.Contains(",") || s.Contains(";"))
                return false;

            // VIN length should be either 10 or 17 chars
            if (!(new []{ 10, 17 }.Contains(s.Length)))
                return false;

            return true;
        }

        /// <summary>
        /// VIN's should always be automatically converted to UPPERCASE
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ModelMappingConvert(object value)
        {
            var s = value as string;
            if (s == null)
                return value;

            return s.ToUpper();
        }
    }
}
