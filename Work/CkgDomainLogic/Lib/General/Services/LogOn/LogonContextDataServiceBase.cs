using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using MvcTools.Models;
using MvcTools.Web;

namespace CkgDomainLogic.General.Services
{
    public class LogonContextDataServiceBase 
    {
        public ISecurityService SecurityService { get; set; }

        public ILocalizationService LocalizationService { get; set; }

        
        public string PersistanceKey { get { return UserName; } }

        public IPersistanceService PersistanceService { get; set; }


        public string LogoutUrl { get; set; }

        // ReSharper disable LocalizableElement

        public int CustomerID { get { return Customer == null ? 0 : Customer.CustomerID; } }

        public int AppID { get { return GetAppIdCurrent(); } }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Gruppenname)]
        public string GroupName { get; set; }

        [LocalizedDisplay(LocalizeConstants.UserName)]
        public string UserName { get; set; }

        public virtual bool HasLocalizationTranslationRights { get { return false; } }

        [LocalizedDisplay(LocalizeConstants.UserName)]
        public string UserNameForDisplay { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string FirstName { get; set; }

        [LocalizedDisplay(LocalizeConstants.LastName)]
        public string LastName { get; set; }

        [LocalizedDisplay(LocalizeConstants.FullName)]
        public string FullName { get { return FirstName.IsNullOrEmpty() ? LastName : string.Format("{0}, {1}", LastName, FirstName); } }

        public string AppUrl { get; set; }

        // ReSharper restore LocalizableElement


        public List<ApplicationType> AppTypes { get; set; }

        public User User { get; set; }

        public WebUserInfo UserInfo { get; set; }

        private LogonLevel _userLogonLevel = LogonLevel.User;
        public LogonLevel UserLogonLevel
        {
            get { return _userLogonLevel; }
            set { _userLogonLevel = value; }
        }

        public bool? UserOnProdDataSystem { get; set; }

        public Customer Customer { get; set; }

        public virtual string CustomerName { get { return Customer == null ? "" : Customer.Customername; } }

        public UserGroup Group { get; set; }

        public Organization Organization { get; set; }

        public List<IApplicationUserMenuItem> UserApps { get; set; }

        public bool AppFavoritesEditMode { get; set; }

        public virtual List<IMaintenanceSecurityRuleDataProvider> MaintenanceCoreMessages { get { return null; } }

        public MaintenanceResult MaintenanceInfo { get; private set; }

        public string UserNameEncryptedToUrlEncoded { get; set; }

        public string UserID { get; set; }

        public bool MvcEnforceRawLayout { get; set; }

        public string CurrentLayoutTheme { get; set; }

        protected static string WebRootPath
        {
            get
            {
                var webRootPath = ConfigurationManager.AppSettings["WebAppPath"];
                webRootPath = webRootPath.IsNullOrEmpty() ? "Services" : webRootPath;
                return webRootPath;
            }
        }

        public string ReturnUrl
        {
            get { return SessionHelper.GetSessionString("ReturnUrl"); }
            set { SessionHelper.SetSessionValue("ReturnUrl", value); }
        }

        // only for backward compatibility:
        public string CurrentGridColumns
        {
            get
            {
                var gridSettings = SessionHelper.GetSessionObject("GridCurrentSettings", () => new GridSettings());

                return gridSettings.Columns;
            }
        }


        virtual public bool AppFavoritesEditSwitchOneFavorite(int appID)
        {
            return false;
        }

        virtual public bool LogonUser(string userName)
        {
            return false;
        }

        virtual public bool LogonUserWithUrlRemoteLoginKey(string urlRemoteLoginKey)
        {
            return false;
        }

        virtual public string GetUserNameFromUrlRemoteLoginKey(string urlRemoteLoginKey)
        {
            return "";
        }

        virtual public bool LogonUser(string userName, string password)
        {
            return false;
        }


        virtual public void LogoutUser()
        {
        }

        virtual public bool ChangePassword(string oldPassword, string newPassword)
        {
            return false;
        }

        virtual public string GetLoginUrl(string urlEncodedReturnUrl)
        {
            return "";
        }

        public bool UserNameIsValid(string userID)
        {
            return true;
        }

        public List<IApplicationUserMenuItem> GetMenuItemGroups()
        {
            if (UserApps == null)
                return new List<IApplicationUserMenuItem>();

            var orderedList = UserApps
                .Where(ua => GetAppTypeFriendlyName(ua.AppType).IsNotNullOrEmpty())
                    .GroupBy(ua => ua.AppType)
                        .Select(ua2 => UserApps.FirstOrDefault(uaGroup => uaGroup.AppType == ua2.Key))
                            .OrderBy(ua => ua.AppTypeRank)
                                .ToList();

            return orderedList;
        }

        public List<IApplicationUserMenuItem> GetMenuItems(string appType = null)
        {
            if (UserApps == null)
                return new List<IApplicationUserMenuItem>();

            var menuItems = UserApps;
            
            if (appType.IsNotNullOrEmpty())
                menuItems = menuItems.Where(ua => ua.AppType == appType).ToList();

            var menuItemsList = menuItems.OrderBy(ua => ua.AppRank).ThenBy(TranslateMenuAppName).ToList();
            menuItemsList.ForEach(ConvertMenuItemUrl);
            return menuItemsList;
        }

        virtual protected void ConvertMenuItemUrl(IApplicationUserMenuItem menuItem)
        {
        }

        public IHtmlString GetUserEncrytpedUrl(IApplicationUserMenuItem menuItem)
        {
            if (UserNameEncryptedToUrlEncoded.IsNullOrEmpty())
                return new HtmlString("#");

            var appUrl = menuItem.AppURL.ToLower(); 
            appUrl = FormatUserEncrytpedUrl(appUrl).ToString();

            return new HtmlString(appUrl); // new HtmlString(appUrl.Replace("?", string.Format("?appID={0}&", menuItem.AppID)));
        }

        public IHtmlString FormatUserEncrytpedUrl(string url)
        {
            if (UserNameEncryptedToUrlEncoded.IsNullOrEmpty())
                return new HtmlString("#");

            url = FormatUrl(url).ToString();

            if (url.Contains("?"))
            {
                return new HtmlString(string.Format("{0}&un={1}", url, UserNameEncryptedToUrlEncoded));
            }

            return new HtmlString(string.Format("{0}?un={1}", url, UserNameEncryptedToUrlEncoded));
        }

        public IHtmlString FormatUrl(string url)
        {
            var appUrl = url.ToLower();
            appUrl = appUrl.Replace("../", string.Format("/{0}/", WebRootPath));
            if (appUrl.StartsWith("mvc/"))
                appUrl = appUrl.Replace("mvc/", string.Format("/{0}Mvc/", WebRootPath));

            return new HtmlString(appUrl);
        }

        virtual protected string GetAppTypeFriendlyName(string appType)
        {
            switch (appType.ToLower())
            {
                case "change":
                    return "Zulassung";

                case "changeah":
                    return "Autohaus";

                case "report":
                    return "Aufträge";

                case "tools":
                    return "Tools";
            }

            return "";
        }

        protected bool GetMvcRawLayoutFlag()
        {
            if (ConfigurationManager.AppSettings["ForceResponsiveLayout"].NotNullOrEmpty().ToLower() == "true")
                //   note: web.config appsettings parameter "ForceResponsiveLayout" overrides customer or user setting (i. e. "MvcRawLayout")
                return false;

            if (HttpContext.Current.Request["MvcEnforceRawLayout"].NotNullOrEmpty() == "1")
                // note: request querystring parameter "MvcEnforceRawLayout" overrides customer or user setting (i. e. "MvcRawLayout")
                return true;

            return Customer.MvcRawLayout;
        }

        public virtual void DataContextPersist(object dataContext) 
        {
        }

        public virtual object DataContextRestore(string typeName) 
        {
            return null;
        }

        public virtual IEnumerable<string> GetAddressPostcodeCityMappings(string plz)
        {
            return new List<string>();
        }

        public virtual void TryLogonUser(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {

        }

        public virtual string TryGetEmailAddressFromUsername(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
            return "";
        }

        public virtual void CheckIfPasswordResetAllowed(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {

        }

        public virtual bool CheckPasswordHistory(ChangePasswordModel model, int passwordMinHistoryEntries)
        {
            return true;
        }

        public virtual User TryGetUserFromPasswordToken(string passwordToken, int tokenExpirationMinutes)
        {
            return null;
        }

        public virtual User TryGetUserFromUserName(string userName)
        {
            return null;
        }
        
        public virtual Customer TryGetCustomerFromUserName(string userName)
        {
            return null;
        }

        public virtual List<Contact> TryGetGroupContacts(int customerID, string groupName)
        {
            return new List<Contact>();
        }

        public virtual List<Contact> TryGetGroupContacts()
        {
            return new List<Contact>();
        }

        public virtual void StorePasswordRequestKeyToUser(string userName, string passwordRequestKey)
        {
        }

        public virtual void StorePasswordToUser(IPasswordSecurityRuleDataProvider passwordSecurityRuleDataProvider, string userName, string password)
        {
        }

        public bool ValidatePassword(string password, User storedUser)
        {
            if (SecurityService.EncryptPassword(password) == storedUser.Password)
                return true;

            if (password == storedUser.Password)
                // try clear text password validation
                return true;

            return false;
        }

        public MaintenanceResult ValidateMaintenance()
        {
            return MaintenanceInfo = SecurityService.ValidateMaintenance(MaintenanceCoreMessages);
        }

        public bool ValidateUser(IUserSecurityRuleDataProvider userSecurityRuleDataProvider, IPasswordSecurityRuleDataProvider passwordSecurityRuleDataProvider, ILocalizationService localizationService, out List<string> localizedValidationErrorMessages)
        {
            return SecurityService.ValidateUser(userSecurityRuleDataProvider, passwordSecurityRuleDataProvider, localizationService, out localizedValidationErrorMessages);
        }

        public virtual string TranslateMenuAppType(IApplicationUserMenuItem menuItem)
        {
            if (LocalizationService == null)
                return menuItem.AppTypeFriendlyName;

            var translatedText = LocalizationService.TranslateResourceKey(menuItem.AppType);

            return translatedText.IsNotNullOrEmpty() ? translatedText : menuItem.AppTypeFriendlyName;
        }

        public virtual string TranslateMenuAppName(IApplicationUserMenuItem menuItem)
        {
            if (LocalizationService == null)
                return menuItem.AppFriendlyName;

            var translatedText = LocalizationService.TranslateResourceKey(menuItem.AppName);

            return translatedText.IsNotNullOrEmpty() ? translatedText : menuItem.AppFriendlyName;
        }

        public void AppUrlQueryAndStore()
        {
            if (HttpContext.Current == null)
                return;

            AppUrl = HttpContext.Current.Request.Url.AbsolutePath;
        }

        public void RewriteUrlToLogPageVisit(IApplicationUserMenuItem menuItem)
        {
            if (menuItem.AppURL.NotNullOrEmpty().ToLower().StartsWith("http"))
                return;

            var appId = menuItem.AppID;
            var url = menuItem.AppURL;
            var urlUtf8 = Encoding.UTF8.GetString(Encoding.Default.GetBytes(url));
            var urlEncoded = HttpUtility.UrlEncode(urlUtf8);

            var modifiedUrl = string.Concat("mvc/DomainCommon/LogPageVisit?", "logappid=", appId, "&url=", urlEncoded);
            menuItem.AppURL = modifiedUrl;
        }

        virtual public void MaintenanceMessageConfirmAndDontShowAgain()
        {
            
        }

        public int GetAppIdCurrent()
        {
            return LogonContextHelper.GetAppIdCurrent(UserApps);
        }

        public virtual string GetEmailAddressForUser()
        {
            return "";
        }

        public void TrySetLogoutLink()
        {
            if (Customer != null)
                LogoutUrl = Customer.LogoutLink;
    }
}
}
