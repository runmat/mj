using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutohausRestService.Models
{
    public class Datensatz : IValidatableObject
    {
        public Partner Partnerdaten { get; set; }

        public List<Fahrzeug> Fahrzeuge { get; set; }

        public Datensatz()
        {
            Fahrzeuge = new List<Fahrzeug>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Partnerdaten == null && Fahrzeuge.Count == 0)
                yield return new ValidationResult("Leerer Datensatz.", new[] { "" });
        }
    }
}