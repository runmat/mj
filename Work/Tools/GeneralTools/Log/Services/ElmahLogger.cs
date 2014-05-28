using System;
using System.Configuration;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace GeneralTools.Log.Services
{
    public class ElmahLogger
    {
        private readonly Logger _log;

        public ElmahLogger()
        {
            _log = LogManager.GetLogger("ElmahLogger");
        }

        public void Log(string application, string host, string type, string source, string message, string user, string allXml, int statusCode, DateTime timeUtc)
        {
            LogEventInfo logEventInfo = new LogEventInfo();
            logEventInfo.Level = LogLevel.Error;
            logEventInfo.TimeStamp = DateTime.Now;

            var connectionString = ConfigurationManager.AppSettings["Logs"];

            // Falls keine Verbindungsdaten vorhanden sind das Logging unterlassen
            if (string.IsNullOrEmpty(connectionString))
            {
                return;
            }

            logEventInfo.Properties["connectionString"] = connectionString;
            logEventInfo.Properties["commandText"] = "INSERT INTO elmah_error (ErrorId,  Application,  Host,  Type,  Source,  Message,  User,  AllXml,  StatusCode,  TimeUtc) " +
                                                                     "VALUES (@ErrorId, @Application, @Host, @Type, @Source, @Message, @User, @AllXml, @StatusCode, @TimeUtc);";

            logEventInfo.Properties["ErrorId"] = Guid.NewGuid();
            logEventInfo.Properties["Application"] = string.IsNullOrEmpty(application) ? "/" : application;
            logEventInfo.Properties["Host"] = host;
            logEventInfo.Properties["Type"] = type;
            logEventInfo.Properties["Source"] = source;
            logEventInfo.Message = message;
            logEventInfo.Properties["User"] = user;
            logEventInfo.Properties["AllXml"] = allXml;
            logEventInfo.Properties["StatusCode"] = statusCode;
            logEventInfo.Properties["TimeUtc"] = timeUtc.ToString("yyyy-MM-dd HH:mm:ss");

            logEventInfo.Parameters = new object[] {
                new DatabaseParameterInfo("@ErrorId", Layout.FromString("${event-context:item=ErrorId}")), 
                new DatabaseParameterInfo("@Application", Layout.FromString("${event-context:item=Application}")),
                new DatabaseParameterInfo("@Host", Layout.FromString("${event-context:item=Host}")),
                new DatabaseParameterInfo("@Type", Layout.FromString("${event-context:item=Type}")),
                new DatabaseParameterInfo("@Source", Layout.FromString("${event-context:item=Source}")),
                new DatabaseParameterInfo("@Message", Layout.FromString("${Message}")),
                new DatabaseParameterInfo("@User", Layout.FromString("${event-context:item=User}")),
                new DatabaseParameterInfo("@AllXml", Layout.FromString("${event-context:item=AllXml}")),
                new DatabaseParameterInfo("@StatusCode", Layout.FromString("${event-context:item=StatusCode}")),
                new DatabaseParameterInfo("@TimeUtc", Layout.FromString("${event-context:item=TimeUtc}"))
            };

            _log.Log(logEventInfo);
        }
    }
}
