using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_V_Ueberf_Verfuegbarkeit1
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_V_Ueberf_Verfuegbarkeit1).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_V_Ueberf_Verfuegbarkeit1).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class T_VERFUEG1 : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public DateTime? VERFDAT { get; set; }

			public string ANZ_FAHRER { get; set; }

			public string LIFNR { get; set; }

			public static T_VERFUEG1 Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T_VERFUEG1
				{
					VERFDAT = (string.IsNullOrEmpty(row["VERFDAT"].ToString())) ? null : (DateTime?)row["VERFDAT"],
					ANZ_FAHRER = (string)row["ANZ_FAHRER"],
					LIFNR = (string)row["LIFNR"],

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

			public static IEnumerable<T_VERFUEG1> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T_VERFUEG1> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<T_VERFUEG1> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T_VERFUEG1).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T_VERFUEG1> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<T_VERFUEG1> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T_VERFUEG1> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_VERFUEG1>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_Ueberf_Verfuegbarkeit1", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<T_VERFUEG1> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_VERFUEG1>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<T_VERFUEG1> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_VERFUEG1>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<T_VERFUEG1> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_VERFUEG1>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_Ueberf_Verfuegbarkeit1", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<T_VERFUEG1> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_VERFUEG1>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_V_Ueberf_Verfuegbarkeit1.T_VERFUEG1> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_V_Ueberf_Verfuegbarkeit1.T_VERFUEG1> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
