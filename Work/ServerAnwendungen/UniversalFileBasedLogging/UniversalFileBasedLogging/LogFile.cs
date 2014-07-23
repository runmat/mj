using System.Collections.Generic;

namespace UniversalFileBasedLogging
{
    /// <summary>
    /// Stellt eine Datei dar für die Einträge geloggt werden.
    /// </summary>
    public class LogFile : List<LogEntry>
    {
        public LogFile(string kennzeichen, string fin, string titel, LogCustomer customer)
        {
            Kennzeichen = kennzeichen;
            FIN = fin;
            Customer = customer;

            Trys = 0;
            Filename = "";
            NameEasy = "";
            strTyp = "";
            Finished = false;
            Titel = titel;

            customer.Add(this);
        }
        /// <summary>
        /// Dateiname
        /// </summary>
        public string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// Name der Datei im EasyArchiv
        /// </summary>
        public string NameEasy
        {
            get;
            set;
        }

        /// <summary>
        /// Übergeordneter Kunde
        /// </summary>
        public LogCustomer Customer
        {
            get;
            set;
        }

        /// <summary>
        /// Versuchsnummer
        /// </summary>
        public int Trys
        {
            get;
            set;
        }

        /// <summary>
        /// Kennzeichen
        /// </summary>
        public string Kennzeichen
        {
            get;
            set;
        }

        /// <summary>
        /// Fahrgestellnummer
        /// </summary>
        public string FIN
        {
            get;
            set;
        }
		
        /// <summary>
        /// strTyp der im Dateinamen mit verschlüsselt wird
        /// </summary>
        public string strTyp
        {
            get;
            set;
        }
        /// <summary>
        /// Datei übertragung abgeschlossen
        /// </summary>
        /// <remarks>Wenn Flag gesetzt kann dieser Eintrag beim Parsen übersprungen werden</remarks>
        public bool Finished
        {
            get;
            set;
        }

        /// <summary>
        /// Titel des Files z.B. COC,ZB2,ZB1,...
        /// </summary>
        public string Titel
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Filename;
        }

    }
}
