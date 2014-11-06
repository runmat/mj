using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    public class CryptoMd5Service 
    {
        public static string Encrypt(string stringToEncrypt, string password)
        {
            if (stringToEncrypt.IsNullOrEmpty())
                return "";

            string stringEncrypted;
            try
            {
                var rd = new RijndaelManaged();
                var md5 = new MD5CryptoServiceProvider();
                var key = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

                md5.Clear();
                rd.Key = key;
                rd.GenerateIV();

                var iv = rd.IV;
                var ms = new MemoryStream();
                ms.Write(iv, 0, iv.Length);

                var cs = new CryptoStream(ms, rd.CreateEncryptor(), CryptoStreamMode.Write);
                var data = Encoding.UTF8.GetBytes(stringToEncrypt);

                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();

                var encryptedData = ms.ToArray();
                stringEncrypted = Convert.ToBase64String(encryptedData);
                cs.Close();
                rd.Clear();
            }
            catch
            {
                stringEncrypted = "";
            }

            return stringEncrypted;
        }

        public static string Decrypt(string stringToDecrypt, string password)
        {
            if (stringToDecrypt.IsNullOrEmpty())
                return "";
            
            string stringDecrypted;
            try
            {
                var rd = new RijndaelManaged();
                var md5 = new MD5CryptoServiceProvider();
                const int rijndaelIvLength = 16;
                var key = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

                md5.Clear();

                var encryptedData = Convert.FromBase64String(stringToDecrypt);
                var ms = new MemoryStream(encryptedData);
                var iv = new byte[rijndaelIvLength];
                ms.Read(iv, 0, rijndaelIvLength);
                rd.IV = iv;
                rd.Key = key;

                var cs = new CryptoStream(ms, rd.CreateDecryptor(), CryptoStreamMode.Read);
                var data = new byte[ms.Length - rijndaelIvLength];
                var i = cs.Read(data, 0, data.Length);

                stringDecrypted = Encoding.UTF8.GetString(data, 0, i);
                cs.Close();
                rd.Clear();
            }
            catch
            {
                stringDecrypted = "";
            }

            return stringDecrypted;
        }
    }
}
