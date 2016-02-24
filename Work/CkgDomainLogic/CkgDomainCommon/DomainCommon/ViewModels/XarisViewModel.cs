using System.Configuration;
using CkgDomainLogic.General.ViewModels;
using WebTools.Services;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class XarisViewModel : CkgBaseViewModel
    {
        private string UserNameHashed { get { return LogonContext.User.UrlRemoteLoginKey; } }

        private static string ExpirationToken { get { return UserSecurityService.UrlRemoteEncryptHourAndDate(); } }

        private static string XarisRootUrl { get { return ConfigurationManager.AppSettings["XarisSepiaRootUrl"]; } }

        private static string XarisAppRelativeUrl { get { return ConfigurationManager.AppSettings["XarisSepiaRelativeUrl"]; } }

        public string XarisUrl { get { return string.Format("{0}{1}&ra={2}&rb={3}", XarisRootUrl, XarisAppRelativeUrl, UserNameHashed, ExpirationToken); } }
    }
}
