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

        [Required]
        [LocalizedDisplay("Straße / Haus-Nr.")]            // LocalizeConstants.Customer
        public string AbholungStrasseHausNr { get; set; }

        public string AbholungPlz { get; set; }
        public string AbholungOrt { get; set; }
        public string AbholungAnsprechpartner { get; set; }
        public string AbholungTel { get; set; }

        public DateTime AbholungDatumUhrzeit { get; set; }
        public string AbholungUhrzeitStunden { get; set; }
        public string AbholungUhrzeitMinuten { get; set; }

        

        public string AbholungHinweis { get; set; }
        public bool AbholungMobilitaetsfahrzeug { get; set; }


    }
}
