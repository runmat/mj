using System;

namespace GeneralTools.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SelectListKeyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SelectListTextAttribute : Attribute
    {
    }
}
