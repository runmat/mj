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
        public string AnlieferungKunde { get; set; }

        [LocalizedDisplay(LocalizeConstants.StreetHouseNo)]
        public string AnlieferungStrasseHausNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string AnlieferungPlz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        [AddressPostcodeCityMapping("AnlieferungPlz", "Land")]
        public string AnlieferungOrt { get; set; }

        public string Land { get { return "D"; } }

        [LocalizedDisplay(LocalizeConstants.Contactperson)]
        public string AnlieferungAnsprechpartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string AnlieferungTel { get; set; }
        
        public string AbholungAbUhrzeitStunden { get; set; }
        public string AbholungAbUhrzeitMinuten { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime AnlieferungDatum { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Time)]
        public string AnlieferungBisUhrzeitStunden { get; set; }
        [Required]
        public string AnlieferungBisUhrzeitMinuten { get; set; }

        [StringLength(200)]
        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string AnlieferungHinweis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mobilitaetsfahrzeug)]
        public bool AnlieferungMobilitaetsfahrzeug { get; set; }

    }
}
