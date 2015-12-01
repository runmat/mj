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
	public partial class Z_DPM_FILL_VERSAUFTR
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_FILL_VERSAUFTR).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_FILL_VERSAUFTR).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_FNAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("FNAME", value);
		}

		public static void SetImportParameter_FNUM_CHECK(ISapDataService sap, string value)
		{
			sap.SetImportParameter("FNUM_CHECK", value);
		}

		public static void SetImportParameter_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("KUNNR_AG", value);
		}

		public static void SetImportParameter_Z1_UEB(ISapDataService sap, string value)
		{
			sap.SetImportParameter("Z1_UEB", value);
		}

		public static string GetExportParameter_E_FNAME(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_FNAME").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_ERR : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR_AG { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string BEMERKUNG { get; set; }

			public static GT_ERR Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ERR
				{
					KUNNR_AG = (string)row["KUNNR_AG"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					BEMERKUNG = (string)row["BEMERKUNG"],

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

			public static IEnumerable<GT_ERR> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ERR> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ERR> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ERR).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ERR> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ERR> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ERR>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FILL_VERSAUFTR", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ERR>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ERR>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ERR>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FILL_VERSAUFTR", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ERR>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string ZZKUNNR_AG { get; set; }

			public string LICENSE_NUM { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string ZZBRFVERS { get; set; }

			public string ZZSCHLVERS { get; set; }

			public string IDNRK { get; set; }

			public string ZANF_NR { get; set; }

			public string ZZABMELD { get; set; }

			public string ABCKZ { get; set; }

			public string ZZPLFOR { get; set; }

			public string MATNR { get; set; }

			public string ZZVGRUND { get; set; }

			public DateTime? ZZANFDT { get; set; }

			public string ZZKUNNR_ZS { get; set; }

			public string ZZADRNR_ZS { get; set; }

			public string ZZKONZS_ZS { get; set; }

			public string ZZNAME1_ZS { get; set; }

			public string ZZNAME2_ZS { get; set; }

			public string ZZSTRAS_ZS { get; set; }

			public string ZZHAUSNR_ZS { get; set; }

			public string ZZPSTLZ_ZS { get; set; }

			public string ZZORT01_ZS { get; set; }

			public string ZZLAND_ZS { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERZET { get; set; }

			public string ERNAM { get; set; }

			public string ZZFCODE { get; set; }

			public string ZKUNNR_TG { get; set; }

			public string ZSUPPLIER_ID { get; set; }

			public string ZZHERST { get; set; }

			public string ZZMODELL { get; set; }

			public string ZZVSFREIGABE { get; set; }

			public DateTime? VERKAUFSDATUM { get; set; }

			public string LIZNR { get; set; }

			public string ZZ_MAHNA { get; set; }

			public string ZZBETREFF { get; set; }

			public string ZZNAME_ZH { get; set; }

			public string VERSANDWEG { get; set; }

			public string ZUSANSCHREIBEN1 { get; set; }

			public string ZUSANSCHREIBEN2 { get; set; }

			public string ZUSANSCHREIBEN3 { get; set; }

			public string NUR_MIT_ZB2 { get; set; }

			public DateTime? VERSDAT_MIN { get; set; }

			public string VERS_NACH_ZUL { get; set; }

			public string TREUGEBER_VERS { get; set; }

			public string KREDITBETRAG { get; set; }

			public string INKASSO { get; set; }

			public string HAENDLER_NR { get; set; }

			public string USER_AUTOR { get; set; }

			public DateTime? DATUM_AUTOR { get; set; }

			public string UZEIT_AUTOR { get; set; }

			public string ZZNAME3_ZS { get; set; }

			public string ZZNAME4_ZS { get; set; }

			public string KNZ_VORN_VORH { get; set; }

			public string KNZ_HINT_VORH { get; set; }

			public string ZB1_VORH { get; set; }

			public string COUNTRY_ZS { get; set; }

			public string MENGE { get; set; }

			public static GT_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IN
				{
					MANDT = (string)row["MANDT"],
					ZZKUNNR_AG = (string)row["ZZKUNNR_AG"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					ZZBRFVERS = (string)row["ZZBRFVERS"],
					ZZSCHLVERS = (string)row["ZZSCHLVERS"],
					IDNRK = (string)row["IDNRK"],
					ZANF_NR = (string)row["ZANF_NR"],
					ZZABMELD = (string)row["ZZABMELD"],
					ABCKZ = (string)row["ABCKZ"],
					ZZPLFOR = (string)row["ZZPLFOR"],
					MATNR = (string)row["MATNR"],
					ZZVGRUND = (string)row["ZZVGRUND"],
					ZZANFDT = string.IsNullOrEmpty(row["ZZANFDT"].ToString()) ? null : (DateTime?)row["ZZANFDT"],
					ZZKUNNR_ZS = (string)row["ZZKUNNR_ZS"],
					ZZADRNR_ZS = (string)row["ZZADRNR_ZS"],
					ZZKONZS_ZS = (string)row["ZZKONZS_ZS"],
					ZZNAME1_ZS = (string)row["ZZNAME1_ZS"],
					ZZNAME2_ZS = (string)row["ZZNAME2_ZS"],
					ZZSTRAS_ZS = (string)row["ZZSTRAS_ZS"],
					ZZHAUSNR_ZS = (string)row["ZZHAUSNR_ZS"],
					ZZPSTLZ_ZS = (string)row["ZZPSTLZ_ZS"],
					ZZORT01_ZS = (string)row["ZZORT01_ZS"],
					ZZLAND_ZS = (string)row["ZZLAND_ZS"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ERZET = (string)row["ERZET"],
					ERNAM = (string)row["ERNAM"],
					ZZFCODE = (string)row["ZZFCODE"],
					ZKUNNR_TG = (string)row["ZKUNNR_TG"],
					ZSUPPLIER_ID = (string)row["ZSUPPLIER_ID"],
					ZZHERST = (string)row["ZZHERST"],
					ZZMODELL = (string)row["ZZMODELL"],
					ZZVSFREIGABE = (string)row["ZZVSFREIGABE"],
					VERKAUFSDATUM = string.IsNullOrEmpty(row["VERKAUFSDATUM"].ToString()) ? null : (DateTime?)row["VERKAUFSDATUM"],
					LIZNR = (string)row["LIZNR"],
					ZZ_MAHNA = (string)row["ZZ_MAHNA"],
					ZZBETREFF = (string)row["ZZBETREFF"],
					ZZNAME_ZH = (string)row["ZZNAME_ZH"],
					VERSANDWEG = (string)row["VERSANDWEG"],
					ZUSANSCHREIBEN1 = (string)row["ZUSANSCHREIBEN1"],
					ZUSANSCHREIBEN2 = (string)row["ZUSANSCHREIBEN2"],
					ZUSANSCHREIBEN3 = (string)row["ZUSANSCHREIBEN3"],
					NUR_MIT_ZB2 = (string)row["NUR_MIT_ZB2"],
					VERSDAT_MIN = string.IsNullOrEmpty(row["VERSDAT_MIN"].ToString()) ? null : (DateTime?)row["VERSDAT_MIN"],
					VERS_NACH_ZUL = (string)row["VERS_NACH_ZUL"],
					TREUGEBER_VERS = (string)row["TREUGEBER_VERS"],
					KREDITBETRAG = (string)row["KREDITBETRAG"],
					INKASSO = (string)row["INKASSO"],
					HAENDLER_NR = (string)row["HAENDLER_NR"],
					USER_AUTOR = (string)row["USER_AUTOR"],
					DATUM_AUTOR = string.IsNullOrEmpty(row["DATUM_AUTOR"].ToString()) ? null : (DateTime?)row["DATUM_AUTOR"],
					UZEIT_AUTOR = (string)row["UZEIT_AUTOR"],
					ZZNAME3_ZS = (string)row["ZZNAME3_ZS"],
					ZZNAME4_ZS = (string)row["ZZNAME4_ZS"],
					KNZ_VORN_VORH = (string)row["KNZ_VORN_VORH"],
					KNZ_HINT_VORH = (string)row["KNZ_HINT_VORH"],
					ZB1_VORH = (string)row["ZB1_VORH"],
					COUNTRY_ZS = (string)row["COUNTRY_ZS"],
					MENGE = (string)row["MENGE"],

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

			public static IEnumerable<GT_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FILL_VERSAUFTR", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FILL_VERSAUFTR", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_FILL_VERSAUFTR.GT_ERR> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_FILL_VERSAUFTR.GT_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
