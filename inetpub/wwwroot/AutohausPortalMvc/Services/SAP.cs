using System.Web;
using AutohausPortalMvc;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Contracts;
using MvcTools.Web;
using SapORM.Contracts;
using SapORM.Services;

namespace PortalMvcTools.Services
{
    public class S
    {
        static IAppSettings AppSettings { get { return MvcApplication.Instance.AppSettings; } }

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
            get
            {
                return AppSettings.SapNoSqlCache ? (ISapDataServiceFactory)new SapDataServiceFromConfigNoCacheFactory() : new SapDataServiceDefaultFactory();
            }
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
