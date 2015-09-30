using System;
using System.ComponentModel.DataAnnotations;

namespace GeneralTools.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ContainsNotAttribute : ValidationAttribute
    {
        public string StringToDeny { get; set; }

        public ContainsNotAttribute(string stringToDeny)
        {
            StringToDeny = stringToDeny;
        }

        public override bool IsValid(object value)
        {
            return !((string)value).NotNullOrEmpty().Contains(StringToDeny);
        }
    }
}
