using MySql.Data.MySqlClient;
using NLog;
using NLog.Common;
using NLog.Targets;

namespace RefImplBibl.Logging
{
    [Target("DatabaseLog")]
    public sealed class DatabaseLogTarget : TargetWithLayout
    {
        protected override void Write(AsyncLogEventInfo logEvent)
        {
            SaveToDatabase(logEvent.LogEvent);
        }

        protected override void Write(AsyncLogEventInfo[] logEvents)
        {
            foreach (AsyncLogEventInfo info in logEvents)
            {
                SaveToDatabase(info.LogEvent);
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {
            //string logMessage = this.Layout.Render(logEvent);
            SaveToDatabase(logEvent);
        }

        private void SaveToDatabase(LogEventInfo logInfo)
        {
            //Create the connection
            using (MySqlConnection conn = new MySqlConnection(logInfo.Properties["connectionString"].ToString()))
            {
                //Create the command
                using (MySqlCommand com = new MySqlCommand(logInfo.Properties["commandText"].ToString(), conn))
                {
                    foreach (DatabaseParameterInfo dbi in logInfo.Parameters)
                    {
                        //Add the parameter info, using Layout.Render() to get the actual value
                        com.Parameters.AddWithValue(dbi.Name, dbi.Layout.Render(logInfo));
                    }

                    //open the connection
                    com.Connection.Open();

                    //Execute the sql command
                    com.ExecuteNonQuery();
                }
            }
        }
    }
}

