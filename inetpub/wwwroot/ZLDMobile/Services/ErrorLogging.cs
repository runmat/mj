using System;
using System.Web;
using System.IO;

namespace ZLDMobile.Services
{
    public static class ErrorLogging
    {
        public static void WriteErrorToLogFile(string errorMessage, string errorDetail = null, bool isAppException = false)
        {
            if (String.IsNullOrEmpty(((AppSettings)MvcApplication.AppSettings).GlobalErrorlogDirectory))
            {
                return;
            }

            DateTime jetzt = DateTime.Now;

            string strLogVerzeichnis = ((AppSettings) MvcApplication.AppSettings).GlobalErrorlogDirectory;

            if (!Directory.Exists(strLogVerzeichnis))
            {
                try
                {
                    Directory.CreateDirectory(strLogVerzeichnis);
                }
                catch
                {
                    return;
                }
            }

            using (StreamWriter srWriter = new StreamWriter(strLogVerzeichnis + "\\ErrorLog_" + jetzt.ToString("yyyyMMdd") + ".txt", true))
            {
                srWriter.WriteLine("================================================================");
                if (isAppException)
                {
                    srWriter.WriteLine("Unbehandelte Ausnahme in ZLDMobile");
                }
                else
                {
                    srWriter.WriteLine("Fehler in ZLDMobile");
                }
                srWriter.WriteLine("----------------------------------------------------------------");
                srWriter.WriteLine("Fehler        : " + errorMessage);
                srWriter.WriteLine("----------------------------------------------------------------");
                srWriter.WriteLine("Zeit          : " + jetzt.ToString());
                srWriter.WriteLine("----------------------------------------------------------------");
                srWriter.WriteLine("URL           : " + HttpContext.Current.Request.Url.ToString());
                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    srWriter.WriteLine("----------------------------------------------------------------");
                    srWriter.WriteLine("Referrer      : " + HttpContext.Current.Request.UrlReferrer.ToString());
                }
                srWriter.WriteLine("----------------------------------------------------------------");
                srWriter.WriteLine("Client-IP     : " + HttpContext.Current.Request.UserHostAddress);
                srWriter.WriteLine("----------------------------------------------------------------");
                srWriter.WriteLine("Client-Browser: " + HttpContext.Current.Request.Browser.Browser + " " + HttpContext.Current.Request.Browser.Version);
                if (errorDetail != null)
                {
                    srWriter.WriteLine("----------------------------------------------------------------");
                    srWriter.WriteLine("Fehler-Details: " + errorDetail);
                }
                srWriter.Close();
            }
        }

        public static void WriteErrorToLogFile(Exception ex, bool isAppException = false)
        {
            WriteErrorToLogFile(ex.Message, ex.ToString(), isAppException);
        }
    }
}
