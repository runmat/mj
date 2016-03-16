using System.Web;
using GeneralTools.Services;

namespace WebTools.Services
{
    public class CryptoMd5Web : CryptoMd5
    {
        #region Encryption

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

        #endregion
    }
}