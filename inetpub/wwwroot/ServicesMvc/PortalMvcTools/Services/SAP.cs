using System.Web;
using CkgDomainLogic.General.Contracts;
using MvcTools.Web;
using SapORM.Contracts;
using SapORM.Services;

namespace PortalMvcTools.Services
{
    public class S
    {
        public static ISapDataService AP
        {
            get
            {
                TrySetProdSapForWebLogonUser();

                return SessionStore<ISapDataService>.GetModel(SapDataServiceFactory.Create);
            }
        }

        public static ISapDataServiceFactory SapDataServiceFactory
        {
            get { return new SapDataServiceDefaultFactory(); }
        }

        private static void TrySetProdSapForWebLogonUser()
        {
            SessionHelper.ClearSessionObject("WebLogonUserOnProdDataSystem");

            var sessionLogonContext = SessionHelper.GetSessionObject("LogonContext");
            if (sessionLogonContext == null)
                return;

            var logonContextDataService = (sessionLogonContext as ILogonContextDataService);
            if (logonContextDataService == null)
                return;

            if (logonContextDataService.UserOnProdDataSystem != null)
                HttpContext.Current.Session["WebLogonUserOnProdDataSystem"] = logonContextDataService.UserOnProdDataSystem;
        }
    }
}
