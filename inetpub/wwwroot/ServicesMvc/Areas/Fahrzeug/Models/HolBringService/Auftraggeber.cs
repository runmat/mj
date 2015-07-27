using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models.HolBringService
{
    public class Auftraggeber 
    {
        [LocalizedDisplay(LocalizeConstants.Auftragsersteller)]
        public string Auftragsersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        [StringLength(30)]
        public string AuftragerstellerTel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Factory)]
        public string Betrieb { get; set; }

        [LocalizedDisplay(LocalizeConstants.Repco)]
        [StringLength(30)]
        [Required]
        public string Repco { get; set; }

        [LocalizedDisplay(LocalizeConstants.Contactperson)]
        [StringLength(50)]
        [Required]
        public string Ansprechpartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        [StringLength(30)]
        public string AnsprechpartnerTel { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerHolBring)]
        [StringLength(50)]
        [Required]
        public string Kunde { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        [StringLength(30)]
        public string KundeTel { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [StringLength(10)]
        public string Kennnzeichen { get; set; }
    
        [LocalizedDisplay(LocalizeConstants.VehicleSpecies)]
        [Required]
        public int FahrzeugartId { get; set; }
        public string Fahrzeugart { get; set; }

        public string BetriebName { get; set; }
        public string BetriebStrasse { get; set; }
        public string BetriebHausNr { get; set; }
        public string BetriebPLZ { get; set; }
        public string BetriebOrt { get; set; }

    }
}
