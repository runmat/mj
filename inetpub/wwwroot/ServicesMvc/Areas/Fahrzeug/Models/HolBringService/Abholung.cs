using System;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models.HolBringService
{
    public class Abholung 
    {
        [LocalizedDisplay(LocalizeConstants.CustomerHolBring)]
        [StringLength(50)]
        [Required]
        public string AbholungKunde { get; set; }

        [LocalizedDisplay(LocalizeConstants.StreetHouseNo)]
        [StringLength(50)]
        [Required]
        public string AbholungStrasseHausNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        [StringLength(5)]
        [Required]
        public string AbholungPlz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        [AddressPostcodeCityMapping("AbholungPlz", "Land")]
        [StringLength(30)]
        [Required]
        public string AbholungOrt { get; set; }

        public string Land { get { return "DE"; } }

        [LocalizedDisplay(LocalizeConstants.Contactperson)]
        [StringLength(50)]
        public string AbholungAnsprechpartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        [StringLength(30)]
        [Required]
        public string AbholungTel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? AbholungDatum { get; set; }

        public string AbholungUhrzeitStunden { get; set; }
        public string AbholungUhrzeitMinuten { get; set; }

        [StringLength(200)]
        [LocalizedDisplay(LocalizeConstants.AdditionalInformations)]
        public string AbholungHinweis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mobilitaetsfahrzeug)]
        public bool AbholungMobilitaetsfahrzeug { get; set; }

    }
}
