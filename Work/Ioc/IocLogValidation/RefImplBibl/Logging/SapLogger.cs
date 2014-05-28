using System;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace RefImplBibl.Logging
{
    public class SapLogger
    {
        private readonly Logger _log = null;

        public SapLogger()
        {
            //ConfigurationItemFactory.Default.Targets.RegisterDefinition("DatabaseLog", typeof(DatabaseLogTarget));
            _log = LogManager.GetLogger("SapLogger");
        }

        /// <summary>
        /// Attaches the database connection information and parameter names and layouts
        /// to the outgoing LogEventInfo object. The custom database target uses
        /// this to log the data.
        /// </summary>
        /// <param name="anmeldeName"></param>
        /// <param name="bapi"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public void Log(string anmeldeName, string bapi, string parameter)
        {
            LogEventInfo logEventInfo = new LogEventInfo();
            logEventInfo.Level = LogLevel.Trace;
            logEventInfo.TimeStamp = DateTime.Now;
            logEventInfo.Properties["connectionString"] = "server=vms074;User Id=LogUser;password=seE@Anemone;Persist Security Info=True;database=elmah";
            logEventInfo.Properties["commandText"] = "insert into SapBapi(anmeldename, bapi, params) values (@anmeldename, @bapi, @params);";
            logEventInfo.Properties["anmeldename"] = anmeldeName;
            logEventInfo.Properties["bapi"] = bapi;
            logEventInfo.Properties["params"] = parameter;

            logEventInfo.Parameters = new object[] {
                new DatabaseParameterInfo("@anmeldename", Layout.FromString("${event-context:item=anmeldename}")), 
                new DatabaseParameterInfo("@bapi", Layout.FromString("${event-context:item=bapi}")),
                new DatabaseParameterInfo("@params", Layout.FromString("${event-context:item=params}"))
            };

            _log.Log(logEventInfo);
        }
    }
}
