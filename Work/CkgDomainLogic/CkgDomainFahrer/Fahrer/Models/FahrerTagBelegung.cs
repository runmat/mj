using System;

namespace CkgDomainLogic.Fahrer.Models
{
    public enum FahrerTagBelegungsTyp { Leer = 0, Verfuegbar = 1, EingeschraenktVerfuegbar = 2, NichtVerfuegbar = 3, Urlaub = 4, Krank = 5 };

    public class FahrerTagBelegung
    {
        public string FahrerID { get; set; }

        public DateTime Datum { get; set; }

        public FahrerTagBelegungsTyp BelegungsTyp { get; set; }

        public string Verfuegbarkeit { get { return BelegungsTyp.ToString("F"); } }

        public int FahrerAnzahl { get; set; }

        public string Kommentar { get; set; }

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
