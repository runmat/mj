using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_READ_COC_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_COC_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_COC_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_AKTION(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AKTION", value);
		}

		public static void SetImportParameter_I_AUFTRAG_DAT(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_AUFTRAG_DAT", value);
		}

		public static void SetImportParameter_I_AUFTRAG_DAT_INIT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AUFTRAG_DAT_INIT", value);
		}

		public static void SetImportParameter_I_AUFTR_NR_KD(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AUFTR_NR_KD", value);
		}

		public static void SetImportParameter_I_AUSLIEFER_DATUM(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_AUSLIEFER_DATUM", value);
		}

		public static void SetImportParameter_I_DRU_DATUM_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DRU_DATUM_BIS", value);
		}

		public static void SetImportParameter_I_DRU_DATUM_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DRU_DATUM_VON", value);
		}

		public static void SetImportParameter_I_FIN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FIN", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VORG_NR { get; set; }

			public string VIN { get; set; }

			public string AUFTR_NR_KD { get; set; }

			public DateTime? AUSLIEFER_DATUM { get; set; }

			public string LAND { get; set; }

			public string EQUNR { get; set; }

			public string KUNNR_AG { get; set; }

			public DateTime? COC_ERF_DAT { get; set; }

			public DateTime? AUFTRAG_DAT { get; set; }

			public string COC_KD_ORIG { get; set; }

			public string COC_KD_KOPIE { get; set; }

			public string COC_DRUCK_ORIG { get; set; }

			public string COC_DRUCK_KOPIE { get; set; }

			public DateTime? COC_DRUCK_DATUM { get; set; }

			public string COC_DRUCK_ZEIT { get; set; }

			public string ZBII_DRUCK { get; set; }

			public DateTime? ZBII_DRUCK_DATUM { get; set; }

			public string ZBII_DRUCK_ZEIT { get; set; }

			public DateTime? ZBII_KBA_MELD { get; set; }

			public string VERSAND { get; set; }

			public string ZUL_DEZ { get; set; }

			public string ZUL_AUSLAND { get; set; }

			public string ZUL_EXPORT { get; set; }

			public string COC_0_1 { get; set; }

			public string COC_0_2_TYP { get; set; }

			public string COC_0_2_VAR { get; set; }

			public string COC_0_2_VERS { get; set; }

			public string COC_0_4 { get; set; }

			public string COC_0_5 { get; set; }

			public string COC_0_6_SCHILD { get; set; }

			public string COC_0_6_VIN { get; set; }

			public string COC_0_9 { get; set; }

			public string COC_1_ANZ_ACHS { get; set; }

			public string COC_1_ANZ_RAED { get; set; }

			public string COC_1_1 { get; set; }

			public string COC_2_ANZ_GEL_ACHS { get; set; }

			public string COC_2_LAG_GEL_ACHS { get; set; }

			public string COC_4_MIN { get; set; }

			public string COC_4_MAX { get; set; }

			public string COC_4_1_1 { get; set; }

			public string COC_4_1_2 { get; set; }

			public string COC_4_1_3 { get; set; }

			public string COC_5_MIN { get; set; }

			public string COC_5_MAX { get; set; }

			public string COC_6 { get; set; }

			public string COC_7 { get; set; }

			public string COC_10_MIN { get; set; }

			public string COC_10_MAX { get; set; }

			public string COC_11_MIN { get; set; }

			public string COC_11_MAX { get; set; }

			public string COC_12_MIN { get; set; }

			public string COC_12_MAX { get; set; }

			public string COC_13 { get; set; }

			public string COC_13_1_1 { get; set; }

			public string COC_13_1_2 { get; set; }

			public string COC_13_1_3 { get; set; }

			public string COC_13_1_4 { get; set; }

			public string COC_16_1 { get; set; }

			public string COC_16_2_1 { get; set; }

			public string COC_16_2_2 { get; set; }

			public string COC_16_2_3 { get; set; }

			public string COC_16_2_4 { get; set; }

			public string COC_16_3_1 { get; set; }

			public string COC_16_3_2 { get; set; }

			public string COC_17_1 { get; set; }

			public string COC_17_2_1 { get; set; }

			public string COC_17_2_2 { get; set; }

			public string COC_17_2_3 { get; set; }

			public string COC_17_2_4 { get; set; }

			public string COC_17_3_1 { get; set; }

			public string COC_17_3_2 { get; set; }

			public string COC_19 { get; set; }

			public string COC_29 { get; set; }

			public string COC_31_1 { get; set; }

			public string COC_31_2 { get; set; }

			public string COC_31_3 { get; set; }

			public string COC_31_4 { get; set; }

			public string COC_32_1 { get; set; }

			public string COC_32_2 { get; set; }

			public string COC_32_3 { get; set; }

			public string COC_32_4 { get; set; }

			public string COC_34_JA { get; set; }

			public string COC_34_NEIN { get; set; }

			public string COC_35_ACHSE1_R { get; set; }

			public string COC_35_ACHSE2_R { get; set; }

			public string COC_35_ACHSE3_R { get; set; }

			public string COC_35_ACHSE1_F { get; set; }

			public string COC_35_ACHSE2_F { get; set; }

			public string COC_35_ACHSE3_F { get; set; }

			public string COC_36_MECHANISCH { get; set; }

			public string COC_36_ELEKTRISCH { get; set; }

			public string COC_36_PNEUMATISCH { get; set; }

			public string COC_36_HYDRAULISCH { get; set; }

			public string COC_38 { get; set; }

			public string COC_44 { get; set; }

			public string COC_45_1_D { get; set; }

			public string COC_45_1_V { get; set; }

			public string COC_45_1_S { get; set; }

			public string COC_45_1_U { get; set; }

			public string COC_50_JA { get; set; }

			public string COC_50_EXII { get; set; }

			public string COC_50_EXIII { get; set; }

			public string COC_50_FL { get; set; }

			public string COC_50_OX { get; set; }

			public string COC_50_AT { get; set; }

			public string COC_51 { get; set; }

			public string COC_52_REIFEN_ALT { get; set; }

			public string COC_52_REG_ITALIEN { get; set; }

			public string COC_52_TEXT { get; set; }

			public string COC_EG_TYP_GEN { get; set; }

			public string COC_EG_TYP_GEN_DAT { get; set; }

			public string ZBII_D_1 { get; set; }

			public string ZBII_D_2_TYP { get; set; }

			public string ZBII_D_2_VARIANTE { get; set; }

			public string ZBII_D_2_VERSION { get; set; }

			public string ZBII_2 { get; set; }

			public string ZBII_2_1 { get; set; }

			public string ZBII_2_2_TYP { get; set; }

			public string ZBII_2_2_VVS { get; set; }

			public string ZBII_2_2_PZ { get; set; }

			public string ZBII_3 { get; set; }

			public string ZBII_D_3 { get; set; }

			public string ZBII_J { get; set; }

			public string ZBII_4 { get; set; }

			public string ZBII_5_KLASSE { get; set; }

			public string ZBII_5_AUFBAU { get; set; }

			public string ZBII_R { get; set; }

			public string ZBII_11 { get; set; }

			public string ZBII_P_1 { get; set; }

			public string ZBII_P_2 { get; set; }

			public string ZBII_P_3 { get; set; }

			public string ZBII_P_4 { get; set; }

			public string ZBII_10 { get; set; }

			public string ZBII_K { get; set; }

			public string ZBII_6 { get; set; }

			public string ZBII_17 { get; set; }

			public string ZBII_23 { get; set; }

			public string ZBII_25 { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					VORG_NR = (string)row["VORG_NR"],
					VIN = (string)row["VIN"],
					AUFTR_NR_KD = (string)row["AUFTR_NR_KD"],
					AUSLIEFER_DATUM = string.IsNullOrEmpty(row["AUSLIEFER_DATUM"].ToString()) ? null : (DateTime?)row["AUSLIEFER_DATUM"],
					LAND = (string)row["LAND"],
					EQUNR = (string)row["EQUNR"],
					KUNNR_AG = (string)row["KUNNR_AG"],
					COC_ERF_DAT = string.IsNullOrEmpty(row["COC_ERF_DAT"].ToString()) ? null : (DateTime?)row["COC_ERF_DAT"],
					AUFTRAG_DAT = string.IsNullOrEmpty(row["AUFTRAG_DAT"].ToString()) ? null : (DateTime?)row["AUFTRAG_DAT"],
					COC_KD_ORIG = (string)row["COC_KD_ORIG"],
					COC_KD_KOPIE = (string)row["COC_KD_KOPIE"],
					COC_DRUCK_ORIG = (string)row["COC_DRUCK_ORIG"],
					COC_DRUCK_KOPIE = (string)row["COC_DRUCK_KOPIE"],
					COC_DRUCK_DATUM = string.IsNullOrEmpty(row["COC_DRUCK_DATUM"].ToString()) ? null : (DateTime?)row["COC_DRUCK_DATUM"],
					COC_DRUCK_ZEIT = (string)row["COC_DRUCK_ZEIT"],
					ZBII_DRUCK = (string)row["ZBII_DRUCK"],
					ZBII_DRUCK_DATUM = string.IsNullOrEmpty(row["ZBII_DRUCK_DATUM"].ToString()) ? null : (DateTime?)row["ZBII_DRUCK_DATUM"],
					ZBII_DRUCK_ZEIT = (string)row["ZBII_DRUCK_ZEIT"],
					ZBII_KBA_MELD = string.IsNullOrEmpty(row["ZBII_KBA_MELD"].ToString()) ? null : (DateTime?)row["ZBII_KBA_MELD"],
					VERSAND = (string)row["VERSAND"],
					ZUL_DEZ = (string)row["ZUL_DEZ"],
					ZUL_AUSLAND = (string)row["ZUL_AUSLAND"],
					ZUL_EXPORT = (string)row["ZUL_EXPORT"],
					COC_0_1 = (string)row["COC_0_1"],
					COC_0_2_TYP = (string)row["COC_0_2_TYP"],
					COC_0_2_VAR = (string)row["COC_0_2_VAR"],
					COC_0_2_VERS = (string)row["COC_0_2_VERS"],
					COC_0_4 = (string)row["COC_0_4"],
					COC_0_5 = (string)row["COC_0_5"],
					COC_0_6_SCHILD = (string)row["COC_0_6_SCHILD"],
					COC_0_6_VIN = (string)row["COC_0_6_VIN"],
					COC_0_9 = (string)row["COC_0_9"],
					COC_1_ANZ_ACHS = (string)row["COC_1_ANZ_ACHS"],
					COC_1_ANZ_RAED = (string)row["COC_1_ANZ_RAED"],
					COC_1_1 = (string)row["COC_1_1"],
					COC_2_ANZ_GEL_ACHS = (string)row["COC_2_ANZ_GEL_ACHS"],
					COC_2_LAG_GEL_ACHS = (string)row["COC_2_LAG_GEL_ACHS"],
					COC_4_MIN = (string)row["COC_4_MIN"],
					COC_4_MAX = (string)row["COC_4_MAX"],
					COC_4_1_1 = (string)row["COC_4_1_1"],
					COC_4_1_2 = (string)row["COC_4_1_2"],
					COC_4_1_3 = (string)row["COC_4_1_3"],
					COC_5_MIN = (string)row["COC_5_MIN"],
					COC_5_MAX = (string)row["COC_5_MAX"],
					COC_6 = (string)row["COC_6"],
					COC_7 = (string)row["COC_7"],
					COC_10_MIN = (string)row["COC_10_MIN"],
					COC_10_MAX = (string)row["COC_10_MAX"],
					COC_11_MIN = (string)row["COC_11_MIN"],
					COC_11_MAX = (string)row["COC_11_MAX"],
					COC_12_MIN = (string)row["COC_12_MIN"],
					COC_12_MAX = (string)row["COC_12_MAX"],
					COC_13 = (string)row["COC_13"],
					COC_13_1_1 = (string)row["COC_13_1_1"],
					COC_13_1_2 = (string)row["COC_13_1_2"],
					COC_13_1_3 = (string)row["COC_13_1_3"],
					COC_13_1_4 = (string)row["COC_13_1_4"],
					COC_16_1 = (string)row["COC_16_1"],
					COC_16_2_1 = (string)row["COC_16_2_1"],
					COC_16_2_2 = (string)row["COC_16_2_2"],
					COC_16_2_3 = (string)row["COC_16_2_3"],
					COC_16_2_4 = (string)row["COC_16_2_4"],
					COC_16_3_1 = (string)row["COC_16_3_1"],
					COC_16_3_2 = (string)row["COC_16_3_2"],
					COC_17_1 = (string)row["COC_17_1"],
					COC_17_2_1 = (string)row["COC_17_2_1"],
					COC_17_2_2 = (string)row["COC_17_2_2"],
					COC_17_2_3 = (string)row["COC_17_2_3"],
					COC_17_2_4 = (string)row["COC_17_2_4"],
					COC_17_3_1 = (string)row["COC_17_3_1"],
					COC_17_3_2 = (string)row["COC_17_3_2"],
					COC_19 = (string)row["COC_19"],
					COC_29 = (string)row["COC_29"],
					COC_31_1 = (string)row["COC_31_1"],
					COC_31_2 = (string)row["COC_31_2"],
					COC_31_3 = (string)row["COC_31_3"],
					COC_31_4 = (string)row["COC_31_4"],
					COC_32_1 = (string)row["COC_32_1"],
					COC_32_2 = (string)row["COC_32_2"],
					COC_32_3 = (string)row["COC_32_3"],
					COC_32_4 = (string)row["COC_32_4"],
					COC_34_JA = (string)row["COC_34_JA"],
					COC_34_NEIN = (string)row["COC_34_NEIN"],
					COC_35_ACHSE1_R = (string)row["COC_35_ACHSE1_R"],
					COC_35_ACHSE2_R = (string)row["COC_35_ACHSE2_R"],
					COC_35_ACHSE3_R = (string)row["COC_35_ACHSE3_R"],
					COC_35_ACHSE1_F = (string)row["COC_35_ACHSE1_F"],
					COC_35_ACHSE2_F = (string)row["COC_35_ACHSE2_F"],
					COC_35_ACHSE3_F = (string)row["COC_35_ACHSE3_F"],
					COC_36_MECHANISCH = (string)row["COC_36_MECHANISCH"],
					COC_36_ELEKTRISCH = (string)row["COC_36_ELEKTRISCH"],
					COC_36_PNEUMATISCH = (string)row["COC_36_PNEUMATISCH"],
					COC_36_HYDRAULISCH = (string)row["COC_36_HYDRAULISCH"],
					COC_38 = (string)row["COC_38"],
					COC_44 = (string)row["COC_44"],
					COC_45_1_D = (string)row["COC_45_1_D"],
					COC_45_1_V = (string)row["COC_45_1_V"],
					COC_45_1_S = (string)row["COC_45_1_S"],
					COC_45_1_U = (string)row["COC_45_1_U"],
					COC_50_JA = (string)row["COC_50_JA"],
					COC_50_EXII = (string)row["COC_50_EXII"],
					COC_50_EXIII = (string)row["COC_50_EXIII"],
					COC_50_FL = (string)row["COC_50_FL"],
					COC_50_OX = (string)row["COC_50_OX"],
					COC_50_AT = (string)row["COC_50_AT"],
					COC_51 = (string)row["COC_51"],
					COC_52_REIFEN_ALT = (string)row["COC_52_REIFEN_ALT"],
					COC_52_REG_ITALIEN = (string)row["COC_52_REG_ITALIEN"],
					COC_52_TEXT = (string)row["COC_52_TEXT"],
					COC_EG_TYP_GEN = (string)row["COC_EG_TYP_GEN"],
					COC_EG_TYP_GEN_DAT = (string)row["COC_EG_TYP_GEN_DAT"],
					ZBII_D_1 = (string)row["ZBII_D_1"],
					ZBII_D_2_TYP = (string)row["ZBII_D_2_TYP"],
					ZBII_D_2_VARIANTE = (string)row["ZBII_D_2_VARIANTE"],
					ZBII_D_2_VERSION = (string)row["ZBII_D_2_VERSION"],
					ZBII_2 = (string)row["ZBII_2"],
					ZBII_2_1 = (string)row["ZBII_2_1"],
					ZBII_2_2_TYP = (string)row["ZBII_2_2_TYP"],
					ZBII_2_2_VVS = (string)row["ZBII_2_2_VVS"],
					ZBII_2_2_PZ = (string)row["ZBII_2_2_PZ"],
					ZBII_3 = (string)row["ZBII_3"],
					ZBII_D_3 = (string)row["ZBII_D_3"],
					ZBII_J = (string)row["ZBII_J"],
					ZBII_4 = (string)row["ZBII_4"],
					ZBII_5_KLASSE = (string)row["ZBII_5_KLASSE"],
					ZBII_5_AUFBAU = (string)row["ZBII_5_AUFBAU"],
					ZBII_R = (string)row["ZBII_R"],
					ZBII_11 = (string)row["ZBII_11"],
					ZBII_P_1 = (string)row["ZBII_P_1"],
					ZBII_P_2 = (string)row["ZBII_P_2"],
					ZBII_P_3 = (string)row["ZBII_P_3"],
					ZBII_P_4 = (string)row["ZBII_P_4"],
					ZBII_10 = (string)row["ZBII_10"],
					ZBII_K = (string)row["ZBII_K"],
					ZBII_6 = (string)row["ZBII_6"],
					ZBII_17 = (string)row["ZBII_17"],
					ZBII_23 = (string)row["ZBII_23"],
					ZBII_25 = (string)row["ZBII_25"],

					SAPConnection = sapConnection,
					DynSapProxyFactory = dynSapProxyFactory,
				};
				o.OnInitFromSap();
				return o;
			}

			partial void OnInitFromSap();

			partial void OnInitFromExtern();

			public void OnModelMappingApplied()
			{
				OnInitFromExtern();
			}

			public static IEnumerable<GT_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_COC_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_COC_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_COC_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
