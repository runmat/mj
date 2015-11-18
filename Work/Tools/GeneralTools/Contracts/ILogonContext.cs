namespace GeneralTools.Contracts
{
    public interface ILogonContext
    {
        int CustomerID { get; }

        int AppID { get; }

        string KundenNr { get; set; }

        string GroupName { get; set; }

        string UserID { get; set; }

        string UserName { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string FullName { get; }

        string AppUrl { get; set; }

        bool MvcEnforceRawLayout { get; set; }

        string CurrentLayoutTheme { get; set; }

        string GetLoginUrl(string urlEncodedReturnUrl);

        bool UserNameIsValid(string userID);

        bool LogonUser(string userName);

        bool LogonUser(string userName, string password);

        bool LogonUserWithUrlRemoteLoginKey(string urlRemoteLoginKey);

        string GetUserNameFromUrlRemoteLoginKey(string urlRemoteLoginKey);

        void LogoutUser();

        bool ChangePassword(string oldPassword, string newPassword);
        
        ISecurityService SecurityService { get; set; }

        string LogoutUrl { get; set; }

        void AppUrlQueryAndStore();

        void Clear();
    }
}
