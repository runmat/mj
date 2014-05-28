using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_V_UEBERF_AUFTR_FAHRER
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_V_UEBERF_AUFTR_FAHRER).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_V_UEBERF_AUFTR_FAHRER).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class T_AUFTRAEGE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string AUFNR { get; set; }

			public string FAHRTNR { get; set; }

			public DateTime? WADAT { get; set; }

			public string EQUNR { get; set; }

			public string ZZKENN { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZREFNR { get; set; }

			public string ZZBEZEI { get; set; }

			public string FAHRTVON { get; set; }

			public string FAHRTNACH { get; set; }

			public string KVGR2 { get; set; }

			public static T_AUFTRAEGE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T_AUFTRAEGE
				{
					AUFNR = (string)row["AUFNR"],
					FAHRTNR = (string)row["FAHRTNR"],
					WADAT = (string.IsNullOrEmpty(row["WADAT"].ToString())) ? null : (DateTime?)row["WADAT"],
					EQUNR = (string)row["EQUNR"],
					ZZKENN = (string)row["ZZKENN"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZREFNR = (string)row["ZZREFNR"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					FAHRTVON = (string)row["FAHRTVON"],
					FAHRTNACH = (string)row["FAHRTNACH"],
					KVGR2 = (string)row["KVGR2"],

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

			public static IEnumerable<T_AUFTRAEGE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T_AUFTRAEGE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<T_AUFTRAEGE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T_AUFTRAEGE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T_AUFTRAEGE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<T_AUFTRAEGE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T_AUFTRAEGE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_AUFTRAEGE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_UEBERF_AUFTR_FAHRER", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<T_AUFTRAEGE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_AUFTRAEGE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<T_AUFTRAEGE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_AUFTRAEGE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<T_AUFTRAEGE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_AUFTRAEGE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_UEBERF_AUFTR_FAHRER", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<T_AUFTRAEGE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_AUFTRAEGE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_V_UEBERF_AUFTR_FAHRER.T_AUFTRAEGE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_V_UEBERF_AUFTR_FAHRER.T_AUFTRAEGE> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
