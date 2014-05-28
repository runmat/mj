using System;
using System.Configuration;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace GeneralTools.Log.Services
{
    /// <summary>
    /// NLog speziefische Implementierung für das Loggen der Page Visits
    /// </summary>
    public class PageVisitLogger
    {
        private readonly Logger _log;

        /// <summary>
        /// Logger für Page Visits
        /// TODO IoC Integration implementiern: Logger soll den nLog Logger über IoC erhalten
        /// </summary>
        public PageVisitLogger()
        {
            _log = LogManager.GetLogger("PageVisitLogger");
        }

        public void Log(int appID, int userID, int customerID, int kunnr, int portalType, string clientIp = null)
        {
            var logEventInfo = new LogEventInfo
                {
                    Level = LogLevel.Trace, TimeStamp = DateTime.Now
                };

            var connectionString = ConfigurationManager.AppSettings["Logs"];

            // Falls keine Verbindungsdaten vorhanden sind das Logging unterlassen
            if (string.IsNullOrEmpty(connectionString))
                return;

            logEventInfo.Properties["connectionString"] = connectionString;
            logEventInfo.Properties["commandText"] = "insert into PageVisit(AppID, UserID, CustomerID, KUNNR, PortalType) " +
                                                                 "values (@AppID, @UserID, @CustomerID, @KUNNR, @PortalType);";
            logEventInfo.Properties["AppID"] = appID;
            logEventInfo.Properties["UserID"] = userID;
            logEventInfo.Properties["CustomerID"] = customerID;
            logEventInfo.Properties["KUNNR"] = kunnr;
            logEventInfo.Properties["PortalType"] = portalType;


            logEventInfo.Parameters = new object[] {
                new DatabaseParameterInfo("@AppId", Layout.FromString("${event-context:item=AppID}")),
                new DatabaseParameterInfo("@UserID", Layout.FromString("${event-context:item=UserID}")),
                new DatabaseParameterInfo("@CustomerID", Layout.FromString("${event-context:item=CustomerID}")),
                new DatabaseParameterInfo("@KUNNR", Layout.FromString("${event-context:item=KUNNR}")),
                new DatabaseParameterInfo("@PortalType", Layout.FromString("${event-context:item=PortalType}"))
            };

            _log.Log(logEventInfo);
        }
    }
}
