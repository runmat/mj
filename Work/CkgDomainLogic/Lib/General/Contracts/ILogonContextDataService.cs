using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Contracts
{
    public enum LogonLevel { Admin = 0, Customer = 1, Group = 2, User = 3 }

    public enum GridColumnMode { Master, Slave }
    
    public interface ILogonContextDataService : ILogonContext, IApplicationUserMenuProvider
    {
        IPersistanceService PersistanceService { get; set; }

        List<ApplicationType> AppTypes { get; set; }

        User User { get; set; }

        bool HasLocalizationTranslationRights { get; }

        string UserNameForDisplay { get; set; }

        WebUserInfo UserInfo { get; set; }

        LogonLevel UserLogonLevel { get; set; }
        
        bool? UserOnProdDataSystem { get; set; }

        Customer Customer { get; set; }

        string CustomerName { get; }

        UserGroup Group { get; set; }

        UserOrganization Organization { get; set; }

        List<IMaintenanceSecurityRuleDataProvider> MaintenanceCoreMessages { get; }

        MaintenanceResult MaintenanceInfo { get;  }

        List<IApplicationUserMenuItem> UserApps { get; set; }

        string ReturnUrl { get; set; }

        // only for backward compatibility:
        string CurrentGridColumns { get; }

        AdminLevel HighestAdminLevel { get; }

        IHtmlString GetUserEncrytpedUrl(IApplicationUserMenuItem menuItem);

        IHtmlString FormatUserEncrytpedUrl(string url);

        IHtmlString FormatUrl(string url);

        void DataContextPersist(object dataContext);

        object DataContextRestore(string typeName);

        void TryLogonUser(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError);

        string TryGetEmailAddressFromUsername(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError);
        
        string GetEmailAddressForUser();

        void CheckIfPasswordResetAllowed(LoginModel loginModel, Action<Expression<Func<LoginModel, object>>, string> addModelError);

        bool CheckPasswordHistory(ChangePasswordModel model, int passwordMinHistoryEntries);

        User TryGetUserFromPasswordToken(string passwordToken, int tokenExpirationMinutes);

        User TryGetUserFromUserName(string userName);

        Customer TryGetCustomerFromUserName(string userName);
        
        List<Contact> TryGetGroupContacts(int customerID, string groupName);
        List<Contact> TryGetGroupContacts();

        void StorePasswordRequestKeyToUser(string userName, string passwordRequestKey);
        void StorePasswordToUser(IPasswordSecurityRuleDataProvider passwordSecurityRuleDataProvider, string userName, string password);

        bool ValidatePassword(string password, User storedUser);

        bool ValidateUser(IUserSecurityRuleDataProvider userSecurityRuleDataProvider, IPasswordSecurityRuleDataProvider passwordSecurityRuleDataProvider, ILocalizationService localizationService, out List<string> localizedValidationErrorMessages);

        string TranslateMenuAppType(IApplicationUserMenuItem menuItem);

        string TranslateMenuAppName(IApplicationUserMenuItem menuItem);

        MaintenanceResult ValidateMaintenance();

        void MaintenanceMessageConfirmAndDontShowAgain();

        int GetAppIdCurrent();

        string GetCategorySettingsForWebUser(string rightname);
    }
}
