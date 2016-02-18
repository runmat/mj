using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using ERPConnect;
using SapORM.Services;
using SapORM.Models;
using SapORM.Contracts;
using GeneralTools.Models;
using WebTools.Services;

namespace SapORM
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App 
    {
        //static ISapDataService Sap { get { return SapDataService.DefaultInstance; } }

        private static ISapDataService _sap;
        public static ISapDataService Sap { get { return (_sap ?? (_sap = new SapDataServiceTestSystemNoCacheFactory().Create())); } }

        private static ISapDataService _sapProd;
        public static ISapDataService SapProd { get { return (_sapProd ?? (_sapProd = new SapDataServiceLiveSystemNoCacheFactory().Create())); } }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var encryptedHourAndDate = UserSecurityService.UrlRemoteEncryptHourAndDate();

            FunctionReflector.DataService = Sap;

            ConfigurationMerger.MergeTestWebConfigAppSettings();

            #region Model-Generation

            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_EQUI_STL_01");

            //// Hol- und Bringservice
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_2015_HOLUNDBRING_PDF");

            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_BELNR");
            //FunctionReflector.WriteOrmForSapFunction("Z_VB_EXPORT_FAELLE");
            //FunctionReflector.WriteOrmForSapFunction("Z_VB_IMPORT_FALL");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_BATCH_UPDATE");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_BATCH_INSERT");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_MODELID_TAB");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_BATCH_SELECT");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_BATCH_UNIT_SELECT");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_STATUS_EINSTEUERUNG");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_NUR_BRIEF_VORH");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_STATUS_BESTAND");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_LIST_POOLS_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_AUFTR_006");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_LIST_PDI_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_STATUS_ZUL");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_ZULASSUNGEN");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_IMP_MODELL_ID_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_FREIG_VERSAND_SPERR_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_VERSAND_SPERR_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_TH_INS_VORGANG");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CHECK_TH_CODE");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_TREUHAND_AUTHORITY");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_TH_GET_TREUH_AG");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_ABMBEREIT_LAUFZEIT");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_ABMBEREIT_LAUFAEN");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_ECA_TAB_BESTAND");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_HERSTELLERGROUP");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_Abm_Abgemeldete_Kfz");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_SCHLUE_TEMP_VERS_MAHN_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_SCHLUE_SET_MAHNSP_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_UF_MELDUNGS_SUCHE");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_BRIEF_TEMP_VERS_MAHN_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_TH_BESTAND");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_PLATARTIKEL");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_PLATSTAMM");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_PO_CREATE");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_HAENDLER_KONTINGENT_STD");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_ADRESSDATEN_STD");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_UNANGEF_ALLG_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_UNANGEFORDERT_005");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_AVIS_EXP_REDAT");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_SCHLUESSELDIFFERENZEN");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_Schluesselverloren");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_READ_GRUNDDAT_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_BRIEFLEBENSLAUF_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_BRIEFLEBENSLAUF_002");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_SAVE_AUFTRDAT_006");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_READ_AUFTRDAT_006");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_REM_READ_GUTABERICHT");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_ABEZUFZG");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_VERTRAGSBESTAND_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DAD_WEB_RGPRUEFUNG");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_UNB_HAENDLER");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_Brief_3mal_Gemahnt");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_Imp_Zul_Haend_002");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_OFFENE_ABM_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_AUFTRAGSDAT_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CD_OPTISCH_ARCHIVIERT");
            //FunctionReflector.WriteOrmForSapFunction("Z_V_Zdad_V_Vwnutz_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CD_READ_GRUEQUIDAT_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CD_READ_GRUEQUIDAT_02");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_BRIEFLEBENSLAUF_005");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_SCHEINKOPIEN");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_Unzugelassene_Fzge_Sixt_L");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_Eingabe_Lvnummer_Sixtleas");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_Abm_Fehl_Unterl_Sixt_Leas");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_ABMELDUNG_SIXT_LEASING");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SIXT_PG_KLAERFALL");
           
            //// ZLD Mobile MVC
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_USER_GET_VG");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_USER_PUT_VG");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_STAMMD");

            //// Autohaus MVC
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_EQUI_003");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_IMP_AUFTRDAT_007");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_Land_Plz_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_PARTNER_AUS_KNVP_LESEN");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_LV_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_TAB_PROT_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_UEB_CREATE_ORDER_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_UEB_NEXT_NUMBER_VORGANG_01");

            //// CoC
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_COC_TYPDATEN");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_COC_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_UPD_COC_01");

            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_ZDAD_AUFTR_006");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_PFLEGE_ZDAD_AUFTR_006");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_Blocken_Farben");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_AUTOACT_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SAVE_STATUS_AUTOACT_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_READ_ZUL_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_TYPDATEN_02");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_ZDAD_AUFTR_006");
            //FunctionReflector.WriteOrmForSapFunction("Z_V_Kcl_Gruppendaten");
            //FunctionReflector.WriteOrmForSapFunction("Z_V_Ueberf_Auftr_Kund_Port");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_Print_Coc_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_IMP_DAT_RUECKL_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_EXP_DAT_RUECKL_02");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_FILL_VERSAUFTR");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_SENDTAB_03");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CD_ABM_STATUS_TXT");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CD_ABM_HIST");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CD_ABM_LIST");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CD_Strafzettel");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_IMP_FEHLTEILETIK_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_DRUCK_FEHLTEILETIK");
            //FunctionReflector.WriteOrmForSapFunction("Z_Get_Zulst_By_Plz");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_WEB_ZULASSUNG_01");

            //// Aktivcheck
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_AKTIVCHECK_READ");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_AKTIVCHECK_CHANGE");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_DOMAENENFESTWERTE");

            //// Gebührenauslage
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_EXP_GBA_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SAVE_GBA_01");

            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SAVE_DAT_IN_RUECKL_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_RUECKL_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_STANDORTE");
            //FunctionReflector.WriteOrmForSapFunction("Z_V_Ueberf_Verfuegbarkeit1");
            //FunctionReflector.WriteOrmForSapFunction("Z_V_UEBERF_VERFUEGBARKEIT2");
            //FunctionReflector.WriteOrmForSapFunction("Z_V_UEBERF_AUFTR_UPL_PROT_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_GET_FAHRER_AUFTRAEGE");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_SET_FAHRER_AUFTRAGS_STATUS");
            //FunctionReflector.WriteOrmForSapFunction("Z_V_UEBERF_AUFTR_FAHRER");
            //FunctionReflector.WriteOrmForSapFunction("Z_UEB_FAHRER_QM");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SIS_BESTAND");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SAVE_WERTE_GUTA_VERW_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_TAB_ZEVENT_KONFIG_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_TAB_ZEVENT_KONFIG_02");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_TAB_ZEVENT_KONFIG_03");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_TAB_ZEVENT_TERMIN_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_TAB_ZEVENT_SCHADEN_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_EVENT_READ_SCHAD_STAT_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_EVENT_SET_SCHAD_STAT_01");

            //// Assistance
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_ASSIST_IMP_BESTAND_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_ASSIST_READ_BESTAND_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_ASSIST_READ_VTRAGVERL_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_ASSIST_CHG_VTRAGVERL_01");

            //// Autohaus
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_KUNDE_MAT");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_DOMAENEN_WERTE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_FS_CHECK");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_FS_SAVE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_FS_STATISTIK");

            //FunctionReflector.WriteOrmForSapFunction("Z_M_BRIEFBESTAND_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_BRIEFBESTAND_001");

            //// Versandsperre
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_ZZAKTSPERRE");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_LIST_ZZAKTSPERRE");

            //FunctionReflector.WriteOrmForSapFunction("Z_dpm_Read_Prot_Telefonate_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_UPLOAD_GRUDAT_TIP_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_VERS_GRUND_KUN_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_EQUI_MAHN_01");

            //// Daten ohne Dokumente
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_DAT_OHNE_DOKUMENT_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_MARK_DAT_OHNE_DOKUM_01");

            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_BRIEFBESTAND_002");

            //// Halterabweichungen
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_DAT_MIT_ABW_ZH_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SET_ZH_ABW_ERL_01");

            //// Dokumente ohne Daten
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_DOKUMENT_OHNE_DAT_01");
            
            //// Mahnsperre
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_TEMP_VERS_EQUI_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CHANGE_MAHNSP_EQUI_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_GET_ZZSEND2");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_ADRESSPOOL_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_STL_MAHNUNGEN_01");

            //// Mahnstop setzen
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_MAHN_EQSTL_02");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SAVE_MAHN_EQSTL_01");
            
            //// Webbearbeitung Prüfschritte
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_PRUEFPUNKTE_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_PRUEFSCHRITTE_03");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SAVE_ERL_PRUEFSCHR_01");

            //// VHC
            //FunctionReflector.WriteOrmForSapFunction("Z_M_VHC_ZBII_BESTAND_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_VHC_KLAERFAELLE_001");

            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_EXP_MAHN_ERSTEINGANG");

            //// Kroschke Zulassung
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_BELNR");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_IMPORT_ERFASSUNG1");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_MATERIAL");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_ZULST_BY_PLZ");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_ZULLISTE");

            //FunctionReflector.WriteOrmForSapFunction("Z_FI_CONV_IBAN_2_BANK_ACCOUNT");
            //FunctionReflector.WriteOrmForSapFunction("Z_AHP_CRE_CHG_PARTNER");
            //FunctionReflector.WriteOrmForSapFunction("Z_AHP_READ_PARTNER");
            //FunctionReflector.WriteOrmForSapFunction("Z_AHP_CRE_CHG_FZG_AKT_BEST");
            //FunctionReflector.WriteOrmForSapFunction("Z_AHP_READ_FZGBESTAND");
            //FunctionReflector.WriteOrmForSapFunction("Z_AHP_READ_TYPDAT_BESTAND");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_AF_ABM_SAVE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_AUSGABE_ZULFORMS");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_ZULSTEL");
            //FunctionReflector.WriteOrmForSapFunction("Z_AHP_CRE_CHG_PARTNER_FZGDATEN");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZANF_READ_KLAERF_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_ZULDOK_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SAVE_ZULDOK_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_DOK_ARCHIV_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EXPORTAENDERUNG_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_MODELID_TAB");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CHANGE_MODELID");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_BAPIRDZ");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_KLAERFAELLEVW");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_READ_AUFTRAEGE_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_READ_KONVERTER_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_STORNO_AUFTRAG_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_READ_INFO_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_WRITE_INFO_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_LIST_DOKU_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_READ_DOKU_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_WRITE_DOKU_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_READ_TODO_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_SET_STATUS_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_WRITE_TODO_02");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_HERSTELLERGROUP");

            //// ZLD-Portal
            //FunctionReflector.WriteOrmForSapFunction("Z_ALL_DEBI_CHECK_TABLES");
            //FunctionReflector.WriteOrmForSapFunction("Z_ALL_DEBI_VORERFASSUNG_WEB");
            //FunctionReflector.WriteOrmForSapFunction("Z_BC_LTEXT_DELETE");
            //FunctionReflector.WriteOrmForSapFunction("Z_BC_LTEXT_INSERT");
            //FunctionReflector.WriteOrmForSapFunction("Z_BC_LTEXT_READ");
            //FunctionReflector.WriteOrmForSapFunction("Z_BC_LTEXT_UPDATE");
            //FunctionReflector.WriteOrmForSapFunction("Z_FI_CONV_IBAN_2_BANK_ACCOUNT");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_GET_KOSTL");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_PLATARTIKEL");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_PLATSTAMM");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_PO_CREATE");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_UML_MAT");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_UML_MAT_GROESSE");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_UML_OFF_POS");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_UML_STEP1");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_EFA_UML_STEP2");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_READ_OFF_BEST_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_BAPIRDZ");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_ZGBS_BEN_ZULASSUNGSUNT");
            //FunctionReflector.WriteOrmForSapFunction("Z_MC_CONNECT");
            //FunctionReflector.WriteOrmForSapFunction("Z_MC_GET_IN_OUT");
            //FunctionReflector.WriteOrmForSapFunction("Z_MC_NEW_VORGANG");
            //FunctionReflector.WriteOrmForSapFunction("Z_MC_SAVE_ANSWER");
            //FunctionReflector.WriteOrmForSapFunction("Z_MC_SAVE_STATUS_IN");
            //FunctionReflector.WriteOrmForSapFunction("Z_MC_SAVE_STATUS_OUT");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_EX_VSZUL");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_VZ_SAVE2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_BARABHEBUNG");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CHANGE_VZOZUERL");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CHECK_ZLD");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CJ2_CALC_MWST");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CJ2_CASHJOURNALDOC_CRE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CJ2_GET_ALL_DOCS");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CJ2_GET_CJNR");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CJ2_GET_NEW_NUMBER");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CJ2_GET_PERIOD");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CJ2_GET_TRANSACTIONS");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CJ2_GET_WERTE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CJ2_SAVE_DOC");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_DOMAENEN_WERTE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_AUSWERTUNG_1");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_BELNR");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_FILIAL_ADRESSE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_INFOPOOL");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_KUNDE_MAT");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_LS");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_NACHERF2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_NEW_DEBI");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_PRALI");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_SOFORT_ABRECH2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_TAGLI");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_VZOZUERL");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_ZULSTEL");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_FIND_DAD_SD_ORDER");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_GET_BARQ_FROM_EASY");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_GET_DAD_SD_ORDER");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_GET_GRUPPE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_GET_GRUPPE_KDZU");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_GET_NICKNAME");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_GET_ORDER2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_IMP_KOMPER2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_IMP_NACHERF2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_IMP_NACHERF_DZLD2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_IMPORT_ERFASSUNG2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_IMPORT_SOFORT_ABRECH2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_IMPORT_ZULUNT");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_DISPO_GET_VG");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_DISPO_SET_USER");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_GET_USER");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_GET_VG_FOR_UPD");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_SET_VG_STATUS");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_PREISFINDUNG2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_SAVE_DATA2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_SAVE_TAGGLEICHE_MELDUNG");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_SET_GRUPPE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_SET_GRUPPE_KDZU");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_SET_LOEKZ");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_SET_NICKNAME");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_SETNEW_DEBI_ERL");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_STO_GET_ORDER2");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_STO_STORNO_CHECK");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_STO_STORNO_LISTE");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_STO_STORNO_ORDER");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_STO_STORNOGRUENDE");

            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_CHECK_OPEN_002");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_READ_OFF_BEST_POS_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_WE_ZUR_BEST_POS_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_FAHRZEUGHISTORIE_AVM");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_DRUCK_FZG_HISTORIE_AVM");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_HERSTELLERGROUP");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_ZULAUF");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_GET_USER_AEMTER");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_ZULST_OPEN");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_ZULASSUNGSSPERRE_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SET_BEM_FZGPOOL_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_PDIWECHSEL");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_CHECK_48H");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_EXPORT_ANGENOMMENE");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_KENNZ_SERIE");
            //FunctionReflector.WriteOrmForSapFunction("Z_Massenzulassung");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_HERST_VWZWECK_MODID");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_ANZ_BEAUFTR_ZUL");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_Ec_Avm_Zulassungssperre");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_EC_AVM_MELDUNGEN_PDI1");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_FFD_DATEN_OHNE_DOKUMENTE");
            //FunctionReflector.WriteOrmForSapFunction("Z_V_UEBERF_AUFTR_REFERENZ");
            //FunctionReflector.WriteOrmForSapFunction("Z_V_UEBERF_AUFTR_PROTOKOLL_AB");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_QM_READ_QPCD");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_EXPORT_WARENKORB");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_EXPORT_AH_WARENKORB");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_IMPORT_AH_WARENKORB");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_DELETE_AH_WARENKORB");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_2015_ETIKETT_SEL");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_2015_ETIKETT_DRU");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZANF_READ_DATEN_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_FAELLIGE_EQUI_LP");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_FAELLIGE_EQUI_UPDATE_LP");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_ZUL_EXPORT_ORDER");
            //FunctionReflector.WriteOrmForSapFunction("Z_FIL_ZUL_IMPORT_STATUS");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_IMP_MEL_CARP_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_UEBERMITTLUNG_STAT_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_SET_STATUS_UEBERM_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_PRUEF_FIN_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_EXP_VERS_AUSWERTUNG_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_AHP_READ_VERSUNTERNEHMEN");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_CARPID_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_MEL_CARP_01");
            //FunctionReflector.WriteOrmForSapFunction("ISA_ADDR_POSTAL_CODE_CHECK");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_VERSAUFTR_FEHLERHAFTE");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_ABWEICH_ABRUFGRUND_02");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_WARENKORB_SPERRE_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_IMPORT_ABMELDUNG_VWL");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_UF_STORNO");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_SAVE_ZABWVERSGRUND");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_CHANGE_ADDR002_001");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_UF_EQUI_SUCHE");
            //FunctionReflector.WriteOrmForSapFunction("Z_DAD_DATEN_EINAUS_REPORT_002");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_UF_CREATE");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_IMP_CARPORT_MELD_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_TAB_ZEVENT_TERMIN_02");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_AVM_DOKUMENT_MAIL");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_AVM_DOKUMENT_KOPIE");
            //FunctionReflector.WriteOrmForSapFunction("Z_M_VERSAUFTR_FEHLERHAFTE_DEL");
            //FunctionReflector.WriteOrmForSapFunction("Z_WFM_CALC_DURCHLAUFZEIT_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SET_DAT_ABM_STATUS_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_MOB_CHECK_BEB_STATUS");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_LAND_02");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_REM_VERS_VORG_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_SAVE_REM_VERS_VORG_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_CARPORT_MELD_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_INS_CARPORT_NACHLIEF_01");
            // CKG Partner-Portal
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_PP_GET_PO_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_PP_SAVE_PO_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_PP_GET_ZULASSUNGEN_01");
            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_PP_STAMMDATEN");

            //FunctionReflector.WriteOrmForSapFunction("Z_ZLD_AH_2015_ZULLISTE");
            //FunctionReflector.WriteOrmForSapFunction("Z_DPM_READ_EQUI_STL_02");

            #endregion


            //CoCTest();
            //TeslaTest();
            //BrieflebenslaufTest();

            //Z_DPM_READ_AUTOACT_01_Test();

            //TargoTest2();
            //VwTest();
            //AkfTest();
            //BpLgTest();

            //CwTest2();

            //ArtikelTest();
            //HaendlerReadTest();

            //KennzeichenImportTest();
            //KennzeichenReadTest();

            //BriefReadTest();
            //AdacReadTest();

            //AvisReadTest();
            //AvisSchluesseldiffTest();
            //AvisZulassungsTest();
            //AvisHistorieTest();

            //GutachtenTest();


            //AhMvcTest();
            //AhMvcTest2();
            //AhÜberführungTest();
            //AhÜberführungTest2();
            //AhÜberführungTest3();

            //VwTest();

            //StoreTest();

            //AvisTest5();
            //SilvergreenTest();

            //HannoverLeasingTest();

            //ScheinkopienTest();

            //FehlteilEtikettTest();

            //ZulassungsServicesTest();


            
            //CardocuTest();

            //CardocuEquiTest();
            //FahrerQmTestRead();
            //FahrerQmTestUpd();

            //FahrerQmTestFleet();



            //UeberfuehrungHistoryTest();
            //ZulassungsStellenTest();

            //ErpBulkCopySqlTest_Autovermieter();
            //GetAndFillStandorte();
            //ErpBulkCopySqlTest_FeinstaubPlaketten();
            //ErpBulkCopySqlTest_KennzeichenVerkauf();

            
            //CsiTest();

            //TargoTest3();

            //AhpZullisteTest();

            //ModellIdTest();

            Shutdown();
        }

        //static void GutachtenTest()
        //{
        //    var importList = Z_DPM_REM_READ_GUTABERICHT.GT_IN_KLASSE.GetImportListWithInit(Sap,
        //                                                "I_KUNNR, I_EMAIL, I_DAT_VON, I_DAT_BIS",
        //                                                "0000322489", "test@test.de", new DateTime(2013, 01, 01), new DateTime(2013, 01, 31));

        //    var item = new Z_DPM_REM_READ_GUTABERICHT.GT_IN_KLASSE
        //                   {
        //                       WERT = "00001"
        //                   };
        //    importList.Add(item);
            
        //    Sap.ApplyImport(importList);
        //    Sap.Execute();
        //}

        //static void KennzeichenImportTest()
        //{
        //    var kostSt = "4981";
        //    var kredNr = "0000900153";
        //    var lsNr = "4711-manuell";
        //    var lsDatum = DateTime.Today.AddDays(-30);

        //    var importList = Z_FIL_EFA_PO_CREATE.GT_POS.GetImportListWithInit(Sap,
        //                                                "I_KOSTL, I_LIFNR, I_VERKAEUFER, I_LIEF_KZ, I_LIEF_NR, I_EEIND",
        //                                                 kostSt, kredNr, "", "X", lsNr, lsDatum);
        //    var newPos = new Z_FIL_EFA_PO_CREATE.GT_POS
        //                     {
        //                         ARTLIF = "99",
        //                         LTEXT_NR = "0000075527",
        //                         MENGE = 242,
        //                         PREIS = 272.42m,
        //                         ZUSINFO_TXT = "",
        //                     };
        //    importList.Add(newPos);
        //    Sap.ApplyImport(importList);

        //    Sap.Execute();

        //    var resultCode = Sap.ResultCode;
        //    var resultMessage = Sap.ResultMessage;
        //}

        //static void KennzeichenReadTest()
        //{
        //    var kredNr = "0000900153";
        //    var kostSt = "4981";

        //    var kopfList = Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST.GT_PO_K.GetExportListWithInitExecute(Sap, "I_KOSTL, I_LIFNR",
        //                                                                             kostSt, kredNr);
        //    var kopfListCount = kopfList.Count();
        //    var dtKopf = kopfList.ToTable();

        //    var posList = Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST.GT_PO_P.GetExportList(Sap);
        //    var pos1 = posList.FirstOrDefault();
        //    var posListCount = posList.Count();
        //    var dtPos = posList.ToTable();

        //    var test = posList.FirstOrDefault(p => p.PREIS == 272.42m);
        //    if (test != null)
        //    {
        //        var text = test.LTEXT;
        //    }
        //}

        //static void GetAndFillStandorte()
        //{
        //    var standorte = Z_FIL_STANDORTE.GT_STANDORTE.GetExportListWithInitExecute(Sap);

        //    var standorteCount = standorte.Count;
        //    var standorteTable = standorte.ToTable();
        //    var definedTable = standorte.ToTable();

        //    foreach (DataColumn dtclm in standorteTable.Columns.Cast<DataColumn>().Where(dtclm => dtclm.ColumnName != "ZKBA1" & dtclm.ColumnName != "PRCTR" & dtclm.ColumnName != "BUTXT"
        //                                                                                          & dtclm.ColumnName != "ZKREIS" & dtclm.ColumnName != "STRAS" & dtclm.ColumnName != "PSTLZ"
        //                                                                                          & dtclm.ColumnName != "ORT01"))
        //    {
        //        definedTable.Columns.Remove(dtclm.ColumnName);
        //    }


        //    SqlBulkCopy(definedTable, "GT_STANDORTE", null);

        //}

        //static void AdacReadTest()
        //{
        //    //var kundennr = "0000341523";
        //    //var eqtyp = "B";
        //    var kundennr = "0000350798";
        //    var eqtyp = "T";
        //    var fahrgestell = "*42*";
        //    var kennzeichen = "*47*";
        //    var referenz1 = "*8*";

        //    var schluesselList = Z_DPM_UNANGEFORDERT_005.GT_WEB.GetExportListWithInitExecute(Sap,
        //                                                        "I_KUNNR, I_EQTYP, I_LICENSE_NUM, I_CHASSIS_NUM, I_ZZREFERENZ1",
        //                                                        kundennr, eqtyp, kennzeichen, fahrgestell, referenz1);
        //    var schluesselListCount = schluesselList.Count;
        //}

        //static void AvisReadTest()
        //{
        //    var avisList = Z_DPM_AVIS_EXP_REDAT.GT_OUT.GetExportListWithInitExecute(Sap,
        //                                                        "I_SEL_ART, I_REDAT_VON, I_REDAT_BIS",
        //                                                        "", "01.12.2011", "31.12.2012"); // R/F
        //    var avisListListCount = avisList.Count;
        //}

        //static void AvisSchluesseldiffTest()
        //{
        //    //var avisList = Z_M_SCHLUESSELDIFFERENZEN.GT_WEB_OUT_BRIEFE.GetExportListWithInitExecute(Sap,
        //    //                                                    "I_KUNNR",
        //    //                                                    "0000314582");
        //    //var avisListListCount = avisList.Count;

        //    var exportTables = Sap.GetExportTablesWithInitExecute("Z_M_SCHLUESSELDIFFERENZEN",
            //                                                    "I_KUNNR",
            //                                                    "0000314582");
        //}

        //static void AvisZulassungsTest()
        //{
        //    var dateStart = new DateTime(2012, 06, 01);
        //    var dateEnd = new DateTime(2012, 06, 30);

        //    var avisList = Z_M_READ_GRUNDDAT_001.GT_WEB.GetExportListWithInitExecute(Sap,
        //                                                        "I_KUNNR_AG, I_EING_DAT_VON, I_EING_DAT_BIS, I_ZUGELASSEN",
        //                                                        "0000314582", dateStart, dateEnd, "N");
        //    var avisListListCount = avisList.Count;

        //    //var dt = Sap.GetExportTableWithInitExecute("Z_M_READ_FZGPOOL_ZUL_FZG_006.GT_WEB", "I_KUNNR_AG", "0000314582");
        //    //var rowCount = dt.Rows.Count;

        //    //var dt = Sap.GetExportTableWithInitExecute("Z_M_Datenimport_Ohne_Briefe.GT_WEB",
        //    //                                           "I_KNRZE, I_KONZS, I_KUNNR, I_VKORG",
        //    //                                           "10", "314582", "0000314582", "1510");
        //    //var rowCount = dt.Rows.Count;
        //}

        //static void AvisHistorieTest()
        //{
        //    var avisTable = Sap.GetExportTableWithInitExecute("Z_M_BRIEFLEBENSLAUF_002.GT_WEB",
        //                                                        "I_KUNNR, I_ZZKENN, I_ZZFAHRG, I_ZZBRIEF, I_PROD_KENNZIFFER, I_ZZREF1, I_EQUNR, I_MVA_NUMMER",
        //                                                        "0000314582", "", "", "DZ987638", "", "", "", "");
        //    var row = avisTable.Rows[0];

        //    var exportList = Z_M_BRIEFLEBENSLAUF_002.GT_QMMA.GetExportListWithInitExecute(Sap,
        //                                                        "I_KUNNR, I_ZZKENN, I_ZZFAHRG, I_ZZBRIEF, I_PROD_KENNZIFFER, I_ZZREF1, I_EQUNR, I_MVA_NUMMER",
        //                                                        "0000314582", "", "", "DZ987638", "", "", "", "");
        //    var exportItem = exportList.FirstOrDefault();
        //    var exportListCount = exportList.Count;
        //}

        //static void BriefReadTest()
        //{
        //    var importList = Z_DPM_UNANGEF_ALLG_01.GT_IN.GetExportListWithInitExecute(Sap,
        //                                                        "I_KUNNR_AG, I_EQTYP",      //"I_KUNNR_ZP, I_RESTLAUFZEIT, I_ABMDAT_VON, I_ABMDAT_BIS, I_ABMAUF_VON, I_ABMAUF_BIS, I_ABMELD, I_ZULASS_DAT_VON, I_ZULASS_DAT_BIS",
        //                                                        "0000341523", "B");         //, null, null, null, null, null, null, null, null, null);

        //    var newPos = new Z_DPM_UNANGEF_ALLG_01.GT_IN { LICENSE_NUM = "WAF-K193" };
        //    importList.Add(newPos);

        //    var dt = Sap.GetImportTableWithInit("Z_DPM_UNANGEF_ALLG_01.GT_ABRUFBAR",
        //                                           "I_KUNNR_AG, I_EQTYP",
        //                                           "0000341523", "B");

        //    Sap.ApplyImport(importList);

        //    Sap.Execute();

        //    Sap.GetExportTable("MY_TABLE_NAME");

        //    var briefList = Z_DPM_UNANGEF_ALLG_01.GT_ABRUFBAR.ToList(Sap);
        //    var briefListCount = briefList.Count;
        //}

        //static void HaendlerReadTest()
        //{
        //    var haendlerList = Z_M_ADRESSDATEN_STD.GT_ADRS.GetExportListWithInitExecute(Sap,
        //                                                      "I_AG, I_HAENDLER_EX, I_NAME, I_ORT, I_PSTLZ, I_MAX",
        //                                                      "0000347452", "", "", "", "", "100000");
        //    var haendlerListCount = haendlerList.Count;
        //}

        //static void ArtikelTest()
        //{
        //    //var kredNr = "0000900153";
        //    //var kostSt = "[vkbur]";
            //var kredNr = "0000900153";
        //    var kostSt = "4359";

        //    var artikelList = Z_FIL_EFA_PLATARTIKEL.GT_PLATART.GetExportListWithInitExecute(Sap,
        //                                                        "I_KOSTL, I_LIFNR, I_RUECKS, I_FIL, I_ZLD",
        //                                                        kostSt, kredNr, " ", " ", "X");

        //    var artikelListCount = artikelList.Count();
        //}

        //static void AkfTest()
        //{
        //    var dt = DateTime.Now;
        //    var akfList = Z_M_UNB_HAENDLER.GT_WEB.GetExportListWithInitExecute(Sap,
        //                                                        "I_KUNNR",
        //                                                        "0000302660");
        //    var dt2 = DateTime.Now;
        //    var diff = dt2 - dt;
        //    var akfListCount = akfList.Count();
        //}

        //static void BpLgTest()
        //{
        //    var list = Z_M_Brief_3mal_Gemahnt.EXP_BRIEFE.GetExportListWithInitExecute(Sap, "I_AG", "0000343537");
            //var listCount = list.Count();


        //    //var list = Z_ZLD_MOB_USER_GET_VG.GT_VG_KOPF.GetExportListWithInitExecute(Sap, "I_Vkorg,I_Vkbur,I_Mobuser", 
        //    //    "1010", "4265", "ZLDTEST");
        //    //var listCount = list.Count();
        //}

        //static void CwTest2()
        //{
        //    var list = Z_DPM_OFFENE_ABM_01.GT_OUT.GetExportListWithInitExecute(Sap,
        //                                                        "I_KUNNR_AG, I_ERDAT_VON, I_ERDAT_BIS, I_FAHRG, I_KENNZ, I_SCHEIN_FEHLT, I_SCHILD_FEHLT",
        //                                                        "0010020162", new DateTime(2012, 01, 01), new DateTime(2012, 12, 30), null, null
        //                                                        , "X", "X"
        //                                                        );
        //    var listCount = list.Count();
        //    var listCount2 = list.Count(c => c.SCHILD_FEHLT != "X" && c.SCHILD_FEHLT == "X");
        //}

        //static void CwTest()
        //{
        //    var list = Z_DPM_READ_AUFTRAGSDAT_01.GT_OUT.GetExportListWithInitExecute(Sap,
        //                                                        "I_AG, I_VDATU_VON, I_VDATU_BIS, I_AUGRU, I_NUR_OFFENE_UK, I_NUR_KLAERFAELLE",
        //                                                        "0010020162", 
        //                                                        //new DateTime(2010, 01, 01), new DateTime(2013, 02, 28), 
        //                                                        null, null,
        //                                                        //"019", "X", null
        //                                                        "016", null, null
        //                                                        );
        //    var listCount = list.Count();
        //    var listCount2 = list.Count(c => c.ERLEDIGT != "0");
        //}

        //static void VwTest2()
        //{
        //    var vwList = Z_DAD_WEB_RGPRUEFUNG.GT_RECHNUNGEN.GetExportListWithInitExecute(Sap,
        //                                                        "I_KUNNR, I_ZZREFERENZ1, I_LIZNR, I_AUSLIDAT_VON, I_AUSLIDAT_BIS, I_RECHNUNGSNUMMERN",
        //                                                        "0000338201", "", "", DateTime.Parse("04.01.2012"), DateTime.Parse("04.02.2012"), "");
        //    var vwListCount = vwList.Count();
        //}

        //static void VwTest()
        //{
        //    var list = Z_V_Zdad_V_Vwnutz_001.VWNUTZ_TAB.GetExportListWithInitExecute(Sap,
        //                                      "KUNNR, STATUS, UEBERGDAT_VON, UEBERGDAT_BIS, VORHABEN",
        //                                      "0000338201", "A",
        //                                      DateTime.Parse("01.06.2011"), DateTime.Parse("01.08.2013"), "13*");
        //    var rowCount = list.Count;
        //}

        //static void TargoTest2()
        //{
        //    Sap.InitExecute("Z_DPM_VERTRAGSBESTAND_01",
        //                    "I_KUNNR_AG, I_PAID",
        //                    "0010026883", "0201208144225620");

        //    var outs = Z_DPM_VERTRAGSBESTAND_01.GT_OUT.GetExportList(Sap);
        //    var notes = Z_DPM_VERTRAGSBESTAND_01.GT_NOTIZ.GetExportList(Sap);
        //    var dt = outs[0].MAHNSP_GES_AM;
        //    var dt2 = outs[0].ENDG_VERS;
        //    var dt3 = outs[0].DAT_VERTR_ENDE;
        //}

        //static void TargoTest()
        //{
        //    Sap.InitExecute("Z_DPM_VERTRAGSBESTAND_01",
        //        "I_KUNNR_AG, I_PAID",
        //        "0010026883", "0201207033133247");

        //    var outs = Z_DPM_VERTRAGSBESTAND_01.GT_OUT.GetExportList(Sap);
        //    var notes = Z_DPM_VERTRAGSBESTAND_01.GT_NOTIZ.GetExportList(Sap);
        //    var dt = outs[0].MAHNSP_GES_AM;
        //    var dt2 = outs[0].ENDG_VERS;
        //    var dt3 = outs[0].DAT_VERTR_ENDE;
        //}

        //static void AhMvcTest()
        //{
        //    var list = Z_DPM_READ_EQUI_003.GT_OUT.GetExportListWithInitExecute(Sap,
        //                                                        "I_KUNNR_AG, I_EQTYP, I_CHASSIS_NUM, I_LICENSE_NUM, I_LIZNR",
        //                                                        "0010000649", "B", "", "BS-", ""
        //                                                        );
        //    var listCount = list.Count();
        //    var listCount2 = list.Count(i => !string.IsNullOrEmpty(i.LIZNR));
        //}

        //static void AhMvcTest2()
        //{
        //    var tb = Sap.GetExportTableWithInitExecute("Z_M_Land_Plz_001.GT_WEB", "");

        //    var laenderList = Z_M_Land_Plz_001.GT_WEB.GetExportListWithInitExecute(Sap, "");
        //    var list = Z_M_IMP_AUFTRDAT_007.GT_WEB.GetExportListWithInitExecute(Sap,
        //                                                        "I_KUNNR, I_KENNUNG, I_NAME1, I_PSTLZ, I_ORT01",
        //                                                        "0010000649", "AUSLIEFERUNG", "", "", ""
        //                                                        );
            //var listCount = list.Count();
        //}

        //static void CardocuTest()
        //{
        //    var importList = Z_DPM_CD_OPTISCH_ARCHIVIERT.GT_WEB.GetImportListWithInit(Sap,
        //                                                        "I_AG",
        //                                                        "0010051385");

        //    var newPos = new Z_DPM_CD_OPTISCH_ARCHIVIERT.GT_WEB
        //                     {
        //                         INDEXNR = "ZD34567890123",
        //                         QMCOD = "STRA",
        //                         STORT = "1601",
        //                         SUBRC = 9999,
        //                         MESSAGE = "---"
        //                     };
        //    importList.Add(newPos);

        //    Sap.ApplyImport(importList);

        //    Sap.Execute();

        //    var message = Z_DPM_CD_OPTISCH_ARCHIVIERT.GT_WEB.GetExportList(Sap)[0].MESSAGE;
        //}

        //static void AhÜberführungTest()
        //{
        //    //var list = Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE.GetExportListWithInitExecute(Sap, "KUNNR", "0010000649");
        //    //var listCount = list.Count();

        //    var list = Z_DPM_READ_TAB_PROT_01.GT_OUT.GetExportListWithInitExecute(Sap, "I_KUNNR_AG", "0010000649");
        //    var listCount = list.Count();
        //}

        //static void AhÜberführungTest2()
        //{
        //    var kuNr = "0010000649".ToSapKunnr();

            //var importListAG = Z_DPM_READ_LV_001.GT_IN_AG.GetImportListWithInit(Sap, "I_VWAG", "X");
            //importListAG.Add(new Z_DPM_READ_LV_001.GT_IN_AG { AG = kuNr });
            //Sap.ApplyImport(importListAG);

            //var importListProcess = Z_DPM_READ_LV_001.GT_IN_PROZESS.GetImportList(Sap);
            //importListProcess.Add(new Z_DPM_READ_LV_001.GT_IN_PROZESS { SORT1 = "7" });
            //Sap.ApplyImport(importListProcess);


            //Sap.Execute();


            //// TransportTypen (5 Stück)
            //var listTransportTypen = Z_DPM_READ_LV_001.GT_OUT_DL.GetExportList(Sap).Where(d => string.IsNullOrEmpty(d.ASNUM) && string.IsNullOrEmpty(d.KTEXT1_H2)).ToList();


            //// alle Dienstleistungen
            //var listAll = Z_DPM_READ_LV_001.GT_OUT_DL.GetExportList(Sap).Where(d => !string.IsNullOrEmpty(d.ASNUM) && string.IsNullOrEmpty(d.KTEXT1_H2));

            //// Dienstleistungen für Transporttyp 1
            //var preisSumme = listAll.Sum(n => n.TBTWR);

            //listAll = listAll.Where(d => d.EXTGROUP == "1");

            //var listStandard = listAll.Where(d => d.VW_AG == "X").ToList();
            //var listNonStandard = listAll.Where(d => d.VW_AG != "X").ToList();
        //}

        //static void AhÜberführungTest3()
        //{
        //    var kuNr = "0010000649".ToSapKunnr();

        //    Sap.InitExecute("Z_UEB_NEXT_NUMBER_VORGANG_01", "E_VORGANG", "");
        //    var vorgangsNr1 = Sap.GetExportParameter("vorgangsNr1");
        //    var vorgangsNr2 = Sap.GetExportParameter("vorgangsNr2");
        //    var vorgangsNr3 = Sap.GetExportParameter("vorgangsNr3");
        //    //var importListAG = Z_DPM_READ_LV_001.GT_IN_AG.GetImportListWithInit(Sap, "I_VWAG", "X");
        //    //importListAG.Add(new Z_DPM_READ_LV_001.GT_IN_AG { AG = kuNr });
        //    //Sap.ApplyImport(importListAG);

        //    //var importListProcess = Z_DPM_READ_LV_001.GT_IN_PROZESS.GetImportList(Sap);
        //    //importListProcess.Add(new Z_DPM_READ_LV_001.GT_IN_PROZESS { SORT1 = "7" });
        //    //Sap.ApplyImport(importListProcess);


        //    //Sap.Execute();
                                    
                                    
        //    //// TransportTypen (5 Stück)
        //    //var listTransportTypen = Z_DPM_READ_LV_001.GT_OUT_DL.GetExportList(Sap).Where(d => string.IsNullOrEmpty(d.ASNUM) && string.IsNullOrEmpty(d.KTEXT1_H2)).ToList();


        //    //// alle Dienstleistungen
        //    //var listAll = Z_DPM_READ_LV_001.GT_OUT_DL.GetExportList(Sap).Where(d => !string.IsNullOrEmpty(d.ASNUM) && string.IsNullOrEmpty(d.KTEXT1_H2));

        //    //// Dienstleistungen für Transporttyp 1
        //    //var preisSumme = listAll.Sum(n => n.TBTWR);

        //    //listAll = listAll.Where(d => d.EXTGROUP == "1");

        //    //var listStandard = listAll.Where(d => d.VW_AG == "X").ToList();
        //    //var listNonStandard = listAll.Where(d => d.VW_AG != "X").ToList();
        //}

        //private static void CoCTestSubstring()
        //{
        //    var s = "ABCD123456abcdef";

        //    var s1 = s.SubstringTry(0, 2);
        //    var s2 = s.SubstringTry(1, 2);
        //    var s3 = s.SubstringTry(4, 6);
        //    var s4 = s.SubstringTry(10, 6);
        //    var s5 = s.SubstringTry(10, 8);
        //    var s6 = s.SubstringTry(14, 2);
        //    var s7 = s.SubstringTry(14, 99);
        //    var s8 = s.SubstringTry(16, 1);
        //    var s9 = s.SubstringTry(16, 99);
        //}

        //readonly static string KUNNR = "0010104040";

        //#region Test

        //private static List<Z_DPM_COC_TYPDATEN.GT_WEB> _sapCocTypen;
        //private static List<Z_DPM_COC_TYPDATEN.GT_WEB> SapCocTypen
        //{
        //    get
        //    {
        //        return (_sapCocTypen ?? (_sapCocTypen =
        //            new List<Z_DPM_COC_TYPDATEN.GT_WEB>
        //                {
        //                    new Z_DPM_COC_TYPDATEN.GT_WEB
        //                        {
        //                            VORLAGE = "",
        //                            KUNNR = KUNNR,
        //                            COC_0_2_TYP = "AUDI",
        //                            //COC_0_2_TYP_2 = "",
        //                            //COC_0_2_TYP_3 = "",
        //                            //COC_0_2_TYP_4 = "",

        //                            COC_0_2_VAR = "A3",
        //                            //COC_0_2_VAR_2 = "",
        //                            //COC_0_2_VAR_3 = "",
        //                            //COC_0_2_VAR_4 = "",
        //                            //COC_0_2_VAR_5 = "",
        //                            //COC_0_2_VAR_6 = "",
        //                            //COC_0_2_VAR_7 = "",
            
        //                            COC_0_2_VERS = "SPORT",
        //                            //COC_0_2_VERS_2 = "",
        //                            //COC_0_2_VERS_3 = "",
        //                            //COC_0_2_VERS_4 = "",
        //                            //COC_0_2_VERS_5 = "",
        //                            //COC_0_2_VERS_6 = "",
        //                            //COC_0_2_VERS_7 = "",
        //                            //COC_0_2_VERS_8 = "",
        //                            //COC_0_2_VERS_9 = "",

        //                            COC_0_1 = "A5",
        //                            //COC_0_4 = "MI",
        //                            //COC_36_PNEUMATISCH = "X",
        //                            //ERDAT = DateTime.Parse("20.04.2013"),

        //                            COC_50_JA = "X",
        //                        },
        //                }
        //        ));
        //    }
        //    set { _sapCocTypen = value; }
        //}

        //#endregion

        //private static void CoCTest()
        //{
        //    CocTestInsert();
        //    //return;

        //    //var sap = new SapDataServiceFromConfigNoCacheFactory().Create();
        //    //var sap = SapDataService.DefaultInstance;
        //    var sap = Sap;

        //    Z_DPM_COC_TYPDATEN.Init(sap);
        //    sap.SetImportParameter("I_KUNNR", KUNNR);
        //    sap.SetImportParameter("I_VERKZ", "L");

        //    sap.SetImportParameter("I_TYP", "AUDI");
        //    sap.SetImportParameter("I_VARIANTE", "A3");
        //    sap.SetImportParameter("I_VERSION", "SPORT");


        //    sap.Execute();
        //    var list = Z_DPM_COC_TYPDATEN.GT_WEB.GetExportList(sap);            
        //    if (list.Count > 0)
        //    {
        //        var item = list.First();
        //        var coc01 = item.COC_0_1;
        //        var ja = item.COC_50_JA;
        //    }

        //    var message = sap.ResultMessage;
        //}

        //static void CocTestInsert()
        //{
        //    //var sap = new SapDataServiceFromConfigNoCacheFactory().Create();
        //    //var sap = SapDataService.DefaultInstance;
        //    var sap = Sap;

        //    Z_DPM_COC_TYPDATEN.Init(sap);
        //    sap.SetImportParameter("I_KUNNR", KUNNR);
        //    sap.SetImportParameter("I_VERKZ", "U");

        //    sap.SetImportParameter("I_TYP", "AUDI");
        //    sap.SetImportParameter("I_VARIANTE", "A3");
        //    sap.SetImportParameter("I_VERSION", "SPORT");
        //    sap.SetImportParameter("I_VORLAGE", "");

        //    var importList = Z_DPM_COC_TYPDATEN.GT_WEB.GetImportList(sap);
        //    importList.AddRange(SapCocTypen);
        //    sap.ApplyImport(importList);

        //    sap.Execute();
        //    var updateMessage = sap.ResultMessage;
        //}

        //readonly static string KUNNR_Tesla = "0000350798";
        //readonly static string VIN = "TEST0000000000003";

        //private static void TeslaTest()
        //{
        //    var list = GetCocAuftraege("N").Concat(GetCocAuftraege("D")).ToList();
        //    var count = list.Count();

        //    //var farbList = Z_M_Blocken_Farben.FARBE.GetExportListWithInitExecute(Sap);
        //    //var landList = Z_M_Land_Plz_001.GT_WEB.GetExportListWithInitExecute(Sap);

        //    //TeslaTestUpd();
        //    TeslaTestRead();

        //    count = list.Count();
        //}

        //private static void TeslaTestRead()
        //{
        //    //var vorgangsNr = Sap.GetExportParameterWithInitExecute("Z_UEB_NEXT_NUMBER_VORGANG_01", "E_VORGANG", "");
        //    //if (vorgangsNr.IsNullOrEmpty())
        //    //    return returnList;

        //    var list = Z_DPM_READ_COC_01.GT_OUT.GetExportListWithInitExecute(Sap, "I_AG", KUNNR_Tesla);
        //    var fList = list.Where(i => i.VIN == VIN).ToList();
        //    //var fListCount = fList.Count(i => i.VIN == VIN);
        //    //if (fListCount == 0)
        //    //    return;

        //    var zb2druck = fList[0].ZBII_DRUCK;
        //    var vorgangnr = fList[0].VORG_NR;
        //    var auftrdat = fList[0].AUFTRAG_DAT;
        //    var erdat = fList[0].COC_ERF_DAT;
        //    var ausliefdat = fList[0].AUSLIEFER_DATUM;

        //    //var zb2druck2 = fList[1].ZBII_DRUCK;
        //    //var vorgangnr2 = fList[1].VORG_NR;
        //    //var auftrdat2 = fList[1].AUFTRAG_DAT;

        //    var farbe = fList[0].ZBII_R;
        //    var farbCode = fList[0].ZBII_11;
        //}

        //private static void TeslaTestUpd()
        //{
        //    var importList = Z_DPM_UPD_COC_01.GT_DAT.GetImportListWithInit(Sap);
        //    importList.Add(new Z_DPM_UPD_COC_01.GT_DAT
        //        {
        //            VORG_NR = "0000000283",
        //            AKTION = "U",
        //            KUNNR_AG = KUNNR_Tesla,
        //            //AUFTR_NR_KD = "1000060568",
        //            //LAND = "DE",
        //            //ZBII_DRUCK = "Z",
        //            //VORG_NR = "0000000031",
        //            VIN = VIN,
            
        //            //COC_DRUCK_ORIG = "X",
        //            //COC_KD_ORIG = "X",
        //            AUFTRAG_DAT = DateTime.Now.AddDays(7),

        //            //AUSLIEFER_DATUM = DateTime.Now.AddDays(38),
        //            COC_ERF_DAT = DateTime.Now.AddDays(-2),

        //            // Farbe
        //            //ZBII_R = "blau",
        //            //ZBII_11 = "2",
        //        });
        //    Sap.ApplyImport(importList);
        //    Sap.Execute();

        //    var message = Sap.ResultMessage;
        //}

        //static IEnumerable<Z_DPM_READ_COC_01.GT_OUT> GetCocAuftraege(string aktion)
        //{
        //    return Z_DPM_READ_COC_01.GT_OUT.GetExportListWithInitExecute(Sap, "I_AG, I_AKTION", KUNNR_Tesla, aktion);
        //}

        //static void StoreTest()
        //{
        //    var vm = new TestViewModel();

        //    var list = vm.MyNames;
        //    var list2 = vm.MyNames;

        //    vm.Reset();
        //    vm.Reset();
        //    var list3 = vm.MyNames;
        //    var list4 = vm.MyNames;
        //    var list5 = vm.MyNames;
        //    vm.Set(new List<string>{ "HSV", "Bayern" });
        //    var list6 = vm.MyNames;
        //}

        //private static void BrieflebenslaufTest()
        //{
        //    var exportTables = Sap.GetExportTablesWithInitExecute("Z_M_BRIEFLEBENSLAUF_001",
        //                                                          "I_KUNNR, I_EQUNR", "0000336070", "000000000011760130")
        //                          .ToList();
        //    var rows = exportTables[0].Rows;
        //    var columns = exportTables[0].Columns;

        //    var obj = rows[0]["ENGINE_POWER"];
        //}

        ////static void Z_DPM_READ_AUTOACT_01_Test()
        ////{
        ////    // Variante 1, DataTable (ALT)
        ////    //var exportTables = Sap.GetExportTablesWithInitExecute("Z_DPM_READ_AUTOACT_01",
        ////    //                                                      "I_KUNNR_AG, I_STATUS", "0000336070", "2")
        ////    //                      .ToList();
            
        ////    // Variante 2, ORM
        ////    var list = Z_DPM_READ_AUTOACT_01.GT_OUT.GetExportListWithInitExecute(Sap, "I_KUNNR_AG, I_STATUS", "312680", "1");
        ////    var count = list.Count;

        ////    //Sap.InitExecute("Z_DPM_SAVE_STATUS_AUTOACT_01", "I_BELEGNR,I_STATUS", "11", "1");
        ////}

        //static void AvisTest5()
        //        {
        //    var list = Z_M_READ_ZUL_001.GT_WEB.GetExportListWithInitExecute(Sap,
        //                    "I_KUNNR_AG, I_ZULDAT_VON, I_ZULDAT_BIS, I_DAT_FREIS_ZUL_VON, I_DAT_FREIS_ZUL_BIS, I_VERWENDUNGSZWECK",
        //                    "314582".ToSapKunnr(), new DateTime(2013, 8, 1), new DateTime(2013, 8, 31), null, null, null);
        //    var count = list.Count;

        //}

        //static void SilvergreenTest()
        //    {
        //    //var list = Z_DPM_TYPDATEN_02.GT_WEB.GetExportListWithInitExecute(Sap,
        //    //                "I_ZZHERSTELLER_SCH, I_ZZTYP_SCHL, I_ZZVVS_SCHLUESSEL",
        //    //                "1480", "AAC", "00003");
        //    //var count = list.Count;

        //    var list = Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB.GetExportListWithInitExecute(Sap,
        //        "I_KUNNR, I_KENNUNG, I_POS_KURZTEXT",
        //        "0000315638", "", "10");
        //    var count = list.Count;
        //    if (list.None())
        //        return;

        //    var adresse = list[0];
        //    adresse.NAME2 = "x";
        //}


        ////static void HannoverLeasingTest()
        ////{
        ////    Z_DPM_IMP_DAT_RUECKL_01.Init(Sap);

        ////    Sap.SetImportParameter("I_KUNNR_AG", "10048516");

        ////    var gtdatList = Z_DPM_IMP_DAT_RUECKL_01.GT_DAT.GetImportList(Sap);
        ////    gtdatList.Add(new Z_DPM_IMP_DAT_RUECKL_01.GT_DAT
        ////        {
        ////            VORGANGS_ID = "87389011",
        ////        });
        ////    Sap.ApplyImport(gtdatList);

        ////    var gtdatListRep = Z_DPM_IMP_DAT_RUECKL_01.GT_REP.GetImportList(Sap);
        ////    gtdatListRep.Add(new Z_DPM_IMP_DAT_RUECKL_01.GT_REP
        ////    {
        ////        VORGANGS_ID = "87389011",
        ////    });
        ////    Sap.ApplyImport(gtdatListRep);


        ////    Sap.Execute();

        ////    var gtdatExportList = Z_DPM_IMP_DAT_RUECKL_01.GT_DAT.GetExportList(Sap);
        ////    var gtReps = Z_DPM_IMP_DAT_RUECKL_01.GT_REP.GetExportList(Sap);
        ////    var gtTransps = Z_DPM_IMP_DAT_RUECKL_01.GT_TRANSP.GetExportList(Sap);
        ////}

        //static void ScheinkopienTest()
        //{
        //    var list = Z_DPM_READ_SCHEINKOPIEN.GT_OUT.GetExportListWithInitExecute(Sap,
        //        "I_KUNNR, I_DATUM_VON, I_DATUM_BIS, I_KUNDE_PG",
        //        "0000300997", "01.10.2013", "11.11.2013", "X");
        //    var count = list.Count;
        //    if (list.None())
        //        return;

        //}

        //static void CardocuEquiTest()
        //{
        //    DateTime? xx = DateTime.Now;
        //    var sss = xx.NotNullOrEmptyToString();

        //    Z_DPM_CD_READ_GRUEQUIDAT_02.Init(Sap, "I_AG", "0010051385");
            
        //    Sap.Execute();

        //    //var sapItemsEquisTable = Sap.GetExportTable("GT_OUT");
        //    //var rows = sapItemsEquisTable.Rows;
        //    //var rowCount = rows.Count;


        //    var sapItemsEquis = Z_DPM_CD_READ_GRUEQUIDAT_02.GT_OUT.GetExportList(Sap);
        //    var webItemsEquis = AppModelMappings.Z_DPM_CD_READ_GRUEQUIDAT_02_GT_OUT_To_GrunddatenEqui.Copy(sapItemsEquis).ToList(); //.OrderBy(w => w.Fahrgestellnummer).ToList();
        //}

            

        ////static void FehlteilEtikettTestOld()
        ////{
        ////    var con = CreateErpConnection(Sap.SapConnection);
        ////    con.Open(false);

        ////    var func = con.CreateFunction("Z_DPM_DRUCK_FEHLTEILETIK");
            
        ////    // read:
        ////    func.Exports["I_AG"].ParamValue = "314582";
        ////    func.Exports["I_VERART"].ParamValue = "L";
        //    //func.Exports["I_POSITION"].ParamValue = "1";
        ////    //func.Exports["I_PREVIEW"].ParamValue = "X";

        ////    var changeParam = (RFCStructure)func.Changings["IO_ETIKETT"].ParamValue;
        ////    changeParam.BeginEdit();
        ////    changeParam["CHASSIS_NUM"] = "DE87654321";
        ////    changeParam.EndEdit();

        ////    func.Execute();

        ////    var chassis = changeParam["CHASSIS_NUM"];
        ////    var header1 = changeParam["UEBERSCHRIFT_1"];
        ////    var content1 = changeParam["INHALT_1"];


        ////    // save:
        ////    func = con.CreateFunction("Z_DPM_DRUCK_FEHLTEILETIK");
        ////    func.Exports["I_AG"].ParamValue = "314582";
        ////    func.Exports["I_VERART"].ParamValue = "S";
        ////    //func.Exports["I_POSITION"].ParamValue = "1";

        ////    changeParam = (RFCStructure)func.Changings["IO_ETIKETT"].ParamValue;
        ////    changeParam.BeginEdit();
        ////    changeParam["CHASSIS_NUM"] = "DE87654321";
        ////    changeParam["INHALT_1"] = content1 + ".";
        ////    changeParam.EndEdit();
        ////    func.Execute();
            
        ////    var pdf = func.Imports["E_PDF"].ParamValue;
        ////}

        //private static void FehlteilEtikettTest()
        //{
        //    //var s = "1,23456789012346E+15";
        //    //Double d;
        //    //Double.TryParse(s, NumberStyles.Float, new NumberFormatInfo { NumberDecimalSeparator = "," }, out d);
        //    //var sd = d.ToString("0");

        //    Z_DPM_DRUCK_FEHLTEILETIK.Init(Sap, "I_AG", "314582".ToSapKunnr());

        //    Sap.SetImportParameter("I_VERART", "L");
        //    Sap.SetImportParameter("I_POSITION", "1");
        //    //Sap.SetImportParameter("I_PREVIEW", "X");

        //    var importList = Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT.GetImportList(Sap);
        //    importList.Add(new Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT { CHASSIS_NUM = "DE87654323" });
        //    Sap.ApplyImport(importList);

        //    Sap.Execute();

        //    var exportList = Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT.GetExportList(Sap);
        //    if (exportList.Any())
        //{
        //        var item = exportList[0];
        //        item.INHALT_1 += ".";

        //        Z_DPM_DRUCK_FEHLTEILETIK.Init(Sap, "I_AG", "314582".ToSapKunnr());
        //        Sap.SetImportParameter("I_VERART", "S");
        //        item.LICENSE_NUM = "OD-J 1040";

        //        importList = Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT.GetImportList(Sap);
        //        importList.Add(item);
        //        Sap.ApplyImport(importList);

        //        Sap.Execute();
        //}

        //    var eSubrc = Sap.GetExportParameter("E_SUBRC");
        //    var eMessage = Sap.GetExportParameter("E_MESSAGE");
        //    var pdfString = Sap.GetExportParameterByte("E_PDF");

        //    var list = Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT.GetExportList(Sap);
        //    if (list.Any())
        //    {
        //        var item = list[0];
        //    }
        //}

        ////private static R3Connection CreateErpConnection(ISapConnection sapConnection = null)
        ////{
        ////    if (sapConnection == null)
        ////        throw new Exception("SAP Connection not initialized!");

        ////    if (string.IsNullOrEmpty(sapConnection.ErpConnectLicense))
        ////        throw new Exception("SAP 'ERPConnectLicense' is empty!");

        ////    var conn = new R3Connection(sapConnection.SAPAppServerHost, sapConnection.SAPSystemNumber, sapConnection.SAPUsername, sapConnection.SAPPassword, "DE", Convert.ToInt16(sapConnection.SAPClient).ToString());

        ////    LIC.SetLic(sapConnection.ErpConnectLicense);

        ////    return conn;
        ////}

        //static void ZulassungsServicesTest()
        //{
        //    var transportType = "1";

        //    Z_DPM_READ_LV_001.Init(Sap, "I_VWAG", "X");

        //    var importListAG = Z_DPM_READ_LV_001.GT_IN_AG.GetImportList(Sap);
        //    importListAG.Add(new Z_DPM_READ_LV_001.GT_IN_AG { AG = "0010010753".ToSapKunnr() });
        //    Sap.ApplyImport(importListAG);

        //    var importListProcess = Z_DPM_READ_LV_001.GT_IN_PROZESS.GetImportList(Sap);
        //    importListProcess.Add(new Z_DPM_READ_LV_001.GT_IN_PROZESS { SORT1 = "1" });
        //    Sap.ApplyImport(importListProcess);

        //    Sap.Execute();

        //    // Zulassungs-Dienstleistungen
        //    var list = Z_DPM_READ_LV_001.GT_OUT_DL.GetExportList(Sap)
        //        .Where(x => x.ASNUM.IsNotNullOrEmpty() && x.EXTGROUP == transportType && x.KTEXT1_H2.IsNullOrEmpty())
        //        .ToList();
        //    var listSelected = list.Where(x => x.VW_AG.IsNotNullOrEmpty()).ToList();
        //}

        //static void UeberfuehrungHistoryTest()
        //{
        //    var historyAuftragFilter = new HistoryAuftragFilter
        //    {
        //        KundenNr = "261010".ToSapKunnr(),

        //        UeberfuehrungsDatumVon = DateTime.Parse("01.10.2013"),
        //        UeberfuehrungsDatumBis = DateTime.Parse("31.10.2013"),
        //        AlleOrganisationen = true,
        //        AuftragsArt = "A",
        //    };

        //    var importList = Z_V_Ueberf_Auftr_Kund_Port.T_SELECT.GetImportListWithInit(Sap);
        //    var model = AppModelMappings.Z_V_Ueberf_Auftr_Kund_Port_T_SELECT_To_HistoryAuftragFilter.CopyBack(historyAuftragFilter);
        //    importList.Add(model);
        //    Sap.ApplyImport(importList);
        //    Sap.Execute();
        //    var sapAuftraege = Z_V_Ueberf_Auftr_Kund_Port.T_AUFTRAEGE.GetExportList(Sap);

        //    var list = AppModelMappings.Z_V_Ueberf_Auftr_Kund_Port_T_AUFTRAEGE_To_HistoryAuftrag.Copy(sapAuftraege).ToList();
        //}


        //static void FahrerVerfuegbarkeitTestRead()
        //{
            //var fahrerNr = "499930";
            //var list = Z_V_Ueberf_Verfuegbarkeit1.T_VERFUEG1.GetExportListWithInitExecute(Sap,
            //    "I_FAHRER, I_VONDAT, I_BISDAT",
            //    fahrerNr.ToSapKunnr(), "10022014", "14122099");

            //var count = list.Count;
        //}

        //static void FahrerVerfuegbarkeitTestUpd()
        //{
        //    var fahrerNr = "499930";
        //    var list = Z_V_UEBERF_VERFUEGBARKEIT2.GT_FAHRER.GetImportListWithInit(Sap,
        //        "I_FAHRER", fahrerNr.ToSapKunnr());

        //    list.Add(new Z_V_UEBERF_VERFUEGBARKEIT2.GT_FAHRER
        //    {
        //        VERDAT = "20140210",
        //        ANZ_FAHRER = "7"
        //    });
        //    Sap.ApplyImport(list);
        //    Sap.Execute();

        //    var xxx = DateTime.Now.ToString("yyyyMMdd");

        //    var result = Sap.ResultCode;
            //}



        //static void FahrerQmTestRead()
        //{
        //    //var fahrerNr = "499930";
        //    //var list = Z_V_Ueberf_Verfuegbarkeit1.T_VERFUEG1.GetExportListWithInitExecute(Sap,
        //    //    "I_FAHRER, I_VONDAT, I_BISDAT",
        //    //    fahrerNr.ToSapKunnr(), "10022014", "14122099");

        //    //var count = list.Count;

        //    //var pdf = Sap.GetExportParameterByteWithInitExecute("Z_M_CRE_FAHRER_AUFTRAG_PDF", "E_XSTRING", "I_VBELN", "24436273".PadLeft10());

        //    //var destinationFileName = @"C:\Users\JenzenM\Pictures\Jenzen\Kristina Abitur\1000000000627-left-rotated.png";
        //    //var thumbnailFileName = @"C:\Users\JenzenM\Pictures\Jenzen\Kristina Abitur\test\tina-4.jpg";
        //    //ImagingService.ScaleAndSaveImage(destinationFileName, thumbnailFileName, 200);

        //    //var folder = @"C:\Users\JenzenM\Pictures\Jenzen\Kristina Abitur\test\test";
        //    //foreach (var file in Directory.GetFiles(folder))
        //    //{
        //    //    var tbFile = Path.Combine(folder, "_tb");
        //    //    tbFile = Path.Combine(tbFile, Path.GetFileName(file));
        //    //    ImagingService.ScaleAndSaveImage(file, tbFile, 200);
        //    //}
        //}

        //static void FahrerQmTestUpd()
        //{
        //    var fahrerNr = "499930";
        //    var list = Z_V_UEBERF_VERFUEGBARKEIT2.GT_FAHRER.GetImportListWithInit(Sap,
        //        "I_FAHRER", fahrerNr.ToSapKunnr());

        //    list.Add(new Z_V_UEBERF_VERFUEGBARKEIT2.GT_FAHRER
        //    {
        //        VERDAT = "20140210",
        //        ANZ_FAHRER = "7"
        //    });
        //    Sap.ApplyImport(list);
        //    Sap.Execute();

        //    var xxx = DateTime.Now.ToString("yyyyMMdd");

        //    var result = Sap.ResultCode;
        //}

        //static void FahrerQmTestFleet()
        //{
        //    Z_UEB_FAHRER_QM.Init(Sap, 
        //        "I_LIFNR, I_DATAB, I_DATBI", 
        //        "490454".ToSapKunnr(), DateTime.Parse("01.01.2013"), DateTime.Parse("31.12.2013"));
        //    Sap.Execute();

        //    var qmList = Z_UEB_FAHRER_QM.ET_QM.GetExportList(Sap);
        //    var etList = Z_UEB_FAHRER_QM.ET_FLEET.GetExportList(Sap);

        //    var rankingCount = Sap.GetExportParameter("E_RANKING_COUNT");
        //}

        //static void ZulassungsStellenTest()
        //{
        //    Z_Get_Zulst_By_Plz.Init(Sap);
        //    Sap.Execute();

        //    var orte = Z_Get_Zulst_By_Plz.T_ORTE.GetExportList(Sap);
        //    var zulst = Z_Get_Zulst_By_Plz.T_ZULST.GetExportList(Sap);
        //    var od = zulst.FirstOrDefault(z => z.ZKBA1 == "01062");
        //    var barg = zulst.FirstOrDefault(z => z.PSTLZ == "22941");
        //}

        //static readonly string KunnrCsi = "0000004711";

        //static void CsiTest()
        //{
        //    CsiInsert();
        //    //CsiUpdate();
        //    //CsiRead();
        //}

        //static void CsiRead()
        //{
        //    var list = Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT.GetExportListWithInitExecute(Sap,
        //                "I_KUNNR_AG, I_AKTION",
        //                KunnrCsi.ToSapKunnr(),
        //                "R");

        //    var listCount = list.Count;
        //}

        //static void CsiUpdate()
        //{
        //    var list = Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT.GetImportListWithInit(Sap,
        //        "I_KUNNR_AG, I_AKTION, I_EVENT",
        //        KunnrCsi.ToSapKunnr(),
        //        "U",
        //        "0000000014");
        
        //    list.Add(new Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT
        //    {
        //        EVENT = "0000000014",
        //        EVENT_NAME = "Test-Event 2-2",
        //        BESCHREIBUNG = "Dies ist ein Test-Event 2-2",
        //        STARTDATUM = DateTime.Now.AddMonths(1),
        //        ENDDATUM = DateTime.Now.AddMonths(10),
        //        ANLAGEDATUM = DateTime.Now,
        //        ANLAGEUSER = "MJE",
        //        LOESCHDATUM = null, //DateTime.Now,
        //        LOESCHUSER = null, //"MJE",
        //    });
        //    Sap.ApplyImport(list);
        //    Sap.Execute();

        //    var result = Sap.ResultCode;
        //}

        //static void CsiInsert()
        //{
        //    var list = Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT.GetImportListWithInit(Sap,
        //        "I_KUNNR_AG, I_AKTION", 
        //        KunnrCsi.ToSapKunnr(), 
        //        "I");

        //    list.Add(new Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT
        //    {
        //        EVENT = "0",
        //        EVENT_NAME = "Test-Event 3",
        //        BESCHREIBUNG = "Dies ist ein Test-Event 3",
        //        STARTDATUM = DateTime.Now.AddMonths(1),
        //        ENDDATUM = DateTime.Now.AddMonths(10),
        //        ANLAGEDATUM = DateTime.Now,
        //        ANLAGEUSER = "MJE"
        //    });
        //    Sap.ApplyImport(list);
        //    Sap.Execute();

        //    var result = Sap.ResultCode;

        //    var exportList = Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT.GetExportList(Sap);
        //    var savedItem = exportList.FirstOrDefault();
        //}

        //static readonly string KunnrTargo = "0010026883";
        
        //static void TargoTest3()
        //{
        //    var list = Z_dpm_Read_Prot_Telefonate_01.GT_OUT.GetExportListWithInitExecute(Sap,
        //                "I_AG",
        //                KunnrTargo.ToSapKunnr()
            //            );

            //var listCount = list.Count;
        //}

        //static readonly string KunnrLueg = "240042"; // 0000329245

        //static void AhpZullisteTest()
        //{
        //    var list2 = Z_ZLD_AH_ZULST_BY_PLZ.T_ZULST.GetExportListWithInitExecute(Sap);

        //    var hhs = list2.Where(l => l.ZKREIS.ToUpper().StartsWith("HAMBURG")).ToList();
        
        //    //var list = Z_ZLD_AH_ZULLISTE.GT_OUT.GetExportListWithInitExecute(Sap,
        //    //            "I_KUNNR, I_GRUPPE, I_VKORG, I_VKBUR, I_ZZZLDAT_VON, I_ZZZLDAT_BIS, I_LISTE",
        //    //                "",
        //    //                "LUEG_BOCHUM",
        //    //                "1010",
        //    //                "4340",
        //    //                DateTime.Today.AddMonths(-2),
        //    //                DateTime.Today,
        //    //                "1"
        //    //            );

        //    //var listCount = list.Count;
        //}

        //const string ModellIdKunnr = "0000219853";

        //static void ModellIdTest()
        //{
        //    ModellIdSave();
        //}

        //static void ModellIdLoad()
        //{
        //    var list2 = Z_DPM_READ_MODELID_TAB.GT_OUT.GetExportListWithInitExecute(Sap, "I_KUNNR", ModellIdKunnr);
        //}
        
        //static void ModellIdSave()
        //{
        //    Z_DPM_CHANGE_MODELID.Init(Sap, "I_KUNNR", ModellIdKunnr);

        //    Sap.SetImportParameter("I_VERKZ", "N");
        //    Sap.SetImportParameter("I_MODELID", "BMW 327I");
        //    Sap.SetImportParameter("I_ZZBEZEI", "Dies ist ein Test");
        //    Sap.SetImportParameter("I_ZSIPP_CODE", "4711");
        //    Sap.SetImportParameter("I_HERST", "BMW");
        //    Sap.Execute();

        //    var resultCode = Sap.ResultCode;
        //    var resultMessage = Sap.ResultMessage;
        //}


        #region Chart Table Export

        private static void ErpBulkCopySqlTest_Autovermieter()
        {
            ErpTableBulkCopySql(
                "ZDAD_V_REM_DATEN",
                "KUNNR,FAHRGNR,MODELL,AVNR,AUSLDAT,KENNZ,BRIEFNR,ZULDAT,NUMMER_RE",
                "AUSLDAT,ZULDAT", 
                sqlConnection => new SqlCommand(
                    "update ZDAD_V_REM_DATEN set ZULDAT = '00000000' where ZULDAT = '10200222'", sqlConnection).ExecuteNonQuery());
        }


        private static void ErpBulkCopySqlTest_FeinstaubPlaketten()
        {
            ErpTableBulkCopySql( 
                "ZFIL_FSP_VK",
                "VKBUR,VKORG,BONNR,KENNZ,KUNNR, SELLDAT",
                "SELLDAT",
                null);
        }

        private static void ErpBulkCopySqlTest_KennzeichenVerkauf()
        {
            ErpTableBulkCopySql(
                "ZFIL_KENNZ_VK",
                "VKBUR,BONNR,KENNZ,KUNNR,SELLDAT,MATNR",
                "SELLDAT",
                null);
        }



        private static void ErpTableBulkCopySql(string tableName, string columnNames, string dateColumnNamesToConvert, Action<SqlConnection> preDateConvertSqlStatement)
        {
            var sc = SapProd.SapConnection;
            var con = new R3Connection(sc.SAPAppServerHost, sc.SAPSystemNumber, sc.SAPUsername, sc.SAPPassword, "DE", Convert.ToInt16(sc.SAPClient).ToString());
            LIC.SetLic(sc.ErpConnectLicense);
            con.Open(false);

            var table = new ERPConnect.Utils.ReadTable(con) { TableName = tableName };
            //table.AddCriteria("FAHRGNR = 'WVGZZZ1TZBW095262'");

            columnNames.Split(',').ToList().ForEach(table.AddField);
            //table.RowCount = 2000000;
            //table.RowCount = 10000;

            //table.WhereClause = string.Format("SELLDAT_TIM >= '2013-01-01' and SELLDAT_TIM < '2013-03-31'");
            //table.WhereClause = string.Format("SELLDAT_TIM >= '2013-04-01' and SELLDAT_TIM < '2013-06-30'"); 
            //table.WhereClause = string.Format("SELLDAT_TIM >= '2013-07-01' and SELLDAT_TIM < '2013-09-30'"); 
            //table.WhereClause = string.Format("SELLDAT_TIM >= '2013-10-01' and SELLDAT_TIM < '2013-12-31'"); 
            //table.WhereClause = string.Format("SELLDAT_TIM >= '2014-01-01'");

            table.Run();

            // Datums-Konvertierung
            SqlBulkCopy(table.Result, table.TableName,
                        sqlConnection =>
                            {
                                // Daten-Cleaning:
                                if (preDateConvertSqlStatement != null)
                                    preDateConvertSqlStatement(sqlConnection);


                                if (dateColumnNamesToConvert != null)
                                {
                                    // Datums-Konvertierung:
                                    new SqlCommand(string.Format("update {0} set {1}",
                                                                 table.TableName,
                                                                 dateColumnNamesToConvert.Split(',')
                                                                                         .Select(
                                                                                             GetSqlDateUpdateStatement)
                                                                                         .JoinIfNotNull(",")), sqlConnection).ExecuteNonQuery();
                                }
                            }
                );
        }

        static string GetSqlDateUpdateStatement(string columnName)
        {
            return string.Format("{0}_DATE = case when ({0} is null or isnull({0},'00000000') = '00000000') then null else convert(datetime, SUBSTRING({0},1,4) + '-' + SUBSTRING({0},5,2) + '-' + SUBSTRING({0},7,2), 110) end ", columnName);
        }

        static void SqlBulkCopy(DataTable table, string tableName, Action<SqlConnection> postSqlStatement)
        {
            var connectionString = "Data Source=VMS026;Initial Catalog=KBA;User ID=DADWebAccess;Password=seE?Anemone;Persist Security Info=True;";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                new SqlCommand(string.Format("delete from {0}", tableName), connection).ExecuteNonQuery();

                //Open bulkcopy connection.
                using (var bulkcopy = new SqlBulkCopy(connection))
                {
                    //Set destination table name
                    //to table previously created.
                    bulkcopy.DestinationTableName = tableName;

                    try
                    {
                        bulkcopy.WriteToServer(table);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        
                    }

                    if (postSqlStatement != null) 
                        postSqlStatement(connection);

                    connection.Close();
                }
            }
        }

        #endregion
    }
}
    
