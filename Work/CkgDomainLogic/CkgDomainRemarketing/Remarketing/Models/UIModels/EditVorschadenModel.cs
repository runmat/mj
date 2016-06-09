using System;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Remarketing.Models
{
    public class EditVorschadenModel
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.SequenceNo)]
        public string LaufendeNr { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.DamageAmount)]
        public decimal? Schadensbetrag { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.DamageDate)]
        public DateTime? Schadensdatum { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.DescriptionBeschreibung)]
        public string Beschreibung { get; set; }

        public EditVorschadenModel()
        {        
        }

        public EditVorschadenModel(Schadensmeldung item)
        {
            FahrgestellNr = item.FahrgestellNr;
            Kennzeichen = item.Kennzeichen;
            LaufendeNr = item.LaufendeNr;
            Schadensbetrag = item.Schadensbetrag;
            Schadensdatum = item.Schadensdatum;
            Beschreibung = item.Beschreibung;
        }
    }
}
