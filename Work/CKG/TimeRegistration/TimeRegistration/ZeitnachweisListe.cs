using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeRegistration
{
    public class ZeitnachweisListe : List<ZeitnachweisObj>
    {
        string _basispfad;
        string _bedienernummer;

        public string Bedienernummer { get { return _bedienernummer; } }
        public string Basispfad { get { return _basispfad; } }

        public void FillNachweise(string bedienernummer, string basispfad)
        { 
            _basispfad = basispfad;
            _bedienernummer = bedienernummer;

            if (DateTime.Today.Month == 1)
            {
                ZeitnachweisObj zw1 = new ZeitnachweisObj(bedienernummer, 12, DateTime.Today.Year - 1, basispfad);
                ZeitnachweisObj zw2 = new ZeitnachweisObj(bedienernummer, DateTime.Today.Month, DateTime.Today.Year, basispfad);

                if (zw1.FileExist) { Add(zw1); }
                if (zw2.FileExist) { Add(zw2); }                
            }
            else 
            {
                ZeitnachweisObj zw1 = new ZeitnachweisObj(bedienernummer, DateTime.Today.Month - 1, DateTime.Today.Year, basispfad);
                ZeitnachweisObj zw2 = new ZeitnachweisObj(bedienernummer, DateTime.Today.Month, DateTime.Today.Year, basispfad);

                if (zw1.FileExist) { Add(zw1); }
                if (zw2.FileExist) { Add(zw2); }    
            }
        }
    }
}
