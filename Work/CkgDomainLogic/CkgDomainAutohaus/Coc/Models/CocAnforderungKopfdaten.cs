using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class CocAnforderungKopfdaten
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<CocAnforderungViewModel> GetViewModel { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugTyp { get; set; }

        [XmlIgnore]
        public static List<Domaenenfestwert> FahrzeugTypList { get { return GetViewModel == null ? new List<Domaenenfestwert>() : GetViewModel().Fahrzeugarten; } }

        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugTypBezeichnung
        {
            get { return (FahrzeugTypList.Any(f => f.Wert == FahrzeugTyp) ? FahrzeugTypList.First(f => f.Wert == FahrzeugTyp).Beschreibung : FahrzeugTyp); }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller { get; set; }

        [XmlIgnore]
        public static List<Hersteller> HerstellerList { get { return GetViewModel == null ? new List<Hersteller>() : GetViewModel().Hersteller; } }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string HerstellerBezeichnung
        {
            get { return (HerstellerList.Any(h => h.Code == Hersteller) ? HerstellerList.First(h => h.Code == Hersteller).Name : Hersteller); }
        }

        [Required]
        [LocalizedDisplay(LocalizeConstants.CountryOfFirstRegistration)]
        public string LandDerErstenZulassung { get; set; }

        [XmlIgnore]
        public static List<Land> Laender { get { return GetViewModel == null ? new List<Land>() : GetViewModel().Laender; } }

        [LocalizedDisplay(LocalizeConstants.CountryOfFirstRegistration)]
        public string LandDerErstenZulassungBezeichnung
        {
            get { return (Laender.Any(l => l.ID == LandDerErstenZulassung) ? Laender.First(l => l.ID == LandDerErstenZulassung).Name : LandDerErstenZulassung); }
        }
    }
}
