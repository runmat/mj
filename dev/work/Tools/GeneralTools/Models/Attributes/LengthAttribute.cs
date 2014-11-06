using System;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Resources;

namespace GeneralTools.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LengthAttribute : ValidationAttribute
    {
        public int Length { get; private set; }

        public LengthAttribute(int length) 
        {
            this.ErrorMessageResourceType = typeof(CommonResources);
            this.ErrorMessageResourceName = "FieldLengthInvalid";

            Length = length;
        }

        public override bool IsValid(object value)
        {
            return value == null || (value.ToString().Length == Length);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, Length);
        }
    }
}
