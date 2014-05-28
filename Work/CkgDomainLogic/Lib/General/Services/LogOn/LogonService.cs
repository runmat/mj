using System.Web.Security;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Services
{
    public class LogonService : ILogonService
    {
        public string EncryptPassword(string password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
        }
    }
}
