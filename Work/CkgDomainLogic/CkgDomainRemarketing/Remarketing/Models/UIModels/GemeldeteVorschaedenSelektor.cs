using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Remarketing.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Remarketing.Models
{
    public class GemeldeteVorschaedenSelektor : Store, IValidatableObject
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<GemeldeteVorschaedenViewModel> GetViewModel { get; set; }

        private string _fahrgestellNr;
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr
        {
            get { return _fahrgestellNr; }
            set { _fahrgestellNr = value.NotNullOrEmpty().ToUpper().Replace(" ", ""); }
        }

        private string _kennzeichen;
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen
        {
            get { return _kennzeichen; }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper().Replace(" ", ""); }
        }

        [LocalizedDisplay(LocalizeConstants.InventoryNo)]
        public string InventarNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string Vermieter { get; set; }

        [XmlIgnore]
        public static List<Vermieter> VermieterList
        {
            get { return (GetViewModel == null ? new List<Vermieter>() : GetViewModel().Vermieter).CopyAndInsertAtTop(new Vermieter { VermieterId = "", VermieterName = Localize.DropdownDefaultOptionAll }); }
        }

        [LocalizedDisplay(LocalizeConstants.DamageDate)]
        public DateRange SchadensdatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.ContractYear)]
        public string Vertragsjahr { get; set; }

        public bool IsAv { get { return (GetViewModel != null && GetViewModel().IsAv); } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var vertragsJahr = Vertragsjahr.NotNullOrEmpty().Trim();

            if (!string.IsNullOrEmpty(vertragsJahr) && (vertragsJahr.Length < 4 || !vertragsJahr.IsNumeric() || vertragsJahr.ToInt(0) < 1900 || vertragsJahr.ToInt(0) > 2500))
                yield return new ValidationResult(string.Format("{0} {1}", Localize.ContractYear, Localize.Invalid.ToLower()), new[] { "Vertragsjahr" });
        }
    }
}
