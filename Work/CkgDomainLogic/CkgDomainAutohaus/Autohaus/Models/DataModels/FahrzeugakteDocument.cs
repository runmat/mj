using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class FahrzeugakteDocument : CustomerDocument, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.Vehicle)]
        public int FahrzeugID 
        {
            get 
            { 
                int tempInt;
                Int32.TryParse(ReferenceField, out tempInt);
                return tempInt;
            }
            set { ReferenceField = value.ToString(); }
        }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung
        {
            get { return AdditionalData; }
            set { AdditionalData = value; }
        }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FahrzeugID < 0)
            {
                yield return new ValidationResult(Localize.VehicleInvalid, new[] { "FahrzeugID" });
            }
        }
    }
}
