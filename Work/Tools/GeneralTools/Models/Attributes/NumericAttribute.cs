using System;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Resources;

namespace GeneralTools.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NumericAttribute : RegularExpressionAttribute
    {
        public NumericAttribute()
            : base(@"^[0-9]+$")
        {
            this.ErrorMessageResourceType = typeof(CommonResources);
            this.ErrorMessageResourceName = "NumericFieldInvalid";
        }
    }}
