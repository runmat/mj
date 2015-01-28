using System;

namespace CkgDomainLogic.General.Services
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DashboardItemsLoadMethod : Attribute
    {
    }
}
