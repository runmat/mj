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
        public string VkBuero { get; set; }

        [Display(Name = "Kundennummer")]
        public string Kunnr { get; set; }

        [Display(Name = "Kundenname")]
        public string Kunname { get; set; }

        [Display(Name = "Kunde")]
        public string Kunde
        {
            get { return Kunnr.TrimStart('0') + " " + Kunname; }
        }

        [Display(Name = "Referenz 1")]
        public string Referenz1 { get; set; }

        [Display(Name = "Referenz 2")]
        public string Referenz2 { get; set; }

        [Display(Name = "Zul.datum")]
        public DateTime ZulDat { get; set; }

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
                string erg = "";

                if (ZahlartEC == "X")
                {
                    erg = "EC";
                }
                if (ZahlartBar == "X")
                {
                    erg = "Bar";
                }
                if (ZahlartRE == "X")
                {
                    erg = "RE";
                }
                return erg;
            }
        }

        [Display(Name = "EC")]
        // ReSharper disable InconsistentNaming
        public string ZahlartEC { get; set; }
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// für JSON-Übergabe
        /// </summary>
        [Display(Name = "EC")]
        // ReSharper disable InconsistentNaming
        public bool ZahlartECBool { get; set; }
        // ReSharper restore InconsistentNaming

        [Display(Name = "Bar")]
        public string ZahlartBar { get; set; }

        /// <summary>
        /// für JSON-Übergabe
        /// </summary>
        [Display(Name = "Bar")]
        public bool ZahlartBarBool { get; set; }

        [Display(Name = "RE")]
        // ReSharper disable InconsistentNaming
        public string ZahlartRE { get; set; }
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// für JSON-Übergabe
        /// </summary>
        [Display(Name = "RE")]
        // ReSharper disable InconsistentNaming
        public bool ZahlartREBool { get; set; }
        // ReSharper restore InconsistentNaming

        [Display(Name = "Bemerkung")]
        public string Bemerkung { get; set; }

        [Display(Name = "Nachbearbeiten")]
        public string Nachbearbeiten { get; set; }

        /// <summary>
        /// für JSON-Übergabe
        /// </summary>
        [Display(Name = "Nachbearbeiten")]
        public bool NachbearbeitenBool { get; set; }

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
            this.Id = "";
            this.BlTyp = "";
            this.VkOrg = "";
            this.VkBuero = "";
            this.Kunnr = "";
            this.Kunname = "";
            this.Referenz1 = "";
            this.Referenz2 = "";
            this.ZulDat = DateTime.MinValue;
            this.Amt = "";
            this.Kennzeichen = "";
            this.Status = "";
            this.ZahlartEC = "";
            this.ZahlartBar = "";
            this.ZahlartRE = "";
            this.Positionen = new List<VorgangPosition>();
            this.IsDirty = false;
            this.IsShown = false;
        }

        public Vorgang(string id, string bltyp, string vkorg, string vkbuero, string kunnr, string kunname, string ref1, string ref2, DateTime zuldat,
            string amt, string kennzeichen, string status, string ec, string bar, string re)
        {
            this.Id = id;
            this.BlTyp = bltyp;
            this.VkOrg = vkorg;
            this.VkBuero = vkbuero;
            this.Kunnr = kunnr;
            this.Kunname = kunname;
            this.Referenz1 = ref1;
            this.Referenz2 = ref2;
            this.ZulDat = zuldat;
            this.Amt = amt;
            this.Kennzeichen = kennzeichen;
            this.Status = status;
            this.ZahlartEC = ec;
            this.ZahlartBar = bar;
            this.ZahlartRE = re;
            this.Positionen = new List<VorgangPosition>();
            this.IsDirty = false;
            this.IsShown = false;
        }
    }
}
