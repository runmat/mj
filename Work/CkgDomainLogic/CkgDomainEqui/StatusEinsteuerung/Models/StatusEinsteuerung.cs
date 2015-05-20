using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class StatusEinsteuerung
    {
                            
        [LocalizedDisplay(LocalizeConstants.PDINumber)]
        public string PDINummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string PDIBezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]
        public string Sipp { get; set; }

        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        public string HerstellerCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Hersteller { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string ModellCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string Modellbezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string Fahrzeuggruppe { get; set; }

        [LocalizedDisplay(LocalizeConstants.Stock)]
        public int Bestand { get; set; }

        //[LocalizedDisplay(LocalizeConstants.Stock)]
        public int EingangGesamt { get; set; }

        //[LocalizedDisplay(LocalizeConstants.Stock)]
        public int AusVorjahr { get; set; }

        //[LocalizedDisplay(LocalizeConstants.Stock)]
        public int ZulassungVormonat { get; set; }

        //[LocalizedDisplay(LocalizeConstants.Stock)]
        public int ZulassungLfdMonat { get; set; }

        //[LocalizedDisplay(LocalizeConstants.Stock)]
        public int ZulassungGesamtMonat { get; set; }

        public int Zul_PZ_LFD_M { get; set; }

        public int ZUL_PZ_FM { get; set; }

        public int FZG_BEST { get; set; }

        //[LocalizedDisplay(LocalizeConstants.Stock)]
        public int Ausgerüstet { get; set; }

        //[LocalizedDisplay(LocalizeConstants.zb2)]
        public int MitBrief { get; set; }

        //[LocalizedDisplay(LocalizeConstants.Stock)]
        public int Zulassungsbereit { get; set; }

        //[LocalizedDisplay(LocalizeConstants.Stock)]
        public int OhneUnitnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleGroup)]
        public string FzgGruppe { get; set; }

        [LocalizedDisplay(LocalizeConstants.Blocked)]
        public bool Gesperrt { get; set; }        
    }
}
