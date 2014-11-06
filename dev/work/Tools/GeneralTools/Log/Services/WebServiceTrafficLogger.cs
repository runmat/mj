using System;
using System.Configuration;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace GeneralTools.Log.Services
{
    /// <summary>
    /// NLog spezifische Implementierung für das Loggen des In-/Output von Webservices
    /// </summary>
    public class WebServiceTrafficLogger
    {
        private readonly Logger _log;

        /// <summary>
        /// Logger für In-/Output von Webservices
        /// </summary>
        public WebServiceTrafficLogger()
        {
            _log = LogManager.GetLogger("WebServiceTrafficLogger");
        }

        public void Log(string typ, string daten, string destinationSqlTable)
        {
            var logEventInfo = new LogEventInfo
                {
                    Level = LogLevel.Trace, TimeStamp = DateTime.Now
                };

            var connectionString = ConfigurationManager.AppSettings["Logs"];

            // Falls keine Verbindungsdaten vorhanden sind das Logging unterlassen
            if (String.IsNullOrEmpty(connectionString))
                return;

            if (String.IsNullOrEmpty(destinationSqlTable))
                return;

            logEventInfo.Properties["connectionString"] = connectionString;
            logEventInfo.Properties["commandText"] = "insert into " + destinationSqlTable + "(Type, AllXml) " +
                                                                 "values (@Type, @AllXml);";
            logEventInfo.Properties["Type"] = typ;
            logEventInfo.Properties["AllXml"] = daten;


            logEventInfo.Parameters = new object[] {
                new DatabaseParameterInfo("@Type", Layout.FromString("${event-context:item=Type}")),
                new DatabaseParameterInfo("@AllXml", Layout.FromString("${event-context:item=AllXml}"))
            };

            _log.Log(logEventInfo);
        }
    }
}
