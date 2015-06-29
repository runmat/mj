using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CkgDomainLogic.Fahrzeuge.Models.HolBringService
{
    public class Abholung : IValidatableObject
    {
        public string AbholungKunde { get; set; }
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // throw new NotImplementedException();
            return null;
        }
    }
}
