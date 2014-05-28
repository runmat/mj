using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.VersEvents.Models
{
    public class VersEventOrt : IAddressStreetHouseNo, IValidatableObject
    {
        [Key]
        public int ID { get; set; }

        [GridHidden]
        public int VersEventID { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        [GridHidden]
        public DateTime? LoeschDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteUser)]
        [GridHidden]
        public string LoeschUser { get; set; }


        [LocalizedDisplay(LocalizeConstants.Name1)]
        [Required]
        public string OrtName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string OrtName2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        [GridHidden]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        [GridHidden]
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


        [LocalizedDisplay(LocalizeConstants.TimeStartMoFr)]
        public string StartZeitMoFr { get; set; }

        [LocalizedDisplay(LocalizeConstants.TimeEndMoFr)]
        public string EndZeitMoFr { get; set; }

        [LocalizedDisplay(LocalizeConstants.TimeStartSa)]
        public string StartZeitSa { get; set; }

        [LocalizedDisplay(LocalizeConstants.TimeEndSa)]
        public string EndZeitSa { get; set; }


        [GridHidden]
        [NotMapped]
        [LocalizedDisplay(LocalizeConstants.VersEventLocation)]
        public string OrtAsText { get { return string.Concat(OrtName, Ort.ReplaceIfNotNull(", "), Land.AppendIfNotNull("-"), PLZ.AppendIfNotNull(" "), Ort); } }


        [XmlIgnore]
        static public List<Land> Laender { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Land.NotNullOrEmpty().ToLower() == "de" && PLZ.IsNotNullOrEmpty() && PLZ.NotNullOrEmpty().Length != 5)
                yield return new ValidationResult(Localize.GermanPlzMustHave5Digits, new[] { "PLZ" });

            ValidationResult validationResult;
            if (!ValidateTimeRange(StartZeitMoFr, EndZeitMoFr, "Zeit (Mo-Fr)", out validationResult))
                yield return validationResult;
            if (!ValidateTimeRange(StartZeitSa, EndZeitSa, "Zeit (Sa)", out validationResult))
                yield return validationResult;
        }

        static bool ValidateTimeRange(string startTime, string endTime, string title, out ValidationResult validationResult)
        {
            validationResult = null;

            if (startTime.IsNullOrEmpty() && endTime.IsNullOrEmpty())
                return true;

            if (startTime.IsNullOrEmpty() || endTime.IsNullOrEmpty())
            {
                validationResult = new ValidationResult(string.Format("{0}: {1}", title, "Start und Endezeit müssen entweder beide mit gültigen Werten belegt oder leer sein"));
                return false;
            }
            if (!startTime.Contains(":") || startTime.Split(':').Any(t => t.ToInt() == -1))
            {
                validationResult = new ValidationResult(string.Format("{0}: {1}", title, "Die Startzeit bitte im gültigen Format HH:MM angeben"));
                return false;
            }
            if (!endTime.Contains(":") || endTime.Split(':').Any(t => t.ToInt() == -1))
            {
                validationResult = new ValidationResult(string.Format("{0}: {1}", title, "Die Endezeit bitte im gültigen Format HH:MM angeben"));
                return false;
            }

            var startVals = startTime.Split(':');
            var startTimeSpan = new TimeSpan(0, startVals[0].ToInt(), startVals[1].ToInt());
            var endVals = endTime.Split(':');
            var endTimeSpan = new TimeSpan(0, endVals[0].ToInt(), endVals[1].ToInt());
            if (startTimeSpan > endTimeSpan)
            {
                validationResult = new ValidationResult(string.Format("{0}: {1}", title, "Die Startzeit muss vor der Endezeit liegen"));
                return false;
            }

            return true;
        }
    }
}
