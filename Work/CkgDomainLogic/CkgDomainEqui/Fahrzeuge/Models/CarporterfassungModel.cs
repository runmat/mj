using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using MvcTools.Web;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    [GridColumnsAutoPersist]
    public class CarporterfassungModel : Store, IValidatableObject
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Region)]
        public string Organisation { get; set; }

        [LocalizedDisplay(LocalizeConstants.Region)]
        public string OrganisationText
        {
            get { return (Organizations.Any(m => m.Key == Organisation) ? Organizations.First(m => m.Key == Organisation).Value : Organisation); }
        }

        [LocalizedDisplay(LocalizeConstants.Region)]
        public string SelectedOrganizationId { get; set; }

        [LocalizedDisplay(LocalizeConstants.User)]
        public string UserName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        [Required]
        public string CarportId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string SelectedCarportId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string CarportSelectionMode { get; set; }
        public string CarportSelectionModes { get { return GetViewModel == null ? "" : GetViewModel().CarportSelectionModes; } }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string CarportName { get; set; }

        public IDictionary<string, string> Organizations
        {
            get { return GetViewModel == null ? new Dictionary<string, string>() : GetViewModel().Organizations; }
        }

        public IDictionary<string, string> OrganizationCarportPdis
        {
            get { return GetViewModel == null ? new Dictionary<string, string>() : GetViewModel().OrganizationCarportPdis; }
        }

        public IDictionary<string, string> AllCarportPdis
        {
            get { return GetViewModel == null ? new Dictionary<string, string>() : GetViewModel().AllCarportPdis; }
        }

        public IDictionary<string, string> CarportPdisForListFilter
        {
            get { return GetViewModel == null ? new Dictionary<string, string>() : GetViewModel().CarportPdisForListFilter; }
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

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [RequiredConditional]
        public string AnzahlKennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [RequiredConditional]
        public string AnzahlKennzeichenText
        {
            get { return (AnzahlKennzeichenOptionen.Any(m => m.Value == AnzahlKennzeichen) ? AnzahlKennzeichenOptionen.First(m => m.Value == AnzahlKennzeichen).Text : AnzahlKennzeichen); }
        }

        public static SelectList AnzahlKennzeichenOptionen
        {
            get
            {
                return string.Format(",{0};0,{1};2,{2}",
                    Localize.DropdownDefaultOptionPleaseChoose,
                    Localize.NoLicensePlatesAvailable,
                    Localize.BothLicensePlatesAvailable).ToSelectList();
            }
        }

        [LocalizedDisplay(LocalizeConstants.DisassemblyDate)]
        public DateTime? DemontageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.SelfDeregistrator)]
        public bool Abgemeldet { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZBI)]
        [Required]
        public string Zb1Vorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZBI)]
        [RequiredConditional]
        public string Zb1VorhandenText
        {
            get { return (MaterialVorhandenOptionen.Any(m => m.Value == Zb1Vorhanden) ? MaterialVorhandenOptionen.First(m => m.Value == Zb1Vorhanden).Text : Zb1Vorhanden); }
        }

        [LocalizedDisplay(LocalizeConstants.ZBII)]
        [Required]
        public string Zb2Vorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZBII)]
        [RequiredConditional]
        public string Zb2VorhandenText
        {
            get { return (MaterialVorhandenOptionen.Any(m => m.Value == Zb2Vorhanden) ? MaterialVorhandenOptionen.First(m => m.Value == Zb2Vorhanden).Text : Zb2Vorhanden); }
        }

        [LocalizedDisplay(LocalizeConstants.Coc)]
        [Required]
        public string CocVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.Coc)]
        [RequiredConditional]
        public string CocVorhandenText
        {
            get { return (MaterialVorhandenOptionen.Any(m => m.Value == CocVorhanden) ? MaterialVorhandenOptionen.First(m => m.Value == CocVorhanden).Text : CocVorhanden); }
        }

        [LocalizedDisplay(LocalizeConstants.ServiceRecord)]
        [Required]
        public string ServiceheftVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.ServiceRecord)]
        [RequiredConditional]
        public string ServiceheftVorhandenText
        {
            get { return (MaterialVorhandenOptionen.Any(m => m.Value == ServiceheftVorhanden) ? MaterialVorhandenOptionen.First(m => m.Value == ServiceheftVorhanden).Text : ServiceheftVorhanden); }
        }

        [LocalizedDisplay(LocalizeConstants.HuAuReport)]
        [Required]
        public string HuAuBerichtVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.HuAuReport)]
        [RequiredConditional]
        public string HuAuBerichtVorhandenText
        {
            get { return (MaterialVorhandenOptionen.Any(m => m.Value == HuAuBerichtVorhanden) ? MaterialVorhandenOptionen.First(m => m.Value == HuAuBerichtVorhanden).Text : HuAuBerichtVorhanden); }
        }

        [LocalizedDisplay(LocalizeConstants.SpareKey)]
        [Required]
        public string ZweitschluesselVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.SpareKey)]
        [RequiredConditional]
        public string ZweitschluesselVorhandenText
        {
            get { return (ZweitschluesselVorhandenOptionen.Any(m => m.Value == ZweitschluesselVorhanden) ? ZweitschluesselVorhandenOptionen.First(m => m.Value == ZweitschluesselVorhanden).Text : ZweitschluesselVorhanden); }
        }

        [LocalizedDisplay(LocalizeConstants.NaviCd)]
        [Required]
        public string NaviCdVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.NaviCd)]
        [RequiredConditional]
        public string NaviCdVorhandenText
        {
            get { return (MaterialVorhandenOptionen.Any(m => m.Value == NaviCdVorhanden) ? MaterialVorhandenOptionen.First(m => m.Value == NaviCdVorhanden).Text : NaviCdVorhanden); }
        }

        public static SelectList MaterialVorhandenOptionen
        {
            get
            {
                return string.Format(",{0};1,{1};0,{2};N,{3}",
                    Localize.DropdownDefaultOptionPleaseChoose,
                    Localize.Available,
                    Localize.NotAvailable,
                    Localize.WillBeDelivered).ToSelectList();
            }
        }

        public static SelectList ZweitschluesselVorhandenOptionen
        {
            get
            {
                return string.Format(",{0};0,0;1,1;2,2;3,3;N,{1}",
                    Localize.DropdownDefaultOptionPleaseChoose,
                    Localize.WillBeDelivered).ToSelectList();
            }
        }

        [LocalizedDisplay(LocalizeConstants.DeliveryNoteNo)]
        public string LieferscheinNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActionDot)]
        public string Action { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActionDot2)]
        public string Action2 { get; set; }

        [XmlIgnore]
        public string TmpStatus { get; set; }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<CarporterfassungViewModel> GetViewModel { get; set; }

        public bool UserHasCarportId
        {
            get { return GetViewModel != null && !String.IsNullOrEmpty(GetViewModel().UserCarportId); }
        }

        public bool UserHasAllOrganizations
        {
            get { return GetViewModel != null && GetViewModel().UserAllOrganizations; }
        }

        public bool ModusNacherfassung
        {
            get { return GetViewModel != null && GetViewModel().ModusNacherfassung; }
        }

        public bool UiUpdateOnly { get; set; }

        public bool IsValid { get; set; }

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
                case "2":
                case "3":
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

            if (!ModusNacherfassung && !Ausland && !regexItem.IsMatch(Kennzeichen.NotNullOrEmpty().Trim().ToUpper()))
                yield return new ValidationResult(Localize.LicenseNoInvalid, new[] { "Kennzeichen" });

            if (DemontageDatum.HasValue && DemontageDatum.Value.Date > DateTime.Now.Date)
                yield return new ValidationResult(string.Format("{0} {1}", Localize.DisassemblyDate, Localize.Invalid.NotNullOrEmpty().ToLower()), new[] { "DemontageDatum" });
        }
    }
}
