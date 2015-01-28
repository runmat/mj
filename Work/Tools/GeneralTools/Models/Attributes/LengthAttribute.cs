using System;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Resources;

namespace GeneralTools.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LengthAttribute : ValidationAttribute
    {
        public int Length { get; private set; }

        public bool ForceExactLength { get; set; }

        public LengthAttribute(int length, bool forceExactLength = false)
        {
            ForceExactLength = forceExactLength;
            this.ErrorMessageResourceType = typeof(CommonResources);
            this.ErrorMessageResourceName = (forceExactLength ? "Exact" :"") + "FieldLengthInvalid";

            Length = length;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            if (ForceExactLength)
                return (value.ToString().Length == Length);

            return (value.ToString().Length <= Length);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, Length);
        }
    }
}
