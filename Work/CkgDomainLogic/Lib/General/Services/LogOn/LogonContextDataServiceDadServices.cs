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
using WebTools.Services;

namespace CkgDomainLogic.General.Services
{
    /// <summary>
    /// DAD Services spezifischer LogonContext
    /// </summary>
    public class LogonContextDataServiceDadServices : LogonContextDataServiceBase, ILogonContextDataService
    {
        public string CurrentGridColumns { get; set; }

        private IEnumerable<LoginUserMessageConfirmations> _loginUserMessageConfirmations;

        public override List<IMaintenanceSecurityRuleDataProvider> MaintenanceCoreMessages
        {
            get { return MaintenanceCoreLoginUserMessages.Cast<IMaintenanceSecurityRuleDataProvider>().ToList(); }
        }

        public List<LoginUserMessage> MaintenanceCoreLoginUserMessages
        {
            get
            {
                var messageList = CreateDbContext().ActiveLoginMessages.ToListOrEmptyList();

                messageList.ForEach(message => message.MessageIsConfirmedByUser = GetLoginUserMessageConfirmations(message.ID, message.ShowMessageFrom).Any());

                return messageList.ToList();
            }
        }


        public override string GetLoginUrl(string urlEncodedReturnUrl)
        {
            var returnUrl = HttpUtility.UrlDecode(urlEncodedReturnUrl).NotNullOrEmpty().ToLower();

            if (ReturnUrl.IsNullOrEmpty() && ! returnUrl.Contains(GetSubDomainPath("login")))
                ReturnUrl = urlEncodedReturnUrl;

            if (   ConfigurationManager.AppSettings["ForceResponsiveLayout"].NotNullOrEmpty().ToLower() == "true" 
                ||
                   ConfigurationManager.AppSettings["ForceResponsiveLayoutSubDomains"].NotNullOrEmpty().Split(',').Any(subDomain => returnUrl.Contains(GetSubDomainPath(subDomain.ToLower())) || returnUrl.EndsWith(subDomain.ToLower()))
                )
                return string.Format("/ServicesMvc/Login/Index?ReturnUrl={0}", urlEncodedReturnUrl);

            return string.Format("/{0}/Start/Login.aspx?ReturnUrl={1}", WebRootPath, urlEncodedReturnUrl);
        }

