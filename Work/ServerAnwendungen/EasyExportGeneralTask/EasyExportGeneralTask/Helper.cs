using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using UniversalFileBasedLogging;

namespace EasyExportGeneralTask
{
    public static class Helper
    {
        /// <summary>
        /// Kennzeichen verfälschen
        /// </summary>
        /// <param name="kennzeichen"></param>
        public static string verfaelscheKennzeichen(string kennzeichen)
        {
            string erg = kennzeichen;

            if (!String.IsNullOrEmpty(kennzeichen))
            {
                for (int i = 0; i < kennzeichen.Length; i++)
                {
                    if (Char.IsNumber(kennzeichen[i]))
                    {
                        erg = kennzeichen.Insert(i, " ");
                        break;
                    }
                }
            }

            return erg;
        }

        /// <summary>
        /// EMail versenden
        /// </summary>
        /// <param name="betreff"></param>
        /// <param name="text"></param>
        /// <param name="empfaenger"></param>
        /// <param name="dateipfad">sendet die angegebene Datei als Anhang mit</param>
        /// <param name="absender">abweichender Absender - optional</param>
        public static void SendEMail(string betreff, string text, string empfaenger, string dateipfad, string absender = null)
        {
            string strBetreff = betreff;
            string strText = text;
            string strAbsender = (absender ?? Konfiguration.mailAbsender);
            string strEmpfaenger = empfaenger;
            string strDateiname = dateipfad;

            // Für Tests Mailempfänger umschalten
            if (!Konfiguration.isProdSap)
            {
                strEmpfaenger = Konfiguration.mailEmpfaengerTest;
            }

            MailMessage mail = new MailMessage {
                    From = new MailAddress(strAbsender),
                    Subject = strBetreff,
                    Body = strText
                };

            mail.To.Add(strEmpfaenger.Replace(';', ','));

            if ((!String.IsNullOrEmpty(strDateiname)) && (File.Exists(strDateiname)))
            {
                mail.Attachments.Add(new Attachment(strDateiname));
            }

            SmtpClient mailserver = new SmtpClient(Konfiguration.mailSmtpServer);
            mailserver.Send(mail);
        }

        /// <summary>
        /// EMail versenden
        /// </summary>
        /// <param name="betreff"></param>
        /// <param name="text"></param>
        /// <param name="empfaenger"></param>
        /// <param name="dateiBytes">sendet die angegebene Datei als Anhang mit</param>
        /// <param name="dateiName">Name für den Dateianhang</param>
        /// <param name="absender">abweichender Absender - optional</param>
        public static void SendEMail(string betreff, string text, string empfaenger, byte[] dateiBytes, string dateiName, string absender = null)
        {
            string strBetreff = betreff;
            string strText = text;
            string strAbsender = (absender ?? Konfiguration.mailAbsender);
            string strEmpfaenger = empfaenger;

            // Für Tests Mailempfänger umschalten
            if (!Konfiguration.isProdSap)
            {
                strEmpfaenger = Konfiguration.mailEmpfaengerTest;
            }

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(strAbsender),
                Subject = strBetreff,
                Body = strText
            };

            mail.To.Add(strEmpfaenger.Replace(';', ','));

            if (dateiBytes != null)
            {
                mail.Attachments.Add(new Attachment(new MemoryStream(dateiBytes), dateiName));
            }

            SmtpClient mailserver = new SmtpClient(Konfiguration.mailSmtpServer);
            mailserver.Send(mail);
        }

        /// <summary>
        /// Fehler-EMail versenden
        /// </summary>
        /// <param name="betreff"></param>
        /// <param name="text"></param>
        public static void SendErrorEMail(string betreff, string text)
        {
            SendErrorMail(betreff, text);
        }

        /// <summary>
        /// Fehler-EMail versenden
        /// </summary>
        /// <param name="betreff"></param>
        /// <param name="lfiles"></param>
        public static void SendErrorEMail(string betreff, List<LogFile> lfiles)
        {
            SendErrorMail(betreff, "", lfiles);
        }

        /// <summary>
        /// Fehler-EMail versenden
        /// </summary>
        /// <param name="betreff"></param>
        /// <param name="text"></param>
        /// <param name="lfiles"></param>
        private static void SendErrorMail(string betreff, string text, List<LogFile> lfiles = null)
        {
            string strBetreff = betreff;
            string strText = text;
            string strAbsender = Konfiguration.mailAbsenderError;
            string strEmpfaenger = Konfiguration.mailEmpfaengerError;

            if (lfiles != null)
            {
                strText = "Fehlerbericht:\r\n";
                foreach (LogFile lfile in lfiles)
                {
                    strText += "Die Datei " + lfile.Filename + " konnte nicht ordnungsgemäß aus dem EasyArchiv abgerufen werden!\r\n";
                }
                strText += "Der Fehler wurde verursacht vom Programm: EasyExportGeneralTask.exe";
            }

            MailMessage mail = new MailMessage {
                From = new MailAddress(strAbsender),
                Subject = strBetreff,
                Body = strText
            };

            mail.To.Add(strEmpfaenger.Replace(';', ','));

            SmtpClient mailserver = new SmtpClient(Konfiguration.mailSmtpServer);
            mailserver.Send(mail);
        }
    }
}
