using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfTools4.Common
{
    public class AppCommandlineArgs
    {
        private static AppCommandlineArgs _instance;
        public static AppCommandlineArgs Instance
        {
            get { return (_instance ?? (_instance = new AppCommandlineArgs())); }
        }

        private readonly Dictionary<string, string> _commandlineArgDict;

        public AppCommandlineArgs()
        {
            var commandlineArgs = Environment.GetCommandLineArgs();

            _commandlineArgDict = new Dictionary<string, string>();
            commandlineArgs.ToList().ForEach(e => { if (e.Contains("=")) _commandlineArgDict.Add(e.Split('=')[0], e.Split('=')[1]); });
        }

        public string this[string key]
        {
            get
            {
                string value;
                return _commandlineArgDict.TryGetValue(key, out value) ? value : "";
            }
        }

        public Int64 GetInt64(string key, int defaultValue = -1)
        {
            Int64 value;
            if (Int64.TryParse(this[key], out value))
                return value;

            return defaultValue;
        }
    }
}
