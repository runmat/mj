using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Insurance.Models
{
    public class VertragsverlaengerungModel
    {
        [LocalizedDisplay(LocalizeConstants.Id)]
        public string ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string VIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.Conditions)]
        public string Bedingungen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ProductType)]
        public string Produkttyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractStart)]
        public DateTime? Vertragsbeginn { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractEnd)]
        public DateTime? Vertragsende { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        [GridExportIgnore]
        public bool Verlaengern { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [GridHidden]
        [GridExportIgnore]
        public int Resttage { 
            get
            {
                if (!Vertragsende.HasValue)
                {
                    return 0;
                }
                var tage = (Vertragsende.Value - DateTime.Now).TotalDays;
                return Convert.ToInt32(Math.Floor(tage));
            } 
        }
    }
}
