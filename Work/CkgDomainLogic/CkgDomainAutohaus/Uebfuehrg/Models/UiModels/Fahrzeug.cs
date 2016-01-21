using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class Fahrzeug : CommonUiModel
    {
        private string _mFIN;
        private string _mTyp;
        private string _mReferenznummer;
        private string _kennzeichen;
        private string _hersteller;

        [Required]
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN
        {
            get { return _mFIN; }
            set { _mFIN = value.NotNullOrEmpty().ToUpper(); }
        }

        [XmlIgnore]
        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [KennzeichenPartial]
        public string Kennzeichen
        {
            get { return _kennzeichen; }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper(); }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller
        {
            get { return _hersteller; }
            set { _hersteller = value.NotNullOrEmpty().ToUpper(); }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Model)]
        public string Modell
        {
            get { return _mTyp; }
            set { _mTyp = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants._Referenznummer)]
        public string Referenznummer
        {
            get { return _mReferenznummer; }
            set { _mReferenznummer = value.NotNullOrEmpty().ToUpper(); }
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

        [LocalizedDisplay(LocalizeConstants.VehicleValue)]
        public string FahrzeugwertText { get { return FahrzeugwertOptions.Find(o => o.Key == Fahrzeugwert.NotNullOrEmpty()).Text; } }

        [LocalizedDisplay(LocalizeConstants.VehiclesLicenseStatus)]
        public bool FahrzeugZugelassen { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehiclesLicenseOrderStatus)]
        public bool ZulassungBeauftragt { get; set; }

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
        public int AnzahlFahrzeugeGewuenscht { get; set; }

        [XmlIgnore]
        public string EmptyString { get { return ""; } }

        [XmlIgnore]
        public string FahrzeugklasseConverted { get { return Fahrzeugklasse.NotNullOrEmpty().Replace("1", "W"); } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Tires)]
        public string Bereifung { get; set; }

        [XmlIgnore]
        public List<SelectItem> BereifungOptions
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("", ""),
                    new SelectItem("S", "Sommerreifen"),
                    new SelectItem("W", "Winterreifen"),
                    new SelectItem("G", "Ganzjahresreifen"),
                };
            }
        }

        [LocalizedDisplay(LocalizeConstants.Tires)]
        public string BereifungText { get { return BereifungOptions.Find(o => o.Key == Bereifung.NotNullOrEmpty()).Text; } }
    
        public override string GetSummaryString()
        {
            return string.Format("FIN: {0}<br/>Kennzeichen: {1}<br/>", FIN, Kennzeichen);
        }
    }
}
