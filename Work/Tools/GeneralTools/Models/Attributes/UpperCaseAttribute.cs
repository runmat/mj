using System;
using GeneralTools.Contracts;

namespace GeneralTools.Models
{
    public class UpperCaseAttribute : Attribute, IAttributeWithModelMappingConvert 
    {
        public object ModelMappingConvert(object value)
        {
            var s = value as string;
            if (s == null)
                return value;

            return s.ToUpper();
        }
    }
}
