using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Ersatzkennzeichendaten : IValidatableObject
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        public Fahrzeugdaten Fahrzeugdaten { get; set; }

        public Ersatzkennzeichendaten()
        {
            Fahrzeugdaten = new Fahrzeugdaten { FahrzeugartId = "1" };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var dateResult in ValidateWochenendeUndFeiertage(Zulassungsdatum, "Zulassungsdatum").ToList())
                yield return dateResult;
        }

        static IEnumerable<ValidationResult> ValidateWochenendeUndFeiertage(DateTime? dateValue, string datePropertyName)
        {
            if (dateValue == null)
                yield break;

            var datum = dateValue.GetValueOrDefault();
            if (datum < DateTime.Today)
                yield return new ValidationResult("Bitte geben Sie ein Datum ab heute an", new[] { datePropertyName });
            else if (datum.DayOfWeek == DayOfWeek.Saturday || datum.DayOfWeek == DayOfWeek.Sunday)
                yield return new ValidationResult("Bitte vermeiden Sie Wochenendtage", new[] { datePropertyName });
            else
            {
                var feiertag = DateService.GetFeiertag(datum);
                if (feiertag != null)
                    yield return new ValidationResult(
                        $"Der {datum.ToString("dd.MM.yy")} ist ein Feiertag, '{feiertag.Name}'. Bitte vermeiden Sie Feiertage."
                        , new[] { datePropertyName });
            }
        }
    }
}
