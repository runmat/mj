using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class CocAnforderung
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<CocAnforderungViewModel> GetViewModel { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugTyp { get; set; }

        [XmlIgnore]
        public static List<Domaenenfestwert> FahrzeugTypList { get { return GetViewModel == null ? new List<Domaenenfestwert>() : GetViewModel().Fahrzeugarten; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller { get; set; }

        [XmlIgnore]
        public static List<Hersteller> HerstellerList { get { return GetViewModel == null ? new List<Hersteller>() : GetViewModel().Hersteller; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.CountryOfFirstRegistration)]
        public string LandDerErstenZulassung { get; set; }

        [XmlIgnore]
        public static List<SelectItem> Laendergruppen { get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().Laendergruppen; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.YearOfFirstRegistration)]
        public string JahrDerErstzulassung { get; set; }

        [XmlIgnore]
        public static List<SelectItem> Jahre { get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().Jahre; } }

        [LocalizedDisplay(LocalizeConstants.Salutation)]
        public string Anrede { get; set; }

        [XmlIgnore]
        public static List<SelectItem> Anreden { get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().Anreden; } }

        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string Vorname { get; set; }

        [LocalizedDisplay(LocalizeConstants.LastName)]
        public string Nachname { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }

        [LocalizedDisplay(LocalizeConstants.PhoneNo)]
        public string TelefonNr { get; set; }
    }
}
