using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models.HolBringService
{
    public class Anlieferung  
    {
        [LocalizedDisplay(LocalizeConstants.Customer)]
        [StringLength(50)]
        public string AnlieferungKunde { get; set; }

        [LocalizedDisplay(LocalizeConstants.StreetHouseNo)]
        [StringLength(50)]
        public string AnlieferungStrasseHausNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        [StringLength(5)]
        public string AnlieferungPlz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        [AddressPostcodeCityMapping("AnlieferungPlz", "Land")]
        [StringLength(30)]
        public string AnlieferungOrt { get; set; }
        
        public string Land { get { return "DE"; } }

        [LocalizedDisplay(LocalizeConstants.Contactperson)]
        [StringLength(50)]
        public string AnlieferungAnsprechpartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        [StringLength(30)]
        public string AnlieferungTel { get; set; }
        
        public string AbholungAbUhrzeitStunden { get; set; }
        public string AbholungAbUhrzeitMinuten { get; set; }

        // [Required]
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? AnlieferungDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string AnlieferungBisUhrzeitStunden { get; set; }
        public string AnlieferungBisUhrzeitMinuten { get; set; }

        [StringLength(200)]
        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string AnlieferungHinweis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mobilitaetsfahrzeug)]
        public bool AnlieferungMobilitaetsfahrzeug { get; set; }

    }
}
