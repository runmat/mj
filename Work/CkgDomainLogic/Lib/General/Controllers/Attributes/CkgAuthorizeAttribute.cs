using System.Linq;
using System.Web.Mvc;
using GeneralTools.Contracts;
using GeneralTools.Models;
using MvcTools.Contracts;
using MvcTools.Controllers;

namespace CkgDomainLogic.General.Controllers
{
    public class CkgAuthorizeAttribute : AuthorizeAttribute 
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                return;

            var requestUserName = filterContext.HttpContext.Request["un"];
            var requestRemoteLoginID = filterContext.HttpContext.Request["ra"];
            var requestRemoteLoginDateTime = filterContext.HttpContext.Request["rb"];
            var requestRemoteLoginLogoutUrl = filterContext.HttpContext.Request["logouturl"];
            var requestIsGet = (filterContext.HttpContext.Request.RequestType.NotNullOrEmpty().ToUpper() == "GET");

            // if MVC runs in embedded mode (iFrame), extract the session id of the surrounding web application
            // => user context might not be valid any more if the surrounding session has changed
            var requestSurroundingSessionId = "";
            if (!string.IsNullOrEmpty(requestUserName) && filterContext.HttpContext.Request.UrlReferrer != null)
            {
                var segments = filterContext.HttpContext.Request.UrlReferrer.Segments;

                if (segments != null && segments.Length > 2)
                {
                    if ((segments[1].ToLower() == "services/" || segments[1].ToLower() == "portal/" || segments[1].ToLower() == "autohausportal/" || segments[1].ToLower() == "portalzld/") 
                        && (segments[2].ToLower().StartsWith("(s(")))
                    {
                        requestSurroundingSessionId = segments[2].ToLower().Replace("(s(", "").Replace("))/", "");
                    }
                }
            }

            var controller = filterContext.Controller;
            var logonController = controller as LogonCapableController;
            if (logonController == null)
                return;

            logonController.ValidateMaintenance();

            var requestIsCkgApplication = filterContext.ActionDescriptor.GetCustomAttributes(true).OfType<CkgApplicationAttribute>().Any();
            if (requestIsCkgApplication)
                logonController.ValidateApplication();

            if (requestIsGet && requestIsCkgApplication)
            {
                var gridColumnsAutoPersistProvider = (controller as IGridColumnsAutoPersistProvider);
                if (gridColumnsAutoPersistProvider != null)
                    gridColumnsAutoPersistProvider.ResetGridCurrentModelTypeAutoPersist();

                var persistableSelectorProvider = (controller as IPersistableSelectorProvider);
                if (persistableSelectorProvider != null)
                    persistableSelectorProvider.PersistableSelectorResetCurrent();
            }

            var logonAction = logonController.UrlGetLogonAction(requestUserName, requestRemoteLoginID, requestRemoteLoginDateTime, requestRemoteLoginLogoutUrl, requestSurroundingSessionId);
            if (logonAction != null)
                filterContext.Result = logonAction;
        }
    }
}
