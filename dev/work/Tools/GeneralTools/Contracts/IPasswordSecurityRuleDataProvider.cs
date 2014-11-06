namespace GeneralTools.Contracts
{
    public interface IPasswordSecurityRuleDataProvider
    {
        int PasswordMinNumericChars { get; }

        int PasswordMinLength { get; }

        int PasswordMinCapitalChars { get; }

        int PasswordMinSpecialChars { get; }

        int PasswordMinHistoryEntries { get; }

        int PasswordMaxLoginFailures { get; }

        //[Obsolete]
        //int PasswordLockedAfterFailedLogins { get; }
    }
}
