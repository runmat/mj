using System.Configuration;
using Mobile.Interfaces;
using Mobile.Resources;

namespace Mobile.Utils
{
    public class SettingsReader : ISettingsReader
    {
        /// <summary>
        /// Basisverzeichnis für Dokumente die zur mobile.de übermittelt werden
        /// </summary>
        public string DocumentFolder
        {
            get { return ConfigurationManager.AppSettings[ApplicationStrings.DocumentFolder]; }
        }

        /// <summary>
        /// SellerId für die Zuordnung der Inserate bei mobile.de
        /// </summary>
        public string SellerId
        {
            get { return ConfigurationManager.AppSettings[ApplicationStrings.ConfigSellerId]; }
        }

        /// <summary>
        /// Token für die Authentizierung der Kommunikation mit mobile.de
        /// </summary>
        public string Token
        {
            get { return ConfigurationManager.AppSettings[ApplicationStrings.ConfigAuthenticationToken]; }
        }

        /// <summary>
        /// Basis URL der mobile.de Anwendung
        /// </summary>
        public string MobiledeUrl
        {
            get { return ConfigurationManager.AppSettings[ApplicationStrings.MobiledeUrl]; }
        }

        /// <summary>
        /// In welchem SAP befinden wir uns?
        /// </summary>
        public bool SapProdSystem
        {
            get
            {
                var value = ConfigurationManager.AppSettings[ApplicationStrings.SapProdSytem];
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }

                return value.ToLower() == "true";
            }
        }
    }
}
