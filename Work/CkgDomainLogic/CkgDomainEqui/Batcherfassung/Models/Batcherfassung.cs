using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.FzgModelle.Models
{
    public enum BatchStatusEnum { Neu, ImZulauf, Geschlossen }

    public class Batcherfassung : Store
    {
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        [Required]
        public string HerstellerName { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.BatchID)]
        [Required]
        [Length(8)]
        public string ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]
        [Required]
        [Length(40)]
        public string ModellId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        [Required]
        [Length(40)]
        public string Modellbezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]
        [Required]
        [Length(4)]
        public string SippCode { get; set; }
       
        [LocalizedDisplay(LocalizeConstants.UnitnumberFrom)]      
        [Length(8)]
        public string UnitnummerVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitnumberUntil)]        
        [Length(8)]
        public string UnitnummerBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Unitnumbers)]
        public string Unitnummern { get; set; }
       
        [LocalizedDisplay(LocalizeConstants.Quantity)]
        [Required]
        [Length(5)]
        public string Anzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryMonth)]
        [Required]
        [Length(7)]
        public string Liefermonat { get; set; }

        [LocalizedDisplay(LocalizeConstants.PeriodOfValidityDays)]
        [Required]
        [Length(4)]
        public string Laufzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.PeriodOfValidityBinding)]
        public bool Laufzeitbindung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Remark)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumberFrom)]
        public string AuftragsnummerVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumberUntil)]
        public string AuftragsnummerBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        public bool StatusNeu { get { return (Status == "NEU"); } }

        public string Fahrzeuggruppe { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }
       
        [LocalizedDisplay(LocalizeConstants.WinterTires)]
        public bool Winterreifen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Towbar)]
        public bool AnhaengerKupplung { get; set; }

        [LocalizedDisplay(LocalizeConstants.NaviAvailable)]
        public bool NaviVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.SecurityFleet)]
        public bool SecurityFleet { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateLeaseCar)]
        public bool KennzeichenLeasingFahrzeug { get; set; }

        [GridHidden, GridExportIgnore]
        public string Antrieb { get; set; }

        [LocalizedDisplay(LocalizeConstants.Bluetooth)]
        public bool Bluetooth { get; set; }
  
        [LocalizedDisplay(LocalizeConstants.Usage)]
        public string Verwendung { get; set; }

        [LocalizedDisplay(LocalizeConstants.WebUser)]
        public string WebUser { get; set; }      
    }
}
