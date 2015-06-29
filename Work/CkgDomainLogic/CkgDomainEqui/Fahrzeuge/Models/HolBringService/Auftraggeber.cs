using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CkgDomainLogic.Fahrzeuge.Models.HolBringService
{
    public class Auftraggeber : IValidatableObject    
    {
        public string Auftragersteller { get; set; }
        public string AuftragerstellerTel { get; set; }
        public string Betrieb { get; set; }
        public string Repco { get; set; }
        public string Ansprechpartner { get; set; }
        public string AnsprechpartnerTel { get; set; }
        public string Kunde { get; set; }
        public string KundeTel { get; set; }
        public string Kennnzeichen { get; set; }
        public int Fahrzeugart { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // throw new NotImplementedException();
            return null;
        }
    }
}
