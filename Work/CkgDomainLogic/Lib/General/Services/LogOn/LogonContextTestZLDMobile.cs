using System.Collections.Generic;
using System.Configuration;
using System;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Services
{
    /// <summary>
    /// ZLD-spezifischer LogonContext (Test)
    /// </summary>
    public class LogonContextTestZLDMobile : LogonContextDataServiceBase, ILogonContextDataServiceZLDMobile
    {
        public new string CurrentGridColumns { get; set; }

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
        /// Anmeldung mit Username und Passwort
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

            AppTypes = new List<ApplicationType>();
            User = new User();
            User.Username = userName;
            User.Reference = "10104265";

            UserName = userName;
            UserInfo = new WebUserInfo{ ID_User = User.UserID };

            return true;
        }

        public override bool LogonUser(string userName)
        {
            throw new System.NotImplementedException();
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
            throw new NotImplementedException();
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
