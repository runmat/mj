using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class TreuhandverwaltungCsvUpload
    {

        [ImportIgnore()]        
        public bool IsSelected { get; set; }

        [ImportIgnore()]
        [ScaffoldColumn(false)]
        public string AppId { get; set; }

        [ImportIgnore()]
        [ScaffoldColumn(false)]
        public string AGNummer { get; set; }

        [ImportIgnore()]
        [ScaffoldColumn(false)]
        public string TGNummer { get; set; }

        [ImportIgnore()]
        [ScaffoldColumn(false)]
        public bool IsSperren { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateofBlock)]
        public DateTime? Sperrdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string Referenznummer { get; set; }

        [ImportIgnore()]
        [LocalizedDisplay(LocalizeConstants.Officiale)]
        public string Sachbearbeiter { get; set; }

        [ImportIgnore()]
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? Datum { get; set; }

        [ImportIgnore()]
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Aktion { get; set; }

        [ImportIgnore()]
        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [ImportIgnore()]
        [LocalizedDisplay(LocalizeConstants.Error)]
        public string ValidationErrors { get; set; }

        
        public bool MemberHasValidationError(string propertyName = null)
        {
            if (propertyName.IsNullOrEmpty())
                return ValidationErrors.IsNotNullOrEmpty();

            return ValidationErrors.NotNullOrEmpty().Split(',').Contains(propertyName);
        }
    }
}
