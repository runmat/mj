using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.VersEvents.Models
{
    [Table("VersEventOrtBoxVorgang")]
    public class Vorgang : IAddressStreetHouseNo
    {
        private string _kennzeichen;

        [Key]
        public int ID { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public int KundenNr { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        public DateTime? LoeschDatum { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.DeleteUser)]
        public string LoeschUser { get; set; }


        [Required]
        [LocalizedDisplay(LocalizeConstants.InsuranceCompany)]
        public string VersicherungID { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.InsuranceNo)]
        public string VersicherungNr { get; set; }

        [Kennzeichen]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen
        {
            get { return _kennzeichen; }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper(); }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string HausNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string StrasseHausNr { get { return AddressService.FormatStreetAndHouseNo(this); } }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string PLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        [AddressPostcodeCityMapping("PLZ", "Land")]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        public string Land { get; set; }

        [GridHidden]
        [LocalizedDisplay(LocalizeConstants.Note)]
        public string Bemerkung { get; set; }

        [GridHidden]
        [NotMapped]
        [LocalizedDisplay(LocalizeConstants.Occurrence)]
        public string VorgangAsText { get { return string.Concat(Kennzeichen.AppendIfNotNull(", "), Name1, Name2.PrependIfNotNull(" "), VersicherungNr.PrependIfNotNull(", Vers.Nr. ")); } }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime AnlageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateUser)]
        public string AnlageUser { get; set; }

        
        [XmlIgnore]
        static public List<SelectItem> Versicherungen { get; set; }

        [XmlIgnore]
        static public List<Land> Laender { get; set; }


        [ModelMappingCompareIgnore]
        [GridHidden]
        [NotMapped]
        public bool InsertModeTmp { get; set; }

        public Vorgang SetInsertMode(bool insertMode)
        {
            InsertModeTmp = insertMode;
            return this;
        }
    }
}
