namespace CkgDomainLogic.General.Contracts
{
    public interface ILogonContextDataServiceZLDMobile : ILogonContextDataService
    {
        string VkOrg { get; }

        string VkBur { get; }
    }
}
