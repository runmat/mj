using System.Collections.Generic;

namespace GeneralTools.Contracts
{
    public interface IMailService
    {
        bool SendMail(string to, string subject, string body, IEnumerable<string> filesToAttach = null);
    }
}