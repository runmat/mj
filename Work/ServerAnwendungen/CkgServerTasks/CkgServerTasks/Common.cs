using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace CkgServerTasks
{
    public static class Common
    {
        public static void SendMail(string betreff, string mailText)
        {
            try
            {
                string mailServer = ConfigurationManager.AppSettings["SmtpMailServer"];
                string mailAbsender = ConfigurationManager.AppSettings["SmtpMailSender"];
                string mailEmpfaenger = ConfigurationManager.AppSettings["SmtpMailRecipient"];

                MailMessage mail = new MailMessage(mailAbsender, mailEmpfaenger, betreff, mailText);

                SmtpClient client = new SmtpClient(mailServer);
                client.Send(mail);
            }
            catch (Exception)
            {
                // NOP
            }
        }

        public static void SendErrorMail(string fehler)
        {
            try
            {
                string mailBody = "Es ist ein Anwendungsfehler aufgetreten!" + Environment.NewLine + Environment.NewLine;
                mailBody += "System  : " + Environment.MachineName + Environment.NewLine;
                mailBody += "Quelle  : <" + Assembly.GetExecutingAssembly().FullName + ">" + Environment.NewLine;
                mailBody += "Fehler  : " + fehler + Environment.NewLine;
                mailBody += "Details : " + Environment.CurrentDirectory + "\\log.txt" + Environment.NewLine + Environment.NewLine;

                mailBody += "Info an : IT-Abteilung Webentwicklung";

                SendMail("Maschinell erzeugte Mail.", mailBody);
            }
            catch (Exception)
            {
                // NOP
            }
        }

        public static void WriteLogEntry(string kategorie, string logMessage)
        {
            try
            {
                FileInfo protocol = new FileInfo("log.txt");

                using (StreamWriter writer = new StreamWriter(protocol.FullName, protocol.Exists))
                {
                    writer.WriteLine(kategorie + ": " + logMessage);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                SendErrorMail(ex.Message);
            }
        }
    }
}
