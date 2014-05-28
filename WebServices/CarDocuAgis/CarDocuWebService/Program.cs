using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using CarDocuWebService.CarDocu;

namespace CarDocuWebService
{

    /// <summary>
    /// Wenn das HTTP Verkehr in Fiddler sichtbar werden soll, bitte das Zertifikat im Verzeichnis C:\Users\[USER]\Documents\Fiddler2 ablegen
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            CarDocuWebServicePortClient client = new CarDocuWebServicePortClient();

            // Zertifikat Objekt erstellen und dem client anhängen
            // Zertifikate aus dem Zertifikat-Tresor werden nicht angenommen 
            var certificate = new X509Certificate2(@"C:\tmp\TAMCDOE.p12", "76WBLQMK");
            client.ClientCredentials.ClientCertificate.Certificate = certificate;
            ServicePointManager.ServerCertificateValidationCallback = OnValidationCallback;

            client.Endpoint.Address = new EndpointAddress(@"https://tui-agis-services.audi.de/agis/adapter/ws");

            var getDriverInformationResponse = client.GetDriverInformation(new GetDriverInformation
                {
                    Date = new DateTime(2013, 12, 12), NumberPlate = "IN-FA 1000", SysUser = "axdfee"
                });

        }

        private static bool OnValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }
    }



}
