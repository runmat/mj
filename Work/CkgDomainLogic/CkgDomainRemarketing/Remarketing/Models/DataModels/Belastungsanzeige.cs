using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Remarketing.Models
{
    public class Belastungsanzeige
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.InventoryNo)]
        public string InventarNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarRentalCompany)]
        public string AutoVermieter { get; set; }

        [LocalizedDisplay(LocalizeConstants.TakeInCenter)]
        public string Hereinnahmecenter { get; set; }

        [LocalizedDisplay(LocalizeConstants.HcReceipt)]
        public DateTime? HcEingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateTime? Beauftragungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Sum)]
        public decimal? Summe { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfSurveys)]
        public string AnzahlGutachten { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string StatusText { get; set; }

        [LocalizedDisplay(LocalizeConstants.SurveyDate)]
        public DateTime? Gutachtendatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReleaseDate)]
        public DateTime? Freigabedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.BillNo)]
        public string RechnungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Surveyor)]
        public string Gutachter { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeprecationAv)]
        public decimal? MinderwertAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.RentalVehicle)]
        public bool Mietfahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfRepairCalculations)]
        public int AnzahlReparaturKalkulationen { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfRepairCalculation)]
        public DateTime? DatumReparaturKalkulation { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public bool Auswahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.BlockText)]
        public string BlockadeText { get; set; }
    }
}
