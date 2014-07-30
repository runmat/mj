using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Equi.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_OUT, EquiGrunddaten> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_OUT_To_GrunddatenEqui
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_OUT, EquiGrunddaten>(
                    // MJE, 10.02.2014
                    // Performance optimization, avoid property mapping via reflection here, use inline mapping code instead
                    new Dictionary<string, string> ()
                    //{
                        //{ "FIN", "Fahrgestellnummer" },
                        //{ "FIN_10", "Fahrgestellnummer10" },
                        //{ "ERDAT", "Erfassungsdatum" },
                        //{ "LICENSE_NUM", "Kennzeichen" },
                        //{ "TIDNR", "TechnIdentnummer" },
                        //{ "REPLA_DATE", "Erstzulassungsdatum" },
                        //{ "EXPIRY_DATE", "Abmeldedatum" },
                        //{ "BETRIEB", "Betrieb" },
                        //{ "STORT", "Standort" },
                        //{ "STORT_TEXT", "StandortBez" },
                        //{ "ZZCOCKZ", "CocVorhanden" },
                        //{ "ZZEDCOC", "EingangCoc" },
                        //{ "ZZHANDELSNAME", "Handelsname" },
                        //{ "ZIELORT", "Zielort" },
                    //}
                    , (s, d) =>
                        {
                            d.Fahrgestellnummer = s.FIN;
                            d.Fahrgestellnummer10 = s.FIN_10;
                            d.Erfassungsdatum = s.ERDAT;
                            d.Kennzeichen = s.LICENSE_NUM;
                            d.TechnIdentnummer = s.TIDNR;
                            d.Erstzulassungsdatum = s.REPLA_DATE;
                            d.Abmeldedatum = s.EXPIRY_DATE;
                            d.Betrieb = s.BETRIEB;
                            d.Standort = s.STORT;
                            d.StandortBez = s.STORT_TEXT;
                            d.CocVorhanden = (s.ZZCOCKZ.NotNullOrEmpty().ToUpper() == "X");
                            d.EingangCoc = s.ZZEDCOC;
                            d.Handelsname = s.ZZHANDELSNAME;
                            d.Zielort = s.ZIELORT;
                        }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_ZIELORT, Zielort> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_ZIELORT_To_Zielort
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_ZIELORT, Zielort>(
                    new Dictionary<string, string> {
                        { "ZIELORT", "Id" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_BETRIEB, Betriebsnummer> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_BETRIEB_To_Betriebsnummer
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_BETRIEB, Betriebsnummer>(
                    new Dictionary<string, string> {
                        { "BETRIEB", "Id" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_FIN_17, Fahrgestellnummer> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_FIN_17_To_Fahrgestellnummer
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_FIN_17, Fahrgestellnummer>(
                    new Dictionary<string, string> {
                        { "FIN_17", "FIN" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_FIN_10, Fahrgestellnummer10> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_FIN_10_To_Fahrgestellnummer10
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_FIN_10, Fahrgestellnummer10>(
                    new Dictionary<string, string> {
                        { "FIN_10", "FIN" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_STORT, Standort> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_STORT_To_Standort
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_STORT, Standort>(
                    new Dictionary<string, string> {
                        { "STORT", "Id" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_WEB, EquiHistorie> Z_M_BRIEFLEBENSLAUF_001_GT_WEB_To_EquiHistorie
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_WEB, EquiHistorie>(
                    new Dictionary<string, string> {
                        { "EQUNR", "Equipmentnummer" },
                        { "ZZKENN", "Kennzeichen" },
                        { "ZZBRIEF", "Briefnummer" },
                        { "ZZKENN_OLD", "KennzeichenAlt" },
                        { "ZZBRIEF_OLD", "BriefnummerAlt" },
                        { "ZZBRIEF_A", "Briefaufbietung" },
                        { "ZZFAHRG", "Fahrgestellnummer" },
                        { "REPLA_DATE", "Erstzulassungsdatum" },
                        { "EXPIRY_DATE", "Abmeldedatum" },
                        { "ZZSTATUS_ZUB", "StatusZulassungsfaehig" },
                        { "ZZSTATUS_ZUL", "StatusZugelassen" },
                        { "ZZSTATUS_ABG", "StatusAbgemeldet" },
                        { "ZZSTATUS_BAG", "StatusBeiAbmeldung" },
                        { "ZZSTATUS_OZU", "StatusOhneZulassung" },
                        { "ZZAKTSPERRE", "StatusGesperrt" },
                        { "ZZCOCKZ", "CocVorhanden" },
                        { "NAME1_VS", "StandortName1" },
                        { "NAME2_VS", "StandortName2" },
                        { "STRAS_VS", "StandortStrasse" },
                        { "HSNR_VS", "StandortHausnummer" },
                        { "PSTLZ_VS", "StandortPlz" },
                        { "ORT01_VS", "StandortOrt" },
                        { "NAME1_ZH", "HalterName1" },
                        { "NAME2_ZH", "HalterName2" },
                        { "STRAS_ZH", "HalterStrasse" },
                        { "HSNR_ZH", "HalterHausnummer" },
                        { "PSTLZ_ZH", "HalterPlz" },
                        { "ORT01_ZH", "HalterOrt" },
                        { "ABCKZ", "AbcKennzeichen" },
                        { "ZZTMPDT", "Versanddatum" },
                        { "UDATE", "Ummeldedatum" },
                        { "ZZHERST_TEXT", "Hersteller" },
                        { "ZZHANDELSNAME", "Fahrzeugmodell" },
                        { "ZZHERSTELLER_SCH", "HerstellerSchluessel" },
                        { "ZZTYP_SCHL", "Typschluessel" },
                        { "ZZVVS_SCHLUESSEL", "VarianteVersion" },
                        { "ZZREFERENZ1", "Referenz1" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_QMEL, EquiMeldungsdaten> Z_M_BRIEFLEBENSLAUF_001_GT_QMEL_To_EquiMeldungsdaten
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_QMEL, EquiMeldungsdaten>(
                    new Dictionary<string, string> {
                        { "QMNUM", "Meldungsnummer" },
                        { "STRMN", "Durchfuehrungsdatum" },
                        { "ERDAT", "Erfassungsdatum" },
                        { "KURZTEXT", "Kurztext" },
                        { "QMNAM", "BeauftragtDurch" },
                        { "NAME1_Z5", "VersandName1" },
                        { "NAME2_Z5", "VersandName2" },
                        { "STREET_Z5", "VersandStrasse" },
                        { "HOUSE_NUM1_Z5", "VersandHausnummer" },
                        { "POST_CODE1_Z5", "VersandPlz" },
                        { "CITY1_Z5", "VersandOrt" },
                        { "ZZDIEN1", "Versandart" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_BRIEFBESTAND_001.GT_DATEN, Fahrzeugbrief> Z_M_BRIEFBESTAND_001_GT_DATEN_To_Fahrzeugbrief
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_BRIEFBESTAND_001.GT_DATEN, Fahrzeugbrief>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Equipmentnummer = s.EQUNR;
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.Vertragsnummer = s.LIZNR;
                        d.TechnIdentnummer = s.TIDNR;
                        d.AbcKennzeichen = s.ABCKZ;
                        d.Raum = s.MSGRP;
                        d.Standort = s.STORT;
                        d.Versandgrund = s.ZZVGRUND;
                        d.Eingangsdatum = s.DATAB;
                        d.Versanddatum = s.ZZTMPDT;
                        d.Stilllegungsdatum = s.EXPIRY_DATE;
                        d.Adresse = s.ADRNR;
                        d.Name1 = s.NAME1;
                        d.Name2 = s.NAME2;
                        d.Ort = s.CITY1;
                        d.PLZ = s.POST_CODE1;
                        d.Strasse = s.STREET;
                        d.Hausnummer = s.HOUSE_NUM1;
                        d.Pickdatum = s.PICKDAT;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_BRIEFBESTAND_001.GT_DATEN, Fahrzeugbrief> Z_DPM_BRIEFBESTAND_001_GT_DATEN_To_Fahrzeugbrief
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_BRIEFBESTAND_001.GT_DATEN, Fahrzeugbrief>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Equipmentnummer = s.EQUNR;
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.Vertragsnummer = s.LIZNR;
                        d.TechnIdentnummer = s.TIDNR;
                        d.AbcKennzeichen = s.ABCKZ;
                        d.Raum = s.MSGRP;
                        d.Standort = s.STORT;
                        d.Versandgrund = s.ZZVGRUND;
                        d.Eingangsdatum = s.DATAB;
                        d.Versanddatum = s.ZZTMPDT;
                        d.Stilllegungsdatum = s.EXPIRY_DATE;
                        d.Pickdatum = s.PICKDAT;
                        d.Referenz1 = s.ZZREFERENZ1;
                        d.Referenz2 = s.ZZREFERENZ2;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_READ_EQUI_MAHN_01.GT_OUT, EquiMahn> Z_DPM_READ_EQUI_MAHN_01_GT_OUT_To_EquiMahn
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_EQUI_MAHN_01.GT_OUT, EquiMahn>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.VertragsNr = s.LIZNR;
                            d.FahrgestellNr = s.CHASSIS_NUM;
                            d.Kennzeichen = s.LICENSE_NUM;
                            d.Erstzulassung = s.REPLA_DATE;
                            d.Versanddatum = s.ZZTMPDT;
                            d.UeberfaelligSeit = s.UEBERF_SEIT;
                            d.Mahnstufe = s.ZZMAHNS;
                            d.EmpfaengerName = s.NAME1_Z5;
                            d.EmpfaengerStrasse = s.STREET_Z5;
                            d.EmpfaengerPlz = s.POST_CODE1_Z5;
                            d.EmpfaengerOrt = s.CITY1_Z5;
                        }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_DAT_OHNE_DOKUMENT_01.GT_OUT, DatenOhneDokumente> Z_DPM_DAT_OHNE_DOKUMENT_01_GT_OUT_To_DatenOhneDokumente
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_DAT_OHNE_DOKUMENT_01.GT_OUT, DatenOhneDokumente>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Vertragsnummer = s.LIZNR;
                        d.Erfassungsdatum = s.ERDAT;
                        d.Vertragsbeginn = s.DAT_VERTR_BEG;
                        d.Vertragsende = s.DAT_VERTR_END;
                        d.Name1 = s.NAME1_ZL;
                        d.Name2 = s.NAME2_ZL;
                        d.Strasse = s.STREET_ZL;
                        d.Hausnummer = s.HOUSE_NUM1_ZL;
                        d.PLZ = s.POST_CODE1_ZL;
                        d.Ort = s.CITY1_ZL;
                        d.Vertragsstatus = s.VERTRAGS_STAT;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_BRIEFBESTAND_002.GT_DATEN, FahrzeugbriefErweitert> Z_DPM_BRIEFBESTAND_002_GT_DATEN_To_FahrzeugbriefErweitert
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_BRIEFBESTAND_002.GT_DATEN, FahrzeugbriefErweitert>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Equipmentnummer = s.EQUNR;
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.Vertragsnummer = s.LIZNR;
                        d.TechnIdentnummer = s.TIDNR;
                        d.AbcKennzeichen = s.ABCKZ;
                        d.Raum = s.MSGRP;
                        d.Standort = s.STORT;
                        d.Versandgrund = s.ZZVGRUND;
                        d.VersandgrundText = s.VERSGRU_TXT;
                        d.Eingangsdatum = s.DATAB;
                        d.Versanddatum = s.ZZTMPDT;
                        d.Stilllegungsdatum = s.EXPIRY_DATE;
                        d.Pickdatum = s.PICKDAT;
                        d.Referenz1 = s.ZZREFERENZ1;
                        d.Referenz2 = s.ZZREFERENZ2;
                        d.Name1 = s.NAME1_ZL;
                        d.Name2 = s.NAME2_ZL;
                        d.Ort = s.CITY1_ZL;
                        d.PLZ = s.POST_CODE1_ZL;
                        d.Strasse = s.STREET_ZL;
                        d.Hausnummer = s.HOUSE_NUM1_ZL;
                        d.VertragsBeginn = s.DAT_VERTR_BEG;
                        d.VertragsEnde = s.DAT_VERTR_END;
                        d.VertragsStatus = s.VERTRAGS_STAT;
                    }));
            }
        }

        #endregion


        #region Save to Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_MARK_DAT_OHNE_DOKUM_01.GT_DAT, DatenOhneDokumente> Z_DPM_MARK_DAT_OHNE_DOKUM_01_GT_DAT_From_DatenOhneDokumente
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_MARK_DAT_OHNE_DOKUM_01.GT_DAT, DatenOhneDokumente>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.CHASSIS_NUM = s.Fahrgestellnummer;
                        d.VERTRAGS_STAT = s.Vertragsstatus;
                        d.LOESCH = (s.Loeschkennzeichen ? "X" : "");
                    }));
            }
        }
        
        #endregion
    }
}