using System;

namespace AppZulassungsdienst.lib.Models
{
    public class ZLDKopfdaten
    {
        public string SapId { get; set; }

        public bool IsNewVorgang { get { return (String.IsNullOrEmpty(SapId) || SapId == "999"); } }

        /// <summary>
        /// Web-Vorgangsart: V = Vorerfassung / K = Kompletterfassung
        /// </summary>
        public string Vorgang { get; set; }

        public string AuftragsNr { get; set; }

        public string VkOrg { get; set; }

        public string VkBur { get; set; }
        
        public DateTime? Vorerfassungsdatum { get; set; }

        public string Vorerfassungszeit { get; set; }

        public string Vorerfasser { get; set; }

        public DateTime? Erfassungsdatum { get; set; }

        public string Erfasser { get; set; }

        public string StatusVersandzulassung { get; set; }

        public string Belegart { get; set; }

        public string VersandzulassungBearbeitungsstatus { get; set; }

        public string VersandzulassungDurchfuehrendesVkBur { get; set; }

        public DateTime? VersandzulassungErledigtDatum { get; set; }

        public string Barcode { get; set; }

        public string KundenNr { get; set; }

        public string Referenz1 { get; set; }

        public string Referenz2 { get; set; }

        public string Landkreis { get; set; }

        public string KreisBezeichnung { get; set; }

        public bool? Wunschkennzeichen { get; set; }

        public bool? KennzeichenReservieren { get; set; }

        public string ReserviertesKennzeichen { get; set; }

        public DateTime? Zulassungsdatum { get; set; }

        public string Kennzeichen { get; set; }

        public string Kennzeichenform { get; set; }

        public string AnzahlKennzeichen { get; set; }

        public bool? NurEinKennzeichen { get; set; }

        public string Bemerkung { get; set; }

        public bool? Zahlart_EC { get; set; }

        public bool? Zahlart_Bar { get; set; }

        public bool? Zahlart_Rechnung { get; set; }

        public string FrachtbriefNrHin { get; set; }

        public string FrachtbriefNrZurueck { get; set; }

        public string LieferantenNr { get; set; }

        public bool? BarzahlungKunde { get; set; }

        public string Loeschkennzeichen { get; set; }

        public string Kopfstatus { get; set; }

        public string Fehlertext { get; set; }

        public bool? PraegelisteErstellt { get; set; }

        public bool? Flieger { get; set; }

        public string Bearbeitungsstatus { get; set; }

        public bool? Nachbearbeiten { get; set; }

        public string MobilUser { get; set; }

        public string EvbNr { get; set; }

        public string Infotext { get; set; }

        public string LangtextNr { get; set; }

        /// <summary>
        /// B = bearbeitet, O = OK, ...
        /// </summary>
        public string WebBearbeitungsStatus { get; set; }
    }
}