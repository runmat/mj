using System.Collections.Generic;
using System.Xml.Serialization;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class Fahrzeug
    {
        private string _mFIN;

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.VINRequired)]
        public string FIN
        {
            get { return _mFIN; }
            set { _mFIN = (value == null ? null : value.ToUpper()); }
        }

        [XmlIgnore]
        public string Kennzeichen { get { return string.Format("{0}-{1}", KennzeichenOrt, KennzeichenRest); } }

        private string _mKennzeichenOrt;

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.LicenseNoRequired)]
        public string KennzeichenOrt
        {
            get { return _mKennzeichenOrt; }
            set { _mKennzeichenOrt = (value == null ? null : value.ToUpper()); }
        }

        private string _mKennzeichenRest;

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.Hyphen)]
        public string KennzeichenRest
        {
            get { return _mKennzeichenRest; }
            set { _mKennzeichenRest = (value == null ? null : value.ToUpper()); }
        }

        private string _mTyp;

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants._TypRequired)]
        public string Typ
        {
            get { return _mTyp; }
            set { _mTyp = (value == null ? null : value.ToUpper()); }
        }

        private string _mReferenznummer;

        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants._Referenznummer)]
        public string Referenznummer
        {
            get { return _mReferenznummer; }
            set { _mReferenznummer = (value == null ? null : value.ToUpper()); }
        }

        public string FahrzeugIndex { get; set; }

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants._FahrzeugwertRequired)]
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

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants._FahrzeugZugelassenUndBetriebsbereitRequired)]
        public string FahrzeugZugelassen { get; set; }

        public string FahrzeugZugelassenDAD { get { return "N"; } }

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants._ZulassungAnKroschkeBeauftragtRequired)]
        public string ZulassungsauftragAnDAD { get; set; }

        [RequiredButModelOptional]
        [ModelMappingClearable]
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

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants._FahrzeugklasseRequired)]
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

        public bool RequestLoadCarData { get; set; }
        public string RequestLoadCarDataSource { get; set; }

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
