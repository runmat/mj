using System;
using GeneralTools.Models;
using GeneralTools.Services;

namespace WebTools.Services
{
    public class UserSecurityService
    {
        #region User URL Remote Login

        public static bool UrlRemoteUserTryLogin(string requestRemoteLoginID, string requestRemoteLoginDateTime, int tokenExpirationMinutes)
        {
            var rid = requestRemoteLoginID;
            var dat = requestRemoteLoginDateTime;

            if (rid.IsNullOrEmpty())
                return false;
            if (dat.IsNullOrEmpty())
                return false;

            if ((rid.Length < 30))
                return false;
            if ((!UrlRemoteHashedDateIsValid(dat, tokenExpirationMinutes)))
                return false;

            return true;
        }

        public static bool UrlRemoteHashedDateIsValid(string strHashedDate, int tokenExpirationMinutes)
        {
            if ((string.IsNullOrEmpty(strHashedDate)))
                return false;
            if ((strHashedDate.Length != 10))
                return false;

            var strEncryptedDate = "";
            var reversal = false;
            for (var i = 0; i <= strHashedDate.Length - 1; i++)
            {
                var hashedChar = strHashedDate[i];
                int hashedVal = hashedChar;
                strEncryptedDate = strEncryptedDate + (char)((!reversal ? hashedVal : 'A' + 'Z' - hashedVal) - 30);
                reversal = !reversal;
            }

            if ((string.IsNullOrEmpty(strEncryptedDate)))
                return false;
            if ((strEncryptedDate.Length != 10))
                return false;

            int intHour;
            if ((!Int32.TryParse(strEncryptedDate.Substring(0, 2), out intHour)))
                return false;

            var strDate = strEncryptedDate.Substring(2, 8);
            if ((!strDate.IsNumeric()))
                return false;

            DateTime dDate;
            try
            {
                dDate = new DateTime(Int32.Parse(strDate.Substring(4, 4)), Int32.Parse(strDate.Substring(2, 2)), Int32.Parse(strDate.Substring(0, 2)), intHour, 0, 0);
            }
            catch
            {
                return false;
            }

            var differenceToNow = DateTime.Now - dDate;
            if (differenceToNow.TotalMinutes > tokenExpirationMinutes)    // 120))
                return false;

            return true;
        }

        static public string UrlRemoteEncryptHourAndDate(string strHourAndDate = null)
        {
            if (strHourAndDate.IsNullOrEmpty())
                strHourAndDate = DateTime.Now.ToString("HHddMMyyy");

            if (string.IsNullOrEmpty(strHourAndDate)) return "*** error: empty string ***";
            if (strHourAndDate.Length != 10) return "*** error: invalid length ***";
            Int64 dummy;
            if (!Int64.TryParse(strHourAndDate, out dummy)) return "*** error: invalid value for hour + date ***";

            var strEncryptedHourAndDate = "";
            var reversal = false;
            for (var i = 0; i < strHourAndDate.Length; i++)
            {
                var ch = strHourAndDate[i] + 30;
                strEncryptedHourAndDate += ((char)(!reversal ? ch : ('A' + 'Z' - ch))).ToString();
                reversal = !reversal;
            }

            return strEncryptedHourAndDate;
        }

        #endregion


        private static string GetTokenContent(string content)
        {
            return string.Format("{0}~{1}", content, DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
        }

        public static string GenerateToken(string content)
        {
            return CryptoMd5.Encrypt(GetTokenContent(content));
        }
    }
}
