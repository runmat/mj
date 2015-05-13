using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Unfallmeldung
    {
        private string _kennzeichen;
        private string _fahrgestellnummer;
        private string _stationsCode;
        private string _briefNummer;
        private string _unitNummer;

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? AnlageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.WebUser)]
        public string WebUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen
        {
            get { return _kennzeichen.NotNullOrEmpty().ToUpper(); }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer
        {
            get { return _fahrgestellnummer.NotNullOrEmpty().ToUpper(); }
            set { _fahrgestellnummer = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? ErstzulassungDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateArrival)]
        public DateTime? KennzeicheneingangsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Autohaus_Abmeldung)]
        public DateTime? AbmeldeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.StationCode)]
        [RequiredConditional]
        public string StationsCode
        {
            get { return _stationsCode.NotNullOrEmpty().ToUpper(); }
            set { _stationsCode = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.DunningLevel)]
        public string Mahnstufe { get; set; }

        [LocalizedDisplay(LocalizeConstants.AccidentNo)]
        public string UnfallNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CancelDate)]
        public DateTime? StornoDatum { get; set; }


        [ModelMappingCompareIgnore]
        public bool IsSelected { get; set; }

        [ModelMappingCompareIgnore]
        public bool IsValidForCancellation
        {
            get { return (KennzeicheneingangsDatum == null && StornoDatum == null); }
        }


        [LocalizedDisplay(LocalizeConstants.Selection)]
        [GridHidden, NotMapped, XmlIgnore]
        public string MeldungTyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationNo)]
        [GridHidden, NotMapped, XmlIgnore]
        public string BriefNummer
        {
            get { return _briefNummer.NotNullOrEmpty().ToUpper(); }
            set { _briefNummer = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.UnitNumber)]
        [GridHidden, NotMapped, XmlIgnore]
        public string UnitNummer
        {
            get { return _unitNummer.NotNullOrEmpty().ToUpper(); }
            set { _unitNummer = value.NotNullOrEmpty().ToUpper(); }
        }

        [GridHidden, NotMapped, XmlIgnore]
        public static string MeldungTypen { get { return string.Format("U,{0};D,{1}", "Unfallfahrzeug", "Diebstahl"); } }

        [LocalizedDisplay(LocalizeConstants.Location)]
        [GridHidden, NotMapped, XmlIgnore]
        public string Standort { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        [GridHidden, NotMapped, XmlIgnore]
        public string EquiNr { get; set; }
    }
}
