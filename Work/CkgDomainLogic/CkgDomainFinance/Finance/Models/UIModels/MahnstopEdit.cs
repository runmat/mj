using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    /// <summary>
    /// Parametersatz für die Mahnstop-Bearbeitung
    /// </summary>
    public class MahnstopEdit : IValidatableObject
    {
        public string EquiNr { get; set; }

        public string MaterialNr { get; set; }

        public string PAID { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningBlock)]
        public bool Mahnsperre { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningStopUntil)]
        public DateTime? MahnstopBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Mahnsperre && !MahnstopBis.HasValue)
                yield return new ValidationResult(Localize.DunningStopUntilRequired, new[] { "MahnstopBis" });
        }
    }
}