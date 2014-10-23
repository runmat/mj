using System.Web;
using GeneralTools.Services;

namespace WebTools.Services
{
    public class CryptoMd5
    {
        private const string PasswordForInternalUseOnly = "Dad@inTern!17onLy";


        #region Encryption

        public static string Encrypt(string stringToEncrypt)
        {
            return Encrypt(stringToEncrypt, PasswordForInternalUseOnly);
        }

        public static string Encrypt(string stringToEncrypt, string password)
        {
            return CryptoMd5Service.Encrypt(stringToEncrypt, password);
        }

        public static string EncryptToUrlEncoded(string stringToEncrypt)
        {
            return EncryptToUrlEncoded(stringToEncrypt, PasswordForInternalUseOnly);
        }

        public static string EncryptToUrlEncoded(string stringToEncrypt, string password)
        {
            return HttpUtility.UrlEncode(Encrypt(stringToEncrypt, password));
        }

        #endregion


        #region Decryption

        public static string DecryptFromUrlEncoded(string stringUrlEncodedToDecrypt)
        {
            return DecryptFromUrlEncoded(stringUrlEncodedToDecrypt, PasswordForInternalUseOnly);
        }

        public static string DecryptFromUrlEncoded(string stringUrlEncodedToDecrypt, string password)
        {
            return Decrypt(HttpUtility.UrlDecode(stringUrlEncodedToDecrypt), password);
        }

        public static string Decrypt(string stringUrlEncodedToDecrypt)
        {
            return Decrypt(stringUrlEncodedToDecrypt, PasswordForInternalUseOnly);
        }

        public static string Decrypt(string stringUrlEncodedToDecrypt, string password)
        {
            return CryptoMd5Service.Decrypt(stringUrlEncodedToDecrypt, password);
        }

        #endregion
    }
}
