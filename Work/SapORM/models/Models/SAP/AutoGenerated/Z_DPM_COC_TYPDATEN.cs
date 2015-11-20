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
	public partial class Z_DPM_COC_TYPDATEN
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_COC_TYPDATEN).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_COC_TYPDATEN).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_MAXROWS(ISapDataService sap, int? value)
		{
			sap.SetImportParameter("I_MAXROWS", value);
		}

		public void SetImportParameter_I_TYP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TYP", value);
		}

		public void SetImportParameter_I_VARIANTE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VARIANTE", value);
		}

		public void SetImportParameter_I_VERKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VERKZ", value);
		}

		public void SetImportParameter_I_VERSION(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VERSION", value);
		}

		public void SetImportParameter_I_VORLAGE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORLAGE", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string VORLAGE { get; set; }

			public DateTime? ERDAT { get; set; }

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

			public string ZBII_2 { get; set; }

			public string ZBII_2_1 { get; set; }

			public string ZBII_2_2_TYP { get; set; }

			public string ZBII_2_2_VVS { get; set; }

			public string ZBII_2_2_PZ { get; set; }

			public string ZBII_D_3 { get; set; }

			public string ZBII_4 { get; set; }

			public string ZBII_5_KLASSE { get; set; }

			public string ZBII_5_AUFBAU { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					KUNNR = (string)row["KUNNR"],
					VORLAGE = (string)row["VORLAGE"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
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
					ZBII_2 = (string)row["ZBII_2"],
					ZBII_2_1 = (string)row["ZBII_2_1"],
					ZBII_2_2_TYP = (string)row["ZBII_2_2_TYP"],
					ZBII_2_2_VVS = (string)row["ZBII_2_2_VVS"],
					ZBII_2_2_PZ = (string)row["ZBII_2_2_PZ"],
					ZBII_D_3 = (string)row["ZBII_D_3"],
					ZBII_4 = (string)row["ZBII_4"],
					ZBII_5_KLASSE = (string)row["ZBII_5_KLASSE"],
					ZBII_5_AUFBAU = (string)row["ZBII_5_AUFBAU"],

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

			public static IEnumerable<GT_WEB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_COC_TYPDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_COC_TYPDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_COC_TYPDATEN.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
