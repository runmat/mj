using AutoAct.Resources;
using SapORM.Models;

namespace AutoAct.Bapi
{
    public class Kunde
    {
        public Kunde(Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB quelle)
        {
            Nummer = quelle.KUNNR;
            Adresse = string.Format(ApplicationStrings.Adresse, quelle.STRAS, quelle.LAND1, quelle.PSTLZ, quelle.ORT01);
            AnzahlDerBilder = int.Parse(quelle.INTNR);
            Anmeldename = quelle.NAME1;
            Passwort = quelle.POS_TEXT;
        }

        /// <summary>
        /// Constructor nur für Testzwecke
        /// </summary>
        public Kunde()
        {
        }

        public string Nummer { get; set; }
        public string Adresse { get; set; }
        public int AnzahlDerBilder { get; set; }
        public string Anmeldename { get; set; }
        public string Passwort { get; set; }
    }
}
