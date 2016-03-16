using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using WebTools.Services;
using System.Web;

namespace CkgDomainLogic.General.Services
{
    /// <summary>
    /// Autohaus-spezifischer LogonContext
    /// </summary>
    public class LogonContextDataServiceAutohaus : LogonContextDataServiceBase, ILogonContextDataServiceAutohaus
    {
        public override List<IMaintenanceSecurityRuleDataProvider> MaintenanceCoreMessages { get { return null; } }

        public string VkOrg
        {
            get
            {
                if ((!String.IsNullOrEmpty(User.Reference)) && (User.Reference.Length > 4))
                {
                    return User.Reference.Substring(0, 4);
                }
                return User.Reference;
            }
        }

        public string VkBur
        {
            get
            {
                if ((!String.IsNullOrEmpty(User.Reference)) && (User.Reference.Length > 4))
                {
                    return User.Reference.Substring(4);
                }
                return "";
            }
        }

        public override string GetLoginUrl(string urlEncodedReturnUrl)
        {
            return string.Format("/{0}/Start/Login.aspx?ReturnUrl={1}", WebRootPath, urlEncodedReturnUrl);
        }

        public override bool LogonUser(string userName)
        {
            if (UserName == userName)
                return true;

            UserName = userName;

            UserNameEncryptedToUrlEncoded = "";
            if (userName.IsNullOrEmpty())
                return true;

            var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName);

            AppTypes = dbContext.ApplicationTypes.ToList();
            User = dbContext.User;
            UserInfo = dbContext.GetUserInfo();
            Customer = dbContext.GetCustomer(User.CustomerID);
            if (Customer == null)
                return false;

            var userGroup = dbContext.UserGroup;

            UserID = User.UserID.ToString();
            UserOnProdDataSystem = !User.TestUser;
            KundenNr = User.Reference;
            if (CkgDomainRules.IstKroschkeAutohaus(Customer.KUNNR))
                KundenNr = Customer.KUNNR.Trim();
            if (userGroup != null)
                GroupName = userGroup.GroupName;

            MvcEnforceRawLayout = GetMvcRawLayoutFlag();

            FirstName = User.FirstName;
            LastName = User.LastName;

            UserNameEncryptedToUrlEncoded = CryptoMd5Web.EncryptToUrlEncoded(User.Username);
            UserApps = dbContext.UserApps.Where(ua => ua.AppInMenu).Cast<IApplicationUserMenuItem>().ToList();
            UserApps.ForEach(ua =>
                {
                    var appType = AppTypes.FirstOrDefault(at => at.AppType == ua.AppType);
                    if (appType != null)
                    {
                        ua.AppTypeRank = appType.Rank;
                        ua.AppTypeFriendlyName = GetAppTypeFriendlyName(appType.AppType);
                    }

                    if (!ua.AppURL.ToLower().StartsWith("mvc/"))
                    {
                        ua.AppURL = String.Format("{0}://{1}{2}",
                            HttpContext.Current.Request.Url.Scheme,
                            HttpContext.Current.Request.Url.Authority,
                            ua.AppURL.Replace("../", "/AutohausPortal/"));
                    }
                        
                    RewriteUrlToLogPageVisit(ua);
                });

            dbContext.SetLastLogin(DateTime.Now);

            return true;
        }

        public override bool LogonUser(string userName, string password)
        {
            throw new System.NotImplementedException();
        }

        public override void LogoutUser()
        {
            throw new System.NotImplementedException();
        }

        public override bool ChangePassword(string oldPassword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            KundenNr = "";
            GroupName = "";
            UserID = "";
            UserName = "";
            FirstName = "";
            LastName = "";
            AppUrl = "";
            MvcEnforceRawLayout = false;
            LogoutUrl = "";
        }

        /// <summary>
        /// override für die gleichnamige LogonContextDataServiceBase-Funktion
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        public new IHtmlString GetUserEncrytpedUrl(IApplicationUserMenuItem menuItem)
        {
            if (UserNameEncryptedToUrlEncoded.IsNullOrEmpty())
                return new HtmlString("#");

            var appUrl = menuItem.AppURL.ToLower();
            appUrl = FormatUserEncrytpedUrl(appUrl).ToString();

            return new HtmlString(appUrl.Replace("?", string.Format("?appID={0}&", menuItem.AppID)));
        }
    }
}
