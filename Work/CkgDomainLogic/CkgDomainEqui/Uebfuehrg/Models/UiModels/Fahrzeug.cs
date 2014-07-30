using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class Fahrzeug : CommonUiModel
    {
        private string _mFIN;

        [Required]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN
        {
            get { return _mFIN; }
            set { _mFIN = (value == null ? null : value.ToUpper()); }
        }

        [XmlIgnore]
        public string Kennzeichen { get { return string.Format("{0}-{1}", KennzeichenOrt, KennzeichenRest); } }

        private string _mKennzeichenOrt;

        [Required]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string KennzeichenOrt
        {
            get { return _mKennzeichenOrt; }
            set { _mKennzeichenOrt = (value == null ? null : value.ToUpper()); }
        }

        private string _mKennzeichenRest;

        [Required]
        [LocalizedDisplay(LocalizeConstants.Hyphen)]
        public string KennzeichenRest
        {
            get { return _mKennzeichenRest; }
            set { _mKennzeichenRest = (value == null ? null : value.ToUpper()); }
        }

        private string _mTyp;

        [Required]
        [LocalizedDisplay(LocalizeConstants._Typ)]
        public string Typ
        {
            get { return _mTyp; }
            set { _mTyp = (value == null ? null : value.ToUpper()); }
        }

        private string _mReferenznummer;

        [LocalizedDisplay(LocalizeConstants._Referenznummer)]
        public string Referenznummer
        {
            get { return _mReferenznummer; }
            set { _mReferenznummer = (value == null ? null : value.ToUpper()); }
        }

        public string FahrzeugIndex { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VehicleValue)]
        public string Fahrzeugwert { get; set; }

        [XmlIgnore]
        public List<SelectItem> FahrzeugwertOptions
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("", ""),
                    new SelectItem("Z00", "bis 50 Tsd. €"),
                    new SelectItem("Z50", "bis 150 Tsd. €"),
                };
            }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VehiclesLicenseStatus)]
        public string FahrzeugZugelassen { get; set; }

        public string FahrzeugZugelassenDAD { get { return "N"; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants._ZulassungAnKroschkeBeauftragtRequired)]
        public string ZulassungsauftragAnDAD { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants._Bereifung)]
        public string Bereifung { get; set; }
        [XmlIgnore]
        public List<SelectItem> BereifungOptions
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("", ""),
                    new SelectItem("S", "Sommer"),
                    new SelectItem("W", "Winter"),
                    new SelectItem("G", "Ganzjahresreifen"),
                };
            }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VehicleClass)]
        public string Fahrzeugklasse { get; set; }
        [XmlIgnore]
        public List<SelectItem> FahrzeugklasseOptions 
        { 
            get 
            { 
                return new List<SelectItem>
                {
                    new SelectItem("", ""),
                    new SelectItem("PKW", "< 3,5 Tonnen"),
                    new SelectItem("PK1", "3,5 - 7,5 Tonnen"),
                    new SelectItem("LKW", "> 7,5 Tonnen"),
                }; 
            } 
        }

        [XmlIgnore]
        public string EmptyString { get { return ""; } }

        [XmlIgnore]
        public string FahrzeugZugelassenConverted { get { return FahrzeugZugelassen.NotNullOrEmpty() == "N" ? "" : FahrzeugZugelassen; } }

        [XmlIgnore]
        public string KennzeichenConverted { get { return Kennzeichen.NotNullOrEmpty().Replace(" ", ""); } }

        [XmlIgnore]
        public string FahrzeugklasseConverted { get { return Fahrzeugklasse.NotNullOrEmpty().Replace("1", "W"); } }
    }
}
