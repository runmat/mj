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

        static public ModelMapping<Z_DPM_READ_PROT_TELEFONATE_01.GT_OUT, TelefoniedatenItem> Z_dpm_Read_Prot_Telefonate_01_GT_OUT_To_TelefoniedatenItem
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_PROT_TELEFONATE_01.GT_OUT, TelefoniedatenItem>(
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

        static public ModelMapping<Z_DPM_READ_PRUEFPUNKTE_01.GT_OUT, Pruefpunkt> Z_DPM_READ_PRUEFPUNKTE_01_GT_OUT_To_Pruefpunkt
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_PRUEFPUNKTE_01.GT_OUT, Pruefpunkt>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.Kontonummer = source.KONTONR;
                        destination.PAID = source.PAID;
                        destination.BucID = source.BUC_ID;
                        destination.Aktionsname = source.AKTIONSNAME;
                        destination.PruefpunktID = source.PRUEFPUNKT;
                        destination.PruefpunktText = source.PRUEFPUNKT_TXT;
                        destination.Pruefstatus = source.PRUEFP_IO;
                        destination.Ergebnis = source.PRUEFP_IO_TEXT;
                        destination.Pruefdatum = source.PRUEDAT;
                        destination.Bemerkung = source.BEMERKUNG;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_READ_PRUEFSCHRITTE_03.GT_OUT, Pruefschritt> Z_DPM_READ_PRUEFSCHRITTE_03_GT_OUT_To_Pruefschritt
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_PRUEFSCHRITTE_03.GT_OUT, Pruefschritt>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.Kontonummer = source.KONTONR;
                        destination.PAID = source.PAID;
                        destination.BucID = source.BUC_ID;
                        destination.Aktionsnummer = source.AKTIONSNR;
                        destination.Aktionsart = source.AKTIONSART;
                        destination.Aktionsname = source.AKTIONSNAME;
                        destination.Aktionstext = source.AKTION_TEXT;
                        destination.Infotext = source.INFOTEXT;
                        destination.Erledigt = (source.ERLEDIGT.NotNullOrEmpty().ToUpper() == "X" || source.ERLEDIGT.NotNullOrEmpty().ToUpper() == "TRUE");
                        destination.Pruefdatum = source.PRUEFDAT;
                        destination.Webuser = source.WEB_USER;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_EXP_MAHN_ERSTEINGANG.GT_WEB, Mahnung> Z_DPM_EXP_MAHN_ERSTEINGANG_GT_WEB_To_Mahnung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_EXP_MAHN_ERSTEINGANG.GT_WEB, Mahnung>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.Kontonummer = source.KONTONR;
                        destination.CIN = source.CIN;
                        destination.PAID = source.PAID;
                        destination.Vertragsart = source.ZVERT_ART;
                        destination.Materialtext = source.MAKTX;
                        destination.Mahnstufe = source.ZZMAHNS;
                        destination.Mahnverfahren = source.ZZ_MAHNA;
                        destination.Mahnsperre = (source.MANSP.NotNullOrEmpty() == "X");
                        destination.MahnsperreBis = source.MASPB;
                        destination.LetzteMahnungAm = source.MADAT;
                        destination.NaechsteMahnungAm = source.MNDAT;
                    }));
            }
        }

        static public ModelMapping<Z_M_BRIEF_TEMP_VERS_MAHN_001.GT_WEB, TempZb2Versand> Z_DPM_EXP_MAHN_ERSTEINGANG_GT_WEB_To_TempZb2Versand
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_BRIEF_TEMP_VERS_MAHN_001.GT_WEB, TempZb2Versand>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {                        
                        destination.Kennzeichen = source.LICENSE_NUM;
                        destination.Fahrgestellnummer = source.CHASSIS_NUM;
                        destination.Zb2Nummer = source.TIDNR;
                        destination.Name = source.NAME1;
                        destination.Strasse = source.STREET;
                        destination.Postleitzahl = source.POST_CODE1;
                        destination.Ort = source.CITY1;
                        destination.Versandadresse = string.Concat(source.NAME1.AppendIfNotNull(", "), source.STREET.AppendIfNotNull(", "),
                            source.POST_CODE1.AppendIfNotNull(", "), source.CITY1); 
                        destination.Versanddatum = source.ZZTMPDT;
                        destination.Versandgrund = source.ZZVGRUND_TEXT;                       
                    }));
            }
        }

        static public ModelMapping<Z_M_SCHLUE_TEMP_VERS_MAHN_001.GT_WEB, TempVersandZweitschluessel> Z_M_SCHLUE_TEMP_VERS_MAHN_001_To_TempVersandZweitschluessel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_SCHLUE_TEMP_VERS_MAHN_001.GT_WEB, TempVersandZweitschluessel>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.Kennzeichen = source.LICENSE_NUM;
                        destination.Fahrgestellnummer = source.CHASSIS_NUM;
                        destination.Versandadresse = string.Concat(source.NAME1.AppendIfNotNull(", "), source.STREET.AppendIfNotNull(", "),
                            source.POST_CODE1.AppendIfNotNull(", "), source.CITY1);                       
                        destination.Versanddatum = source.ZZTMPDT;
                        destination.Vertragsnummer = source.EQUNR;                      
                    }));
            }
        }

        static public ModelMapping<Z_M_SCHLUESSELDIFFERENZEN.GT_WEB_OUT_BRIEFE, FehlendeSchluesseltuete> Z_M_SCHLUESSELDIFFERENZEN_To_FehlendeSchluesseltuete
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_SCHLUESSELDIFFERENZEN.GT_WEB_OUT_BRIEFE, FehlendeSchluesseltuete>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.Kennzeichen = source.LICENSE_NUM;
                        destination.Fahrgestellnummer = source.CHASSIS_NUM;                       
                        destination.Briefnummer = source.TIDNR;
                        destination.Erstzulassung = source.REPLA_DATE;
                        destination.PDINummer = source.KUNPDI.Replace("PDI", String.Empty);                        
                        destination.Hersteller = String.Empty;
                        destination.Vertragsnummer = source.EQUNR;                        
                    }));
            }
        }


        static public ModelMapping<Z_M_ABMBEREIT_LAUFZEIT.AUSGABE, CarporteingaengeOhneEH> Z_M_ABMBEREIT_LAUFZEIT_To_CarporteingaengeOhneEH
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_ABMBEREIT_LAUFZEIT.AUSGABE, CarporteingaengeOhneEH>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.Kennzeichen = source.ZZKENN;
                        destination.Fahrgestellnummer = source.ZZFAHRG;
                        destination.Modellbezeichnung = source.ZZHANDELSNAME;
                        destination.FruehesteAbmeldung = source.REPLA_DATE;
                        destination.Erstzulassung = source.VDATU;
                        destination.PDINummer = source.KUNPDI;
                        destination.Hersteller = source.ZZHERST_TEXT;
                        destination.Laufzeit = source.ZZLAUFZEIT;
                        destination.BelegNr = source.BELNR;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_EXP_VERS_AUSWERTUNG_01.GT_OUT, Versendung> Z_DPM_EXP_VERS_AUSWERTUNG_01_GT_OUT_To_Versendung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_EXP_VERS_AUSWERTUNG_01.GT_OUT, Versendung>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                        {
                            destination.Aenderungsdatum = source.DAT_GEAEND;
                            destination.AnfordererAnrede = source.ANREDE_ANF;
                            destination.AnfordererEmail = source.EMAIL_ANF;
                            destination.AnfordererFax1 = source.FAX1_ANF;
                            destination.AnfordererFax2 = source.FAX2_ANF;
                            destination.AnfordererHausnummer = source.HNR_ANF;
                            destination.AnfordererInfo = source.INFO_ANF;
                            destination.AnfordererLand = source.LAND_ANF;
                            destination.AnfordererMobil = source.MOBIL_ANF;
                            destination.AnfordererName = source.NAME_ANF;
                            destination.AnfordererOrt = source.ORT_ANF;
                            destination.AnfordererPostleitzahl = source.PLZ_ANF;
                            destination.AnfordererStrasse = source.STRASSE_ANF;
                            destination.AnfordererTelefon1 = source.TEL1_ANF;
                            destination.AnfordererTelefon2 = source.TEL2_ANF;
                            destination.AnfordererVorname = source.VORNAME_ANF;
                            destination.Anforderungsart = source.ANF_ART;
                            destination.Anforderungsweg = source.ANFORDERUNGSWEG;
                            destination.Anlagedatum = source.DAT_ANGEL;
                            destination.AnsprechpartnerAnrede = source.ANREDE_ANSP;
                            destination.AnsprechpartnerName = source.NAME_ANSP;
                            destination.AnsprechpartnerVorname = source.VORNAME_ANSP;
                            destination.AuftraggeberId = source.AUFTRAGGEBERID;
                            destination.AutorisierungsUser = source.USER_AUTOR;
                            destination.CIN = source.CIN;
                            destination.ClientLd = source.CLIENTLD.XToBool();
                            destination.EmpfaengerAnrede = source.ANREDE_EMPF;
                            destination.EmpfaengerArt = source.EMPF_ART;
                            destination.EmpfaengerHausnummer = source.HNR_EMPF;
                            destination.EmpfaengerLand = source.LAND_EMPF;
                            destination.EmpfaengerName = source.NAME_EMPF;
                            destination.EmpfaengerOrt = source.ORT_EMPF;
                            destination.EmpfaengerPostleitzahl = source.PLZ_EMPF;
                            destination.EmpfaengerStrasse = source.STRASSE_EMPF;
                            destination.EmpfaengerVorname = source.VORNAME_EMPF;
                            destination.EndgueltigerVersand = source.ENDG_VERS;
                            destination.Kontonummer = source.KONTONR;
                            destination.PAID = source.PAID;
                            destination.PicklistenFormular = source.ZZPLFOR;
                            destination.SicherheitsIdCMS = source.SICHERHEITSIDCMS;
                            destination.StornoVersand = source.VERS_STORNO.XToBool();
                            destination.Systemkennzeichen = source.SYSTEMKENNZ;
                            destination.Uebermittlungsdatum = source.DAT_UEBERM_1;
                            destination.Uebernahmedatum = source.DAT_VERSAUFTR;
                            destination.Versandart = source.VERS_ART;
                            destination.Versanddatum = source.ZZTMPDT;
                            destination.Versandgrund = source.VERS_GRUND;
                            destination.Vertragsart = source.ZVERT_ART;
                            destination.Zb2Nummer = source.ZBRIEF;
                            destination.HaendlerNr = source.KUNNR_BEIM_AG;
                            destination.HaendlerName = source.NAME;
                            destination.HaendlerOrt = source.CITY1;
                            destination.AnforderungsUhrzeit = source.UZEIT_ANGEL;
                            destination.StatusSicherheit = source.STATUS_SI;
                            destination.MahnstufeSicherheit = source.MAHNS_SI;
                        }));
            }
        }

        static public ModelMapping<Z_DPM_EXP_VERS_AUSWERTUNG_01.GT_OUT_SUM, VersendungSummiert> Z_DPM_EXP_VERS_AUSWERTUNG_01_GT_OUT_SUM_To_VersendungSummiert
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_EXP_VERS_AUSWERTUNG_01.GT_OUT_SUM, VersendungSummiert>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.HaendlerNr = source.KUNNR_BEIM_AG;
                        destination.HaendlerName = source.NAME;
                        destination.Vertragsart = source.ZVERT_ART;
                        destination.Anforderungsweg = source.ANFORDERUNGSWEG;
                        destination.Anforderungsart = source.ANF_ART;
                        destination.Versandgrund = source.VERS_GRUND;
                        destination.Summe = source.SUMME;
                        destination.BestandHaendler = source.BEST_HAEND;
                        destination.AnteilInProzent = source.PROZ_VERS;
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

        static public ModelMapping<Z_DPM_SAVE_ERL_PRUEFSCHR_01.GT_DAT, Pruefschritt> Z_DPM_SAVE_ERL_PRUEFSCHR_01_GT_DAT_From_Pruefschritt
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_SAVE_ERL_PRUEFSCHR_01.GT_DAT, Pruefschritt>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                    {
                        destination.KONTONR = source.Kontonummer;
                        destination.PAID = source.PAID;
                        destination.BUC_ID = source.BucID;
                        destination.AKTIONSNR = source.Aktionsnummer;
                        destination.ERLEDIGT = (source.Erledigt ? "X" : "");
                        destination.WEB_USER = source.Webuser;
                    }));
            }
        }



        /// <summary>
        /// Upload Fahrzeugeinsteuerung
        /// </summary>
        static public ModelMapping<Z_M_SCHLUESSELVERLOREN.GT_WEB_IN, FehlendeSchluesseltuete> Z_M_Schluesselverloren_GT_WEB_IN_From_FehlendeSchluesseltuete
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_SCHLUESSELVERLOREN.GT_WEB_IN, FehlendeSchluesseltuete>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                    {
                        destination.KUNNR = source.KUNNR;
                        destination.CHASSIS_NUM = source.Fahrgestellnummer;                      
                        destination.EQUNR = source.Vertragsnummer;
                        destination.FLAG = "X";                        
                    }
                ));
            }
        }


        #endregion
    }
}