using System;
using System.Collections.Generic;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Strafzettel.Models
{
    public class StrafzettelSelektor : Store 
    {
        [LocalizedDisplay(LocalizeConstants.VIN17)]
        public string Fin
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value == null ? "" : value.ToUpper()); }
        }

        [LocalizedDisplay(LocalizeConstants.VIN10)]
        public string Fin10
        {
            get { return PropertyCacheGet(() => ""); }
            set { PropertyCacheSet(value == null ? "" : value.ToUpper()); }
        }
    
        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string VertragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorityFileNumber)]
        public string Aktenzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [FormPersistable]
        [Kennzeichen]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateRange EingangsDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.AuthorityDate)]
        [FormPersistable]
        public DateRange BehoerdeDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.Authority)]
        public string BehoerdeName { get; set; }

        [LocalizedDisplay(LocalizeConstants.AuthorityPostcode)]
        [FormPersistable]
        public string BehoerdePlz { get; set; }


        [LocalizedDisplay(LocalizeConstants.CustomizableStrafzettelRef1)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomizableStrafzettelRef2)]
        public string Referenz2 { get; set; }


        [LocalizedDisplay(LocalizeConstants.ForeignCountries)]
        public bool IstAusland { get; set; }

        [LocalizedDisplay(LocalizeConstants.InvoiceDate)]
        public DateTime? RechnungsDatum { get; set; }


        [LocalizedDisplay(LocalizeConstants._Standorte)]
        public static List<Standort> AlleStandorte
        {
            get
            {
                return new List<Standort>
                {
                    new Standort("", ""),
                    new Standort("1601", "Ingolstadt Neuwagen"),
                    new Standort("1602", "Ingolstadt SD"),
                    new Standort("1603", "Gebrauchtwagen"),
                    new Standort("1604", "Ingolstadt MFC"),
                    new Standort("1651", "Neckarsulm Neuwagen"),
                };
            }
        }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public List<string> Standorte
        {
            get { return PropertyCacheGet(() => new List<string> { "1602", "1604"}); }
            set { PropertyCacheSet(value); }
        }


        [LocalizedDisplay(LocalizeConstants.VehiclesLicenseStatus)]
        [FormPersistable]
        public string FahrzeugStatus { get { return PropertyCacheGet(() => "VehiclesOnlyLicensed"); } set { PropertyCacheSet(value); } }

        public static string FahrzeugStatusWerte
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
