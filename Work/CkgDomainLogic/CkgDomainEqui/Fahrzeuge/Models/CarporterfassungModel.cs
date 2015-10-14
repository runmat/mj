using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class CarporterfassungModel : Store, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.User)]
        [XmlIgnore]
        public string UserName { get { return EditUser.NotNullOr(GetViewModel == null ? "" : GetViewModel().LogonContext.UserName); } }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        [Required]
        public string CarportId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string CarportIdPersisted { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string CarportSelectionMode { get; set; }
        public string CarportSelectionModes { get { return GetViewModel == null ? "" : GetViewModel().CarportSelectionModes; } }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string CarportName { get; set; }

        public IDictionary<string, string> CarportPdis
        {
            get { return GetViewModel == null ? new Dictionary<string, string>() : GetViewModel().CarportPdis; }
        }

        public IDictionary<string, string> CarportPersistedPdis
        {
            get { return GetViewModel == null ? new Dictionary<string, string>() : GetViewModel().CarportPersistedPdis; }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.ForeignCountries)]
        public bool Ausland { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.CheckDigit)]
        public string FahrgestellNrPruefziffer { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [Required]
        [Length(7, true)]
        [RegularExpression(@"^[a-zA-Z]{2}\d{5}$")]
        [LocalizedDisplay(LocalizeConstants.InventoryNumber)]
        public string BestandsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Barcode)]
        [Required, Numeric, Length(8, forceExactLength: true)]
        public string Barcode { get; set; }

        [Length(1)]
        [LocalizedDisplay(LocalizeConstants.NumberOfLicensePlates)]
        public string AnzahlKennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.DisassemblyDate)]
        public DateTime? DemontageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.SelfDeregistrator)]
        public bool Abgemeldet { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb1Available)]
        public bool Zb1Vorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb2Available)]
        public bool Zb2Vorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.CocAvailable)]
        public bool CocVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.ServiceRecordAvailable)]
        public bool ServiceheftVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.HuAuReportAvailable)]
        public bool HuAuBerichtVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.SpareKeyAvailable)]
        public bool ZweitschluesselVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.NaviCdAvailable)]
        public bool NaviCdVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryNoteNo)]
        public string LieferscheinNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        public string Action { get; set; }

        [XmlIgnore]
        public string TmpStatus { get; set; }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<CarporterfassungViewModel> GetViewModel { get; set; }

        public bool UserHasCarportId
        {
            get { return GetViewModel != null && !String.IsNullOrEmpty(GetViewModel().UserCarportId); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var regexItem = new Regex("^[A-ZÄÖÜ]{1,3}-[A-ZÄÖÜ]{1,2}[0-9]{1,4}$");

            if (!Ausland && !regexItem.IsMatch(Kennzeichen.NotNullOrEmpty().ToUpper()))
                yield return new ValidationResult(Localize.LicenseNoInvalid, new[] { "Kennzeichen" });
        }
    }
}
