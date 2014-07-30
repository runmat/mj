using System.Collections.Generic;

namespace UniversalFileBasedLogging
{
    /// <summary>
    /// Stellt einen Kunden und seine Einträge im Log dar.
    /// </summary>
    public class LogCustomer : List<LogFile>
    {
        private static LogFile lfSearch;

        public LogCustomer(string name, string archivname,LogDataset dataset)
        {
            Name = name;
            Archivname = archivname;

            dataset.Add(this);
        }

        public string Name
        {
            get;
            set;
        }

        public string Archivname
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Name;
        }

        public void AddNoDoubles(LogFile item) 
        {
            lfSearch = item;
            LogFile lfFound = this.Find(isFile);

            if (lfFound == null)
            {
                this.Add(item);
            }
        }

        private static bool isFile(LogFile file)
        {
            bool found = false;

            if(lfSearch.Kennzeichen != null)
            {
                if(file.Kennzeichen == lfSearch.Kennzeichen)
                {
                    found=true;
                }
                else { return false; }
            }

            if(lfSearch.FIN != null)
            {
                if (file.FIN == lfSearch.FIN)
                {
                    found = true;
                }
                else { return false; }
            }

            if(lfSearch.Titel != null)
            {
                if(file.Titel == lfSearch.Titel)
                {
                    found=true;
                }
                else { return false; }
            }

            return found;
        }
    }
}
