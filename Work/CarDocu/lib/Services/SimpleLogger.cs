using System;
using System.IO;
using System.Threading;
using GeneralTools.Models;

namespace CarDocu.Services
{
    public class SimpleLogger
    {
        readonly string _logFileName;
        public string LogFileName { get { return _logFileName; } }

        public SimpleLogger(string logFilePath)
        {
            _logFileName = Path.Combine(logFilePath, "ErrorLog.txt");
        }

        public void LogMessage(String pMessage)
        {
            var successful = false;

            for (var idx = 0; idx < 10; idx++)
            {
                try
                {
                    // Log message to default log file.
                    var str = new StreamWriter(_logFileName, true) { AutoFlush = true };

                    str.WriteLine(Environment.NewLine + Environment.NewLine + "Time: " + DateTime.Now + Environment.NewLine + "Message: " + pMessage);
                    str.Close();

                    successful = true;
                }
                catch (Exception)
                {
                }

                if (successful)     // Logging successful
                    break;

                Thread.Sleep(10);
            }
        }

        public void LogMessage(Exception exception)
        {
            var message = "";

            try
            {
                var stackContext = new StackContext();
                stackContext.Init(null);
                message += Environment.NewLine +
                            "Exception, Message: " + exception.Message + Environment.NewLine +
                            "Current StackTrace: " + Environment.NewLine + string.Join(Environment.NewLine + "   - ", stackContext.StackFrames);
            }
            catch (Exception)
            {
                message = "LogMessage.Error";
            }

            LogMessage("Exception: " + exception.Message + Environment.NewLine + "Details: " + message);
        }
    }
}
