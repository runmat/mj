﻿// ReSharper disable InconsistentNaming

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

                            //destination.ZMODEL_ID_VON
                            //destination.ZMODEL_ID_BIS

                            //destination.ZPURCH_MTH_VON
                            //destination.ZPURCH_MTH_BIS

                            //destination.ZBATCH_ID_VON
                            //destination.ZBATCH_ID_BIS

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
                            destination.AuftragsnummerVon = source.ZAUFNR_VON;
                            destination.AuftragsnummerBis = source.ZAUFNR_BIS;
                            destination.Bemerkung = source.ZBEMERKUNG;
                            
                            int i = 0; // TODO -> Type?
                            int.TryParse(source.ZANZAHL, out i);
                            destination.Anzahl = i.ToString();
                            
                            string[] dat = source.ZPURCH_MTH.Split('.');
                            destination.Liefermonat = dat.Length == 2 ? dat[0] : "";
                            destination.Lieferjahr = dat.Length == 2 ? dat[1] : "";
                            destination.LiefermonatBAPIFormat = source.ZPURCH_MTH;
                            
                            destination.Status = source.STATUS;

                            destination.Winterreifen = (source.ZMS_REIFEN.ToUpper() == "X");
                            destination.SecurityFleet = (source.ZSECU_FLEET.ToUpper() == "X");
                            destination.KennzeichenLeasingFahrzeug = (source.ZLEASING.ToUpper() == "X");
                            destination.NaviVorhanden = (source.ZNAVI.ToUpper() == "X");    

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
                            destination.HerstellerCode = source.HERST;                           
                            destination.HerstellerName = source.HERST_T;
                            destination.Antrieb = source.ANTR;
                            destination.Bluetooth = (source.BLUETOOTH.ToUpper() == "X");                            
                            destination.AnhaengerKupplung = (source.AHK.ToUpper() == "X");
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
                            destination.ZFZG_GROUP = "PKW";
                            destination.ZLAUFZEIT = source.Laufzeit;
                            destination.ZLZBINDUNG = source.Laufzeitbindung ? "X" : "";
                            destination.ZAUFNR_VON = source.AuftragsnummerVon;
                            destination.ZAUFNR_BIS = source.AuftragsnummerBis;
                            destination.ZMS_REIFEN = source.Winterreifen ? "X" : "";
                            destination.ZSECU_FLEET = source.SecurityFleet ? "X" : "";
                            destination.ZLEASING = source.KennzeichenLeasingFahrzeug ? "X" : "";
                            destination.ZNAVI = source.NaviVorhanden ? "X" : "";
                            destination.ZAHK = source.AnhaengerKupplung ? "X" : "";
                            // destination.ZBEMERKUNG = source.Bemerkung;

                        }
                    ));
            }
        }



    }
}