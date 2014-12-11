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
    public class LogonContextTest : Store, ILogonContextDataService 
    {
        public string CurrentGridColumns { get; set; }

        public ILocalizationService LocalizationService { get; private set; }

        public List<IMaintenanceSecurityRuleDataProvider> MaintenanceCoreMessages { get { return new List<IMaintenanceSecurityRuleDataProvider>(); } }
        
        public MaintenanceResult MaintenanceInfo { get { return PropertyCacheGet(() => new MaintenanceResult()); } set { PropertyCacheSet(value); } }

        public string KundenNr { get { return PropertyCacheGet(() => ConfigurationManager.AppSettings["LogonContextTestKundenNr"]); } set { PropertyCacheSet(value); } }

        public string GroupName { get; set; }

        public string UserName
        {
            get { return PropertyCacheGet(() => User == null ? "TestUser" : User.Username); }
            set { PropertyCacheSet(value); }
        }

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


        public LogonContextTest(ILocalizationService localizationService)
        {
            var ct = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName);
            if (User != null)
                Customer = ct.GetCustomer(User.CustomerID); 
            else 
                Customer = new Customer { CustomerID = 0, Customername = "Test-Kunde", KUNNR = ConfigurationManager.AppSettings["LogonContextTestKundenNr"] };

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

        public Organization Organization { get; set; }

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

        public virtual string GetUserGridColumnNames(Type modelType, GridColumnMode gridColumnMode, string gridGroup)
        {
            var logonLevel = UserLogonLevel;

            if (gridGroup.IsNullOrEmpty() || (logonLevel == LogonLevel.Admin && gridColumnMode == GridColumnMode.Master))
                // let give a chance to all model properties here, return empty string
                return string.Join("~", modelType.GetScaffoldPropertyNames());

            var customerNo = KundenNrToInt();
            var userName = "";

            if ((logonLevel == LogonLevel.Admin && gridColumnMode == GridColumnMode.Slave) ||
                (logonLevel == LogonLevel.Customer && gridColumnMode == GridColumnMode.Master))
                customerNo = 0;

            if ((logonLevel == LogonLevel.Customer && gridColumnMode == GridColumnMode.Slave))
                userName = UserName;

            var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName);
            var affectedColumns = dbContext.ColumnTranslations.FirstOrDefault(ct => ct.CustomerNo == customerNo && ct.GridGroup == gridGroup && ct.UserName == userName);
            if (affectedColumns == null)
                return string.Join("~", modelType.GetScaffoldPropertyNames());

            return affectedColumns.ColumnNames;
        }

        public virtual void SetUserGridColumnNames(string gridGroup, string columns)
        {
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

        public void StorePasswordToUser(string userName, string password)
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
