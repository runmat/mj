using System.ComponentModel.DataAnnotations;
using GeneralTools.Contracts;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models
{
    public class ZulassungsKreisAttribute : ValidationAttribute, IAttributeWithModelMappingConvert 
    {
        public ZulassungsKreisAttribute()
        {
            this.ErrorMessageResourceType = typeof(CommonResources);
            this.ErrorMessageResourceName = "RegistrationAreaInvalid";
        }

        public override bool IsValid(object value)
        {
            value = ModelMappingConvert(value);

            var s = value as string;
            if (s == null)
                // Note: 
                // Regardless that a "ZulassungsKreis" may be required in most cases, we should let the "Required" Attribute to do this job!
                // Also, sometimes we want to accept empty values for a VIN
                return true;

            foreach (var c in s)
                if (c < 'A' || c > 'Z')
                    return false;

            if (s.Length > 3)
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
