using System;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class ZulassungsReportModel
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string KundenNrAndName { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReceiptNo)]
        public string BelegNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._Position)]
        public string PositionsNummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [Kennzeichen]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? ZulassungDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        public DateTime? ErfassungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationArea)]
        public string ZulassungsKreis { get; set; }

        [LocalizedDisplay(LocalizeConstants._KundenReferenz)]
        public string KundenReferenz { get; set; }

        [LocalizedDisplay(LocalizeConstants.EvbNumber)]
        public string EvbNummmer { get; set; }

        [LocalizedDisplay(LocalizeConstants.OurReceiptNo)]
        public string VertriebsBelegnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string KennzeichenMerkmal { get; set; }

        [LocalizedDisplay(LocalizeConstants.SalesUser)]
        public string VkUser { get; set; }

        [LocalizedDisplay(LocalizeConstants.Remark)]
        public string KundenNotiz { get; set; }

        [GridExportIgnore, GridHidden]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string StatusAsText { get; set; }

        [LocalizedDisplay(LocalizeConstants.MaterialNo)]
        public string MaterialNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.MaterialText)]
        public string MaterialKurztext { get; set; }

        [LocalizedDisplay(LocalizeConstants.F)]
        public string FeinstaubAmt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Document)]
        [GridExportIgnore]
        public string DokumentName { get; set; }

        [GridExportIgnore]
        public bool DokumentNameIsValid { get { return DokumentName.IsNotNullOrEmpty(); } }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.AhZulassungCostcenter)]
        public string Referenz3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.AhZulassungOrderNo)]
        public string Referenz4 { get; set; }

        [LocalizedDisplay(LocalizeConstants.AhZulassungReferenceNo)]
        public string Referenz5 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Price)]
        public decimal? Preis { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.Tax)]
        public decimal? PreisGebuehr { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.PriceTax)]
        public decimal? PreisSteuer { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.PriceLicenseNo)]
        public decimal? PreisKz { get; set; }

        [LocalizedDisplay(LocalizeConstants.Precollector)]
        public string Vorerfasser { get; set; }
    }
}
