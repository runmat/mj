using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using System;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class FzgUnitnummer : Batcherfassung  //: Store
    {

        //public bool IsSelected { get; set; }
        /*

        [LocalizedDisplay(LocalizeConstants.BatchID)]      
        public string ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]       
        public string ModellId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]      
        public string Modellbezeichnung { get; set; }      

       
        [LocalizedDisplay(LocalizeConstants.OrderNumberFrom)]      
        public string AuftragsnummerVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumberUntil)]        
        public string AuftragsnummerBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Quantity)]        
        public string Anzahl { get; set; }

      

        [LocalizedDisplay(LocalizeConstants.LicensePlateLeaseCar)]
        public bool KennzeichenLeasingFahrzeug { get; set; }
        */

        [LocalizedDisplay(LocalizeConstants.UnitNumber)]
        public string Unitnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string Equinummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.IntroductionOfVehicles)]
        public DateTime? Einsteuerung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Blocked)]
        public bool IstGesperrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.BlockRemark)]
        public string Sperrvermerk { get; set; }

      
    }
}
