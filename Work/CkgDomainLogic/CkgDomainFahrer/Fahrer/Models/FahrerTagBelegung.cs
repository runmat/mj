using System;

namespace CkgDomainLogic.Fahrer.Models
{
    public enum FahrerTagBelegungsTyp { Leer = 0, Verfuegbar = 1, NichtVerfuegbar = 2, Urlaub = 3, Krank = 4 };

    public class FahrerTagBelegung
    {
        public string FahrerID { get; set; }

        public DateTime Datum { get; set; }

        public FahrerTagBelegungsTyp BelegungsTyp { get; set; }

        public string Verfuegbarkeit { get { return BelegungsTyp.ToString("F"); } }

        public int FahrerAnzahl { get; set; }

        public string CssClass { get { return GetCssClass(BelegungsTyp); } }


        public FahrerTagBelegung()
        {
            FahrerAnzahl = 1;
        }

        public static string GetCssClass(FahrerTagBelegungsTyp belegung)
        {
            return string.Format("fahrer-belegung-{0}", belegung.ToString("F").ToLower());
        }
    }
}
