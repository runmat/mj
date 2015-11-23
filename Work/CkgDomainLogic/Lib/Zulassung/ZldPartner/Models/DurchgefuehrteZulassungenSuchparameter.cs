using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.ZldPartner.Models
{
    public class DurchgefuehrteZulassungenSuchparameter : Store, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string Kunde { get; set; }

        public string Kunden { get { return ",Alle;DAD,DAD;CKG,Kroschke"; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateRange ZulassungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.CurrentMonth, true)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay("")]
        public string ZulassungsDatumRangeHinweis { get { return String.Format(Localize.DateRangeMaxnDays, "92"); } }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string Auswahl { get; set; }

        public string AuswahlOptionen { get { return "A,Alle Aufträge;D,Durchgeführte Aufträge;O,Offene Aufträge"; } }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Halter { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ZulassungsDatumRange.IsSelected)
                yield return new ValidationResult(Localize.ThisFieldIsRequired, new[] { "ZulassungsDatumRange" });

            if (ZulassungsDatumRange.MoreDaysThan(92))
                yield return new ValidationResult(String.Format(Localize.DateRangeMaxnDays, "92"), new[] { "ZulassungsDatumRange" });
        }
    }
}
