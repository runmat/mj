namespace CkgDomainLogic.General.Contracts
{
    public interface ILogonContextDataServiceAutohaus : ILogonContextDataService
    {
        string VkOrg { get; }

        string VkBur { get; }
    }
}
