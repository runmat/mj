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
	public partial class Z_V_KCL_GRUPPENDATEN
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_V_KCL_GRUPPENDATEN).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_V_KCL_GRUPPENDATEN).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_GRUPPE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("GRUPPE", value);
		}

		public static void SetImportParameter_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("KUNNR", value);
		}

		public partial class ZZGRUPPENDATEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZFILIALE { get; set; }

			public string NAME1 { get; set; }

			public string STRAS { get; set; }

			public string ORT01 { get; set; }

			public static ZZGRUPPENDATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ZZGRUPPENDATEN
				{
					ZFILIALE = (string)row["ZFILIALE"],
					NAME1 = (string)row["NAME1"],
					STRAS = (string)row["STRAS"],
					ORT01 = (string)row["ORT01"],

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

			public static IEnumerable<ZZGRUPPENDATEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ZZGRUPPENDATEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ZZGRUPPENDATEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ZZGRUPPENDATEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ZZGRUPPENDATEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ZZGRUPPENDATEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ZZGRUPPENDATEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ZZGRUPPENDATEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_KCL_GRUPPENDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ZZGRUPPENDATEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ZZGRUPPENDATEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ZZGRUPPENDATEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ZZGRUPPENDATEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ZZGRUPPENDATEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ZZGRUPPENDATEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_KCL_GRUPPENDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ZZGRUPPENDATEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ZZGRUPPENDATEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_V_KCL_GRUPPENDATEN.ZZGRUPPENDATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
