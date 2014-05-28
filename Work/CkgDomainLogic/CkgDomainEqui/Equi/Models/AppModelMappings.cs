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

        #endregion


        #region Save to Repository

        

        #endregion
    }
}