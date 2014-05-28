using System.Configuration;
using AutoAct.Interfaces;
using AutoAct.Resources;

namespace AutoAct.Utils
{
    public class SettingsReader : ISettingsReader
    {
        /// <summary>
        /// Basisverzeichnis für Dokumente die zur autoAct übermittelt werden
        /// </summary>
        public string RootFoleder
        {
            get { return ConfigurationManager.AppSettings[ApplicationStrings.DocumentFolder]; }
        }

        /// <summary>
        /// Anmeldename für die AutoAct REST API, wie in app.config eingetragen
        /// </summary>
        public string Logon
        {
            get { return ConfigurationManager.AppSettings[ApplicationStrings.ConfigAuthenticationLogon]; }
        }

        /// <summary>
        /// Passwort für die AutoAct REST API, wie in der app.config eingetragen
        /// </summary>
        public string Password
        {
            get { return ConfigurationManager.AppSettings[ApplicationStrings.ConfigAuthenticationPassword]; }
        }
    }
}
