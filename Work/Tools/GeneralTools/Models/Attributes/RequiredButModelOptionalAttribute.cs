using System;

namespace GeneralTools.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredButModelOptionalAttribute : Attribute
    {
    }
}
