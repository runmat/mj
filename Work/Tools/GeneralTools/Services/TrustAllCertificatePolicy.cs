using System.Net;

namespace GeneralTools.Services
{
    public class TrustAllCertificatePolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint sp,
         System.Security.Cryptography.X509Certificates.X509Certificate cert, WebRequest req, int problem)
        {
            return true;
        }
    }
}
