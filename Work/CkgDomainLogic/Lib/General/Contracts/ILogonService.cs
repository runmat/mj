namespace CkgDomainLogic.General.Contracts
{
    public interface ILogonService
    {
        string EncryptPassword(string password);
    }
}