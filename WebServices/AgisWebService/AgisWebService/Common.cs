using System.Security.Cryptography.X509Certificates;

namespace AgisWebService
{
    public class Common
    {
        public static string AgisUrl { get; set; }

        public static X509Certificate2 AgisCert { get; set; }

        public static string LogTable { get; set; }
    }
}