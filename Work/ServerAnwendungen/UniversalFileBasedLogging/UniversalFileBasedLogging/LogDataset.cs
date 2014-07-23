using System.Collections.Generic;

namespace UniversalFileBasedLogging
{
    /// <summary>
    /// Eine Sammlung von Logeinträgen
    /// </summary>
    public class LogDataset : List<LogCustomer>
    {
        private static readonly LogDataset LdSearch = new LogDataset();
        protected static LogCustomer LcSearch;
        protected static LogFile LfSearch;
        
        // Fügt der Liste neue Einträge hinzu, wenn sie nicht bereits existieren
        public void AddNoDoubles(LogCustomer item)
        {
            LcSearch = item;
            LogCustomer lcFound = this.Find(isCustomer);
            LcSearch = null;

            if (lcFound != null)
            {
                // Dateien des Kunden übertragen
                lcFound.AddRange(item);
                return;
            }

            this.Add(item);
        }

        public LogCustomer FindCustomer(string name, string archivname)
        {
            LcSearch = new LogCustomer(name, archivname, LdSearch);
            LogCustomer lcFound = this.Find(isCustomer);
            LdSearch.Clear();
            return lcFound;
            
        }

        // Prüft ob Kunde 2 Kunden gleich sind
        private static bool isCustomer(LogCustomer custom)
        {
            if (LcSearch.Name == custom.Name && LcSearch.Archivname == custom.Archivname)
            {
                return true;
            }
            return false;
        }

        // liefert eine Datei mit den angegebenen Parametern
        public LogFile FindFile(string fin, string kennzeichen, string titel)
        {
            LfSearch = new LogFile(fin, kennzeichen, titel, new LogCustomer("", "", LdSearch));
            foreach (LogCustomer custom in this)
            {
                return custom.Find(isFile);
            }
            return null;
        }

        private static bool isFile(LogFile file)
        {
            bool found = false;

            if (LfSearch.Kennzeichen != null)
            {
                if (file.Kennzeichen == LfSearch.Kennzeichen)
                {
                    found = true;
                }
                else { return false; }
            }

            if (LfSearch.FIN != null)
            {
                if (file.FIN == LfSearch.FIN)
                {
                    found = true;
                }
                else { return false; }
            }

            if (LfSearch.Titel != null)
            {
                if (file.Titel == LfSearch.Titel)
                {
                    found = true;
                }
                else { return false; }
            }

            return found;
        }
    }
}
