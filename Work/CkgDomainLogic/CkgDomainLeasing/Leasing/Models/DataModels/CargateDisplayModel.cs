using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Leasing.Models
{
    public class CargateDisplayModel
    {
        [LocalizedDisplay(LocalizeConstants.ProcessID)]
        public string VorgangsId { get; set; }

        [LocalizedDisplay(LocalizeConstants._VERTRAGSNR_HLA)]
        public string VertragsNrHla { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FahrgestellNr { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Standort { get; set; }

        [LocalizedDisplay(LocalizeConstants._RUECKGAB_OPTION)]
        public string Prozess { get; set; }

        [LocalizedDisplay(LocalizeConstants._ERDAT)]
        public DateTime? BeauftragungRuecknahme { get; set; }

        [LocalizedDisplay(LocalizeConstants._WLIEFDAT_VON)]
        public DateTime? WunschLieferDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants._BEST_ABH_TERMIN)]
        public DateTime? VereinbarungRuecknahmeTermin { get; set; }

        [LocalizedDisplay(LocalizeConstants._IUG_DAT)]
        public DateTime? RuecknahmeFahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants._PROT_EING_O)]
        public DateTime? BereitstellungRuecknahmeProtokoll { get; set; }

        [LocalizedDisplay(LocalizeConstants._FE_BLG)]
        public DateTime? UebergabeFahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants._GUTA_ERSTELL_1)]
        public string GutachtenErstellen { get; set; }

        [LocalizedDisplay(LocalizeConstants._DAT_GUTA_BEAUFTRAGT)]
        public DateTime? BeauftragungGutachten { get; set; }

        [LocalizedDisplay(LocalizeConstants._PROT_EING_D)]
        public DateTime? BereitstellungUebergabeProtokoll { get; set; }

        [LocalizedDisplay(LocalizeConstants._ABMELDEDATUM)]
        public DateTime? AbmeldungFahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants._FB_GUTA)]
        public DateTime? BereitstellungFahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants._DAT_GUTA_ERHALT)]
        public DateTime? BereitstellungGutachten { get; set; }

        [LocalizedDisplay(LocalizeConstants._VERW_ZULETZT_AKTUALISIERT)]
        public string VerwertungsEntscheidung { get; set; }

        [LocalizedDisplay(LocalizeConstants._DAT_INSERAT)]
        public DateTime? BereitstellungInserat { get; set; }

        [LocalizedDisplay(LocalizeConstants._EING_AUFBER_AUFTR)]
        public DateTime? BeauftragungAufbereitung { get; set; }

        [LocalizedDisplay(LocalizeConstants._AUFBER_FERTIG)]
        public DateTime? AufbereitungFertig { get; set; }

        [LocalizedDisplay(LocalizeConstants._FZG_BEREIT_BLG_SGS)]
        public DateTime? AusgangPlatz { get; set; }

        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_2_3)]
        public string ServiceLevel2_3 { get; set; }

        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_5_7)]
        public string ServiceLevel5_7 { get; set; }

        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_7_14)]
        public string ServiceLevel7_14 { get; set; }

        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_8_16)]
        public string ServiceLevel8_16 { get; set; }

        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_2_7)]
        public string ServiceLevel2_7 { get; set; }

        [LocalizedDisplay(LocalizeConstants._SERVICE_LEVEL_19_21)]
        public string ServiceLevel19_21 { get; set; }

        public int Dauer_WunschLieferDatum_VereinbarungRuecknahmeTermin { get { return BenoetigteTageFuerProzessschritt(WunschLieferDatum, VereinbarungRuecknahmeTermin); } }

        public int Dauer_VereinbarungRuecknahmeTermin_RuecknahmeFahrzeug { get { return BenoetigteTageFuerProzessschritt(VereinbarungRuecknahmeTermin, RuecknahmeFahrzeug); } }

        public int Dauer_RuecknahmeFahrzeug_BereitstellungRuecknahmeProtokoll { get { return BenoetigteTageFuerProzessschritt(RuecknahmeFahrzeug, BereitstellungRuecknahmeProtokoll); } }

        public int Dauer_VereinbarungRuecknahmeTermin_UebergabeFahrzeug { get { return BenoetigteTageFuerProzessschritt(VereinbarungRuecknahmeTermin, UebergabeFahrzeug); } }

        public int Dauer_UebergabeFahrzeug_BeauftragungGutachten { get { return BenoetigteTageFuerProzessschritt(UebergabeFahrzeug, BeauftragungGutachten); } }

        public int Dauer_RuecknahmeFahrzeug_BereitstellungUebergabeProtokoll { get { return BenoetigteTageFuerProzessschritt(RuecknahmeFahrzeug, BereitstellungUebergabeProtokoll); } }

        public int Dauer_UebergabeFahrzeug_AbmeldungFahrzeug { get { return BenoetigteTageFuerProzessschritt(UebergabeFahrzeug, AbmeldungFahrzeug); } }

        public int Dauer_UebergabeFahrzeug_BereitstellungFahrzeug { get { return BenoetigteTageFuerProzessschritt(UebergabeFahrzeug, BereitstellungFahrzeug); } }

        public int Dauer_BereitstellungFahrzeug_BereitstellungGutachten { get { return BenoetigteTageFuerProzessschritt(BereitstellungFahrzeug, BereitstellungGutachten); } }

        private static int BenoetigteTageFuerProzessschritt(DateTime? schritt1, DateTime? schritt2)
        {
            if (!schritt1.HasValue || !schritt2.HasValue)
                return -1;

            return (int)(schritt2.Value - schritt1.Value).TotalDays;
        }
    }
}
