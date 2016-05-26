using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Parametersatz für die Grunddaten-/Equi-Suche
    /// </summary>
    public class EquiGrunddatenSelektor : Store 
    {
        [LocalizedDisplay(LocalizeConstants._Zielorte)]
        public static List<SelectItem> AlleZielorte { get { return (GetViewModel == null ? new List<SelectItem>() : GetViewModel().Zielorte); } }

        [LocalizedDisplay(LocalizeConstants._Standorte)]
        public static List<SelectItem> AlleStandorte  { get { return (GetViewModel == null ? new List<SelectItem>() : GetViewModel().Standorte); } }

        [LocalizedDisplay(LocalizeConstants._Betriebsnummern)]
        public static List<SelectItem> AlleBetriebsnummern { get { return (GetViewModel == null ? new List<SelectItem>() : GetViewModel().Betriebsnummern); } }

        [LocalizedDisplay(LocalizeConstants._Zielorte)]
        public List<string> Zielorte { get; set; }

        [LocalizedDisplay(LocalizeConstants._StandortFahrzeugdokument)]
        public List<string> Standorte { get; set; }

        [LocalizedDisplay(LocalizeConstants._Betriebsnummern)]
        [FormPersistable]
        public List<string> Betriebsnummern { get; set; }

        [LocalizedDisplay(LocalizeConstants._Erstzulassung)]
        [FormPersistable]
        public DateRange ErstzulassungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.CancellationDate)]
        [FormPersistable]
        public DateRange AbmeldeDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants._Erfassungsdatum)]
        public DateRange ErfassungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.VIN17)]
        public List<Fahrgestellnummer> Fahrgestellnummern { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN17)]
        public string Fahrgestellnummer
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value == null ? "" : value.ToUpper()); }
        }

        [LocalizedDisplay(LocalizeConstants.VIN10)]
        public List<Fahrgestellnummer10> Fahrgestellnummern10 { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN10)]
        [FormPersistable]
        public string Fahrgestellnummer10
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value == null ? "" : value.ToUpper()); }
        }

        [LocalizedDisplay(LocalizeConstants.VehiclesOnlyUnregistrated)]
        [FormPersistable]
        public bool NurAbgemeldeteFahrzeuge { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehiclesLicenseStatus)]
        [FormPersistable]
        public string FahrzeugeMitZulassungsStatus { get { return PropertyCacheGet(() => "VehiclesAll"); } set { PropertyCacheSet(value); } }

        public static string FahrzeugeZulassungsStatusWerte
        {
            get
            {
                return string.Format("{0},{1};{2},{3};{4},{5}",
                    "VehiclesAll", Localize.VehiclesAll,
                    "VehiclesOnlyLicensed", Localize.VehiclesOnlyLicensed,
                    "VehiclesOnlyUnlicensed", Localize.VehiclesOnlyWithoutFirstRegistration);
            }
        }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<EquiGrunddatenViewModel> GetViewModel { get; set; }
    }
}
