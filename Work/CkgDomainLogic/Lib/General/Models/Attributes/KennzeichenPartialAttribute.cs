using System;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Contracts;

namespace CkgDomainLogic.General.Models
{
    public class KennzeichenPartialAttribute: Attribute, IAttributeWithModelMappingConvert, IKennzeichenAttribute
    {
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
