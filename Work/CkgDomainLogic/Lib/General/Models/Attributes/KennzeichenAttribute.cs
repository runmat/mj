using System.ComponentModel.DataAnnotations;
using GeneralTools.Contracts;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models
{
    public class KennzeichenAttribute : RegularExpressionAttribute, IAttributeWithModelMappingConvert 
    {
        public KennzeichenAttribute()
            : base("[A-Z]{1,3}-([A-Z]{1,2} [0-9]{1,4}$)*")
        {
            this.ErrorMessageResourceType = typeof(CommonResources);
            this.ErrorMessageResourceName = "InvalidLicenseNo";
        }

        /// <summary>
        /// "Kennzeichen" should always be automatically converted to UPPERCASE
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
