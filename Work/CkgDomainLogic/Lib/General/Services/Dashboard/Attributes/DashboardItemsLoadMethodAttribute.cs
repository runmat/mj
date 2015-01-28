using System;

namespace CkgDomainLogic.General.Services
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DashboardItemsLoadMethodAttribute : Attribute
    {
        public string Key { get; set; }

        public DashboardItemsLoadMethodAttribute(string key)
        {
            Key = key;
        }
    }
}
