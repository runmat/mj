// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class BatchModelMappings : ModelMappings
    {
        #region From SAP

        static public ModelMapping<Z_M_EC_AVM_BATCH_SELECT.GT_OUT, Batcherfassung> Z_M_EC_AVM_BATCH_SELECT_GT_OUT_To_Batcherfassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_BATCH_SELECT.GT_OUT, Batcherfassung>(
                        new Dictionary<string, string>()
                        , (s, d) =>
                        {
                            d.ID = s.ZBATCH_ID;
                            d.ModellId = s.ZMODEL_ID;
                            d.Modellbezeichnung = s.ZMOD_DESCR;
                            d.SippCode = s.ZSIPP_CODE;
                            d.HerstellerName = s.ZMAKE;
                            d.UnitnummerVon = s.ZUNIT_NR_VON;
                            d.UnitnummerBis = s.ZUNIT_NR_BIS;
                            d.Laufzeit = s.ZLAUFZEIT;
                            d.Laufzeitbindung = s.ZLZBINDUNG.XToBool();
                            d.AuftragsnummerVon = s.ZAUFNR_VON.TrimStart('0');
                            d.AuftragsnummerBis = s.ZAUFNR_BIS.TrimStart('0');
                            d.Bemerkung = s.ZBEMERKUNG;
                            d.Fahrzeuggruppe = s.ZFZG_GROUP;
                            d.Verwendung = s.ZVERWENDUNG;
                            d.Anzahl = s.ZANZAHL;
                            d.Liefermonat = s.ZPURCH_MTH;
                            d.Status = s.STATUS;
                            d.Winterreifen = s.ZMS_REIFEN.XToBool();
                            d.SecurityFleet = s.ZSECU_FLEET.XToBool();
                            d.KennzeichenLeasingFahrzeug = s.ZLEASING.XToBool();
                            d.NaviVorhanden = s.ZNAVI.XToBool();
                            d.AnhaengerKupplung = s.ZAHK.XToBool();

                            if (s.ZLEASING.IsNotNullOrEmpty() || s.ZSECU_FLEET.IsNotNullOrEmpty())
                                d.Vertragsart = (s.ZLEASING.XToBool() ? "Leasing" : "Securiti Fleet");
                        }
                    ));
            }
        }

        static public ModelMapping<Z_DPM_READ_MODELID_TAB.GT_OUT, ModelHersteller> Z_DPM_READ_MODELID_TAB_GT_OUT_To_ModelHersteller
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_MODELID_TAB.GT_OUT, ModelHersteller>(
                        new Dictionary<string, string>()
                        , (s, d) =>
                        {
                            d.ModelID = s.ZZMODELL;
                            d.Modellbezeichnung = s.ZZBEZEI;
                            d.HerstellerCode = s.HERST;
                            d.HerstellerName = s.HERST_T;
                            d.SippCode = s.SIPP1 + s.SIPP2 + s.SIPP3 + s.SIPP4;
                            d.Laufzeit = s.ZLAUFZEIT;
                            d.Laufzeitbindung = s.ZLZBINDUNG.XToBool();
                            d.Antrieb = s.ANTR;
                            d.NaviVorhanden = s.NAVI_VORH.XToBool();
                            d.Winterreifen = s.WINTERREIFEN.XToBool();
                            d.SecurityFleet = s.SECU_FLEET.XToBool();
                            d.KennzeichenLeasingFahrzeug = s.LEASING.XToBool();
                            d.Bluetooth = s.BLUETOOTH.XToBool();
                            d.AnhaengerKupplung = s.AHK.XToBool();
                            d.Fahrzeuggruppe = (s.LKW.XToBool() ? "LKW" : "PKW");
                        }
                    ));
            }
        }

        static public ModelMapping<Z_DPM_READ_AUFTR_006.GT_OUT, Auftragsnummer> Z_DPM_READ_AUFTR_006_GT_OUT_To_Auftragsnummer
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_AUFTR_006.GT_OUT, Auftragsnummer>(
                        new Dictionary<string, string>()
                        , (s, d) =>
                        {
                            d.Nummer = s.POS_KURZTEXT;
                            d.AuftragsNrText = s.POS_TEXT;
                        }
                    ));
            }
        }

        static public ModelMapping<Z_M_EC_AVM_BATCH_UNIT_SELECT.GT_OUT, FzgUnitnummer> Z_M_EC_AVM_BATCH_UNIT_SELECT_GT_OUT_To_Unitnummer
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_BATCH_UNIT_SELECT.GT_OUT, FzgUnitnummer>(
                        new Dictionary<string, string>()
                        , (s, d) =>
                        {
                            d.Unitnummer = s.ZUNIT_NR;
                            d.Fahrgestellnummer = s.CHASSIS_NUM;
                            d.Kennzeichen = s.LICENSE_NUM;
                            d.IstGesperrt = s.ZLOEVM.XToBool();
                            d.Sperrvermerk = s.ZBEM_SPERR;
                            d.IsSelected = d.IstGesperrt;
                            d.Einsteuerung = s.REPLA_DATE;
                        }
                    ));
            }
        }

        #endregion

        #region To SAP

        static public ModelMapping<Z_M_EC_AVM_BATCH_SELECT.GT_IN, BatcherfassungSelektor> Z_M_EC_AVM_BATCH_SELECT_GT_IN_From_Batcherfassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_BATCH_SELECT.GT_IN, BatcherfassungSelektor>(
                        new Dictionary<string, string>()
                        , null
                        , (s, d) =>
                        {
                            s.UnitNummerVon = s.UnitNummerVon.NotNullOrEmpty();
                            s.UnitNummerBis = s.UnitNummerBis.NotNullOrEmpty();
                            d.ZUNIT_NR_VON = s.UnitNummerVon.PadLeft0(8);
                            d.ZUNIT_NR_BIS = s.UnitNummerBis.PadLeft0(8);
                            s.Auftragnummer = s.Auftragnummer.NotNullOrEmpty();
                            d.ZAUFNR = s.Auftragnummer.PadLeft0(12);

                            if (s.AnlageDatumRange.IsSelected)
                            {
                                d.ERDAT_VON = s.AnlageDatumRange.StartDate;
                                d.ERDAT_BIS = s.AnlageDatumRange.EndDate;
                            }
                        }
                    ));
            }
        }

        static public ModelMapping<Z_M_EC_AVM_BATCH_INSERT.ZBATCH_IN, Batcherfassung> Z_M_EC_AVM_BATCH_INSERT_ZBATCH_IN_From_Batcherfassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_BATCH_INSERT.ZBATCH_IN, Batcherfassung>(
                        new Dictionary<string, string>()
                        , null
                        , (s, d) =>
                        {
                            d.ZBATCH_ID = s.ID;
                            d.ZMODEL_ID = s.ModellId;
                            d.ZSIPP_CODE = s.SippCode;
                            d.ZMAKE = s.HerstellerName;
                            d.ZMOD_DESCR = s.Modellbezeichnung;
                            d.ZPURCH_MTH = s.Liefermonat;
                            d.ZANZAHL = s.Anzahl;
                            d.ZUNIT_NR_VON = s.UnitnummerVon;
                            d.ZUNIT_NR_BIS = s.UnitnummerBis;
                            d.ZFZG_GROUP = s.Fahrzeuggruppe;
                            d.ZLAUFZEIT = s.Laufzeit;
                            d.ZLZBINDUNG = s.Laufzeitbindung.BoolToX();
                            d.ZAUFNR_VON = s.AuftragsnummerVon;
                            d.ZAUFNR_BIS = s.AuftragsnummerBis;
                            d.ZMS_REIFEN = s.Winterreifen.BoolToX();
                            d.ZSECU_FLEET = s.SecurityFleet.BoolToX();
                            d.ZLEASING = s.KennzeichenLeasingFahrzeug.BoolToX();
                            d.ZNAVI = s.NaviVorhanden.BoolToX();
                            d.ZAHK = s.AnhaengerKupplung.BoolToX();
                            d.ZBEMERKUNG = s.Bemerkung;
                            d.ZERNAM = s.WebUser;
                        }
                    ));
            }
        }

        static public ModelMapping<Z_M_EC_AVM_BATCH_UPDATE.GT_WEB, FzgUnitnummer> Z_M_EC_AVM_BATCH_UPDATE_GT_WEB_IN_From_FzgUnitnummer
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_BATCH_UPDATE.GT_WEB, FzgUnitnummer>(
                        new Dictionary<string, string>()
                        , null
                        , (s, d) =>
                        {                           
                            d.ZBATCH_ID = s.ID;
                            d.ZUNIT_NR = s.Unitnummer;
                            d.ZLOEVM = s.IstGesperrt.BoolToX();
                            d.ZDAT_SPERR = DateTime.Now;
                            d.ZBEM_SPERR = s.Sperrvermerk;
                            d.ZUSER_SPERR = s.WebUser;
                        }
                    ));
            }
        }

        static public ModelMapping<Z_M_EC_AVM_BATCH_INSERT.GT_IN, FzgUnitnummer> Z_M_EC_AVM_BATCH_INSERT_GT_IN_From_FzgUnitnummer
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_BATCH_INSERT.GT_IN, FzgUnitnummer>(
                        new Dictionary<string, string>()
                        , null
                        , (s, d) =>
                        {
                            d.ZUNIT_NR = s.Unitnummer;
                        }
                    ));
            }
        }

        #endregion
    }
}