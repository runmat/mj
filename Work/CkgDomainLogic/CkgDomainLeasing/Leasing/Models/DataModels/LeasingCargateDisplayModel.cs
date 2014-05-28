using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLeasing.Leasing.Models.DataModels
{
    public class LeasingCargateDisplayModel
    {
        /// <summary>
        /// Vorgangs-ID
        /// </summary>
        [LocalizedDisplay(LocalizeConstants.ProcessID)]
        public string VORGANGS_ID { get; set; }

        /// <summary>
        /// HLA-Vertragsnr.
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._VERTRAGSNR_HLA)]
        public string VERTRAGSNR_HLA { get; set; }
        
        /// <summary>
        /// FIN
        /// </summary>
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string CHASSIS_NUM { get; set; }
        
        /// <summary>
        /// Standort
        /// </summary>
        [LocalizedDisplay(LocalizeConstants.Location)]
        public string STANDORT { get; set; }

        /// <summary>
        /// Prozess HLAEasy/ HLALive
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._RUECKGAB_OPTION)]
        public string RUECKGAB_OPTION { get; set; }

        /// <summary>
        /// Beauftragung RN (HLA an DAD)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._ERDAT)]
        public DateTime? ERDAT { get; set; }

        /// <summary>
        /// Vereinbarung RN-Termin (DAD mit LN)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._BEST_ABH_TERMIN)]
        public DateTime? BEST_ABH_TERMIN { get; set; }

        /// <summary>
        /// RN Fahrzeug (DAD mit LN)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._IUG_DAT)]
        public DateTime? IUG_DAT { get; set; }

        /// <summary>
        /// ÜG Fahrzeug (DAD an BLG)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._FE_BLG)]
        public DateTime? FE_BLG { get; set; }

        /// <summary>
        /// Beauftragung GA (DAD an SGS)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._DAT_GUTA_BEAUFTRAGT)]
        public DateTime? DAT_GUTA_BEAUFTRAGT { get; set; }

        /// <summary>
        /// Bereitstellung ÜG-Protokoll (DAD an BLG / HLA)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._PROT_EING_D)]
        public DateTime? PROT_EING_D { get; set; }

        /// <summary>
        /// Bereitstellung ÜG-Protokoll (DAD an BLG / HLA)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._PROT_EING_O)]
        public DateTime? PROT_EING_O { get; set; }

        /// <summary>
        /// Abmeldung Fahrzeug (DAD)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._ABMELDEDATUM)]
        public DateTime? ABMELDEDATUM { get; set; }

        /// <summary>
        /// Bereitstellung Fahrzeug (BLG an SGS)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._FB_GUTA)]
        public DateTime? FB_GUTA { get; set; }

        /// <summary>
        /// VW-Entscheidung (HLA an DAD)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._VERW_ZULETZT_AKTUALISIERT)]
        public string VERW_ZULETZT_AKTUALISIERT { get; set; }

        /// <summary>
        /// Bereitstellung GA (SGS an DAD)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._DAT_GUTA_ERHALT)]
        public DateTime? DAT_GUTA_ERHALT { get; set; }

        /// <summary>
        /// Bereitstellung FZG-Daten & Fotos (DAD an Online- Vermarktungsplattform)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._DAT_INSERAT)]
        public DateTime? DAT_INSERAT { get; set; }

        /// <summary>
        /// Beauftragung Aufbereitung (HLA an BLG)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._EING_AUFBER_AUFTR)]
        public DateTime? EING_AUFBER_AUFTR { get; set; }

        /// <summary>
        /// Fertigmeldung Aufbereitung (BLG an DAD)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._AUFBER_FERTIG)]
        public DateTime? AUFBER_FERTIG { get; set; }

        /// <summary>
        /// Fahrzeug hat Logistikplatz verlassen (Ausgang Platz)
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._FZG_BEREIT_BLG_SGS)]
        public DateTime? FZG_BEREIT_BLG_SGS { get; set; }

        /// <summary>
        /// Service Level 2-3
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_2_3)]
        public string SERVICE_LEVEL_2_3 { get; set; }

        /// <summary>
        /// Service Level 5-7
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_5_7)]
        public string SERVICE_LEVEL_5_7 { get; set; }

        /// <summary>
        /// Service Level 7-14
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_7_14)]
        public string SERVICE_LEVEL_7_14 { get; set; }

        /// <summary>
        /// Service Level 8-16
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_8_16)]
        public string SERVICE_LEVEL_8_16 { get; set; }

        /// <summary>
        /// Service Level 2-7
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_2_7)]
        public string SERVICE_LEVEL_2_7 { get; set; }

        /// <summary>
        /// Service Level 19-21
        /// </summary>
        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_19_21)]
        public string SERVICE_LEVEL_19_21 { get; set; }
    }
}
