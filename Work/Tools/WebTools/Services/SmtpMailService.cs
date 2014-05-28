using System.Collections.Generic;
using System.Web.Helpers;
using GeneralTools.Contracts;

namespace WebTools.Services
{
    public class SmtpMailService : IMailService
    {
        public ISmtpSettings SmtpSettings { get; private set; }
        

        public SmtpMailService(ISmtpSettings smtpSettings)
        {
            SmtpSettings = smtpSettings;

            // Nasser
            // Ich muss hier keine Werte setzen, brauche einfach auf die ISmtpSettings zuzugreifen
            // SetSmtpSettings(smtpSettings);
        }

        private static void SetSmtpSettings(ISmtpSettings smtpSettings)
        {
            WebMail.SmtpServer = smtpSettings.SmtpServer;
            WebMail.From = smtpSettings.SmtpSender;

            //if (!string.IsNullOrEmpty(smtpSettings.SmtpPort))
            //    WebMail.SmtpPort = Convert.ToInt32(smtpSettings.SmtpPort);

            //WebMail.EnableSsl = smtpSettings.SmtpEnableSsl;

            //if (!string.IsNullOrEmpty(smtpSettings.SmtpUserName))
            //{
            //    WebMail.UserName = smtpSettings.SmtpUserName;
            //    WebMail.Password = smtpSettings.SmtpPassword;
            //}
        }

        public bool SendMail(string to, string subject, string body, IEnumerable<string> filesToAttach = null)
        {
            try
            {
                SetSmtpSettings(SmtpSettings);

                WebMail.Send(
                    to: to,
                    subject: subject,
                    body: body,
                    isBodyHtml: true,
                    filesToAttach: filesToAttach
                    );
            }
            catch { return false; }

            return true;
        }
    }
}
