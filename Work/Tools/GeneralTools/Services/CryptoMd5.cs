namespace GeneralTools.Services
{
    public class CryptoMd5
    {
        protected const string PasswordForInternalUseOnly = "Dad@inTern!17onLy";


        #region Encryption

        public static string Encrypt(string stringToEncrypt)
        {
            return Encrypt(stringToEncrypt, PasswordForInternalUseOnly);
        }

        public static string Encrypt(string stringToEncrypt, string password)
        {
            return CryptoMd5Service.Encrypt(stringToEncrypt, password);
        }

        #endregion


        #region Decryption

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
