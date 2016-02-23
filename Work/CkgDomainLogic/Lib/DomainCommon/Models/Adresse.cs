using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class Adresse : CommonUiModel, IAddressStreetHouseNo, IValidatableObject
    {
        [SelectListKey]
        [ModelMappingCompareIgnore]
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [SelectListText]
        [GridHidden, ScriptIgnore]
        public string DisplayName { get { return GetAutoSelectString(); } }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        [Required]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        [Required]
        [GridHidden]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        [GridHidden]
        public string HausNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.StreetHouseNo)]
        [Required]
        public string StrasseHausNr { get { return AddressService.FormatStreetAndHouseNo(this); } }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        [Required]
        public string PLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        [Required]
        [AddressPostcodeCityMapping("PLZ", "Land")]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        [Required]
        public string Land { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string Telefon { get; set; }

        [LocalizedDisplay(LocalizeConstants.Fax)]
        public string Fax { get; set; }

        [XmlIgnore]
        static public List<Land> Laender { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email)]
        [EmailAddress]
        public string Email { get; set; }

        [LocalizedDisplay(LocalizeConstants.Recognition)]
        public string Kennung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Remark)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants._blank)]
        public bool Gewerblich { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string ReferenzNr { get; set; }

        public string Typ { get; set; }

        public int ID { get; set; }

        public int TestInt { get; set; }

        [GridHidden]
        public bool NoSaveButUiRefreshOnly { get; set; }

        [GridHidden]
        public bool IsValid { get; set; }

        [GridHidden]
        public bool InsertModeTmp { get; set; }

        [GridHidden]
        public string InternalKey { get; set; }

        [GridHidden]
        public string InternalKey2 { get; set; }

        [GridHidden]
        public string TmpSelectionKey { get; set; }

        [GridHidden]
        public bool IsDefaultPartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.EVB)]
        public string EvbNr { get; set; }                   

        [LocalizedDisplay(LocalizeConstants.SepaBooking)]
        public DateTime? Stichtagsabbuchung { get; set; }   

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.PartnerRolesToCreate)]
        public List<string> KennungenToInsert { get; set; }


        #region Bankdaten

        [LocalizedDisplay(LocalizeConstants.AccountHolder)]
        public string Kontoinhaber { get; set; }

        [LocalizedDisplay(LocalizeConstants.Iban)]
        public string Iban { get; set; }

        [LocalizedDisplay(LocalizeConstants.Swift)]
        public string Swift { get; set; }

        [LocalizedDisplay(LocalizeConstants.AccountNo)]
        public string KontoNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.BankCode)]
        public string Bankleitzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreditInstitution)]
        public string Geldinstitut { get; set; }

        #endregion


        public Adresse SetInsertMode(bool insertMode)
        {
            InsertModeTmp = insertMode;
            return this;
        }

        [XmlIgnore]
        public Func<string> GetAutoSelectStringCustom { get; set; }

        public string GetAutoSelectString()
        {
            if (GetAutoSelectStringCustom != null)
                return GetAutoSelectStringCustom();

            if (Land.IsNullOrEmpty() && PLZ.IsNullOrEmpty() && Ort.IsNullOrEmpty())
                return Name1;

            return string.Format("{0}, {1}{2} {3}", Name1, LandAsFormatted(Land), PLZ, Ort);
        }

        public string GetPostLabelString()
        {
            return string.Format("{0}{1}<br/>{2}<br/>{3}{4} {5}", Name1, (Name2.IsNullOrEmpty() ? "" : " " + Name2), StrasseHausNr, LandAsFormatted(Land), PLZ, Ort);
        }

        public static string LandAsFormatted(string land)
        {
            return land.IsNullOrEmpty() || land == "-" ? "" : land + "-";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Land.IsNotNullOrEmpty())
            {
                var generalDataService = DependencyResolver.Current.GetService(typeof (IAdressenDataService)) as IAdressenDataService;
                if (generalDataService != null)
                {
                    var countryPlzValidationMessage = generalDataService.CountryPlzValidate(Land, PLZ);
                    if (countryPlzValidationMessage.IsNotNullOrEmpty())
                        yield return new ValidationResult(countryPlzValidationMessage, new[] { "PLZ", "Land" });
                }
            }

            if (!string.IsNullOrEmpty(EvbNr) && EvbNr.Length != 7)      // 20150617 MMA
                yield return new ValidationResult(Localize.EvbNumberLengthMustBe7, new[] { "EvbNr" });

        }
    }
}
