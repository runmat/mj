using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class CocAnforderung
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<CocAnforderungViewModel> GetViewModel { get; set; }

        public CocAnforderungKopfdaten Kopfdaten { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.YearOfFirstRegistration)]
        public string JahrDerErstzulassung { get; set; }

        [XmlIgnore]
        public static List<SelectItem> Jahre { get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().Jahre; } }

        private string _fahrgestellNr;

        [Required]
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr
        {
            get { return _fahrgestellNr; }
            set { _fahrgestellNr = value.NotNullOrEmpty().ToUpper(); }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string Vorname { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.LastName)]
        public string Nachname { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.PhoneNo)]
        public string TelefonNr { get; set; }

        public CocAnforderung()
        {
            Kopfdaten = new CocAnforderungKopfdaten();
        }
    }
}
