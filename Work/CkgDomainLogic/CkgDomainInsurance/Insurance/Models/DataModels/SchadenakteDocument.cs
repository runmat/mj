using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Insurance.Models
{
    public class SchadenakteDocument : CustomerDocument, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.DamageCase)]
        public int SchadenfallID 
        {
            get 
            { 
                int tempInt;
                Int32.TryParse(ReferenceField, out tempInt);
                return tempInt;
            }
            set { ReferenceField = value.ToString(); }
        }

        [LocalizedDisplay(LocalizeConstants.ServiceProvider)]
        public string Dienstleister
        {
            get { return AdditionalData; }
            set { AdditionalData = value; }
        }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SchadenfallID < 0)
            {
                yield return new ValidationResult(Localize.DamageCaseInvalid, new[] { "SchadenfallID" });
            }
        }
    }
}
