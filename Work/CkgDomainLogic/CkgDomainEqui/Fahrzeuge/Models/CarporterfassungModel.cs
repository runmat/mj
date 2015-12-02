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
        [LocalizedDisplay(LocalizeConstants.LicenseNoForeignCountries)]
        public bool Ausland { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CheckDigit)]
        public string FahrgestellNrPruefziffer { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [Required]
        [Length(7, true)]
        [RegularExpression(@"^[a-zA-Z]{2}\d{5}$", ErrorMessage = "Ungültiges Bestandsnummer-Format")]
        [LocalizedDisplay(LocalizeConstants.InventoryNumber)]
        public string BestandsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Barcode)]
        [Required, Numeric, Length(8, forceExactLength: true)]
        public string Barcode { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfLicensePlates)]
        [Required]
        public string AnzahlKennzeichen { get; set; }

        public static string AnzahlKennzeichenOptionen
        {
            get
            {
                return string.Format(",{0};0,{1};1,{2};2,{3};H,{4};V,{5}",
                    Localize.DropdownDefaultOptionPleaseChoose,
                    Localize.NoLicensePlatesAvailable,
                    Localize.OneLicensePlateAvailableTrailerBike,
                    Localize.BothLicensePlatesAvailable,
                    Localize.LicensePlateRearAvailable,
                    Localize.LicensePlateFrontAvailable);
            }
        }

        [LocalizedDisplay(LocalizeConstants.DisassemblyDate)]
        public DateTime? DemontageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.SelfDeregistrator)]
        public bool Abgemeldet { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb1Available)]
        [Required]
        public string Zb1Vorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb2Available)]
        [Required]
        public string Zb2Vorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.CocAvailable)]
        [Required]
        public string CocVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.ServiceRecordAvailable)]
        [Required]
        public string ServiceheftVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.HuAuReportAvailable)]
        [Required]
        public string HuAuBerichtVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.SpareKeyAvailable)]
        [Required]
        public string ZweitschluesselVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.NaviCdAvailable)]
        [Required]
        public string NaviCdVorhanden { get; set; }

        public static string MaterialVorhandenOptionen
        {
            get
            {
                return string.Format(",{0};1,{1};0,{2};N,{3}",
                    Localize.DropdownDefaultOptionPleaseChoose,
                    Localize.Available,
                    Localize.NotAvailable,
                    Localize.WillBeDelivered);
            }
        }

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

        public static string GetMaterialVorhandenOptionWeb(string sapWert)
        {
            switch (sapWert)
            {
                case "X":
                    return "1";
                case "N":
                    return "N";
                default:
                    return "0";
            }
        }

        public static string GetMaterialVorhandenOptionSap(string webWert)
        {
            switch (webWert)
            {
                case "1":
                    return "X";
                case "N":
                    return "N";
                default:
                    return "";
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!BarcodeService.CheckBarcodeEan(Barcode.NotNullOrEmpty().Trim()))
                yield return new ValidationResult(Localize.BarcodeInvalid, new[] { "Barcode" });

            var regexItem = new Regex("^[A-ZÄÖÜ]{1,3}-[A-ZÄÖÜ]{1,2}[0-9]{1,4}[hH]?$");

            if (!Ausland && !regexItem.IsMatch(Kennzeichen.NotNullOrEmpty().Trim().ToUpper()))
                yield return new ValidationResult(Localize.LicenseNoInvalid, new[] { "Kennzeichen" });
        }
    }
}
