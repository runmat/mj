using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    [Table("AHFahrzeug")]
    public class Fahrzeug
    {
        private string _fahrgestellNr;
        private string _kennzeichen;

        [Key]
        [LocalizedDisplay(LocalizeConstants.Id)]
        public int ID { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string ReferenzNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr
        {
            get { return _fahrgestellNr; }
            set { _fahrgestellNr = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen
        {
            get { return _kennzeichen; }
            set { _kennzeichen = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string Vorname { get; set; }

        [LocalizedDisplay(LocalizeConstants.LastName)]
        public string Nachname { get; set; }

        [EmailAddress]
        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }

        [LocalizedDisplay(LocalizeConstants.PhoneNo)]
        public string TelefonNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string FzgHersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string FzgModell { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime AnlageDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateUser)]
        public string AnlageUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        public DateTime? LoeschDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteUser)]
        public string LoeschUser { get; set; }
    }
}
