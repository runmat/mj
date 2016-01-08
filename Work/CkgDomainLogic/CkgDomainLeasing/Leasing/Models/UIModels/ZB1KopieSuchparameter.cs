using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    /// <summary>
    /// Parametersatz für die ZB1Kopie-Suche
    /// </summary>
    public class ZB1KopieSuchparameter : IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.DateFrom)]
        [Required]
        public string DatumVon { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.DateTo)]
        [Required]
        public string DatumBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        [Required]
        public int Kunde { get; set; }

        public List<ZB1KopieSucheKunde> Kundenauswahl 
        { 
            get 
            { 
                List<ZB1KopieSucheKunde> liste = new List<ZB1KopieSucheKunde>();
                liste.Add(new ZB1KopieSucheKunde(1, Localize.CustomerPG));
                liste.Add(new ZB1KopieSucheKunde(2, Localize.CustomerOther));
                return liste;
            }
        }

        [LocalizedDisplay(LocalizeConstants.ClarificationCases)]
        public bool Klaerfaelle { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public ZB1KopieSuchparameter()
        {
            // Kundenauswahl mit "P & G" vorbelegen
            Kunde = 1;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime vonDatum;
            if (!DateTime.TryParseExact(DatumVon, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out vonDatum))
                yield return new ValidationResult(Localize.DateFromInvalid, new[] { "DatumVon" });

            DateTime bisDatum;
            if (!DateTime.TryParseExact(DatumVon, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out bisDatum))
                yield return new ValidationResult(Localize.DateToInvalid, new[] { "DatumBis" });

            if (vonDatum > bisDatum)
                yield return new ValidationResult(Localize.DateRangeInvalid, new[] { "DatumBis" });
        }
    }
}
