using System.IO;
using System.Globalization;

namespace TimeRegistration
{
    public class ZeitnachweisObj
    {
        string _pfad;
        string _dateiname;
        int _monat; 
        int _jahr; 
        string _basispfad;
        string _bedienernummer;
        bool _fileexist;
        string _monatsbezeichnung;
       
        #region Properties

        public string Pfad
        {
            get { return _pfad; }
        }

        public int Monat { 
            get { return _monat; } 
        }

        public string MonatsBezeichnung
        {
            get { return _monatsbezeichnung; }
        }

        public int Jahr { 
            get { return _jahr; } 
        }

        public string Bedienernummer 
        { 
            get { return _bedienernummer; } 
        }

        public string Basispfad 
        {
            get { return _basispfad; } 
        }

        public string Dateiname 
        {
            get { return _dateiname; } 
        }

        public bool FileExist
        {
            get { return _fileexist; }
        }
               
#endregion

        /// <summary>
        /// Zeitnachweis-Datei Objekt
        /// </summary>
        /// <param name="bedienernummer"></param>
        /// <param name="monat"></param>
        /// <param name="jahr"></param>
        /// <param name="basispfad"></param>  
        public ZeitnachweisObj(string bedienernummer, int monat, int jahr,string basispfad) {

            _bedienernummer = bedienernummer;
            _monat = monat;
            _jahr = jahr;
            _basispfad = basispfad;
            
            _monatsbezeichnung = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monat);
            _dateiname = bedienernummer + "_" + Monat.ToString().PadLeft(2,'0') + Jahr.ToString().Remove(0,2) + ".PDF";
            _pfad = basispfad + _dateiname;           

            try
            {
                FileInfo fi = new FileInfo(_pfad);
                _fileexist = fi.Exists;
            }
            catch (IOException)
            { 
                
            }
        }
    }
}
