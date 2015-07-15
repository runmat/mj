using System;
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
        }

        private static void SetSmtpSettings(ISmtpSettings smtpSettings)
        {
            WebMail.SmtpServer = smtpSettings.SmtpServer;
            WebMail.From = smtpSettings.SmtpSender;
        }

        public bool SendMail(string to, string subject, string body, IEnumerable<string> filesToAttach = null)
        {
            return SendMailMain(to, subject, body, filesToAttach) == null;
        }

        public string SendMailMain(string to, string subject, string body, IEnumerable<string> filesToAttach = null)
        {
            string result = null;

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
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

    }
}
