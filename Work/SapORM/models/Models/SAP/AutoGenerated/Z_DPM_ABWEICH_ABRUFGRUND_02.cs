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
	public partial class Z_DPM_ABWEICH_ABRUFGRUND_02
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_ABWEICH_ABRUFGRUND_02).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_ABWEICH_ABRUFGRUND_02).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_ABWEICHUNG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ABWEICHUNG", value);
		}

		public void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string EQUNR { get; set; }

			public DateTime? DATAUS { get; set; }

			public string ZZVGRUND { get; set; }

			public string ZZVGRUND_TXT { get; set; }

			public DateTime? DATEIN { get; set; }

			public string ZZVGRUND_1 { get; set; }

			public string ZZVGRUND_1_TXT { get; set; }

			public string LICENSE_NUM { get; set; }

			public string LICENSE_NUM_NEU { get; set; }

			public string TIDNR { get; set; }

			public string TIDNR_NEU { get; set; }

			public string ZH_KUNNR { get; set; }

			public string ZH_KUNNR_NEU { get; set; }

			public string MEMO { get; set; }

			public string ZH_NEU_NAME1 { get; set; }

			public string ZH_NEU_NAME2 { get; set; }

			public string ZH_NEU_NAME3 { get; set; }

			public string ZH_NEU_CITY1 { get; set; }

			public string ZH_NEU_POST_CODE1 { get; set; }

			public string ZH_NEU_STREET { get; set; }

			public string ZH_NEU_HOUSE_NUM1 { get; set; }

			public string ZH_ALT_NAME1 { get; set; }

			public string ZH_ALT_NAME2 { get; set; }

			public string ZH_ALT_NAME3 { get; set; }

			public string ZH_ALT_CITY1 { get; set; }

			public string ZH_ALT_POST_CODE1 { get; set; }

			public string ZH_ALT_STREET { get; set; }

			public string ZH_ALT_HOUSE_NUM1 { get; set; }

			public string ZF_KUNNR { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string LIZNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string ZZCOCKZ { get; set; }

			public string ZZCOCKZ_NEU { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public string ERNAM { get; set; }

			public string ZB1 { get; set; }

			public string ZB1_NEU { get; set; }

			public string KNRZE { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					KUNNR = (string)row["KUNNR"],
					EQUNR = (string)row["EQUNR"],
					DATAUS = string.IsNullOrEmpty(row["DATAUS"].ToString()) ? null : (DateTime?)row["DATAUS"],
					ZZVGRUND = (string)row["ZZVGRUND"],
					ZZVGRUND_TXT = (string)row["ZZVGRUND_TXT"],
					DATEIN = string.IsNullOrEmpty(row["DATEIN"].ToString()) ? null : (DateTime?)row["DATEIN"],
					ZZVGRUND_1 = (string)row["ZZVGRUND_1"],
					ZZVGRUND_1_TXT = (string)row["ZZVGRUND_1_TXT"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					LICENSE_NUM_NEU = (string)row["LICENSE_NUM_NEU"],
					TIDNR = (string)row["TIDNR"],
					TIDNR_NEU = (string)row["TIDNR_NEU"],
					ZH_KUNNR = (string)row["ZH_KUNNR"],
					ZH_KUNNR_NEU = (string)row["ZH_KUNNR_NEU"],
					MEMO = (string)row["MEMO"],
					ZH_NEU_NAME1 = (string)row["ZH_NEU_NAME1"],
					ZH_NEU_NAME2 = (string)row["ZH_NEU_NAME2"],
					ZH_NEU_NAME3 = (string)row["ZH_NEU_NAME3"],
					ZH_NEU_CITY1 = (string)row["ZH_NEU_CITY1"],
					ZH_NEU_POST_CODE1 = (string)row["ZH_NEU_POST_CODE1"],
					ZH_NEU_STREET = (string)row["ZH_NEU_STREET"],
					ZH_NEU_HOUSE_NUM1 = (string)row["ZH_NEU_HOUSE_NUM1"],
					ZH_ALT_NAME1 = (string)row["ZH_ALT_NAME1"],
					ZH_ALT_NAME2 = (string)row["ZH_ALT_NAME2"],
					ZH_ALT_NAME3 = (string)row["ZH_ALT_NAME3"],
					ZH_ALT_CITY1 = (string)row["ZH_ALT_CITY1"],
					ZH_ALT_POST_CODE1 = (string)row["ZH_ALT_POST_CODE1"],
					ZH_ALT_STREET = (string)row["ZH_ALT_STREET"],
					ZH_ALT_HOUSE_NUM1 = (string)row["ZH_ALT_HOUSE_NUM1"],
					ZF_KUNNR = (string)row["ZF_KUNNR"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					LIZNR = (string)row["LIZNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					ZZCOCKZ = (string)row["ZZCOCKZ"],
					ZZCOCKZ_NEU = (string)row["ZZCOCKZ_NEU"],
					REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
					ERNAM = (string)row["ERNAM"],
					ZB1 = (string)row["ZB1"],
					ZB1_NEU = (string)row["ZB1_NEU"],
					KNRZE = (string)row["KNRZE"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_ABWEICH_ABRUFGRUND_02", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_ABWEICH_ABRUFGRUND_02", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_ABWEICH_ABRUFGRUND_02.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
