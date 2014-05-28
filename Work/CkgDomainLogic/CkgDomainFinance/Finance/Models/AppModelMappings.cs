using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Finance.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_DPM_AKTIVCHECK_READ.ET_TREFF, AktivcheckTreffer> Z_DPM_AKTIVCHECK_READ_ET_TREFF_To_AktivcheckTreffer
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_AKTIVCHECK_READ.ET_TREFF, AktivcheckTreffer>(
                    new Dictionary<string, string> {
                        { "VORGANGS_ID", "VorgangsID" },
                        { "FIN", "Fahrgestellnummer" },
                        { "ZB2", "ZB2" },
                        { "VERTRAGNR", "Vertragsnummer" },
                        { "ERDAT", "Erstelldatum" },
                        { "PRUEFDAT", "Pruefdatum" },
                        { "KOLLISION", "Kollision" },
                        { "KLASSIFIZIER", "Klassifizierung" },
                        { "Klassifizierungstext", "Klassifizierungstext" },
                        { "BEARBEITET", "Kontaktanfrage" },
                        { "TEXT", "Bemerkung" },
                        { "MANDT", "Mandant" },
                        { "AG", "Kundennummer" },
                        { "EQUNR", "EquiNummer" },
                        { "BESTAND_AG", "BestandsAg" },
                        { "AG_KOL", "Kundennummer_Kol" },
                        { "FIN_KOL", "Fahrgestellnummer_Kol" },
                        { "ZB2_KOL", "ZB2_Kol" },
                        { "VERTRAGNR_KOL", "Vertragsnummer_Kol" },
                    }));
            }
        }

        static public ModelMapping<Z_DPM_EXP_GBA_01.GT_OUT, Auftrag> Z_DPM_EXP_GBA_01_GT_OUT_To_Auftrag
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_EXP_GBA_01.GT_OUT, Auftrag>(
                    new Dictionary<string, string> {
                        { "VBELN", "Auftragsnummer" },
                        { "VDATU", "Wunschlieferdatum" },
                        { "ZVERT_ART", "Vertragsart" },
                        { "KONTONR_C", "Kontonummer" },
                        { "CIN_C", "CIN" },
                        { "PAID_C", "PAID" },
                        { "NAME_KRED", "Kreditnehmer" },
                        { "ZZBISDATUM", "DatumTempVersand" },
                        { "NAME_KRED_BEST", "KreditorAusBestellung" },
                        { "MAKTX", "GrundDerAuslage" },
                        { "DAT_RE_EING", "Rechnungseingangsdatum" },
                        { "NETWR_C", "Wert" },
                        { "GEBUEHREN_C", "Gebuehren" },
                        { "BEM_ZEILE1", "BemerkungZeile1" },
                        { "BEM_ZEILE2", "BemerkungZeile2" },
                        { "ZZPROTOKOLL", "Erledigt" },
                    }));
            }
        }

        static public ModelMapping<Z_DPM_VERTRAGSBESTAND_01.GT_OUT, VorgangInfo> Z_DPM_VERTRAGSBESTAND_01_GT_OUT_To_VorgangInfo
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_VERTRAGSBESTAND_01.GT_OUT, VorgangInfo>(
                    new Dictionary<string, string> {
                        { "KONTONR", "Kontonummer" },
                        { "CIN", "CIN" },
                        { "PAID", "PAID" },
                    }));
            }
        }

        #endregion


        #region ToSap

        static public ModelMapping<Z_DPM_AKTIVCHECK_CHANGE.IT_TREFF, AktivcheckTreffer> Z_DPM_AKTIVCHECK_CHANGE_IT_TREFF_From_AktivcheckTreffer
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_AKTIVCHECK_CHANGE.IT_TREFF, AktivcheckTreffer>(
                    new Dictionary<string, string> {
                        { "VORGANGS_ID", "VorgangsID" },
                        { "FIN", "Fahrgestellnummer" },
                        { "ZB2", "ZB2" },
                        { "VERTRAGNR", "Vertragsnummer" },
                        { "ERDAT", "Erstelldatum" },
                        { "PRUEFDAT", "Pruefdatum" },
                        { "KOLLISION", "Kollision" },
                        { "KLASSIFIZIER", "Klassifizierung" },
                        { "BEARBEITET", "Kontaktanfrage" },
                        { "TEXT", "Bemerkung" },
                        { "MANDT", "Mandant" },
                        { "AG", "Kundennummer" },
                        { "EQUNR", "EquiNummer" },
                        { "BESTAND_AG", "BestandsAg" },
                        { "AG_KOL", "Kundennummer_Kol" },
                        { "FIN_KOL", "Fahrgestellnummer_Kol" },
                        { "ZB2_KOL", "ZB2_Kol" },
                        { "VERTRAGNR_KOL", "Vertragsnummer_Kol" },
                    }));
            }
        }

        static public ModelMapping<Z_DPM_SAVE_GBA_01.GT_IN, Auftrag> Z_DPM_SAVE_GBA_01_GT_IN_From_Auftrag
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_SAVE_GBA_01.GT_IN, Auftrag>(
                    new Dictionary<string, string> {
                        { "VBELN", "Auftragsnummer" },
                        { "BEM_ZEILE1", "BemerkungZeile1" },
                        { "BEM_ZEILE2", "BemerkungZeile2" },
                        { "ZZPROTOKOLL", "Erledigt" },
                    }));
            }
        }

        static public ModelMapping<Z_DPM_SAVE_WERTE_GUTA_VERW_01.GT_IN, VorgangBewertung> Z_DPM_SAVE_WERTE_GUTA_VERW_01_GT_IN_From_VorgangBewertung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_SAVE_WERTE_GUTA_VERW_01.GT_IN, VorgangBewertung>(
                    new Dictionary<string, string> {
                        { "KONTONR", "Kontonummer" },
                        { "CIN", "CIN" },
                        { "PAID", "PAID" },
                        { "PRUEFDAT", "Pruefdatum" },
                        { "HAENDL_EINK_WERT_NET", "HaendlerNettoEkWert" },
                        { "HAENDL_VERK_WERT_NET", "HaendlerNettoVkWert" },
                        { "HAENDL_EINK_WERT_BRU", "HaendlerBruttoEkWert" },
                        { "HAENDL_VERK_WERT_BRU", "HaendlerBruttoVkWert" },
                        { "KOSTEN", "Kosten" },
                    }));
            }
        }

        #endregion
    }
}