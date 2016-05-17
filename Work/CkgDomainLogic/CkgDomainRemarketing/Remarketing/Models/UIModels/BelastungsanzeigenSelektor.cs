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
    public class BelastungsanzeigenSelektor : Store, IValidatableObject
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<BelastungsanzeigenViewModel> GetViewModel { get; set; }

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

        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string Vermieter { get; set; }

        [XmlIgnore]
        public static List<Vermieter> VermieterList
        {
            get { return (GetViewModel == null ? new List<Vermieter>() : GetViewModel().Vermieter).CopyAndInsertAtTop(new Vermieter { VermieterId = "", VermieterName = Localize.DropdownDefaultOptionAll }); }
        }

        [LocalizedDisplay(LocalizeConstants.TakeInCenter)]
        public string Hereinnahmecenter { get; set; }

        [XmlIgnore]
        public static List<Hereinnahmecenter> HereinnahmecenterList
        {
            get { return (GetViewModel == null ? new List<Hereinnahmecenter>() : GetViewModel().Hereinnahmecenter).CopyAndInsertAtTop(new Hereinnahmecenter { HereinnahmecenterId = "", HereinnahmecenterName = Localize.DropdownDefaultOptionAll }); }
        }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [XmlIgnore]
        public static List<SelectItem> StatusList
        {
            get { return (GetViewModel == null ? new List<SelectItem>() : GetViewModel().StatusList).CopyAndInsertAtTop(new SelectItem { Key = "", Text = Localize.DropdownDefaultOptionAll }); }
        }
     
        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateRange EingangsdatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.ContractYear)]
        public string Vertragsjahr { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var vertragsJahr = Vertragsjahr.NotNullOrEmpty().Trim();

            if (vertragsJahr.Length < 4 && !EingangsdatumRange.IsSelected)
                yield return new ValidationResult(Localize.PleaseChooseAtLeastOneOption, new[] { "EingangsdatumRange", "Vertragsjahr" });

            if (!string.IsNullOrEmpty(vertragsJahr) && (vertragsJahr.Length < 4 || !vertragsJahr.IsNumeric() || vertragsJahr.ToInt(0) < 1900 || vertragsJahr.ToInt(0) > 2500))
                yield return new ValidationResult(string.Format("{0} {1}", Localize.ContractYear, Localize.Invalid.ToLower()), new[] { "Vertragsjahr" });
        }
    }
}
