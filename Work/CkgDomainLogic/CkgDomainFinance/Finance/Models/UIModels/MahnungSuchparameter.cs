using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Finance.Models
{
    /// <summary>
    /// Parametersatz für die Mahnungssuche
    /// </summary>
    public class MahnungSuchparameter : Store, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDispatch)]
        public DateRange VersanddatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days) { IsSelected = false }); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.DunningDate)]
        public DateTime? Mahndatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningLevel)]
        public string Mahnstufe { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningBlock)]
        public bool Mahnsperre { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (VersanddatumRange.IsSelected && VersanddatumRange.StartDate.HasValue && VersanddatumRange.EndDate.HasValue && VersanddatumRange.StartDate.Value > VersanddatumRange.EndDate.Value)
                yield return new ValidationResult(Localize.DateRangeInvalid, new[] { "VersanddatumRange" });
        }
    }
}
