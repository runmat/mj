using System.Web.Security;
using GeneralTools.Contracts;

namespace WebTools.Services
{
    public class WebLogonService : ILogonService
    {
        public string EncryptPassword(string password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
        }
    }
}
