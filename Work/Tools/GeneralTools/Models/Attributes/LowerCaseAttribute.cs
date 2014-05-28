using System;
using GeneralTools.Contracts;

namespace GeneralTools.Models
{
    public class LowerCaseAttribute : Attribute, IAttributeWithModelMappingConvert 
    {
        public object ModelMappingConvert(object value)
        {
            var s = value as string;
            if (s == null)
                return value;

            return s.ToLower();
        }
    }
}
