using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models.HolBringService
{
    public class Abholung 
    {
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string AbholungKunde { get; set; }

        [LocalizedDisplay(LocalizeConstants.StreetHouseNo)]
        public string AbholungStrasseHausNr { get; set; }

        public string AbholungPlz { get; set; }
        public string AbholungOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Contactperson)]
        public string AbholungAnsprechpartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string AbholungTel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime AbholungDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string AbholungUhrzeitStunden { get; set; }
        public string AbholungUhrzeitMinuten { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        [StringLength(200)]
        public string AbholungHinweis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mobilitaetsfahrzeug)]
        public bool AbholungMobilitaetsfahrzeug { get; set; }


    }
}
