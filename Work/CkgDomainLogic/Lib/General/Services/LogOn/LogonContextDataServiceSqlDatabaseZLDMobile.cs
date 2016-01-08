using System.Collections.Generic;
using System.Configuration;
using System;
using System.Linq;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using WebTools.Services;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Services
{
    /// <summary>
    /// ZLD-spezifischer LogonContext
    /// </summary>
    public class LogonContextDataServiceSqlDatabaseZLDMobile : LogonContextDataServiceBase, ILogonContextDataServiceZLDMobile
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
            return string.Format("/{0}/Login/Login", WebRootPath);
        }

        /// <summary>
        /// Anmeldung mit Username und Passwort sowie Setzen des LastLogin-Zeitpunktes in der DB
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool LogonUser(string userName, string password)
        {
            UserNameEncryptedToUrlEncoded = "";

            if (userName.IsNullOrEmpty())
                return false;

            var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], userName);

            AppTypes = dbContext.ApplicationTypes.ToList();
            User = dbContext.User;

            if (User == null || !dbContext.TryLogin(password) || User.AccountIsLockedOut || !User.Approved)
                return false;

            UserName = userName;
            UserInfo = dbContext.GetUserInfo();
            UserID = User.UserID.ToString();
            UserOnProdDataSystem = !User.TestUser;

            UserNameEncryptedToUrlEncoded = CryptoMd5Web.EncryptToUrlEncoded(User.Username);

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
        }

        public override bool ChangePassword(string oldPassword, string newPassword)
        {
            if (newPassword.IsNullOrEmpty())
                return false;

            var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName);

            if (dbContext.User == null)
            {
                return false;
            }

            if (!dbContext.TryChangePassword(oldPassword, newPassword))
            {
                return false;
            }

            return true;
        }

        public override void DataContextPersist(object dataContext) 
        {
        }

        public override object DataContextRestore(string typeName) 
        {
            return null;
        }

        public override IEnumerable<string> GetAddressPostcodeCityMappings(string plz)
        {
            return new List<string>();
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
    }
}
