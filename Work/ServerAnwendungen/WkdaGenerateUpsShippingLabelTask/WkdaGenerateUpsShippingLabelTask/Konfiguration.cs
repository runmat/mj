using System.Configuration;

namespace WkdaGenerateUpsShippingLabelTask
{
    /// <summary>
    /// globale Konfiguration
    /// </summary>
    internal static class Konfiguration
    {
        public static bool pauseAfterCompletion { get { return (ConfigurationManager.AppSettings["PauseAfterCompletion"].ToUpper() == "TRUE"); } }

        public static string WkdaKunnr { get { return ConfigurationManager.AppSettings["WkdaKunnr"]; } }
        public static string WkdaLabelAblagePfad { get { return ConfigurationManager.AppSettings["WkdaLabelAblagePfad"]; } }
        public static string WkdaLabelDateiname { get { return ConfigurationManager.AppSettings["WkdaLabelDateiname"]; } }

        public static string UpsShippingWebServiceUrl { get { return ConfigurationManager.AppSettings["UpsShippingWebServiceUrl"]; } }
        public static string UpsShippingWebServiceUsername { get { return ConfigurationManager.AppSettings["UpsShippingWebServiceUsername"]; } }
        public static string UpsShippingWebServicePassword { get { return ConfigurationManager.AppSettings["UpsShippingWebServicePassword"]; } }
        public static string UpsShippingWebServiceAccessKey { get { return ConfigurationManager.AppSettings["UpsShippingWebServiceAccessKey"]; } }
    }
}
