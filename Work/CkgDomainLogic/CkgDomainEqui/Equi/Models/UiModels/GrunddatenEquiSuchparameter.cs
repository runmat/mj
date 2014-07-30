using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Parametersatz für die Grunddaten-/Equi-Suche
    /// </summary>
    public class GrunddatenEquiSuchparameter : Store 
    {
        [LocalizedDisplay(LocalizeConstants._Zielorte)]
        public List<Zielort> AlleZielorte
        {
            get
            {
                return PropertyCacheGet(() => new List<Zielort>
                {
                    new Zielort("", ""), 
                    new Zielort("900", "900 Neckarsulm"), 
                    new Zielort("960", "960 Ingolstadt")
                });
            }
        }

        [LocalizedDisplay(LocalizeConstants._Standorte)]
        public List<Standort> AlleStandorte 
        {
            get
            {
                return PropertyCacheGet(() => new List<Standort>
                {
                    new Standort("", ""),
                    new Standort("1601", "Ingolstadt Neuwagen"),
                    new Standort("1602", "Ingolstadt SD"),
                    new Standort("1603", "Gebrauchtwagen"),
                    new Standort("1604", "Ingolstadt MFC"),
                    new Standort("1651", "Neckarsulm Neuwagen"),
                });
            }
        }


        [LocalizedDisplay(LocalizeConstants._Betriebsnummern)]
        public List<Betriebsnummer> AlleBetriebsnummern
        {
            get
            {
                return PropertyCacheGet(() => new List<Betriebsnummer>
                {
                    new Betriebsnummer("", ""), 
                    new Betriebsnummer("849", "849 Direktkunden Kauf"), 
                    new Betriebsnummer("923", "923 Geschäftsfahrzeuge"), 
                    new Betriebsnummer("926", "926 Vorserienfahrzeuge"), 
                    new Betriebsnummer("953", "953 Behörden-Miete/VIP-Miete Fahrzeuge"), 
                    new Betriebsnummer("956", "956 Leasingfahrzeuge"), 
                    new Betriebsnummer("980", "980 Mitarbeiter Kauffahrzeuge")
                });
            }
        }


        [LocalizedDisplay(LocalizeConstants._Zielorte)]
        public List<string> Zielorte { get; set; }

        [LocalizedDisplay(LocalizeConstants._StandortFahrzeugdokument)]
        public List<string> Standorte { get; set; }

        [LocalizedDisplay(LocalizeConstants._Betriebsnummern)]
        public List<string> Betriebsnummern { get; set; }

        [LocalizedDisplay(LocalizeConstants._Erstzulassung)]
        public DateRange ErstzulassungsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.CancellationDate)]
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
        public string Fahrgestellnummer10
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value == null ? "" : value.ToUpper()); }
        }

        [LocalizedDisplay(LocalizeConstants.VehiclesOnlyUnregistrated)]
        public bool NurAbgemeldeteFahrzeuge { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehiclesLicenseStatus)]
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
    }
}
