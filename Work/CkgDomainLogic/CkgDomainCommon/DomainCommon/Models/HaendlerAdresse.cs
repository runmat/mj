using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class HaendlerAdresse 
    {
        public string ID { get { return HaendlerNr + "-" + LaenderCode; } }

        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        [Required]
        public string HaendlerNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CountryCode)]
        [Required]
        public string LaenderCode { get; set; }


        //
        // Brief-Adresse
        //

        [LocalizedDisplay(LocalizeConstants.Name1, "Brief")]
        [Required]
        public string Name1Brief { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2, "Brief")]
        public string Name2Brief { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street, "Brief")]
        [Required]
        [GridHidden]
        public string StrasseBrief { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo, "Brief")]
        [GridHidden]
        [Required]
        public string HausNrBrief { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street, "Brief")]
        [Required]
        public string StrasseHausNrBrief { get { return AddressService.FormatStreetAndHouseNo(StrasseBrief, HausNrBrief); } }

        [LocalizedDisplay(LocalizeConstants.PostCode, "Brief")]
        [Required]
        public string PlzBrief { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country, "Brief")]
        [Required]
        public string LandBrief { get; set; }

        [LocalizedDisplay(LocalizeConstants.City, "Brief")]
        [Required]
        [AddressPostcodeCityMapping("PlzBrief", "LandBrief")]
        public string OrtBrief { get; set; }


        [LocalizedDisplay(LocalizeConstants.KeyAddressSeparate)]
        public bool SchluesselAdresseVerfuegbar { get; set; }

        //
        // Schlüssel-Adresse
        //

        [LocalizedDisplay(LocalizeConstants.Name1, "Schlüssel")]
        [RequiredConditional]
        public string Name1Schluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2, "Schlüssel")]
        public string Name2Schluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street, "Schlüssel")]
        [RequiredConditional]
        [GridHidden]
        public string StrasseSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo, "Schlüssel")]
        [GridHidden]
        [RequiredConditional]
        public string HausNrSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street, "Schlüssel")]
        [RequiredConditional]
        public string StrasseHausNrSchluessel { get { return AddressService.FormatStreetAndHouseNo(StrasseSchluessel, HausNrSchluessel); } }

        [LocalizedDisplay(LocalizeConstants.PostCode, "Schlüssel")]
        [RequiredConditional]
        public string PlzSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country, "Schlüssel")]
        [RequiredConditional]
        public string LandSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.City, "Schlüssel")]
        [RequiredConditional]
        [AddressPostcodeCityMapping("PlzSchluessel", "LandSchluessel")]
        public string OrtSchluessel { get; set; }


        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<HaendlerAdressenViewModel> GetViewModel { get; set; }

        [GridHidden, NotMapped]
        public bool InsertModeTmp { get; set; }

        public HaendlerAdresse SetInsertMode(bool insertMode)
        {
            InsertModeTmp = insertMode;
            return this;
        }

        [XmlIgnore, GridHidden, NotMapped]
        public List<SelectItem> LaenderListWithDefaultOption
        {
            get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().LaenderListWithOptionPleaseChoose; }
        }


        [XmlIgnore, NotMapped, GridExportIgnore]
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Aktion { get; set; }
    }
}