        private static DomainDbContext CreateDbContext(string userName)
        { 
            return new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], userName);
        }

        private DomainDbContext CreateDbContext()
        {
            return CreateDbContext(UserName);
        }

        public override bool LogonUser(string userName, string password)
        {
            return false;
        }

        public override bool LogonUser(string userName)
        {
            return LogonUserCore(userName, false);
        }

        public override bool LogonUserWithUrlRemoteLoginKey(string urlRemoteLoginKey)
        {
            return LogonUserCore(urlRemoteLoginKey, true);
        }

        public override string GetUserNameFromUrlRemoteLoginKey(string urlRemoteLoginKey)
        {
            var user = CreateDbContext("").GetUserFromUrlRemoteLoginKey(urlRemoteLoginKey);
            if (user == null)
                return "";

            return user.Username;
        }

        private bool LogonUserCore(string userName, bool userNameIsUrlRemoteLoginKey)
        {
            if (UserName == userName)
                return true;

            UserName = userName;

            UserNameEncryptedToUrlEncoded = "";
            if (userName.IsNullOrEmpty())
                return true;

            var dbContext = CreateDbContext(userName);

            if (userNameIsUrlRemoteLoginKey)
                User = dbContext.GetAndSetUserFromUrlRemoteLoginKey(userName);
            else
                User = dbContext.GetAndSetUser(userName);

            if (User == null)
                return false;

            if (userNameIsUrlRemoteLoginKey)
            {
                var loginModel = new LoginModel { UserName = User.Username, Password = User.Password };
                var modelErrors = new Dictionary<string, string>();
                TryLogonUser(loginModel, (s, e) => modelErrors.Add(s.GetPropertyName(), e));
                if (modelErrors.Any())
                    return false;
            }

            _loginUserMessageConfirmations = null;
            UserName = User.Username;
            UserInfo = dbContext.GetUserInfo();
            AppTypes = dbContext.ApplicationTypes.ToList();
            Customer = dbContext.GetCustomer(User.CustomerID);
            if (Customer == null)
                return false;

            if (userNameIsUrlRemoteLoginKey)
                if (!Customer.AllowUrlRemoteLogin.GetValueOrDefault())
                    return false;

            var userGroup = dbContext.UserGroup;

            UserID = User.UserID.ToString();
            UserOnProdDataSystem = !User.TestUser;
            KundenNr = Customer.KUNNR;
            if (userGroup != null)
            {
                Group = userGroup;
                GroupName = userGroup.GroupName;
            }

            var organization = dbContext.Organization;
            if (organization != null)
            {
                Organization = organization;
            }
                
            MvcEnforceRawLayout = GetMvcRawLayoutFlag();

            FirstName = User.FirstName;
            LastName = User.LastName;

            UserNameEncryptedToUrlEncoded = CryptoMd5.EncryptToUrlEncoded(User.Username);
            UserApps = dbContext.UserApps.Where(ua => ua.AppInMenu).Cast<IApplicationUserMenuItem>().ToList();
            UserApps.ForEach(ua =>
                {
                    var appType = AppTypes.FirstOrDefault(at => at.AppType == ua.AppType);
                    if (appType != null)
                    {
                        ua.AppTypeRank = appType.Rank;
                        ua.AppTypeFriendlyName = GetAppTypeFriendlyName(appType.AppType);
                    }
                    RewriteUrlToLogPageVisit(ua);
                });

            dbContext.SetLastLogin(DateTime.Now);

            return true;
        }

        public override void TryLogonUser(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
            var maintenance = ValidateMaintenance();
            if (maintenance.LogonDisabled)
            {
                addModelError(m => m.UserName, Localize.LoginDisabledDueToMaintenance);
                return;
            }

            var dbContext = CreateDbContext(loginModel.UserName);
            if (dbContext.User == null)
            {
                addModelError(m => m.UserName, Localize.LoginUserOrPasswordWrong);
                return;
            }

            var customer = dbContext.GetCustomer(dbContext.User.CustomerID);

            if (!ValidatePassword(loginModel.Password, dbContext.User))
            {
                if (dbContext.User.UserCountFailedLogins < customer.LockedAfterNLogins)
                {
                    addModelError(m => m.UserName, Localize.LoginUserOrPasswordWrong);
                    dbContext.FailedLoginsIncrementAndSave(loginModel.UserName);
                }
                else
                {
                    if (!dbContext.User.UserIsDisabled)
                        dbContext.LockUserAndSave(loginModel.UserName);

                    addModelError(m => m.UserName, Localize.LoginUserDisabled);
                }
                
                return;
            }

            List<string> userErrorMessages;
            if (!ValidateUser(dbContext.User, customer, LocalizationService, out userErrorMessages))
            {
                addModelError(m => m.UserName, userErrorMessages.FirstOrDefault());
                return;
            }

            if (customer != null && customer.PortalType.NotNullOrEmpty().ToLower() != "mvc")
            {
                var urlParam = "FromMvc_" + dbContext.User.UserID + "_" + DateTime.Now.ToString("dd.MM.yyyy-HH:mm");
                var crypted = CryptoMd5.EncryptToUrlEncoded(urlParam);
                ReturnUrl = "/Services/Start/Login.aspx?unm=" + crypted;
            }

            loginModel.RedirectUrl = ReturnUrl;
            ReturnUrl = "";

            dbContext.FailedLoginsResetAndSave(loginModel.UserName);
            LogonUser(loginModel.UserName);
        }

        public override string TryGetEmailAddressFromUsername(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
            var dbContext = CreateDbContext(loginModel.UserName);
            //if (dbContext.User == null)
            //    addModelError(m => m.UserName, Localize.LoginUserDoesNotExist);

            var email = dbContext.GetEmailAddressFromUserName(dbContext.UserName);
            //if (email.IsNullOrEmpty())
            //    addModelError(m => m.UserName, Localize.UserInvalidEmail);

            return email;
        }

        public override void CheckIfPasswordResetAllowed(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError)
        {
            var dbContext = CreateDbContext(loginModel.UserName);
            if (dbContext.User != null && dbContext.User.AccountIsLockedOut)
            {
                var lastLockedBy = dbContext.GetUserAccountLastLockedBy(dbContext.UserName);

                if (String.Compare(loginModel.UserName, lastLockedBy, true) != 0 && String.Compare("[admin-regelprozess]", lastLockedBy, true) != 0)
                    addModelError(m => m.UserName, Localize.PasswordResetNotAllowedHint);
            }
        }

        public override User TryGetUserFromPasswordToken(string passwordToken, int tokenExpirationMinutes)
        {
            if (!SecurityService.ValidatePasswordResetToken(passwordToken, tokenExpirationMinutes))
                return null;

            var dbContext = CreateDbContext();

            return dbContext.GetUserFromPasswordToken(passwordToken);
        }

        public override User TryGetUserFromUserName(string userName)
        {
            var dbContext = CreateDbContext();

            return dbContext.GetUser(userName);
        }

        public override Customer TryGetCustomerFromUserName(string userName)
        {
            var dbContext = CreateDbContext();

            return dbContext.GetCustomerFromUserName(userName);
        }

        public override List<Contact> TryGetGroupContacts(int customerID, string groupName)
        {
            var dbContext = CreateDbContext();

            return dbContext.GetGroupContacts(customerID, groupName);
        }

        public override List<Contact> TryGetGroupContacts()
        {
            if (Customer == null)
                return new List<Contact>();

            var dbContext = CreateDbContext();

            return dbContext.GetGroupContacts(Customer.CustomerID, GroupName);
        }

        public override void StorePasswordToUser(string userName, string password)
        {
            var dbContext = CreateDbContext(userName);
            if (dbContext.User == null)
                return;

            dbContext.StorePasswordToUser(userName, password);
        }

        public override void StorePasswordRequestKeyToUser(string userName, string passwordRequestKey)
        {
            var dbContext = CreateDbContext(userName);
            if (dbContext.User == null)
                return;

            dbContext.StorePasswordRequestKeyToUser(userName, passwordRequestKey);
        }

        public override void LogoutUser()
        {
            UserID = "";
            UserName = "";
            User = null;
            UserInfo = null;
            FirstName = "";
            LastName = "";
            KundenNr = "";
            Customer = null;
            GroupName = "";
            Group = null;
            Organization = null;
            if (AppTypes != null)
                AppTypes.Clear();
            if (UserApps != null)
                UserApps.Clear();
            UserNameEncryptedToUrlEncoded = "";
        }

        public override bool ChangePassword(string oldPassword, string newPassword)
        {
            return false;
        }

        override protected string GetAppTypeFriendlyName(string appType)
        {
            var appTypeModel = AppTypes.FirstOrDefault(a => a.AppType == appType);
            if (appTypeModel == null)
                return "";

            return Localize.TranslateResourceKey(appTypeModel.AppType);
        }

        override protected void ConvertMenuItemUrl(IApplicationUserMenuItem menuItem)
        {
            var url = menuItem.AppURL;

            if (url.StartsWith("/"))
                return;

            url = url.Replace("../", "");
            menuItem.AppURL = string.Format("/{0}{1}{2}", 
                WebRootPath.ToLower(), 
                url.StartsWith("mvc/") ? "" : "/",
                url);
        }

        int KundenNrToInt()
        {
            int customerNo;
            if (!Int32.TryParse(KundenNr, out customerNo))
                return -1;

            return customerNo;
        }

        private ColumnTranslationsMvc GetDbContextColumnTranslations(DomainDbContext dbContext, GridColumnMode gridColumnMode, string gridGroup, LogonLevel logonLevel, string defaultColumnNames)
        {
            var customerNo = 0;
            var userGroupName = "";
            var userName = "";

            if (gridGroup.IsNullOrEmpty())
                return null;

            if (logonLevel == LogonLevel.Admin)
            {
                if (gridColumnMode == GridColumnMode.Master)
                    return null;
            }
            if (logonLevel == LogonLevel.Customer)
            {
                if (gridColumnMode == GridColumnMode.Slave)
                    customerNo = KundenNrToInt();
            }
            if (logonLevel == LogonLevel.Group)
            {
                customerNo = KundenNrToInt();
                if (gridColumnMode == GridColumnMode.Slave)
                    userGroupName = GroupName;
            }
            if (logonLevel == LogonLevel.User)
            {
                customerNo = KundenNrToInt();
                userGroupName = GroupName;
                if (gridColumnMode == GridColumnMode.Slave)
                    userName = UserName;
            }

            var affectedColumn = dbContext.ColumnTranslations.FirstOrDefault(ct =>  ct.GridGroup == gridGroup && ct.CustomerNo == customerNo && ct.UserGroupName == userGroupName && ct.UserName == userName);
            if (affectedColumn == null)
            {
                ColumnTranslationsMvc newDefaultColumnNames = null;
                
                if (gridColumnMode == GridColumnMode.Slave)
                    // if no columnNames perstisted yet in *slave* mode, let's ask the corresponing *master* persistent columnNames
                    newDefaultColumnNames = GetDbContextColumnTranslations(dbContext, GridColumnMode.Master, gridGroup, logonLevel, defaultColumnNames);

                if (gridColumnMode == GridColumnMode.Master)
                {
                    // if no columnNames perstisted yet in *master* mode, let's ask the corresponing *slave* persistent columnNames ** OF THE PARENT GridColumnMode **
                    var parentLogonLevel = GetParentLogonLevel(logonLevel);
                    if (logonLevel != parentLogonLevel)
                        newDefaultColumnNames = GetDbContextColumnTranslations(dbContext, GridColumnMode.Slave, gridGroup, parentLogonLevel, defaultColumnNames);
                }

                if (gridColumnMode == GridColumnMode.Slave)
                {
                    affectedColumn = new ColumnTranslationsMvc
                        {
                            GridGroup = gridGroup,
                            CustomerNo = customerNo,
                            UserName = userName,
                            UserGroupName = userGroupName,
                            ColumnNames = newDefaultColumnNames == null ? defaultColumnNames : newDefaultColumnNames.ColumnNames,
                        };
                    dbContext.ColumnTranslations.Add(affectedColumn);
                    dbContext.SaveChanges();
                }
            }

            return affectedColumn;
        }

        private static LogonLevel GetParentLogonLevel(LogonLevel logonLevel)
        {
            var logonLevelAsInt = Int32.Parse(logonLevel.ToString("D"));
            if (logonLevelAsInt == 0)
                return logonLevel;

            logonLevelAsInt--;

            var values = Enum.GetValues(typeof(LogonLevel));
            return (LogonLevel)values.GetValue(logonLevelAsInt);
        }

        public override string GetUserGridColumnNames(Type modelType, GridColumnMode gridColumnMode, string gridGroup)
        {
            var defaultColumnNames = string.Join(",", modelType.GetScaffoldPropertyNames());
            if (gridGroup.IsNullOrEmpty())
                return defaultColumnNames;

            var dbContext = CreateDbContext();
            var affectedColumns = GetDbContextColumnTranslations(dbContext, gridColumnMode, gridGroup, UserLogonLevel, defaultColumnNames);

            if (affectedColumns == null)
                return defaultColumnNames;

            return TakeAndSaveOnlyIntersectionsInHierarchicalColumnTranslations(dbContext, affectedColumns);
        }

        static string TakeAndSaveOnlyIntersectionsInHierarchicalColumnTranslations(DomainDbContext dbContext, ColumnTranslationsMvc columnTranslations)
        {
            var allParallelTranslations = dbContext.ColumnTranslations
                                                   .Where(ct => ct.GridGroup == columnTranslations.GridGroup)
                                                   .OrderBy(ct => ct.CustomerNo).ThenBy(ct => ct.UserGroupName).ThenBy(ct => ct.UserName);
            if (allParallelTranslations.None())
                return columnTranslations.ColumnNames;

            var parentColumns = allParallelTranslations.ToList().TakeWhile(ct => !ct.Equals(columnTranslations)).ToList();
            if (parentColumns.None())
                return columnTranslations.ColumnNames;

            parentColumns.Add(columnTranslations);
            var parentTranslation = parentColumns.First();
            parentColumns.ToList().ForEach(ct =>
                {
                    var parentColumnList = parentTranslation.ColumnNames.Split(',');
                    var thisColumnList = ct.ColumnNames.Split(',').ToList();

                    thisColumnList = thisColumnList.Intersect(parentColumnList).ToList();
                    if (thisColumnList.Any())
                    {
                        ct.ColumnNames = string.Join(",", thisColumnList);
                        dbContext.SaveChanges();
                    }

                    parentTranslation = ct;
                });

            return columnTranslations.ColumnNames;
        }

        public override void SetUserGridColumnNames(string gridGroup, string columns)
        {
            if (gridGroup == null)
                return;

            const GridColumnMode gridColumnMode = GridColumnMode.Slave;
            
            var dbContext = CreateDbContext();
            var affectedColumns = GetDbContextColumnTranslations(dbContext, gridColumnMode, gridGroup, UserLogonLevel, "");
            affectedColumns.ColumnNames = columns;
            dbContext.SaveChanges();
        }

        public override void DataContextPersist(object dataContext) 
        {
            CreateDbContext().DataContextPersist(dataContext);
        }

        public override object DataContextRestore(string typeName) 
        {
            return CreateDbContext().DataContextRestore(typeName);
        }

        public override IEnumerable<string> GetAddressPostcodeCityMappings(string plz)
        {
            return CreateDbContext().GetAddressPostcodeCityMapping(plz);
        }

        private static string GetSubDomainPath(string subDomain)
        {
            return string.Format("/{0}/", subDomain.Trim().ToLower());
        }

        public override string CustomerName
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Request.Url.ToString().ToLower().Contains("/charts/") && base.CustomerName.ToLower().StartsWith("zmjetest"))
                    return "DAD Dashboards";

                return base.CustomerName;
            }
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

        override public bool AppFavoritesEditSwitchOneFavorite(int appID)
        {
            var sql = string.Format("update ApplicationUserSettings set AppIsMvcFavorite = case when (AppIsMvcFavorite = 0) then 1 else 0 end where AppID = {0} and UserID = {1}", appID, UserID);
            CreateDbContext().Database.ExecuteSqlCommand(sql);
            
            CreateDbContext().UserAppsRefresh();
            UserApps = CreateDbContext().UserApps.Where(ua => ua.AppInMenu).Cast<IApplicationUserMenuItem>().ToList();

            return UserApps.First(a => a.AppID == appID).AppIsMvcFavorite;
        }

        IEnumerable<LoginUserMessageConfirmations> GetLoginUserMessageConfirmations(int? messageID = null, DateTime? showMessageFromDate = null)
        {
            var confirmations = _loginUserMessageConfirmations ?? (_loginUserMessageConfirmations = CreateDbContext().GetLoginUserMessageConfirmations());

            if (messageID == null && showMessageFromDate == null)
                return confirmations;

            return confirmations.Where(c => c.MessageID == messageID && c.ShowMessageFrom == showMessageFromDate);
        }

        override public void MaintenanceMessageConfirmAndDontShowAgain()
        {
            var dbContext = CreateDbContext();
            MaintenanceCoreLoginUserMessages.ForEach(message =>
            {
                if (message.MaintenanceShowAndLetConfirmMessageAfterLogin)
                    dbContext.SetLoginUserMessageConfirmation(message.ID, message.ShowMessageFrom.GetValueOrDefault());
            });
        }
    }
}
