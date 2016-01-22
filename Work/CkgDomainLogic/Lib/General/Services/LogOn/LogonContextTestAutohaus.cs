using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using CkgDomainLogic.General.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Services
{
    public class LogonContextTestAutohaus : Store, ILogonContextDataServiceAutohaus
    {
        public string CurrentGridColumns { get; set; }
        public AdminLevel HighestAdminLevel { get; private set; }

        public string UserNameForDisplay { get; set; }

        public ILocalizationService LocalizationService { get; private set; }

        public List<IMaintenanceSecurityRuleDataProvider> MaintenanceCoreMessages { get { return new List<IMaintenanceSecurityRuleDataProvider>(); } }
        
        public MaintenanceResult MaintenanceInfo { get { return PropertyCacheGet(() => new MaintenanceResult()); } set { PropertyCacheSet(value); } }

        public int CustomerID { get { return 0; } }

        public int AppID { get { return 0; } }

        public string KundenNr { get { return PropertyCacheGet(() => ConfigurationManager.AppSettings["LogonContextTestKundenNr"]); } set { PropertyCacheSet(value); } }

        public string GroupName { get; set; }

        public string UserName
        {
            get { return PropertyCacheGet(() => User == null ? "TestUser" : User.Username); }
            set { PropertyCacheSet(value); }
        }

        public virtual bool HasLocalizationTranslationRights { get { return false; } }

        public WebUserInfo UserInfo { get; set; }

        private LogonLevel _userLogonLevel = LogonLevel.User;
        public LogonLevel UserLogonLevel
        {
            get { return _userLogonLevel; }
            set { _userLogonLevel = value; }
        }

        public bool? UserOnProdDataSystem { get; set; }

        private string _firstName = "";
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _lastName = "Test";
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string FullName { get { return FirstName.IsNullOrEmpty() ? LastName : string.Format("{0}, {1}", LastName, FirstName); } }
        
        public string AppUrl { get; set; }

        private string _userID = "1";

        public string UserID 
        { 
            get { return _userID; }
            set { _userID = value; }
        }

        public bool MvcEnforceRawLayout { get; set; }

        public string CurrentLayoutTheme { get; set; }


        public LogonContextTestAutohaus(ILocalizationService localizationService)
        {
            var ct = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName);
            Customer = ct.GetCustomer(User.CustomerID); // new Customer { CustomerID = this.User.CustomerID, Customername = "Test-Kunde", KUNNR = ConfigurationManager.AppSettings["LogonContextTestKundenNr"] };
            LocalizationService = localizationService;

            Group = new UserGroup {GroupName = "Standard", GroupID = 52, CustomerID = 0};
        }

        public string GetLoginUrl(string urlEncodedReturnUrl)
        {
            return null;
        }

        public bool LogonUser(string userName)
        {
            return false;
        }

        public bool LogonUser(string userName, string password)
        {
            return false;
        }

        public bool LogonUserWithUrlRemoteLoginKey(string urlRemoteLoginKey)
        {
            return false;
        }

        public string GetUserNameFromUrlRemoteLoginKey(string urlRemoteLoginKey)
        {
            return "";
        }

        public void LogoutUser()
        {
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return false;
        }

        public bool UserNameIsValid(string userID)
        {
            return true;
        }

        public List<ApplicationType> AppTypes { get; set; }

        public User User 
        { 
            get
            {
                return PropertyCacheGet(() =>
                    {
                        var userName = ConfigurationManager.AppSettings["LogonContextTestUserName"];
                        var ct = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], userName);

                        return ct.User;
                    });
            }  
            set { PropertyCacheSet(value); } 
        }

        public Customer Customer { get; set; }

        public string CustomerName { get { return "Test-Kunde"; } }
        
        public UserGroup Group { get; set; }

        public UserOrganization Organization { get; set; }

        public List<IApplicationUserMenuItem> UserApps { get; set; }

        public bool AppFavoritesEditMode { get; set; }
        
        public bool AppFavoritesEditSwitchOneFavorite(int appID)
        {
            return false;
        }

        public string ReturnUrl { get; set; }

        public ISecurityService SecurityService { get; set; }
        
        public string LogoutUrl { get; set; }

        public void AppUrlQueryAndStore()
        {
            AppUrl = "Test-Application";
        }

        public List<IApplicationUserMenuItem> GetMenuItemGroups()
        {
            return new List<IApplicationUserMenuItem>();
        }

        public List<IApplicationUserMenuItem> GetMenuItems(string appType)
        {
            return new List<IApplicationUserMenuItem>();
        }

        public IHtmlString GetUserEncrytpedUrl(IApplicationUserMenuItem menuItem)
        {
            return new HtmlString("#");
        }

        public IHtmlString FormatUserEncrytpedUrl(string url)
        {
            return new HtmlString("#");
        }

        public IHtmlString FormatUrl(string url)
        {
            return new HtmlString(string.Format("{0}", url));
        }

        int KundenNrToInt()
        {
            int customerNo;
            if (!Int32.TryParse(KundenNr, out customerNo))
                return -1;

            return customerNo;
        }

        public void DataContextPersist(object dataContext) 
        {
            var ct = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName);
            ct.DataContextPersist(dataContext);
        }

        public object DataContextRestore(string typeName) 
        {
            var ct = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName);
            return ct.DataContextRestore(typeName);
        }

        public void TryLogonUser(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
        }

        public string TryGetEmailAddressFromUsername(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
            return "";
        }

        public void CheckIfPasswordResetAllowed(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
        }

        public bool CheckPasswordHistory(ChangePasswordModel model, int passwordMinHistoryEntries)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAddressPostcodeCityMappings(string plz)
        {
            var ct = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName);

            return ct.GetAddressPostcodeCityMapping(plz);
        }

        public virtual User TryGetUserFromPasswordToken(string passwordToken, int tokenExpirationMinutes)
        {
            return null;
        }

        public User TryGetUserFromUserName(string userName)
        {
            return null;
        }

        public Customer TryGetCustomerFromUserName(string userName)
        {
            return null;
        }

        public List<Contact> TryGetGroupContacts(int customerID, string groupName)
        {
            return new List<Contact>();
        }

        public List<Contact> TryGetGroupContacts()
        {
            return new List<Contact>();
        }


        public void StorePasswordToUser(IPasswordSecurityRuleDataProvider passwordSecurityRuleDataProvider, string userName, string password)
        {
        }

        public void StorePasswordRequestKeyToUser(string userName, string passwordRequestKey)
        {
        }

        public bool ValidatePassword(string password, User storedUser)
        {
            return true;
        }

        public bool ValidateUser(IUserSecurityRuleDataProvider userSecurityRuleDataProvider, IPasswordSecurityRuleDataProvider passwordSecurityRuleDataProvider, ILocalizationService localizationService, out List<string> localizedValidationErrorMessages)
        {
            localizedValidationErrorMessages = new List<string>();
            return true;
        }

        public string TranslateLocalizationKey(string localizationKey)
        {
            return LocalizationService.TranslateResourceKey(localizationKey);
        }

        public virtual string TranslateMenuAppType(IApplicationUserMenuItem menuItem)
        {
            var translatedText = LocalizationService.TranslateResourceKey(menuItem.AppType);

            return translatedText.IsNotNullOrEmpty() ? translatedText : menuItem.AppTypeFriendlyName;
        }

        public virtual string TranslateMenuAppName(IApplicationUserMenuItem menuItem)
        {
            var translatedText = LocalizationService.TranslateResourceKey(menuItem.AppName);

            return translatedText.IsNotNullOrEmpty() ? translatedText : menuItem.AppFriendlyName;
        }

        public MaintenanceResult ValidateMaintenance()
        {
            return new MaintenanceResult();
        }

        public void MaintenanceMessageConfirmAndDontShowAgain()
        {
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

        public void TrySetLogoutLink()
        {
            throw new NotImplementedException();
        }

        public string VkOrg { get; private set; }
        public string VkBur { get; private set; }

        public string PersistanceKey { get { return UserName; } }

        public IPersistanceService PersistanceService { get; set; }

        public int GetAppIdCurrent()
        {
            return LogonContextHelper.GetAppIdCurrent(UserApps);
        }

        public string GetEmailAddressForUser()
        {
            return "";
        }
    }
}
