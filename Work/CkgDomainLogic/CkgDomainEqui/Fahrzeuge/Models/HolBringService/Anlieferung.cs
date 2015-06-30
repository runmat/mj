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

        public string AnlieferungPlz { get; set; }
        public string AnlieferungOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Contactperson)]
        public string AnlieferungAnsprechpartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string AnlieferungTel { get; set; }

        public string AbholungAbUhrzeitStunden { get; set; }
        public string AbholungAbUhrzeitMinuten { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime AnlieferungDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string AnlieferungBisUhrzeitStunden { get; set; }
        public string AnlieferungBisUhrzeitMinuten { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        [StringLength(200)]
        public string AnlieferungHinweis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mobilitaetsfahrzeug)]
        public bool AnlieferungMobilitaetsfahrzeug { get; set; }

    }
}
