// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.CoC.Models
{
    public class AppModelMappings : ModelMappings
    {
        static int _tmpID;


        #region COC Typen

        static public void MapCocTypdatenFromSAP(Z_DPM_COC_TYPDATEN.GT_WEB source, CocEntity destination)
        {
            destination.ID = ++_tmpID;
        }

        static public void MapCocTypdatenToSAP(CocEntity source, Z_DPM_COC_TYPDATEN.GT_WEB destination)
        {
        }
        
        #endregion


        #region COC Erfassungsdaten

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_READ_COC_01.GT_OUT, CocEntity> Z_DPM_READ_COC_01_GT_OUT_To_CocEntity
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_COC_01.GT_OUT, CocEntity>(
                    new Dictionary<string, string>()
                    , (source, destination) =>
                    {
                        destination.ID = ++_tmpID;

                        destination.KUNNR = source.KUNNR_AG;
                        destination.ERDAT = source.COC_ERF_DAT;

                        destination.AUFTR_NR_KD = source.AUFTR_NR_KD;
                        destination.AUFTRAG_DAT = source.AUFTRAG_DAT;
                        destination.AUSLIEFER_DATUM = source.AUSLIEFER_DATUM;

                        destination.COC_0_1 = source.COC_0_1;
                        destination.COC_0_2_TYP = source.COC_0_2_TYP;
                        destination.COC_0_2_VAR = source.COC_0_2_VAR;
                        destination.COC_0_2_VERS = source.COC_0_2_VERS;
                        destination.COC_0_4 = source.COC_0_4;
                        destination.COC_0_5 = source.COC_0_5;
                        destination.COC_0_6_SCHILD = source.COC_0_6_SCHILD;
                        destination.COC_0_6_VIN = source.COC_0_6_VIN;
                        destination.COC_0_9 = source.COC_0_9;

                        destination.COC_1_1 = source.COC_1_1.ToNullableInt();
                        destination.COC_1_ANZ_ACHS = source.COC_1_ANZ_ACHS.ToNullableInt();
                        destination.COC_1_ANZ_RAED = source.COC_1_ANZ_RAED.ToNullableInt();

                        destination.COC_10_MAX = source.COC_10_MAX.ToNullableInt();
                        destination.COC_10_MIN = source.COC_10_MIN.ToNullableInt();

                        destination.COC_11_MAX = source.COC_11_MAX.ToNullableInt();
                        destination.COC_11_MIN = source.COC_11_MIN.ToNullableInt();

                        destination.COC_12_MAX = source.COC_12_MAX.ToNullableInt();
                        destination.COC_12_MIN = source.COC_12_MIN.ToNullableInt();

                        destination.COC_13 = source.COC_13.ToNullableInt();
                        destination.COC_13_1_1 = source.COC_13_1_1.ToNullableInt();
                        destination.COC_13_1_2 = source.COC_13_1_2.ToNullableInt();
                        destination.COC_13_1_3 = source.COC_13_1_3.ToNullableInt();
                        destination.COC_13_1_4 = source.COC_13_1_4.ToNullableInt();

                        destination.COC_16_1 = source.COC_16_1.ToNullableInt();
                        destination.COC_16_2_1 = source.COC_16_2_1.ToNullableInt();
                        destination.COC_16_2_2 = source.COC_16_2_2.ToNullableInt();
                        destination.COC_16_2_3 = source.COC_16_2_3.ToNullableInt();
                        destination.COC_16_2_4 = source.COC_16_2_4.ToNullableInt();
                        destination.COC_16_3_1 = source.COC_16_3_1.ToNullableInt();
                        destination.COC_16_3_2 = source.COC_16_3_2.ToNullableInt();

                        destination.COC_17_1 = source.COC_17_1.ToNullableInt();
                        destination.COC_17_2_1 = source.COC_17_2_1.ToNullableInt();
                        destination.COC_17_2_2 = source.COC_17_2_2.ToNullableInt();
                        destination.COC_17_2_3 = source.COC_17_2_3.ToNullableInt();
                        destination.COC_17_2_4 = source.COC_17_2_4.ToNullableInt();
                        destination.COC_17_3_1 = source.COC_17_3_1.ToNullableInt();
                        destination.COC_17_3_2 = source.COC_17_3_2.ToNullableInt();

                        destination.COC_19 = source.COC_19.ToNullableInt();

                        destination.COC_2_ANZ_GEL_ACHS = source.COC_2_ANZ_GEL_ACHS.ToNullableInt();
                        destination.COC_2_LAG_GEL_ACHS = source.COC_2_LAG_GEL_ACHS;

                        destination.COC_29 = source.COC_29.ToNullableInt();

                        destination.COC_31_1 = source.COC_31_1.XToBool();
                        destination.COC_31_2 = source.COC_31_2.XToBool();
                        destination.COC_31_3 = source.COC_31_3.XToBool();
                        destination.COC_31_4 = source.COC_31_4.XToBool();

                        destination.COC_32_1 = source.COC_32_1.XToBool();
                        destination.COC_32_2 = source.COC_32_2.XToBool();
                        destination.COC_32_3 = source.COC_32_3.XToBool();
                        destination.COC_32_4 = source.COC_32_4.XToBool();

                        destination.COC_34_JA = source.COC_34_JA;

                        destination.COC_35_ACHSE1_F = source.COC_35_ACHSE1_F;
                        destination.COC_35_ACHSE1_R = source.COC_35_ACHSE1_R;
                        destination.COC_35_ACHSE2_F = source.COC_35_ACHSE2_F;
                        destination.COC_35_ACHSE2_R = source.COC_35_ACHSE2_R;
                        destination.COC_35_ACHSE3_F = source.COC_35_ACHSE3_F;
                        destination.COC_35_ACHSE3_R = source.COC_35_ACHSE3_R;

                        destination.COC_36_ELEKTRISCH = source.COC_36_ELEKTRISCH.XToBool();
                        destination.COC_36_HYDRAULISCH = source.COC_36_HYDRAULISCH.XToBool();
                        destination.COC_36_MECHANISCH = source.COC_36_MECHANISCH.XToBool();
                        destination.COC_36_PNEUMATISCH = source.COC_36_PNEUMATISCH.XToBool();

                        destination.COC_38 = source.COC_38;

                        destination.COC_4_1_1 = source.COC_4_1_1.ToNullableInt();
                        destination.COC_4_1_2 = source.COC_4_1_2.ToNullableInt();
                        destination.COC_4_1_3 = source.COC_4_1_3.ToNullableInt();
                        destination.COC_4_MAX = source.COC_4_MAX.ToNullableInt();
                        destination.COC_4_MIN = source.COC_4_MIN.ToNullableInt();

                        destination.COC_44 = source.COC_44;

                        destination.COC_45_1_D = source.COC_45_1_D;
                        destination.COC_45_1_S = source.COC_45_1_S;
                        destination.COC_45_1_U = source.COC_45_1_U;
                        destination.COC_45_1_V = source.COC_45_1_V;

                        destination.COC_5_MAX = source.COC_5_MAX.ToNullableInt();
                        destination.COC_5_MIN = source.COC_5_MIN.ToNullableInt();

                        destination.COC_50_AT = source.COC_50_AT.XToBool();
                        destination.COC_50_EXII = source.COC_50_EXII.XToBool();
                        destination.COC_50_EXIII = source.COC_50_EXIII.XToBool();
                        destination.COC_50_FL = source.COC_50_FL.XToBool();
                        destination.COC_50_JA = source.COC_50_JA;
                        destination.COC_50_OX = source.COC_50_OX.XToBool();

                        destination.COC_51 = source.COC_51;

                        destination.COC_52_REG_ITALIEN = source.COC_52_REG_ITALIEN;
                        destination.COC_52_REIFEN_ALT = source.COC_52_REIFEN_ALT;
                        destination.COC_52_TEXT = source.COC_52_TEXT;

                        destination.COC_6 = source.COC_6.ToNullableInt();

                        destination.COC_7 = source.COC_7.ToNullableInt();

                        destination.COC_DRUCK_DATUM = source.COC_DRUCK_DATUM;
                        destination.COC_DRUCK_KOPIE = source.COC_DRUCK_KOPIE;
                        destination.COC_DRUCK_ORIG = source.COC_DRUCK_ORIG;
                        destination.COC_DRUCK_ZEIT = source.COC_DRUCK_ZEIT;
                        destination.COC_EG_TYP_GEN = source.COC_EG_TYP_GEN;
                        destination.COC_EG_TYP_GEN_DAT = source.COC_EG_TYP_GEN_DAT.ToNullableDateTime("dd.MM.yyyy");
                        destination.COC_KD_KOPIE = source.COC_KD_KOPIE;
                        destination.COC_KD_ORIG = source.COC_KD_ORIG;

                        destination.LAND = source.LAND;
                        destination.VERSAND = source.VERSAND;
                        destination.VIN = source.VIN;
                        destination.VORG_NR = source.VORG_NR;

                        destination.ZBII_2 = source.ZBII_2;
                        destination.ZBII_2_1 = source.ZBII_2_1;
                        destination.ZBII_2_2_PZ = source.ZBII_2_2_PZ;
                        destination.ZBII_2_2_TYP = source.ZBII_2_2_TYP;
                        destination.ZBII_2_2_VVS = source.ZBII_2_2_VVS;
                        destination.ZBII_4 = source.ZBII_4;
                        destination.ZBII_5_AUFBAU = source.ZBII_5_AUFBAU;
                        destination.ZBII_5_KLASSE = source.ZBII_5_KLASSE;
                        destination.ZBII_6 = source.ZBII_6;

                        destination.ZBII_D_1 = source.ZBII_D_1;
                        destination.ZBII_D_2_TYP = source.ZBII_D_2_TYP;
                        destination.ZBII_D_2_VARIANTE = source.ZBII_D_2_VARIANTE;
                        destination.ZBII_D_2_VERSION = source.ZBII_D_2_VERSION;
                        destination.ZBII_D_3 = source.ZBII_D_3;

                        destination.ZBII_DRUCK = source.ZBII_DRUCK;
                        destination.ZBII_DRUCK_DATUM = source.ZBII_DRUCK_DATUM;
                        destination.ZBII_DRUCK_ZEIT = source.ZBII_DRUCK_ZEIT;

                        destination.ZBII_J = source.ZBII_J;
                        destination.ZBII_K = source.ZBII_K;
                        destination.ZBII_KBA_MELD = source.ZBII_KBA_MELD;
                        destination.ZBII_R = source.ZBII_R;

                        destination.ZUL_AUSLAND = source.ZUL_AUSLAND;
                        destination.ZUL_DEZ = source.ZUL_DEZ;
                        destination.ZUL_EXPORT = source.ZUL_EXPORT;

                        // Test
                        //if (source.COC_0_6_SCHILD.NotNullOrEmpty().Contains("-"))
                        //    destination.COC_0_6_SCHILD = source.COC_0_6_SCHILD + "-";
                        //if (source.VIN.NotNullOrEmpty().Contains("-"))
                        //    destination.VIN = source.VIN + "-";
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_UPD_COC_01.GT_DAT, CocEntity> Z_DPM_UPD_COC_01_GT_DAT_From_CocEntity
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UPD_COC_01.GT_DAT, CocEntity>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                    {
                        destination.KUNNR_AG = source.KUNNR;
                        destination.COC_ERF_DAT = source.ERDAT;

                        destination.AUFTR_NR_KD = source.AUFTR_NR_KD;
                        destination.AUFTRAG_DAT = source.AUFTRAG_DAT;
                        destination.AUSLIEFER_DATUM = source.AUSLIEFER_DATUM;

                        destination.COC_0_1 = source.COC_0_1;
                        destination.COC_0_2_TYP = source.COC_0_2_TYP;
                        destination.COC_0_2_VAR = source.COC_0_2_VAR;
                        destination.COC_0_2_VERS = source.COC_0_2_VERS;
                        destination.COC_0_4 = source.COC_0_4;
                        destination.COC_0_5 = source.COC_0_5;
                        destination.COC_0_6_SCHILD = source.COC_0_6_SCHILD;
                        destination.COC_0_6_VIN = source.COC_0_6_VIN;
                        destination.COC_0_9 = source.COC_0_9;

                        destination.COC_1_1 = source.COC_1_1.ToStringNotNull();
                        destination.COC_1_ANZ_ACHS = source.COC_1_ANZ_ACHS.ToStringNotNull();
                        destination.COC_1_ANZ_RAED = source.COC_1_ANZ_RAED.ToStringNotNull();

                        destination.COC_10_MAX = source.COC_10_MAX.ToStringNotNull();
                        destination.COC_10_MIN = source.COC_10_MIN.ToStringNotNull();

                        destination.COC_11_MAX = source.COC_11_MAX.ToStringNotNull();
                        destination.COC_11_MIN = source.COC_11_MIN.ToStringNotNull();

                        destination.COC_12_MAX = source.COC_12_MAX.ToStringNotNull();
                        destination.COC_12_MIN = source.COC_12_MIN.ToStringNotNull();

                        destination.COC_13 = source.COC_13.ToStringNotNull();
                        destination.COC_13_1_1 = source.COC_13_1_1.ToStringNotNull();
                        destination.COC_13_1_2 = source.COC_13_1_2.ToStringNotNull();
                        destination.COC_13_1_3 = source.COC_13_1_3.ToStringNotNull();
                        destination.COC_13_1_4 = source.COC_13_1_4.ToStringNotNull();

                        destination.COC_16_1 = source.COC_16_1.ToStringNotNull();
                        destination.COC_16_2_1 = source.COC_16_2_1.ToStringNotNull();
                        destination.COC_16_2_2 = source.COC_16_2_2.ToStringNotNull();
                        destination.COC_16_2_3 = source.COC_16_2_3.ToStringNotNull();
                        destination.COC_16_2_4 = source.COC_16_2_4.ToStringNotNull();
                        destination.COC_16_3_1 = source.COC_16_3_1.ToStringNotNull();
                        destination.COC_16_3_2 = source.COC_16_3_2.ToStringNotNull();

                        destination.COC_17_1 = source.COC_17_1.ToStringNotNull();
                        destination.COC_17_2_1 = source.COC_17_2_1.ToStringNotNull();
                        destination.COC_17_2_2 = source.COC_17_2_2.ToStringNotNull();
                        destination.COC_17_2_3 = source.COC_17_2_3.ToStringNotNull();
                        destination.COC_17_2_4 = source.COC_17_2_4.ToStringNotNull();
                        destination.COC_17_3_1 = source.COC_17_3_1.ToStringNotNull();
                        destination.COC_17_3_2 = source.COC_17_3_2.ToStringNotNull();

                        destination.COC_19 = source.COC_19.ToStringNotNull();

                        destination.COC_2_ANZ_GEL_ACHS = source.COC_2_ANZ_GEL_ACHS.ToStringNotNull();
                        destination.COC_2_LAG_GEL_ACHS = source.COC_2_LAG_GEL_ACHS;

                        destination.COC_29 = source.COC_29.ToStringNotNull();

                        destination.COC_31_1 = source.COC_31_1.BoolToX();
                        destination.COC_31_2 = source.COC_31_2.BoolToX();
                        destination.COC_31_3 = source.COC_31_3.BoolToX();
                        destination.COC_31_4 = source.COC_31_4.BoolToX();

                        destination.COC_32_1 = source.COC_32_1.BoolToX();
                        destination.COC_32_2 = source.COC_32_2.BoolToX();
                        destination.COC_32_3 = source.COC_32_3.BoolToX();
                        destination.COC_32_4 = source.COC_32_4.BoolToX();

                        destination.COC_34_JA = source.COC_34_JA;

                        destination.COC_35_ACHSE1_F = source.COC_35_ACHSE1_F;
                        destination.COC_35_ACHSE1_R = source.COC_35_ACHSE1_R;
                        destination.COC_35_ACHSE2_F = source.COC_35_ACHSE2_F;
                        destination.COC_35_ACHSE2_R = source.COC_35_ACHSE2_R;
                        destination.COC_35_ACHSE3_F = source.COC_35_ACHSE3_F;
                        destination.COC_35_ACHSE3_R = source.COC_35_ACHSE3_R;

                        destination.COC_36_ELEKTRISCH = source.COC_36_ELEKTRISCH.BoolToX();
                        destination.COC_36_HYDRAULISCH = source.COC_36_HYDRAULISCH.BoolToX();
                        destination.COC_36_MECHANISCH = source.COC_36_MECHANISCH.BoolToX();
                        destination.COC_36_PNEUMATISCH = source.COC_36_PNEUMATISCH.BoolToX();

                        destination.COC_38 = source.COC_38;

                        destination.COC_4_1_1 = source.COC_4_1_1.ToStringNotNull();
                        destination.COC_4_1_2 = source.COC_4_1_2.ToStringNotNull();
                        destination.COC_4_1_3 = source.COC_4_1_3.ToStringNotNull();
                        destination.COC_4_MAX = source.COC_4_MAX.ToStringNotNull();
                        destination.COC_4_MIN = source.COC_4_MIN.ToStringNotNull();

                        destination.COC_44 = source.COC_44;

                        destination.COC_45_1_D = source.COC_45_1_D;
                        destination.COC_45_1_S = source.COC_45_1_S;
                        destination.COC_45_1_U = source.COC_45_1_U;
                        destination.COC_45_1_V = source.COC_45_1_V;

                        destination.COC_5_MAX = source.COC_5_MAX.ToStringNotNull();
                        destination.COC_5_MIN = source.COC_5_MIN.ToStringNotNull();

                        destination.COC_50_AT = source.COC_50_AT.BoolToX();
                        destination.COC_50_EXII = source.COC_50_EXII.BoolToX();
                        destination.COC_50_EXIII = source.COC_50_EXIII.BoolToX();
                        destination.COC_50_FL = source.COC_50_FL.BoolToX();
                        destination.COC_50_JA = source.COC_50_JA;
                        destination.COC_50_OX = source.COC_50_OX.BoolToX();

                        destination.COC_51 = source.COC_51;

                        destination.COC_52_REG_ITALIEN = source.COC_52_REG_ITALIEN;
                        destination.COC_52_REIFEN_ALT = source.COC_52_REIFEN_ALT;
                        destination.COC_52_TEXT = source.COC_52_TEXT;

                        destination.COC_6 = source.COC_6.ToStringNotNull();

                        destination.COC_7 = source.COC_7.ToStringNotNull();

                        destination.COC_DRUCK_DATUM = source.COC_DRUCK_DATUM;
                        destination.COC_DRUCK_KOPIE = source.COC_DRUCK_KOPIE;
                        destination.COC_DRUCK_ORIG = source.COC_DRUCK_ORIG;
                        destination.COC_DRUCK_ZEIT = source.COC_DRUCK_ZEIT;
                        destination.COC_EG_TYP_GEN = source.COC_EG_TYP_GEN;
                        destination.COC_EG_TYP_GEN_DAT = (source.COC_EG_TYP_GEN_DAT.HasValue ? source.COC_EG_TYP_GEN_DAT.Value.ToShortDateString() : "");
                        destination.COC_KD_KOPIE = source.COC_KD_KOPIE;
                        destination.COC_KD_ORIG = source.COC_KD_ORIG;

                        destination.LAND = source.LAND;
                        destination.VERSAND = source.VERSAND;
                        destination.VIN = source.VIN;
                        destination.VORG_NR = source.VORG_NR;

                        destination.ZBII_2 = source.ZBII_2;
                        destination.ZBII_2_1 = source.ZBII_2_1;
                        destination.ZBII_2_2_PZ = source.ZBII_2_2_PZ;
                        destination.ZBII_2_2_TYP = source.ZBII_2_2_TYP;
                        destination.ZBII_2_2_VVS = source.ZBII_2_2_VVS;
                        destination.ZBII_4 = source.ZBII_4;
                        destination.ZBII_5_AUFBAU = source.ZBII_5_AUFBAU;
                        destination.ZBII_5_KLASSE = source.ZBII_5_KLASSE;
                        destination.ZBII_6 = source.ZBII_6;

                        destination.ZBII_D_1 = source.ZBII_D_1;
                        destination.ZBII_D_2_TYP = source.ZBII_D_2_TYP;
                        destination.ZBII_D_2_VARIANTE = source.ZBII_D_2_VARIANTE;
                        destination.ZBII_D_2_VERSION = source.ZBII_D_2_VERSION;
                        destination.ZBII_D_3 = source.ZBII_D_3;

                        destination.ZBII_DRUCK = source.ZBII_DRUCK;
                        destination.ZBII_DRUCK_DATUM = source.ZBII_DRUCK_DATUM;
                        destination.ZBII_DRUCK_ZEIT = source.ZBII_DRUCK_ZEIT;

                        destination.ZBII_J = source.ZBII_J;
                        destination.ZBII_K = source.ZBII_K;
                        destination.ZBII_KBA_MELD = source.ZBII_KBA_MELD;
                        destination.ZBII_R = source.ZBII_R;

                        destination.ZUL_AUSLAND = source.ZUL_AUSLAND;
                        destination.ZUL_DEZ = source.ZUL_DEZ;
                        destination.ZUL_EXPORT = source.ZUL_EXPORT;
                    }));
            }
        }

        #endregion


        #region CSV Upload
        
        static public void MapCsvUploadEntityDpmCocToSAP(CsvUploadEntityDpmCoc source, Z_DPM_UPD_COC_01.GT_DAT destination)
        {
            destination.KUNNR_AG = source.CustomerNo;

            destination.AUFTR_NR_KD = source.OrderID;

            destination.VIN = source.VIN;

            destination.AUSLIEFER_DATUM = source.DeliveryDate;

            destination.LAND = source.Country.ToLower();
            if (destination.LAND.NotNullOrEmpty() == "de")
                destination.ZBII_DRUCK = "x";

            if (source.Color.IsNumeric())
                destination.ZBII_11 = source.Color;
            else
                destination.ZBII_R = source.Color;

            destination.ZBII_2_1 = source.CodeManufacturer;

            destination.ZBII_2_2_TYP = source.CodeTypeVersion.SubstringTry(0, 3);
            destination.ZBII_2_2_VVS = source.CodeTypeVersion.SubstringTry(3, 5);
            destination.ZBII_2_2_PZ = source.CodeTypeVersion.SubstringTry(8, 1);

            destination.COC_ERF_DAT = DateTime.Today;
        }

        #endregion


        #region Zulassung

        #region Save to Repository

        #endregion

        #endregion


        #region Sendungsaufträge

        static public ModelMapping<Z_DPM_GET_ZZSEND2.GT_WEB, SendungsAuftrag> Z_DPM_GET_ZZSEND2_GT_WEB_To_SendungsAuftrag
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_GET_ZZSEND2.GT_WEB, SendungsAuftrag>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.AnlageDatum = s.ERDAT;
                        d.FIN = s.ZZFAHRG;
                        d.Kennzeichen = s.ZZKENN;
                        d.RechnungsNr = s.VBELN;
                        d.VersandDatum = s.VDATU;
                        d.ZulassungsDatum = s.VDATU;
                        d.VersandID = s.ZZSEND2;
                        d.VertragsNr = s.ZZREFNR;
                        d.VersandKey = "1"; // 1 = DHL
                    }));
            }
        }

        static public ModelMapping<Z_DPM_READ_SENDTAB_03.GT_OUT, SendungsAuftrag> Z_DPM_READ_SENDTAB_03_GT_OUT_To_SendungsAuftrag
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_SENDTAB_03.GT_OUT, SendungsAuftrag>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.FIN = s.ZZFAHRG;
                        d.Fahrzeugbrief = s.ZZBRIEF;
                        d.Kennzeichen = s.ZZKENN;
                        d.RechnungsNr = s.VBELN;
                        d.VersandDatum = s.ZZLSDAT;
                        d.VersandID = s.ZZTRACK;
                        d.VertragsNr = s.ZZREFNR;
                        d.Referenz = s.POOLNR;
                        d.StatusText = s.STATUS_CODE.NotNullOrEmpty();
                        d.VersandWeg = s.VERSANDWEG;
                        d.VersandKey = s.VERSANDVALUE;
                        d.PoolNummer = s.POOLNR;
                        d.Referenz1 = s.ZZREFERENZ1;
                        d.Referenz2 = s.ZZREFERENZ2;
                        d.Materialnummer = s.IDNRK;
                        d.Bezeichnung = s.MAKTX;

                        d.VersandAdresseAsText = string.Format("{0} {1}<br />{2} {3}, {4} {5}", s.NAME1, s.NAME2, s.STRAS, s.HSNM1, s.PSTLZ, s.CITY1);
                    }));
            }
        }

        #endregion
    }
}