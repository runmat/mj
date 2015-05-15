// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.FzgModelle.Models;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class BatchModelMappings : ModelMappings
    {
        static public ModelMapping<Z_M_EC_AVM_BATCH_SELECT.GT_IN, BatcherfassungSelektor> Z_M_EC_AVM_BATCH_SELECT_GT_IN_From_Batcherfassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_BATCH_SELECT.GT_IN, BatcherfassungSelektor>(
                        new Dictionary<string, string>()
                        , null
                        , (source, destination) =>
                        {

                            source.UnitNummerVon = source.UnitNummerVon.IsNullOrEmpty() ? String.Empty : source.UnitNummerVon;
                            source.UnitNummerBis = source.UnitNummerBis.IsNullOrEmpty() ? String.Empty : source.UnitNummerBis;
                            destination.ZUNIT_NR_VON = source.UnitNummerVon.PadLeft(8, '0');
                            destination.ZUNIT_NR_BIS = source.UnitNummerBis.PadLeft(8, '0');                                                        
                            source.Auftragnummer = source.Auftragnummer.IsNullOrEmpty() ? String.Empty : source.Auftragnummer;
                            destination.ZAUFNR = source.Auftragnummer.PadLeft(12, '0');

                            destination.ERDAT_VON = source.AnalageDatumRange.StartDate;
                            destination.ERDAT_BIS = source.AnalageDatumRange.EndDate;
                        }
                    ));
            }
        }


        static public ModelMapping<Z_M_EC_AVM_BATCH_SELECT.GT_OUT, Batcherfassung> Z_M_EC_AVM_BATCH_SELECT_GT_OUT_To_Batcherfassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_BATCH_SELECT.GT_OUT, Batcherfassung>(
                        new Dictionary<string, string>()
                        , (source, destination) =>
                        {
                            destination.ID = source.ZBATCH_ID;
                            destination.ModellId = source.ZMODEL_ID;
                            destination.Modellbezeichnung = source.ZMOD_DESCR;
                            destination.SippCode = source.ZSIPP_CODE;
                            destination.HerstellerName = source.ZMAKE;
                            destination.UnitnummerVon = source.ZUNIT_NR_VON;
                            destination.UnitnummerBis = source.ZUNIT_NR_BIS;
                            destination.Laufzeit = source.ZLAUFZEIT;
                            destination.Laufzeitbindung = (source.ZLZBINDUNG.ToUpper() == "X");
                            destination.AuftragsnummerVon = source.ZAUFNR_VON.TrimStart(new Char[] { '0' }); 
                            destination.AuftragsnummerBis = source.ZAUFNR_BIS.TrimStart(new Char[] { '0' }); 
                            destination.Bemerkung = source.ZBEMERKUNG;
                            destination.Fahrzeuggruppe = source.ZFZG_GROUP;
                            destination.Verwendung = source.ZVERWENDUNG;                          
                            destination.Anzahl = source.ZANZAHL;
                            
                            string[] dat = source.ZPURCH_MTH.Split('.');
                            destination.Liefermonat = dat.Length == 2 ? dat[0] : "";
                            destination.Lieferjahr = dat.Length == 2 ? dat[1] : "";
                            destination.LiefermonatBAPIFormat = source.ZPURCH_MTH;
                            
                            destination.Status = source.STATUS;

                            destination.Winterreifen = (source.ZMS_REIFEN.ToUpper() == "X");
                            destination.SecurityFleet = (source.ZSECU_FLEET.ToUpper() == "X");
                            destination.KennzeichenLeasingFahrzeug = (source.ZLEASING.ToUpper() == "X");
                            destination.NaviVorhanden = (source.ZNAVI.ToUpper() == "X");
                            destination.AnhaengerKupplung = (source.ZAHK.ToUpper() == "X");
                            
                            if(source.ZLEASING.Length > 0 || source.ZSECU_FLEET.Length > 0)
                                destination.Vertragsart = source.ZLEASING.ToUpper() == "X" ? "Leasing" :
                                                                                             "Securiti Fleet";
                           
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
                        , (source, destination) =>
                        {
                            destination.ModelID = source.ZZMODELL;
                            destination.Modellbezeichnung = source.ZZBEZEI;
                            destination.HerstellerCode = source.HERST;                           
                            destination.HerstellerName = source.HERST_T;
                            destination.SippCode = source.SIPP1 + source.SIPP2 + source.SIPP3 + source.SIPP4;
                            destination.Laufzeit = source.ZLAUFZEIT;
                            destination.Laufzeitbindung = (source.ZLZBINDUNG.ToUpper() == "X");
                            destination.Antrieb = source.ANTR;
                            destination.NaviVorhanden = (source.NAVI_VORH.ToUpper() == "X");  
                            destination.Winterreifen = (source.WINTERREIFEN.ToUpper() == "X");  
                            destination.SecurityFleet = (source.SECU_FLEET.ToUpper() == "X");
                            destination.KennzeichenLeasingFahrzeug = (source.LEASING.ToUpper() == "X"); 
                            destination.Bluetooth = (source.BLUETOOTH.ToUpper() == "X");                            
                            destination.AnhaengerKupplung = (source.AHK.ToUpper() == "X");
                            destination.Fahrzeuggruppe = (source.LKW.ToUpper() == "X") ? "LKW" : "PKW";
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
                        , (source, destination) =>
                        {
                            destination.ZBATCH_ID = source.ID;
                            destination.ZMODEL_ID = source.ModellId;
                            destination.ZSIPP_CODE = source.SippCode;
                            destination.ZMAKE = source.HerstellerName;
                            destination.ZMOD_DESCR = source.Modellbezeichnung;
                            destination.ZPURCH_MTH = source.LiefermonatBAPIFormat;
                            destination.ZANZAHL = source.Anzahl;
                            destination.ZUNIT_NR_VON = source.UnitnummerVon;
                            destination.ZUNIT_NR_BIS = source.UnitnummerBis;
                            destination.ZFZG_GROUP = source.Fahrzeuggruppe;
                            destination.ZLAUFZEIT = source.Laufzeit;
                            destination.ZLZBINDUNG = source.Laufzeitbindung ? "X" : "";
                            destination.ZAUFNR_VON = source.AuftragsnummerVon;
                            destination.ZAUFNR_BIS = source.AuftragsnummerBis;
                            destination.ZMS_REIFEN = source.Winterreifen ? "X" : "";
                            destination.ZSECU_FLEET = source.SecurityFleet ? "X" : "";
                            destination.ZLEASING = source.KennzeichenLeasingFahrzeug ? "X" : "";
                            destination.ZNAVI = source.NaviVorhanden ? "X" : "";
                            destination.ZAHK = source.AnhaengerKupplung ? "X" : "";
                            destination.ZBEMERKUNG = source.Bemerkung;
                            destination.ZERNAM = source.WebUser;
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
                        , (source, destination) =>
                        {
                            destination.Nummer = source.POS_KURZTEXT;
                            destination.AuftragsNrText = source.POS_TEXT;                            
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
                        , (source, destination) =>
                        {
                            destination.Unitnummer = source.ZUNIT_NR;                            
                            destination.Fahrgestellnummer = source.CHASSIS_NUM;
                            destination.Kennzeichen = source.LICENSE_NUM;
                            destination.IstGesperrt = (source.ZLOEVM.ToUpper() == "X");
                            destination.Einsteuerung = source.REPLA_DATE;
                        }
                    ));
            }
        }


        static public ModelMapping<Z_M_EC_AVM_BATCH_UPDATE.GT_WEB, Batcherfassung> Z_M_EC_AVM_BATCH_UPDATE_GT_WEB_IN_From_Batcherfassung
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_BATCH_UPDATE.GT_WEB, Batcherfassung>(
                        new Dictionary<string, string>()
                        , null
                        , (source, destination) =>
                        {
                            // TODO -> brauchen wir diese?
                            /*
                            destination.ZMODEL_ID = source.ModellId;
                            destination.ZSIPP_CODE = source.SippCode;
                            destination.ZMAKE = source.HerstellerName;
                            destination.ZMOD_DESCR = source.Modellbezeichnung;
                            destination.ZPURCH_MTH = source.LiefermonatBAPIFormat;
                            destination.ZANZAHL = source.Anzahl;                           
                            destination.ZFZG_GROUP = source.Fahrzeuggruppe;
                            destination.ZLAUFZEIT = source.Laufzeit;
                            
                            destination.ZAUFNR_VON = source.AuftragsnummerVon;
                            destination.ZAUFNR_BIS = source.AuftragsnummerBis;
                            destination.ZMS_REIFEN = source.Winterreifen ? "X" : "";
                            destination.ZSECU_FLEET = source.SecurityFleet ? "X" : "";
                            destination.ZLEASING = source.KennzeichenLeasingFahrzeug ? "X" : "";
                            destination.ZNAVI = source.NaviVorhanden ? "X" : "";
                            destination.ZAHK = source.AnhaengerKupplung ? "X" : "";
                            */
                              
                            destination.ZBATCH_ID = source.ID;
                            destination.ZUNIT_NR = source.UnitnummerUpdate;
                            destination.ZLZBINDUNG = source.Laufzeitbindung ? "X" : "";
                            destination.ZLOEVM = source.IstGesperrt ? "X" : "";
                            destination.ZDAT_SPERR = DateTime.Now;
                            destination.ZBEM_SPERR = source.Sperrvermerk;
                            destination.ZUSER_SPERR = source.WebUser;
                            destination.ZBEMERKUNG = source.Bemerkung;

                        }
                    ));
            }
        }

        static public ModelMapping<Z_M_EC_AVM_BATCH_INSERT.GT_IN, FzgUnitnummer> Z_M_EC_AVM_BATCH_INSERT_GT_IN_From_BatcherfassungUnitnummerVon
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_EC_AVM_BATCH_INSERT.GT_IN, FzgUnitnummer>(
                        new Dictionary<string, string>()
                        , null
                        , (source, destination) =>
                        {
                            destination.ZUNIT_NR = source.Unitnummer;                           
                        }
                    ));
            }
        }

    }
}