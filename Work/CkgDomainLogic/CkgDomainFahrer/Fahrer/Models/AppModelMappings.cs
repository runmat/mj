using System;
using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Fahrer.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region FromSap

// ReSharper disable InconsistentNaming
        static public ModelMapping<Z_V_UEBERF_VERFUEGBARKEIT1.T_VERFUEG1, FahrerTagBelegung> Z_V_Ueberf_Verfuegbarkeit1_T_VERFUEG1_To_FahrerTagBelegung
// ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_V_UEBERF_VERFUEGBARKEIT1.T_VERFUEG1, FahrerTagBelegung>(
                    new Dictionary<string, string> (),
                    // Copy
                    (sap, business) =>
                        {
                            var sapBelegung = sap.ANZ_FAHRER.NotNullOrEmpty().Trim().ToUpper();
                                                        
                            business.Datum = sap.VERFDAT.GetValueOrDefault();
                            business.FahrerID = sap.LIFNR;
                            business.FahrerAnzahl = 0;

                            switch (sapBelegung)
                            {
                                case "":
                                    business.BelegungsTyp = FahrerTagBelegungsTyp.Leer;
                                    break;
                                case "K": 
                                    business.BelegungsTyp = FahrerTagBelegungsTyp.Krank; 
                                    break;
                                case "U": 
                                    business.BelegungsTyp = FahrerTagBelegungsTyp.Urlaub; 
                                    break;
                                case "I":
                                    business.BelegungsTyp = FahrerTagBelegungsTyp.EingeschraenktVerfuegbar;
                                    business.Kommentar = sap.BEMERKUNG;
                                    break;
                                case "0": 
                                    business.BelegungsTyp = FahrerTagBelegungsTyp.NichtVerfuegbar; 
                                    business.Kommentar = sap.BEMERKUNG;
                                    break;
                                                            
                                default:
                                    int anzahl;
                                    if (Int32.TryParse(sapBelegung, out anzahl))
                                    {
                                        business.BelegungsTyp = FahrerTagBelegungsTyp.Verfuegbar;
                                        business.FahrerAnzahl = anzahl;
                                    }
                                    break;
                            }
                        }
                    ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_GET_FAHRER_AUFTRAEGE.GT_ORDER, FahrerAuftrag> Z_M_GET_FAHRER_AUFTRAEGE_GT_ORDER_To_FahrerAuftrag
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_GET_FAHRER_AUFTRAEGE.GT_ORDER, FahrerAuftrag>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.KundenNr = s.KUNNR_AG;

                        d.AuftragsNr = s.VBELN;
                        d.WunschLieferDatum = s.VDATU;

                        d.PlzStart = s.ZB_POST_CODE1;
                        d.OrtStart = s.ZB_CITY1;

                        d.PlzZiel = s.WE_POST_CODE1;
                        d.OrtZiel = s.WE_CITY1;

                        d.PlzRueck = s.ZR_POST_CODE1;
                        d.OrtRueck = s.ZR_CITY1;

                        d.FahrerStatus = s.FAHRER_STATUS;
                    }
                    ));
            }
        }

        static public ModelMapping<Z_DPM_READ_AUFTR_FAHR_EDISPO_1.GT_ORDER, FahrerAuftrag> Z_DPM_READ_AUFTR_FAHR_EDISPO_1_GT_ORDER_To_FahrerAuftrag
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_AUFTR_FAHR_EDISPO_1.GT_ORDER, FahrerAuftrag>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        
                        d.AuftragsNr = s.VBELN;
                        d.KundenName = s.NAME1_AG;
                        d.WunschLieferDatum = s.AUN_DAT;

                        d.PlzStart = s.POST_CODE1_ZB;
                        d.OrtStart = s.CITY1_ZB;

                        d.PlzZiel = s.POST_CODE1_WE;
                        d.OrtZiel = s.CITY1_WE;

                        d.PlzRueck = s.POST_CODE1_ZR;
                        d.OrtRueck = s.CITY1_ZR;

                        d.UebergabeZeitVon = s.AUN_TIM_VON_H;
                        d.UebergabeZeitBis = s.AUN_TIM_BIS_H;

                        d.UebernahmeZeitVon = s.AUN_TIM_VON_R;
                        d.UebernahmeZeitBis = s.AUN_TIM_BIS_R;
                        
                    }
                    ));
            }
        }



        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_V_UEBERF_AUFTR_FAHRER.T_AUFTRAEGE, FahrerAuftragsFahrt> Z_V_UEBERF_AUFTR_FAHRER_T_AUFTRAEGE_to_FahrerAuftragsFahrt
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_V_UEBERF_AUFTR_FAHRER.T_AUFTRAEGE, FahrerAuftragsFahrt>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.AuftragsNr = s.AUFNR;
                        d.Fahrt = s.FAHRTNR;

                        d.WunschLieferDatum = s.WADAT;

                        d.OrtStart = s.FAHRTVON;
                        d.OrtZiel = s.FAHRTNACH;

                        d.Kennzeichen = s.ZZKENN;
                        d.VIN = s.ZZFAHRG;
                        d.FahrzeugTyp = s.ZZBEZEI;
                    }
                    ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_V_UEBERF_AUFTR_UPL_PROT_01.GT_OUT, FahrerAuftragsProtokoll> Z_V_UEBERF_AUFTR_UPL_PROT_01_GT_OUT_to_FahrerAuftragsProtokoll
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_V_UEBERF_AUFTR_UPL_PROT_01.GT_OUT, FahrerAuftragsProtokoll>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.KundenNr = s.KUNNR_AG;

                        d.AuftragsNr = s.VBELN;
                        d.Referenz = s.ZZREFNR;

                        d.WunschLieferDatum = s.WADAT;

                        d.OrtStart = s.FAHRTVON;
                        d.OrtZiel = s.FAHRTNACH;

                        d.Kennzeichen = s.ZZKENN;
                        d.VIN = s.ZZFAHRG;

                        d.ProtokollArt = s.ZZPROTKAT1;
                        d.ProtokollArt2 = s.ZZPROTKAT2;

                        d.Fahrt = s.FAHRTNR;

                        d.ProtokollName = s.ZZPOSPROTKAT1.NotNullOr(
                                            s.ZZPOSPROTKAT2.NotNullOr(
                                                s.ZZPOSPROTKAT3.NotNullOr(
                                                    s.ZZPROTKAT1.NotNullOr(
                                                        s.ZZPROTKAT2.NotNullOr(
                                                            s.ZZPROTKAT3)))));
                    }
                    ));
            }
        }

        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_UEB_FAHRER_QM.ET_QM, QmFahrer> Z_UEB_FAHRER_QM_ET_QM_To_QmFahrer
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_UEB_FAHRER_QM.ET_QM, QmFahrer>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.CodeGruppe = s.FEGRP;
                        d.SchadensCode = s.FECOD;
                        d.KatalogText = s.KATALOGTXT;
                        d.MengeFehler = s.MENGE_SEL;
                        d.MengeVorjahr = s.MENGE_VORJAHR;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_UEB_FAHRER_QM.ET_FLEET, QmFleetMonitor> Z_UEB_FAHRER_QM_ET_FLEET_To_QmFleetMonitor
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_UEB_FAHRER_QM.ET_FLEET, QmFleetMonitor>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.Bewertung = s.BEWERTUNG;
                        d.Freundlichkeit = s.FRAGE5;
                        d.Erscheinungsbild = s.FRAGE6;
                        d.Professionalitaet = s.FRAGE7;
                        d.Puenktlichkeit = s.FRAGE8;
                        d.GesamtEindruck = s.FRAGE9;
                        d.Einweisung = s.FRAGE10;
                        d.FahrzeugZustand = s.FRAGE11;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_DPM_QM_READ_QPCD.GT_OUTQPCD, SelectItem> Z_DPM_QM_READ_QPCD_GT_OUTQPCD_To_SelectItem
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_QM_READ_QPCD.GT_OUTQPCD, SelectItem>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.Key = s.CODE;
                        d.Text = s.CODETEXT;
                    }));
            }
        }

        #endregion


        #region ToSap

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_V_UEBERF_VERFUEGBARKEIT2.GT_FAHRER, FahrerTagBelegung> Z_V_UEBERF_VERFUEGBARKEIT2_GT_FAHRER_To_FahrerTagBelegung
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_V_UEBERF_VERFUEGBARKEIT2.GT_FAHRER, FahrerTagBelegung>(
                    new Dictionary<string, string>(),
                    // Copy 
                    null,
                    // Copy Back
                    (business, sap) =>
                        {
                            sap.VERDAT = business.Datum.ToString("yyyyMMdd");
                            switch (business.BelegungsTyp)
                            {
                                case FahrerTagBelegungsTyp.Leer:
                                    sap.ANZ_FAHRER = "  ";  // 2 blanks for "not set"
                                    break;
                                case FahrerTagBelegungsTyp.Urlaub:
                                    sap.ANZ_FAHRER = "U";
                                    break;
                                case FahrerTagBelegungsTyp.Krank:
                                    sap.ANZ_FAHRER = "K";
                                    break;
                                case FahrerTagBelegungsTyp.EingeschraenktVerfuegbar:
                                    sap.ANZ_FAHRER = "I";
                                    sap.BEMERKUNG = business.Kommentar;
                                    break;
                                case FahrerTagBelegungsTyp.NichtVerfuegbar:
                                    sap.ANZ_FAHRER = "0";
                                    sap.BEMERKUNG = business.Kommentar;
                                    break;

                                case FahrerTagBelegungsTyp.Verfuegbar:
                                    sap.ANZ_FAHRER = business.FahrerAnzahl.ToString();
                                    break;
                            }
                        }
                    ));
            }
        }

        #endregion
    }
}