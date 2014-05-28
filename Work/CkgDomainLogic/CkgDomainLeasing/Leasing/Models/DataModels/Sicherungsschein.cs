using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class Sicherungsschein
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string Kundennummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string StandortCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Standort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Lessee)]
        public string LN_Name { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string LN_Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string LN_Plz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string LN_Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningLevel)]
        public string Mahnstufe { get; set; }

        [LocalizedDisplay(LocalizeConstants.ClarificationCase)]
        public string Klaerfallcode { get; set; }

        [LocalizedDisplay(LocalizeConstants.ClarificationCase)]
        public string Klaerfalltext { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CompanyNo)]
        public string Konzernnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string Kundenreferenz { get; set; }
    }
}
