using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Finance.Models
{
    /// <summary>
    /// Parametersatz für die Telefoniedatensuche
    /// </summary>
    public class TelefoniedatenSuchparameter : Store, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }

        public List<string> AuswahlVertragsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateRange DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days){ IsSelected = false }); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay("")]
        public string DatumRangeHinweis { get { return Localize.HintIfNoDateRangeSelectedDefaultWillBeLastMonth; } }

        [LocalizedDisplay(LocalizeConstants.CallType)]
        public string Anrufart { get; set; }

        public string Anrufarten { get { return ",alle;E,Eingehend;A,Ausgehend"; } }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatumRange.StartDate.HasValue && DatumRange.EndDate.HasValue && DatumRange.StartDate.Value > DatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid);
        }
    }
}
