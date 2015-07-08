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
        public string BetriebName { get; set; }
        public string BetriebStrasse { get; set; }
        public string BetriebHausNr { get; set; }
        public string BetriebPLZ { get; set; }
        public string BetriebOrt { get; set; }

        public string AuftragerstellerTel { get; set; }
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
        public DateTime? AbholungDateTime { get; set; }
        public string AbholungMobilitaetsfahrzeug { get; set; }
        public string AbholungHinweis { get; set; }

        public string AnlieferungKunde { get; set; }
        public string AnlieferungStrasseHausNr { get; set; }
        public string AnlieferungPlz { get; set; }
        public string AnlieferungOrt { get; set; }
        public string AnlieferungAnsprechpartner { get; set; }
        public string AnlieferungTel { get; set; }
        public DateTime? AnlieferungAbholungAbDt { get; set; }
        public DateTime? AnlieferungAnlieferungBisDt { get; set; }
        public string AnlieferungMobilitaetsfahrzeug { get; set; }
        public string AnlieferungHinweis { get; set; }

        public string Return { get; set; }
    }
}