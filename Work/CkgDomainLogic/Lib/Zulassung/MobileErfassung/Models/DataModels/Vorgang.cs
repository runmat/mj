using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// ZLD-Vorgang/-Kopfsatz mit den für die mobile Erfassung relevanten Daten
    /// </summary>
    public class Vorgang
    {
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Vorgangsart")]
        public string BlTyp { get; set; }

        [Display(Name = "VkOrg")]
        public string VkOrg { get; set; }

        [Display(Name = "VkBur")]
        public string VkBur { get; set; }

        [Display(Name = "Kundennummer")]
        public string Kunnr { get; set; }

        [Display(Name = "Kundenname")]
        public string Kunname { get; set; }

        [Display(Name = "Kunde")]
        public string Kunde
        {
            get { return Kunnr + " " + Kunname; }
        }

        [Display(Name = "Referenz 1")]
        public string Referenz1 { get; set; }

        [Display(Name = "Referenz 2")]
        public string Referenz2 { get; set; }

        [Display(Name = "Zul.datum")]
        public DateTime? ZulDat { get; set; }

        /// <summary>
        /// für JSON-Übergabe
        /// </summary>
        [Display(Name = "Zul.datum")]
        public string ZulDatText { get; set; }

        /// <summary>
        /// für JSON-Übergabe: extra Feld für änderbaren "ZulDat"-Wert, damit Info über das urspr. ZulDat erhalten bleibt
        /// </summary>
        [Display(Name = "Zul.datum")]
        public string ZulDatTextEdit { get; set; }

        [Display(Name = "Amt")]
        public string Amt { get; set; }

        /// <summary>
        /// für JSON-Übergabe: extra Feld für änderbaren "Amt"-Wert, damit Info über das urspr. Amt erhalten bleibt
        /// </summary>
        [Display(Name = "Amt")]
        public string AmtEdit { get; set; }

        [Display(Name = "Kennzeichen")]
        public string Kennzeichen { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        /// <summary>
        /// für JSON-Übergabe
        /// </summary>
        [Display(Name = "Durchgeführt")]
        public bool StatusDurchgefuehrt { get; set; }

        /// <summary>
        /// für JSON-Übergabe
        /// </summary>
        [Display(Name = "Fehlgeschlagen")]
        public bool StatusFehlgeschlagen { get; set; }

        [Display(Name = "Zahlart")]
        public string Zahlart
        {
            get
            {
                if (ZahlartEC)
                    return "EC";

                if (ZahlartBar)
                    return "Bar";

                if (ZahlartRE)
                    return "RE";

                return "";
            }
        }

        [Display(Name = "EC")]
        public bool ZahlartEC { get; set; }

        [Display(Name = "Bar")]
        public bool ZahlartBar { get; set; }

        [Display(Name = "RE")]
        public bool ZahlartRE { get; set; }

        [Display(Name = "Infotext")]
        public string Infotext { get; set; }

        [Display(Name = "Nachbearbeiten")]
        public bool Nachbearbeiten { get; set; }

        [Display(Name = "Wunschkennzeichen")]
        public bool Wunschkennzeichen { get; set; }

        [Display(Name = "Reserviert")]
        public bool Reserviert { get; set; }

        [Display(Name = "R/W")]
        public string RWKennzeichen { get { return (Reserviert ? "R" : (Wunschkennzeichen ? "W" : "")); } }

        [Display(Name = "Saisonkennzeichen")]
        public bool Saisonkennzeichen { get; set; }

        [Display(Name = "Saison von")]
        public string SaisonVon { get; set; }

        [Display(Name = "Saison bis")]
        public string SaisonBis { get; set; }

        [Display(Name = "Saison")]
        public string Saison { get { return (Saisonkennzeichen ? string.Format("{0}-{1}", SaisonVon, SaisonBis) : ""); } }

        [Display(Name = "Nur ein Kennzeichen")]
        public bool NurEinKennzeichen { get; set; }

        [Display(Name = "Kennz.- Größe")]
        public string KennzeichenGroesse { get; set; }

        [Display(Name = "Kennz.- Anzahl")]
        public string KennzeichenAnzahl { get; set; }

        [Display(Name = "Bemerkung")]
        public string Bemerkung { get; set; }

        public string TelefonNrVorwahl { get; set; }

        public string TelefonNrDurchwahl { get; set; }

        [Display(Name = "Telefon-Nr.")]
        public string TelefonNr { get { return String.Format("{0} {1}", TelefonNrVorwahl, TelefonNrDurchwahl); } }

        public string VorerfasserUser { get; set; }

        public string VorerfasserAnrede { get; set; }

        public string VorerfasserName1 { get; set; }

        public string VorerfasserName2 { get; set; }

        [Display(Name = "Erfasser")]
        public string Vorerfasser
        {
            get
            {
                if (String.IsNullOrEmpty(VorerfasserName1))
                    return VorerfasserUser;

                return String.Format("{0}{1}{2}",
                    (String.IsNullOrEmpty(VorerfasserAnrede) ? "" : VorerfasserAnrede + " "),
                    VorerfasserName1,
                    (String.IsNullOrEmpty(VorerfasserName2) ? "" : " " + VorerfasserName2)
                );
            }
        }

        [Display(Name = "Durchführendes VkBur")]
        public string DurchfVkBur { get; set; }

        [Display(Name = "Versandzul. von")]
        public string VersandzulVkBur { get; set; }

        [Display(Name = "Positionen")]
        public List<VorgangPosition> Positionen { get; set; }

        [Display(Name = "Is dirty")]
        public bool IsDirty { get; set; }

        /// <summary>
        /// für UI/Javascript
        /// </summary>
        [Display(Name = "Is shown")]
        public bool IsShown { get; set; }

        public Vorgang()
        {
            Positionen = new List<VorgangPosition>();
        }
    }
}
