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
	public partial class Z_DPM_REM_READ_FZG_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_REM_READ_FZG_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_REM_READ_FZG_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_FIN_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public static GT_FIN_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_FIN_IN
				{
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],

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

			public static IEnumerable<GT_FIN_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_FIN_IN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_FIN_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_FIN_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_FIN_IN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_FIN_IN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_FIN_IN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FIN_IN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_READ_FZG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FIN_IN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FIN_IN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FIN_IN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FIN_IN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FIN_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FIN_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_READ_FZG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FIN_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FIN_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR_AG { get; set; }

			public string CHASSIS_NUM { get; set; }

			public DateTime? DAT_IMP { get; set; }

			public string ZEIT_IMP { get; set; }

			public string STORNO { get; set; }

			public string TIDNR { get; set; }

			public string BELNR { get; set; }

			public string BETRAG_RE { get; set; }

			public DateTime? BELDT { get; set; }

			public DateTime? VALDT { get; set; }

			public DateTime? RELDT { get; set; }

			public string DZLART { get; set; }

			public string LICENSE_NUM { get; set; }

			public string KUNNR_ZF { get; set; }

			public string RDEALER { get; set; }

			public string NAME1_ZF { get; set; }

			public string NAME2_ZF { get; set; }

			public string NAME3_ZF { get; set; }

			public string STREET_ZF { get; set; }

			public string POST_CODE1_ZF { get; set; }

			public string CITY1_ZF { get; set; }

			public string LAND_CODE_ZF { get; set; }

			public string LAND_BEZ_ZF { get; set; }

			public string KUNNR_BANK { get; set; }

			public string NAME1_BANK { get; set; }

			public string NAME2_BANK { get; set; }

			public string NAME3_BANK { get; set; }

			public string STREET_BANK { get; set; }

			public string POST_CODE1_BANK { get; set; }

			public string CITY1_BANK { get; set; }

			public string LAND_CODE_BANK { get; set; }

			public string LAND_BEZ_BANK { get; set; }

			public DateTime? B_VERSAUFTR_DAT { get; set; }

			public DateTime? T_VERSAUFTR_DAT { get; set; }

			public string MATNR { get; set; }

			public string VERSANDSPERR { get; set; }

			public string WEB_USER { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					KUNNR_AG = (string)row["KUNNR_AG"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					DAT_IMP = (string.IsNullOrEmpty(row["DAT_IMP"].ToString())) ? null : (DateTime?)row["DAT_IMP"],
					ZEIT_IMP = (string)row["ZEIT_IMP"],
					STORNO = (string)row["STORNO"],
					TIDNR = (string)row["TIDNR"],
					BELNR = (string)row["BELNR"],
					BETRAG_RE = (string)row["BETRAG_RE"],
					BELDT = (string.IsNullOrEmpty(row["BELDT"].ToString())) ? null : (DateTime?)row["BELDT"],
					VALDT = (string.IsNullOrEmpty(row["VALDT"].ToString())) ? null : (DateTime?)row["VALDT"],
					RELDT = (string.IsNullOrEmpty(row["RELDT"].ToString())) ? null : (DateTime?)row["RELDT"],
					DZLART = (string)row["DZLART"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					KUNNR_ZF = (string)row["KUNNR_ZF"],
					RDEALER = (string)row["RDEALER"],
					NAME1_ZF = (string)row["NAME1_ZF"],
					NAME2_ZF = (string)row["NAME2_ZF"],
					NAME3_ZF = (string)row["NAME3_ZF"],
					STREET_ZF = (string)row["STREET_ZF"],
					POST_CODE1_ZF = (string)row["POST_CODE1_ZF"],
					CITY1_ZF = (string)row["CITY1_ZF"],
					LAND_CODE_ZF = (string)row["LAND_CODE_ZF"],
					LAND_BEZ_ZF = (string)row["LAND_BEZ_ZF"],
					KUNNR_BANK = (string)row["KUNNR_BANK"],
					NAME1_BANK = (string)row["NAME1_BANK"],
					NAME2_BANK = (string)row["NAME2_BANK"],
					NAME3_BANK = (string)row["NAME3_BANK"],
					STREET_BANK = (string)row["STREET_BANK"],
					POST_CODE1_BANK = (string)row["POST_CODE1_BANK"],
					CITY1_BANK = (string)row["CITY1_BANK"],
					LAND_CODE_BANK = (string)row["LAND_CODE_BANK"],
					LAND_BEZ_BANK = (string)row["LAND_BEZ_BANK"],
					B_VERSAUFTR_DAT = (string.IsNullOrEmpty(row["B_VERSAUFTR_DAT"].ToString())) ? null : (DateTime?)row["B_VERSAUFTR_DAT"],
					T_VERSAUFTR_DAT = (string.IsNullOrEmpty(row["T_VERSAUFTR_DAT"].ToString())) ? null : (DateTime?)row["T_VERSAUFTR_DAT"],
					MATNR = (string)row["MATNR"],
					VERSANDSPERR = (string)row["VERSANDSPERR"],
					WEB_USER = (string)row["WEB_USER"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_READ_FZG_01", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_READ_FZG_01", inputParameterKeys, inputParameterValues);
				 
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

		public partial class GT_ERR_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string BEM { get; set; }

			public string LICENSE_NUM { get; set; }

			public static GT_ERR_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ERR_OUT
				{
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					BEM = (string)row["BEM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],

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

			public static IEnumerable<GT_ERR_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ERR_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ERR_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ERR_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ERR_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ERR_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ERR_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_READ_FZG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ERR_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ERR_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ERR_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_READ_FZG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ERR_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ERR_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_READ_FZG_01.GT_FIN_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_REM_READ_FZG_01.GT_FIN_IN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_READ_FZG_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_REM_READ_FZG_01.GT_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_READ_FZG_01.GT_ERR_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_REM_READ_FZG_01.GT_ERR_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
