// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Equi.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_OUT, EquiGrunddaten> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_OUT_To_GrunddatenEqui
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_OUT, EquiGrunddaten>(
                    new Dictionary<string, string> ()
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

        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_ZIELORT, Zielort> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_ZIELORT_To_Zielort
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_ZIELORT, Zielort>(
                    new Dictionary<string, string> {
                        { "ZIELORT", "Id" },
                    }));
            }
        }

        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_BETRIEB, Betriebsnummer> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_BETRIEB_To_Betriebsnummer
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_BETRIEB, Betriebsnummer>(
                    new Dictionary<string, string> {
                        { "BETRIEB", "Id" },
                    }));
            }
        }

        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_FIN_17, Fahrgestellnummer> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_FIN_17_To_Fahrgestellnummer
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_FIN_17, Fahrgestellnummer>(
                    new Dictionary<string, string> {
                        { "FIN_17", "FIN" },
                    }));
            }
        }

        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_FIN_10, Fahrgestellnummer10> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_FIN_10_To_Fahrgestellnummer10
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_FIN_10, Fahrgestellnummer10>(
                    new Dictionary<string, string> {
                        { "FIN_10", "FIN" },
                    }));
            }
        }

        static public ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_STORT, Standort> Z_DPM_CD_READ_GRUEQUIDAT_02_GT_STORT_To_Standort
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_READ_GRUEQUIDAT_02.GT_STORT, Standort>(
                    new Dictionary<string, string> {
                        { "STORT", "Id" },
                    }));
            }
        }

        static public ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_WEB, EquiHistorie> Z_M_BRIEFLEBENSLAUF_001_GT_WEB_To_EquiHistorie
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_WEB, EquiHistorie>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.AbcKennzeichen = s.ABCKZ;
                            d.Abmeldedatum = s.EXPIRY_DATE;
                            d.BeideKennzeichenVorhanden = s.SCHILDER_PHY.XToBool();
                            d.Briefaufbietung = s.ZZBRIEF_A.XToBool();
                            d.Briefnummer = s.ZZBRIEF;
                            d.BriefnummerAlt = s.ZZBRIEF_OLD;
                            d.CarportEingang = s.ZZCARPORT_EING;
                            d.CheckInAbmeldeauftrag = s.CHECK_IN;
                            d.CocVorhanden = s.ZZCOCKZ.XToBool();
                            d.Equipmentnummer = s.EQUNR;
                            d.Erstzulassungsdatum = s.REPLA_DATE;
                            d.Fahrgestellnummer = s.ZZFAHRG;
                            d.Fahrzeugmodell = s.ZZHANDELSNAME;
                            d.Fahrzeugschein = s.SCHEIN_PHY.XToBool();
                            d.Finanzierungsart = s.ZZFINART_TXT;
                            d.HaendlerNr = s.KUNNR_ZF;
                            d.HalterHausnummer = s.HSNR_ZH;
                            d.HalterName1 = s.NAME1_ZH;
                            d.HalterName2 = s.NAME2_ZH;
                            d.HalterOrt = s.ORT01_ZH;
                            d.HalterPlz = s.PSTLZ_ZH;
                            d.HalterStrasse = s.STRAS_ZH;
                            d.Hersteller = s.ZZHERST_TEXT;
                            d.HerstellerSchluessel = s.ZZHERSTELLER_SCH;
                            d.Kennzeichen = s.ZZKENN;
                            d.KennzeichenAlt = s.ZZKENN_OLD;
                            d.KennzeichenEingang = s.ZZKENN_EING;
                            d.Lagerort = s.STANDORT_VERSSTAT_TEXT;
                            d.Ordernummer = s.ZZREF1;
                            d.Referenz1 = s.ZZREFERENZ1;
                            d.Referenz2 = s.ZZREFERENZ2;
                            d.StandortHausnummer = s.HSNR_VS;
                            d.StandortName1 = s.NAME1_VS;
                            d.StandortName2 = s.NAME2_VS;
                            d.StandortOrt = s.ORT01_VS;
                            d.StandortPlz = s.PSTLZ_VS;
                            d.StandortStrasse = s.STRAS_VS;
                            d.StatusAbgemeldet = s.ZZSTATUS_ABG.XToBool();
                            d.StatusBeiAbmeldung = s.ZZSTATUS_BAG.XToBool();
                            d.StatusGesperrt = s.ZZAKTSPERRE.XToBool();
                            d.StatusOhneZulassung = s.ZZSTATUS_OZU.XToBool();
                            d.StatusZugelassen = s.ZZSTATUS_ZUL.XToBool();
                            d.StatusZulassungsfaehig = s.ZZSTATUS_ZUB.XToBool();
                            d.Stilllegung = s.EXPIRY_DATE;
                            d.TypSchluessel = s.ZZTYP_SCHL;
                            d.Ummeldedatum = s.UDATE;
                            d.VarianteVersion = s.ZZVVS_SCHLUESSEL;
                            d.Versanddatum = s.ZZTMPDT;
                            d.VersandgrundId = s.ZZVGRUND;
                        }));
            }
        }

        static public ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_QMEL, EquiMeldungsdaten> Z_M_BRIEFLEBENSLAUF_001_GT_QMEL_To_EquiMeldungsdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_QMEL, EquiMeldungsdaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.BeauftragtDurch = s.QMNAM;
                            d.Durchfuehrungsdatum = s.STRMN;
                            d.Erfassungsdatum = s.ERDAT;
                            d.Vorgang = s.KURZTEXT;
                            d.Meldungsnummer = s.QMNUM;
                            d.Hausnummer = s.HOUSE_NUM1_Z5;
                            d.Name1 = s.NAME1_Z5;
                            d.Name2 = s.NAME2_Z5;
                            d.Ort = s.CITY1_Z5;
                            d.Plz = s.POST_CODE1_Z5;
                            d.Strasse = s.STREET_Z5;
                            d.Versandart = s.ZZDIEN1;
                        }));
            }
        }

        static public ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_QMMA, EquiAktionsdaten> Z_M_BRIEFLEBENSLAUF_001_GT_QMMA_To_EquiAktionsdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_QMMA, EquiAktionsdaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.Aenderungszeit = (String.IsNullOrEmpty(s.AEZEIT) ? "" : String.Format("{0}:{1}", s.AEZEIT.Substring(0, 2), s.AEZEIT.Substring(2, 2)));
                            d.Aktionscode = s.MNCOD;
                            d.Meldungsnummer = s.QMNUM;
                            d.Statusdatum = s.PSTER;
                            d.Uebermittlungsdatum = s.ZZUEBER;
                            d.Vorgang = s.MATXT;
                        }));
            }
        }

        static public ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_ADDR, EquiHaendlerdaten> Z_M_BRIEFLEBENSLAUF_001_GT_ADDR_To_EquiHaendlerdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_ADDR, EquiHaendlerdaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.Hausnummer = s.HOUSE_NUM1;
                            d.Land = s.COUNTRY;
                            d.Name1 = s.NAME1;
                            d.Name2 = s.NAME2;
                            d.Name3 = s.NAME3;
                            d.Ort = s.CITY1;
                            d.Plz = s.POST_CODE1;
                            d.Strasse = s.STREET;
                        }));
            }
        }

        static public ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_EQUI, EquiHistorieInfo> Z_M_BRIEFLEBENSLAUF_001_GT_EQUI_To_EquiHistorieInfo
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_BRIEFLEBENSLAUF_001.GT_EQUI, EquiHistorieInfo>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.AbcKennzeichen = s.ABCKZ;
                            d.Anlagedatum = s.ERDAT;
                            d.Briefnummer = s.TIDNR;
                            d.Equipmentnummer = s.EQUNR;
                            d.Fahrgestellnummer = s.CHASSIS_NUM;
                            d.Kennzeichen = s.LICENSE_NUM;
                            d.Partnernummer = s.EX_KUNNR;
                            d.Referenznummer = s.ZZREFERENZ1;
                            d.Versanddatum = s.ZZTMPDT;
                            d.Vertragsnummer = s.LIZNR;
                        }));
            }
        }

        static public ModelMapping<Z_M_BRIEFBESTAND_001.GT_DATEN, Fahrzeugbrief> Z_M_BRIEFBESTAND_001_GT_DATEN_To_Fahrzeugbrief
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

        static public ModelMapping<Z_DPM_BRIEFBESTAND_001.GT_DATEN, Fahrzeugbrief> Z_DPM_BRIEFBESTAND_001_GT_DATEN_To_Fahrzeugbrief
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
                        d.Standort = s.TEXT_STO;
                        d.Versandgrund = s.ZZVGRUND;
                        d.Eingangsdatum = s.DATAB;
                        d.Versanddatum = s.ZZTMPDT;
                        d.Stilllegungsdatum = s.EXPIRY_DATE;
                        d.Pickdatum = s.PICKDAT;
                        d.Referenz1 = s.ZZREFERENZ1;
                        d.Referenz2 = s.ZZREFERENZ2;
                        d.Erstzulassungsdatum = s.REPLA_DATE;
                        d.Ummeldedatum = s.ZZZLDAT;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_READ_EQUI_MAHN_01.GT_OUT, EquiMahn> Z_DPM_READ_EQUI_MAHN_01_GT_OUT_To_EquiMahn
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

        static public ModelMapping<Z_DPM_DAT_OHNE_DOKUMENT_01.GT_OUT, DatenOhneDokumente> Z_DPM_DAT_OHNE_DOKUMENT_01_GT_OUT_To_DatenOhneDokumente
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

        static public ModelMapping<Z_DPM_BRIEFBESTAND_002.GT_DATEN, FahrzeugbriefErweitert> Z_DPM_BRIEFBESTAND_002_GT_DATEN_To_FahrzeugbriefErweitert
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

        static public ModelMapping<Z_DPM_DAT_MIT_ABW_ZH_01.GT_OUT, Halterabweichung> Z_DPM_DAT_MIT_ABW_ZH_01_GT_OUT_To_Halterabweichung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_DAT_MIT_ABW_ZH_01.GT_OUT, Halterabweichung>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Vertragsnummer = s.LIZNR;
                        d.Vertragspartner_Name1 = s.NAME1_ZL;
                        d.Vertragspartner_Name2 = s.NAME2_ZL;
                        d.Vertragspartner_Strasse = s.STREET_ZL;
                        d.Vertragspartner_Hausnummer = s.HOUSE_NUM1_ZL;
                        d.Vertragspartner_PLZ = s.POST_CODE1_ZL;
                        d.Vertragspartner_Ort = s.CITY1_ZL;
                        d.Halter_Name1 = s.NAME1_ZH;
                        d.Halter_Name2 = s.NAME2_ZH;
                        d.Halter_Strasse = s.STREET_ZH;
                        d.Halter_Hausnummer = s.HOUSE_NUM1_ZH;
                        d.Halter_PLZ = s.POST_CODE1_ZH;
                        d.Halter_Ort = s.CITY1_ZH;
                        d.Vertragsbeginn = s.DAT_VERTR_BEG;
                        d.Vertragsende = s.DAT_VERTR_END;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_DOKUMENT_OHNE_DAT_01.GT_OUT, DokumentOhneDaten> Z_DPM_DOKUMENT_OHNE_DAT_01_GT_OUT_To_DokumentOhneDaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_DOKUMENT_OHNE_DAT_01.GT_OUT, DokumentOhneDaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Eingangsdatum = s.DATAB;
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.ZB2 = s.TIDNR;
                        d.Name1 = s.NAME1_ZH;
                        d.Name2 = s.NAME2_ZH;
                        d.Strasse = s.STREET_ZH;
                        d.Hausnummer = s.HOUSE_NUM1_ZH;
                        d.PLZ = s.POST_CODE1_ZH;
                        d.Ort = s.CITY1_ZH;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_READ_TEMP_VERS_EQUI_01.GT_WEB, EquiMahnsperre> Z_DPM_READ_TEMP_VERS_EQUI_01_GT_WEB_To_EquiMahnsperre
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_TEMP_VERS_EQUI_01.GT_WEB, EquiMahnsperre>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.EquiNr = s.EQUNR;
                        d.FahrgestellNr = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.VertragsNr = s.LIZNR;
                        d.BriefNr = s.TIDNR;
                        d.Versanddatum = s.ZZTMPDT;
                        d.Name1 = s.NAME1;
                        d.Name2 = s.NAME2;
                        d.Strasse = s.STREET;
                        d.Hausnummer = s.HOUSE_NUM1;
                        d.PLZ = s.POST_CODE1;
                        d.Ort = s.CITY1;
                        d.Mahnsperre = (s.ZZMANSP.NotNullOrEmpty().ToUpper() == "X");
                        d.MahnsperreBis = s.ZZMANSP_DATBI;
                        d.KomponentenID = s.IDNRK;
                        d.Komponente = s.MAKTX;
                        d.StuecklistenPosKnotenNr = s.STLKN;
                        d.VersandID = s.VERS_ID;
                        d.Kontonummer = s.KONTONR;
                        d.CIN = s.CIN;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_VHC_ZBII_BESTAND_001.GT_WEB, FahrzeugbriefVhc> Z_M_VHC_ZBII_BESTAND_001_GT_WEB_To_FahrzeugbriefVhc
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_VHC_ZBII_BESTAND_001.GT_WEB, FahrzeugbriefVhc>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.HaendlerNr = s.ZZHAENR;
                        d.FahrgestellNr = s.ZZFAHRG;
                        d.Anlagedatum = s.ERDAT;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_VHC_KLAERFAELLE_001.GT_WEB, KlaerfallVhc> Z_M_VHC_KLAERFAELLE_001_GT_WEB_To_KlaerfallVhc
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_VHC_KLAERFAELLE_001.GT_WEB, KlaerfallVhc>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.HaendlerNr = s.ZZHAENR;
                        d.FahrgestellNr = s.ZZFAHRG;
                        d.VertragsNr = s.ZZORDER;
                        d.Anlagedatum = s.ERDAT;
                        d.AnlagedatumKlaerfall = s.ANLAGEDAT_KLAERF;
                        d.EingangsdatumZb2 = s.ZB2EINGDAT;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_FFD_DATEN_OHNE_DOKUMENTE.GT_WEB, KlaerfallVhc> Z_DPM_FFD_DATEN_OHNE_DOKUMENTE_GT_WEB_To_KlaerfallVhc
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FFD_DATEN_OHNE_DOKUMENTE.GT_WEB, KlaerfallVhc>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.HaendlerNr = s.ZZHAENR;
                        d.FahrgestellNr = s.ZZFAHRG;
                        d.VertragsNr = s.ZZVTGNR;
                        d.Anlagedatum = s.ERDAT;
                        d.Plancode = s.PLANCODE;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_ABWEICH_ABRUFGRUND_02.GT_OUT, Fahrzeugbrief> Z_DPM_ABWEICH_ABRUFGRUND_02_GT_OUT_To_Fahrzeugbrief
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_ABWEICH_ABRUFGRUND_02.GT_OUT, Fahrzeugbrief>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Vertragsnummer = s.LIZNR;
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Equipmentnummer = s.EQUNR;
                        d.Memo = s.MEMO;
                        d.Eingangsdatum = s.DATEIN;
                        d.Versanddatum = s.DATAUS;
                        d.Kennzeichen = s.LICENSE_NUM;

                        d.HalterAdresse = new[] { s.ZH_ALT_NAME1, s.ZH_ALT_NAME2, s.ZH_ALT_NAME3, s.ZH_ALT_STREET, string.Format("{0} {1}", s.ZH_ALT_POST_CODE1, s.ZH_ALT_CITY1) }
                            .JoinIfNotNull(", ");
                    }));
            }
        }

        static public ModelMapping<Z_M_VERSAUFTR_FEHLERHAFTE.GT_WEB, Fahrzeugbrief> Z_M_VERSAUFTR_FEHLERHAFTE_GT_WEB_To_Fahrzeugbrief
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_VERSAUFTR_FEHLERHAFTE.GT_WEB, Fahrzeugbrief>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Vertragsnummer = s.LIZNR;
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;

                        d.BriefVersand = (s.ZZBRFVERS.IsNotNullOrEmpty() && s.ZZBRFVERS.NotNullOrEmpty() != "0");
                        d.SchluesselVersand = (s.ZZSCHLVERS.IsNotNullOrEmpty() && s.ZZSCHLVERS.NotNullOrEmpty() != "0");

                        d.VersandAdresse = new[] { s.ZZNAME1_ZS, s.ZZNAME2_ZS, s.ZZSTRAS_ZS, string.Format("{0} {1}", s.ZZPSTLZ_ZS, s.ZZORT01_ZS) }
                            .JoinIfNotNull(", ");
                    }));
            }
        }

        static public ModelMapping<Z_DAD_DATEN_EINAUS_REPORT_002.EINNEU, Fahrzeugbrief> Z_DAD_DATEN_EINAUS_REPORT_002_EINNEU_To_Fahrzeugbrief
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DAD_DATEN_EINAUS_REPORT_002.EINNEU, Fahrzeugbrief>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.FahrzeugHersteller = s.ZZHERST_TEXT;
                        d.FahrzeugTyp = s.ZZHANDELSNAME;
                        d.Vertragsnummer = s.LIZNR;

                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.TechnIdentnummer = s.TIDNR;
                        d.Kennzeichen = s.ZZKENN;
                        d.Eingangsdatum = s.ERDAT;

                        d.Versanddatum = s.ZZTMPDT;
                        d.VersendetVon = s.FREIGEBER;
                        d.Versandgrund = s.TEXT50;

                        d.Name1 = s.NAME1;
                        d.Name2 = s.NAME2;
                        d.Strasse = s.STREET;
                        d.Hausnummer = s.HOUSE_NUM1;
                        d.PLZ = s.POST_CODE1;
                        d.Ort = s.CITY1;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EQUIS, EquiHistorieInfoVermieter> Z_DPM_FAHRZEUGHISTORIE_AVM_GT_EQUIS_To_EquiHistorieInfoVermieter
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EQUIS, EquiHistorieInfoVermieter>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.BriefNr = s.TIDNR;
                        d.EquipmentNr = s.EQUNR;
                        d.FahrgestellNr = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.MeldungsNr = s.QMNUM;
                        d.Referenz1 = s.ZZREFERENZ1;
                        d.VertragsNr = s.LIZNR;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_UEBER, EquiHistorieVermieter> Z_DPM_FAHRZEUGHISTORIE_AVM_GT_UEBER_To_EquiHistorieVermieter
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_UEBER, EquiHistorieVermieter>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Abmeldedatum = s.EXPIRY_DATE;
                        d.BriefNr = s.TIDNR;
                        d.Erstzulassungsdatum = s.REPLA_DATE;
                        d.Farbcode = s.ZFARBE;
                        d.Farbe = s.ZFARBTXT;
                        d.Hersteller = s.ZZHERST_TEXT;
                        d.Modell = s.ZZMODELL;
                        d.Referenz1 = s.ZZREFERENZ1;
                        d.Referenz2 = s.ZZREFERENZ2;
                        d.VertragsNr = s.LIZNR;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EINST, EquiEinsteuerung> Z_DPM_FAHRZEUGHISTORIE_AVM_GT_EINST_To_EquiEinsteuerung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EINST, EquiEinsteuerung>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.AbsenderZb2Hausnummer = s.HOUSE_NUM1_ZP;
                        d.AbsenderZb2Name1 = s.NAME1_ZP;
                        d.AbsenderZb2Name2 = s.NAME2_ZP;
                        d.AbsenderZb2Ort = s.CITY1_ZP;
                        d.AbsenderZb2Plz = s.POST_CODE1_ZP;
                        d.AbsenderZb2Strasse = s.STREET_ZP;
                        d.BeauftragungZulassung = s.QMDAT;
                        d.Bemerkung = s.ZBEMERKUNG;
                        d.HalterHausnummer = s.HOUSE_NUM1_ZH;
                        d.HalterName1 = s.NAME1_ZH;
                        d.HalterName2 = s.NAME2_ZH;
                        d.HalterOrt = s.CITY1_ZH;
                        d.HalterPlz = s.POST_CODE1_ZH;
                        d.HalterStrasse = s.STREET_ZH;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.ModellId = s.ZZMODELL;
                        d.ModellName = s.ZZBEZEI;
                        d.PdiBereit = s.ZZDAT_BER;
                        d.PdiEingang = s.ZZDAT_EIN;
                        d.PdiName = s.PDINAME;
                        d.PdiNr = s.KUNPDI;
                        d.PdiOrt = s.PDIORT;
                        d.SchluesselEingang = s.EINGANG_EQT;
                        d.Zb2Eingang = s.EINGANG_ZB2;
                        d.Zulassungsdatum = s.REPLA_DATE;
                        d.Zulassungssperre = s.ZZAKTSPERRE.XToBool();
                    }));
            }
        }

        static public ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_AUSST, EquiAussteuerung> Z_DPM_FAHRZEUGHISTORIE_AVM_GT_AUSST_To_EquiAussteuerung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_AUSST, EquiAussteuerung>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Abmeldebeauftragung = s.ERDAT_ABMK;
                        d.Abmeldedatum = s.EXPIRY_DATE;
                        d.Abmeldedurchfuehrung = s.DATUM_ABMZ;
                        d.AnzahlKennzeichenVorhanden = s.PSCHILD;
                        d.CarportId = s.CARPORT;
                        d.CarportName = s.PDINAME;
                        d.Mindesthaltedatum = s.ZZABMDAT;
                        d.Unfallfahrzeug = s.UNFALLFZG.XToBool();
                        d.Zb1Vorhanden = s.PSCHEIN.XToBool();
                    }));
            }
        }

        static public ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TYPEN, EquiTypdaten> Z_DPM_FAHRZEUGHISTORIE_AVM_GT_TYPEN_To_EquiTypdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TYPEN, EquiTypdaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Abgasrichtlinie = s.ZZABGASRICHTL_TG;
                        d.AnzahlAchsen = s.ZZANZACHS.TrimStart('0');
                        d.AnzahlAntriebsachsen = s.ZZANTRIEBSACHS.TrimStart('0');
                        d.AnzahlSitze = s.ZZANZSITZE.TrimStart('0');
                        d.Aufbauart = s.ZZTEXT_AUFBAU;
                        d.Bemerkungen = String.Join(Environment.NewLine,
                            s.ZZBEMER1,
                            s.ZZBEMER2,
                            s.ZZBEMER3,
                            s.ZZBEMER4,
                            s.ZZBEMER5,
                            s.ZZBEMER6,
                            s.ZZBEMER7,
                            s.ZZBEMER8,
                            s.ZZBEMER9,
                            s.ZZBEMER10,
                            s.ZZBEMER11,
                            s.ZZBEMER12,
                            s.ZZBEMER13,
                            s.ZZBEMER14).TrimEnd('\r', '\n');
                        d.BereifungAchse1 = s.ZZBEREIFACHSE1;
                        d.BereifungAchse2 = s.ZZBEREIFACHSE2;
                        d.BereifungAchse3 = s.ZZBEREIFACHSE3;
                        d.Breite = s.ZZBREITEMIN.TrimStart('0');
                        d.Co2Emission = s.ZZCO2KOMBI;
                        d.Fabrikname = s.ZZFABRIKNAME;
                        d.Fahrgeraeusch = s.ZZFAHRGERAEUSCH.TrimStart('0');
                        d.Fahrzeugklasse = s.ZZFHRZKLASSE_TXT;
                        d.FassungsvermoegenTank = s.ZZFASSVERMOEGEN;
                        d.GenehmigungsNr = s.ZZGENEHMIGNR;
                        d.Genehmigungsdatum = s.ZZGENEHMIGDAT.ToNullableDateTime();
                        d.Handelsname = s.ZZHANDELSNAME;
                        d.HerstSchluessel = s.ZZHERSTELLER_SCH;
                        d.Hersteller = s.ZZHERST_TEXT;
                        d.Hoechstgeschwindigkeit = s.ZZHOECHSTGESCHW;
                        d.Hoehe = s.ZZHOEHEMIN.TrimStart('0');
                        d.Hubraum = s.ZZHUBRAUM.TrimStart('0');
                        d.Kraftstoffart = s.ZZKRAFTSTOFF_TXT;
                        d.Kraftstoffcode = s.ZZCODE_KRAFTSTOF;
                        d.Laenge = s.ZZLAENGEMIN.TrimStart('0');
                        d.Leistung = s.ZZNENNLEISTUNG.TrimStart('0');
                        d.MaxAchslastAchse1 = s.ZZACHSL_A1_STA.TrimStart('0');
                        d.MaxAchslastAchse2 = s.ZZACHSL_A2_STA.TrimStart('0');
                        d.MaxAchslastAchse3 = s.ZZACHSL_A3_STA.TrimStart('0');
                        d.NationaleEmissionsklasseCode = s.ZZSLD;
                        d.NationaleEmissionsklasse = s.ZZNATIONALE_EMIK;
                        d.Standgeraeusch = s.ZZSTANDGERAEUSCH.TrimStart('0');
                        d.Typ = s.ZZKLARTEXT_TYP;
                        d.TypSchluessel = s.ZZTYP_SCHL;
                        d.UmdrehungenProMin = s.ZZBEIUMDREH.TrimStart('0');
                        d.Variante = s.ZZVARIANTE;
                        d.Version = s.ZZVERSION;
                        d.ZulGesamtgewicht = s.ZZZULGESGEW.TrimStart('0');
                    }));
            }
        }

        static public ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_LLZB2, EquiMeldungsdaten> Z_DPM_FAHRZEUGHISTORIE_AVM_GT_LLZB2_To_EquiMeldungsdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_LLZB2, EquiMeldungsdaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.BeauftragtDurch = s.QMNAM;
                        d.Durchfuehrungsdatum = s.STRMN;
                        d.Erfassungsdatum = s.ERDAT;
                        d.Vorgang = s.KURZTEXT;
                        d.Meldungsnummer = s.QMNUM;
                        d.Hausnummer = s.HOUSE_NUM1_Z5;
                        d.Name1 = s.NAME1_Z5;
                        d.Name2 = s.NAME2_Z5;
                        d.Ort = s.CITY1_Z5;
                        d.Plz = s.POST_CODE1_Z5;
                        d.Strasse = s.STREET_Z5;
                        d.Versandart = s.ZZDIEN1;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_LLSCH, EquiMeldungsdaten> Z_DPM_FAHRZEUGHISTORIE_AVM_GT_LLSCH_To_EquiMeldungsdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_LLSCH, EquiMeldungsdaten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.BeauftragtDurch = s.QMNAM;
                        d.Durchfuehrungsdatum = s.STRMN;
                        d.Erfassungsdatum = s.ERDAT;
                        d.Vorgang = s.KURZTEXT;
                        d.Meldungsnummer = s.QMNUM;
                        d.Hausnummer = s.HOUSE_NUM1_Z5;
                        d.Name1 = s.NAME1_Z5;
                        d.Name2 = s.NAME2_Z5;
                        d.Ort = s.CITY1_Z5;
                        d.Plz = s.POST_CODE1_Z5;
                        d.Strasse = s.STREET_Z5;
                        d.Versandart = s.ZZDIEN1;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TUETE, EquiTueteninhalt> Z_DPM_FAHRZEUGHISTORIE_AVM_GT_TUETE_To_EquiTueteninhalt
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TUETE, EquiTueteninhalt>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Anzahl = s.ANZAHL.ToNullableInt();
                        d.Bezeichnung = s.ATBEZ;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_READ_EQUI_STL_01.GT_OUT, StuecklistenKomponente> Z_DPM_READ_EQUI_STL_01_GT_OUT_To_StuecklistenKomponente
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_EQUI_STL_01.GT_OUT, StuecklistenKomponente>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;
                        d.Equipmentnummer = s.EQUNR;
                        d.Nr = s.IDNRK;
                        d.Bezeichnung = s.MAKTX;
                        d.ErsatzteilKennung = s.ERSKZ;
                        d.ErsatzteilKennungText = s.ETEXT;
                        d.Status = s.STATUS_TEXT;
                        d.EntgueltigVersandt = !(d.Status.NotNullOrEmpty().ToUpper().Contains("LAGERND") || d.Status.NotNullOrEmpty().ToUpper().Contains("TEMPORÄR")); ;
                        d.IstLagernd = (d.ErsatzteilKennung.NotNullOrEmpty().ToUpper() == "L");
                    }));
            }
        }


        static public ModelMapping<Z_DPM_READ_EQUI_STL_02.GT_OUT, Stuecklisten> Z_DPM_READ_EQUI_STL_02_GT_OUT_To_StuecklistenKomponente
        {        
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_EQUI_STL_02.GT_OUT, Stuecklisten>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Fahrgestellnummer = s.CHASSIS_NUM;
                        d.Kennzeichen = s.LICENSE_NUM;                        
                        d.Materialnummer = s.IDNRK;
                        d.Bezeichnung = s.MAKTX;
                        d.Status = s.STATUS_TEXT;
                        d.Eingangsdatum = s.ANDAT;                                                                       
                    }));
            }
        }

        #endregion


        #region Save to Repository

        static public ModelMapping<Z_DPM_MARK_DAT_OHNE_DOKUM_01.GT_DAT, DatenOhneDokumente> Z_DPM_MARK_DAT_OHNE_DOKUM_01_GT_DAT_From_DatenOhneDokumente
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

        static public ModelMapping<Z_DPM_SET_ZH_ABW_ERL_01.GT_TAB, Halterabweichung> Z_DPM_SET_ZH_ABW_ERL_01_GT_TAB_From_Halterabweichung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_SET_ZH_ABW_ERL_01.GT_TAB, Halterabweichung>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                    {
                        destination.CHASSIS_NUM = source.Fahrgestellnummer;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CHANGE_MAHNSP_EQUI_01.GT_WEB, EquiMahnsperre> Z_DPM_CHANGE_MAHNSP_EQUI_01_GT_WEB_From_EquiMahnsperre
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CHANGE_MAHNSP_EQUI_01.GT_WEB, EquiMahnsperre>(
                    new Dictionary<string, string>()
                    , null
                    , (s, d) =>
                    {
                        d.EQUNR = s.EquiNr;
                        d.CHASSIS_NUM = s.FahrgestellNr;
                        d.ZZTMPDT = s.Versanddatum;
                        d.IDNRK = s.KomponentenID;
                        d.STLKN = s.StuecklistenPosKnotenNr;
                        d.VERS_ID = s.VersandID;
                        d.ZZMANSP = (s.Mahnsperre ? "X" : "");
                        d.ZZMANSP_DATBI = s.MahnsperreBis;
                    }));
            }
        }
        
        #endregion


        #region Load/Save from/to Repository


        public static ModelMapping<Z_DPM_AVM_DOKUMENT_KOPIE.GT_WEB, FahrzeugAnforderung> Z_DPM_AVM_DOKUMENT_KOPIE_GT_WEB_To_FahrzeugAnforderung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_AVM_DOKUMENT_KOPIE.GT_WEB, FahrzeugAnforderung>(
                    new Dictionary<string, string>
                    {
                        {"CHASSIS_NUM", "Fahrgestellnummer"},
                        {"LICENSE_NUM", "Kennzeichen"},
                        {"EMAIL1", "EmailAnlageUser"},
                        {"EMAIL2", "EmailExtern"},
                        {"DOK_TYP", "DokTyp"},
                        {"ERNAM", "AnlageUser"},
                        {"ERDAT", "AnlageDatum"},
                        {"FREITEXT", "Bemerkung"},
                    }));
            }
        }

        static public ModelMapping<Z_DPM_READ_AUFTR_006.GT_OUT, SelectItem> Z_DPM_READ_AUFTR_006_GT_OUT_To_SelectItem
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_AUFTR_006.GT_OUT, SelectItem>(
                    new Dictionary<string, string>()
                    , (sap, business) =>
                    {
                        business.Key = sap.POS_KURZTEXT;
                        business.Text = sap.POS_TEXT;
                    }));
            }
        }

        #endregion


        #region Fahrzeugbrief

        static private readonly Dictionary<string, string> MapFahrzeugbriefeFromSapDict = new Dictionary<string, string>
            {
                {"EQUNR", "Equipmentnummer"},
                {"CHASSIS_NUM", "Fahrgestellnummer"},
                {"LICENSE_NUM", "Kennzeichen"},
                {"LIZNR", "Vertragsnummer"},
                {"TIDNR", "TechnIdentnummer"},
                {"ZZREFERENZ1", "Referenz1"},
                {"ZZREFERENZ2", "Referenz2"},
                {"FEHLERTEXT", "Memo"},
            };

        static public ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_ABRUFBAR, Fahrzeugbrief> MapFahrzeugbriefeAbrufbarFromSAP
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_ABRUFBAR, Fahrzeugbrief>(MapFahrzeugbriefeFromSapDict,
                    (source, destination) =>
                    {
                        destination.IsMissing = false;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_FEHLER, Fahrzeugbrief> MapFahrzeugbriefeFehlerhaftFromSAP
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_FEHLER, Fahrzeugbrief>(MapFahrzeugbriefeFromSapDict,
                    (source, destination) =>
                    {
                        destination.IsMissing = true;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_IN, Fahrzeugbrief> MapFahrzeugbriefeImportToSAP
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_IN, Fahrzeugbrief>(new Dictionary<string, string>
                {
                    {"CHASSIS_NUM", "Fahrgestellnummer"},
                    {"LICENSE_NUM", "Kennzeichen"},
                    {"LIZNR", "Vertragsnummer"},
                    {"TIDNR", "TechnIdentnummer"},
                    {"ZZREFERENZ1", "Referenz1"},
                    {"ZZREFERENZ2", "Referenz2"},
               }));
            }
        }

        #endregion

    }
}