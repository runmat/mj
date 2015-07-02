using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesMvc.Areas.Fahrzeug.Models.HolBringService
{
    /// <summary>
    /// Wird für den Bapi-Aufruf benötigt, um die Formulardaten zu übermitteln
    /// </summary>
    public class BapiParameterSet
    {
        public string Auftragsersteller { get; set; }
        public string AuftragerstellerTel { get; set; }
        public string BetriebStraße { get; set; }
        public string BetriebHausNr { get; set; }
        public string BetriebPLZ { get; set; }
        public string BetriebOrt { get; set; }
        public string Repco { get; set; }
        public string Ansprechpartner { get; set; }
        public string AnsprechpartnerTel { get; set; }
        public string KundeTel { get; set; }
        public string Kennnzeichen { get; set; }
        public string Fahrzeugart { get; set; }

        public string AbholungKunde { get; set; }
        public string AbholungStrasseHausNr { get; set; }
        public string AbholungPlz { get; set; }
        public string AbholungOrt { get; set; }
        public string AbholungAnsprechpartner { get; set; }
        public string AbholungTel { get; set; }
        public string AbholungDateTime { get; set; }
        public string AbholungMobilitaetsfahrzeug { get; set; }
        public string AbholungHinweis { get; set; }

        public string AnlieferungKunde { get; set; }
        public string AnlieferungStrasseHausNr { get; set; }
        public string AnlieferungPlz { get; set; }
        public string AnlieferungOrt { get; set; }
        public string AnlieferungAnsprechpartner { get; set; }
        public string AnlieferungTel { get; set; }
        public string AnlieferungAbholungAbDt { get; set; }
        public string AnlieferungAnlieferungBisDt { get; set; }
        public string AnlieferungMobilitaetsfahrzeug { get; set; }
        public string AnlieferungHinweis { get; set; }
    }
}