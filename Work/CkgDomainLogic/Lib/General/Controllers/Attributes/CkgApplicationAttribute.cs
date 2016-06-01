using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;

namespace CkgDomainLogic.General.Controllers
{
    public enum CkgApplicationAccessCheckType
    {
        Default
    }

    public class CkgApplicationAttribute : ActionFilterAttribute 
    {
        public CkgApplicationAccessCheckType AccessCheckType { get; protected set; }

        public AdminLevel RequiredAdminLevel { get; protected set; }

        public CkgApplicationAttribute()
        {
            AccessCheckType = CkgApplicationAccessCheckType.Default;
            RequiredAdminLevel = AdminLevel.None;
        }

        public CkgApplicationAttribute(CkgApplicationAccessCheckType accessCheckType)
        {
            AccessCheckType = accessCheckType;
            RequiredAdminLevel = AdminLevel.None;
        }

        public CkgApplicationAttribute(AdminLevel requiredAdminLevel)
        {
            AccessCheckType = CkgApplicationAccessCheckType.Default;
            RequiredAdminLevel = requiredAdminLevel;
        }

        public CkgApplicationAttribute(CkgApplicationAccessCheckType accessCheckType, AdminLevel requiredAdminLevel)
        {
            AccessCheckType = accessCheckType;
            RequiredAdminLevel = requiredAdminLevel;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (RequiredAdminLevel != AdminLevel.None)
            {
                var tmpController = filterContext.Controller as LogonCapableController;
                if (tmpController != null)
                {
                    var tmpLogonContext = tmpController.LogonContext as ILogonContextDataService;
                    if (tmpLogonContext == null || tmpLogonContext.HighestAdminLevel < RequiredAdminLevel)
                        filterContext.Result = new RedirectResult("~"); // redirect to main menu
                }
            }
        }
    }
}
