using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiVersanddaten
    {
        [LocalizedDisplay(LocalizeConstants.DispatchZb2)]
        public DateTime? VersandZb2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.DispatchKey)]
        public DateTime? VersandSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.ExternalVoucherNumber)]
        public string ExterneBelegnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.BillingAmount)]
        public string Rechnungsbetrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.VoucherDate)]
        public DateTime? Belegdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ValutaFixDate)]
        public DateTime? ValutaFixDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReleaseDate)]
        public DateTime? Freigabedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.PaymentType)]
        public string Zahlungsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.Dealer)]
        public string Haendler { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string HaendlerName1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string HaendlerName2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name3)]
        public string HaendlerName3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.StreetHouseNo)]
        public string HaendlerStrasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string HaendlerPlz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string HaendlerOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        public string HaendlerLand { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string BankName1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string BankName2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name3)]
        public string BankName3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.StreetHouseNo)]
        public string BankStrasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string BankPlz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string BankOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Country)]
        public string BankLand { get; set; }
    }
}
