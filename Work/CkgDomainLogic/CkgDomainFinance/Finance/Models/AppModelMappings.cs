using System;
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

        static public ModelMapping<Z_DPM_LIST_ZZAKTSPERRE.GT_WEB, VorgangVersandsperre> Z_DPM_LIST_ZZAKTSPERRE_GT_WEB_To_VorgangVersandsperre
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_LIST_ZZAKTSPERRE.GT_WEB, VorgangVersandsperre>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                        {
                            destination.PAID = source.LIZNR;
                            destination.Fahrgestellnummer = source.CHASSIS_NUM;
                            destination.Kennzeichen = source.LICENSE_NUM;
                            destination.Erstzulassung = source.REPLA_DATE;
                            destination.Vertragsart = source.ZVERT_ART;
                            destination.Kontonummer = source.KONTONR;
                            destination.CIN = source.CIN;
                        }));
            }
        }

        static public ModelMapping<Z_dpm_Read_Prot_Telefonate_01.GT_OUT, TelefoniedatenItem> Z_dpm_Read_Prot_Telefonate_01_GT_OUT_To_TelefoniedatenItem
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_dpm_Read_Prot_Telefonate_01.GT_OUT, TelefoniedatenItem>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                        {
                            destination.Kundennummer = source.KUNNR_AG;
                            destination.Kontonummer = source.KONTONR;
                            destination.CIN = source.CIN;
                            destination.PAID = source.PAID;
                            destination.Vertragsart = source.ZVERT_ART;
                            destination.Telefonnummer = source.TEL_NUMBER;
                            destination.Anrufart = source.ANRUFART;
                            destination.Anrufdatum = source.ANRUFDATUM;
                            destination.AnrufzeitVon = source.UZEIT_VON;
                            destination.AnrufzeitBis = source.UZEIT_BIS;
                            destination.Anrufername = source.NAME_ANRUFER;
                            destination.Anrufgrund = source.TEXT_ANRUFGRUND;
                            destination.AnrufgrundBemerkung = source.FREITEXT_GRUND;
                        }));
            }
        }

        static public ModelMapping<Z_DPM_READ_STL_MAHNUNGEN_01.GT_OUT, Mahnung> Z_DPM_READ_STL_MAHNUNGEN_01_GT_OUT_To_Mahnung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_STL_MAHNUNGEN_01.GT_OUT, Mahnung>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.Kontonummer = source.KONTONR;
                        destination.CIN = source.CIN;
                        destination.PAID = source.PAID;
                        destination.Vertragsart = source.ZVERT_ART;
                        destination.Fahrgestellnummer = source.CHASSIS_NUM;
                        destination.Materialtext = source.MAKTX;
                        destination.Mahnstufe = source.ZZMAHNS;
                        destination.LetzteMahnungAm = source.MAHNDAT;
                        destination.NaechsteMahnungAm = source.NEXT_MAHNDAT;
                        destination.Mahnsperre = (source.ZZMANSP.NotNullOrEmpty() == "X");
                        destination.MahnsperreBis = source.ZZMANSP_DATBI;
                        destination.Name1 = source.NAME1;
                        destination.Name2 = source.NAME2;
                        destination.Strasse = source.STREET;
                        destination.Hausnummer = source.HOUSE_NUM1;
                        destination.Postleitzahl = source.POST_CODE1;
                        destination.Ort = source.CITY1;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_READ_MAHN_EQSTL_02.GT_OUT, Mahnstop> Z_DPM_READ_MAHN_EQSTL_02_GT_OUT_To_Mahnstop
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_MAHN_EQSTL_02.GT_OUT, Mahnstop>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                        {
                            destination.EquiNr = source.EQUNR;
                            destination.MaterialNr = source.MATNR;
                            destination.PAID = source.CHASSIS_NUM;
                            destination.Kontonummer = source.KONTONR;
                            destination.Dokument = source.MAKTX;
                            destination.Mahnsperre = source.MAHNSP_GES_AM.HasValue;
                            destination.MahnstopBis = source.MAHNDATUM_AB;
                            destination.Bemerkung = source.BEM;
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

        static public ModelMapping<Z_DPM_ZZAKTSPERRE.GT_WEB, VorgangVersandsperre> Z_DPM_ZZAKTSPERRE_GT_WEB_From_VorgangVersandsperre
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_ZZAKTSPERRE.GT_WEB, VorgangVersandsperre>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                        {
                            destination.EQUNR = source.Equipmentnummer;
                            destination.LIZNR = source.PAID;
                        }));
            }
        }

        static public ModelMapping<Z_DPM_SAVE_MAHN_EQSTL_01.GT_IN, Mahnstop> Z_DPM_SAVE_MAHN_EQSTL_01_GT_IN_From_Mahnstop
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_SAVE_MAHN_EQSTL_01.GT_IN, Mahnstop>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                        {
                            destination.EQUNR = source.EquiNr;
                            destination.CHASSIS_NUM = source.PAID;
                            destination.MATNR = source.MaterialNr;
                            destination.MAHNSP_SETZEN = (source.Mahnsperre ? "X" : "");
                            destination.MAHNSP_ENTF = (source.Mahnsperre ? "" : "X");
                            destination.MAHNDATUM_AB = source.MahnstopBis;
                            destination.MAHNDATUM_AB_ENTF = (source.MahnstopBis.HasValue ? "" : "X");
                            destination.BEM = source.Bemerkung;
                            destination.BEM_ENTF = (String.IsNullOrEmpty(source.Bemerkung) ? "X" : "");
                            destination.CIN = source.CIN;
                            destination.KONTONR = source.Kontonummer;
                        }));
            }
        }

        #endregion
    }
}