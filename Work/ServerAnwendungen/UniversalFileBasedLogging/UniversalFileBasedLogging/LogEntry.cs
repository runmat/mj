using System;

namespace UniversalFileBasedLogging
{
    /// <summary>
    /// Stellt einen Logeintrag dar.
    /// </summary>
    public class LogEntry
    {
        public LogEntry(string status, string entry, LogFile file)
        {
            Status = status;
            Entry = entry;
            LogTime = DateTime.Now.ToLongTimeString();
            LogDate = DateTime.Now.ToShortDateString();
            File = file;
            Customer = file.Customer;

            file.Add(this);
        }

        public string Status { get; set; }

        public string Entry { get; set; }

        public string LogTime { get; set; }

        public string LogDate { get; set; }

        public LogCustomer Customer { get; set; }

        public LogFile File { get; set; }

        public override string ToString()
        {
            return Entry;
        }
    }
}
