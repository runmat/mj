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
	public partial class Z_DPM_LIST_POOLS_001
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_LIST_POOLS_001).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_LIST_POOLS_001).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string LIZNR { get; set; }

			public string ZUNIT_NR_BIS { get; set; }

			public string ZZSIPP { get; set; }

			public string ZZMODELL { get; set; }

			public string ZZBEZEI { get; set; }

			public string ZZHERST_TEXT { get; set; }

			public string ZFARBE { get; set; }

			public string TIDNR { get; set; }

			public string ZZKRAFTSTOFF_TXT { get; set; }

			public string KUNPDI { get; set; }

			public string KUNPDI_TXT { get; set; }

			public DateTime? ERDAT_EQUI { get; set; }

			public DateTime? ZZDAT_EIN { get; set; }

			public DateTime? ZZDAT_BER { get; set; }

			public string ZULBEREIT { get; set; }

			public string ZZAKTSPERRE { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string STATUS_TEXT { get; set; }

			public string NAME1_ZP { get; set; }

			public string CITY1_ZP { get; set; }

			public string FZGART { get; set; }

			public string FIN_ART { get; set; }

			public string BEMERKUNG_INTERN { get; set; }

			public string BEMERKUNG_EXTERN { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					LIZNR = (string)row["LIZNR"],
					ZUNIT_NR_BIS = (string)row["ZUNIT_NR_BIS"],
					ZZSIPP = (string)row["ZZSIPP"],
					ZZMODELL = (string)row["ZZMODELL"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					ZZHERST_TEXT = (string)row["ZZHERST_TEXT"],
					ZFARBE = (string)row["ZFARBE"],
					TIDNR = (string)row["TIDNR"],
					ZZKRAFTSTOFF_TXT = (string)row["ZZKRAFTSTOFF_TXT"],
					KUNPDI = (string)row["KUNPDI"],
					KUNPDI_TXT = (string)row["KUNPDI_TXT"],
					ERDAT_EQUI = (string.IsNullOrEmpty(row["ERDAT_EQUI"].ToString())) ? null : (DateTime?)row["ERDAT_EQUI"],
					ZZDAT_EIN = (string.IsNullOrEmpty(row["ZZDAT_EIN"].ToString())) ? null : (DateTime?)row["ZZDAT_EIN"],
					ZZDAT_BER = (string.IsNullOrEmpty(row["ZZDAT_BER"].ToString())) ? null : (DateTime?)row["ZZDAT_BER"],
					ZULBEREIT = (string)row["ZULBEREIT"],
					ZZAKTSPERRE = (string)row["ZZAKTSPERRE"],
					REPLA_DATE = (string.IsNullOrEmpty(row["REPLA_DATE"].ToString())) ? null : (DateTime?)row["REPLA_DATE"],
					EXPIRY_DATE = (string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString())) ? null : (DateTime?)row["EXPIRY_DATE"],
					ZZTMPDT = (string.IsNullOrEmpty(row["ZZTMPDT"].ToString())) ? null : (DateTime?)row["ZZTMPDT"],
					STATUS_TEXT = (string)row["STATUS_TEXT"],
					NAME1_ZP = (string)row["NAME1_ZP"],
					CITY1_ZP = (string)row["CITY1_ZP"],
					FZGART = (string)row["FZGART"],
					FIN_ART = (string)row["FIN_ART"],
					BEMERKUNG_INTERN = (string)row["BEMERKUNG_INTERN"],
					BEMERKUNG_EXTERN = (string)row["BEMERKUNG_EXTERN"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_LIST_POOLS_001", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_LIST_POOLS_001", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_LIST_POOLS_001.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_LIST_POOLS_001.GT_WEB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
