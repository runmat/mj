using System.Configuration;
using System;
using System.Linq;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Models;
using WebTools.Services;

namespace CkgDomainLogic.General.Services
{
    /// <summary>
    /// ZLD-spezifischer LogonContext
    /// </summary>
    public class LogonContextDataServiceSqlDatabaseZLD : LogonContextDataServiceBase
    {
        public string Password { get; set; }

        public string VkOrg
        {
            get
            {
                if ((!String.IsNullOrEmpty(KundenNr)) && (KundenNr.Length > 4))
                {
                    return KundenNr.Substring(0, 4);
                }
                return KundenNr;
            }
        }

        public string VkBur
        {
            get
            {
                if ((!String.IsNullOrEmpty(KundenNr)) && (KundenNr.Length > 4))
                {
                    return KundenNr.Substring(4);
                }
                return "";
            }
        }

        public override string GetLoginUrl(string urlEncodedReturnUrl)
        {
            return string.Format("/{0}/Login?ReturnUrl={1}", WebRootPath, urlEncodedReturnUrl);
        }

        /// <summary>
        /// Anmeldung mit Username und Passwort sowie Setzen des LastLogin-Zeitpunktes in der DB
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool LogonUser(string userName, string password)
        {
            UserNameEncryptedToUrlEncoded = "";

            if (userName.IsNullOrEmpty())
                return false;

            var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], userName);

            AppTypes = dbContext.ApplicationTypes.ToList();
            User = dbContext.User;

            if (dbContext.User == null)
            {
                return false;
            }

            if (!dbContext.TryLogin(password))
            {
                return false;
            }

            UserName = userName;

            UserID = User.UserID.ToString();
            //KundenNr = "10000649";
            KundenNr = User.Reference;

            UserNameEncryptedToUrlEncoded = CryptoMd5.EncryptToUrlEncoded(User.Username);
            UserApps = dbContext.UserApps.Where(ua => ua.AppInMenu).ToList();
            UserApps.ForEach(ua =>
                {
                    var appType = AppTypes.FirstOrDefault(at => at.AppType == ua.AppType);
                    if (appType != null)
                    {
                        ua.AppTypeRank = appType.Rank;
                        ua.AppTypeFriendlyName = GetAppTypeFriendlyName(appType.AppType);
                    }
                });

            dbContext.SetLastLogin(DateTime.Now);

            return true;
        }

        public override bool LogonUser(string userName)
        {
            // nur Anmeldung mit Passwort
            return false;
        }

        public override void LogoutUser()
        {
            UserID = "";
            UserName = "";
            KundenNr = "";
            User = null;
            if (AppTypes != null)
            {
                AppTypes.Clear();
            }
            UserNameEncryptedToUrlEncoded = "";
            if (UserApps != null)
            {
                UserApps.Clear();
            }
        }
    }
}
