namespace GeneralTools.Contracts
{
    public interface IUserSecurityRuleDataProvider
    {
        bool UserIsApproved { get; }

        bool UserIsDisabled { get; }

        int UserCountFailedLogins { get; }
    }
}
