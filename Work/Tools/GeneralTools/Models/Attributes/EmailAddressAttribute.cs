using System;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Resources;

namespace GeneralTools.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailAddressAttribute : RegularExpressionAttribute
    {
        public EmailAddressAttribute()
            : base(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}")
        {
            this.ErrorMessageResourceType = typeof(CommonResources);
            this.ErrorMessageResourceName = "EmailAddressInvalid";
        }
    }}
