using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CkgDomainLogic.Fahrzeuge.ViewModels;

namespace CkgDomainLogic.Fahrzeuge.Models.HolBringService
{
    public class Anlieferung    //  : IValidatableObject
    {
        public string AnlieferungKunde { get; set; }
        public string AnlieferungStrasseHausNr { get; set; }
        public string AnlieferungPlz { get; set; }
        public string AnlieferungOrt { get; set; }
        public string AnlieferungAnsprechpartner { get; set; }
        public string AnlieferungTel { get; set; }

        public DateTime AbholungDatumUhrzeit { get; set; }
        public string AbholungAbUhrzeitStunden { get; set; }
        public string AbholungAbUhrzeitMinuten { get; set; }

        public string AnlieferungBisUhrzeitStunden { get; set; }
        public string AnlieferungBisUhrzeitMinuten { get; set; }

        public string AnlieferungHinweis { get; set; }
        public bool AnlieferungMobilitaetsfahrzeug { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    //if (KostenstelleVisible && Kostenstelle.IsNullOrEmpty())
        //    //    yield return new ValidationResult(Localize.CostcenterRequired, new[] { "Kostenstelle" });
        //    // throw new NotImplementedException();
        //    return null; ;
        //}
    }
}
