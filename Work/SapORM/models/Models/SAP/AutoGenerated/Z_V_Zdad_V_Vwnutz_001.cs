using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_V_Zdad_V_Vwnutz_001
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_V_Zdad_V_Vwnutz_001).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_V_Zdad_V_Vwnutz_001).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class VWNUTZ_TAB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string LIZNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public DateTime? AULDT { get; set; }

			public DateTime? DATAB { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? ZZABRDT { get; set; }

			public DateTime? ZZMADAT { get; set; }

			public string ZZMAHNS { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public DateTime? ZZFAEDT { get; set; }

			public string DIFF_TAGE { get; set; }

			public static VWNUTZ_TAB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new VWNUTZ_TAB
				{
					LIZNR = (string)row["LIZNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					AULDT = (string.IsNullOrEmpty(row["AULDT"].ToString())) ? null : (DateTime?)row["AULDT"],
					DATAB = (string.IsNullOrEmpty(row["DATAB"].ToString())) ? null : (DateTime?)row["DATAB"],
					REPLA_DATE = (string.IsNullOrEmpty(row["REPLA_DATE"].ToString())) ? null : (DateTime?)row["REPLA_DATE"],
					ZZABRDT = (string.IsNullOrEmpty(row["ZZABRDT"].ToString())) ? null : (DateTime?)row["ZZABRDT"],
					ZZMADAT = (string.IsNullOrEmpty(row["ZZMADAT"].ToString())) ? null : (DateTime?)row["ZZMADAT"],
					ZZMAHNS = (string)row["ZZMAHNS"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					ZZFAEDT = (string.IsNullOrEmpty(row["ZZFAEDT"].ToString())) ? null : (DateTime?)row["ZZFAEDT"],
					DIFF_TAGE = (string)row["DIFF_TAGE"],

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

			public static IEnumerable<VWNUTZ_TAB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<VWNUTZ_TAB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<VWNUTZ_TAB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(VWNUTZ_TAB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<VWNUTZ_TAB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<VWNUTZ_TAB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<VWNUTZ_TAB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<VWNUTZ_TAB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_Zdad_V_Vwnutz_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<VWNUTZ_TAB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<VWNUTZ_TAB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<VWNUTZ_TAB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<VWNUTZ_TAB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<VWNUTZ_TAB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<VWNUTZ_TAB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_Zdad_V_Vwnutz_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<VWNUTZ_TAB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<VWNUTZ_TAB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_V_Zdad_V_Vwnutz_001.VWNUTZ_TAB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_V_Zdad_V_Vwnutz_001.VWNUTZ_TAB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
